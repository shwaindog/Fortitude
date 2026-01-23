// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.UnitFieldContent;

public class TwoStringBearersFields<TBearer>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TBearer> 
    where TBearer : class, IStringBearer
{
    public TwoStringBearersFields()
    {
        FirstStringBearerField  = null!;
        SecondStringBearerField = null!;
    }

    public TwoStringBearersFields(TBearer? first, TBearer? second)
    {
        FirstStringBearerField  = first;
        SecondStringBearerField = second;
    }

    public TBearer? FirstStringBearerField { get; set; }
    public TBearer? SecondStringBearerField { get; set; }

    public PalantírReveal<TBearer> CreateRevealer => (cloaked, tos) =>
        tos.StartComplexType(cloaked)
           .Field.AlwaysReveal
               ($"CloakedRevealer{nameof(SecondStringBearerField)}"
              , cloaked, ValueFormatString)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstStringBearerField)
              , FirstStringBearerField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondStringBearerField)
              , SecondStringBearerField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringBearerField)}:{FirstStringBearerField}, " +
        $"{nameof(SecondStringBearerField)}:{SecondStringBearerField})";
}

public class TwoStringBearersFirstAsSimpleCloakedValueContent<TBearer>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TBearer> 
    where TBearer : class, IStringBearer
{
    private PalantírReveal<TBearer>? cachedRevealer;
    
    public TwoStringBearersFirstAsSimpleCloakedValueContent()
    {
        FirstStringBearerField  = null!;
        SecondStringBearerField = null!;
    }

    public TwoStringBearersFirstAsSimpleCloakedValueContent(TBearer? first, TBearer? second)
    {
        FirstStringBearerField  = first;
        SecondStringBearerField = second;
    }

    public TBearer? FirstStringBearerField { get; set; }
    public TBearer? SecondStringBearerField { get; set; }

    public PalantírReveal<TBearer> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(cloaked)
           .RevealAsValue (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstStringBearerField)
              , FirstStringBearerField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondStringBearerField)
              , SecondStringBearerField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringBearerField)}:{FirstStringBearerField}, " +
        $"{nameof(SecondStringBearerField)}:{SecondStringBearerField})";
}

public class TwoStringBearersSecondAsSimpleCloakedValueContent<TBearer>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TBearer> 
    where TBearer : class, IStringBearer
{
    private PalantírReveal<TBearer>? cachedRevealer;
    
    public TwoStringBearersSecondAsSimpleCloakedValueContent()
    {
        FirstStringBearerField  = null!;
        SecondStringBearerField = null!;
    }

    public TwoStringBearersSecondAsSimpleCloakedValueContent(TBearer? first, TBearer? second)
    {
        FirstStringBearerField  = first;
        SecondStringBearerField = second;
    }

    public TBearer? FirstStringBearerField { get; set; }
    public TBearer? SecondStringBearerField { get; set; }

    public PalantírReveal<TBearer> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(cloaked)
           .RevealAsValue (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstStringBearerField)
              , FirstStringBearerField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondStringBearerField)
              , SecondStringBearerField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringBearerField)}:{FirstStringBearerField}, " +
        $"{nameof(SecondStringBearerField)}:{SecondStringBearerField})";
}

public class TwoStringBearersFirstAsSimpleCloakedStringContent<TBearer>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TBearer> 
    where TBearer : class, IStringBearer
{
    private PalantírReveal<TBearer>? cachedRevealer;
    
    public TwoStringBearersFirstAsSimpleCloakedStringContent()
    {
        FirstStringBearerField  = null!;
        SecondStringBearerField = null!;
    }

    public TwoStringBearersFirstAsSimpleCloakedStringContent(TBearer? first, TBearer? second)
    {
        FirstStringBearerField  = first;
        SecondStringBearerField = second;
    }

    public TBearer? FirstStringBearerField { get; set; }
    public TBearer? SecondStringBearerField { get; set; }

    public PalantírReveal<TBearer> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(cloaked)
           .RevealAsString (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstStringBearerField)
              , FirstStringBearerField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondStringBearerField)
              , SecondStringBearerField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringBearerField)}:{FirstStringBearerField}, " +
        $"{nameof(SecondStringBearerField)}:{SecondStringBearerField})";
}

public class TwoStringBearersSecondAsSimpleCloakedStringContent<TBearer>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TBearer> 
    where TBearer : class, IStringBearer
{
    private PalantírReveal<TBearer>? cachedRevealer;
    
    public TwoStringBearersSecondAsSimpleCloakedStringContent()
    {
        FirstStringBearerField  = null!;
        SecondStringBearerField = null!;
    }

    public TwoStringBearersSecondAsSimpleCloakedStringContent(TBearer? first, TBearer? second)
    {
        FirstStringBearerField  = first;
        SecondStringBearerField = second;
    }

    public TBearer? FirstStringBearerField { get; set; }
    public TBearer? SecondStringBearerField { get; set; }

    public PalantírReveal<TBearer> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(cloaked)
           .RevealAsValue (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstStringBearerField)
              , FirstStringBearerField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondStringBearerField)
              , SecondStringBearerField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringBearerField)}:{FirstStringBearerField}, " +
        $"{nameof(SecondStringBearerField)}:{SecondStringBearerField})";
}

