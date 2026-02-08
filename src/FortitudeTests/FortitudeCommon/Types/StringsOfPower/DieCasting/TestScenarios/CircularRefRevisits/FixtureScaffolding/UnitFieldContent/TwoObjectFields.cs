// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.UnitFieldContent;


public class TwoObjectFields: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
{
    public TwoObjectFields()
    {
        FirstObjectField  = null!;
        SecondObjectField = null!;
    }

    public TwoObjectFields(object? first, object? second)
    {
        FirstObjectField  = first;
        SecondObjectField = second;
    }

    public object? FirstObjectField { get; set; }
    public object? SecondObjectField { get; set; }

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddMatch(nameof(FirstObjectField), FirstObjectField, ValueFormatString, FormattingFlags)
           .Field.AlwaysAddMatch(nameof(SecondObjectField), SecondObjectField, ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstObjectField)}:{FirstObjectField}, " +
        $"{nameof(SecondObjectField)}:{SecondObjectField})";
}

public class TwoObjectsFirstAsSimpleCloakedValueContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
   , IPalantirRevealerFactory<object> 
{
    private PalantírReveal<object>? cachedRevealer;
    
    public TwoObjectsFirstAsSimpleCloakedValueContent()
    {
        FirstObjectField  = null!;
        SecondObjectField = null!;
    }

    public TwoObjectsFirstAsSimpleCloakedValueContent(object? first, object? second)
    {
        FirstObjectField  = first;
        SecondObjectField = second;
    }

    public object? FirstObjectField { get; set; }
    public object? SecondObjectField { get; set; }

    public PalantírReveal<object> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(FirstObjectField)
           .AsValueMatch(cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal(nameof(FirstObjectField), FirstObjectField, CreateRevealer , ValueFormatString, FormattingFlags)
           .Field.AlwaysAddMatch(nameof(SecondObjectField), SecondObjectField, ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstObjectField)}:{FirstObjectField}, " +
        $"{nameof(SecondObjectField)}:{SecondObjectField})";
}

public class TwoObjectsSecondAsSimpleCloakedValueContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<object> 
{
    private PalantírReveal<object>? cachedRevealer;
    
    public TwoObjectsSecondAsSimpleCloakedValueContent()
    {
        FirstObjectField  = null!;
        SecondObjectField = null!;
    }

    public TwoObjectsSecondAsSimpleCloakedValueContent(object? first, object? second)
    {
        FirstObjectField  = first;
        SecondObjectField = second;
    }

    public object? FirstObjectField { get; set; }
    public object? SecondObjectField { get; set; }

    public PalantírReveal<object> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(SecondObjectField)
           .AsValueMatch(cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddMatch(nameof(FirstObjectField), FirstObjectField, ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal(nameof(SecondObjectField), SecondObjectField, CreateRevealer, ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstObjectField)}:{FirstObjectField}, " +
        $"{nameof(SecondObjectField)}:{SecondObjectField})";
}

public class TwoObjectsFirstAsSimpleCloakedStringContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<object> 
{
    private PalantírReveal<object>? cachedRevealer;
    
    public TwoObjectsFirstAsSimpleCloakedStringContent()
    {
        FirstObjectField  = null!;
        SecondObjectField = null!;
    }

    public TwoObjectsFirstAsSimpleCloakedStringContent(object? first, object? second)
    {
        FirstObjectField  = first;
        SecondObjectField = second;
    }

    public object? FirstObjectField { get; set; }
    public object? SecondObjectField { get; set; }

    public PalantírReveal<object> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(FirstObjectField)
           .AsStringMatch(cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal(nameof(FirstObjectField), FirstObjectField, CreateRevealer, ValueFormatString, FormattingFlags)
           .Field.AlwaysAddMatch(nameof(SecondObjectField), SecondObjectField, ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstObjectField)}:{FirstObjectField}, " +
        $"{nameof(SecondObjectField)}:{SecondObjectField})";
}

public class TwoObjectsSecondAsSimpleCloakedStringContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<object> 
{
    private PalantírReveal<object>? cachedRevealer;
    
    public TwoObjectsSecondAsSimpleCloakedStringContent()
    {
        FirstObjectField  = null!;
        SecondObjectField = null!;
    }

    public TwoObjectsSecondAsSimpleCloakedStringContent(object? first, object? second)
    {
        FirstObjectField  = first;
        SecondObjectField = second;
    }

    public object? FirstObjectField { get; set; }
    public object? SecondObjectField { get; set; }

    public PalantírReveal<object> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(SecondObjectField)
           .AsStringMatch(cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddMatch(nameof(FirstObjectField), FirstObjectField, ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal(nameof(SecondObjectField), SecondObjectField, CreateRevealer, ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstObjectField)}:{FirstObjectField}, " +
        $"{nameof(SecondObjectField)}:{SecondObjectField})";
}

public class TwoObjectsFirstAsComplexCloakedValueContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<object> 
{
    private PalantírReveal<object>? cachedRevealer;
    
