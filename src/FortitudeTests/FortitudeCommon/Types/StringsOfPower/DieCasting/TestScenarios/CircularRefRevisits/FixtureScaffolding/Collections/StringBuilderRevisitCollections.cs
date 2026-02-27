// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.Collections.StringBuilderCollectionRevisitInstances;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.Collections;

public static class StringBuilderCollectionRevisitInstances
{
    public static StringBuilder NewStringBuilder(int number) => new ($"new StringBuilder {number}");

    public static readonly StringBuilder SingletonInst1  = new ("singleton StringBuilder 1");
    public static readonly StringBuilder SingletonInst2  = new ("singleton StringBuilder 2");
    public static readonly StringBuilder SingletonInst3  = new ("singleton StringBuilder 3");
}

public class PreFieldStringBuilderArrayStructUnionRevisit : IStringBearer
{
    private readonly StringBuilder? firstPreField = null;

    public PreFieldStringBuilderArrayStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<StringBuilder>? stringRevealer = null
      , PalantírReveal<StringBuilderOrArrayStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringBuilderOrArrayStructUnion([null, NewStringBuilder(1), NewStringBuilder(2), NewStringBuilder(3)]
                                                      , isSimple, isValue, stringRevealer);
        List<StringBuilderOrArrayStructUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((StringBuilder?[]?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, SingletonInst2, SingletonInst3]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly StringBuilderOrArrayStructUnion[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Complete();
    }
}

public class StringBuilderArrayPostFieldStructUnionRevisit : IStringBearer
{
    private readonly StringBuilder? firstPostField = SingletonInst2;

