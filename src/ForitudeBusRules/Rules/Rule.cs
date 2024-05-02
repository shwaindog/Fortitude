#region

using FortitudeBusRules.BusMessaging.Pipelines.Timers;
using FortitudeBusRules.Messages;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;

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

public interface IRule
{
    IEnumerable<IAsyncValueTaskDisposable> OnStopResourceCleanup();
    T AddOnStopResourceCleanup<T>(T resourceCleanup) where T : IAsyncValueTaskDisposable;
    IRule ParentRule { get; }
    IQueueContext Context { get; set; }
    string FriendlyName { get; }
    string? Id { get; set; }

    IRuleTimer Timer { get; }
    RuleLifeCycle LifeCycleState { get; set; }
    IEnumerable<IRule> ChildRules { get; }
    RuleFilter AppliesToThisRule { get; }
    RuleFilter NotAppliesToThisRule { get; }

    event LifeCycleChangeHandler LifeCycleChanged;
    ValueTask StartAsync();
    ValueTask StopAsync();
    void Start();
    void Stop();
    void AddChild(IRule child);
}

public interface IListeningRule : IRule
{
    int IncrementLifeTimeCount();
    int DecrementLifeTimeCount();
    bool ShouldBeStopped();

    ValueTask MessageBusStopAsync();
}

public class InvalidRuleTransitionStateException(string name, RuleLifeCycle current, RuleLifeCycle proposed
        , List<RuleLifeCycle> allowed)
    : Exception($"Invalid rule: '{name}' state transition from {current} to {proposed} allowed are [{allowed}]");

public class Rule : IListeningRule
{
    private IRuleTimer? ruleTimer;
    private List<IAsyncValueTaskDisposable> onStopResourceCleanup = new ();
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(Rule));

    public static readonly Rule NoKnownSender = new()
    {
        Id = "UnknownSender"
    };

    private readonly List<IRule> children = [];
    private RuleFilter? appliesToThisRuleFilter;
    private RuleFilter? notAppliesToThisRuleFilter;
    private IRule? parentRule;

    private int refCount;
    private RuleLifeCycle ruleLifeCycle;

    public Rule()
    {
        LifeCycleState = RuleLifeCycle.NotStarted;
        parentRule = null!;
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
            IncrementLifeTimeCount();
        child.LifeCycleChanged += Child_LifeCycleChanged;
    }

    public IEnumerable<IRule> ChildRules => children;
    public IQueueContext Context { get; set; } = null!;

    public string FriendlyName { get; set; } = "Friendly Name Not Set";
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

    public int IncrementLifeTimeCount() => Interlocked.Increment(ref refCount);

    public int DecrementLifeTimeCount() => Interlocked.Decrement(ref refCount);

    public bool ShouldBeStopped() => LifeCycleState == RuleLifeCycle.Started && refCount <= 0;

    public virtual ValueTask StartAsync()
    {
        return new ValueTask(StartTaskAsync());
    }

    public virtual ValueTask StopAsync()
    {
        return new ValueTask(StopTaskAsync());
    }

    public virtual void Start() { }

    public virtual void Stop() { }

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
                        , [ RuleLifeCycle.Starting ]);
                break;
            case RuleLifeCycle.Starting:
                if (value != RuleLifeCycle.Started)
                    throw new InvalidRuleTransitionStateException(FriendlyName, ruleLifeCycle, value
                        , [ RuleLifeCycle.Started ]);
                break;
            case RuleLifeCycle.Started:
                if (value != RuleLifeCycle.ShutDownRequested)
                    throw new InvalidRuleTransitionStateException(FriendlyName, ruleLifeCycle, value
                        , [ RuleLifeCycle.ShutDownRequested ]);
                break;
            case RuleLifeCycle.ShutDownRequested:
                if (value is RuleLifeCycle.ShuttingDown)
                    break;

                throw new InvalidRuleTransitionStateException(FriendlyName, ruleLifeCycle, value
                    , [ RuleLifeCycle.ShuttingDown ]);
            case RuleLifeCycle.ShuttingDown:
                if (value is RuleLifeCycle.Stopped)
                    break;

                throw new InvalidRuleTransitionStateException(FriendlyName, ruleLifeCycle, value
                    , [ RuleLifeCycle.Stopped ]);
            case RuleLifeCycle.Stopped:
                if (value is RuleLifeCycle.NotStarted)
                    break;
                throw new InvalidRuleTransitionStateException(FriendlyName, ruleLifeCycle, value
                    , [RuleLifeCycle.NotStarted ]);
        }

        return oldState;
    }

    private void ParentRule_LifeCycleChanged(IRule sender, RuleLifeCycle oldState, RuleLifeCycle newState)
    {
        if (newState is RuleLifeCycle.ShuttingDown or RuleLifeCycle.ShutDownRequested)
        {
            Logger.Debug("Parent rule: '{0}' shuttingDown on sending Stop to rule: '{1}'", sender.FriendlyName
                , FriendlyName);
            Context.RegisteredOn.EnqueuePayloadBody(Stop, sender, MessageType.RunActionPayload, null);
        }
    }

    private void Child_LifeCycleChanged(IRule sender, RuleLifeCycle oldState, RuleLifeCycle newState)
    {
        if (newState == RuleLifeCycle.Stopped)
        {
            Logger.Debug("Child rule: '{0}' stopped on rule: '{1}'", sender.FriendlyName, FriendlyName);
            DecrementLifeTimeCount();
            sender.LifeCycleChanged -= Child_LifeCycleChanged;
            children.Remove(sender);
        }
    }

    public async ValueTask MessageBusStopAsync()
    {
        if (LifeCycleState != RuleLifeCycle.ShutDownRequested) return;
        LifeCycleState = RuleLifeCycle.ShuttingDown;
        await StopAsync();
    }

    public override string ToString() =>
        $"{nameof(Context)}: {Context}, {nameof(FriendlyName)}: " +
        $"{FriendlyName}, {nameof(Id)}: {Id}, {nameof(LifeCycleState)}: {LifeCycleState}";
}
