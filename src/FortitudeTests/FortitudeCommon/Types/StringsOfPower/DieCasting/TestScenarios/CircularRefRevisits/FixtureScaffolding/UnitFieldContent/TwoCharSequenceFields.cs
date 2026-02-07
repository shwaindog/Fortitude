// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.UnitFieldContent;


public class TwoCharSequenceFields<TCharSeq>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
    where TCharSeq : class, ICharSequence
{
    public TwoCharSequenceFields()
    {
        FirstCharSequenceField  = null!;
        SecondCharSequenceField = null!;
    }

    public TwoCharSequenceFields(TCharSeq? first, TCharSeq? second)
    {
        FirstCharSequenceField  = first;
        SecondCharSequenceField = second;
    }

    public TCharSeq? FirstCharSequenceField { get; set; }
    public TCharSeq? SecondCharSequenceField { get; set; }

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddCharSeq
               (nameof(FirstCharSequenceField)
              , FirstCharSequenceField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAddCharSeq
               (nameof(SecondCharSequenceField)
              , SecondCharSequenceField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstCharSequenceField)}:{FirstCharSequenceField}, " +
        $"{nameof(SecondCharSequenceField)}:{SecondCharSequenceField})";
}

public class TwoCharSequencesFirstAsSimpleCloakedValueContent<TCharSeq>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TCharSeq> 
    where TCharSeq : class, ICharSequence
{
    private PalantírReveal<TCharSeq>? cachedRevealer;
    
    public TwoCharSequencesFirstAsSimpleCloakedValueContent()
    {
        FirstCharSequenceField  = null!;
        SecondCharSequenceField = null!;
    }

    public TwoCharSequencesFirstAsSimpleCloakedValueContent(TCharSeq? first, TCharSeq? second)
    {
        FirstCharSequenceField  = first;
        SecondCharSequenceField = second;
    }

    public TCharSeq? FirstCharSequenceField { get; set; }
    public TCharSeq? SecondCharSequenceField { get; set; }

    public PalantírReveal<TCharSeq> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(FirstCharSequenceField)
           .AsValue (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstCharSequenceField)
              , FirstCharSequenceField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAddCharSeq
               (nameof(SecondCharSequenceField)
              , SecondCharSequenceField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstCharSequenceField)}:{FirstCharSequenceField}, " +
        $"{nameof(SecondCharSequenceField)}:{SecondCharSequenceField})";
}

public class TwoCharSequencesSecondAsSimpleCloakedValueContent<TCharSeq>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TCharSeq> 
    where TCharSeq : class, ICharSequence
{
    private PalantírReveal<TCharSeq>? cachedRevealer;
    
    public TwoCharSequencesSecondAsSimpleCloakedValueContent()
    {
        FirstCharSequenceField  = null!;
        SecondCharSequenceField = null!;
    }

    public TwoCharSequencesSecondAsSimpleCloakedValueContent(TCharSeq? first, TCharSeq? second)
    {
        FirstCharSequenceField  = first;
        SecondCharSequenceField = second;
    }

    public TCharSeq? FirstCharSequenceField { get; set; }
    public TCharSeq? SecondCharSequenceField { get; set; }

    public PalantírReveal<TCharSeq> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(SecondCharSequenceField)
           .AsValue (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddCharSeq
               (nameof(FirstCharSequenceField)
              , FirstCharSequenceField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondCharSequenceField)
              , SecondCharSequenceField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstCharSequenceField)}:{FirstCharSequenceField}, " +
        $"{nameof(SecondCharSequenceField)}:{SecondCharSequenceField})";
}

