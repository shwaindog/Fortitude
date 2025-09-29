#region

using FortitudeBusRules.BusMessaging.Pipelines.Timers;
using FortitudeBusRules.Messages;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.LoggerViews;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

// ReSharper disable MemberCanBeProtected.Global

#endregion

namespace FortitudeBusRules.Rules;

public enum RuleLifeCycle
{
    NotStarted = 0
  , Starting
  , Started
  , ShutDownRequested
  , ShuttingDown
  , Stopped
}

public delegate void LifeCycleChangeHandler(IRule sender, RuleLifeCycle oldState, RuleLifeCycle newState);

public interface IRule : IStringBearer
{
    IRule ParentRule { get; }
    IQueueContext Context { get; set; }
    string FriendlyName { get; }
    string? Id { get; set; }

    IRuleTimer Timer { get; }
    RuleLifeCycle LifeCycleState { get; set; }
    IEnumerable<IRule> ChildRules { get; }
    RuleFilter AppliesToThisRule { get; }
    RuleFilter NotAppliesToThisRule { get; }
    IEnumerable<IAsyncValueTaskDisposable> OnStopResourceCleanup();
    T AddOnStopResourceCleanup<T>(T resourceCleanup) where T : IAsyncValueTaskDisposable;

    event LifeCycleChangeHandler LifeCycleChanged;
    ValueTask StartAsync();
    ValueTask StopAsync();
    void Start();
    void Stop();
    void AddChild(IRule child);
}

public interface IListeningRule : IRule
{
    new IRule ParentRule { get; set; }
    int IncrementLifeTimeCount();
    int DecrementLifeTimeCount();
    bool ShouldBeStopped();

    ValueTask MessageBusStopAsync();
}

public class InvalidRuleTransitionStateException
(
    string name
  , RuleLifeCycle current
  , RuleLifeCycle proposed
  , List<RuleLifeCycle> allowed)
    : Exception($"Invalid rule: '{name}' state transition from {current} to {proposed} allowed are [{allowed}]");

public class Rule : IListeningRule
{
    private static readonly IVersatileFLogger Logger = FLog.FLoggerForType.As<IVersatileFLogger>();

    public static readonly Rule NoKnownSender = new()
    {
        Id = "UnknownSender"
    };

    private readonly ReusableList<IRule> children = [];

    private static int instanceCount;

    private readonly int instanceNumber;

    private int aliveRefCount;

    private RuleFilter?   appliesToThisRuleFilter;
    private IQueueContext context = null!;
    private RuleFilter?   notAppliesToThisRuleFilter;

    private readonly ReusableList<IAsyncValueTaskDisposable> onStopResourceCleanup = new();

    private IRule? parentRule;

    private RuleLifeCycle ruleLifeCycle;
    private IRuleTimer?   ruleTimer;

    public Rule()
    {
        instanceNumber = Interlocked.Increment(ref instanceCount);

        LifeCycleState = RuleLifeCycle.NotStarted;
        parentRule     = null!;
        FriendlyName   = GetType().Name + TypeInstanceCounter.Instance.GetNextInstanceNumber(GetType());
    }

    public Rule(string friendlyName, string id) : this()
    {
        FriendlyName = friendlyName;

        Id = id;
    }

    public Rule(string id) : this()
    {
        FriendlyName = id;

        Id = id;
    }

    public IRule ParentRule
    {
        get => parentRule!;
        set
        {
            if (parentRule == value) return;
            parentRule = value;
            if (parentRule != null) parentRule.LifeCycleChanged += ParentRule_LifeCycleChanged;
        }
    }

    IRule IListeningRule.ParentRule
    {
        get => ParentRule;
        set => ParentRule = value;
    }

    public event LifeCycleChangeHandler? LifeCycleChanged;

    public IRuleTimer Timer
    {
        get
        {
            if (ruleTimer == null)
            {
                ruleTimer = Context.QueueTimer.CreateRuleTimer(this);
                onStopResourceCleanup.Add(ruleTimer);
            }
            return ruleTimer;
        }
    }

    public IEnumerable<IAsyncValueTaskDisposable> OnStopResourceCleanup() => onStopResourceCleanup;

    public T AddOnStopResourceCleanup<T>(T resourceCleanup) where T : IAsyncValueTaskDisposable
    {
        onStopResourceCleanup.AddReturn(resourceCleanup);
        return resourceCleanup;
    }

    public void AddChild(IRule child)
    {
        if (child == this) return;
        children.Add(child);
        if (child.LifeCycleState is RuleLifeCycle.NotStarted or RuleLifeCycle.Starting or RuleLifeCycle.Started)
        {
            IncrementLifeTimeCount();
            if (child is IListeningRule listeningChild) listeningChild.ParentRule = this;
        }

        child.LifeCycleChanged += Child_LifeCycleChanged;
    }

    public IEnumerable<IRule> ChildRules => children;
    public IQueueContext Context
    {
        get => context;
        set
        {
            context = value;

            onStopResourceCleanup.Recycler = context.PooledRecycler;
            children.Recycler              = context.PooledRecycler;
        }
    }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public string FriendlyName { get; set; }
    public string? Id { get; set; }

