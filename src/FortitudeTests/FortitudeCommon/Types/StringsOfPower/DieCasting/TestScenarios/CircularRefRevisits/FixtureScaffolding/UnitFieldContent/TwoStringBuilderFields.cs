// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.UnitFieldContent;


public class TwoStringBuilderFields: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
{
    public TwoStringBuilderFields()
    {
        FirstStringBuilderField  = null!;
        SecondStringBuilderField = null!;
    }

    public TwoStringBuilderFields(StringBuilder? first, StringBuilder? second)
    {
        FirstStringBuilderField  = first;
        SecondStringBuilderField = second;
    }

    public StringBuilder? FirstStringBuilderField { get; set; }
    public StringBuilder? SecondStringBuilderField { get; set; }

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstStringBuilderField)
              , FirstStringBuilderField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondStringBuilderField)
              , SecondStringBuilderField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringBuilderField)}:{FirstStringBuilderField}, " +
        $"{nameof(SecondStringBuilderField)}:{SecondStringBuilderField})";
}

public class TwoStringBuildersFirstAsSimpleCloakedValueContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
   , IPalantirRevealerFactory<StringBuilder> 
{
    private PalantírReveal<StringBuilder>? cachedRevealer;
    
    public TwoStringBuildersFirstAsSimpleCloakedValueContent()
    {
        FirstStringBuilderField  = null!;
        SecondStringBuilderField = null!;
    }

    public TwoStringBuildersFirstAsSimpleCloakedValueContent(StringBuilder? first, StringBuilder? second)
    {
        FirstStringBuilderField  = first;
        SecondStringBuilderField = second;
    }

    public StringBuilder? FirstStringBuilderField { get; set; }
    public StringBuilder? SecondStringBuilderField { get; set; }

    public PalantírReveal<StringBuilder> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(FirstStringBuilderField)
           .AsValue (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstStringBuilderField)
              , FirstStringBuilderField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondStringBuilderField)
              , SecondStringBuilderField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringBuilderField)}:{FirstStringBuilderField}, " +
        $"{nameof(SecondStringBuilderField)}:{SecondStringBuilderField})";
}

public class TwoStringBuildersSecondAsSimpleCloakedValueContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<StringBuilder> 
{
    private PalantírReveal<StringBuilder>? cachedRevealer;
    
    public TwoStringBuildersSecondAsSimpleCloakedValueContent()
    {
        FirstStringBuilderField  = null!;
        SecondStringBuilderField = null!;
    }

    public TwoStringBuildersSecondAsSimpleCloakedValueContent(StringBuilder? first, StringBuilder? second)
    {
        FirstStringBuilderField  = first;
        SecondStringBuilderField = second;
    }

    public StringBuilder? FirstStringBuilderField { get; set; }
    public StringBuilder? SecondStringBuilderField { get; set; }

    public PalantírReveal<StringBuilder> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(SecondStringBuilderField)
           .AsValue (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstStringBuilderField)
              , FirstStringBuilderField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondStringBuilderField)
              , SecondStringBuilderField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringBuilderField)}:{FirstStringBuilderField}, " +
        $"{nameof(SecondStringBuilderField)}:{SecondStringBuilderField})";
}

public class TwoStringBuildersFirstAsSimpleCloakedStringContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<StringBuilder> 
{
    private PalantírReveal<StringBuilder>? cachedRevealer;
    
    public TwoStringBuildersFirstAsSimpleCloakedStringContent()
    {
        FirstStringBuilderField  = null!;
        SecondStringBuilderField = null!;
    }

    public TwoStringBuildersFirstAsSimpleCloakedStringContent(StringBuilder? first, StringBuilder? second)
    {
        FirstStringBuilderField  = first;
        SecondStringBuilderField = second;
    }

    public StringBuilder? FirstStringBuilderField { get; set; }
    public StringBuilder? SecondStringBuilderField { get; set; }

    public PalantírReveal<StringBuilder> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(FirstStringBuilderField)
           .AsString (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstStringBuilderField)
              , FirstStringBuilderField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondStringBuilderField)
              , SecondStringBuilderField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringBuilderField)}:{FirstStringBuilderField}, " +
        $"{nameof(SecondStringBuilderField)}:{SecondStringBuilderField})";
}

public class TwoStringBuildersSecondAsSimpleCloakedStringContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<StringBuilder> 
{
    private PalantírReveal<StringBuilder>? cachedRevealer;
    
    public TwoStringBuildersSecondAsSimpleCloakedStringContent()
    {
        FirstStringBuilderField  = null!;
        SecondStringBuilderField = null!;
    }

    public TwoStringBuildersSecondAsSimpleCloakedStringContent(StringBuilder? first, StringBuilder? second)
    {
        FirstStringBuilderField  = first;
        SecondStringBuilderField = second;
    }

    public StringBuilder? FirstStringBuilderField { get; set; }
    public StringBuilder? SecondStringBuilderField { get; set; }

    public PalantírReveal<StringBuilder> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(SecondStringBuilderField)
           .AsString (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstStringBuilderField)
              , FirstStringBuilderField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondStringBuilderField)
              , SecondStringBuilderField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringBuilderField)}:{FirstStringBuilderField}, " +
        $"{nameof(SecondStringBuilderField)}:{SecondStringBuilderField})";
}

public class TwoStringBuildersFirstAsComplexCloakedValueContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<StringBuilder> 
{
    private PalantírReveal<StringBuilder>? cachedRevealer;
    
