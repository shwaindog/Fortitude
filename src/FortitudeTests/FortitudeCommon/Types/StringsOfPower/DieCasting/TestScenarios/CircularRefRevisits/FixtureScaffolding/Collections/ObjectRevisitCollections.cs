// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.Collections.ObjectCollectionRevisitInstances;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.Collections;

public static class ObjectCollectionRevisitInstances
{
    public static object NewObject(int number) => new MyOtherTypeClass($"new Object {number}");

    public static readonly object SingletonInst1 = new MyOtherTypeClass("singleton Object 1");
    public static readonly object SingletonInst2 = new MyOtherTypeClass("singleton Object 2");
    public static readonly object SingletonInst3 = new MyOtherTypeClass("singleton Object 3");
}

public class PreFieldObjectArrayStructUnionRevisit : IStringBearer
{
    private readonly Object? firstPreField = null;

    public PreFieldObjectArrayStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<object>? stringRevealer = null
      , PalantírReveal<ObjectOrArrayStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new ObjectOrArrayStructUnion([null, NewObject(1), NewObject(2), NewObject(3)]
                                                      , isSimple, isValue, stringRevealer);
        List<ObjectOrArrayStructUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((object?[]?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, SingletonInst2, SingletonInst3]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly ObjectOrArrayStructUnion[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAddObject(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Complete();
    }
}

public class ObjectArrayPostFieldStructUnionRevisit : IStringBearer
{
    private readonly Object? firstPostField = SingletonInst2;

    public ObjectArrayPostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<object>? stringRevealer = null
      , PalantírReveal<ObjectOrArrayStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new ObjectOrArrayStructUnion([SingletonInst1, SingletonInst2, SingletonInst3, null]
                                                      , isSimple, isValue, stringRevealer);
        List<ObjectOrArrayStructUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((object?[]?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, SingletonInst2, SingletonInst3]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly ObjectOrArrayStructUnion[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Field.AlwaysAddObject(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldObjectArrayClassUnionRevisit : IStringBearer
{
    private readonly Object? firstPreField = null;

    public PreFieldObjectArrayClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<object>? stringRevealer = null
      , PalantírReveal<ObjectOrArrayClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new ObjectOrArrayClassUnion([NewObject(1), null, NewObject(2), NewObject(3)]
                                                     , isSimple, isValue, stringRevealer);
        List<ObjectOrArrayClassUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((Object?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, SingletonInst2, SingletonInst3]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly ObjectOrArrayClassUnion[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAddObject(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Complete();
    }
}

public class ObjectArrayPostFieldClassUnionRevisit : IStringBearer
{
    private readonly Object? firstPostField = SingletonInst2;

    public ObjectArrayPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<object>? stringRevealer = null
      , PalantírReveal<ObjectOrArrayClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new ObjectOrArrayClassUnion([SingletonInst1, SingletonInst2, null, SingletonInst3]
                                                     , isSimple, isValue, stringRevealer);
        List<ObjectOrArrayClassUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((object?[]?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([NewObject(1), NewObject(2), NewObject(3), null]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly ObjectOrArrayClassUnion[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Field.AlwaysAddObject(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldObjectSpanClassUnionRevisit : IStringBearer
{
    private readonly Object? firstPreField = SingletonInst2;

    public PreFieldObjectSpanClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<object>? stringRevealer = null
      , PalantírReveal<ObjectOrSpanClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new ObjectOrSpanClassUnion([null, NewObject(1), NewObject(2), NewObject(3)]
                                                    , isSimple, isValue, stringRevealer);
        List<ObjectOrSpanClassUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((object?[]?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, SingletonInst2, SingletonInst3], isSimple, isValue, stringRevealer)
          , repeatedItem  
        ];
        firstSpan = firstAsList.ToArray();
    }

    private readonly ObjectOrSpanClassUnion[] firstSpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAddObject(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstSpan), firstSpan.AsSpan())
           .Complete();
    }
}

public class ObjectSpanPostFieldClassUnionRevisit : IStringBearer
{
    private readonly Object? firstPostField = null;

    public ObjectSpanPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<object>? stringRevealer = null
      , PalantírReveal<ObjectOrSpanClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new ObjectOrSpanClassUnion([SingletonInst1, SingletonInst2, SingletonInst3, null]
                                                    , isSimple, isValue, stringRevealer);
        List<ObjectOrSpanClassUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((Object?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([NewObject(1), null, NewObject(2), NewObject(3)]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstSpan = firstAsList.ToArray();
    }

    private readonly ObjectOrSpanClassUnion[] firstSpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstSpan), firstSpan.AsSpan())
           .Field.AlwaysAddObject(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldObjectReadOnlySpanClassUnionRevisit : IStringBearer
{
    private readonly Object? firstPreField = SingletonInst2;

    public PreFieldObjectReadOnlySpanClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<object>? stringRevealer = null
      , PalantírReveal<ObjectOrReadOnlySpanClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new ObjectOrReadOnlySpanClassUnion([NewObject(1), null, NewObject(2), null, NewObject(3)]
                                                            , isSimple, isValue, stringRevealer);
        List<ObjectOrReadOnlySpanClassUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((Object?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([null, SingletonInst1, SingletonInst2, SingletonInst3], isSimple, isValue, stringRevealer)
          , repeatedItem  
        ];
        firstReadOnlySpan = firstAsList.ToArray();
    }

    private readonly ObjectOrReadOnlySpanClassUnion[] firstReadOnlySpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAddObject(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstReadOnlySpan), (ReadOnlySpan<ObjectOrReadOnlySpanClassUnion>)firstReadOnlySpan)
           .Complete();
    }
}

public class ObjectReadOnlySpanPostFieldClassUnionRevisit : IStringBearer
{
    private readonly Object? firstPostField = null;

    public ObjectReadOnlySpanPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<object>? stringRevealer = null
      , PalantírReveal<ObjectOrReadOnlySpanClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new ObjectOrReadOnlySpanClassUnion([null, SingletonInst1, SingletonInst2, SingletonInst3]
                                                            , isSimple, isValue, stringRevealer);
        List<ObjectOrReadOnlySpanClassUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((object?[]?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([NewObject(1), null, NewObject(2), NewObject(3)]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstReadOnlySpan = firstAsList.ToArray();
    }

    private readonly ObjectOrReadOnlySpanClassUnion[] firstReadOnlySpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstReadOnlySpan), (ReadOnlySpan<ObjectOrReadOnlySpanClassUnion>)firstReadOnlySpan)
           .Field.AlwaysAddObject(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldObjectListStructUnionRevisit : IStringBearer
{
    private readonly Object? firstPreField = null;

    public PreFieldObjectListStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<object>? stringRevealer = null
      , PalantírReveal<ObjectOrListStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new ObjectOrListStructUnion([NewObject(1), NewObject(2), NewObject(3), null]
                                                     , isSimple, isValue, stringRevealer);
        List<ObjectOrListStructUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((Object?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, SingletonInst2, SingletonInst3], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<ObjectOrListStructUnion> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAddObject(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Complete();
    }
}

public class ObjectListPostFieldStructUnionRevisit : IStringBearer
{
    private readonly Object? firstPostField = SingletonInst2;

    public ObjectListPostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<object>? stringRevealer = null
      , PalantírReveal<ObjectOrArrayStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new ObjectOrListStructUnion([SingletonInst1, null, SingletonInst2, SingletonInst3]
                                                     , isSimple, isValue, stringRevealer);
        List<ObjectOrListStructUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((List<object?>?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([NewObject(1), NewObject(2), null, NewObject(3)]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<ObjectOrListStructUnion> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Field.AlwaysAddObject(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldObjectListClassUnionRevisit : IStringBearer
{
    private readonly Object? firstPreField = SingletonInst2;

    public PreFieldObjectListClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<object>? stringRevealer = null
      , PalantírReveal<ObjectOrListClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new ObjectOrListClassUnion([NewObject(1), NewObject(2), null, NewObject(3)]
                                                    , isSimple, isValue, stringRevealer);
        List<ObjectOrListClassUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((Object?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, null, SingletonInst2, SingletonInst3], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<ObjectOrListClassUnion> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAddObject(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Complete();
    }
}

public class ObjectListPostFieldClassUnionRevisit : IStringBearer
{
    private readonly Object? firstPostField = null;

    public ObjectListPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<object>? stringRevealer = null
      , PalantírReveal<ObjectOrListClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new ObjectOrListClassUnion([null, SingletonInst1, SingletonInst2, SingletonInst3]
                                                    , isSimple, isValue, stringRevealer);
        List<ObjectOrListClassUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((List<object?>?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([NewObject(1), NewObject(2), NewObject(3), null]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<ObjectOrListClassUnion> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Field.AlwaysAddObject(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldObjectEnumerableStructUnionRevisit : IStringBearer
{
    private readonly Object? firstPreField = null;

    public PreFieldObjectEnumerableStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<object>? stringRevealer = null
      , PalantírReveal<ObjectOrEnumerableStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new ObjectOrEnumerableStructUnion([NewObject(1), null, NewObject(2), NewObject(3)]
                                                           , isSimple, isValue, stringRevealer);
        List<ObjectOrEnumerableStructUnion> firstAsEnumerable = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((List<object?>?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, SingletonInst2, null, SingletonInst3], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly IEnumerable<ObjectOrEnumerableStructUnion> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAddObject(nameof(firstPreField), firstPreField)
           .CollectionField
           .AlwaysRevealAllEnumerate<IEnumerable<ObjectOrEnumerableStructUnion>, ObjectOrEnumerableStructUnion>(nameof(firstEnumerable), firstEnumerable)
           .Complete();
    }
}

public class ObjectEnumerablePostFieldStructUnionRevisit : IStringBearer
{
    private readonly Object? firstPostField = SingletonInst2;

    public ObjectEnumerablePostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<object>? stringRevealer = null
      , PalantírReveal<ObjectOrArrayStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new ObjectOrEnumerableStructUnion([SingletonInst1, SingletonInst2, null, SingletonInst3]
                                                           , isSimple, isValue, stringRevealer);
        List<ObjectOrEnumerableStructUnion> firstAsEnumerable = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((List<object?>?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([NewObject(1), null, NewObject(2), NewObject(3)], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly List<ObjectOrEnumerableStructUnion> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField
           .AlwaysRevealAllEnumerate<List<ObjectOrEnumerableStructUnion>, ObjectOrEnumerableStructUnion>(nameof(firstEnumerable), firstEnumerable)
           .Field.AlwaysAddObject(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldObjectEnumerableClassUnionRevisit : IStringBearer
{
    private readonly Object? firstPreField = SingletonInst2;

    public PreFieldObjectEnumerableClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<object>? stringRevealer = null
      , PalantírReveal<ObjectOrEnumerableClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new ObjectOrEnumerableClassUnion([null, NewObject(1), NewObject(2), NewObject(3)]
                                                          , isSimple, isValue, stringRevealer);
        List<ObjectOrEnumerableClassUnion> firstAsEnumerable = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((Object?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, SingletonInst2, SingletonInst3, null], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly List<ObjectOrEnumerableClassUnion> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAddObject(nameof(firstPreField), firstPreField)
           .CollectionField
           .AlwaysRevealAllEnumerate<List<ObjectOrEnumerableClassUnion>, ObjectOrEnumerableClassUnion>
               (nameof(firstEnumerable), firstEnumerable)
           .Complete();
    }
}

public class ObjectEnumerablePostFieldClassUnionRevisit : IStringBearer
{
    private readonly Object? firstPostField = null;

    public ObjectEnumerablePostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<object>? stringRevealer = null
      , PalantírReveal<ObjectOrEnumerableClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new ObjectOrEnumerableClassUnion([SingletonInst1, null, SingletonInst2, SingletonInst3]
                                                          , isSimple, isValue, stringRevealer);
        List<ObjectOrEnumerableClassUnion> firstAsEnumerable = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((List<object?>?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([NewObject(1), NewObject(2), null, NewObject(3)]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly List<ObjectOrEnumerableClassUnion> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField
           .AlwaysRevealAllEnumerate<List<ObjectOrEnumerableClassUnion>, ObjectOrEnumerableClassUnion>
               (nameof(firstEnumerable), firstEnumerable)
           .Field.AlwaysAddObject(nameof(firstPostField), firstPostField)
           .Complete();
    }
}


public class PreFieldObjectEnumeratorStructUnionRevisit : IStringBearer
{
    private readonly Object? firstPreField = null;

    public PreFieldObjectEnumeratorStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<object>? stringRevealer = null
      , PalantírReveal<ObjectOrEnumeratorStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new ObjectOrEnumeratorStructUnion([NewObject(1), NewObject(2), null, NewObject(3)]
                                                           , isSimple, isValue, stringRevealer);
        List<ObjectOrEnumeratorStructUnion> firstAsEnumerator = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((Object?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, null, SingletonInst2, SingletonInst3], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<ObjectOrEnumeratorStructUnion>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<ObjectOrEnumeratorStructUnion>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .Field.AlwaysAddObject(nameof(firstPreField), firstPreField)
           .CollectionField
           .AlwaysRevealAllIterate<List<ObjectOrEnumeratorStructUnion>.Enumerator, ObjectOrEnumeratorStructUnion>
               (nameof(firstEnumerator), firstEnumerator)
           .Complete();
    }
}

public class ObjectEnumeratorPostFieldStructUnionRevisit : IStringBearer
{
    private readonly Object? firstPostField = SingletonInst2;

    public ObjectEnumeratorPostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<object>? stringRevealer = null
      , PalantírReveal<ObjectOrArrayStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new ObjectOrEnumeratorStructUnion([null, SingletonInst1, SingletonInst2, SingletonInst3]
                                                           , isSimple, isValue, stringRevealer);
        List<ObjectOrEnumeratorStructUnion> firstAsEnumerator = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((List<object?>?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([NewObject(1), NewObject(2), NewObject(3), null], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<ObjectOrEnumeratorStructUnion>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<ObjectOrEnumeratorStructUnion>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .CollectionField
           .AlwaysRevealAllIterate<List<ObjectOrEnumeratorStructUnion>.Enumerator, ObjectOrEnumeratorStructUnion>
               (nameof(firstEnumerator), firstEnumerator)
           .Field.AlwaysAddObject(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldObjectEnumeratorClassUnionRevisit : IStringBearer
{
    private readonly Object? firstPreField = SingletonInst2;

    public PreFieldObjectEnumeratorClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<object>? stringRevealer = null
      , PalantírReveal<ObjectOrEnumeratorClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new ObjectOrEnumeratorClassUnion([NewObject(1), null, NewObject(2), NewObject(3)]
                                                          , isSimple, isValue, stringRevealer);
        List<ObjectOrEnumeratorClassUnion> firstAsEnumerator = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((Object?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, SingletonInst2, null, SingletonInst3], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<ObjectOrEnumeratorClassUnion>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<ObjectOrEnumeratorClassUnion>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .Field.AlwaysAddObject(nameof(firstPreField), firstPreField)
           .CollectionField
           .AlwaysRevealAllIterate<List<ObjectOrEnumeratorClassUnion>.Enumerator, ObjectOrEnumeratorClassUnion>
               (nameof(firstEnumerator), firstEnumerator)
           .Complete();
    }
}

public class ObjectEnumeratorPostFieldClassUnionRevisit : IStringBearer
{
    private readonly Object? firstPostField = null;

    public ObjectEnumeratorPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<object>? stringRevealer = null
      , PalantírReveal<ObjectOrEnumeratorClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new ObjectOrEnumeratorClassUnion([SingletonInst1, SingletonInst2, null, SingletonInst3]
                                                          , isSimple, isValue, stringRevealer);
        List<ObjectOrEnumeratorClassUnion> firstAsEnumerator = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((List<object?>?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([NewObject(1), null, NewObject(2), NewObject(3)], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<ObjectOrEnumeratorClassUnion>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<ObjectOrEnumeratorClassUnion>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .CollectionField
           .AlwaysRevealAllIterate<List<ObjectOrEnumeratorClassUnion>.Enumerator, ObjectOrEnumeratorClassUnion>
               (nameof(firstEnumerator), firstEnumerator)
           .Field.AlwaysAddObject(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

