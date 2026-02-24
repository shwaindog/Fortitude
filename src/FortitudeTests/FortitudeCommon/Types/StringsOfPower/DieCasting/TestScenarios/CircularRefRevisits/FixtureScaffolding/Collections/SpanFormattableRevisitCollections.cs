// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.Collections;

public class PreFieldDoubleArrayStructUnionRevisit : IStringBearer
{
    private readonly double firstPreField = Math.E;

    public PreFieldDoubleArrayStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<SpanFormattableOrArrayStructUnion<double, double>>? nodeRevealer = null)
    {
        var repeatedItem = new SpanFormattableOrArrayStructUnion<double, double>([1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<SpanFormattableOrArrayStructUnion<double, double>> firstAsList = [
            new (Math.PI, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly SpanFormattableOrArrayStructUnion<double, double>[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Complete();
    }
}

public class DoubleArrayPostFieldStructUnionRevisit : IStringBearer
{
    private readonly double firstPostField = Math.PI;

    public DoubleArrayPostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<SpanFormattableOrArrayStructUnion<double, double>>? nodeRevealer = null)
    {
        var repeatedItem = new SpanFormattableOrArrayStructUnion<double, double>([1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<SpanFormattableOrArrayStructUnion<double, double>> firstAsList = [
            new (Math.E, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly SpanFormattableOrArrayStructUnion<double, double>[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class NullablePreFieldNullableDoubleArrayStructUnionRevisit : IStringBearer
{
    private readonly double? firstPreField = Math.E;

    public NullablePreFieldNullableDoubleArrayStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<NullableStructSpanFormattableOrArrayStructUnion<double>>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructSpanFormattableOrArrayStructUnion<double>([null, 1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<NullableStructSpanFormattableOrArrayStructUnion<double>> firstAsList = [
            new (Math.PI, isSimple, isValue, boolRevealer)
          , new ((double?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, null, 5.0, 6.0], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly NullableStructSpanFormattableOrArrayStructUnion<double>[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Complete();
    }
}

public class NullableDoubleArrayNullablePostFieldStructUnionRevisit : IStringBearer
{
    private readonly double? firstPostField = Math.PI;

    public NullableDoubleArrayNullablePostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<NullableStructSpanFormattableOrArrayStructUnion<double>>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructSpanFormattableOrArrayStructUnion<double>([1.0, 2.0, null, 3.0], isSimple, isValue, boolRevealer);
        List<NullableStructSpanFormattableOrArrayStructUnion<double>> firstAsList = [
            new (Math.E, isSimple, isValue, boolRevealer)
          , new ((double?[]?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0, null], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly NullableStructSpanFormattableOrArrayStructUnion<double>[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldDoubleArrayClassUnionRevisit : IStringBearer
{
    private readonly double firstPreField = Math.E;

    public PreFieldDoubleArrayClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<SpanFormattableOrArrayClassUnion<double, double>>? nodeRevealer = null)
    {
        var repeatedItem = new SpanFormattableOrArrayClassUnion<double, double>([1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<SpanFormattableOrArrayClassUnion<double, double>> firstAsList = [
            new (Math.PI, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly SpanFormattableOrArrayClassUnion<double, double>[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Complete();
    }
}

public class DoubleArrayPostFieldClassUnionRevisit : IStringBearer
{
    private readonly double firstPostField = Math.PI;

    public DoubleArrayPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<SpanFormattableOrArrayClassUnion<double, double>>? nodeRevealer = null)
    {
        var repeatedItem = new SpanFormattableOrArrayClassUnion<double, double>([1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<SpanFormattableOrArrayClassUnion<double, double>> firstAsList = [
            new (Math.E, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly SpanFormattableOrArrayClassUnion<double, double>[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class NullablePreFieldNullableDoubleArrayClassUnionRevisit : IStringBearer
{
    private readonly double? firstPreField = null;

    public NullablePreFieldNullableDoubleArrayClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<NullableStructSpanFormattableOrArrayClassUnion<double>>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructSpanFormattableOrArrayClassUnion<double>([null, 1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<NullableStructSpanFormattableOrArrayClassUnion<double>> firstAsList = [
            new (Math.PI, isSimple, isValue, boolRevealer)
          , new ((double?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, null, 5.0, 6.0], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly NullableStructSpanFormattableOrArrayClassUnion<double>[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Complete();
    }
}

public class NullableDoubleArrayNullablePostFieldClassUnionRevisit : IStringBearer
{
    private readonly double? firstPostField = null;

    public NullableDoubleArrayNullablePostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<NullableStructSpanFormattableOrArrayClassUnion<double>>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructSpanFormattableOrArrayClassUnion<double>([1.0, 2.0, null, 3.0], isSimple, isValue, boolRevealer);
        List<NullableStructSpanFormattableOrArrayClassUnion<double>> firstAsList = [
            new (Math.E, isSimple, isValue, boolRevealer)
          , new ((double?[]?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0, null], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly NullableStructSpanFormattableOrArrayClassUnion<double>[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldDoubleSpanClassUnionRevisit : IStringBearer
{
    private readonly double firstPreField = Math.E;

    public PreFieldDoubleSpanClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<SpanFormattableOrSpanClassUnion<double, double>>? nodeRevealer = null)
    {
        var repeatedItem = new SpanFormattableOrSpanClassUnion<double, double>([1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<SpanFormattableOrSpanClassUnion<double, double>> firstAsList = [
            new (Math.PI, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0], isSimple, isValue, boolRevealer)
          , repeatedItem  
        ];
        firstSpan = firstAsList.ToArray();
    }

    private readonly SpanFormattableOrSpanClassUnion<double, double>[] firstSpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstSpan), firstSpan.AsSpan())
           .Complete();
    }
}

public class DoubleSpanPostFieldClassUnionRevisit : IStringBearer
{
    private readonly double firstPostField = Math.PI;

    public DoubleSpanPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<SpanFormattableOrSpanClassUnion<double, double>>? nodeRevealer = null)
    {
        var repeatedItem = new SpanFormattableOrSpanClassUnion<double, double>([1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<SpanFormattableOrSpanClassUnion<double, double>> firstAsList = [
            new (Math.E, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstSpan = firstAsList.ToArray();
    }

    private readonly SpanFormattableOrSpanClassUnion<double, double>[] firstSpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstSpan), firstSpan.AsSpan())
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class NullablePreFieldNullableDoubleSpanClassUnionRevisit : IStringBearer
{
    private readonly double? firstPreField = null;

    public NullablePreFieldNullableDoubleSpanClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<NullableStructSpanFormattableOrSpanClassUnion<double>>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructSpanFormattableOrSpanClassUnion<double>([null, 1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<NullableStructSpanFormattableOrSpanClassUnion<double>> firstAsList = [
            new (Math.PI, isSimple, isValue, boolRevealer)
          , new ((double?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, null, 5.0, 6.0], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstSpan = firstAsList.ToArray();
    }

    private readonly NullableStructSpanFormattableOrSpanClassUnion<double>[] firstSpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstSpan), firstSpan.AsSpan())
           .Complete();
    }
}

public class NullableDoubleSpanNullablePostFieldClassUnionRevisit : IStringBearer
{
    private readonly double? firstPostField = null;

    public NullableDoubleSpanNullablePostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<NullableStructSpanFormattableOrSpanClassUnion<double>>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructSpanFormattableOrSpanClassUnion<double>([1.0, 2.0, null, 3.0], isSimple, isValue, boolRevealer);
        List<NullableStructSpanFormattableOrSpanClassUnion<double>> firstAsList = [
            new (Math.E, isSimple, isValue, boolRevealer)
          , new ((double?[]?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0, null], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstSpan = firstAsList.ToArray();
    }

    private readonly NullableStructSpanFormattableOrSpanClassUnion<double>[] firstSpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstSpan), firstSpan.AsSpan())
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldDoubleReadOnlySpanClassUnionRevisit : IStringBearer
{
    private readonly double firstPreField = Math.E;

    public PreFieldDoubleReadOnlySpanClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<SpanFormattableOrReadOnlySpanClassUnion<double, double>>? nodeRevealer = null)
    {
        var repeatedItem = new SpanFormattableOrReadOnlySpanClassUnion<double, double>([1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<SpanFormattableOrReadOnlySpanClassUnion<double, double>> firstAsList = [
            new (Math.PI, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0], isSimple, isValue, boolRevealer)
          , repeatedItem  
        ];
        firstReadOnlySpan = firstAsList.ToArray();
    }

    private readonly SpanFormattableOrReadOnlySpanClassUnion<double, double>[] firstReadOnlySpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstReadOnlySpan), (ReadOnlySpan<SpanFormattableOrReadOnlySpanClassUnion<double, double>>)firstReadOnlySpan)
           .Complete();
    }
}

public class DoubleReadOnlySpanPostFieldClassUnionRevisit : IStringBearer
{
    private readonly double firstPostField = Math.PI;

    public DoubleReadOnlySpanPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<SpanFormattableOrReadOnlySpanClassUnion<double, double>>? nodeRevealer = null)
    {
        var repeatedItem = new SpanFormattableOrReadOnlySpanClassUnion<double, double>([1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<SpanFormattableOrReadOnlySpanClassUnion<double, double>> firstAsList = [
            new (Math.E, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstReadOnlySpan = firstAsList.ToArray();
    }

    private readonly SpanFormattableOrReadOnlySpanClassUnion<double, double>[] firstReadOnlySpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstReadOnlySpan), (ReadOnlySpan<SpanFormattableOrReadOnlySpanClassUnion<double, double>>)firstReadOnlySpan)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class NullablePreFieldNullableDoubleReadOnlySpanClassUnionRevisit : IStringBearer
{
    private readonly double? firstPreField = null;

    public NullablePreFieldNullableDoubleReadOnlySpanClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>([null, 1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>> firstAsList = [
            new (Math.PI, isSimple, isValue, boolRevealer)
          , new ((double?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, null, 5.0, 6.0], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstReadOnlySpan = firstAsList.ToArray();
    }

    private readonly NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>[] firstReadOnlySpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstReadOnlySpan), (ReadOnlySpan<NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>>)firstReadOnlySpan)
           .Complete();
    }
}

public class NullableDoubleReadOnlySpanNullablePostFieldClassUnionRevisit : IStringBearer
{
    private readonly double? firstPostField = null;

    public NullableDoubleReadOnlySpanNullablePostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>([1.0, 2.0, null, 3.0], isSimple, isValue, boolRevealer);
        List<NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>> firstAsList = [
            new (Math.E, isSimple, isValue, boolRevealer)
          , new ((double?[]?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0, null], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstReadOnlySpan = firstAsList.ToArray();
    }

    private readonly NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>[] firstReadOnlySpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstReadOnlySpan), (ReadOnlySpan<NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>>)firstReadOnlySpan)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldDoubleListStructUnionRevisit : IStringBearer
{
    private readonly double firstPreField = Math.E;

    public PreFieldDoubleListStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<SpanFormattableOrListStructUnion<double, double>>? nodeRevealer = null)
    {
        var repeatedItem = new SpanFormattableOrListStructUnion<double, double>([1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<SpanFormattableOrListStructUnion<double, double>> firstAsList = [
            new (Math.PI, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<SpanFormattableOrListStructUnion<double, double>> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Complete();
    }
}

public class DoubleListPostFieldStructUnionRevisit : IStringBearer
{
    private readonly double firstPostField = Math.PI;

    public DoubleListPostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<SpanFormattableOrArrayStructUnion<double, double>>? nodeRevealer = null)
    {
        var repeatedItem = new SpanFormattableOrListStructUnion<double, double>([1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<SpanFormattableOrListStructUnion<double, double>> firstAsList = [
            new (Math.E, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<SpanFormattableOrListStructUnion<double, double>> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class NullablePreFieldNullableDoubleListStructUnionRevisit : IStringBearer
{
    private readonly double? firstPreField = Math.E;

    public NullablePreFieldNullableDoubleListStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<NullableStructSpanFormattableOrListStructUnion<double>>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructSpanFormattableOrListStructUnion<double>([null, 1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<NullableStructSpanFormattableOrListStructUnion<double>> firstAsList = [
            new (Math.PI, isSimple, isValue, boolRevealer)
          , new ((double?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, null, 5.0, 6.0], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<NullableStructSpanFormattableOrListStructUnion<double>> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Complete();
    }
}

public class NullableDoubleListNullablePostFieldStructUnionRevisit : IStringBearer
{
    private readonly double? firstPostField = Math.PI;

    public NullableDoubleListNullablePostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<NullableStructSpanFormattableOrListStructUnion<double>>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructSpanFormattableOrListStructUnion<double>([1.0, 2.0, null, 3.0], isSimple, isValue, boolRevealer);
        List<NullableStructSpanFormattableOrListStructUnion<double>> firstAsList = [
            new (Math.E, isSimple, isValue, boolRevealer)
          , new ((List<double?>?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0, null], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<NullableStructSpanFormattableOrListStructUnion<double>> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldDoubleListClassUnionRevisit : IStringBearer
{
    private readonly double firstPreField = Math.E;

    public PreFieldDoubleListClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<SpanFormattableOrListClassUnion<double, double>>? nodeRevealer = null)
    {
        var repeatedItem = new SpanFormattableOrListClassUnion<double, double>([1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<SpanFormattableOrListClassUnion<double, double>> firstAsList = [
            new (Math.PI, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<SpanFormattableOrListClassUnion<double, double>> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Complete();
    }
}

public class DoubleListPostFieldClassUnionRevisit : IStringBearer
{
    private readonly double firstPostField = Math.PI;

    public DoubleListPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<SpanFormattableOrListClassUnion<double, double>>? nodeRevealer = null)
    {
        var repeatedItem = new SpanFormattableOrListClassUnion<double, double>([1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<SpanFormattableOrListClassUnion<double, double>> firstAsList = [
            new (Math.E, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<SpanFormattableOrListClassUnion<double, double>> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class NullablePreFieldNullableDoubleListClassUnionRevisit : IStringBearer
{
    private readonly double? firstPreField = null;

    public NullablePreFieldNullableDoubleListClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<NullableStructSpanFormattableOrListClassUnion<double>>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructSpanFormattableOrListClassUnion<double>([null, 1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<NullableStructSpanFormattableOrListClassUnion<double>> firstAsList = [
            new (Math.PI, isSimple, isValue, boolRevealer)
          , new ((double?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, null, 5.0, 6.0], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<NullableStructSpanFormattableOrListClassUnion<double>> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Complete();
    }
}

public class NullableDoubleListNullablePostFieldClassUnionRevisit : IStringBearer
{
    private readonly double? firstPostField = null;

    public NullableDoubleListNullablePostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<NullableStructSpanFormattableOrListClassUnion<double>>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructSpanFormattableOrListClassUnion<double>([1.0, 2.0, null, 3.0], isSimple, isValue, boolRevealer);
        List<NullableStructSpanFormattableOrListClassUnion<double>> firstAsList = [
            new (Math.E, isSimple, isValue, boolRevealer)
          , new ((List<double?>?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0, null], isSimple, isValue, boolRevealer)
           ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<NullableStructSpanFormattableOrListClassUnion<double>> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
                  .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
                  .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
                  .Complete();
    }
}

public class PreFieldDoubleEnumerableStructUnionRevisit : IStringBearer
{
    private readonly double firstPreField = Math.E;

    public PreFieldDoubleEnumerableStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<SpanFormattableOrEnumerableStructUnion<double, double>>? nodeRevealer = null)
    {
        var repeatedItem = new SpanFormattableOrEnumerableStructUnion<double, double>([1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<SpanFormattableOrEnumerableStructUnion<double, double>> firstAsEnumerable = [
            new (Math.PI, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly List<SpanFormattableOrEnumerableStructUnion<double, double>> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField
           .AlwaysRevealAllEnumerate<
                   List<SpanFormattableOrEnumerableStructUnion<double, double>>
                     , SpanFormattableOrEnumerableStructUnion<double, double>
               >(nameof(firstEnumerable), firstEnumerable)
           .Complete();
    }
}

public class DoubleEnumerablePostFieldStructUnionRevisit : IStringBearer
{
    private readonly double firstPostField = Math.PI;

    public DoubleEnumerablePostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<SpanFormattableOrArrayStructUnion<double, double>>? nodeRevealer = null)
    {
        var repeatedItem = new SpanFormattableOrEnumerableStructUnion<double, double>([1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<SpanFormattableOrEnumerableStructUnion<double, double>> firstAsEnumerable = [
            new (Math.E, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly List<SpanFormattableOrEnumerableStructUnion<double, double>> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate<
               List<SpanFormattableOrEnumerableStructUnion<double, double>>
             , SpanFormattableOrEnumerableStructUnion<double, double>
           >(nameof(firstEnumerable), firstEnumerable)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class NullablePreFieldNullableDoubleEnumerableStructUnionRevisit : IStringBearer
{
    private readonly double? firstPreField = Math.E;

    public NullablePreFieldNullableDoubleEnumerableStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<NullableStructSpanFormattableOrEnumerableStructUnion<double>>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructSpanFormattableOrEnumerableStructUnion<double>([null, 1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<NullableStructSpanFormattableOrEnumerableStructUnion<double>> firstAsEnumerable = [
            new (Math.PI, isSimple, isValue, boolRevealer)
          , new ((double?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, null, 5.0, 6.0], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly List<NullableStructSpanFormattableOrEnumerableStructUnion<double>> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAllEnumerate<
               List<NullableStructSpanFormattableOrEnumerableStructUnion<double>>
             , NullableStructSpanFormattableOrEnumerableStructUnion<double>
           >(nameof(firstEnumerable), firstEnumerable)
           .Complete();
    }
}

public class NullableDoubleEnumerableNullablePostFieldStructUnionRevisit : IStringBearer
{
    private readonly double? firstPostField = Math.PI;

    public NullableDoubleEnumerableNullablePostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<NullableStructSpanFormattableOrEnumerableStructUnion<double>>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructSpanFormattableOrEnumerableStructUnion<double>([1.0, 2.0, null, 3.0], isSimple, isValue, boolRevealer);
        List<NullableStructSpanFormattableOrEnumerableStructUnion<double>> firstAsEnumerable = [
            new (Math.E, isSimple, isValue, boolRevealer)
          , new ((List<double?>?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0, null], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly List<NullableStructSpanFormattableOrEnumerableStructUnion<double>> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField
           .AlwaysRevealAllEnumerate<
               List<NullableStructSpanFormattableOrEnumerableStructUnion<double>>
             , NullableStructSpanFormattableOrEnumerableStructUnion<double>
           >(nameof(firstEnumerable), firstEnumerable)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldDoubleEnumerableClassUnionRevisit : IStringBearer
{
    private readonly double firstPreField = Math.E;

    public PreFieldDoubleEnumerableClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<SpanFormattableOrEnumerableClassUnion<double, double>>? nodeRevealer = null)
    {
        var repeatedItem = new SpanFormattableOrEnumerableClassUnion<double, double>([1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<SpanFormattableOrEnumerableClassUnion<double, double>> firstAsEnumerable = [
            new (Math.PI, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly List<SpanFormattableOrEnumerableClassUnion<double, double>> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField
           .AlwaysRevealAllEnumerate<
               List<SpanFormattableOrEnumerableClassUnion<double, double>>
             , SpanFormattableOrEnumerableClassUnion<double, double>
           >(nameof(firstEnumerable), firstEnumerable)
           .Complete();
    }
}

public class DoubleEnumerablePostFieldClassUnionRevisit : IStringBearer
{
    private readonly double firstPostField = Math.PI;

    public DoubleEnumerablePostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<SpanFormattableOrEnumerableClassUnion<double, double>>? nodeRevealer = null)
    {
        var repeatedItem = new SpanFormattableOrEnumerableClassUnion<double, double>([1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<SpanFormattableOrEnumerableClassUnion<double, double>> firstAsEnumerable = [
            new (Math.E, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly List<SpanFormattableOrEnumerableClassUnion<double, double>> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField
           .AlwaysRevealAllEnumerate<
               List<SpanFormattableOrEnumerableClassUnion<double, double>>
             , SpanFormattableOrEnumerableClassUnion<double, double>
           >(nameof(firstEnumerable), firstEnumerable)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class NullablePreFieldNullableDoubleEnumerableClassUnionRevisit : IStringBearer
{
    private readonly double? firstPreField = null;

    public NullablePreFieldNullableDoubleEnumerableClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<NullableStructSpanFormattableOrEnumerableClassUnion<double>>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructSpanFormattableOrEnumerableClassUnion<double>([null, 1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<NullableStructSpanFormattableOrEnumerableClassUnion<double>> firstAsEnumerable = [
            new (Math.PI, isSimple, isValue, boolRevealer)
          , new ((double?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, null, 5.0, 6.0], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly List<NullableStructSpanFormattableOrEnumerableClassUnion<double>> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField
           .AlwaysRevealAllEnumerate<
               List<NullableStructSpanFormattableOrEnumerableClassUnion<double>>
             , NullableStructSpanFormattableOrEnumerableClassUnion<double>
           >(nameof(firstEnumerable), firstEnumerable)
           .Complete();
    }
}

public class NullableDoubleEnumerableNullablePostFieldClassUnionRevisit : IStringBearer
{
    private readonly double? firstPostField = null;

    public NullableDoubleEnumerableNullablePostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<NullableStructSpanFormattableOrEnumerableClassUnion<double>>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructSpanFormattableOrEnumerableClassUnion<double>([1.0, 2.0, null, 3.0], isSimple, isValue, boolRevealer);
        List<NullableStructSpanFormattableOrEnumerableClassUnion<double>> firstAsEnumerable = [
            new (Math.E, isSimple, isValue, boolRevealer)
          , new ((List<double?>?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0, null], isSimple, isValue, boolRevealer)
           ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly List<NullableStructSpanFormattableOrEnumerableClassUnion<double>> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
                  .CollectionField
                  .AlwaysRevealAllEnumerate<
                      List<NullableStructSpanFormattableOrEnumerableClassUnion<double>>
                    , NullableStructSpanFormattableOrEnumerableClassUnion<double>
                  >(nameof(firstEnumerable), firstEnumerable)
                  .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
                  .Complete();
    }
}


public class PreFieldDoubleEnumeratorStructUnionRevisit : IStringBearer
{
    private readonly double firstPreField = Math.E;

    public PreFieldDoubleEnumeratorStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<SpanFormattableOrEnumeratorStructUnion<double, double>>? nodeRevealer = null)
    {
        var repeatedItem = new SpanFormattableOrEnumeratorStructUnion<double, double>([1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<SpanFormattableOrEnumeratorStructUnion<double, double>> firstAsEnumerator = [
            new (Math.PI, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<SpanFormattableOrEnumeratorStructUnion<double, double>>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<SpanFormattableOrEnumeratorStructUnion<double, double>>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAllIterate<
               List<SpanFormattableOrEnumeratorStructUnion<double, double>>.Enumerator
             , SpanFormattableOrEnumeratorStructUnion<double, double>
           >(nameof(firstEnumerator), firstEnumerator)
           .Complete();
    }
}

public class DoubleEnumeratorPostFieldStructUnionRevisit : IStringBearer
{
    private readonly double firstPostField = Math.PI;

    public DoubleEnumeratorPostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<SpanFormattableOrArrayStructUnion<double, double>>? nodeRevealer = null)
    {
        var repeatedItem = new SpanFormattableOrEnumeratorStructUnion<double, double>([1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<SpanFormattableOrEnumeratorStructUnion<double, double>> firstAsEnumerator = [
            new (Math.E, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<SpanFormattableOrEnumeratorStructUnion<double, double>>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<SpanFormattableOrEnumeratorStructUnion<double, double>>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllIterate<
               List<SpanFormattableOrEnumeratorStructUnion<double, double>>.Enumerator
             , SpanFormattableOrEnumeratorStructUnion<double, double>
           >(nameof(firstEnumerator), firstEnumerator)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class NullablePreFieldNullableDoubleEnumeratorStructUnionRevisit : IStringBearer
{
    private readonly double? firstPreField = Math.E;

    public NullablePreFieldNullableDoubleEnumeratorStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<NullableStructSpanFormattableOrEnumeratorStructUnion<double>>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructSpanFormattableOrEnumeratorStructUnion<double>([null, 1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<NullableStructSpanFormattableOrEnumeratorStructUnion<double>> firstAsEnumerator = [
            new (Math.PI, isSimple, isValue, boolRevealer)
          , new ((double?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, null, 5.0, 6.0], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<NullableStructSpanFormattableOrEnumeratorStructUnion<double>>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<NullableStructSpanFormattableOrEnumeratorStructUnion<double>>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAllIterate<
               List<NullableStructSpanFormattableOrEnumeratorStructUnion<double>>.Enumerator
             , NullableStructSpanFormattableOrEnumeratorStructUnion<double>
           >(nameof(firstEnumerator), firstEnumerator)
           .Complete();
    }
}

public class NullableDoubleEnumeratorNullablePostFieldStructUnionRevisit : IStringBearer
{
    private readonly double? firstPostField = Math.PI;

    public NullableDoubleEnumeratorNullablePostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<NullableStructSpanFormattableOrEnumeratorStructUnion<double>>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructSpanFormattableOrEnumeratorStructUnion<double>([1.0, 2.0, null, 3.0], isSimple, isValue, boolRevealer);
        List<NullableStructSpanFormattableOrEnumeratorStructUnion<double>> firstAsEnumerator = [
            new (Math.E, isSimple, isValue, boolRevealer)
          , new ((List<double?>?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0, null], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<NullableStructSpanFormattableOrEnumeratorStructUnion<double>>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<NullableStructSpanFormattableOrEnumeratorStructUnion<double>>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllIterate<
               List<NullableStructSpanFormattableOrEnumeratorStructUnion<double>>.Enumerator
             , NullableStructSpanFormattableOrEnumeratorStructUnion<double>
           >(nameof(firstEnumerator), firstEnumerator)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldDoubleEnumeratorClassUnionRevisit : IStringBearer
{
    private readonly double firstPreField = Math.E;

    public PreFieldDoubleEnumeratorClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<SpanFormattableOrEnumeratorClassUnion<double, double>>? nodeRevealer = null)
    {
        var repeatedItem = new SpanFormattableOrEnumeratorClassUnion<double, double>([1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<SpanFormattableOrEnumeratorClassUnion<double, double>> firstAsEnumerator = [
            new (Math.PI, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<SpanFormattableOrEnumeratorClassUnion<double, double>>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<SpanFormattableOrEnumeratorClassUnion<double, double>>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAllIterate<
               List<SpanFormattableOrEnumeratorClassUnion<double, double>>.Enumerator
             , SpanFormattableOrEnumeratorClassUnion<double, double>
           >(nameof(firstEnumerator), firstEnumerator)
           .Complete();
    }
}

public class DoubleEnumeratorPostFieldClassUnionRevisit : IStringBearer
{
    private readonly double firstPostField = Math.PI;

    public DoubleEnumeratorPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<SpanFormattableOrEnumeratorClassUnion<double, double>>? nodeRevealer = null)
    {
        var repeatedItem = new SpanFormattableOrEnumeratorClassUnion<double, double>([1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<SpanFormattableOrEnumeratorClassUnion<double, double>> firstAsEnumerator = [
            new (Math.E, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<SpanFormattableOrEnumeratorClassUnion<double, double>>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<SpanFormattableOrEnumeratorClassUnion<double, double>>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllIterate<
               List<SpanFormattableOrEnumeratorClassUnion<double, double>>.Enumerator
             , SpanFormattableOrEnumeratorClassUnion<double, double>
           >(nameof(firstEnumerator), firstEnumerator)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class NullablePreFieldNullableDoubleEnumeratorClassUnionRevisit : IStringBearer
{
    private readonly double? firstPreField = null;

    public NullablePreFieldNullableDoubleEnumeratorClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<NullableStructSpanFormattableOrEnumeratorClassUnion<double>>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructSpanFormattableOrEnumeratorClassUnion<double>([null, 1.0, 2.0, 3.0], isSimple, isValue, boolRevealer);
        List<NullableStructSpanFormattableOrEnumeratorClassUnion<double>> firstAsEnumerator = [
            new (Math.PI, isSimple, isValue, boolRevealer)
          , new ((double?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, null, 5.0, 6.0], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<NullableStructSpanFormattableOrEnumeratorClassUnion<double>>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<NullableStructSpanFormattableOrEnumeratorClassUnion<double>>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAllIterate<
               List<NullableStructSpanFormattableOrEnumeratorClassUnion<double>>.Enumerator
             , NullableStructSpanFormattableOrEnumeratorClassUnion<double>
           >(nameof(firstEnumerator), firstEnumerator)
           .Complete();
    }
}

public class NullableDoubleEnumeratorNullablePostFieldClassUnionRevisit : IStringBearer
{
    private readonly double? firstPostField = null;

    public NullableDoubleEnumeratorNullablePostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<double>? boolRevealer = null
      , PalantírReveal<NullableStructSpanFormattableOrEnumeratorClassUnion<double>>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructSpanFormattableOrEnumeratorClassUnion<double>([1.0, 2.0, null, 3.0], isSimple, isValue, boolRevealer);
        List<NullableStructSpanFormattableOrEnumeratorClassUnion<double>> firstAsEnumerator = [
            new (Math.E, isSimple, isValue, boolRevealer)
          , new ((List<double?>?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([4.0, 5.0, 6.0, null], isSimple, isValue, boolRevealer)
           ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<NullableStructSpanFormattableOrEnumeratorClassUnion<double>>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<NullableStructSpanFormattableOrEnumeratorClassUnion<double>>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
                  .CollectionField.AlwaysRevealAllIterate<
                      List<NullableStructSpanFormattableOrEnumeratorClassUnion<double>>.Enumerator
                    , NullableStructSpanFormattableOrEnumeratorClassUnion<double>
                  >(nameof(firstEnumerator), firstEnumerator)
                  .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
                  .Complete();
    }
}