public class TwCharSequencesFirstAsSimpleCloakedStringContent<TCharSeq>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TCharSeq> 
    where TCharSeq : class, ICharSequence
{
    private PalantírReveal<TCharSeq>? cachedRevealer;
    
    public TwCharSequencesFirstAsSimpleCloakedStringContent()
    {
        FirstCharSequenceField  = null!;
        SecondCharSequenceField = null!;
    }

    public TwCharSequencesFirstAsSimpleCloakedStringContent(TCharSeq? first, TCharSeq? second)
    {
        FirstCharSequenceField  = first;
        SecondCharSequenceField = second;
    }

    public TCharSeq? FirstCharSequenceField { get; set; }
    public TCharSeq? SecondCharSequenceField { get; set; }

    public PalantírReveal<TCharSeq> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(FirstCharSequenceField)
           .AsString (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstCharSequenceField)
              , FirstCharSequenceField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAddCharSeq
               (nameof(SecondCharSequenceField)
              , SecondCharSequenceField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstCharSequenceField)}:{FirstCharSequenceField}, " +
        $"{nameof(SecondCharSequenceField)}:{SecondCharSequenceField})";
}

public class TwoCharSequencesSecondAsSimpleCloakedStringContent<TCharSeq>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TCharSeq> 
    where TCharSeq : class, ICharSequence
{
    private PalantírReveal<TCharSeq>? cachedRevealer;
    
    public TwoCharSequencesSecondAsSimpleCloakedStringContent()
    {
        FirstCharSequenceField  = null!;
        SecondCharSequenceField = null!;
    }

    public TwoCharSequencesSecondAsSimpleCloakedStringContent(TCharSeq? first, TCharSeq? second)
    {
        FirstCharSequenceField  = first;
        SecondCharSequenceField = second;
    }

    public TCharSeq? FirstCharSequenceField { get; set; }
    public TCharSeq? SecondCharSequenceField { get; set; }

    public PalantírReveal<TCharSeq> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(cloaked)
           .AsString(cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddCharSeq
               (nameof(FirstCharSequenceField)
              , FirstCharSequenceField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondCharSequenceField)
              , SecondCharSequenceField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstCharSequenceField)}:{FirstCharSequenceField}, " +
        $"{nameof(SecondCharSequenceField)}:{SecondCharSequenceField})";
}

public class TwoCharSequencesFirstAsComplexCloakedValueContent<TCharSeq>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TCharSeq> 
    where TCharSeq : class, ICharSequence
{
    private PalantírReveal<TCharSeq>? cachedRevealer;
    
    public TwoCharSequencesFirstAsComplexCloakedValueContent()
    {
        FirstCharSequenceField  = null!;
        SecondCharSequenceField = null!;
    }

    public TwoCharSequencesFirstAsComplexCloakedValueContent(TCharSeq? first, TCharSeq? second)
    {
        FirstCharSequenceField  = first;
        SecondCharSequenceField = second;
    }

    public TCharSeq? FirstCharSequenceField { get; set; }
    public TCharSeq? SecondCharSequenceField { get; set; }

    private readonly int[] logOnlyArray = [1, 2, 3];

    public PalantírReveal<TCharSeq> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(cloaked)
           .AsValue ($"CloakedRevealer{nameof(FirstCharSequenceField)}", cloaked, ValueFormatString, FormattingFlags)
           .LogOnlyCollectionField.AlwaysAddAll(nameof(logOnlyArray),logOnlyArray)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstCharSequenceField)
              , FirstCharSequenceField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAddCharSeq
               (nameof(SecondCharSequenceField)
              , SecondCharSequenceField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstCharSequenceField)}:{FirstCharSequenceField}, " +
        $"{nameof(SecondCharSequenceField)}:{SecondCharSequenceField})";
}