    public TwoStringBuildersFirstAsComplexCloakedValueContent()
    {
        FirstStringBuilderField  = null!;
        SecondStringBuilderField = null!;
    }

    public TwoStringBuildersFirstAsComplexCloakedValueContent(StringBuilder? first, StringBuilder? second)
    {
        FirstStringBuilderField  = first;
        SecondStringBuilderField = second;
    }

    public StringBuilder? FirstStringBuilderField { get; set; }
    public StringBuilder? SecondStringBuilderField { get; set; }

    private readonly int[] logOnlyArray = [1, 2, 3];

    public PalantírReveal<StringBuilder> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(FirstStringBuilderField)
           .AsValue ($"CloakedRevealer{nameof(FirstStringBuilderField)}", cloaked, ValueFormatString, FormattingFlags)
           .LogOnlyCollectionField.AlwaysAddAll(nameof(logOnlyArray),logOnlyArray)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstStringBuilderField)
              , FirstStringBuilderField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondStringBuilderField)
              , SecondStringBuilderField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringBuilderField)}:{FirstStringBuilderField}, " +
        $"{nameof(SecondStringBuilderField)}:{SecondStringBuilderField})";
}

public class TwoStringBuildersSecondAsComplexCloakedValueContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<StringBuilder> 
{
    private PalantírReveal<StringBuilder>? cachedRevealer;
    
    public TwoStringBuildersSecondAsComplexCloakedValueContent()
    {
        FirstStringBuilderField  = null!;
        SecondStringBuilderField = null!;
    }

    public TwoStringBuildersSecondAsComplexCloakedValueContent(StringBuilder? first, StringBuilder? second)
    {
        FirstStringBuilderField  = first;
        SecondStringBuilderField = second;
    }

    public StringBuilder? FirstStringBuilderField { get; set; }
    public StringBuilder? SecondStringBuilderField { get; set; }

    private readonly Dictionary<string, int> logOnlyMap = new Dictionary<string, int>()
    {
        { "FirstKey", 1 }
      , { "SecondKey", 2 }
      , { "ThirdKey", 3 }
    };

    public PalantírReveal<StringBuilder> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(SecondStringBuilderField)
           .AsValue ($"CloakedRevealer{nameof(SecondStringBuilderField)}", cloaked, ValueFormatString, FormattingFlags)
           .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logOnlyMap), logOnlyMap)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstStringBuilderField)
              , FirstStringBuilderField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondStringBuilderField)
              , SecondStringBuilderField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringBuilderField)}:{FirstStringBuilderField}, " +
        $"{nameof(SecondStringBuilderField)}:{SecondStringBuilderField})";
}

public class TwoStringBuildersFirstAsComplexCloakedStringContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<StringBuilder> 
{
    private PalantírReveal<StringBuilder>? cachedRevealer;
    
    public TwoStringBuildersFirstAsComplexCloakedStringContent()
    {
        FirstStringBuilderField  = null!;
        SecondStringBuilderField = null!;
    }

    public TwoStringBuildersFirstAsComplexCloakedStringContent(StringBuilder? first, StringBuilder? second)
    {
        FirstStringBuilderField  = first;
        SecondStringBuilderField = second;
    }

    public StringBuilder? FirstStringBuilderField { get; set; }
    public StringBuilder? SecondStringBuilderField { get; set; }
    
    private readonly StringBuilder logOnlyStringBuilder = new ("For your eyes only");

    public PalantírReveal<StringBuilder> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(FirstStringBuilderField)
           .AsString ($"CloakedRevealer{nameof(FirstStringBuilderField)}", cloaked, ValueFormatString, FormattingFlags)
           .LogOnlyField.AlwaysAdd(nameof(logOnlyStringBuilder), logOnlyStringBuilder)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstStringBuilderField)
              , FirstStringBuilderField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondStringBuilderField)
              , SecondStringBuilderField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringBuilderField)}:{FirstStringBuilderField}, " +
        $"{nameof(SecondStringBuilderField)}:{SecondStringBuilderField})";
}

public class TwoStringBuildersSecondAsComplexCloakedStringContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<StringBuilder> 
{
    private PalantírReveal<StringBuilder>? cachedRevealer;
    
    public TwoStringBuildersSecondAsComplexCloakedStringContent()
    {
        FirstStringBuilderField  = null!;
        SecondStringBuilderField = null!;
    }

    public TwoStringBuildersSecondAsComplexCloakedStringContent(StringBuilder? first, StringBuilder? second)
    {
        FirstStringBuilderField  = first;
        SecondStringBuilderField = second;
    }

    public StringBuilder? FirstStringBuilderField { get; set; }
    public StringBuilder? SecondStringBuilderField { get; set; }


    private readonly List<MutableString> logOnlyList = [new ("FirstCharSeq"), new ("SecondCharSeq"), new ("ThirdCharSeq")];

    public PalantírReveal<StringBuilder> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(SecondStringBuilderField)
           .AsString ($"CloakedRevealer{nameof(SecondStringBuilderField)}", cloaked, ValueFormatString, FormattingFlags)
           .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(logOnlyList), logOnlyList)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstStringBuilderField)
              , FirstStringBuilderField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondStringBuilderField)
              , SecondStringBuilderField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringBuilderField)}:{FirstStringBuilderField}, " +
        $"{nameof(SecondStringBuilderField)}:{SecondStringBuilderField})";
}