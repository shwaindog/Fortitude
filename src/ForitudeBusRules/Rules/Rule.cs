namespace Fortitude.EventProcessing.BusRules.Rules;

public enum RuleLifeCycle
{
    NotStarted = 0
    , Starting
    , Started
    , ShutingDown
}

public interface IRule
{
    IEventContext Context { get; set; }
    string FriendlyName { get; }
    string? Id { get; set; }
    void Start();
    void Stop();
}

public interface IListeningRule : IRule
{
    RuleLifeCycle LifeCycleState { get; }
    int IncrementLifeTimeCount();
    int DecrementLifeTimeCount();
    bool ShouldBeStopped();
}

public class Rule : IListeningRule
{
    public static readonly Rule NoKnownSender = new() { Id = "UnknownSender" };

    private int refCount = 0;
    public Rule() => LifeCycleState = RuleLifeCycle.NotStarted;

    public Rule(string friendlyName, string id) : this()
    {
        FriendlyName = friendlyName;
        Id = id;
    }

    public Rule(string id) : this() => Id = id;

    public IEventContext Context { get; set; } = null!;

    public string FriendlyName { get; set; } = "Friendly Name Not Set";
    public string? Id { get; set; }

    public RuleLifeCycle LifeCycleState { get; private set; }

    public int IncrementLifeTimeCount() => Interlocked.Increment(ref refCount);

    public int DecrementLifeTimeCount() => Interlocked.Decrement(ref refCount);

    public bool ShouldBeStopped() => refCount <= 0;

    public virtual void Start()
    {
        LifeCycleState = RuleLifeCycle.Started;
    }

    public virtual void Stop()
    {
        LifeCycleState = RuleLifeCycle.ShutingDown;
    }

    public override string ToString() =>
        $"{nameof(Context)}: {Context}, {nameof(FriendlyName)}: " +
        $"{FriendlyName}, {nameof(Id)}: {Id}, {nameof(LifeCycleState)}: {LifeCycleState}";
}
