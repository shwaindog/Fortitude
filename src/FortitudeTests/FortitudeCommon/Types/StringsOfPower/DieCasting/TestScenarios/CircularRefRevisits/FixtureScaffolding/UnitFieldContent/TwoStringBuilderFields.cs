// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.UnitFieldContent;


public class TwoStringBuilderFields: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString,  IPalantirRevealerFactory<StringBuilder>
{
    public TwoStringBuilderFields()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringBuilderFields(StringBuilder? first, StringBuilder? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public StringBuilder? FirstStringField { get; set; }
    public StringBuilder? SecondStringField { get; set; }

    public PalantírReveal<StringBuilder> CreateRevealer => (cloaked, tos) =>
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

public class TwoStringBuildersFirstAsSimpleCloakedValueContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
   , IPalantirRevealerFactory<StringBuilder> 
{
    private PalantírReveal<StringBuilder>? cachedRevealer;
    
    public TwoStringBuildersFirstAsSimpleCloakedValueContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringBuildersFirstAsSimpleCloakedValueContent(StringBuilder? first, StringBuilder? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public StringBuilder? FirstStringField { get; set; }
    public StringBuilder? SecondStringField { get; set; }

    public PalantírReveal<StringBuilder> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
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

public class TwoStringBuildersSecondAsSimpleCloakedValueContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<StringBuilder> 
{
    private PalantírReveal<StringBuilder>? cachedRevealer;
    
    public TwoStringBuildersSecondAsSimpleCloakedValueContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringBuildersSecondAsSimpleCloakedValueContent(StringBuilder? first, StringBuilder? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public StringBuilder? FirstStringField { get; set; }
    public StringBuilder? SecondStringField { get; set; }

    public PalantírReveal<StringBuilder> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
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

public class TwoStringBuildersFirstAsSimpleCloakedStringContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<StringBuilder> 
{
    private PalantírReveal<StringBuilder>? cachedRevealer;
    
    public TwoStringBuildersFirstAsSimpleCloakedStringContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringBuildersFirstAsSimpleCloakedStringContent(StringBuilder? first, StringBuilder? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public StringBuilder? FirstStringField { get; set; }
    public StringBuilder? SecondStringField { get; set; }

    public PalantírReveal<StringBuilder> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
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

public class TwoStringBuildersSecondAsSimpleCloakedStringContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<StringBuilder> 
{
    private PalantírReveal<StringBuilder>? cachedRevealer;
    
    public TwoStringBuildersSecondAsSimpleCloakedStringContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringBuildersSecondAsSimpleCloakedStringContent(StringBuilder? first, StringBuilder? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public StringBuilder? FirstStringField { get; set; }
    public StringBuilder? SecondStringField { get; set; }

    public PalantírReveal<StringBuilder> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
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

public class TwoStringBuildersFirstAsComplexCloakedValueContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<StringBuilder> 
{
    private PalantírReveal<StringBuilder>? cachedRevealer;
    
    public TwoStringBuildersFirstAsComplexCloakedValueContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringBuildersFirstAsComplexCloakedValueContent(StringBuilder? first, StringBuilder? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public StringBuilder? FirstStringField { get; set; }
    public StringBuilder? SecondStringField { get; set; }

    public PalantírReveal<StringBuilder> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(FirstStringField)
           .AsValue (nameof(FirstStringField), cloaked, ValueFormatString, FormattingFlags)
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

public class TwoStringBuildersSecondAsComplexCloakedValueContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<StringBuilder> 
{
    private PalantírReveal<StringBuilder>? cachedRevealer;
    
    public TwoStringBuildersSecondAsComplexCloakedValueContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringBuildersSecondAsComplexCloakedValueContent(StringBuilder? first, StringBuilder? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public StringBuilder? FirstStringField { get; set; }
    public StringBuilder? SecondStringField { get; set; }

    public PalantírReveal<StringBuilder> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(SecondStringField)
           .AsValue (nameof(SecondStringField), cloaked, ValueFormatString, FormattingFlags)
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

public class TwoStringBuildersFirstAsComplexCloakedStringContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<StringBuilder> 
{
    private PalantírReveal<StringBuilder>? cachedRevealer;
    
    public TwoStringBuildersFirstAsComplexCloakedStringContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringBuildersFirstAsComplexCloakedStringContent(StringBuilder? first, StringBuilder? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public StringBuilder? FirstStringField { get; set; }
    public StringBuilder? SecondStringField { get; set; }

    public PalantírReveal<StringBuilder> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(FirstStringField)
           .AsString (nameof(FirstStringField), cloaked, ValueFormatString, FormattingFlags)
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

public class TwoStringBuildersSecondAsComplexCloakedStringContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<StringBuilder> 
{
    private PalantírReveal<StringBuilder>? cachedRevealer;
    
    public TwoStringBuildersSecondAsComplexCloakedStringContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringBuildersSecondAsComplexCloakedStringContent(StringBuilder? first, StringBuilder? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public StringBuilder? FirstStringField { get; set; }
    public StringBuilder? SecondStringField { get; set; }

    public PalantírReveal<StringBuilder> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(SecondStringField)
           .AsString (nameof(SecondStringField), cloaked, ValueFormatString, FormattingFlags)
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