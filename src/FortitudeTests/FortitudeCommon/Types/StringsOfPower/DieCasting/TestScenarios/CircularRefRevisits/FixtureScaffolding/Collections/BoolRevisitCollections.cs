// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.Collections;

public class PreFieldBoolArrayStructUnionRevisit : IStringBearer
{
    private readonly bool firstPreField = true;

    public PreFieldBoolArrayStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<BoolOrArrayStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new BoolOrArrayStructUnion([true, false, true], isSimple, isValue, boolRevealer);
        List<BoolOrArrayStructUnion> firstAsList = [
            new (false, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([false, true, false], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly BoolOrArrayStructUnion[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Complete();
    }
}

public class BoolArrayPostFieldStructUnionRevisit : IStringBearer
{
    private readonly bool firstPostField = true;

    public BoolArrayPostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<BoolOrArrayStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new BoolOrArrayStructUnion([false, true, false], isSimple, isValue, boolRevealer);
        List<BoolOrArrayStructUnion> firstAsList = [
            new (false, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([true, false, true], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly BoolOrArrayStructUnion[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class NullablePreFieldNullableBoolArrayStructUnionRevisit : IStringBearer
{
    private readonly bool? firstPreField = false;

    public NullablePreFieldNullableBoolArrayStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<NullableStructBoolOrArrayStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructBoolOrArrayStructUnion([null, true, false, true], isSimple, isValue, boolRevealer);
        List<NullableStructBoolOrArrayStructUnion> firstAsList = [
            new (false, isSimple, isValue, boolRevealer)
          , new ((bool?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([false, null, true, false], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly NullableStructBoolOrArrayStructUnion[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Complete();
    }
}

public class NullableBoolArrayNullablePostFieldStructUnionRevisit : IStringBearer
{
    private readonly bool? firstPostField = true;

    public NullableBoolArrayNullablePostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<NullableStructBoolOrArrayStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructBoolOrArrayStructUnion([false, true, null, false], isSimple, isValue, boolRevealer);
        List<NullableStructBoolOrArrayStructUnion> firstAsList = [
            new (false, isSimple, isValue, boolRevealer)
          , new ((bool?[]?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([true, false, true, null], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly NullableStructBoolOrArrayStructUnion[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldBoolArrayClassUnionRevisit : IStringBearer
{
    private readonly bool firstPreField = false;

    public PreFieldBoolArrayClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<BoolOrArrayClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new BoolOrArrayClassUnion([true, false, true], isSimple, isValue, boolRevealer);
        List<BoolOrArrayClassUnion> firstAsList = [
            new (false, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([false, true, false], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly BoolOrArrayClassUnion[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Complete();
    }
}

public class BoolArrayPostFieldClassUnionRevisit : IStringBearer
{
    private readonly bool firstPostField = false;

    public BoolArrayPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<BoolOrArrayClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new BoolOrArrayClassUnion([false, true, false], isSimple, isValue, boolRevealer);
        List<BoolOrArrayClassUnion> firstAsList = [
            new (false, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([true, false, true], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly BoolOrArrayClassUnion[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class NullablePreFieldNullableBoolArrayClassUnionRevisit : IStringBearer
{
    private readonly bool? firstPreField = null;

    public NullablePreFieldNullableBoolArrayClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<NullableStructBoolOrArrayClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructBoolOrArrayClassUnion([null, true, false, true], isSimple, isValue, boolRevealer);
        List<NullableStructBoolOrArrayClassUnion> firstAsList = [
            new (false, isSimple, isValue, boolRevealer)
          , new ((bool?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([false, null, true, false], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly NullableStructBoolOrArrayClassUnion[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Complete();
    }
}

public class NullableBoolArrayNullablePostFieldClassUnionRevisit : IStringBearer
{
    private readonly bool? firstPostField = null;

    public NullableBoolArrayNullablePostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<NullableStructBoolOrArrayClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructBoolOrArrayClassUnion([false, true, null, false], isSimple, isValue, boolRevealer);
        List<NullableStructBoolOrArrayClassUnion> firstAsList = [
            new (false, isSimple, isValue, boolRevealer)
          , new ((bool?[]?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([true, false, true, null], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly NullableStructBoolOrArrayClassUnion[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldBoolSpanClassUnionRevisit : IStringBearer
{
    private readonly bool firstPreField = false;

    public PreFieldBoolSpanClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<BoolOrSpanClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new BoolOrSpanClassUnion([true, false, true], isSimple, isValue, boolRevealer);
        List<BoolOrSpanClassUnion> firstAsList = [
            new (false, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([false, true, false], isSimple, isValue, boolRevealer)
          , repeatedItem  
        ];
        firstSpan = firstAsList.ToArray();
    }

    private readonly BoolOrSpanClassUnion[] firstSpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstSpan), firstSpan.AsSpan())
           .Complete();
    }
}

public class BoolSpanPostFieldClassUnionRevisit : IStringBearer
{
    private readonly bool firstPostField = false;

    public BoolSpanPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<BoolOrSpanClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new BoolOrSpanClassUnion([false, true, false], isSimple, isValue, boolRevealer);
        List<BoolOrSpanClassUnion> firstAsList = [
            new (false, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([true, false, true], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstSpan = firstAsList.ToArray();
    }

    private readonly BoolOrSpanClassUnion[] firstSpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstSpan), firstSpan.AsSpan())
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class NullablePreFieldNullableBoolSpanClassUnionRevisit : IStringBearer
{
    private readonly bool? firstPreField = null;

    public NullablePreFieldNullableBoolSpanClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<NullableStructBoolOrSpanClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructBoolOrSpanClassUnion([null, true, false, true], isSimple, isValue, boolRevealer);
        List<NullableStructBoolOrSpanClassUnion> firstAsList = [
            new (false, isSimple, isValue, boolRevealer)
          , new ((bool?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([false, null, true, false], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstSpan = firstAsList.ToArray();
    }

    private readonly NullableStructBoolOrSpanClassUnion[] firstSpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstSpan), firstSpan.AsSpan())
           .Complete();
    }
}

public class NullableBoolSpanNullablePostFieldClassUnionRevisit : IStringBearer
{
    private readonly bool? firstPostField = null;

    public NullableBoolSpanNullablePostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<NullableStructBoolOrSpanClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructBoolOrSpanClassUnion([false, true, null, false], isSimple, isValue, boolRevealer);
        List<NullableStructBoolOrSpanClassUnion> firstAsList = [
            new (false, isSimple, isValue, boolRevealer)
          , new ((bool?[]?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([true, false, true, null], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstSpan = firstAsList.ToArray();
    }

    private readonly NullableStructBoolOrSpanClassUnion[] firstSpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstSpan), firstSpan.AsSpan())
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldBoolReadOnlySpanClassUnionRevisit : IStringBearer
{
    private readonly bool firstPreField = false;

    public PreFieldBoolReadOnlySpanClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<BoolOrReadOnlySpanClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new BoolOrReadOnlySpanClassUnion([true, false, true], isSimple, isValue, boolRevealer);
        List<BoolOrReadOnlySpanClassUnion> firstAsList = [
            new (false, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([false, true, false], isSimple, isValue, boolRevealer)
          , repeatedItem  
        ];
        firstReadOnlySpan = firstAsList.ToArray();
    }

    private readonly BoolOrReadOnlySpanClassUnion[] firstReadOnlySpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstReadOnlySpan), (ReadOnlySpan<BoolOrReadOnlySpanClassUnion>)firstReadOnlySpan)
           .Complete();
    }
}

public class BoolReadOnlySpanPostFieldClassUnionRevisit : IStringBearer
{
    private readonly bool firstPostField = false;

    public BoolReadOnlySpanPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<BoolOrReadOnlySpanClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new BoolOrReadOnlySpanClassUnion([false, true, false], isSimple, isValue, boolRevealer);
        List<BoolOrReadOnlySpanClassUnion> firstAsList = [
            new (false, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([true, false, true], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstReadOnlySpan = firstAsList.ToArray();
    }

    private readonly BoolOrReadOnlySpanClassUnion[] firstReadOnlySpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstReadOnlySpan), (ReadOnlySpan<BoolOrReadOnlySpanClassUnion>)firstReadOnlySpan)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class NullablePreFieldNullableBoolReadOnlySpanClassUnionRevisit : IStringBearer
{
    private readonly bool? firstPreField = null;

    public NullablePreFieldNullableBoolReadOnlySpanClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<NullableStructBoolOrReadOnlySpanClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructBoolOrReadOnlySpanClassUnion([null, true, false, true], isSimple, isValue, boolRevealer);
        List<NullableStructBoolOrReadOnlySpanClassUnion> firstAsList = [
            new (false, isSimple, isValue, boolRevealer)
          , new ((bool?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([false, null, true, false], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstReadOnlySpan = firstAsList.ToArray();
    }

    private readonly NullableStructBoolOrReadOnlySpanClassUnion[] firstReadOnlySpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstReadOnlySpan), (ReadOnlySpan<NullableStructBoolOrReadOnlySpanClassUnion>)firstReadOnlySpan)
           .Complete();
    }
}

public class NullableBoolReadOnlySpanNullablePostFieldClassUnionRevisit : IStringBearer
{
    private readonly bool? firstPostField = null;

    public NullableBoolReadOnlySpanNullablePostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<NullableStructBoolOrReadOnlySpanClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructBoolOrReadOnlySpanClassUnion([false, true, null, false], isSimple, isValue, boolRevealer);
        List<NullableStructBoolOrReadOnlySpanClassUnion> firstAsList = [
            new (false, isSimple, isValue, boolRevealer)
          , new ((bool?[]?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([true, false, true, null], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstReadOnlySpan = firstAsList.ToArray();
    }

    private readonly NullableStructBoolOrReadOnlySpanClassUnion[] firstReadOnlySpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstReadOnlySpan), (ReadOnlySpan<NullableStructBoolOrReadOnlySpanClassUnion>)firstReadOnlySpan)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldBoolListStructUnionRevisit : IStringBearer
{
    private readonly bool firstPreField = true;

    public PreFieldBoolListStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<BoolOrListStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new BoolOrListStructUnion([true, false, true], isSimple, isValue, boolRevealer);
        List<BoolOrListStructUnion> firstAsList = [
            new (false, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([false, true, false], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<BoolOrListStructUnion> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Complete();
    }
}

public class BoolListPostFieldStructUnionRevisit : IStringBearer
{
    private readonly bool firstPostField = true;

    public BoolListPostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<BoolOrArrayStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new BoolOrListStructUnion([false, true, false], isSimple, isValue, boolRevealer);
        List<BoolOrListStructUnion> firstAsList = [
            new (false, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([true, false, true], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<BoolOrListStructUnion> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class NullablePreFieldNullableBoolListStructUnionRevisit : IStringBearer
{
    private readonly bool? firstPreField = false;

    public NullablePreFieldNullableBoolListStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<NullableStructBoolOrListStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructBoolOrListStructUnion([null, true, false, true], isSimple, isValue, boolRevealer);
        List<NullableStructBoolOrListStructUnion> firstAsList = [
            new (false, isSimple, isValue, boolRevealer)
          , new ((bool?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([false, null, true, false], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<NullableStructBoolOrListStructUnion> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Complete();
    }
}

public class NullableBoolListNullablePostFieldStructUnionRevisit : IStringBearer
{
    private readonly bool? firstPostField = true;

    public NullableBoolListNullablePostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<NullableStructBoolOrListStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructBoolOrListStructUnion([false, true, null, false], isSimple, isValue, boolRevealer);
        List<NullableStructBoolOrListStructUnion> firstAsList = [
            new (false, isSimple, isValue, boolRevealer)
          , new ((List<bool?>?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([true, false, true, null], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<NullableStructBoolOrListStructUnion> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldBoolListClassUnionRevisit : IStringBearer
{
    private readonly bool firstPreField = false;

    public PreFieldBoolListClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<BoolOrListClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new BoolOrListClassUnion([true, false, true], isSimple, isValue, boolRevealer);
        List<BoolOrListClassUnion> firstAsList = [
            new (false, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([false, true, false], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<BoolOrListClassUnion> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Complete();
    }
}

public class BoolListPostFieldClassUnionRevisit : IStringBearer
{
    private readonly bool firstPostField = false;

    public BoolListPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<BoolOrListClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new BoolOrListClassUnion([false, true, false], isSimple, isValue, boolRevealer);
        List<BoolOrListClassUnion> firstAsList = [
            new (false, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([true, false, true], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<BoolOrListClassUnion> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class NullablePreFieldNullableBoolListClassUnionRevisit : IStringBearer
{
    private readonly bool? firstPreField = null;

    public NullablePreFieldNullableBoolListClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<NullableStructBoolOrListClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructBoolOrListClassUnion([null, true, false, true], isSimple, isValue, boolRevealer);
        List<NullableStructBoolOrListClassUnion> firstAsList = [
            new (false, isSimple, isValue, boolRevealer)
          , new ((bool?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([false, null, true, false], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<NullableStructBoolOrListClassUnion> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Complete();
    }
}

public class NullableBoolListNullablePostFieldClassUnionRevisit : IStringBearer
{
    private readonly bool? firstPostField = null;

    public NullableBoolListNullablePostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<NullableStructBoolOrListClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructBoolOrListClassUnion([false, true, null, false], isSimple, isValue, boolRevealer);
        List<NullableStructBoolOrListClassUnion> firstAsList = [
            new (false, isSimple, isValue, boolRevealer)
          , new ((List<bool?>?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([true, false, true, null], isSimple, isValue, boolRevealer)
           ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<NullableStructBoolOrListClassUnion> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
                  .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
                  .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
                  .Complete();
    }
}

public class PreFieldBoolEnumerableStructUnionRevisit : IStringBearer
{
    private readonly bool firstPreField = true;

    public PreFieldBoolEnumerableStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<BoolOrEnumerableStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new BoolOrEnumerableStructUnion([true, false, true], isSimple, isValue, boolRevealer);
        List<BoolOrEnumerableStructUnion> firstAsEnumerable = [
            new (false, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([false, true, false], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly IEnumerable<BoolOrEnumerableStructUnion> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField
           .AlwaysRevealAllEnumerate<IEnumerable<BoolOrEnumerableStructUnion>, BoolOrEnumerableStructUnion>(nameof(firstEnumerable), firstEnumerable)
           .Complete();
    }
}

public class BoolEnumerablePostFieldStructUnionRevisit : IStringBearer
{
    private readonly bool firstPostField = true;

    public BoolEnumerablePostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<BoolOrArrayStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new BoolOrEnumerableStructUnion([false, true, false], isSimple, isValue, boolRevealer);
        List<BoolOrEnumerableStructUnion> firstAsEnumerable = [
            new (false, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([true, false, true], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly List<BoolOrEnumerableStructUnion> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField
           .AlwaysRevealAllEnumerate<List<BoolOrEnumerableStructUnion>, BoolOrEnumerableStructUnion>(nameof(firstEnumerable), firstEnumerable)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class NullablePreFieldNullableBoolEnumerableStructUnionRevisit : IStringBearer
{
    private readonly bool? firstPreField = false;

    public NullablePreFieldNullableBoolEnumerableStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<NullableStructBoolOrEnumerableStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructBoolOrEnumerableStructUnion([null, true, false, true], isSimple, isValue, boolRevealer);
        List<NullableStructBoolOrEnumerableStructUnion> firstAsEnumerable = [
            new (false, isSimple, isValue, boolRevealer)
          , new ((bool?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([false, null, true, false], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly List<NullableStructBoolOrEnumerableStructUnion> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField
           .AlwaysRevealAllEnumerate<List<NullableStructBoolOrEnumerableStructUnion>, NullableStructBoolOrEnumerableStructUnion>
               (nameof(firstEnumerable), firstEnumerable)
           .Complete();
    }
}

public class NullableBoolEnumerableNullablePostFieldStructUnionRevisit : IStringBearer
{
    private readonly bool? firstPostField = true;

    public NullableBoolEnumerableNullablePostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<NullableStructBoolOrEnumerableStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructBoolOrEnumerableStructUnion([false, true, null, false], isSimple, isValue, boolRevealer);
        List<NullableStructBoolOrEnumerableStructUnion> firstAsEnumerable = [
            new (false, isSimple, isValue, boolRevealer)
          , new ((List<bool?>?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([true, false, true, null], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly List<NullableStructBoolOrEnumerableStructUnion> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField
           .AlwaysRevealAllEnumerate<List<NullableStructBoolOrEnumerableStructUnion>, NullableStructBoolOrEnumerableStructUnion>
               (nameof(firstEnumerable), firstEnumerable)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldBoolEnumerableClassUnionRevisit : IStringBearer
{
    private readonly bool firstPreField = false;

    public PreFieldBoolEnumerableClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<BoolOrEnumerableClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new BoolOrEnumerableClassUnion([true, false, true], isSimple, isValue, boolRevealer);
        List<BoolOrEnumerableClassUnion> firstAsEnumerable = [
            new (false, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([false, true, false], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly List<BoolOrEnumerableClassUnion> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField
           .AlwaysRevealAllEnumerate<List<BoolOrEnumerableClassUnion>, BoolOrEnumerableClassUnion>
               (nameof(firstEnumerable), firstEnumerable)
           .Complete();
    }
}

public class BoolEnumerablePostFieldClassUnionRevisit : IStringBearer
{
    private readonly bool firstPostField = false;

    public BoolEnumerablePostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<BoolOrEnumerableClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new BoolOrEnumerableClassUnion([false, true, false], isSimple, isValue, boolRevealer);
        List<BoolOrEnumerableClassUnion> firstAsEnumerable = [
            new (false, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([true, false, true], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly List<BoolOrEnumerableClassUnion> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField
           .AlwaysRevealAllEnumerate<List<BoolOrEnumerableClassUnion>, BoolOrEnumerableClassUnion>
               (nameof(firstEnumerable), firstEnumerable)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class NullablePreFieldNullableBoolEnumerableClassUnionRevisit : IStringBearer
{
    private readonly bool? firstPreField = null;

    public NullablePreFieldNullableBoolEnumerableClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<NullableStructBoolOrEnumerableClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructBoolOrEnumerableClassUnion([null, true, false, true], isSimple, isValue, boolRevealer);
        List<NullableStructBoolOrEnumerableClassUnion> firstAsEnumerable = [
            new (false, isSimple, isValue, boolRevealer)
          , new ((bool?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([false, null, true, false], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly List<NullableStructBoolOrEnumerableClassUnion> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField
           .AlwaysRevealAllEnumerate<List<NullableStructBoolOrEnumerableClassUnion>, NullableStructBoolOrEnumerableClassUnion>
               (nameof(firstEnumerable), firstEnumerable)
           .Complete();
    }
}

public class NullableBoolEnumerableNullablePostFieldClassUnionRevisit : IStringBearer
{
    private readonly bool? firstPostField = null;

    public NullableBoolEnumerableNullablePostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<NullableStructBoolOrEnumerableClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructBoolOrEnumerableClassUnion([false, true, null, false], isSimple, isValue, boolRevealer);
        List<NullableStructBoolOrEnumerableClassUnion> firstAsEnumerable = [
            new (false, isSimple, isValue, boolRevealer)
          , new ((List<bool?>?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([true, false, true, null], isSimple, isValue, boolRevealer)
           ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly List<NullableStructBoolOrEnumerableClassUnion> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
                  .CollectionField
                  .AlwaysRevealAllEnumerate<List<NullableStructBoolOrEnumerableClassUnion>, NullableStructBoolOrEnumerableClassUnion>
                      (nameof(firstEnumerable), firstEnumerable)
                  .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
                  .Complete();
    }
}


public class PreFieldBoolEnumeratorStructUnionRevisit : IStringBearer
{
    private readonly bool firstPreField = true;

    public PreFieldBoolEnumeratorStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<BoolOrEnumeratorStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new BoolOrEnumeratorStructUnion([true, false, true], isSimple, isValue, boolRevealer);
        List<BoolOrEnumeratorStructUnion> firstAsEnumerator = [
            new (false, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([false, true, false], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<BoolOrEnumeratorStructUnion>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<BoolOrEnumeratorStructUnion>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField
           .AlwaysRevealAllIterate<List<BoolOrEnumeratorStructUnion>.Enumerator, BoolOrEnumeratorStructUnion>
               (nameof(firstEnumerator), firstEnumerator)
           .Complete();
    }
}

public class BoolEnumeratorPostFieldStructUnionRevisit : IStringBearer
{
    private readonly bool firstPostField = true;

    public BoolEnumeratorPostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<BoolOrArrayStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new BoolOrEnumeratorStructUnion([false, true, false], isSimple, isValue, boolRevealer);
        List<BoolOrEnumeratorStructUnion> firstAsEnumerator = [
            new (false, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([true, false, true], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<BoolOrEnumeratorStructUnion>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<BoolOrEnumeratorStructUnion>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .CollectionField
           .AlwaysRevealAllIterate<List<BoolOrEnumeratorStructUnion>.Enumerator, BoolOrEnumeratorStructUnion>
               (nameof(firstEnumerator), firstEnumerator)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class NullablePreFieldNullableBoolEnumeratorStructUnionRevisit : IStringBearer
{
    private readonly bool? firstPreField = false;

    public NullablePreFieldNullableBoolEnumeratorStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<NullableStructBoolOrEnumeratorStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructBoolOrEnumeratorStructUnion([null, true, false, true], isSimple, isValue, boolRevealer);
        List<NullableStructBoolOrEnumeratorStructUnion> firstAsEnumerator = [
            new (false, isSimple, isValue, boolRevealer)
          , new ((bool?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([false, null, true, false], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<NullableStructBoolOrEnumeratorStructUnion>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<NullableStructBoolOrEnumeratorStructUnion>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField
           .AlwaysRevealAllIterate<List<NullableStructBoolOrEnumeratorStructUnion>.Enumerator, NullableStructBoolOrEnumeratorStructUnion>
               (nameof(firstEnumerator), firstEnumerator)
           .Complete();
    }
}

public class NullableBoolEnumeratorNullablePostFieldStructUnionRevisit : IStringBearer
{
    private readonly bool? firstPostField = true;

    public NullableBoolEnumeratorNullablePostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<NullableStructBoolOrEnumeratorStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructBoolOrEnumeratorStructUnion([false, true, null, false], isSimple, isValue, boolRevealer);
        List<NullableStructBoolOrEnumeratorStructUnion> firstAsEnumerator = [
            new (false, isSimple, isValue, boolRevealer)
          , new ((List<bool?>?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([true, false, true, null], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<NullableStructBoolOrEnumeratorStructUnion>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<NullableStructBoolOrEnumeratorStructUnion>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .CollectionField
           .AlwaysRevealAllIterate<List<NullableStructBoolOrEnumeratorStructUnion>.Enumerator, NullableStructBoolOrEnumeratorStructUnion>
               (nameof(firstEnumerator), firstEnumerator)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldBoolEnumeratorClassUnionRevisit : IStringBearer
{
    private readonly bool firstPreField = false;

    public PreFieldBoolEnumeratorClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<BoolOrEnumeratorClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new BoolOrEnumeratorClassUnion([true, false, true], isSimple, isValue, boolRevealer);
        List<BoolOrEnumeratorClassUnion> firstAsEnumerator = [
            new (false, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([false, true, false], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<BoolOrEnumeratorClassUnion>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<BoolOrEnumeratorClassUnion>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField
           .AlwaysRevealAllIterate<List<BoolOrEnumeratorClassUnion>.Enumerator, BoolOrEnumeratorClassUnion>
               (nameof(firstEnumerator), firstEnumerator)
           .Complete();
    }
}

public class BoolEnumeratorPostFieldClassUnionRevisit : IStringBearer
{
    private readonly bool firstPostField = false;

    public BoolEnumeratorPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<BoolOrEnumeratorClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new BoolOrEnumeratorClassUnion([false, true, false], isSimple, isValue, boolRevealer);
        List<BoolOrEnumeratorClassUnion> firstAsEnumerator = [
            new (false, isSimple, isValue, boolRevealer)
          , new (null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([true, false, true], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<BoolOrEnumeratorClassUnion>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<BoolOrEnumeratorClassUnion>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .CollectionField
           .AlwaysRevealAllIterate<List<BoolOrEnumeratorClassUnion>.Enumerator, BoolOrEnumeratorClassUnion>
               (nameof(firstEnumerator), firstEnumerator)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class NullablePreFieldNullableBoolEnumeratorClassUnionRevisit : IStringBearer
{
    private readonly bool? firstPreField = null;

    public NullablePreFieldNullableBoolEnumeratorClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<NullableStructBoolOrEnumeratorClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructBoolOrEnumeratorClassUnion([null, true, false, true], isSimple, isValue, boolRevealer);
        List<NullableStructBoolOrEnumeratorClassUnion> firstAsEnumerator = [
            new (false, isSimple, isValue, boolRevealer)
          , new ((bool?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([false, null, true, false], isSimple, isValue, boolRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<NullableStructBoolOrEnumeratorClassUnion>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<NullableStructBoolOrEnumeratorClassUnion>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField
           .AlwaysRevealAllIterate<List<NullableStructBoolOrEnumeratorClassUnion>.Enumerator, NullableStructBoolOrEnumeratorClassUnion>
               (nameof(firstEnumerator), firstEnumerator)
           .Complete();
    }
}

public class NullableBoolEnumeratorNullablePostFieldClassUnionRevisit : IStringBearer
{
    private readonly bool? firstPostField = null;

    public NullableBoolEnumeratorNullablePostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<NullableStructBoolOrEnumeratorClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new NullableStructBoolOrEnumeratorClassUnion([false, true, null, false], isSimple, isValue, boolRevealer);
        List<NullableStructBoolOrEnumeratorClassUnion> firstAsEnumerator = [
            new (false, isSimple, isValue, boolRevealer)
          , new ((List<bool?>?)null, isSimple, isValue, boolRevealer)
          , repeatedItem  
          , new ([true, false, true, null], isSimple, isValue, boolRevealer)
           ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<NullableStructBoolOrEnumeratorClassUnion>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<NullableStructBoolOrEnumeratorClassUnion>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
                  .CollectionField
                  .AlwaysRevealAllIterate<List<NullableStructBoolOrEnumeratorClassUnion>.Enumerator, NullableStructBoolOrEnumeratorClassUnion>
                      (nameof(firstEnumerator), firstEnumerator)
                  .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
                  .Complete();
    }
}
