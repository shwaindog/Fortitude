namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation;

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
