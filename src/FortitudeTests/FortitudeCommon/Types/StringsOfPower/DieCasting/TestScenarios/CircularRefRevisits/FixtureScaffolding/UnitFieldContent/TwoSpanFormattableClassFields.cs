// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.UnitFieldContent;

public class TwoSpanFormattableFields<TFmt>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString,  IPalantirRevealerFactory<TFmt> 
    where TFmt : class, ISpanFormattable
{
    public TwoSpanFormattableFields()
    {
        FirstSpanFormattableField  = null!;
        SecondSpanFormattableField = null!;
    }

    public TwoSpanFormattableFields(TFmt? first, TFmt? second)
    {
        FirstSpanFormattableField  = first;
        SecondSpanFormattableField = second;
    }

    public TFmt? FirstSpanFormattableField { get; set; }
    public TFmt? SecondSpanFormattableField { get; set; }

    public PalantírReveal<TFmt> CreateRevealer => (cloaked, tos) =>
        tos.StartComplexType(cloaked)
           .Field.AlwaysAdd
               ($"CloakedRevealer{nameof(SecondSpanFormattableField)}"
              , cloaked, ValueFormatString)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstSpanFormattableField)
              , FirstSpanFormattableField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondSpanFormattableField)
              , SecondSpanFormattableField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstSpanFormattableField)}:{FirstSpanFormattableField}, " +
        $"{nameof(SecondSpanFormattableField)}:{SecondSpanFormattableField})";
}

public class TwoSpanFormattableFirstAsSimpleCloakedValueContent<TFmt>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
   , IPalantirRevealerFactory<TFmt> 
    where TFmt : class, ISpanFormattable
{
    private PalantírReveal<TFmt>? cachedRevealer;
    
    public TwoSpanFormattableFirstAsSimpleCloakedValueContent()
    {
        FirstSpanFormattableField  = null!;
        SecondSpanFormattableField = null!;
    }

    public TwoSpanFormattableFirstAsSimpleCloakedValueContent(TFmt? first, TFmt? second)
    {
        FirstSpanFormattableField  = first;
        SecondSpanFormattableField = second;
    }

    public TFmt? FirstSpanFormattableField { get; set; }
    public TFmt? SecondSpanFormattableField { get; set; }

    public PalantírReveal<TFmt> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(cloaked)
           .AsValue (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstSpanFormattableField)
              , FirstSpanFormattableField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondSpanFormattableField)
              , SecondSpanFormattableField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstSpanFormattableField)}:{FirstSpanFormattableField}, " +
        $"{nameof(SecondSpanFormattableField)}:{SecondSpanFormattableField})";
}

public class TwoSpanFormattableSecondAsSimpleCloakedValueContent<TFmt>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TFmt> 
    where TFmt : class, ISpanFormattable
{
    private PalantírReveal<TFmt>? cachedRevealer;
    
    public TwoSpanFormattableSecondAsSimpleCloakedValueContent()
    {
        FirstSpanFormattableField  = null!;
        SecondSpanFormattableField = null!;
    }

    public TwoSpanFormattableSecondAsSimpleCloakedValueContent(TFmt? first, TFmt? second)
    {
        FirstSpanFormattableField  = first;
        SecondSpanFormattableField = second;
    }

    public TFmt? FirstSpanFormattableField { get; set; }
    public TFmt? SecondSpanFormattableField { get; set; }

    public PalantírReveal<TFmt> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(cloaked)
           .AsValue (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstSpanFormattableField)
              , FirstSpanFormattableField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondSpanFormattableField)
              , SecondSpanFormattableField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstSpanFormattableField)}:{FirstSpanFormattableField}, " +
        $"{nameof(SecondSpanFormattableField)}:{SecondSpanFormattableField})";
}

