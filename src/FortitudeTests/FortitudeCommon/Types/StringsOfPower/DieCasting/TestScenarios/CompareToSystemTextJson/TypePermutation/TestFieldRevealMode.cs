namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Scenarios.CompareToSystemTextJson.TypePermutation;

public enum TestFieldRevealMode
{
    AlwaysAll,
    WhenNonDefault,
    WhenNonNull,
    WhenNonNullOrNonDefault
}

public enum TestCollectionFieldRevealMode
{
    AlwaysAddAll,
    AlwaysFilter,
    WhenPopulated,
    WhenPopulatedWithFilter
}
