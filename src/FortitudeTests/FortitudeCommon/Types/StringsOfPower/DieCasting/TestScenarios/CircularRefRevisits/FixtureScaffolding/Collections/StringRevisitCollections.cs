// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.Collections;

public class PreFieldStringArrayStructUnionRevisit : IStringBearer
{
    private readonly string? firstPreField = null;

    public PreFieldStringArrayStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<string>? stringRevealer = null
      , PalantírReveal<StringOrArrayStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringOrArrayStructUnion([null, new string("new string 1"), new string("new string 2"), new string("new string 3")]
                                                      , isSimple, isValue, stringRevealer);
        List<StringOrArrayStructUnion> firstAsList = [
            new ("interned string 2", isSimple, isValue, stringRevealer)
          , new ((string?[]?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new (["interned string 1", "interned string 2", "interned string 3"]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly StringOrArrayStructUnion[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Complete();
    }
}

public class StringArrayPostFieldStructUnionRevisit : IStringBearer
{
    private readonly string? firstPostField = "interned string 2";

    public StringArrayPostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<string>? stringRevealer = null
      , PalantírReveal<StringOrArrayStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringOrArrayStructUnion(["interned string 1", "interned string 2", "interned string 3", null]
                                                      , isSimple, isValue, stringRevealer);
        List<StringOrArrayStructUnion> firstAsList = [
            new ("interned string 2", isSimple, isValue, stringRevealer)
          , new ((string?[]?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new (["interned string 1", "interned string 2", "interned string 3"]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly StringOrArrayStructUnion[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldStringArrayClassUnionRevisit : IStringBearer
{
    private readonly string? firstPreField = null;

    public PreFieldStringArrayClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<string>? stringRevealer = null
      , PalantírReveal<StringOrArrayClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringOrArrayClassUnion([new string("new string 1"), null, new string("new string 2"), new string("new string 3")]
                                                     , isSimple, isValue, stringRevealer);
        List<StringOrArrayClassUnion> firstAsList = [
            new ("interned string 2", isSimple, isValue, stringRevealer)
          , new ((string?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new (["interned string 1", "interned string 2", "interned string 3"]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly StringOrArrayClassUnion[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Complete();
    }
}

public class StringArrayPostFieldClassUnionRevisit : IStringBearer
{
    private readonly string? firstPostField = "interned string 2";

    public StringArrayPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<string>? stringRevealer = null
      , PalantírReveal<StringOrArrayClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringOrArrayClassUnion(["interned string 1", "interned string 2", null, "interned string 3"]
                                                     , isSimple, isValue, stringRevealer);
        List<StringOrArrayClassUnion> firstAsList = [
            new ("interned string 2", isSimple, isValue, stringRevealer)
          , new ((string?[]?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([new string("new string 1"), new string("new string 2"), new string("new string 3"), null]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstArray = firstAsList.ToArray();
    }

    private readonly StringOrArrayClassUnion[] firstArray;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstArray), firstArray)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldStringSpanClassUnionRevisit : IStringBearer
{
    private readonly string? firstPreField = "interned string 2";

    public PreFieldStringSpanClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<string>? stringRevealer = null
      , PalantírReveal<StringOrSpanClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringOrSpanClassUnion([null, new string("new string 1"), new string("new string 2"), new string("new string 3")]
                                                    , isSimple, isValue, stringRevealer);
        List<StringOrSpanClassUnion> firstAsList = [
            new ("interned string 2", isSimple, isValue, stringRevealer)
          , new ((string?[]?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new (["interned string 1", "interned string 2", "interned string 3"], isSimple, isValue, stringRevealer)
          , repeatedItem  
        ];
        firstSpan = firstAsList.ToArray();
    }

    private readonly StringOrSpanClassUnion[] firstSpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstSpan), firstSpan.AsSpan())
           .Complete();
    }
}

public class StringSpanPostFieldClassUnionRevisit : IStringBearer
{
    private readonly string? firstPostField = null;

    public StringSpanPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<string>? stringRevealer = null
      , PalantírReveal<StringOrSpanClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringOrSpanClassUnion(["interned string 1", "interned string 2", "interned string 3", null]
                                                    , isSimple, isValue, stringRevealer);
        List<StringOrSpanClassUnion> firstAsList = [
            new ("interned string 2", isSimple, isValue, stringRevealer)
          , new ((string?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([new string("new string 1"), null, new string("new string 2"), new string("new string 3")]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstSpan = firstAsList.ToArray();
    }

    private readonly StringOrSpanClassUnion[] firstSpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstSpan), firstSpan.AsSpan())
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldStringReadOnlySpanClassUnionRevisit : IStringBearer
{
    private readonly string? firstPreField = "interned string 2";

    public PreFieldStringReadOnlySpanClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<string>? stringRevealer = null
      , PalantírReveal<StringOrReadOnlySpanClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringOrReadOnlySpanClassUnion([new string("new string 1"), null, new string("new string 2"), null, new string("new string 3")]
                                                            , isSimple, isValue, stringRevealer);
        List<StringOrReadOnlySpanClassUnion> firstAsList = [
            new ("interned string 2", isSimple, isValue, stringRevealer)
          , new ((string?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([null, "interned string 1", "interned string 2", "interned string 3"], isSimple, isValue, stringRevealer)
          , repeatedItem  
        ];
        firstReadOnlySpan = firstAsList.ToArray();
    }

    private readonly StringOrReadOnlySpanClassUnion[] firstReadOnlySpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstReadOnlySpan), (ReadOnlySpan<StringOrReadOnlySpanClassUnion>)firstReadOnlySpan)
           .Complete();
    }
}

public class StringReadOnlySpanPostFieldClassUnionRevisit : IStringBearer
{
    private readonly string? firstPostField = null;

    public StringReadOnlySpanPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<string>? stringRevealer = null
      , PalantírReveal<StringOrReadOnlySpanClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringOrReadOnlySpanClassUnion([null, "interned string 1", "interned string 2", "interned string 3"]
                                                            , isSimple, isValue, stringRevealer);
        List<StringOrReadOnlySpanClassUnion> firstAsList = [
            new ("interned string 2", isSimple, isValue, stringRevealer)
          , new ((string?[]?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([new string("new string 1"), null, new string("new string 2"), new string("new string 3")]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstReadOnlySpan = firstAsList.ToArray();
    }

    private readonly StringOrReadOnlySpanClassUnion[] firstReadOnlySpan;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstReadOnlySpan), (ReadOnlySpan<StringOrReadOnlySpanClassUnion>)firstReadOnlySpan)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldStringListStructUnionRevisit : IStringBearer
{
    private readonly string? firstPreField = null;

    public PreFieldStringListStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<string>? stringRevealer = null
      , PalantírReveal<StringOrListStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringOrListStructUnion([new string("new string 1"), new string("new string 2"), new string("new string 3"), null]
                                                     , isSimple, isValue, stringRevealer);
        List<StringOrListStructUnion> firstAsList = [
            new ("interned string 2", isSimple, isValue, stringRevealer)
          , new ((string?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new (["interned string 1", "interned string 2", "interned string 3"], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<StringOrListStructUnion> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Complete();
    }
}

public class StringListPostFieldStructUnionRevisit : IStringBearer
{
    private readonly string? firstPostField = "interned string 2";

    public StringListPostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<string>? stringRevealer = null
      , PalantírReveal<StringOrArrayStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringOrListStructUnion(["interned string 1", null, "interned string 2", "interned string 3"]
                                                     , isSimple, isValue, stringRevealer);
        List<StringOrListStructUnion> firstAsList = [
            new ("interned string 2", isSimple, isValue, stringRevealer)
          , new ((List<string?>?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([new string("new string 1"), new string("new string 2"), null, new string("new string 3")]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<StringOrListStructUnion> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldStringListClassUnionRevisit : IStringBearer
{
    private readonly string? firstPreField = "interned string 2";

    public PreFieldStringListClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<string>? stringRevealer = null
      , PalantírReveal<StringOrListClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringOrListClassUnion([new string("new string 1"), new string("new string 2"), null, new string("new string 3")]
                                                    , isSimple, isValue, stringRevealer);
        List<StringOrListClassUnion> firstAsList = [
            new ("interned string 2", isSimple, isValue, stringRevealer)
          , new ((string?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new (["interned string 1", null, "interned string 2", "interned string 3"], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<StringOrListClassUnion> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Complete();
    }
}

public class StringListPostFieldClassUnionRevisit : IStringBearer
{
    private readonly string? firstPostField = null;

    public StringListPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<string>? stringRevealer = null
      , PalantírReveal<StringOrListClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringOrListClassUnion([null, "interned string 1", "interned string 2", "interned string 3"]
                                                    , isSimple, isValue, stringRevealer);
        List<StringOrListClassUnion> firstAsList = [
            new ("interned string 2", isSimple, isValue, stringRevealer)
          , new ((List<string?>?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([new string("new string 1"), new string("new string 2"), new string("new string 3"), null]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstList = firstAsList;
    }

    private readonly List<StringOrListClassUnion> firstList;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(firstList), firstList)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldStringEnumerableStructUnionRevisit : IStringBearer
{
    private readonly string? firstPreField = null;

    public PreFieldStringEnumerableStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<string>? stringRevealer = null
      , PalantírReveal<StringOrEnumerableStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringOrEnumerableStructUnion([new string("new string 1"), null, new string("new string 2"), new string("new string 3")]
                                                           , isSimple, isValue, stringRevealer);
        List<StringOrEnumerableStructUnion> firstAsEnumerable = [
            new ("interned string 2", isSimple, isValue, stringRevealer)
          , new ((List<string?>?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new (["interned string 1", "interned string 2", null, "interned string 3"], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly IEnumerable<StringOrEnumerableStructUnion> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField
           .AlwaysRevealAllEnumerate<IEnumerable<StringOrEnumerableStructUnion>, StringOrEnumerableStructUnion>(nameof(firstEnumerable), firstEnumerable)
           .Complete();
    }
}

public class StringEnumerablePostFieldStructUnionRevisit : IStringBearer
{
    private readonly string? firstPostField = "interned string 2";

    public StringEnumerablePostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<string>? stringRevealer = null
      , PalantírReveal<StringOrArrayStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringOrEnumerableStructUnion(["interned string 1", "interned string 2", null, "interned string 3"]
                                                           , isSimple, isValue, stringRevealer);
        List<StringOrEnumerableStructUnion> firstAsEnumerable = [
            new ("interned string 2", isSimple, isValue, stringRevealer)
          , new ((List<string?>?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([new string("new string 1"), null, new string("new string 2"), new string("new string 3")], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly List<StringOrEnumerableStructUnion> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField
           .AlwaysRevealAllEnumerate<List<StringOrEnumerableStructUnion>, StringOrEnumerableStructUnion>(nameof(firstEnumerable), firstEnumerable)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldStringEnumerableClassUnionRevisit : IStringBearer
{
    private readonly string? firstPreField = "interned string 2";

    public PreFieldStringEnumerableClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<string>? stringRevealer = null
      , PalantírReveal<StringOrEnumerableClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringOrEnumerableClassUnion([null, new string("new string 1"), new string("new string 2"), new string("new string 3")]
                                                          , isSimple, isValue, stringRevealer);
        List<StringOrEnumerableClassUnion> firstAsEnumerable = [
            new ("interned string 2", isSimple, isValue, stringRevealer)
          , new ((string?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new (["interned string 1", "interned string 2", "interned string 3", null], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly List<StringOrEnumerableClassUnion> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField
           .AlwaysRevealAllEnumerate<List<StringOrEnumerableClassUnion>, StringOrEnumerableClassUnion>
               (nameof(firstEnumerable), firstEnumerable)
           .Complete();
    }
}

public class StringEnumerablePostFieldClassUnionRevisit : IStringBearer
{
    private readonly string? firstPostField = null;

    public StringEnumerablePostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<string>? stringRevealer = null
      , PalantírReveal<StringOrEnumerableClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringOrEnumerableClassUnion(["interned string 1", null, "interned string 2", "interned string 3"]
                                                          , isSimple, isValue, stringRevealer);
        List<StringOrEnumerableClassUnion> firstAsEnumerable = [
            new ("interned string 2", isSimple, isValue, stringRevealer)
          , new ((List<string?>?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([new string("new string 1"), new string("new string 2"), null, new string("new string 3")]
               , isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerable = firstAsEnumerable;
    }

    private readonly List<StringOrEnumerableClassUnion> firstEnumerable;

    public AppendSummary RevealState(ITheOneString tos)
    {
        return tos.StartComplexType(this)
           .CollectionField
           .AlwaysRevealAllEnumerate<List<StringOrEnumerableClassUnion>, StringOrEnumerableClassUnion>
               (nameof(firstEnumerable), firstEnumerable)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}


public class PreFieldStringEnumeratorStructUnionRevisit : IStringBearer
{
    private readonly string? firstPreField = null;

    public PreFieldStringEnumeratorStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<string>? stringRevealer = null
      , PalantírReveal<StringOrEnumeratorStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringOrEnumeratorStructUnion([new string("new string 1"), new string("new string 2"), null, new string("new string 3")]
                                                           , isSimple, isValue, stringRevealer);
        List<StringOrEnumeratorStructUnion> firstAsEnumerator = [
            new ("interned string 2", isSimple, isValue, stringRevealer)
          , new ((string?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new (["interned string 1", null, "interned string 2", "interned string 3"], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<StringOrEnumeratorStructUnion>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<StringOrEnumeratorStructUnion>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField
           .AlwaysRevealAllIterate<List<StringOrEnumeratorStructUnion>.Enumerator, StringOrEnumeratorStructUnion>
               (nameof(firstEnumerator), firstEnumerator)
           .Complete();
    }
}

public class StringEnumeratorPostFieldStructUnionRevisit : IStringBearer
{
    private readonly string? firstPostField = "interned string 2";

    public StringEnumeratorPostFieldStructUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<string>? stringRevealer = null
      , PalantírReveal<StringOrArrayStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringOrEnumeratorStructUnion([null, "interned string 1", "interned string 2", "interned string 3"]
                                                           , isSimple, isValue, stringRevealer);
        List<StringOrEnumeratorStructUnion> firstAsEnumerator = [
            new ("interned string 2", isSimple, isValue, stringRevealer)
          , new ((List<string?>?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([new string("new string 1"), new string("new string 2"), new string("new string 3"), null], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<StringOrEnumeratorStructUnion>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<StringOrEnumeratorStructUnion>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .CollectionField
           .AlwaysRevealAllIterate<List<StringOrEnumeratorStructUnion>.Enumerator, StringOrEnumeratorStructUnion>
               (nameof(firstEnumerator), firstEnumerator)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

public class PreFieldStringEnumeratorClassUnionRevisit : IStringBearer
{
    private readonly string? firstPreField = "interned string 2";

    public PreFieldStringEnumeratorClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<string>? stringRevealer = null
      , PalantírReveal<StringOrEnumeratorClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringOrEnumeratorClassUnion([new string("new string 1"), null, new string("new string 2"), new string("new string 3")]
                                                          , isSimple, isValue, stringRevealer);
        List<StringOrEnumeratorClassUnion> firstAsEnumerator = [
            new ("interned string 2", isSimple, isValue, stringRevealer)
          , new ((string?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new (["interned string 1", "interned string 2", null, "interned string 3"], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<StringOrEnumeratorClassUnion>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<StringOrEnumeratorClassUnion>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(firstPreField), firstPreField)
           .CollectionField
           .AlwaysRevealAllIterate<List<StringOrEnumeratorClassUnion>.Enumerator, StringOrEnumeratorClassUnion>
               (nameof(firstEnumerator), firstEnumerator)
           .Complete();
    }
}

public class StringEnumeratorPostFieldClassUnionRevisit : IStringBearer
{
    private readonly string? firstPostField = null;

    public StringEnumeratorPostFieldClassUnionRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<string>? stringRevealer = null
      , PalantírReveal<StringOrEnumeratorClassUnion>? nodeRevealer = null)
    {
        var repeatedItem = new StringOrEnumeratorClassUnion(["interned string 1", "interned string 2", null, "interned string 3"]
                                                          , isSimple, isValue, stringRevealer);
        List<StringOrEnumeratorClassUnion> firstAsEnumerator = [
            new ("interned string 2", isSimple, isValue, stringRevealer)
          , new ((List<string?>?)null, isSimple, isValue, stringRevealer)
          , repeatedItem  
          , new ([new string("new string 1"), null, new string("new string 2"), new string("new string 3")], isSimple, isValue, stringRevealer)
          ,repeatedItem  
        ];
        firstEnumerator = firstAsEnumerator.GetEnumerator();
    }

    private readonly List<StringOrEnumeratorClassUnion>.Enumerator firstEnumerator;

    public AppendSummary RevealState(ITheOneString tos)
    {
        ((IEnumerator<StringOrEnumeratorClassUnion>)firstEnumerator).Reset();
        return tos.StartComplexType(this)
           .CollectionField
           .AlwaysRevealAllIterate<List<StringOrEnumeratorClassUnion>.Enumerator, StringOrEnumeratorClassUnion>
               (nameof(firstEnumerator), firstEnumerator)
           .Field.AlwaysAdd(nameof(firstPostField), firstPostField)
           .Complete();
    }
}