public class TwoCharSequencesSecondAsComplexCloakedValueContent<TCharSeq>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TCharSeq> 
    where TCharSeq : class, ICharSequence
{
    private PalantírReveal<TCharSeq>? cachedRevealer;
    
    public TwoCharSequencesSecondAsComplexCloakedValueContent()
    {
        FirstCharSequenceField  = null!;
        SecondCharSequenceField = null!;
    }

    public TwoCharSequencesSecondAsComplexCloakedValueContent(TCharSeq? first, TCharSeq? second)
    {
        FirstCharSequenceField  = first;
        SecondCharSequenceField = second;
    }

    public TCharSeq? FirstCharSequenceField { get; set; }
    public TCharSeq? SecondCharSequenceField { get; set; }

    private readonly Dictionary<string, int> logOnlyMap = new Dictionary<string, int>()
    {
        { "FirstKey", 1 }
      , { "SecondKey", 2 }
      , { "ThirdKey", 3 }
    };

    public PalantírReveal<TCharSeq> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(cloaked)
           .AsValue ($"CloakedRevealer{nameof(SecondCharSequenceField)}", cloaked, ValueFormatString, FormattingFlags)
           .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logOnlyMap), logOnlyMap)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddCharSeq
               (nameof(FirstCharSequenceField)
              , FirstCharSequenceField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondCharSequenceField)
              , SecondCharSequenceField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstCharSequenceField)}:{FirstCharSequenceField}, " +
        $"{nameof(SecondCharSequenceField)}:{SecondCharSequenceField})";
}

public class TwoCharSequencesFirstAsComplexCloakedStringContent<TCharSeq>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TCharSeq> 
    where TCharSeq : class, ICharSequence
{
    private PalantírReveal<TCharSeq>? cachedRevealer;
    
    public TwoCharSequencesFirstAsComplexCloakedStringContent()
    {
        FirstCharSequenceField  = null!;
        SecondCharSequenceField = null!;
    }

    public TwoCharSequencesFirstAsComplexCloakedStringContent(TCharSeq? first, TCharSeq? second)
    {
        FirstCharSequenceField  = first;
        SecondCharSequenceField = second;
    }

    public TCharSeq? FirstCharSequenceField { get; set; }
    public TCharSeq? SecondCharSequenceField { get; set; }
    
    private readonly StringBuilder logOnlyStringBuilder = new ("For your eyes only");

    public PalantírReveal<TCharSeq> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(cloaked)
           .AsString ($"CloakedRevealer{nameof(FirstCharSequenceField)}", cloaked, ValueFormatString, FormattingFlags)
           .LogOnlyField.AlwaysAdd(nameof(logOnlyStringBuilder), logOnlyStringBuilder)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstCharSequenceField)
              , FirstCharSequenceField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAddCharSeq
               (nameof(SecondCharSequenceField)
              , SecondCharSequenceField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstCharSequenceField)}:{FirstCharSequenceField}, " +
        $"{nameof(SecondCharSequenceField)}:{SecondCharSequenceField})";
}

public class TwoCharSequencesSecondAsComplexCloakedStringContent<TCharSeq>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TCharSeq> 
    where TCharSeq : class, ICharSequence
{
    private PalantírReveal<TCharSeq>? cachedRevealer;
    
    public TwoCharSequencesSecondAsComplexCloakedStringContent()
    {
        FirstCharSequenceField  = null!;
        SecondCharSequenceField = null!;
    }

    public TwoCharSequencesSecondAsComplexCloakedStringContent(TCharSeq? first, TCharSeq? second)
    {
        FirstCharSequenceField  = first;
        SecondCharSequenceField = second;
    }

    public TCharSeq? FirstCharSequenceField { get; set; }
    public TCharSeq? SecondCharSequenceField { get; set; }


    private readonly List<MutableString> logOnlyList = [new ("FirstCharSeq"), new ("SecondCharSeq"), new ("ThirdCharSeq")];

    public PalantírReveal<TCharSeq> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(cloaked)
           .AsString ($"CloakedRevealer{nameof(SecondCharSequenceField)}", cloaked, ValueFormatString, FormattingFlags)
           .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(logOnlyList), logOnlyList)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddCharSeq
               (nameof(FirstCharSequenceField)
              , FirstCharSequenceField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondCharSequenceField)
              , SecondCharSequenceField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstCharSequenceField)}:{FirstCharSequenceField}, " +
        $"{nameof(SecondCharSequenceField)}:{SecondCharSequenceField})";
}