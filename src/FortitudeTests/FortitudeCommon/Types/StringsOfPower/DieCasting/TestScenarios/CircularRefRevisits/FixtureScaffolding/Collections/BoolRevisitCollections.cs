// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.Collections;

public class PreFieldBoolArrayRevisit : IStringBearer
{
    private readonly bool firstPreField = true;

    public PreFieldBoolArrayRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
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

public class BoolArrayPostFieldRevisit : IStringBearer
{
    private readonly bool firstPostField = true;

    public BoolArrayPostFieldRevisit(bool isSimple = true, bool isValue = true, PalantírReveal<bool>? boolRevealer = null
      , PalantírReveal<BoolOrArrayStructUnion>? nodeRevealer = null)
    {
        var repeatedItem = new BoolOrArrayStructUnion([true, false, true], isSimple, isValue, boolRevealer);
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