public class TwoSpanFormattableFirstAsSimpleCloakedStringContent<TFmt>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TFmt> 
    where TFmt : class, ISpanFormattable
{
    private PalantírReveal<TFmt>? cachedRevealer;
    
    public TwoSpanFormattableFirstAsSimpleCloakedStringContent()
    {
        FirstSpanFormattableField  = null!;
        SecondSpanFormattableField = null!;
    }

    public TwoSpanFormattableFirstAsSimpleCloakedStringContent(TFmt? first, TFmt? second)
    {
        FirstSpanFormattableField  = first;
        SecondSpanFormattableField = second;
    }

    public TFmt? FirstSpanFormattableField { get; set; }
    public TFmt? SecondSpanFormattableField { get; set; }

    public PalantírReveal<TFmt> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(cloaked)
           .AsString (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstSpanFormattableField)
              , FirstSpanFormattableField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondSpanFormattableField)
              , SecondSpanFormattableField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstSpanFormattableField)}:{FirstSpanFormattableField}, " +
        $"{nameof(SecondSpanFormattableField)}:{SecondSpanFormattableField})";
}

public class TwoSpanFormattableSecondAsSimpleCloakedStringContent<TFmt>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TFmt> 
    where TFmt : class, ISpanFormattable
{
    private PalantírReveal<TFmt>? cachedRevealer;
    
    public TwoSpanFormattableSecondAsSimpleCloakedStringContent()
    {
        FirstSpanFormattableField  = null!;
        SecondSpanFormattableField = null!;
    }

    public TwoSpanFormattableSecondAsSimpleCloakedStringContent(TFmt? first, TFmt? second)
    {
        FirstSpanFormattableField  = first;
        SecondSpanFormattableField = second;
    }

    public TFmt? FirstSpanFormattableField { get; set; }
    public TFmt? SecondSpanFormattableField { get; set; }

    public PalantírReveal<TFmt> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(cloaked)
           .AsString (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstSpanFormattableField)
              , FirstSpanFormattableField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondSpanFormattableField)
              , SecondSpanFormattableField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstSpanFormattableField)}:{FirstSpanFormattableField}, " +
        $"{nameof(SecondSpanFormattableField)}:{SecondSpanFormattableField})";
}

public class TwoSpanFormattableFirstAsComplexCloakedValueContent<TFmt>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TFmt> 
    where TFmt : class, ISpanFormattable
{
    private PalantírReveal<TFmt>? cachedRevealer;
    
    public TwoSpanFormattableFirstAsComplexCloakedValueContent()
    {
        FirstSpanFormattableField  = null!;
        SecondSpanFormattableField = null!;
    }

    public TwoSpanFormattableFirstAsComplexCloakedValueContent(TFmt? first, TFmt? second)
    {
        FirstSpanFormattableField  = first;
        SecondSpanFormattableField = second;
    }

    public TFmt? FirstSpanFormattableField { get; set; }
    public TFmt? SecondSpanFormattableField { get; set; }

    private readonly int[] logOnlyArray = [1, 2, 3];
    
    public PalantírReveal<TFmt> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(cloaked)
           .AsValue ($"CloakedRevealer{nameof(FirstSpanFormattableField)}", cloaked, ValueFormatString, FormattingFlags)
           .LogOnlyCollectionField.AlwaysAddAll(nameof(logOnlyArray),logOnlyArray)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstSpanFormattableField)
              , FirstSpanFormattableField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondSpanFormattableField)
              , SecondSpanFormattableField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstSpanFormattableField)}:{FirstSpanFormattableField}, " +
        $"{nameof(SecondSpanFormattableField)}:{SecondSpanFormattableField})";
}