    public TwoObjectsFirstAsComplexCloakedValueContent()
    {
        FirstObjectField  = null!;
        SecondObjectField = null!;
    }

    public TwoObjectsFirstAsComplexCloakedValueContent(object? first, object? second)
    {
        FirstObjectField  = first;
        SecondObjectField = second;
    }

    public object? FirstObjectField { get; set; }
    public object? SecondObjectField { get; set; }

    private readonly int[] logOnlyArray = [1, 2, 3];

    public PalantírReveal<object> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(FirstObjectField)
           .AsValueMatch("CloakedRevealer" + nameof(FirstObjectField), cloaked, ValueFormatString, FormattingFlags)
           .LogOnlyCollectionField.AlwaysAddAll(nameof(logOnlyArray),logOnlyArray)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal($"CloakedRevealer{nameof(FirstObjectField)}", FirstObjectField
                             , CreateRevealer, ValueFormatString, FormattingFlags)
           .Field.AlwaysAddMatch(nameof(SecondObjectField), SecondObjectField, ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstObjectField)}:{FirstObjectField}, " +
        $"{nameof(SecondObjectField)}:{SecondObjectField})";
}

public class TwoObjectsSecondAsComplexCloakedValueContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<object> 
{
    private PalantírReveal<object>? cachedRevealer;
    
    public TwoObjectsSecondAsComplexCloakedValueContent()
    {
        FirstObjectField  = null!;
        SecondObjectField = null!;
    }

    public TwoObjectsSecondAsComplexCloakedValueContent(object? first, object? second)
    {
        FirstObjectField  = first;
        SecondObjectField = second;
    }

    public object? FirstObjectField { get; set; }
    public object? SecondObjectField { get; set; }

    private readonly Dictionary<string, int> logOnlyMap = new Dictionary<string, int>()
    {
        { "FirstKey", 1 }
      , { "SecondKey", 2 }
      , { "ThirdKey", 3 }
    };

    public PalantírReveal<object> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(SecondObjectField)
           .AsValueMatch($"CloakedRevealer{nameof(SecondObjectField)}", cloaked, ValueFormatString, FormattingFlags)
           .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logOnlyMap), logOnlyMap)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddMatch(nameof(FirstObjectField), FirstObjectField, ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal(nameof(SecondObjectField), SecondObjectField, CreateRevealer, ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstObjectField)}:{FirstObjectField}, " +
        $"{nameof(SecondObjectField)}:{SecondObjectField})";
}

public class TwoObjectsFirstAsComplexCloakedStringContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<object> 
{
    private PalantírReveal<object>? cachedRevealer;
    
    public TwoObjectsFirstAsComplexCloakedStringContent()
    {
        FirstObjectField  = null!;
        SecondObjectField = null!;
    }

    public TwoObjectsFirstAsComplexCloakedStringContent(object? first, object? second)
    {
        FirstObjectField  = first;
        SecondObjectField = second;
    }

    public object? FirstObjectField { get; set; }
    public object? SecondObjectField { get; set; }
    
    private readonly StringBuilder logOnlyStringBuilder = new ("For your eyes only");

    public PalantírReveal<object> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(FirstObjectField)
           .AsStringMatch($"CloakedRevealer{nameof(FirstObjectField)}", cloaked, ValueFormatString, FormattingFlags)
           .LogOnlyField.AlwaysAddMatch(nameof(logOnlyStringBuilder), logOnlyStringBuilder)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal(nameof(FirstObjectField), FirstObjectField, CreateRevealer, ValueFormatString, FormattingFlags)
           .Field.AlwaysAddMatch(nameof(SecondObjectField), SecondObjectField, ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstObjectField)}:{FirstObjectField}, " +
        $"{nameof(SecondObjectField)}:{SecondObjectField})";
}

public class TwoObjectsSecondAsComplexCloakedStringContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<object> 
{
    private PalantírReveal<object>? cachedRevealer;
    
    public TwoObjectsSecondAsComplexCloakedStringContent()
    {
        FirstObjectField  = null!;
        SecondObjectField = null!;
    }

    public TwoObjectsSecondAsComplexCloakedStringContent(object? first, object? second)
    {
        FirstObjectField  = first;
        SecondObjectField = second;
    }

    public object? FirstObjectField { get; set; }
    public object? SecondObjectField { get; set; }


    private readonly List<MutableString> logOnlyList = [new ("FirstCharSeq"), new ("SecondCharSeq"), new ("ThirdCharSeq")];

    public PalantírReveal<object> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(SecondObjectField)
           .AsStringMatch($"CloakedRevealer{nameof(SecondObjectField)}", cloaked, ValueFormatString, FormattingFlags)
           .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(logOnlyList), logOnlyList)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddMatch(nameof(FirstObjectField), FirstObjectField, ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal(nameof(SecondObjectField), SecondObjectField, CreateRevealer, ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstObjectField)}:{FirstObjectField}, " +
        $"{nameof(SecondObjectField)}:{SecondObjectField})";
}
