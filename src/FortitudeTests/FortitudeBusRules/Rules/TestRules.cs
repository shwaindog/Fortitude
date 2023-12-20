#region

using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;
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

    public override void Start()
    {
        base.Start();
        logger.Info("Started IncrementingRule instance {0}", Id);
        IncrementLifeTimeCount();
        Interlocked.Increment(ref startCount);
    }

    public override void Stop()
    {
        base.Stop();
        logger.Info("Stopped IncrementingRule instance {0}", Id);
        Interlocked.Increment(ref stopCount);
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

    public PublishingRule(int maxPublishCount, string publishAddress = "PublishingRule") : base("PublishingRule"
        , Interlocked.Increment(ref instanceNumber).ToString())
    {
        if (maxPublishCount < 2) throw new ArgumentException("Will publish at least two messages");
        this.maxPublishCount = maxPublishCount;
        PublishAddress = publishAddress;
    }

    public string PublishAddress { get; }

    public int PublishNumber { get; set; }

    public int StartCount => startCount;
    public int StopCount => stopCount;

    public override ValueTask StartAsync()
    {
        try
        {
            logger.Info("Started PublishingRule instance {0}", Id);
            this.PublishAsync(PublishAddress, ++PublishNumber, new DispatchOptions());
            Context.Timer.RunIn(20, PublishInt);
            Interlocked.Increment(ref startCount);
        }
        catch (Exception ex)
        {
            logger.Warn("PublishingRule caught exception on timer callback StartAsync.  {0}", ex);
        }
        finally
        {
            logger.Info("PublishingRule finished StartAsync");
        }

        return new ValueTask();
    }

    public void PublishInt()
    {
        try
        {
            logger.Info("PublishingRule instance {0} publishing message {1}", Id, PublishNumber + 1);
            this.PublishAsync(PublishAddress, ++PublishNumber, new DispatchOptions());
            if (PublishNumber < maxPublishCount) Context.Timer.RunIn(20, PublishInt);
        }
        catch (Exception ex)
        {
            logger.Warn("PublishingRule caught exception on timer callback PublishInt.  {0}", ex);
        }
        finally
        {
            logger.Info("PublishingRule finished PublishInt");
        }
    }

    public override ValueTask StopAsync()
    {
        base.Stop();
        Interlocked.Increment(ref stopCount);
        return new ValueTask();
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


    public ListeningRule(string listenAddress = "PublishingRule") : base("ListeningRule"
        , Interlocked.Increment(ref instanceNumber).ToString()) =>
        ListenAddress = listenAddress;

    public string ListenAddress { get; }

    public int ReceiveCount { get; set; }

    public int LastReceivedPublishNumber { get; set; }

    public int StartCount => startCount;
    public int StopCount => stopCount;

    public override Task StartTaskAsync()
    {
        base.Start();
        logger.Info("Started ListeningRule instance {0}", Id);
        this.RegisterListener<int>(ListenAddress, ReceivePublishIntMessage);
        Interlocked.Increment(ref startCount);
        return Task.CompletedTask;
    }

    public void ReceivePublishIntMessage(IMessage<int> currentMessage)
    {
        try
        {
            logger.Info($"ListeningRule instance {Id} received {currentMessage}");
            ReceiveCount++;
            LastReceivedPublishNumber = currentMessage.PayLoad.Body;
        }
        catch (Exception ex)
        {
            logger.Warn("ListeningRule got error processing ReceivePublishIntMessage {0}", ex);
        }
        finally
        {
            logger.Info("ListeningRule finished ReceivePublishIntMessage");
        }
    }

    public override Task StopTaskAsync()
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
    private readonly int modifier;
    private int startCount;
    private int stopCount;

    public RespondingRule(string listenAddress = "RespondingRule", int modifier = 10) : base("RespondingRule"
        , Interlocked.Increment(ref instanceNumber).ToString())
    {
        ListenAddress = listenAddress;
        this.modifier = modifier;
    }

    public string ListenAddress { get; }

    public int ReceiveCount { get; set; }

    public int LastReceivedRequestNumber { get; set; }

    public int StartCount => startCount;
    public int StopCount => stopCount;

    public override ValueTask StartAsync()
    {
        base.Start();
        logger.Info("Started RespondingRule instance {0}", Id);
        this.RegisterRequestListener<int, int>(ListenAddress, ReceivePublishIntMessage);
        Interlocked.Increment(ref startCount);
        return new ValueTask();
    }

    public int ReceivePublishIntMessage(IRespondingMessage<int, int> requestMessage)
    {
        logger.Info("RespondingRule instance {0} received {1}", Id, requestMessage.ToString());
        ReceiveCount++;
        LastReceivedRequestNumber = requestMessage.PayLoad.Body;
        return requestMessage.PayLoad.Body + modifier;
    }

    public override void Stop()
    {
        base.Stop();
        Interlocked.Increment(ref stopCount);
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


    public RequestingRule(string requestAddress = "RespondingRule") :
        base("RequestingRule", Interlocked.Increment(ref instanceNumber).ToString()) =>
        RequestAddress = requestAddress;

    public string RequestAddress { get; }

    public int ReceiveCount { get; set; }

    public int LastReceivedRequestNumber { get; set; }

    public int PublishNumber { get; set; }

    public int StartCount => startCount;
    public int StopCount => stopCount;

    public override async ValueTask StartAsync()
    {
        logger.Info("Started RequestingRule instance {0}", Id);
        var result = await this.RequestAsync<int, int>(RequestAddress, ++PublishNumber
            , new DispatchOptions());
        ReceiveCount++;
        logger.Info("RequestingRule received first result: {0}", result.Response);
        result = await this.RequestAsync<int, int>(RequestAddress, ++PublishNumber
            , new DispatchOptions());
        ReceiveCount++;
        logger.Info("RequestingRule received second result: {0}", result.Response);
        result = await this.RequestAsync<int, int>(RequestAddress, ++PublishNumber
            , new DispatchOptions());
        ReceiveCount++;
        logger.Info("RequestingRule received third result: {0}", result.Response);
        Interlocked.Increment(ref startCount);
    }

    public override Task StopTaskAsync()
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

public class AsyncValueTaskRespondingRule : Rule
{
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(ListeningRule));

    private static int instanceNumber;
    private int startCount;
    private int stopCount;

    public AsyncValueTaskRespondingRule(string listenAddress = "AsyncValueTaskRespondingRule"
        , string requestAddress = "RespondingRule") : base("AsyncValueTaskRespondingRule"
        , Interlocked.Increment(ref instanceNumber).ToString())
    {
        ListenAddress = listenAddress;
        RequestAddress = requestAddress;
    }

    public string ListenAddress { get; }

    public string RequestAddress { get; }

    public int ReceiveCount { get; set; }

    public int LastReceivedRequestNumber { get; set; }
    public int LastReceivedResponseNumber { get; set; }

    public int StartCount => startCount;
    public int StopCount => stopCount;

    public override Task StartTaskAsync()
    {
        base.Start();
        logger.Info("Started AsyncValueTaskRespondingRule instance {0}", Id);
        this.RegisterRequestListener<int, int>(ListenAddress, ReceivePublishIntMessage);
        Interlocked.Increment(ref startCount);
        return Task.CompletedTask;
    }

    public async ValueTask<int> ReceivePublishIntMessage(IRespondingMessage<int, int> requestMessage)
    {
        logger.Info("AsyncValueTaskRespondingRule instance {0} received {1}", Id, requestMessage.ToString());
        ReceiveCount++;
        LastReceivedRequestNumber = requestMessage.PayLoad.Body;
        var calculatedResult = await this.RequestAsync<int, int>(RequestAddress
            , LastReceivedRequestNumber
            , new DispatchOptions());
        LastReceivedResponseNumber = calculatedResult.Response;
        logger.Info("AsyncValueTaskRespondingRule instance {0} received response {1}", Id, LastReceivedResponseNumber);
        return LastReceivedResponseNumber;
    }

    public override Task StopTaskAsync()
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

public class AsyncTaskRespondingRule : Rule
{
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(ListeningRule));

    private static int instanceNumber;
    private int startCount;
    private int stopCount;

    public AsyncTaskRespondingRule(string listenAddress = "AsyncTaskRespondingRule"
        , string requestAddress = "RespondingRule") : base("AsyncTaskRespondingRule"
        , Interlocked.Increment(ref instanceNumber).ToString())
    {
        ListenAddress = listenAddress;
        RequestAddress = requestAddress;
    }

    public string ListenAddress { get; }

    public string RequestAddress { get; }

    public int ReceiveCount { get; set; }

    public int LastReceivedRequestNumber { get; set; }
    public int LastReceivedResponseNumber { get; set; }

    public int StartCount => startCount;
    public int StopCount => stopCount;

    public override Task StartTaskAsync()
    {
        base.Start();
        logger.Info("Started AsyncTaskRespondingRule instance {0}", Id);
        this.RegisterRequestListener<int, int>(ListenAddress, ReceivePublishIntMessage);
        Interlocked.Increment(ref startCount);
        return Task.CompletedTask;
    }

    public async Task<int> ReceivePublishIntMessage(IRespondingMessage<int, int> requestMessage)
    {
        logger.Info("AsyncTaskRespondingRule instance {0} received {1}", Id, requestMessage.ToString());
        ReceiveCount++;
        LastReceivedRequestNumber = requestMessage.PayLoad.Body;
        var calculatedResult = await this.RequestAsync<int, int>(RequestAddress, LastReceivedRequestNumber
            , new DispatchOptions());
        LastReceivedResponseNumber = calculatedResult.Response;
        logger.Info("AsyncTaskRespondingRule instance {0} received response {1}", Id, LastReceivedResponseNumber);
        return LastReceivedResponseNumber;
    }

    public override Task StopTaskAsync()
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