    public RuleLifeCycle LifeCycleState
    {
        get => ruleLifeCycle;
        set
        {
            if (ruleLifeCycle != value)
            {
                var oldState = ValidateRuleLifeCycleTransition(value);
                ruleLifeCycle = value;
                var changedEvent = LifeCycleChanged;
                changedEvent?.Invoke(this, oldState, ruleLifeCycle);
            }

            ruleLifeCycle = value;
        }
    }

    public RuleFilter AppliesToThisRule => appliesToThisRuleFilter ??= check => ReferenceEquals(check, this);

    public RuleFilter NotAppliesToThisRule => notAppliesToThisRuleFilter ??= check => !ReferenceEquals(check, this);

    public int IncrementLifeTimeCount() => Interlocked.Increment(ref aliveRefCount);

    public int DecrementLifeTimeCount() => Interlocked.Decrement(ref aliveRefCount);

    public bool ShouldBeStopped() => LifeCycleState == RuleLifeCycle.Started && aliveRefCount <= 0;

    public virtual ValueTask StartAsync() => new(StartTaskAsync());

    public virtual ValueTask StopAsync() => new(StopTaskAsync());

    public virtual void Start() { }

    public virtual void Stop() { }

    public async ValueTask MessageBusStopAsync()
    {
        if (LifeCycleState != RuleLifeCycle.ShutDownRequested) return;
        LifeCycleState = RuleLifeCycle.ShuttingDown;
        await StopAsync();
    }

    private void RemoveChild(IRule child)
    {
        if (child == this) return;
        children.Remove(child);
        if (child.LifeCycleState is RuleLifeCycle.ShutDownRequested or RuleLifeCycle.ShuttingDown or RuleLifeCycle.Stopped) DecrementLifeTimeCount();

        child.LifeCycleChanged -= Child_LifeCycleChanged;
    }

    public virtual Task StartTaskAsync()
    {
        Start();
        return Task.CompletedTask;
    }

    public virtual Task StopTaskAsync()
    {
        Stop();
        return Task.CompletedTask;
    }

    private RuleLifeCycle ValidateRuleLifeCycleTransition(RuleLifeCycle value)
    {
        var oldState = ruleLifeCycle;
        switch (ruleLifeCycle)
        {
            case RuleLifeCycle.NotStarted:
                if (value != RuleLifeCycle.Starting)
                    throw new InvalidRuleTransitionStateException(FriendlyName, ruleLifeCycle, value
                                                                , [RuleLifeCycle.Starting]);
                break;
            case RuleLifeCycle.Starting:
                if (value != RuleLifeCycle.Started)
                    throw new InvalidRuleTransitionStateException(FriendlyName, ruleLifeCycle, value
                                                                , [RuleLifeCycle.Started]);
                break;
            case RuleLifeCycle.Started:
                if (value != RuleLifeCycle.ShutDownRequested)
                    throw new InvalidRuleTransitionStateException(FriendlyName, ruleLifeCycle, value
                                                                , [RuleLifeCycle.ShutDownRequested]);
                break;
            case RuleLifeCycle.ShutDownRequested:
                if (value is RuleLifeCycle.ShuttingDown) break;

                throw new InvalidRuleTransitionStateException(FriendlyName, ruleLifeCycle, value
                                                            , [RuleLifeCycle.ShuttingDown]);
            case RuleLifeCycle.ShuttingDown:
                if (value is RuleLifeCycle.Stopped) break;

                throw new InvalidRuleTransitionStateException(FriendlyName, ruleLifeCycle, value
                                                            , [RuleLifeCycle.Stopped]);
            case RuleLifeCycle.Stopped:
                if (value is RuleLifeCycle.NotStarted) break;
                throw new InvalidRuleTransitionStateException(FriendlyName, ruleLifeCycle, value
                                                            , [RuleLifeCycle.NotStarted]);
        }

        return oldState;
    }

    private void ParentRule_LifeCycleChanged(IRule sender, RuleLifeCycle oldState, RuleLifeCycle newState)
    {
        if (newState is RuleLifeCycle.ShuttingDown or RuleLifeCycle.ShutDownRequested)
            Logger.DebugFormat("Parent rule: '{0}' shuttingDown on sending Stop to rule: '{1}'")?.WithParams(sender.FriendlyName)
                  ?.AndFinalParam(FriendlyName);
    }

    private void Child_LifeCycleChanged(IRule sender, RuleLifeCycle oldState, RuleLifeCycle newState)
    {
        if (newState == RuleLifeCycle.Stopped)
        {
            Logger.DebugFormat("Child rule: '{0}' stopped on rule: '{1}'")?.WithParams(sender.FriendlyName)
                  ?.AndFinalParam(FriendlyName);
            RemoveChild(sender);
        }
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal(nameof(Context), Context)
           .Field.AlwaysAdd(nameof(FriendlyName), FriendlyName)
           .Field.AlwaysAdd(nameof(Id), Id)
           .Field.AlwaysAdd(nameof(LifeCycleState), LifeCycleState)
           .Field.AlwaysAdd(nameof(instanceNumber), instanceNumber)
           .Complete();

    public override string ToString() =>
        $"{nameof(Context)}: {Context}, {nameof(FriendlyName)}: " +
        $"{FriendlyName}, {nameof(Id)}: {Id}, {nameof(LifeCycleState)}: {LifeCycleState}";
}