    public StringBuilderArrayPostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<StringBuilder>? stringRevealer = null
      , PalantírReveal<StringBuilderOrArrayStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringBuilderOrArrayStructUnion([SingletonInst1, SingletonInst2, SingletonInst3, null]
                                                      , isSimple, isValue, stringRevealer);
        List<StringBuilderOrArrayStructUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((StringBuilder?[]?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, SingletonInst2, SingletonInst3]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly StringBuilderOrArrayStructUnion[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldStringBuilderArrayClassUnionRevisit : IStringBearer
{
    private readonly StringBuilder? firstPreField = null;

    public PreFieldStringBuilderArrayClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<StringBuilder>? stringRevealer = null
      , PalantírReveal<StringBuilderOrArrayClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringBuilderOrArrayClassUnion([NewStringBuilder(1), null, NewStringBuilder(2), NewStringBuilder(3)]
                                                     , isSimple, isValue, stringRevealer);
        List<StringBuilderOrArrayClassUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((StringBuilder?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, SingletonInst2, SingletonInst3]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly StringBuilderOrArrayClassUnion[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Complete();
    }
}

public class StringBuilderArrayPostFieldClassUnionRevisit : IStringBearer
{
    private readonly StringBuilder? firstPostField = SingletonInst2;

    public StringBuilderArrayPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<StringBuilder>? stringRevealer = null
      , PalantírReveal<StringBuilderOrArrayClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringBuilderOrArrayClassUnion([SingletonInst1, SingletonInst2, null, SingletonInst3]
                                                     , isSimple, isValue, stringRevealer);
        List<StringBuilderOrArrayClassUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((StringBuilder?[]?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([NewStringBuilder(1), NewStringBuilder(2), NewStringBuilder(3), null]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly StringBuilderOrArrayClassUnion[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldStringBuilderSpanClassUnionRevisit : IStringBearer
{
    private readonly StringBuilder? firstPreField = SingletonInst2;

    public PreFieldStringBuilderSpanClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<StringBuilder>? stringRevealer = null
      , PalantírReveal<StringBuilderOrSpanClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringBuilderOrSpanClassUnion([null, NewStringBuilder(1), NewStringBuilder(2), NewStringBuilder(3)]
                                                    , isSimple, isValue, stringRevealer);
        List<StringBuilderOrSpanClassUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((StringBuilder?[]?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, SingletonInst2, SingletonInst3], isSimple, isValue, stringRevealer)
          , repeatedItem  
        ];
        firstSpan = firstAsList.ToArray();
    }

    private readonly StringBuilderOrSpanClassUnion[] firstSpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstSpan), firstSpan.AsSpan())
           .Complete();
    }
}

public class StringBuilderSpanPostFieldClassUnionRevisit : IStringBearer
{
    private readonly StringBuilder? firstPostField = null;

    public StringBuilderSpanPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<StringBuilder>? stringRevealer = null
      , PalantírReveal<StringBuilderOrSpanClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringBuilderOrSpanClassUnion([SingletonInst1, SingletonInst2, SingletonInst3, null]
                                                    , isSimple, isValue, stringRevealer);
        List<StringBuilderOrSpanClassUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((StringBuilder?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([NewStringBuilder(1), null, NewStringBuilder(2), NewStringBuilder(3)]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstSpan = firstAsList.ToArray();
    }

    private readonly StringBuilderOrSpanClassUnion[] firstSpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstSpan), firstSpan.AsSpan())
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldStringBuilderReadOnlySpanClassUnionRevisit : IStringBearer
{
    private readonly StringBuilder? firstPreField = SingletonInst2;

    public PreFieldStringBuilderReadOnlySpanClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<StringBuilder>? stringRevealer = null
      , PalantírReveal<StringBuilderOrReadOnlySpanClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringBuilderOrReadOnlySpanClassUnion([NewStringBuilder(1), null, NewStringBuilder(2), null, NewStringBuilder(3)]
                                                            , isSimple, isValue, stringRevealer);
        List<StringBuilderOrReadOnlySpanClassUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((StringBuilder?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([null, SingletonInst1, SingletonInst2, SingletonInst3], isSimple, isValue, stringRevealer)
          , repeatedItem  
        ];
        firstReadOnlySpan = firstAsList.ToArray();
    }

    private readonly StringBuilderOrReadOnlySpanClassUnion[] firstReadOnlySpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstReadOnlySpan), (ReadOnlySpan<StringBuilderOrReadOnlySpanClassUnion>)firstReadOnlySpan)
           .Complete();
    }
}

public class StringBuilderReadOnlySpanPostFieldClassUnionRevisit : IStringBearer
{
    private readonly StringBuilder? firstPostField = null;

    public StringBuilderReadOnlySpanPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<StringBuilder>? stringRevealer = null
      , PalantírReveal<StringBuilderOrReadOnlySpanClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringBuilderOrReadOnlySpanClassUnion([null, SingletonInst1, SingletonInst2, SingletonInst3]
                                                            , isSimple, isValue, stringRevealer);
        List<StringBuilderOrReadOnlySpanClassUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((StringBuilder?[]?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([NewStringBuilder(1), null, NewStringBuilder(2), NewStringBuilder(3)]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstReadOnlySpan = firstAsList.ToArray();
    }

    private readonly StringBuilderOrReadOnlySpanClassUnion[] firstReadOnlySpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstReadOnlySpan), (ReadOnlySpan<StringBuilderOrReadOnlySpanClassUnion>)firstReadOnlySpan)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldStringBuilderListStructUnionRevisit : IStringBearer
{
    private readonly StringBuilder? firstPreField = null;

    public PreFieldStringBuilderListStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<StringBuilder>? stringRevealer = null
      , PalantírReveal<StringBuilderOrListStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringBuilderOrListStructUnion([NewStringBuilder(1), NewStringBuilder(2), NewStringBuilder(3), null]
                                                     , isSimple, isValue, stringRevealer);
        List<StringBuilderOrListStructUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((StringBuilder?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, SingletonInst2, SingletonInst3], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<StringBuilderOrListStructUnion> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Complete();
    }
}

public class StringBuilderListPostFieldStructUnionRevisit : IStringBearer
{
    private readonly StringBuilder? firstPostField = SingletonInst2;

    public StringBuilderListPostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<StringBuilder>? stringRevealer = null
      , PalantírReveal<StringBuilderOrArrayStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringBuilderOrListStructUnion([SingletonInst1, null, SingletonInst2, SingletonInst3]
                                                     , isSimple, isValue, stringRevealer);
        List<StringBuilderOrListStructUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((List<StringBuilder?>?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([NewStringBuilder(1), NewStringBuilder(2), null, NewStringBuilder(3)]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<StringBuilderOrListStructUnion> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldStringBuilderListClassUnionRevisit : IStringBearer
{
    private readonly StringBuilder? firstPreField = SingletonInst2;

    public PreFieldStringBuilderListClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<StringBuilder>? stringRevealer = null
      , PalantírReveal<StringBuilderOrListClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringBuilderOrListClassUnion([NewStringBuilder(1), NewStringBuilder(2), null, NewStringBuilder(3)]
                                                    , isSimple, isValue, stringRevealer);
        List<StringBuilderOrListClassUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((StringBuilder?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, null, SingletonInst2, SingletonInst3], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<StringBuilderOrListClassUnion> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Complete();
    }
}

public class StringBuilderListPostFieldClassUnionRevisit : IStringBearer
{
    private readonly StringBuilder? firstPostField = null;

    public StringBuilderListPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<StringBuilder>? stringRevealer = null
      , PalantírReveal<StringBuilderOrListClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringBuilderOrListClassUnion([null, SingletonInst1, SingletonInst2, SingletonInst3]
                                                    , isSimple, isValue, stringRevealer);
        List<StringBuilderOrListClassUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((List<StringBuilder?>?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([NewStringBuilder(1), NewStringBuilder(2), NewStringBuilder(3), null]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<StringBuilderOrListClassUnion> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldStringBuilderEnumerableStructUnionRevisit : IStringBearer
{
    private readonly StringBuilder? firstPreField = null;

    public PreFieldStringBuilderEnumerableStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<StringBuilder>? stringRevealer = null
      , PalantírReveal<StringBuilderOrEnumerableStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringBuilderOrEnumerableStructUnion([NewStringBuilder(1), null, NewStringBuilder(2), NewStringBuilder(3)]
                                                           , isSimple, isValue, stringRevealer);
        List<StringBuilderOrEnumerableStructUnion> firstAsEnumerable = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((List<StringBuilder?>?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, SingletonInst2, null, SingletonInst3], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly IEnumerable<StringBuilderOrEnumerableStructUnion> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField
           .AlwaysRevealAllEnumerate<IEnumerable<StringBuilderOrEnumerableStructUnion>, StringBuilderOrEnumerableStructUnion>(nameof(firstEnumerable), firstEnumerable)
           .Complete();
    }
}

public class StringBuilderEnumerablePostFieldStructUnionRevisit : IStringBearer
{
    private readonly StringBuilder? firstPostField = SingletonInst2;

    public StringBuilderEnumerablePostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<StringBuilder>? stringRevealer = null
      , PalantírReveal<StringBuilderOrArrayStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringBuilderOrEnumerableStructUnion([SingletonInst1, SingletonInst2, null, SingletonInst3]
                                                           , isSimple, isValue, stringRevealer);
        List<StringBuilderOrEnumerableStructUnion> firstAsEnumerable = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((List<StringBuilder?>?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([NewStringBuilder(1), null, NewStringBuilder(2), NewStringBuilder(3)], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly List<StringBuilderOrEnumerableStructUnion> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField
           .AlwaysRevealAllEnumerate<List<StringBuilderOrEnumerableStructUnion>, StringBuilderOrEnumerableStructUnion>(nameof(firstEnumerable), firstEnumerable)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldStringBuilderEnumerableClassUnionRevisit : IStringBearer
{
    private readonly StringBuilder? firstPreField = SingletonInst2;

    public PreFieldStringBuilderEnumerableClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<StringBuilder>? stringRevealer = null
      , PalantírReveal<StringBuilderOrEnumerableClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringBuilderOrEnumerableClassUnion([null, NewStringBuilder(1), NewStringBuilder(2), NewStringBuilder(3)]
                                                          , isSimple, isValue, stringRevealer);
        List<StringBuilderOrEnumerableClassUnion> firstAsEnumerable = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((StringBuilder?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, SingletonInst2, SingletonInst3, null], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly List<StringBuilderOrEnumerableClassUnion> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField
           .AlwaysRevealAllEnumerate<List<StringBuilderOrEnumerableClassUnion>, StringBuilderOrEnumerableClassUnion>
               (nameof(firstEnumerable), firstEnumerable)
           .Complete();
    }
}

public class StringBuilderEnumerablePostFieldClassUnionRevisit : IStringBearer
{
    private readonly StringBuilder? firstPostField = null;

    public StringBuilderEnumerablePostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<StringBuilder>? stringRevealer = null
      , PalantírReveal<StringBuilderOrEnumerableClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringBuilderOrEnumerableClassUnion([SingletonInst1, null, SingletonInst2, SingletonInst3]
                                                          , isSimple, isValue, stringRevealer);
        List<StringBuilderOrEnumerableClassUnion> firstAsEnumerable = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((List<StringBuilder?>?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([NewStringBuilder(1), NewStringBuilder(2), null, NewStringBuilder(3)]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly List<StringBuilderOrEnumerableClassUnion> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField
           .AlwaysRevealAllEnumerate<List<StringBuilderOrEnumerableClassUnion>, StringBuilderOrEnumerableClassUnion>
               (nameof(firstEnumerable), firstEnumerable)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}


public class PreFieldStringBuilderEnumeratorStructUnionRevisit : IStringBearer
{
    private readonly StringBuilder? firstPreField = null;

    public PreFieldStringBuilderEnumeratorStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<StringBuilder>? stringRevealer = null
      , PalantírReveal<StringBuilderOrEnumeratorStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringBuilderOrEnumeratorStructUnion([NewStringBuilder(1), NewStringBuilder(2), null, NewStringBuilder(3)]
                                                           , isSimple, isValue, stringRevealer);
        List<StringBuilderOrEnumeratorStructUnion> firstAsEnumerator = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((StringBuilder?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, null, SingletonInst2, SingletonInst3], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<StringBuilderOrEnumeratorStructUnion>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<StringBuilderOrEnumeratorStructUnion>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField
           .AlwaysRevealAllIterate<List<StringBuilderOrEnumeratorStructUnion>.Enumerator, StringBuilderOrEnumeratorStructUnion>
               (nameof(firstEnumerator), firstEnumerator)
           .Complete();
    }
}

public class StringBuilderEnumeratorPostFieldStructUnionRevisit : IStringBearer
{
    private readonly StringBuilder? firstPostField = SingletonInst2;

    public StringBuilderEnumeratorPostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<StringBuilder>? stringRevealer = null
      , PalantírReveal<StringBuilderOrArrayStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringBuilderOrEnumeratorStructUnion([null, SingletonInst1, SingletonInst2, SingletonInst3]
                                                           , isSimple, isValue, stringRevealer);
        List<StringBuilderOrEnumeratorStructUnion> firstAsEnumerator = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((List<StringBuilder?>?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([NewStringBuilder(1), NewStringBuilder(2), NewStringBuilder(3), null], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<StringBuilderOrEnumeratorStructUnion>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<StringBuilderOrEnumeratorStructUnion>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .CollectionField
           .AlwaysRevealAllIterate<List<StringBuilderOrEnumeratorStructUnion>.Enumerator, StringBuilderOrEnumeratorStructUnion>
               (nameof(firstEnumerator), firstEnumerator)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldStringBuilderEnumeratorClassUnionRevisit : IStringBearer
{
    private readonly StringBuilder? firstPreField = SingletonInst2;

    public PreFieldStringBuilderEnumeratorClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<StringBuilder>? stringRevealer = null
      , PalantírReveal<StringBuilderOrEnumeratorClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringBuilderOrEnumeratorClassUnion([NewStringBuilder(1), null, NewStringBuilder(2), NewStringBuilder(3)]
                                                          , isSimple, isValue, stringRevealer);
        List<StringBuilderOrEnumeratorClassUnion> firstAsEnumerator = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((StringBuilder?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, SingletonInst2, null, SingletonInst3], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<StringBuilderOrEnumeratorClassUnion>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<StringBuilderOrEnumeratorClassUnion>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField
           .AlwaysRevealAllIterate<List<StringBuilderOrEnumeratorClassUnion>.Enumerator, StringBuilderOrEnumeratorClassUnion>
               (nameof(firstEnumerator), firstEnumerator)
           .Complete();
    }
}

public class StringBuilderEnumeratorPostFieldClassUnionRevisit : IStringBearer
{
    private readonly StringBuilder? firstPostField = null;

    public StringBuilderEnumeratorPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<StringBuilder>? stringRevealer = null
      , PalantírReveal<StringBuilderOrEnumeratorClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringBuilderOrEnumeratorClassUnion([SingletonInst1, SingletonInst2, null, SingletonInst3]
                                                          , isSimple, isValue, stringRevealer);
        List<StringBuilderOrEnumeratorClassUnion> firstAsEnumerator = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((List<StringBuilder?>?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([NewStringBuilder(1), null, NewStringBuilder(2), NewStringBuilder(3)], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<StringBuilderOrEnumeratorClassUnion>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<StringBuilderOrEnumeratorClassUnion>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .CollectionField
           .AlwaysRevealAllIterate<List<StringBuilderOrEnumeratorClassUnion>.Enumerator, StringBuilderOrEnumeratorClassUnion>
               (nameof(firstEnumerator), firstEnumerator)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

