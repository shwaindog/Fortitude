namespace FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;

public  enum  CollectionItemFilterFlags
{
    DoNotIncludeContinueToNext = -1,
    DoNotIncludeAndComplete    = 0,
    IncludeContinueToNext      = 1,
}

public readonly record struct CollectionItemResult(CollectionItemFilterFlags IncludeAndContinueTo)
{
    public bool IncludeItem => IncludeAndContinueTo > 0;
    public int SkipNextCount => IncludeAndContinueTo > 0 ? (int)IncludeAndContinueTo - 1 : Math.Abs((int)IncludeAndContinueTo) + 1;
    public bool KeepProcessing => IncludeAndContinueTo != 0;
    
    public static readonly CollectionItemResult DoNotIncludeAndGoToNext = new (CollectionItemFilterFlags.DoNotIncludeContinueToNext);  
    
    public static readonly CollectionItemResult DoNotIncludeAndComplete = new (CollectionItemFilterFlags.DoNotIncludeAndComplete);  
    
    public static readonly CollectionItemResult IncludeContinueToNext = new (CollectionItemFilterFlags.IncludeContinueToNext);  
    public static CollectionItemResult IncludeAndSkipNext(int skipAmount) => new ( (CollectionItemFilterFlags)(skipAmount + 1));  
    public static CollectionItemResult DoNotIncludeAndSkipNext(int skipAmount) => new ( (CollectionItemFilterFlags)(-skipAmount - 1));
    
    public static CollectionItemResult EvaluateAndContinue(bool shouldInclude, int skipAmount = 0) => 
        new ((CollectionItemFilterFlags)((shouldInclude ? 1 : -1)  *  Math.Abs(skipAmount == 0 ? 1 : Math.Abs(skipAmount) + 1 )));  
    
    public static CollectionItemResult StopOnFirstExclusion(bool shouldInclude, int skipAmount = 0) => 
        new ((CollectionItemFilterFlags)((shouldInclude ? 1 : 0)  *  Math.Abs(skipAmount == 0 ? 1 : Math.Abs(skipAmount) + 1 )));  
}
