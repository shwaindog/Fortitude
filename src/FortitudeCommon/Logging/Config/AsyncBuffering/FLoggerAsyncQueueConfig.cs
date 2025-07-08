namespace FortitudeCommon.Logging.Config.AsyncBuffering;

public enum AsyncProcessingType
{
    Default
  , AsyncDisabled
  , SingleBackgroundAsyncThread
  , ConfigDefinedAsyncThreads
  , AsyncUsesThreadPool
  , ConfigEventBusQueues
}

public enum QueueExpansionHandling
{
    None
  , LinearToMaxCap
  , QuadraticToMaxCap
  , LinearUnlimited
  , QuadraticUnlimited
}

public enum FullQueueHandling
{
    Default
  , Block
  , DropNewest
  , DropOldest
  , DropNewestForwardToFailAppender
  , DropOldestForwardToFailAppender
  , DropOldestTwoAddDroppedLog
  , DropNewestTwoAddDroppedLog
  , DropAllDebugLevelInQueue
  , DropAllDebugInfoLevelInQueue
  , DropAllAndBounceConsumer
  , DropEveryQueueInterval
  , DropDebugQueueInterval
  , DropDebugAndInfoQueueInterval
}

public interface IFLoggerAsyncBufferingConfig
{
    AsyncProcessingType AsyncProcessingType { get; }

    int DefaultBufferEntriesSize { get; }

    int DefaultMaxBufferEntriesSize { get; }

    FullQueueHandling DefaultBufferFullHandling { get; }

    int DefaultDropInterval { get; }

    int DefaultBufferBatchConsumeSize { get; }

    int InitialAsyncProcessing { get; }

    int MaxAsyncProcessing { get; }

    int DefaultAppenderAsyncQueueNumber { get; }
}

internal class FLoggerAsyncQueueConfig
{

}
