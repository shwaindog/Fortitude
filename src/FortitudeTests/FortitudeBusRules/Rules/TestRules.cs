#region

using Fortitude.EventProcessing.BusRules.MessageBus;
using Fortitude.EventProcessing.BusRules.Messaging;
using Fortitude.EventProcessing.BusRules.Rules;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeTests.FortitudeBusRules.Rules;

public class IncrementingRule : Rule
{
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(IncrementingRule));
    private static int instanceNumber;
    private int startCount;
    private int stopCount;

    public IncrementingRule() : base("IncrementingRule", Interlocked.Increment(ref instanceNumber).ToString()) { }

    public int StartCount => startCount;
    public int StopCount => stopCount;

    public override Task Start()
    {
        base.Start();
        logger.Info("Started IncrementingRule instance {0}", Id);
        IncrementLifeTimeCount();
        Interlocked.Increment(ref startCount);
        return Task.CompletedTask;
    }

    public override Task Stop()
    {
        base.Stop();
        logger.Info("Stopped IncrementingRule instance {0}", Id);
        Interlocked.Increment(ref stopCount);
        return Task.CompletedTask;
    }

    public override string ToString() =>
        $"{FriendlyName}({nameof(instanceNumber)}: {instanceNumber}, {nameof(startCount)}: {startCount}, " +
        $"{nameof(stopCount)}: {stopCount})";
}

public class PublishingRule : Rule
{
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PublishingRule));
    private static int instanceNumber;
    private readonly int maxPublishCount;
    private int startCount;
    private int stopCount;

    public PublishingRule(int maxPublishCount) : base("PublishingRule"
        , Interlocked.Increment(ref instanceNumber).ToString())
    {
        if (maxPublishCount < 2) throw new ArgumentException("Will publish at least two messages");
        this.maxPublishCount = maxPublishCount;
    }

    public int PublishNumber { get; set; }

    public int StartCount => startCount;
    public int StopCount => stopCount;

    public override Task Start()
    {
        logger.Info("Started PublishingRule instance {0}", Id);
        Context.EventBus.PublishAsync(this, "PublishingRule", ++PublishNumber, new DispatchOptions());
        Context.Timer.RunIn(20, PublishInt);
        Interlocked.Increment(ref startCount);
        return Task.CompletedTask;
    }

    public void PublishInt()
    {
        logger.Info("PublishingRule instance {0} publishing message {1}", Id, PublishNumber + 1);
        Context.EventBus.PublishAsync(this, "PublishingRule", ++PublishNumber, new DispatchOptions());
        if (PublishNumber < maxPublishCount) Context.Timer.RunIn(20, PublishInt);
    }

    public override Task Stop()
    {
        base.Stop();
        Interlocked.Increment(ref stopCount);
        return Task.CompletedTask;
    }

    public override string ToString() =>
        $"{FriendlyName}({nameof(instanceNumber)}: {instanceNumber}, {nameof(startCount)}: {startCount}, " +
        $"{nameof(stopCount)}: {stopCount}, {nameof(PublishNumber)}: {PublishNumber})";
}

public class ListeningRule : Rule
{
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(ListeningRule));

    private static int instanceNumber;
    private int startCount;
    private int stopCount;


    public ListeningRule() : base("ListeningRule", Interlocked.Increment(ref instanceNumber).ToString()) { }

    public int ReceiveCount { get; set; }

    public int LastReceivedPublishNumber { get; set; }

    public int StartCount => startCount;
    public int StopCount => stopCount;

    public override Task Start()
    {
        base.Start();
        logger.Info("Started ListeningRule instance {0}", Id);
        Context.EventBus.RegisterListener<int>(this, "PublishingRule", ReceivePublishIntMessage);
        Interlocked.Increment(ref startCount);
        return Task.CompletedTask;
    }

    public void ReceivePublishIntMessage(IMessage<int> currentMessage)
    {
        logger.Info("ListeningRule instance {0} recevied {1}", Id, currentMessage);
        ReceiveCount++;
        LastReceivedPublishNumber = currentMessage.PayLoad.Body;
    }

    public override Task Stop()
    {
        base.Stop();
        Interlocked.Increment(ref stopCount);
        return Task.CompletedTask;
    }

    public override string ToString() =>
        $"{FriendlyName}({nameof(instanceNumber)}: {instanceNumber}, {nameof(startCount)}: {startCount}, " +
        $"{nameof(stopCount)}: {stopCount}, {nameof(ReceiveCount)}: {ReceiveCount}, " +
        $"{nameof(LastReceivedPublishNumber)}: {LastReceivedPublishNumber})";
}