public class TwoStringBearersFirstAsComplexCloakedValueContent<TBearer>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TBearer> 
    where TBearer : class, IStringBearer
{
    private PalantírReveal<TBearer>? cachedRevealer;
    
    public TwoStringBearersFirstAsComplexCloakedValueContent()
    {
        FirstStringBearerField  = null!;
        SecondStringBearerField = null!;
    }

    public TwoStringBearersFirstAsComplexCloakedValueContent(TBearer? first, TBearer? second)
    {
        FirstStringBearerField  = first;
        SecondStringBearerField = second;
    }

    public TBearer? FirstStringBearerField { get; set; }
    public TBearer? SecondStringBearerField { get; set; }

    private readonly Dictionary<StringBuilder, bool?> logOnlyMap = new ()
    {
        { new StringBuilder("FirstKey"), false }
      , { new StringBuilder("SecondKey"), null }
      , { new StringBuilder("ThirdKey"), true }
    };

    public PalantírReveal<TBearer> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(cloaked)
           .RevealAsValue ($"CloakedRevealer{nameof(FirstStringBearerField)}", cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstStringBearerField)
              , FirstStringBearerField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondStringBearerField)
              , SecondStringBearerField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringBearerField)}:{FirstStringBearerField}, " +
        $"{nameof(SecondStringBearerField)}:{SecondStringBearerField})";
}

public class TwoStringBearersSecondAsComplexCloakedValueContent<TBearer>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TBearer> 
    where TBearer : class, IStringBearer
{
    private PalantírReveal<TBearer>? cachedRevealer;
    
    public TwoStringBearersSecondAsComplexCloakedValueContent()
    {
        FirstStringBearerField  = null!;
        SecondStringBearerField = null!;
    }

    public TwoStringBearersSecondAsComplexCloakedValueContent(TBearer? first, TBearer? second)
    {
        FirstStringBearerField  = first;
        SecondStringBearerField = second;
    }

    public TBearer? FirstStringBearerField { get; set; }
    public TBearer? SecondStringBearerField { get; set; }

    public PalantírReveal<TBearer> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(cloaked)
           .RevealAsValue ($"CloakedRevealer{nameof(SecondStringBearerField)}", cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstStringBearerField)
              , FirstStringBearerField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondStringBearerField)
              , SecondStringBearerField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringBearerField)}:{FirstStringBearerField}, " +
        $"{nameof(SecondStringBearerField)}:{SecondStringBearerField})";
}

public class TwoStringBearersFirstAsComplexCloakedStringContent<TBearer>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TBearer> 
    where TBearer : class, IStringBearer
{
    private PalantírReveal<TBearer>? cachedRevealer;
    
    public TwoStringBearersFirstAsComplexCloakedStringContent()
    {
        FirstStringBearerField  = null!;
        SecondStringBearerField = null!;
    }

    public TwoStringBearersFirstAsComplexCloakedStringContent(TBearer? first, TBearer? second)
    {
        FirstStringBearerField  = first;
        SecondStringBearerField = second;
    }

    public TBearer? FirstStringBearerField { get; set; }
    public TBearer? SecondStringBearerField { get; set; }

    public PalantírReveal<TBearer> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(cloaked)
           .RevealAsString ($"CloakedRevealer{nameof(FirstStringBearerField)}", cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstStringBearerField)
              , FirstStringBearerField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondStringBearerField)
              , SecondStringBearerField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringBearerField)}:{FirstStringBearerField}, " +
        $"{nameof(SecondStringBearerField)}:{SecondStringBearerField})";
}

public class TwoStringBearersSecondAsComplexCloakedStringContent<TBearer>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TBearer> 
    where TBearer : class, IStringBearer
{
    private PalantírReveal<TBearer>? cachedRevealer;
    
    public TwoStringBearersSecondAsComplexCloakedStringContent()
    {
        FirstStringBearerField  = null!;
        SecondStringBearerField = null!;
    }

    public TwoStringBearersSecondAsComplexCloakedStringContent(TBearer? first, TBearer? second)
    {
        FirstStringBearerField  = first;
        SecondStringBearerField = second;
    }

    public TBearer? FirstStringBearerField { get; set; }
    public TBearer? SecondStringBearerField { get; set; }

    public PalantírReveal<TBearer> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(cloaked)
           .RevealAsString ($"CloakedRevealer{nameof(SecondStringBearerField)}", cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstStringBearerField)
              , FirstStringBearerField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondStringBearerField)
              , SecondStringBearerField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringBearerField)}:{FirstStringBearerField}, " +
        $"{nameof(SecondStringBearerField)}:{SecondStringBearerField})";
}
