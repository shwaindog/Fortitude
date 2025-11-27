using static FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification.CollectionItemFilterFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;

public enum CollectionItemFilterFlags
{
    DoNotIncludeAndComplete    = int.MinValue
  , DoNotIncludeContinueToNext = -1
  , IncludeContinueToNext      = 0
  , IncludeAndComplete         = int.MaxValue
   ,
}

public readonly record struct CollectionItemResult(CollectionItemFilterFlags IncludeAndContinueTo)
{
    public bool IncludeItem => IncludeAndContinueTo >= 0;
    public int SkipNextCount => IncludeAndContinueTo >= 0 ? (int)IncludeAndContinueTo : Math.Abs((int)IncludeAndContinueTo + 1);
    public bool KeepProcessing => IncludeAndContinueTo != DoNotIncludeAndComplete && IncludeAndContinueTo != IncludeAndComplete;

    public static readonly CollectionItemResult DoNotIncludeAndGoToNext = new (DoNotIncludeContinueToNext);
    public static readonly CollectionItemResult NotIncludedAndComplete = new (DoNotIncludeAndComplete);
    public static readonly CollectionItemResult IncludedContinueToNext = new (IncludeContinueToNext);
    public static readonly CollectionItemResult IncludedAndContinue    = new (IncludeContinueToNext);
    public static readonly CollectionItemResult NotIncludedAndContinue = new (DoNotIncludeContinueToNext);


    public static CollectionItemResult IncludedAndSkipNext(int skipAmount) =>
        new ((CollectionItemFilterFlags)(Math.Abs(skipAmount)));

    public static CollectionItemResult NotIncludedAndSkipNext(int skipAmount) =>
        new ((CollectionItemFilterFlags)(-Math.Abs(skipAmount) + 1));

    public static CollectionItemResult EvaluateIsIncludedAndContinue(bool shouldInclude, int skipAmount = 0) =>
        new ((CollectionItemFilterFlags)(shouldInclude
                ? Math.Abs(skipAmount)
                : -(Math.Abs(skipAmount) + 1)));

    public static CollectionItemResult BetweenRetrieveRange(int currentIndex, int inclusiveFromIndex, int exclusiveToIndex) =>
        new (currentIndex < inclusiveFromIndex
                ? (CollectionItemFilterFlags)(-(inclusiveFromIndex - currentIndex))
                : (currentIndex >= exclusiveToIndex
                    ? DoNotIncludeAndComplete
                    : IncludeContinueToNext));

    public static CollectionItemResult StopOnFirstExclusion(bool shouldInclude, int skipAmount = 0) =>
        new (shouldInclude
                ? ((CollectionItemFilterFlags)(shouldInclude
                    ? Math.Abs(skipAmount)
                    : -(Math.Abs(skipAmount) + 1)))
                : DoNotIncludeAndComplete);

    public static CollectionItemResult First(bool shouldInclude, int skipAmount = 0) =>
        new ((shouldInclude
                ? IncludeAndComplete
                : (CollectionItemFilterFlags)(-(Math.Abs(skipAmount) + 1))));
}