public class RespondingRule : Rule
{
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(ListeningRule));

    private static int instanceNumber;
    private int startCount;
    private int stopCount;

    public RespondingRule() : base("RespondingRule", Interlocked.Increment(ref instanceNumber).ToString()) { }

    public int ReceiveCount { get; set; }

    public int LastReceivedRequestNumber { get; set; }

    public int StartCount => startCount;
    public int StopCount => stopCount;

    public override Task Start()
    {
        base.Start();
        logger.Info("Started RespondingRule instance {0}", Id);
        Context.EventBus.RegisterRequestListener<int, int>(this, "RespondingRule", ReceivePublishIntMessage);
        Interlocked.Increment(ref startCount);
        return Task.CompletedTask;
    }

    public int ReceivePublishIntMessage(IRespondingMessage<int, int> requestMessage)
    {
        logger.Info("RespondingRule instance {0} received {1}", Id, requestMessage.ToString());
        ReceiveCount++;
        LastReceivedRequestNumber = requestMessage.PayLoad.Body;
        return requestMessage.PayLoad.Body + 10;
    }

    public override Task Stop()
    {
        base.Stop();
        Interlocked.Increment(ref stopCount);
        return Task.CompletedTask;
    }

    public override string ToString() =>
        $"{FriendlyName}({nameof(instanceNumber)}: {instanceNumber}, {nameof(startCount)}: {startCount}, " +
        $"{nameof(stopCount)}: {stopCount}, {nameof(ReceiveCount)}: {ReceiveCount}, " +
        $"{nameof(LastReceivedRequestNumber)}: {LastReceivedRequestNumber})";
}

public class RequestingRule : Rule
{
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(ListeningRule));

    private static int instanceNumber;
    private int startCount;
    private int stopCount;


    public RequestingRule() : base("RequestingRule", Interlocked.Increment(ref instanceNumber).ToString()) { }

    public int ReceiveCount { get; set; }

    public int LastReceivedRequestNumber { get; set; }

    public int PublishNumber { get; set; }

    public int StartCount => startCount;
    public int StopCount => stopCount;

    public override async Task Start()
    {
        logger.Info("Started RequestingRule instance {0}", Id);
        var result = await Context.EventBus.RequestAsync<int, int>(this, "RespondingRule", ++PublishNumber
            , new DispatchOptions());
        ReceiveCount++;
        logger.Info("RequestingRule received first result: {0}", result.Response);
        result = await Context.EventBus.RequestAsync<int, int>(this, "RespondingRule", ++PublishNumber
            , new DispatchOptions());
        ReceiveCount++;
        logger.Info("RequestingRule received second result: {0}", result.Response);
        result = await Context.EventBus.RequestAsync<int, int>(this, "RespondingRule", ++PublishNumber
            , new DispatchOptions());
        ReceiveCount++;
        logger.Info("RequestingRule received third result: {0}", result.Response);
        Interlocked.Increment(ref startCount);
    }

    public override Task Stop()
    {
        base.Stop();
        Interlocked.Increment(ref stopCount);
        return Task.CompletedTask;
    }

    public override string ToString() =>
        $"{FriendlyName}({nameof(instanceNumber)}: {instanceNumber}, {nameof(startCount)}: {startCount}, " +
        $"{nameof(stopCount)}: {stopCount}, {nameof(ReceiveCount)}: {ReceiveCount}, " +
        $"{nameof(LastReceivedRequestNumber)}: {LastReceivedRequestNumber})";
}
