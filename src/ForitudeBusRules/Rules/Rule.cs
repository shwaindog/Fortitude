#region

using Fortitude.EventProcessing.BusRules.Messaging;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace Fortitude.EventProcessing.BusRules.Rules;

public enum RuleLifeCycle
{
    NotStarted = 0
    , Starting
    , Started
    , ShuttingDown
    , AwaitingChildShutdown
    , AwaitingListenUnsubscription
    , Stopped
}

public delegate void LifeCycleChangeHandler(IRule sender, RuleLifeCycle oldState, RuleLifeCycle newState);

public interface IRule
{
    IRule ParentRule { get; }
    IEventContext Context { get; set; }
    string FriendlyName { get; }
    string? Id { get; set; }
    RuleLifeCycle LifeCycleState { get; set; }
    IEnumerable<IRule> ChildRules { get; }

    event LifeCycleChangeHandler LifeCycleChanged;
    Task Start();
    Task Stop();
    void AddChild(IRule child);
}

public interface IListeningRule : IRule
{
    int IncrementLifeTimeCount();
    int DecrementLifeTimeCount();
    bool ShouldBeStopped();
}

public class InvalidRuleTransitionStateException : Exception
{
    public InvalidRuleTransitionStateException(string name, RuleLifeCycle current, RuleLifeCycle proposed
        , List<RuleLifeCycle> allowed) : base(
        $"Invalid rule: '{name}' state transition from {current} to {proposed} allowed are [{allowed}]") { }
}

public class Rule : IListeningRule
{
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(Rule));

    public static readonly Rule NoKnownSender = new()
    {
        Id = "UnknownSender"
    };

    private readonly List<IRule> children = new();
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

    public Rule(string id) : this() => Id = id;

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

    public void AddChild(IRule child)
    {
        children.Add(child);
        if (child.LifeCycleState is RuleLifeCycle.NotStarted or RuleLifeCycle.Starting or RuleLifeCycle.Started)
            IncrementLifeTimeCount();
        child.LifeCycleChanged += Child_LifeCycleChanged;
    }

    public IEnumerable<IRule> ChildRules => children;
    public IEventContext Context { get; set; } = null!;

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

    public int IncrementLifeTimeCount() => Interlocked.Increment(ref refCount);

    public int DecrementLifeTimeCount() => Interlocked.Decrement(ref refCount);

    public bool ShouldBeStopped() => LifeCycleState == RuleLifeCycle.Started && refCount <= 0;

    public virtual Task Start() => Task.CompletedTask;

    public virtual Task Stop() => Task.CompletedTask;

    private RuleLifeCycle ValidateRuleLifeCycleTransition(RuleLifeCycle value)
    {
        var oldState = ruleLifeCycle;
        switch (ruleLifeCycle)
        {
            case RuleLifeCycle.NotStarted:
                if (value != RuleLifeCycle.Starting)
                    throw new InvalidRuleTransitionStateException(FriendlyName, ruleLifeCycle, value
                        , new List<RuleLifeCycle> { RuleLifeCycle.Starting });
                break;
            case RuleLifeCycle.Starting:
                if (value != RuleLifeCycle.Started)
                    throw new InvalidRuleTransitionStateException(FriendlyName, ruleLifeCycle, value
                        , new List<RuleLifeCycle> { RuleLifeCycle.Started });
                break;
            case RuleLifeCycle.Started:
                if (value != RuleLifeCycle.ShuttingDown)
                    throw new InvalidRuleTransitionStateException(FriendlyName, ruleLifeCycle, value
                        , new List<RuleLifeCycle> { RuleLifeCycle.ShuttingDown });
                break;
            case RuleLifeCycle.ShuttingDown:
                if (value is RuleLifeCycle.Stopped or RuleLifeCycle.AwaitingChildShutdown
                    or RuleLifeCycle.AwaitingListenUnsubscription)
                    break;

                throw new InvalidRuleTransitionStateException(FriendlyName, ruleLifeCycle, value
                    , new List<RuleLifeCycle>
                    {
                        RuleLifeCycle.Stopped, RuleLifeCycle.AwaitingChildShutdown
                        , RuleLifeCycle.AwaitingListenUnsubscription
                    });
            case RuleLifeCycle.AwaitingChildShutdown:
                if (value is RuleLifeCycle.Stopped or RuleLifeCycle.AwaitingListenUnsubscription) break;

                throw new InvalidRuleTransitionStateException(FriendlyName, ruleLifeCycle, value
                    , new List<RuleLifeCycle>
                    {
                        RuleLifeCycle.Stopped, RuleLifeCycle.AwaitingListenUnsubscription
                    });
            case RuleLifeCycle.AwaitingListenUnsubscription:
                if (value is RuleLifeCycle.Stopped or RuleLifeCycle.AwaitingChildShutdown) break;

                throw new InvalidRuleTransitionStateException(FriendlyName, ruleLifeCycle, value
                    , new List<RuleLifeCycle>
                    {
                        RuleLifeCycle.Stopped, RuleLifeCycle.AwaitingChildShutdown
                    });
            case RuleLifeCycle.Stopped:
                throw new InvalidRuleTransitionStateException(FriendlyName, ruleLifeCycle, value
                    , new List<RuleLifeCycle>());
        }

        return oldState;
    }

    private void ParentRule_LifeCycleChanged(IRule sender, RuleLifeCycle oldState, RuleLifeCycle newState)
    {
        if (newState == RuleLifeCycle.ShuttingDown)
        {
            logger.Debug("Parent rule: '{0}' shuttingDown on sending Stop to rule: '{1}'", sender.FriendlyName
                , FriendlyName);
            Context.RegisteredOn.EnqueuePayload(Stop, sender, null, MessageType.RunActionPayload);
        }
    }

    private void Child_LifeCycleChanged(IRule sender, RuleLifeCycle oldState, RuleLifeCycle newState)
    {
        if (newState == RuleLifeCycle.Stopped)
        {
            logger.Debug("Child rule: '{0}' stopped on rule: '{1}'", sender.FriendlyName, FriendlyName);
            DecrementLifeTimeCount();
            sender.LifeCycleChanged -= Child_LifeCycleChanged;
            children.Remove(sender);
        }
    }

    public override string ToString() =>
        $"{nameof(Context)}: {Context}, {nameof(FriendlyName)}: " +
        $"{FriendlyName}, {nameof(Id)}: {Id}, {nameof(LifeCycleState)}: {LifeCycleState}";
}