public class TwoSpanFormattableSecondAsComplexCloakedValueContent<TFmt>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TFmt> 
    where TFmt : class, ISpanFormattable
{
    private PalantírReveal<TFmt>? cachedRevealer;
    
    public TwoSpanFormattableSecondAsComplexCloakedValueContent()
    {
        FirstSpanFormattableField  = null!;
        SecondSpanFormattableField = null!;
    }

    public TwoSpanFormattableSecondAsComplexCloakedValueContent(TFmt? first, TFmt? second)
    {
        FirstSpanFormattableField  = first;
        SecondSpanFormattableField = second;
    }

    public TFmt? FirstSpanFormattableField { get; set; }
    public TFmt? SecondSpanFormattableField { get; set; }

    private readonly Dictionary<string, int> logOnlyMap = new Dictionary<string, int>()
    {
        { "FirstKey", 1 }
       , { "SecondKey", 2 }
       , { "ThirdKey", 3 }
    };

    public PalantírReveal<TFmt> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(cloaked)
           .AsValue ($"CloakedRevealer{nameof(SecondSpanFormattableField)}" , cloaked, ValueFormatString, FormattingFlags)
           .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logOnlyMap), logOnlyMap)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstSpanFormattableField)
              , FirstSpanFormattableField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondSpanFormattableField)
              , SecondSpanFormattableField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstSpanFormattableField)}:{FirstSpanFormattableField}, " +
        $"{nameof(SecondSpanFormattableField)}:{SecondSpanFormattableField})";
}

public class TwoSpanFormattableFirstAsComplexCloakedStringContent<TFmt>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TFmt> 
    where TFmt : class, ISpanFormattable
{
    private PalantírReveal<TFmt>? cachedRevealer;
    
    public TwoSpanFormattableFirstAsComplexCloakedStringContent()
    {
        FirstSpanFormattableField  = null!;
        SecondSpanFormattableField = null!;
    }

    public TwoSpanFormattableFirstAsComplexCloakedStringContent(TFmt? first, TFmt? second)
    {
        FirstSpanFormattableField  = first;
        SecondSpanFormattableField = second;
    }

    public TFmt? FirstSpanFormattableField { get; set; }
    public TFmt? SecondSpanFormattableField { get; set; }
    
    private readonly StringBuilder logOnlyStringBuilder = new ("For your eyes only");

    public PalantírReveal<TFmt> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(cloaked)
           .AsString ($"CloakedRevealer{nameof(FirstSpanFormattableField)}", cloaked, ValueFormatString, FormattingFlags)
           .LogOnlyField.AlwaysAdd(nameof(logOnlyStringBuilder), logOnlyStringBuilder)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstSpanFormattableField)
              , FirstSpanFormattableField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondSpanFormattableField)
              , SecondSpanFormattableField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstSpanFormattableField)}:{FirstSpanFormattableField}, " +
        $"{nameof(SecondSpanFormattableField)}:{SecondSpanFormattableField})";
}

public class TwoSpanFormattableSecondAsComplexCloakedStringContent<TFmt>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TFmt> 
    where TFmt : class, ISpanFormattable
{
    private PalantírReveal<TFmt>? cachedRevealer;
    
    public TwoSpanFormattableSecondAsComplexCloakedStringContent()
    {
        FirstSpanFormattableField  = null!;
        SecondSpanFormattableField = null!;
    }

    public TwoSpanFormattableSecondAsComplexCloakedStringContent(TFmt? first, TFmt? second)
    {
        FirstSpanFormattableField  = first;
        SecondSpanFormattableField = second;
    }

    public TFmt? FirstSpanFormattableField { get; set; }
    public TFmt? SecondSpanFormattableField { get; set; }


    private readonly List<MutableString> logOnlyList = [new ("FirstCharSeq"), new ("SecondCharSeq"), new ("ThirdCharSeq")];
    
    public PalantírReveal<TFmt> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(cloaked)
           .AsString ($"CloakedRevealer{nameof(SecondSpanFormattableField)}", cloaked, ValueFormatString, FormattingFlags)
           .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(logOnlyList), logOnlyList)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstSpanFormattableField)
              , FirstSpanFormattableField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondSpanFormattableField)
              , SecondSpanFormattableField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstSpanFormattableField)}:{FirstSpanFormattableField}, " +
        $"{nameof(SecondSpanFormattableField)}:{SecondSpanFormattableField})";
}
