// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.UnitFieldContent;


public class TwoStringFields: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString,  IPalantirRevealerFactory<string>
{
    public TwoStringFields()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringFields(string? first, string? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public string? FirstStringField { get; set; }
    public string? SecondStringField { get; set; }

    public PalantírReveal<string> CreateRevealer => (cloaked, tos) =>
        tos.StartComplexType(cloaked)
           .Field.AlwaysAdd
               ($"CloakedRevealer{nameof(SecondStringField)}"
              , cloaked, ValueFormatString)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstStringField)
              , FirstStringField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondStringField)
              , SecondStringField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringField)}:{FirstStringField}, " +
        $"{nameof(SecondStringField)}:{SecondStringField})";
}

public class TwoStringsFirstAsSimpleCloakedValueContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
   , IPalantirRevealerFactory<string> 
{
    private PalantírReveal<string>? cachedRevealer;
    
    public TwoStringsFirstAsSimpleCloakedValueContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringsFirstAsSimpleCloakedValueContent(string? first, string? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public string? FirstStringField { get; set; }
    public string? SecondStringField { get; set; }

    public PalantírReveal<string> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(FirstStringField)
           .AsValue (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstStringField)
              , FirstStringField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondStringField)
              , SecondStringField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringField)}:{FirstStringField}, " +
        $"{nameof(SecondStringField)}:{SecondStringField})";
}

public class TwoStringsSecondAsSimpleCloakedValueContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<string> 
{
    private PalantírReveal<string>? cachedRevealer;
    
    public TwoStringsSecondAsSimpleCloakedValueContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringsSecondAsSimpleCloakedValueContent(string? first, string? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public string? FirstStringField { get; set; }
    public string? SecondStringField { get; set; }

    public PalantírReveal<string> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(SecondStringField)
           .AsValue (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstStringField)
              , FirstStringField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondStringField)
              , SecondStringField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringField)}:{FirstStringField}, " +
        $"{nameof(SecondStringField)}:{SecondStringField})";
}

public class TwoStringsFirstAsSimpleCloakedStringContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<string> 
{
    private PalantírReveal<string>? cachedRevealer;
    
    public TwoStringsFirstAsSimpleCloakedStringContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringsFirstAsSimpleCloakedStringContent(string? first, string? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public string? FirstStringField { get; set; }
    public string? SecondStringField { get; set; }

    public PalantírReveal<string> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(FirstStringField)
           .AsString (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstStringField)
              , FirstStringField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondStringField)
              , SecondStringField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringField)}:{FirstStringField}, " +
        $"{nameof(SecondStringField)}:{SecondStringField})";
}

public class TwoStringsSecondAsSimpleCloakedStringContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<string> 
{
    private PalantírReveal<string>? cachedRevealer;
    
    public TwoStringsSecondAsSimpleCloakedStringContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringsSecondAsSimpleCloakedStringContent(string? first, string? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public string? FirstStringField { get; set; }
    public string? SecondStringField { get; set; }

    public PalantírReveal<string> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(SecondStringField)
           .AsString (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstStringField)
              , FirstStringField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondStringField)
              , SecondStringField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringField)}:{FirstStringField}, " +
        $"{nameof(SecondStringField)}:{SecondStringField})";
}

public class TwoStringsFirstAsComplexCloakedValueContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<string> 
{
    private PalantírReveal<string>? cachedRevealer;
    
    public TwoStringsFirstAsComplexCloakedValueContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringsFirstAsComplexCloakedValueContent(string? first, string? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public string? FirstStringField { get; set; }
    public string? SecondStringField { get; set; }

    private readonly int[] logOnlyArray = [1, 2, 3];

    public PalantírReveal<string> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(FirstStringField)
           .AsValue ("CloakedRevealer" + nameof(FirstStringField), cloaked, ValueFormatString, FormattingFlags)
           .LogOnlyCollectionField.AlwaysAddAll(nameof(logOnlyArray),logOnlyArray)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstStringField)
              , FirstStringField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondStringField)
              , SecondStringField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringField)}:{FirstStringField}, " +
        $"{nameof(SecondStringField)}:{SecondStringField})";
}

public class TwoStringsSecondAsComplexCloakedValueContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<string> 
{
    private PalantírReveal<string>? cachedRevealer;
    
    public TwoStringsSecondAsComplexCloakedValueContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringsSecondAsComplexCloakedValueContent(string? first, string? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public string? FirstStringField { get; set; }
    public string? SecondStringField { get; set; }

    private readonly Dictionary<string, int> logOnlyMap = new Dictionary<string, int>()
    {
        { "FirstKey", 1 }
      , { "SecondKey", 2 }
      , { "ThirdKey", 3 }
    };

    public PalantírReveal<string> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(SecondStringField)
           .AsValue ("CloakedRevealer" + nameof(SecondStringField), cloaked, ValueFormatString, FormattingFlags)
           .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logOnlyMap), logOnlyMap)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstStringField)
              , FirstStringField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondStringField)
              , SecondStringField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringField)}:{FirstStringField}, " +
        $"{nameof(SecondStringField)}:{SecondStringField})";
}

public class TwoStringsFirstAsComplexCloakedStringContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<string> 
{
    private PalantírReveal<string>? cachedRevealer;
    
    public TwoStringsFirstAsComplexCloakedStringContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringsFirstAsComplexCloakedStringContent(string? first, string? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public string? FirstStringField { get; set; }
    public string? SecondStringField { get; set; }
    
    private readonly StringBuilder logOnlyStringBuilder = new ("For your eyes only");

    public PalantírReveal<string> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(FirstStringField)
           .AsString ("CloakedRevealer" + nameof(FirstStringField), cloaked, ValueFormatString, FormattingFlags)
           .LogOnlyField.AlwaysAdd(nameof(logOnlyStringBuilder), logOnlyStringBuilder)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstStringField)
              , FirstStringField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondStringField)
              , SecondStringField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringField)}:{FirstStringField}, " +
        $"{nameof(SecondStringField)}:{SecondStringField})";
}

public class TwoStringsSecondAsComplexCloakedStringContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<string> 
{
    private PalantírReveal<string>? cachedRevealer;
    
    public TwoStringsSecondAsComplexCloakedStringContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringsSecondAsComplexCloakedStringContent(string? first, string? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public string? FirstStringField { get; set; }
    public string? SecondStringField { get; set; }


    private readonly List<MutableString> logOnlyList = [new ("FirstCharSeq"), new ("SecondCharSeq"), new ("ThirdCharSeq")];

    public PalantírReveal<string> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(SecondStringField)
           .AsString ("CloakedRevealer" + nameof(SecondStringField), cloaked, ValueFormatString, FormattingFlags)
           .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(logOnlyList), logOnlyList)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstStringField)
              , FirstStringField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondStringField)
              , SecondStringField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringField)}:{FirstStringField}, " +
        $"{nameof(SecondStringField)}:{SecondStringField})";
}
