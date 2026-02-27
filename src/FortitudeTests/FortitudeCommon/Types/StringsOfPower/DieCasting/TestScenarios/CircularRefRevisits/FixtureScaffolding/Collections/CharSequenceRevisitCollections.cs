// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.Collections.CharSequenceCollectionRevisitInstances;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.Collections;

public static class CharSequenceCollectionRevisitInstances
{
    public static ICharSequence NewCharSequence(int number) => number % 2 == 0 
        ? new MutableString($"new CharSequence {number}")
        : new CharArrayStringBuilder($"new CharSequence {number}");

    public static readonly ICharSequence SingletonInst1  = new CharArrayStringBuilder("singleton CharSequence 1");
    public static readonly ICharSequence SingletonInst2  = new MutableString("singleton CharSequence 2");
    public static readonly ICharSequence SingletonInst3  = new CharArrayStringBuilder("singleton CharSequence 3");
}

public class PreFieldCharSequenceArrayStructUnionRevisit : IStringBearer
{
    private readonly ICharSequence? firstPreField = null;

    public PreFieldCharSequenceArrayStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<ICharSequence>? stringRevealer = null
      , PalantírReveal<CharSeqOrArrayStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new CharSeqOrArrayStructUnion([null, NewCharSequence(1), NewCharSequence(2), NewCharSequence(3)]
                                                      , isSimple, isValue, stringRevealer);
        List<CharSeqOrArrayStructUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((ICharSequence?[]?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, SingletonInst2, SingletonInst3]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly CharSeqOrArrayStructUnion[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAddCharSeq(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Complete();
    }
}

public class CharSequenceArrayPostFieldStructUnionRevisit : IStringBearer
{
    private readonly ICharSequence? firstPostField = SingletonInst2;

    public CharSequenceArrayPostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<ICharSequence>? stringRevealer = null
      , PalantírReveal<CharSeqOrArrayStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new CharSeqOrArrayStructUnion([SingletonInst1, SingletonInst2, SingletonInst3, null]
                                                      , isSimple, isValue, stringRevealer);
        List<CharSeqOrArrayStructUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((ICharSequence?[]?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, SingletonInst2, SingletonInst3]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly CharSeqOrArrayStructUnion[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Field.AlwaysAddCharSeq(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldCharSequenceArrayClassUnionRevisit : IStringBearer
{
    private readonly ICharSequence? firstPreField = null;

    public PreFieldCharSequenceArrayClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<ICharSequence>? stringRevealer = null
      , PalantírReveal<CharSeqOrArrayClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new CharSeqOrArrayClassUnion([NewCharSequence(1), null, NewCharSequence(2), NewCharSequence(3)]
                                                     , isSimple, isValue, stringRevealer);
        List<CharSeqOrArrayClassUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((ICharSequence?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, SingletonInst2, SingletonInst3]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly CharSeqOrArrayClassUnion[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAddCharSeq(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Complete();
    }
}

public class CharSequenceArrayPostFieldClassUnionRevisit : IStringBearer
{
    private readonly ICharSequence? firstPostField = SingletonInst2;

    public CharSequenceArrayPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<ICharSequence>? stringRevealer = null
      , PalantírReveal<CharSeqOrArrayClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new CharSeqOrArrayClassUnion([SingletonInst1, SingletonInst2, null, SingletonInst3]
                                                     , isSimple, isValue, stringRevealer);
        List<CharSeqOrArrayClassUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((ICharSequence?[]?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([NewCharSequence(1), NewCharSequence(2), NewCharSequence(3), null]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly CharSeqOrArrayClassUnion[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Field.AlwaysAddCharSeq(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldCharSequenceSpanClassUnionRevisit : IStringBearer
{
    private readonly ICharSequence? firstPreField = SingletonInst2;

    public PreFieldCharSequenceSpanClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<ICharSequence>? stringRevealer = null
      , PalantírReveal<CharSeqOrSpanClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new CharSeqOrSpanClassUnion([null, NewCharSequence(1), NewCharSequence(2), NewCharSequence(3)]
                                                    , isSimple, isValue, stringRevealer);
        List<CharSeqOrSpanClassUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((ICharSequence?[]?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, SingletonInst2, SingletonInst3], isSimple, isValue, stringRevealer)
          , repeatedItem  
        ];
        firstSpan = firstAsList.ToArray();
    }

    private readonly CharSeqOrSpanClassUnion[] firstSpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAddCharSeq(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstSpan), firstSpan.AsSpan())
           .Complete();
    }
}

public class CharSequenceSpanPostFieldClassUnionRevisit : IStringBearer
{
    private readonly ICharSequence? firstPostField = null;

    public CharSequenceSpanPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<ICharSequence>? stringRevealer = null
      , PalantírReveal<CharSeqOrSpanClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new CharSeqOrSpanClassUnion([SingletonInst1, SingletonInst2, SingletonInst3, null]
                                                    , isSimple, isValue, stringRevealer);
        List<CharSeqOrSpanClassUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((ICharSequence?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([NewCharSequence(1), null, NewCharSequence(2), NewCharSequence(3)]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstSpan = firstAsList.ToArray();
    }

    private readonly CharSeqOrSpanClassUnion[] firstSpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstSpan), firstSpan.AsSpan())
           .Field.AlwaysAddCharSeq(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldCharSequenceReadOnlySpanClassUnionRevisit : IStringBearer
{
    private readonly ICharSequence? firstPreField = SingletonInst2;

    public PreFieldCharSequenceReadOnlySpanClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<ICharSequence>? stringRevealer = null
      , PalantírReveal<CharSeqOrReadOnlySpanClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new CharSeqOrReadOnlySpanClassUnion([NewCharSequence(1), null, NewCharSequence(2), null, NewCharSequence(3)]
                                                            , isSimple, isValue, stringRevealer);
        List<CharSeqOrReadOnlySpanClassUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((ICharSequence?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([null, SingletonInst1, SingletonInst2, SingletonInst3], isSimple, isValue, stringRevealer)
          , repeatedItem  
        ];
        firstReadOnlySpan = firstAsList.ToArray();
    }

    private readonly CharSeqOrReadOnlySpanClassUnion[] firstReadOnlySpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAddCharSeq(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstReadOnlySpan), (ReadOnlySpan<CharSeqOrReadOnlySpanClassUnion>)firstReadOnlySpan)
           .Complete();
    }
}

public class CharSequenceReadOnlySpanPostFieldClassUnionRevisit : IStringBearer
{
    private readonly ICharSequence? firstPostField = null;

    public CharSequenceReadOnlySpanPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<ICharSequence>? stringRevealer = null
      , PalantírReveal<CharSeqOrReadOnlySpanClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new CharSeqOrReadOnlySpanClassUnion([null, SingletonInst1, SingletonInst2, SingletonInst3]
                                                            , isSimple, isValue, stringRevealer);
        List<CharSeqOrReadOnlySpanClassUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((ICharSequence?[]?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([NewCharSequence(1), null, NewCharSequence(2), NewCharSequence(3)]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstReadOnlySpan = firstAsList.ToArray();
    }

    private readonly CharSeqOrReadOnlySpanClassUnion[] firstReadOnlySpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstReadOnlySpan), (ReadOnlySpan<CharSeqOrReadOnlySpanClassUnion>)firstReadOnlySpan)
           .Field.AlwaysAddCharSeq(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldCharSequenceListStructUnionRevisit : IStringBearer
{
    private readonly ICharSequence? firstPreField = null;

    public PreFieldCharSequenceListStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<ICharSequence>? stringRevealer = null
      , PalantírReveal<CharSeqOrListStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new CharSeqOrListStructUnion([NewCharSequence(1), NewCharSequence(2), NewCharSequence(3), null]
                                                     , isSimple, isValue, stringRevealer);
        List<CharSeqOrListStructUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((ICharSequence?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, SingletonInst2, SingletonInst3], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<CharSeqOrListStructUnion> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAddCharSeq(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Complete();
    }
}

public class CharSequenceListPostFieldStructUnionRevisit : IStringBearer
{
    private readonly ICharSequence? firstPostField = SingletonInst2;

    public CharSequenceListPostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<ICharSequence>? stringRevealer = null
      , PalantírReveal<CharSeqOrArrayStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new CharSeqOrListStructUnion([SingletonInst1, null, SingletonInst2, SingletonInst3]
                                                     , isSimple, isValue, stringRevealer);
        List<CharSeqOrListStructUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((List<ICharSequence?>?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([NewCharSequence(1), NewCharSequence(2), null, NewCharSequence(3)]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<CharSeqOrListStructUnion> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Field.AlwaysAddCharSeq(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldCharSequenceListClassUnionRevisit : IStringBearer
{
    private readonly ICharSequence? firstPreField = SingletonInst2;

    public PreFieldCharSequenceListClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<ICharSequence>? stringRevealer = null
      , PalantírReveal<CharSeqOrListClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new CharSeqOrListClassUnion([NewCharSequence(1), NewCharSequence(2), null, NewCharSequence(3)]
                                                    , isSimple, isValue, stringRevealer);
        List<CharSeqOrListClassUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((ICharSequence?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, null, SingletonInst2, SingletonInst3], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<CharSeqOrListClassUnion> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAddCharSeq(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Complete();
    }
}

public class CharSequenceListPostFieldClassUnionRevisit : IStringBearer
{
    private readonly ICharSequence? firstPostField = null;

    public CharSequenceListPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<ICharSequence>? stringRevealer = null
      , PalantírReveal<CharSeqOrListClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new CharSeqOrListClassUnion([null, SingletonInst1, SingletonInst2, SingletonInst3]
                                                    , isSimple, isValue, stringRevealer);
        List<CharSeqOrListClassUnion> firstAsList = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((List<ICharSequence?>?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([NewCharSequence(1), NewCharSequence(2), NewCharSequence(3), null]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<CharSeqOrListClassUnion> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Field.AlwaysAddCharSeq(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldCharSequenceEnumerableStructUnionRevisit : IStringBearer
{
    private readonly ICharSequence? firstPreField = null;

    public PreFieldCharSequenceEnumerableStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<ICharSequence>? stringRevealer = null
      , PalantírReveal<CharSeqOrEnumerableStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new CharSeqOrEnumerableStructUnion([NewCharSequence(1), null, NewCharSequence(2), NewCharSequence(3)]
                                                           , isSimple, isValue, stringRevealer);
        List<CharSeqOrEnumerableStructUnion> firstAsEnumerable = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((List<ICharSequence?>?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, SingletonInst2, null, SingletonInst3], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly IEnumerable<CharSeqOrEnumerableStructUnion> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAddCharSeq(nameof(firstPreField), firstPreField)
           .CollectionField
           .AlwaysRevealAllEnumerate<IEnumerable<CharSeqOrEnumerableStructUnion>, CharSeqOrEnumerableStructUnion>(nameof(firstEnumerable), firstEnumerable)
           .Complete();
    }
}

public class CharSequenceEnumerablePostFieldStructUnionRevisit : IStringBearer
{
    private readonly ICharSequence? firstPostField = SingletonInst2;

    public CharSequenceEnumerablePostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<ICharSequence>? stringRevealer = null
      , PalantírReveal<CharSeqOrArrayStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new CharSeqOrEnumerableStructUnion([SingletonInst1, SingletonInst2, null, SingletonInst3]
                                                           , isSimple, isValue, stringRevealer);
        List<CharSeqOrEnumerableStructUnion> firstAsEnumerable = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((List<ICharSequence?>?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([NewCharSequence(1), null, NewCharSequence(2), NewCharSequence(3)], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly List<CharSeqOrEnumerableStructUnion> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField
           .AlwaysRevealAllEnumerate<List<CharSeqOrEnumerableStructUnion>, CharSeqOrEnumerableStructUnion>(nameof(firstEnumerable), firstEnumerable)
           .Field.AlwaysAddCharSeq(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldCharSequenceEnumerableClassUnionRevisit : IStringBearer
{
    private readonly ICharSequence? firstPreField = SingletonInst2;

    public PreFieldCharSequenceEnumerableClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<ICharSequence>? stringRevealer = null
      , PalantírReveal<CharSeqOrEnumerableClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new CharSeqOrEnumerableClassUnion([null, NewCharSequence(1), NewCharSequence(2), NewCharSequence(3)]
                                                          , isSimple, isValue, stringRevealer);
        List<CharSeqOrEnumerableClassUnion> firstAsEnumerable = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((ICharSequence?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, SingletonInst2, SingletonInst3, null], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly List<CharSeqOrEnumerableClassUnion> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAddCharSeq(nameof(firstPreField), firstPreField)
           .CollectionField
           .AlwaysRevealAllEnumerate<List<CharSeqOrEnumerableClassUnion>, CharSeqOrEnumerableClassUnion>
               (nameof(firstEnumerable), firstEnumerable)
           .Complete();
    }
}

public class CharSequenceEnumerablePostFieldClassUnionRevisit : IStringBearer
{
    private readonly ICharSequence? firstPostField = null;

    public CharSequenceEnumerablePostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<ICharSequence>? stringRevealer = null
      , PalantírReveal<CharSeqOrEnumerableClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new CharSeqOrEnumerableClassUnion([SingletonInst1, null, SingletonInst2, SingletonInst3]
                                                          , isSimple, isValue, stringRevealer);
        List<CharSeqOrEnumerableClassUnion> firstAsEnumerable = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((List<ICharSequence?>?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([NewCharSequence(1), NewCharSequence(2), null, NewCharSequence(3)]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly List<CharSeqOrEnumerableClassUnion> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField
           .AlwaysRevealAllEnumerate<List<CharSeqOrEnumerableClassUnion>, CharSeqOrEnumerableClassUnion>
               (nameof(firstEnumerable), firstEnumerable)
           .Field.AlwaysAddCharSeq(nameof(firstPostField), firstPostField)
           .Complete();
    }
}


public class PreFieldCharSequenceEnumeratorStructUnionRevisit : IStringBearer
{
    private readonly ICharSequence? firstPreField = null;

    public PreFieldCharSequenceEnumeratorStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<ICharSequence>? stringRevealer = null
      , PalantírReveal<CharSeqOrEnumeratorStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new CharSeqOrEnumeratorStructUnion([NewCharSequence(1), NewCharSequence(2), null, NewCharSequence(3)]
                                                           , isSimple, isValue, stringRevealer);
        List<CharSeqOrEnumeratorStructUnion> firstAsEnumerator = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((ICharSequence?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, null, SingletonInst2, SingletonInst3], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<CharSeqOrEnumeratorStructUnion>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<CharSeqOrEnumeratorStructUnion>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .Field.AlwaysAddCharSeq(nameof(firstPreField), firstPreField)
           .CollectionField
           .AlwaysRevealAllIterate<List<CharSeqOrEnumeratorStructUnion>.Enumerator, CharSeqOrEnumeratorStructUnion>
               (nameof(firstEnumerator), firstEnumerator)
           .Complete();
    }
}

public class CharSequenceEnumeratorPostFieldStructUnionRevisit : IStringBearer
{
    private readonly ICharSequence? firstPostField = SingletonInst2;

    public CharSequenceEnumeratorPostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<ICharSequence>? stringRevealer = null
      , PalantírReveal<CharSeqOrArrayStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new CharSeqOrEnumeratorStructUnion([null, SingletonInst1, SingletonInst2, SingletonInst3]
                                                           , isSimple, isValue, stringRevealer);
        List<CharSeqOrEnumeratorStructUnion> firstAsEnumerator = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((List<ICharSequence?>?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([NewCharSequence(1), NewCharSequence(2), NewCharSequence(3), null], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<CharSeqOrEnumeratorStructUnion>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<CharSeqOrEnumeratorStructUnion>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .CollectionField
           .AlwaysRevealAllIterate<List<CharSeqOrEnumeratorStructUnion>.Enumerator, CharSeqOrEnumeratorStructUnion>
               (nameof(firstEnumerator), firstEnumerator)
           .Field.AlwaysAddCharSeq(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldCharSequenceEnumeratorClassUnionRevisit : IStringBearer
{
    private readonly ICharSequence? firstPreField = SingletonInst2;

    public PreFieldCharSequenceEnumeratorClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<ICharSequence>? stringRevealer = null
      , PalantírReveal<CharSeqOrEnumeratorClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new CharSeqOrEnumeratorClassUnion([NewCharSequence(1), null, NewCharSequence(2), NewCharSequence(3)]
                                                          , isSimple, isValue, stringRevealer);
        List<CharSeqOrEnumeratorClassUnion> firstAsEnumerator = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((ICharSequence?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([SingletonInst1, SingletonInst2, null, SingletonInst3], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<CharSeqOrEnumeratorClassUnion>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<CharSeqOrEnumeratorClassUnion>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .Field.AlwaysAddCharSeq(nameof(firstPreField), firstPreField)
           .CollectionField
           .AlwaysRevealAllIterate<List<CharSeqOrEnumeratorClassUnion>.Enumerator, CharSeqOrEnumeratorClassUnion>
               (nameof(firstEnumerator), firstEnumerator)
           .Complete();
    }
}

public class CharSequenceEnumeratorPostFieldClassUnionRevisit : IStringBearer
{
    private readonly ICharSequence? firstPostField = null;

    public CharSequenceEnumeratorPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<ICharSequence>? stringRevealer = null
      , PalantírReveal<CharSeqOrEnumeratorClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new CharSeqOrEnumeratorClassUnion([SingletonInst1, SingletonInst2, null, SingletonInst3]
                                                          , isSimple, isValue, stringRevealer);
        List<CharSeqOrEnumeratorClassUnion> firstAsEnumerator = [
            new (SingletonInst2, isSimple, isValue, stringRevealer)
          , new ((List<ICharSequence?>?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([NewCharSequence(1), null, NewCharSequence(2), NewCharSequence(3)], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<CharSeqOrEnumeratorClassUnion>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<CharSeqOrEnumeratorClassUnion>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .CollectionField
           .AlwaysRevealAllIterate<List<CharSeqOrEnumeratorClassUnion>.Enumerator, CharSeqOrEnumeratorClassUnion>
               (nameof(firstEnumerator), firstEnumerator)
           .Field.AlwaysAddCharSeq(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

