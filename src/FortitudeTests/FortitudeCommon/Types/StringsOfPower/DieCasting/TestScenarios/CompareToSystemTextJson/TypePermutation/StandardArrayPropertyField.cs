using System.Net;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Extensions;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

// ReSharper disable FormatStringProblem

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Scenarios.CompareToSystemTextJson.TypePermutation;

[NoMatchingProductionClass]
public class StandardArrayPropertyFieldClass : IStringBearer
{
    public StandardArrayPropertyFieldClass()
    {
        InitializeAllSet();
    }

    public void InitializeAllSet()
    {
        ByteArrayPropield      = [byte.MinValue, 0, byte.MaxValue];
        SByteArrayPropield     = [sbyte.MinValue, 0, sbyte.MaxValue];
        CharArrayPropield      = [' ', '\u0000', '\uFFFF'];
        ShortArrayPropield     = [short.MinValue, 0, short.MaxValue];
        UShortArrayPropield    = [ushort.MinValue, 0, ushort.MaxValue];
        HalfArrayPropield      = [Half.MinValue, default, Half.NaN, Half.MaxValue];
        IntArrayPropield       = [int.MinValue, 0, int.MaxValue];
        UIntArrayPropield      = [uint.MinValue, 0, uint.MaxValue];
        FloatArrayPropield     = [float.MinValue, 0f, float.NaN, float.MaxValue];
        LongArrayPropield      = [long.MinValue, 0, long.MaxValue];
        ULongArrayPropield     = [ulong.MinValue, 0, ulong.MaxValue];
        DoubleArrayPropield    = [double.MinValue, 0d, double.NaN, double.MaxValue];
        DecimalArrayPropield   = [decimal.MinValue, 0m, decimal.MaxValue];
        VeryLongArrayPropield  = [Int128.MinValue, default, Int128.MaxValue];
        VeryUlongArrayPropield = [UInt128.MinValue, default, UInt128.MaxValue];
        BigIntArrayPropield    = [BigInteger.Parse("-99999999999999999999999999"), default, BigInteger.Parse("99999999999999999999999999")];
        ComplexArrayPropield   = [new Complex(double.MaxValue * -1.0, double.MaxValue * -1), default, new Complex(double.MaxValue, double.MaxValue)];
        DateTimeArrayPropield  = [DateTime.MinValue, default, DateTime.MaxValue];
        DateOnlyArrayPropield  = [DateOnly.MinValue, default, DateOnly.MaxValue];
        TimeSpanArrayPropield  = [TimeSpan.MinValue, TimeSpan.Zero, TimeSpan.MaxValue];
        TimeOnlyArrayPropield  = [TimeOnly.MinValue, default, TimeOnly.MaxValue];
        RuneArrayPropield      = [Rune.GetRuneAt("\U00010000", 0), default, Rune.GetRuneAt("\U0010FFFF", 0)];
        GuidArrayPropield =
            [Guid.ParseExact("00000000-0000-0000-0000-000000000000", "D"), Guid.Empty, Guid.ParseExact("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", "D")];
        IpNetworkArrayPropield = [new IPNetwork(new IPAddress([128, 0, 0, 0]), 1), default, IPNetwork.Parse("255.255.255.254/31")];

        NullByteArrayPropield      = [byte.MinValue, 0, null, byte.MaxValue];
        NullSByteArrayPropield     = [sbyte.MinValue, 0, null, sbyte.MaxValue];
        NullCharArrayPropield      = [' ', '\u0000', null, '\uFFFF'];
        NullShortArrayPropield     = [short.MinValue, 0, null, short.MaxValue];
        NullUShortArrayPropield    = [ushort.MinValue, 0, null, ushort.MaxValue];
        NullHalfArrayPropield      = [Half.MinValue, Half.Zero, Half.NaN, null, Half.MaxValue];
        NullIntArrayPropield       = [int.MinValue, 0, null, int.MaxValue];
        NullUIntArrayPropield      = [uint.MinValue, 0, null, uint.MaxValue];
        NullFloatArrayPropield     = [float.MinValue, 0f, float.NaN, null, float.MaxValue];
        NullLongArrayPropield      = [long.MinValue, 0, null, long.MaxValue];
        NullULongArrayPropield     = [ulong.MinValue, 0, null, ulong.MaxValue];
        NullDoubleArrayPropield    = [double.MinValue, 0d, double.NaN, null, double.MaxValue];
        NullDecimalArrayPropield   = [decimal.MinValue, 0m, null, decimal.MaxValue];
        NullVeryLongArrayPropield  = [Int128.MinValue, Int128.Zero, null, Int128.MaxValue];
        NullVeryUlongArrayPropield = [UInt128.MinValue, UInt128.Zero, null, UInt128.MaxValue];
        NullBigIntArrayPropield    = [BigInteger.Parse("-99999999999999999999999999"), BigInteger.Zero, null, BigInteger.Parse("99999999999999999999999999")];
        NullComplexArrayPropield =
            [new Complex(double.MaxValue * -1.0, double.MaxValue * -1), Complex.Zero, null, new Complex(double.MaxValue, double.MaxValue)];
        NullDateTimeArrayPropield = [DateTime.MinValue, new DateTime(), null, DateTime.MaxValue];
        NullDateOnlyArrayPropield = [DateOnly.MinValue, new DateOnly(), null, DateOnly.MaxValue];
        NullTimeSpanArrayPropield = [TimeSpan.MinValue, TimeSpan.Zero, null, TimeSpan.MaxValue];
        NullTimeOnlyArrayPropield = [TimeOnly.MinValue, null, TimeOnly.MaxValue];
        NullRuneArrayPropield     = [Rune.GetRuneAt("\U00010000", 0), Rune.GetRuneAt("\u0000", 0), null, Rune.GetRuneAt("\U0010FFFF", 0)];
        NullGuidArrayPropield =
            [Guid.ParseExact("00000000-0000-0000-0000-000000000000", "D"), Guid.Empty, null, Guid.ParseExact("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", "D")];
        NullIpNetworkArrayPropield =
            [new IPNetwork(new IPAddress([128, 0, 0, 0]), 1), new IPNetwork(), null, IPNetwork.Parse("255.255.255.254/31")];

        StringArrayPropield        = ["stringArrayPropield_1", "", null!, "stringArrayPropield_4"];
        StringBuilderArrayPropield = [new("stringBuilderArrayPropield_1"), new StringBuilder(), null!, new StringBuilder("stringBuilderArrayPropield_4")];
        CharSequenceArrayPropield =
            [new MutableString("charSequenceArrayPropield_1"), new MutableString(), null!, new MutableString("charSequenceArrayPropield_4")];

        VersionArrayPropield   = [new Version(0, 0), null!, new Version(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue)];
        IpAddressArrayPropield = [new IPAddress("\0\0\0\0"u8.ToArray()), null!, IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff")];
        UriArrayPropield       = [new Uri("https://learn.microsoft.com/en-us/dotnet/api"), null!, new Uri("https://github.com/shwaindog/Fortitude")];

        SpanFormattableArrayPropield = [new MySpanFormattableClass("", true), null!, new MySpanFormattableClass("SpanFormattableSingPropield", true)];
        NdLNfEnumArrayPropield       = [NoDefaultLongNoFlagsEnum.NDLNFE_1, default, NoDefaultLongNoFlagsEnum.NDLNFE_34];
        NdUNfEnumArrayPropield       = [NoDefaultULongNoFlagsEnum.NDUNFE_1, default, NoDefaultULongNoFlagsEnum.NDUNFE_34];
        NdLWfEnumArrayPropield =
        [
            NoDefaultLongWithFlagsEnum.NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_2, default
          , NoDefaultLongWithFlagsEnum.NDLWFE_33 | NoDefaultLongWithFlagsEnum.NDLWFE_34
        ];
        NdUWfEnumArrayPropield =
        [
            NoDefaultULongWithFlagsEnum.NDUWFE_1 | NoDefaultULongWithFlagsEnum.NDUWFE_2, default
          , NoDefaultULongWithFlagsEnum.NDUWFE_33 | NoDefaultULongWithFlagsEnum.NDUWFE_34
        ];

        WdLNfEnumArrayPropield = [WithDefaultLongNoFlagsEnum.WDLNFE_1, default, WithDefaultLongNoFlagsEnum.WDLNFE_34];
        WdUNfEnumArrayPropield = [WithDefaultULongNoFlagsEnum.WDUNFE_1, default, WithDefaultULongNoFlagsEnum.WDUNFE_34];
        WdLWfEnumArrayPropield =
        [
            WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_2, default
          , WithDefaultLongWithFlagsEnum.WDLWFE_33 | WithDefaultLongWithFlagsEnum.WDLWFE_34
        ];
        WdUWfEnumArrayPropield =
        [
            WithDefaultULongWithFlagsEnum.WDUWFE_1 | WithDefaultULongWithFlagsEnum.WDUWFE_2, default
          , WithDefaultULongWithFlagsEnum.WDUWFE_33 | WithDefaultULongWithFlagsEnum.WDUWFE_34
        ];

        NullNdLNfEnumArrayPropield = [NoDefaultLongNoFlagsEnum.NDLNFE_1, default(NoDefaultLongNoFlagsEnum), null, NoDefaultLongNoFlagsEnum.NDLNFE_34];
        NullNdUNfEnumArrayPropield = [NoDefaultULongNoFlagsEnum.NDUNFE_1, default(NoDefaultULongNoFlagsEnum), null, NoDefaultULongNoFlagsEnum.NDUNFE_34];
        NullNdLWfEnumArrayPropield =
        [
            NoDefaultLongWithFlagsEnum.NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_2, default(NoDefaultLongWithFlagsEnum), null
          , NoDefaultLongWithFlagsEnum.NDLWFE_33 | NoDefaultLongWithFlagsEnum.NDLWFE_34
        ];
        NullNdUWfEnumArrayPropield =
        [
            NoDefaultULongWithFlagsEnum.NDUWFE_1 | NoDefaultULongWithFlagsEnum.NDUWFE_2, default(NoDefaultULongWithFlagsEnum), null
          , NoDefaultULongWithFlagsEnum.NDUWFE_33 | NoDefaultULongWithFlagsEnum.NDUWFE_34
        ];

        NullWdLNfEnumArrayPropield = [WithDefaultLongNoFlagsEnum.WDLNFE_1, default(WithDefaultLongNoFlagsEnum), null, WithDefaultLongNoFlagsEnum.WDLNFE_34];
        NullWdUNfEnumArrayPropield = [WithDefaultULongNoFlagsEnum.WDUNFE_1, default(WithDefaultULongNoFlagsEnum), null, WithDefaultULongNoFlagsEnum.WDUNFE_34];
        NullWdLWfEnumArrayPropield =
        [
            WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_2, default(WithDefaultLongWithFlagsEnum), null
          , WithDefaultLongWithFlagsEnum.WDLWFE_33 | WithDefaultLongWithFlagsEnum.WDLWFE_34
        ];
        NullWdUWfEnumArrayPropield =
        [
            WithDefaultULongWithFlagsEnum.WDUWFE_1 | WithDefaultULongWithFlagsEnum.WDUWFE_2, default(WithDefaultULongWithFlagsEnum), null
          , WithDefaultULongWithFlagsEnum.WDUWFE_33 | WithDefaultULongWithFlagsEnum.WDUWFE_34
        ];
    }


    public void InitializeAtSize(int numberToGenerate)
    {
        ByteArrayPropield      = NumberTestDataGenerator.GenRandomNumberRange<byte>(numberToGenerate).ToArray();
        SByteArrayPropield     = NumberTestDataGenerator.GenRandomNumberRange<sbyte>(numberToGenerate).ToArray();
        CharArrayPropield      = NumberTestDataGenerator.GenRandomNumberRange<char>(numberToGenerate).ToArray();
        ShortArrayPropield     = NumberTestDataGenerator.GenRandomNumberRange<short>(numberToGenerate).ToArray();
        UShortArrayPropield    = NumberTestDataGenerator.GenRandomNumberRange<ushort>(numberToGenerate).ToArray();
        HalfArrayPropield      = NumberTestDataGenerator.GenRandomNumberRange<Half>(numberToGenerate).ToArray();
        IntArrayPropield       = NumberTestDataGenerator.GenRandomNumberRange<int>(numberToGenerate).ToArray();
        UIntArrayPropield      = NumberTestDataGenerator.GenRandomNumberRange<uint>(numberToGenerate).ToArray();
        FloatArrayPropield     = NumberTestDataGenerator.GenRandomNumberRange<float>(numberToGenerate).ToArray();
        LongArrayPropield      = NumberTestDataGenerator.GenRandomNumberRange<long>(numberToGenerate).ToArray();
        ULongArrayPropield     = NumberTestDataGenerator.GenRandomNumberRange<ulong>(numberToGenerate).ToArray();
        DoubleArrayPropield    = NumberTestDataGenerator.GenRandomNumberRange<double>(numberToGenerate).ToArray();
        DecimalArrayPropield   = NumberTestDataGenerator.GenRandomNumberRange<decimal>(numberToGenerate).ToArray();
        VeryLongArrayPropield  = NumberTestDataGenerator.GenRandomNumberRange<Int128>(numberToGenerate).ToArray();
        VeryUlongArrayPropield = NumberTestDataGenerator.GenRandomNumberRange<UInt128>(numberToGenerate).ToArray();
        BigIntArrayPropield    = NumberTestDataGenerator.GenRandomNumberRange<BigInteger>(numberToGenerate).ToArray();
        ComplexArrayPropield   = NumberTestDataGenerator.GenRandomNumberRange<Complex>(numberToGenerate).ToArray();
        DateTimeArrayPropield  = DateTimeTestDataGenerator.GenRandomDateTimeRange(numberToGenerate).ToArray();
        DateOnlyArrayPropield  = DateTimeTestDataGenerator.GenRandomDateOnlyRange(numberToGenerate).ToArray();
        TimeSpanArrayPropield  = DateTimeTestDataGenerator.GenRandomTimeSpanRange(numberToGenerate).ToArray();
        TimeOnlyArrayPropield  = DateTimeTestDataGenerator.GenRandomTimeOnlyRange(numberToGenerate).ToArray();
        RuneArrayPropield      = NumberTestDataGenerator.GenRandomNumberRange<Rune>(numberToGenerate).ToArray();
        GuidArrayPropield =
            NumberTestDataGenerator
                .GenRandomNumberRange<byte>(numberToGenerate)
                .Select(_ =>
                            new Guid(NumberTestDataGenerator
                                     .GenRandomNumberRange<
                                         byte>(16).ToArray()))
                .ToArray();
        IpNetworkArrayPropield =
            NumberTestDataGenerator
                .GenRandomNumberRange<byte>(numberToGenerate)
                .Select(b =>
                {
                    if (b % 2 == 0)
                    {
                        return new IPNetwork
                            (new IPAddress
                                 (NumberTestDataGenerator.GenRandomNumberRange<byte>(4).ToArray()), (b % 32));
                    }
                    return new IPNetwork
                        (new IPAddress
                             (NumberTestDataGenerator.GenRandomNumberRange<byte>(16).ToArray()), (b % 128));
                }).ToArray();

        NullByteArrayPropield      = NumberTestDataGenerator.GenRandomNullableNumberRange<byte>(numberToGenerate).ToArray();
        NullSByteArrayPropield     = NumberTestDataGenerator.GenRandomNullableNumberRange<sbyte>(numberToGenerate).ToArray();
        NullCharArrayPropield      = NumberTestDataGenerator.GenRandomNullableNumberRange<char>(numberToGenerate).ToArray();
        NullShortArrayPropield     = NumberTestDataGenerator.GenRandomNullableNumberRange<short>(numberToGenerate).ToArray();
        NullUShortArrayPropield    = NumberTestDataGenerator.GenRandomNullableNumberRange<ushort>(numberToGenerate).ToArray();
        NullHalfArrayPropield      = NumberTestDataGenerator.GenRandomNullableNumberRange<Half>(numberToGenerate).ToArray();
        NullIntArrayPropield       = NumberTestDataGenerator.GenRandomNullableNumberRange<int>(numberToGenerate).ToArray();
        NullUIntArrayPropield      = NumberTestDataGenerator.GenRandomNullableNumberRange<uint>(numberToGenerate).ToArray();
        NullFloatArrayPropield     = NumberTestDataGenerator.GenRandomNullableNumberRange<float>(numberToGenerate).ToArray();
        NullLongArrayPropield      = NumberTestDataGenerator.GenRandomNullableNumberRange<long>(numberToGenerate).ToArray();
        NullULongArrayPropield     = NumberTestDataGenerator.GenRandomNullableNumberRange<ulong>(numberToGenerate).ToArray();
        NullDoubleArrayPropield    = NumberTestDataGenerator.GenRandomNullableNumberRange<double>(numberToGenerate).ToArray();
        NullDecimalArrayPropield   = NumberTestDataGenerator.GenRandomNullableNumberRange<decimal>(numberToGenerate).ToArray();
        NullVeryLongArrayPropield  = NumberTestDataGenerator.GenRandomNullableNumberRange<Int128>(numberToGenerate).ToArray();
        NullVeryUlongArrayPropield = NumberTestDataGenerator.GenRandomNullableNumberRange<UInt128>(numberToGenerate).ToArray();
        NullBigIntArrayPropield    = NumberTestDataGenerator.GenRandomNullableNumberRange<BigInteger>(numberToGenerate).ToArray();
        NullComplexArrayPropield   = NumberTestDataGenerator.GenRandomNullableNumberRange<Complex>(numberToGenerate).ToArray();
        NullDateTimeArrayPropield  = DateTimeTestDataGenerator.GenRandomNullableDateTimeRange(numberToGenerate).ToArray();
        NullDateOnlyArrayPropield  = DateTimeTestDataGenerator.GenRandomNullableDateOnlyRange(numberToGenerate).ToArray();
        NullTimeSpanArrayPropield  = DateTimeTestDataGenerator.GenRandomNullableTimeSpanRange(numberToGenerate).ToArray();
        NullTimeOnlyArrayPropield  = DateTimeTestDataGenerator.GenRandomNullableTimeOnlyRange(numberToGenerate).ToArray();
        NullRuneArrayPropield      = NumberTestDataGenerator.GenRandomNullableNumberRange<Rune>(numberToGenerate).ToArray();

        NullGuidArrayPropield =
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<byte>(numberToGenerate)
                .Select(b =>
                        {
                            if (b == null) return null;
                            return new Guid?
                                (new Guid
                                     (NumberTestDataGenerator
                                      .GenRandomNumberRange<byte>(16)
                                      .ToArray()));
                        }
                       )
                .ToArray();
        NullIpNetworkArrayPropield =
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<byte>(numberToGenerate)
                .Select(b =>
                {
                    if (b == null) return null;
                    if (b % 2 == 0)
                    {
                        return new IPNetwork?
                            (new IPNetwork
                                 (new IPAddress
                                      (NumberTestDataGenerator
                                       .GenRandomNumberRange<byte>(4).ToArray())
                                , (b.Value % 32)));
                    }
                    return new IPNetwork
                        (new IPAddress
                             (NumberTestDataGenerator
                              .GenRandomNumberRange<byte>(16)
                              .ToArray()), (b.Value % 128));
                }).ToArray();

        StringArrayPropield =
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<int>(numberToGenerate)
                .Select(num =>
                {
                    if (num == null) return null!;
                    return "stringArrayPropield_" + num;
                }).ToArray();
        StringBuilderArrayPropield =
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<int>(numberToGenerate)
                .Select(num =>
                {
                    if (num == null) return null!;
                    return new StringBuilder("stringBuilderArrayPropield_1" + num);
                }).ToArray();
        CharSequenceArrayPropield =
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<int>(numberToGenerate)
                .Select(num =>
                {
                    if (num == null) return null!;
                    return (ICharSequence)new MutableString("charSequenceArrayPropield_1" + num);
                }).ToArray();

        VersionArrayPropield =
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<int>(numberToGenerate)
                .Select(num =>
                {
                    if (num == null) return null!;
                    var otherThree = NumberTestDataGenerator
                                     .GenRandomNumberRange<int>(numberToGenerate).Select(Math.Abs).ToArray();
                    return new Version(Math.Abs(num.Value), otherThree[0], otherThree[1], otherThree[2]);
                }).ToArray();

        IpAddressArrayPropield =
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<byte>(numberToGenerate)
                .Select(b =>
                {
                    if (b == null) return null!;
                    if (b % 2 == 0)
                    {
                        return new IPAddress(NumberTestDataGenerator.GenRandomNumberRange<byte>(4).ToArray());
                    }
                    return new IPAddress(NumberTestDataGenerator.GenRandomNumberRange<byte>(16).ToArray());
                }).ToArray();
        UriArrayPropield =
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<int>(numberToGenerate)
                .Select(num =>
                {
                    if (num == null) return null!;
                    return new Uri("https://www.someWebAddress_" + num + ".net");
                }).ToArray();

        SpanFormattableArrayPropield =
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<int>(numberToGenerate)
                .Select(num =>
                {
                    if (num == null) return null!;
                    return new MySpanFormattableClass(" SpanFormattableArrayPropield_" + num);
                }).ToArray();

        NdLNfEnumArrayPropield = EnumTestDataGenerator.GenRandomEnumValues<NoDefaultLongNoFlagsEnum>(numberToGenerate).ToArray();
        NdUNfEnumArrayPropield = EnumTestDataGenerator.GenRandomEnumValues<NoDefaultULongNoFlagsEnum>(numberToGenerate).ToArray();
        NdLWfEnumArrayPropield = EnumTestDataGenerator.GenRandomEnumMultiFlagValues<NoDefaultLongWithFlagsEnum>(numberToGenerate).ToArray();
        NdUWfEnumArrayPropield = EnumTestDataGenerator.GenRandomEnumMultiFlagValues<NoDefaultULongWithFlagsEnum>(numberToGenerate).ToArray();

        WdLNfEnumArrayPropield = EnumTestDataGenerator.GenRandomEnumValues<WithDefaultLongNoFlagsEnum>(numberToGenerate).ToArray();
        WdUNfEnumArrayPropield = EnumTestDataGenerator.GenRandomEnumValues<WithDefaultULongNoFlagsEnum>(numberToGenerate).ToArray();
        WdLWfEnumArrayPropield = EnumTestDataGenerator.GenRandomEnumMultiFlagValues<WithDefaultLongWithFlagsEnum>(numberToGenerate).ToArray();
        WdUWfEnumArrayPropield = EnumTestDataGenerator.GenRandomEnumMultiFlagValues<WithDefaultULongWithFlagsEnum>(numberToGenerate).ToArray();

        NullNdLNfEnumArrayPropield = EnumTestDataGenerator.GenRandomNullableEnumValues<NoDefaultLongNoFlagsEnum>(numberToGenerate).ToArray();
        NullNdUNfEnumArrayPropield = EnumTestDataGenerator.GenRandomNullableEnumValues<NoDefaultULongNoFlagsEnum>(numberToGenerate).ToArray();
        NullNdLWfEnumArrayPropield = EnumTestDataGenerator.GenRandomNullableEnumMultiFlagValues<NoDefaultLongWithFlagsEnum>(numberToGenerate).ToArray();
        NullNdUWfEnumArrayPropield = EnumTestDataGenerator.GenRandomNullableEnumMultiFlagValues<NoDefaultULongWithFlagsEnum>(numberToGenerate).ToArray();

        NullWdLNfEnumArrayPropield = EnumTestDataGenerator.GenRandomNullableEnumValues<WithDefaultLongNoFlagsEnum>(numberToGenerate).ToArray();
        NullWdUNfEnumArrayPropield = EnumTestDataGenerator.GenRandomNullableEnumValues<WithDefaultULongNoFlagsEnum>(numberToGenerate).ToArray();
        NullWdLWfEnumArrayPropield = EnumTestDataGenerator.GenRandomNullableEnumMultiFlagValues<WithDefaultLongWithFlagsEnum>(numberToGenerate).ToArray();
        NullWdUWfEnumArrayPropield = EnumTestDataGenerator.GenRandomNullableEnumMultiFlagValues<WithDefaultULongWithFlagsEnum>(numberToGenerate).ToArray();
    }

    public void InitializeAllNull()
    {
        ByteArrayPropield      = null!;
        SByteArrayPropield     = null!;
        CharArrayPropield      = null!;
        ShortArrayPropield     = null!;
        UShortArrayPropield    = null!;
        HalfArrayPropield      = null!;
        IntArrayPropield       = null!;
        UIntArrayPropield      = null!;
        FloatArrayPropield     = null!;
        LongArrayPropield      = null!;
        ULongArrayPropield     = null!;
        DoubleArrayPropield    = null!;
        DecimalArrayPropield   = null!;
        VeryLongArrayPropield  = null!;
        VeryUlongArrayPropield = null!;
        BigIntArrayPropield    = null!;

        ComplexArrayPropield   = null!;
        DateTimeArrayPropield  = null!;
        DateOnlyArrayPropield  = null!;
        TimeSpanArrayPropield  = null!;
        TimeOnlyArrayPropield  = null!;
        RuneArrayPropield      = null!;
        GuidArrayPropield      = null!;
        IpNetworkArrayPropield = null!;

        NullByteArrayPropield      = null!;
        NullSByteArrayPropield     = null!;
        NullCharArrayPropield      = null!;
        NullShortArrayPropield     = null!;
        NullUShortArrayPropield    = null!;
        NullHalfArrayPropield      = null!;
        NullIntArrayPropield       = null!;
        NullUIntArrayPropield      = null!;
        NullFloatArrayPropield     = null!;
        NullLongArrayPropield      = null!;
        NullULongArrayPropield     = null!;
        NullDoubleArrayPropield    = null!;
        NullDecimalArrayPropield   = null!;
        NullVeryLongArrayPropield  = null!;
        NullVeryUlongArrayPropield = null!;
        NullBigIntArrayPropield    = null!;

        NullComplexArrayPropield   = null!;
        NullDateTimeArrayPropield  = null!;
        NullDateOnlyArrayPropield  = null!;
        NullTimeSpanArrayPropield  = null!;
        NullTimeOnlyArrayPropield  = null!;
        NullRuneArrayPropield      = null!;
        NullGuidArrayPropield      = null!;
        NullIpNetworkArrayPropield = null!;

        StringArrayPropield        = null!;
        StringBuilderArrayPropield = null!;
        CharSequenceArrayPropield  = null!;

        VersionArrayPropield   = null!;
        IpAddressArrayPropield = null!;
        UriArrayPropield       = null!;

        SpanFormattableArrayPropield = null!;

        NdLNfEnumArrayPropield = null!;
        NdUNfEnumArrayPropield = null!;
        NdLWfEnumArrayPropield = null!;
        NdUWfEnumArrayPropield = null!;

        WdLNfEnumArrayPropield = null!;
        WdUNfEnumArrayPropield = null!;
        WdLWfEnumArrayPropield = null!;
        WdUWfEnumArrayPropield = null!;

        NullNdLNfEnumArrayPropield = null!;
        NullNdUNfEnumArrayPropield = null!;
        NullNdLWfEnumArrayPropield = null!;
        NullNdUWfEnumArrayPropield = null!;

        NullWdLNfEnumArrayPropield = null!;
        NullWdUNfEnumArrayPropield = null!;
        NullWdLWfEnumArrayPropield = null!;
        NullWdUWfEnumArrayPropield = null!;
    }


    public byte[] ByteArrayPropield { get; set; } = null!;
    public sbyte[] SByteArrayPropield = null!;
    public char[] CharArrayPropield { get; set; } = null!;
    public short[] ShortArrayPropield = null!;
    public ushort[] UShortArrayPropield { get; set; } = null!;
    public Half[] HalfArrayPropield = null!;
    public int[] IntArrayPropield { get; set; } = null!;
    public uint[] UIntArrayPropield = null!;
    public float[] FloatArrayPropield { get; set; } = null!;
    public long[] LongArrayPropield = null!;
    public ulong[] ULongArrayPropield { get; set; } = null!;
    public double[] DoubleArrayPropield = null!;
    public decimal[] DecimalArrayPropield { get; set; } = null!;
    public Int128[] VeryLongArrayPropield = null!;
    public UInt128[] VeryUlongArrayPropield { get; set; } = null!;
    public BigInteger[] BigIntArrayPropield = null!;

    public Complex[] ComplexArrayPropield { get; set; } = null!;
    public DateTime[] DateTimeArrayPropield = null!;
    public DateOnly[] DateOnlyArrayPropield { get; set; } = null!;
    public TimeSpan[] TimeSpanArrayPropield = null!;
    public TimeOnly[] TimeOnlyArrayPropield { get; set; } = null!;
    public Rune[] RuneArrayPropield = null!;
    public Guid[] GuidArrayPropield { get; set; } = null!;
    public IPNetwork[] IpNetworkArrayPropield = null!;

    public byte?[] NullByteArrayPropield = null!;
    public sbyte?[] NullSByteArrayPropield { get; set; } = null!;
    public char?[] NullCharArrayPropield = null!;
    public short?[] NullShortArrayPropield { get; set; } = null!;
    public ushort?[] NullUShortArrayPropield = null!;
    public Half?[] NullHalfArrayPropield { get; set; } = null!;
    public int?[] NullIntArrayPropield = null!;
    public uint?[] NullUIntArrayPropield { get; set; } = null!;
    public float?[] NullFloatArrayPropield = null!;
    public long?[] NullLongArrayPropield { get; set; } = null!;
    public ulong?[] NullULongArrayPropield = null!;
    public double?[] NullDoubleArrayPropield { get; set; } = null!;
    public decimal?[] NullDecimalArrayPropield = null!;
    public Int128?[] NullVeryLongArrayPropield { get; set; } = null!;
    public UInt128?[] NullVeryUlongArrayPropield = null!;
    public BigInteger?[] NullBigIntArrayPropield { get; set; } = null!;

    public Complex?[] NullComplexArrayPropield { get; set; } = null!;
    public DateTime?[] NullDateTimeArrayPropield = null!;
    public DateOnly?[] NullDateOnlyArrayPropield { get; set; } = null!;
    public TimeSpan?[] NullTimeSpanArrayPropield = null!;
    public TimeOnly?[] NullTimeOnlyArrayPropield { get; set; } = null!;
    public Rune?[] NullRuneArrayPropield = null!;
    public Guid?[] NullGuidArrayPropield { get; set; } = null!;
    public IPNetwork?[] NullIpNetworkArrayPropield = null!;

    public string[] StringArrayPropield { get; set; } = null!;
    public StringBuilder[] StringBuilderArrayPropield = null!;
    public ICharSequence[] CharSequenceArrayPropield { get; set; } = null!;

    public Version[] VersionArrayPropield = null!;
    public IPAddress[] IpAddressArrayPropield { get; set; } = null!;
    public Uri[] UriArrayPropield = null!;

    public MySpanFormattableClass[] SpanFormattableArrayPropield { get; set; } = null!;

    public NoDefaultLongNoFlagsEnum[] NdLNfEnumArrayPropield = null!;
    public NoDefaultULongNoFlagsEnum[] NdUNfEnumArrayPropield { get; set; } = null!;
    public NoDefaultLongWithFlagsEnum[] NdLWfEnumArrayPropield = null!;
    public NoDefaultULongWithFlagsEnum[] NdUWfEnumArrayPropield { get; set; } = null!;

    public WithDefaultLongNoFlagsEnum[] WdLNfEnumArrayPropield { get; set; } = null!;
    public WithDefaultULongNoFlagsEnum[] WdUNfEnumArrayPropield = null!;
    public WithDefaultLongWithFlagsEnum[] WdLWfEnumArrayPropield { get; set; } = null!;
    public WithDefaultULongWithFlagsEnum[] WdUWfEnumArrayPropield = null!;

    public NoDefaultLongNoFlagsEnum?[] NullNdLNfEnumArrayPropield = null!;
    public NoDefaultULongNoFlagsEnum?[] NullNdUNfEnumArrayPropield { get; set; } = null!;
    public NoDefaultLongWithFlagsEnum?[] NullNdLWfEnumArrayPropield = null!;
    public NoDefaultULongWithFlagsEnum?[] NullNdUWfEnumArrayPropield { get; set; } = null!;

    public WithDefaultLongNoFlagsEnum?[] NullWdLNfEnumArrayPropield { get; set; } = null!;
    public WithDefaultULongNoFlagsEnum?[] NullWdUNfEnumArrayPropield = null!;
    public WithDefaultLongWithFlagsEnum?[] NullWdLWfEnumArrayPropield { get; set; } = null!;
    public WithDefaultULongWithFlagsEnum?[] NullWdUWfEnumArrayPropield = null!;

    [JsonIgnore] public TestCollectionFieldRevealMode TestCollectionFieldRevealMode { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos)
    {
        switch (TestCollectionFieldRevealMode)
        {
            case TestCollectionFieldRevealMode.WhenPopulated:           return WhenPopulatedReveal(tos);
            case TestCollectionFieldRevealMode.AlwaysFilter:            return AlwaysAddFiltered(tos);
            case TestCollectionFieldRevealMode.WhenPopulatedWithFilter: return WhenPopulatedWithFilterReveal(tos);
            case TestCollectionFieldRevealMode.AlwaysAddAll:
            default:
                return AlwaysRevealAll(tos);
        }
    }

    public static IFilterRegistry FilterRegistry { get; set; } = new FilterRegistry(new AddOddRetrieveCountFactory());

    public StateExtractStringRange AlwaysRevealAll(ITheOneString tos)
    {
        using var ctb =
            tos.StartComplexType(this);
        ctb.CollectionField.AlwaysAddAll(nameof(ByteArrayPropield), ByteArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(CharArrayPropield), CharArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(UShortArrayPropield), UShortArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(IntArrayPropield), IntArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(FloatArrayPropield), FloatArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(ULongArrayPropield), ULongArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(DecimalArrayPropield), DecimalArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(VeryUlongArrayPropield), VeryUlongArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(ComplexArrayPropield), ComplexArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(DateOnlyArrayPropield), DateOnlyArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(TimeOnlyArrayPropield), TimeOnlyArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(GuidArrayPropield), GuidArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullSByteArrayPropield), NullSByteArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullShortArrayPropield), NullShortArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullHalfArrayPropield), NullHalfArrayPropield, formatFlags: EnsureFormattedDelimited);
        ctb.CollectionField.AlwaysAddAll(nameof(NullUIntArrayPropield), NullUIntArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullLongArrayPropield), NullLongArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullDoubleArrayPropield), NullDoubleArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullVeryLongArrayPropield), NullVeryLongArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullBigIntArrayPropield), NullBigIntArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullComplexArrayPropield), NullComplexArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullDateOnlyArrayPropield), NullDateOnlyArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullTimeOnlyArrayPropield), NullTimeOnlyArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullGuidArrayPropield), NullGuidArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(StringArrayPropield), StringArrayPropield);
        ctb.CollectionField.AlwaysAddAllCharSeq(nameof(CharSequenceArrayPropield), CharSequenceArrayPropield, formatFlags: AsCollection);
        ctb.CollectionField.AlwaysAddAll(nameof(IpAddressArrayPropield), IpAddressArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(SpanFormattableArrayPropield), SpanFormattableArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NdUNfEnumArrayPropield), NdUNfEnumArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NdUWfEnumArrayPropield), NdUWfEnumArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(WdLNfEnumArrayPropield), WdLNfEnumArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(WdLWfEnumArrayPropield), WdLWfEnumArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullNdUNfEnumArrayPropield), NullNdUNfEnumArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullNdUWfEnumArrayPropield), NullNdUWfEnumArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullWdLNfEnumArrayPropield), NullWdLNfEnumArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullWdLWfEnumArrayPropield), NullWdLWfEnumArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(SByteArrayPropield), SByteArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(ShortArrayPropield), ShortArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(HalfArrayPropield), HalfArrayPropield, formatFlags: EnsureFormattedDelimited);
        ctb.CollectionField.AlwaysAddAll(nameof(UIntArrayPropield), UIntArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(LongArrayPropield), LongArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(DoubleArrayPropield), DoubleArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(VeryLongArrayPropield), VeryLongArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(BigIntArrayPropield), BigIntArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(DateTimeArrayPropield), DateTimeArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(TimeSpanArrayPropield), TimeSpanArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(RuneArrayPropield), RuneArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(IpNetworkArrayPropield), IpNetworkArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullByteArrayPropield), NullByteArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullCharArrayPropield), NullCharArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullUShortArrayPropield), NullUShortArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullIntArrayPropield), NullIntArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullFloatArrayPropield), NullFloatArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullULongArrayPropield), NullULongArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullDecimalArrayPropield), NullDecimalArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullVeryUlongArrayPropield), NullVeryUlongArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullDateTimeArrayPropield), NullDateTimeArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullTimeSpanArrayPropield), NullTimeSpanArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullRuneArrayPropield), NullRuneArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullIpNetworkArrayPropield), NullIpNetworkArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(StringBuilderArrayPropield), StringBuilderArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(VersionArrayPropield), VersionArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(UriArrayPropield), UriArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NdLNfEnumArrayPropield), NdLNfEnumArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NdLWfEnumArrayPropield), NdLWfEnumArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(WdUNfEnumArrayPropield), WdUNfEnumArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(WdUWfEnumArrayPropield), WdUWfEnumArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullNdLNfEnumArrayPropield), NullNdLNfEnumArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullNdLWfEnumArrayPropield), NullNdLWfEnumArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullWdUNfEnumArrayPropield), NullWdUNfEnumArrayPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullWdUWfEnumArrayPropield), NullWdUWfEnumArrayPropield);
        return ctb.Complete();
    }

    public StateExtractStringRange WhenPopulatedReveal(ITheOneString tos)
    {
        using var ctb =
            tos.StartComplexType(this);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(ByteArrayPropield), ByteArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(CharArrayPropield), CharArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(UShortArrayPropield), UShortArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(IntArrayPropield), IntArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(FloatArrayPropield), FloatArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(ULongArrayPropield), ULongArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(DecimalArrayPropield), DecimalArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(VeryUlongArrayPropield), VeryUlongArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(ComplexArrayPropield), ComplexArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(DateOnlyArrayPropield), DateOnlyArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(TimeOnlyArrayPropield), TimeOnlyArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(GuidArrayPropield), GuidArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullSByteArrayPropield), NullSByteArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullShortArrayPropield), NullShortArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullHalfArrayPropield), NullHalfArrayPropield, formatFlags: EnsureFormattedDelimited);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullUIntArrayPropield), NullUIntArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullLongArrayPropield), NullLongArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullDoubleArrayPropield), NullDoubleArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullVeryLongArrayPropield), NullVeryLongArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullBigIntArrayPropield), NullBigIntArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullComplexArrayPropield), NullComplexArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullDateOnlyArrayPropield), NullDateOnlyArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullTimeOnlyArrayPropield), NullTimeOnlyArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullGuidArrayPropield), NullGuidArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(StringArrayPropield), StringArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAllCharSeq(nameof(CharSequenceArrayPropield), CharSequenceArrayPropield, formatFlags: AsCollection);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(IpAddressArrayPropield), IpAddressArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(SpanFormattableArrayPropield), SpanFormattableArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NdUNfEnumArrayPropield), NdUNfEnumArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NdUWfEnumArrayPropield), NdUWfEnumArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(WdLNfEnumArrayPropield), WdLNfEnumArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(WdLWfEnumArrayPropield), WdLWfEnumArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullNdUNfEnumArrayPropield), NullNdUNfEnumArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullNdUWfEnumArrayPropield), NullNdUWfEnumArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullWdLNfEnumArrayPropield), NullWdLNfEnumArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullWdLWfEnumArrayPropield), NullWdLWfEnumArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(SByteArrayPropield), SByteArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(ShortArrayPropield), ShortArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(HalfArrayPropield), HalfArrayPropield, formatFlags: EnsureFormattedDelimited);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(UIntArrayPropield), UIntArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(LongArrayPropield), LongArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(DoubleArrayPropield), DoubleArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(VeryLongArrayPropield), VeryLongArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(BigIntArrayPropield), BigIntArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(DateTimeArrayPropield), DateTimeArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(TimeSpanArrayPropield), TimeSpanArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(RuneArrayPropield), RuneArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(IpNetworkArrayPropield), IpNetworkArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullByteArrayPropield), NullByteArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullCharArrayPropield), NullCharArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullUShortArrayPropield), NullUShortArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullIntArrayPropield), NullIntArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullFloatArrayPropield), NullFloatArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullULongArrayPropield), NullULongArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullDecimalArrayPropield), NullDecimalArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullVeryUlongArrayPropield), NullVeryUlongArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullDateTimeArrayPropield), NullDateTimeArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullTimeSpanArrayPropield), NullTimeSpanArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullRuneArrayPropield), NullRuneArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullIpNetworkArrayPropield), NullIpNetworkArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(StringBuilderArrayPropield), StringBuilderArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(VersionArrayPropield), VersionArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(UriArrayPropield), UriArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NdLNfEnumArrayPropield), NdLNfEnumArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NdLWfEnumArrayPropield), NdLWfEnumArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(WdUNfEnumArrayPropield), WdUNfEnumArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(WdUWfEnumArrayPropield), WdUWfEnumArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullNdLNfEnumArrayPropield), NullNdLNfEnumArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullNdLWfEnumArrayPropield), NullNdLWfEnumArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullWdUNfEnumArrayPropield), NullWdUNfEnumArrayPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullWdUWfEnumArrayPropield), NullWdUWfEnumArrayPropield);
        return ctb.Complete();
    }

    public StateExtractStringRange AlwaysAddFiltered(ITheOneString tos)
    {
        using var ctb =
            tos.StartComplexType(this);
        ctb.CollectionField.AlwaysAddFiltered(nameof(ByteArrayPropield), ByteArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(ByteArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(SByteArrayPropield), SByteArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(SByteArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(CharArrayPropield), CharArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(CharArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(ShortArrayPropield), ShortArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(ShortArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(UShortArrayPropield), UShortArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(UShortArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(HalfArrayPropield), HalfArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(HalfArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(IntArrayPropield), IntArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(IntArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(UIntArrayPropield), UIntArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(UIntArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(FloatArrayPropield), FloatArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(FloatArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(LongArrayPropield), LongArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(LongArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(ULongArrayPropield), ULongArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(ULongArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(DoubleArrayPropield), DoubleArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(DoubleArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(DecimalArrayPropield), DecimalArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(DecimalArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(VeryLongArrayPropield), VeryLongArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(VeryLongArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(VeryUlongArrayPropield), VeryUlongArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(VeryUlongArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(BigIntArrayPropield), BigIntArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(BigIntArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(ComplexArrayPropield), ComplexArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(ComplexArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(DateTimeArrayPropield), DateTimeArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(DateTimeArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(DateOnlyArrayPropield), DateOnlyArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(DateOnlyArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(TimeSpanArrayPropield), TimeSpanArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(TimeSpanArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(TimeOnlyArrayPropield), TimeOnlyArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(TimeOnlyArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(RuneArrayPropield), RuneArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(RuneArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(GuidArrayPropield), GuidArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(GuidArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(IpNetworkArrayPropield), IpNetworkArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(IpNetworkArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullByteArrayPropield), NullByteArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullByteArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullSByteArrayPropield), NullSByteArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullSByteArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullCharArrayPropield), NullCharArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullCharArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullShortArrayPropield), NullShortArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullShortArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullUShortArrayPropield), NullUShortArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullUShortArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullHalfArrayPropield), NullHalfArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullHalfArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullIntArrayPropield), NullIntArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullIntArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullUIntArrayPropield), NullUIntArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullUIntArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullFloatArrayPropield), NullFloatArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullFloatArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullLongArrayPropield), NullLongArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullLongArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullULongArrayPropield), NullULongArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullULongArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullDoubleArrayPropield), NullDoubleArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullDoubleArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullDecimalArrayPropield), NullDecimalArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullDecimalArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullVeryLongArrayPropield), NullVeryLongArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullVeryLongArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullVeryUlongArrayPropield), NullVeryUlongArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullVeryUlongArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullBigIntArrayPropield), NullBigIntArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullBigIntArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullComplexArrayPropield), NullComplexArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullComplexArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullDateTimeArrayPropield), NullDateTimeArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullDateTimeArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullDateOnlyArrayPropield), NullDateOnlyArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullDateOnlyArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullTimeSpanArrayPropield), NullTimeSpanArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullTimeSpanArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullTimeOnlyArrayPropield), NullTimeOnlyArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullTimeOnlyArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullRuneArrayPropield), NullRuneArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullRuneArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullGuidArrayPropield), NullGuidArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullGuidArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullIpNetworkArrayPropield), NullIpNetworkArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullIpNetworkArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(StringArrayPropield), StringArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(StringArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(StringBuilderArrayPropield), StringBuilderArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(StringBuilderArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFilteredCharSeq(nameof(CharSequenceArrayPropield), CharSequenceArrayPropield
                                                        , FilterRegistry.OrderedCollectionFilterDefault(CharSequenceArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(VersionArrayPropield), VersionArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(VersionArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(IpAddressArrayPropield), IpAddressArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(IpAddressArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(UriArrayPropield), UriArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(UriArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(SpanFormattableArrayPropield), SpanFormattableArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(SpanFormattableArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NdLNfEnumArrayPropield), NdLNfEnumArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NdLNfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NdUNfEnumArrayPropield), NdUNfEnumArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NdUNfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NdLWfEnumArrayPropield), NdLWfEnumArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NdLWfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NdUWfEnumArrayPropield), NdUWfEnumArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NdUWfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(WdLNfEnumArrayPropield), WdLNfEnumArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(WdLNfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(WdUNfEnumArrayPropield), WdUNfEnumArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(WdUNfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(WdLWfEnumArrayPropield), WdLWfEnumArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(WdLWfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(WdUWfEnumArrayPropield), WdUWfEnumArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(WdUWfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullNdLNfEnumArrayPropield), NullNdLNfEnumArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullNdLNfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullNdUNfEnumArrayPropield), NullNdUNfEnumArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullNdUNfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullNdLWfEnumArrayPropield), NullNdLWfEnumArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullNdLWfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullNdUWfEnumArrayPropield), NullNdUWfEnumArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullNdUWfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullWdLNfEnumArrayPropield), NullWdLNfEnumArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullWdLNfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullWdUNfEnumArrayPropield), NullWdUNfEnumArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullWdUNfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullWdLWfEnumArrayPropield), NullWdLWfEnumArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullWdLWfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullWdUWfEnumArrayPropield), NullWdUWfEnumArrayPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullWdUWfEnumArrayPropield).CheckPredicate);
        return ctb.Complete();
    }

    public StateExtractStringRange WhenPopulatedWithFilterReveal(ITheOneString tos)
    {
        using var ctb = tos.StartComplexType(this);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(ByteArrayPropield), ByteArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(ByteArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(SByteArrayPropield), SByteArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(SByteArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(CharArrayPropield), CharArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(CharArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(ShortArrayPropield), ShortArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(ShortArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(UShortArrayPropield), UShortArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(UShortArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(HalfArrayPropield), HalfArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(HalfArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(IntArrayPropield), IntArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(IntArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(UIntArrayPropield), UIntArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(UIntArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(FloatArrayPropield), FloatArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(FloatArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(LongArrayPropield), LongArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(LongArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(ULongArrayPropield), ULongArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(ULongArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(DoubleArrayPropield), DoubleArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(DoubleArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(DecimalArrayPropield), DecimalArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(DecimalArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(VeryLongArrayPropield), VeryLongArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(VeryLongArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(VeryUlongArrayPropield), VeryUlongArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(VeryUlongArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(BigIntArrayPropield), BigIntArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(BigIntArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(ComplexArrayPropield), ComplexArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(ComplexArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(DateTimeArrayPropield), DateTimeArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(DateTimeArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(DateOnlyArrayPropield), DateOnlyArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(DateOnlyArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(TimeSpanArrayPropield), TimeSpanArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(TimeSpanArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(TimeOnlyArrayPropield), TimeOnlyArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(TimeOnlyArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(RuneArrayPropield), RuneArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(RuneArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(GuidArrayPropield), GuidArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(GuidArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(IpNetworkArrayPropield), IpNetworkArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(IpNetworkArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullByteArrayPropield), NullByteArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullByteArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullSByteArrayPropield), NullSByteArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullSByteArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullCharArrayPropield), NullCharArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullCharArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullShortArrayPropield), NullShortArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullShortArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullUShortArrayPropield), NullUShortArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullUShortArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullHalfArrayPropield), NullHalfArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullHalfArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullIntArrayPropield), NullIntArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullIntArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullUIntArrayPropield), NullUIntArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullUIntArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullFloatArrayPropield), NullFloatArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullFloatArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullLongArrayPropield), NullLongArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullLongArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullULongArrayPropield), NullULongArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullULongArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullDoubleArrayPropield), NullDoubleArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullDoubleArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullDecimalArrayPropield), NullDecimalArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullDecimalArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullVeryLongArrayPropield), NullVeryLongArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullVeryLongArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullVeryUlongArrayPropield), NullVeryUlongArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullVeryUlongArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullBigIntArrayPropield), NullBigIntArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullBigIntArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullComplexArrayPropield), NullComplexArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullComplexArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullDateTimeArrayPropield), NullDateTimeArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullDateTimeArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullDateOnlyArrayPropield), NullDateOnlyArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullDateOnlyArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullTimeSpanArrayPropield), NullTimeSpanArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullTimeSpanArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullTimeOnlyArrayPropield), NullTimeOnlyArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullTimeOnlyArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullRuneArrayPropield), NullRuneArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullRuneArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullGuidArrayPropield), NullGuidArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullGuidArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullIpNetworkArrayPropield), NullIpNetworkArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullIpNetworkArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(StringArrayPropield), StringArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(StringArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(StringBuilderArrayPropield), StringBuilderArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(StringBuilderArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilterCharSeq(nameof(CharSequenceArrayPropield), CharSequenceArrayPropield
                                                              , FilterRegistry.OrderedCollectionFilterDefault(CharSequenceArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(VersionArrayPropield), VersionArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(VersionArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(IpAddressArrayPropield), IpAddressArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(IpAddressArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(UriArrayPropield), UriArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(UriArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(SpanFormattableArrayPropield), SpanFormattableArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(SpanFormattableArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NdLNfEnumArrayPropield), NdLNfEnumArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NdLNfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NdUNfEnumArrayPropield), NdUNfEnumArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NdUNfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NdLWfEnumArrayPropield), NdLWfEnumArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NdLWfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NdUWfEnumArrayPropield), NdUWfEnumArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NdUWfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(WdLNfEnumArrayPropield), WdLNfEnumArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(WdLNfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(WdUNfEnumArrayPropield), WdUNfEnumArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(WdUNfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(WdLWfEnumArrayPropield), WdLWfEnumArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(WdLWfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(WdUWfEnumArrayPropield), WdUWfEnumArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(WdUWfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullNdLNfEnumArrayPropield), NullNdLNfEnumArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullNdLNfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullNdUNfEnumArrayPropield), NullNdUNfEnumArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullNdUNfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullNdLWfEnumArrayPropield), NullNdLWfEnumArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullNdLWfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullNdUWfEnumArrayPropield), NullNdUWfEnumArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullNdUWfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullWdLNfEnumArrayPropield), NullWdLNfEnumArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullWdLNfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullWdUNfEnumArrayPropield), NullWdUNfEnumArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullWdUNfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullWdLWfEnumArrayPropield), NullWdLWfEnumArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullWdLWfEnumArrayPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullWdUWfEnumArrayPropield), NullWdUWfEnumArrayPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullWdUWfEnumArrayPropield).CheckPredicate);
        return ctb.Complete();
    }
}

[NoMatchingProductionClass]
public struct StandardArrayPropertyFieldStruct
{
    public StandardArrayPropertyFieldStruct()
    {
        InitializeAllSet();
    }

    public void InitializeAllSet()
    {
        ByteArrayPropield      = [byte.MinValue, 0, byte.MaxValue];
        SByteArrayPropield     = [sbyte.MinValue, 0, sbyte.MaxValue];
        CharArrayPropield      = [' ', '\u0000', '\uFFFF'];
        ShortArrayPropield     = [short.MinValue, 0, short.MaxValue];
        UShortArrayPropield    = [ushort.MinValue, 0, ushort.MaxValue];
        HalfArrayPropield      = [Half.MinValue, default, Half.NaN, Half.MaxValue];
        IntArrayPropield       = [int.MinValue, 0, int.MaxValue];
        UIntArrayPropield      = [uint.MinValue, 0, uint.MaxValue];
        FloatArrayPropield     = [float.MinValue, 0, float.NaN, float.MaxValue];
        LongArrayPropield      = [long.MinValue, 0, long.MaxValue];
        ULongArrayPropield     = [ulong.MinValue, 0, ulong.MaxValue];
        DoubleArrayPropield    = [double.MinValue, 0, double.NaN, double.MaxValue];
        DecimalArrayPropield   = [decimal.MinValue, 0, decimal.MaxValue];
        VeryLongArrayPropield  = [Int128.MinValue, default, Int128.MaxValue];
        VeryUlongArrayPropield = [UInt128.MinValue, default, UInt128.MaxValue];
        BigIntArrayPropield    = [BigInteger.Parse("-99999999999999999999999999"), default, BigInteger.Parse("99999999999999999999999999")];
        ComplexArrayPropield   = [new Complex(double.MaxValue * -1.0, double.MaxValue * -1), default, new Complex(double.MaxValue, double.MaxValue)];
        DateTimeArrayPropield  = [DateTime.MinValue, default, DateTime.MaxValue];
        DateOnlyArrayPropield  = [DateOnly.MinValue, default, DateOnly.MaxValue];
        TimeSpanArrayPropield  = [TimeSpan.MinValue, TimeSpan.Zero, TimeSpan.MaxValue];
        TimeOnlyArrayPropield  = [TimeOnly.MinValue, default, TimeOnly.MaxValue];
        RuneArrayPropield      = [Rune.GetRuneAt("\U00010000", 0), default, Rune.GetRuneAt("\U0010FFFF", 0)];
        GuidArrayPropield =
            [Guid.ParseExact("00000000-0000-0000-0000-000000000000", "X"), Guid.Empty, Guid.ParseExact("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", "X")];
        IpNetworkArrayPropield = [new IPNetwork(new IPAddress("\0\0\0\0"u8.ToArray()), 0), default, IPNetwork.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff")];

        NullByteArrayPropield      = [byte.MinValue, 0, null, byte.MaxValue];
        NullSByteArrayPropield     = [sbyte.MinValue, 0, null, sbyte.MaxValue];
        NullCharArrayPropield      = [' ', '\u0000', null, '\uFFFF'];
        NullShortArrayPropield     = [short.MinValue, 0, null, short.MaxValue];
        NullUShortArrayPropield    = [ushort.MinValue, 0, null, ushort.MaxValue];
        NullHalfArrayPropield      = [Half.MinValue, Half.Zero, Half.NaN, null, Half.MaxValue];
        NullIntArrayPropield       = [int.MinValue, 0, null, int.MaxValue];
        NullUIntArrayPropield      = [uint.MinValue, 0, null, uint.MaxValue];
        NullFloatArrayPropield     = [float.MinValue, 0f, float.NaN, null, float.MaxValue];
        NullLongArrayPropield      = [long.MinValue, 0, null, long.MaxValue];
        NullULongArrayPropield     = [ulong.MinValue, 0, null, ulong.MaxValue];
        NullDoubleArrayPropield    = [double.MinValue, 0d, double.NaN, null, double.MaxValue];
        NullDecimalArrayPropield   = [decimal.MinValue, 0m, null, decimal.MaxValue];
        NullVeryLongArrayPropield  = [Int128.MinValue, Int128.Zero, null, Int128.MaxValue];
        NullVeryUlongArrayPropield = [UInt128.MinValue, UInt128.Zero, null, UInt128.MaxValue];
        NullBigIntArrayPropield    = [BigInteger.Parse("-99999999999999999999999999"), BigInteger.Zero, null, BigInteger.Parse("99999999999999999999999999")];
        NullComplexArrayPropield =
            [new Complex(double.MaxValue * -1.0, double.MaxValue * -1), Complex.Zero, null, new Complex(double.MaxValue, double.MaxValue)];
        NullDateTimeArrayPropield = [DateTime.MinValue, new DateTime(), null, DateTime.MaxValue];
        NullDateOnlyArrayPropield = [DateOnly.MinValue, new DateOnly(), null, DateOnly.MaxValue];
        NullTimeSpanArrayPropield = [TimeSpan.MinValue, TimeSpan.Zero, null, TimeSpan.MaxValue];
        NullTimeOnlyArrayPropield = [TimeOnly.MinValue, null, TimeOnly.MaxValue];
        NullRuneArrayPropield     = [Rune.GetRuneAt("\U00010000", 0), Rune.GetRuneAt("\u0000", 0), null, Rune.GetRuneAt("\U0010FFFF", 0)];
        NullGuidArrayPropield =
            [Guid.ParseExact("00000000-0000-0000-0000-000000000000", "X"), Guid.Empty, null, Guid.ParseExact("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", "X")];
        NullIpNetworkArrayPropield =
            [new IPNetwork(new IPAddress("\0\0\0\0"u8.ToArray()), 0), new IPNetwork(), null, IPNetwork.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff")];

        StringArrayPropield        = ["stringArrayPropield_1", "", null!, "stringArrayPropield_4"];
        StringBuilderArrayPropield = [new("stringBuilderArrayPropield_1"), new StringBuilder(), null!, new StringBuilder("stringBuilderArrayPropield_4")];
        CharSequenceArrayPropield =
            [new MutableString("charSequenceArrayPropield_1"), new MutableString(), null!, new MutableString("charSequenceArrayPropield_4")];

        VersionArrayPropield = [new Version(0, 0), null!, new Version(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue)];
        IntPtrArrayPropield  = [new IPAddress("\0\0\0\0"u8.ToArray()), null!, IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff")];
        UriArrayPropield     = [new Uri(""), null!, new Uri("https://github.com/shwaindog/Fortitude")];

        SpanFormattableArrayPropield = [new MySpanFormattableClass(""), null!, new MySpanFormattableClass("SpanFormattableSingPropield")];
        NdLNfEnumArrayPropield       = [NoDefaultLongNoFlagsEnum.NDLNFE_1, default, NoDefaultLongNoFlagsEnum.NDLNFE_34];
        NdUNfEnumArrayPropield       = [NoDefaultULongNoFlagsEnum.NDUNFE_1, default, NoDefaultULongNoFlagsEnum.NDUNFE_34];
        NdLWfEnumArrayPropield =
        [
            NoDefaultLongWithFlagsEnum.NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_2, default
          , NoDefaultLongWithFlagsEnum.NDLWFE_33 | NoDefaultLongWithFlagsEnum.NDLWFE_34
        ];
        NdUWfEnumArrayPropield =
        [
            NoDefaultULongWithFlagsEnum.NDUWFE_1 | NoDefaultULongWithFlagsEnum.NDUWFE_2, default
          , NoDefaultULongWithFlagsEnum.NDUWFE_33 | NoDefaultULongWithFlagsEnum.NDUWFE_34
        ];

        WdLNfEnumArrayPropield = [WithDefaultLongNoFlagsEnum.WDLNFE_1, default, WithDefaultLongNoFlagsEnum.WDLNFE_34];
        WdUNfEnumArrayPropield = [WithDefaultULongNoFlagsEnum.WDUNFE_1, default, WithDefaultULongNoFlagsEnum.WDUNFE_34];
        WdLWfEnumArrayPropield =
        [
            WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_2, default
          , WithDefaultLongWithFlagsEnum.WDLWFE_33 | WithDefaultLongWithFlagsEnum.WDLWFE_34
        ];
        WdUWfEnumArrayPropield =
        [
            WithDefaultULongWithFlagsEnum.WDUWFE_1 | WithDefaultULongWithFlagsEnum.WDUWFE_2, default
          , WithDefaultULongWithFlagsEnum.WDUWFE_33 | WithDefaultULongWithFlagsEnum.WDUWFE_34
        ];

        NullNdLNfEnumArrayPropield = [NoDefaultLongNoFlagsEnum.NDLNFE_1, default(NoDefaultLongNoFlagsEnum), null, NoDefaultLongNoFlagsEnum.NDLNFE_34];
        NullNdUNfEnumArrayPropield = [NoDefaultULongNoFlagsEnum.NDUNFE_1, default(NoDefaultULongNoFlagsEnum), null, NoDefaultULongNoFlagsEnum.NDUNFE_34];
        NullNdLWfEnumArrayPropield =
        [
            NoDefaultLongWithFlagsEnum.NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_2, default(NoDefaultLongWithFlagsEnum), null
          , NoDefaultLongWithFlagsEnum.NDLWFE_33 | NoDefaultLongWithFlagsEnum.NDLWFE_34
        ];
        NullNdUWfEnumArrayPropield =
        [
            NoDefaultULongWithFlagsEnum.NDUWFE_1 | NoDefaultULongWithFlagsEnum.NDUWFE_2, default(NoDefaultULongWithFlagsEnum), null
          , NoDefaultULongWithFlagsEnum.NDUWFE_33 | NoDefaultULongWithFlagsEnum.NDUWFE_34
        ];

        NullWdLNfEnumArrayPropield = [WithDefaultLongNoFlagsEnum.WDLNFE_1, default(WithDefaultLongNoFlagsEnum), null, WithDefaultLongNoFlagsEnum.WDLNFE_34];
        NullWdUNfEnumArrayPropield = [WithDefaultULongNoFlagsEnum.WDUNFE_1, default(WithDefaultULongNoFlagsEnum), null, WithDefaultULongNoFlagsEnum.WDUNFE_34];
        NullWdLWfEnumArrayPropield =
        [
            WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_2, default(WithDefaultLongWithFlagsEnum), null
          , WithDefaultLongWithFlagsEnum.WDLWFE_33 | WithDefaultLongWithFlagsEnum.WDLWFE_34
        ];
        NullWdUWfEnumArrayPropield =
        [
            WithDefaultULongWithFlagsEnum.WDUWFE_1 | WithDefaultULongWithFlagsEnum.WDUWFE_2, default(WithDefaultULongWithFlagsEnum), null
          , WithDefaultULongWithFlagsEnum.WDUWFE_33 | WithDefaultULongWithFlagsEnum.WDUWFE_34
        ];
    }

    public void InitializeAtSize(int numberToGenerate)
    {
        ByteArrayPropield      = NumberTestDataGenerator.GenRandomNumberRange<byte>(numberToGenerate).ToArray();
        SByteArrayPropield     = NumberTestDataGenerator.GenRandomNumberRange<sbyte>(numberToGenerate).ToArray();
        CharArrayPropield      = NumberTestDataGenerator.GenRandomNumberRange<char>(numberToGenerate).ToArray();
        ShortArrayPropield     = NumberTestDataGenerator.GenRandomNumberRange<short>(numberToGenerate).ToArray();
        UShortArrayPropield    = NumberTestDataGenerator.GenRandomNumberRange<ushort>(numberToGenerate).ToArray();
        HalfArrayPropield      = NumberTestDataGenerator.GenRandomNumberRange<Half>(numberToGenerate).ToArray();
        IntArrayPropield       = NumberTestDataGenerator.GenRandomNumberRange<int>(numberToGenerate).ToArray();
        UIntArrayPropield      = NumberTestDataGenerator.GenRandomNumberRange<uint>(numberToGenerate).ToArray();
        FloatArrayPropield     = NumberTestDataGenerator.GenRandomNumberRange<float>(numberToGenerate).ToArray();
        LongArrayPropield      = NumberTestDataGenerator.GenRandomNumberRange<long>(numberToGenerate).ToArray();
        ULongArrayPropield     = NumberTestDataGenerator.GenRandomNumberRange<ulong>(numberToGenerate).ToArray();
        DoubleArrayPropield    = NumberTestDataGenerator.GenRandomNumberRange<double>(numberToGenerate).ToArray();
        DecimalArrayPropield   = NumberTestDataGenerator.GenRandomNumberRange<decimal>(numberToGenerate).ToArray();
        VeryLongArrayPropield  = NumberTestDataGenerator.GenRandomNumberRange<Int128>(numberToGenerate).ToArray();
        VeryUlongArrayPropield = NumberTestDataGenerator.GenRandomNumberRange<UInt128>(numberToGenerate).ToArray();
        BigIntArrayPropield    = NumberTestDataGenerator.GenRandomNumberRange<BigInteger>(numberToGenerate).ToArray();
        ComplexArrayPropield   = NumberTestDataGenerator.GenRandomNumberRange<Complex>(numberToGenerate).ToArray();
        DateTimeArrayPropield  = DateTimeTestDataGenerator.GenRandomDateTimeRange(numberToGenerate).ToArray();
        DateOnlyArrayPropield  = DateTimeTestDataGenerator.GenRandomDateOnlyRange(numberToGenerate).ToArray();
        TimeSpanArrayPropield  = DateTimeTestDataGenerator.GenRandomTimeSpanRange(numberToGenerate).ToArray();
        TimeOnlyArrayPropield  = DateTimeTestDataGenerator.GenRandomTimeOnlyRange(numberToGenerate).ToArray();
        RuneArrayPropield      = NumberTestDataGenerator.GenRandomNumberRange<Rune>(numberToGenerate).ToArray();
        GuidArrayPropield =
            NumberTestDataGenerator
                .GenRandomNumberRange<byte>(numberToGenerate)
                .Select(_ =>
                            new Guid(NumberTestDataGenerator
                                     .GenRandomNumberRange<
                                         byte>(16).ToArray()))
                .ToArray();
        IpNetworkArrayPropield =
            NumberTestDataGenerator
                .GenRandomNumberRange<byte>(numberToGenerate)
                .Select(b =>
                {
                    if (b % 2 == 0)
                    {
                        return new IPNetwork
                            (new IPAddress
                                 (NumberTestDataGenerator.GenRandomNumberRange<byte>(4).ToArray()), (b % 32));
                    }
                    return new IPNetwork
                        (new IPAddress
                             (NumberTestDataGenerator.GenRandomNumberRange<byte>(16).ToArray()), (b % 128));
                }).ToArray();

        NullByteArrayPropield      = NumberTestDataGenerator.GenRandomNullableNumberRange<byte>(numberToGenerate).ToArray();
        NullSByteArrayPropield     = NumberTestDataGenerator.GenRandomNullableNumberRange<sbyte>(numberToGenerate).ToArray();
        NullCharArrayPropield      = NumberTestDataGenerator.GenRandomNullableNumberRange<char>(numberToGenerate).ToArray();
        NullShortArrayPropield     = NumberTestDataGenerator.GenRandomNullableNumberRange<short>(numberToGenerate).ToArray();
        NullUShortArrayPropield    = NumberTestDataGenerator.GenRandomNullableNumberRange<ushort>(numberToGenerate).ToArray();
        NullHalfArrayPropield      = NumberTestDataGenerator.GenRandomNullableNumberRange<Half>(numberToGenerate).ToArray();
        NullIntArrayPropield       = NumberTestDataGenerator.GenRandomNullableNumberRange<int>(numberToGenerate).ToArray();
        NullUIntArrayPropield      = NumberTestDataGenerator.GenRandomNullableNumberRange<uint>(numberToGenerate).ToArray();
        NullFloatArrayPropield     = NumberTestDataGenerator.GenRandomNullableNumberRange<float>(numberToGenerate).ToArray();
        NullLongArrayPropield      = NumberTestDataGenerator.GenRandomNullableNumberRange<long>(numberToGenerate).ToArray();
        NullULongArrayPropield     = NumberTestDataGenerator.GenRandomNullableNumberRange<ulong>(numberToGenerate).ToArray();
        NullDoubleArrayPropield    = NumberTestDataGenerator.GenRandomNullableNumberRange<double>(numberToGenerate).ToArray();
        NullDecimalArrayPropield   = NumberTestDataGenerator.GenRandomNullableNumberRange<decimal>(numberToGenerate).ToArray();
        NullVeryLongArrayPropield  = NumberTestDataGenerator.GenRandomNullableNumberRange<Int128>(numberToGenerate).ToArray();
        NullVeryUlongArrayPropield = NumberTestDataGenerator.GenRandomNullableNumberRange<UInt128>(numberToGenerate).ToArray();
        NullBigIntArrayPropield    = NumberTestDataGenerator.GenRandomNullableNumberRange<BigInteger>(numberToGenerate).ToArray();
        NullComplexArrayPropield   = NumberTestDataGenerator.GenRandomNullableNumberRange<Complex>(numberToGenerate).ToArray();
        NullDateTimeArrayPropield  = DateTimeTestDataGenerator.GenRandomNullableDateTimeRange(numberToGenerate).ToArray();
        NullDateOnlyArrayPropield  = DateTimeTestDataGenerator.GenRandomNullableDateOnlyRange(numberToGenerate).ToArray();
        NullTimeSpanArrayPropield  = DateTimeTestDataGenerator.GenRandomNullableTimeSpanRange(numberToGenerate).ToArray();
        NullTimeOnlyArrayPropield  = DateTimeTestDataGenerator.GenRandomNullableTimeOnlyRange(numberToGenerate).ToArray();
        NullRuneArrayPropield      = NumberTestDataGenerator.GenRandomNullableNumberRange<Rune>(numberToGenerate).ToArray();

        NullGuidArrayPropield =
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<byte>(numberToGenerate)
                .Select(b =>
                        {
                            if (b == null) return null;
                            return new Guid?(new Guid(NumberTestDataGenerator
                                                      .GenRandomNumberRange<
                                                          byte>(16).ToArray()));
                        }
                       )
                .ToArray();
        NullIpNetworkArrayPropield =
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<byte>(numberToGenerate)
                .Select(b =>
                {
                    if (b == null) return null;
                    if (b % 2 == 0)
                    {
                        return new IPNetwork?(new IPNetwork
                                                  (new IPAddress
                                                       (NumberTestDataGenerator.GenRandomNumberRange<byte>(4).ToArray()), (b.Value % 32)));
                    }
                    return new IPNetwork(new IPAddress
                                             (NumberTestDataGenerator.GenRandomNumberRange<byte>(16).ToArray()), (b.Value % 128));
                }).ToArray();

        StringArrayPropield =
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<int>(numberToGenerate)
                .Select(num =>
                {
                    if (num == null) return null!;
                    return "stringArrayPropield_" + num;
                }).ToArray();
        StringBuilderArrayPropield =
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<int>(numberToGenerate)
                .Select(num =>
                {
                    if (num == null) return null!;
                    return new StringBuilder("stringBuilderArrayPropield_1" + num);
                }).ToArray();
        CharSequenceArrayPropield =
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<int>(numberToGenerate)
                .Select(num =>
                {
                    if (num == null) return null!;
                    return (ICharSequence)new MutableString("charSequenceArrayPropield_1" + num);
                }).ToArray();

        VersionArrayPropield =
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<int>(numberToGenerate)
                .Select(num =>
                {
                    if (num == null) return null!;
                    var otherThree = NumberTestDataGenerator
                                     .GenRandomNumberRange<int>(numberToGenerate).Select(Math.Abs).ToArray();
                    return new Version(Math.Abs(num.Value), otherThree[0], otherThree[1], otherThree[2]);
                }).ToArray();

        IntPtrArrayPropield =
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<byte>(numberToGenerate)
                .Select(b =>
                {
                    if (b == null) return null!;
                    if (b % 2 == 0)
                    {
                        return new IPAddress(NumberTestDataGenerator.GenRandomNumberRange<byte>(4).ToArray());
                    }
                    return new IPAddress(NumberTestDataGenerator.GenRandomNumberRange<byte>(16).ToArray());
                }).ToArray();
        UriArrayPropield =
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<int>(numberToGenerate)
                .Select(num =>
                {
                    if (num == null) return null!;
                    return new Uri("https://www.someWebAddress_" + num + ".net");
                }).ToArray();

        SpanFormattableArrayPropield =
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<int>(numberToGenerate)
                .Select(num =>
                {
                    if (num == null) return null!;
                    return new MySpanFormattableClass(" SpanFormattableArrayPropield_" + num);
                }).ToArray();

        NdLNfEnumArrayPropield = EnumTestDataGenerator.GenRandomEnumValues<NoDefaultLongNoFlagsEnum>(numberToGenerate).ToArray();
        NdUNfEnumArrayPropield = EnumTestDataGenerator.GenRandomEnumValues<NoDefaultULongNoFlagsEnum>(numberToGenerate).ToArray();
        NdLWfEnumArrayPropield = EnumTestDataGenerator.GenRandomEnumMultiFlagValues<NoDefaultLongWithFlagsEnum>(numberToGenerate).ToArray();
        NdUWfEnumArrayPropield = EnumTestDataGenerator.GenRandomEnumMultiFlagValues<NoDefaultULongWithFlagsEnum>(numberToGenerate).ToArray();

        WdLNfEnumArrayPropield = EnumTestDataGenerator.GenRandomEnumValues<WithDefaultLongNoFlagsEnum>(numberToGenerate).ToArray();
        WdUNfEnumArrayPropield = EnumTestDataGenerator.GenRandomEnumValues<WithDefaultULongNoFlagsEnum>(numberToGenerate).ToArray();
        WdLWfEnumArrayPropield = EnumTestDataGenerator.GenRandomEnumMultiFlagValues<WithDefaultLongWithFlagsEnum>(numberToGenerate).ToArray();
        WdUWfEnumArrayPropield = EnumTestDataGenerator.GenRandomEnumMultiFlagValues<WithDefaultULongWithFlagsEnum>(numberToGenerate).ToArray();

        NullNdLNfEnumArrayPropield = EnumTestDataGenerator.GenRandomNullableEnumValues<NoDefaultLongNoFlagsEnum>(numberToGenerate).ToArray();
        NullNdUNfEnumArrayPropield = EnumTestDataGenerator.GenRandomNullableEnumValues<NoDefaultULongNoFlagsEnum>(numberToGenerate).ToArray();
        NullNdLWfEnumArrayPropield = EnumTestDataGenerator.GenRandomNullableEnumMultiFlagValues<NoDefaultLongWithFlagsEnum>(numberToGenerate).ToArray();
        NullNdUWfEnumArrayPropield = EnumTestDataGenerator.GenRandomNullableEnumMultiFlagValues<NoDefaultULongWithFlagsEnum>(numberToGenerate).ToArray();

        NullWdLNfEnumArrayPropield = EnumTestDataGenerator.GenRandomNullableEnumValues<WithDefaultLongNoFlagsEnum>(numberToGenerate).ToArray();
        NullWdUNfEnumArrayPropield = EnumTestDataGenerator.GenRandomNullableEnumValues<WithDefaultULongNoFlagsEnum>(numberToGenerate).ToArray();
        NullWdLWfEnumArrayPropield = EnumTestDataGenerator.GenRandomNullableEnumMultiFlagValues<WithDefaultLongWithFlagsEnum>(numberToGenerate).ToArray();
        NullWdUWfEnumArrayPropield = EnumTestDataGenerator.GenRandomNullableEnumMultiFlagValues<WithDefaultULongWithFlagsEnum>(numberToGenerate).ToArray();
    }


    public void InitializeAllNull()
    {
        ByteArrayPropield      = null!;
        SByteArrayPropield     = null!;
        CharArrayPropield      = null!;
        ShortArrayPropield     = null!;
        UShortArrayPropield    = null!;
        HalfArrayPropield      = null!;
        IntArrayPropield       = null!;
        UIntArrayPropield      = null!;
        FloatArrayPropield     = null!;
        LongArrayPropield      = null!;
        ULongArrayPropield     = null!;
        DoubleArrayPropield    = null!;
        DecimalArrayPropield   = null!;
        VeryLongArrayPropield  = null!;
        VeryUlongArrayPropield = null!;
        BigIntArrayPropield    = null!;

        ComplexArrayPropield   = null!;
        DateTimeArrayPropield  = null!;
        DateOnlyArrayPropield  = null!;
        TimeSpanArrayPropield  = null!;
        TimeOnlyArrayPropield  = null!;
        RuneArrayPropield      = null!;
        GuidArrayPropield      = null!;
        IpNetworkArrayPropield = null!;

        NullByteArrayPropield      = null!;
        NullSByteArrayPropield     = null!;
        NullCharArrayPropield      = null!;
        NullShortArrayPropield     = null!;
        NullUShortArrayPropield    = null!;
        NullHalfArrayPropield      = null!;
        NullIntArrayPropield       = null!;
        NullUIntArrayPropield      = null!;
        NullFloatArrayPropield     = null!;
        NullLongArrayPropield      = null!;
        NullULongArrayPropield     = null!;
        NullDoubleArrayPropield    = null!;
        NullDecimalArrayPropield   = null!;
        NullVeryLongArrayPropield  = null!;
        NullVeryUlongArrayPropield = null!;
        NullBigIntArrayPropield    = null!;

        NullComplexArrayPropield   = null!;
        NullDateTimeArrayPropield  = null!;
        NullDateOnlyArrayPropield  = null!;
        NullTimeSpanArrayPropield  = null!;
        NullTimeOnlyArrayPropield  = null!;
        NullRuneArrayPropield      = null!;
        NullGuidArrayPropield      = null!;
        NullIpNetworkArrayPropield = null!;

        StringArrayPropield        = null!;
        StringBuilderArrayPropield = null!;
        CharSequenceArrayPropield  = null!;

        VersionArrayPropield = null!;
        IntPtrArrayPropield  = null!;
        UriArrayPropield     = null!;

        SpanFormattableArrayPropield = null!;

        NdLNfEnumArrayPropield = null!;
        NdUNfEnumArrayPropield = null!;
        NdLWfEnumArrayPropield = null!;
        NdUWfEnumArrayPropield = null!;

        WdLNfEnumArrayPropield = null!;
        WdUNfEnumArrayPropield = null!;
        WdLWfEnumArrayPropield = null!;
        WdUWfEnumArrayPropield = null!;

        NullNdLNfEnumArrayPropield = null!;
        NullNdUNfEnumArrayPropield = null!;
        NullNdLWfEnumArrayPropield = null!;
        NullNdUWfEnumArrayPropield = null!;

        NullWdLNfEnumArrayPropield = null!;
        NullWdUNfEnumArrayPropield = null!;
        NullWdLWfEnumArrayPropield = null!;
        NullWdUWfEnumArrayPropield = null!;
    }

    public byte[] ByteArrayPropield = null!;
    public sbyte[] SByteArrayPropield { get; set; } = null!;
    public char[] CharArrayPropield = null!;
    public short[] ShortArrayPropield { get; set; } = null!;
    public ushort[] UShortArrayPropield = null!;
    public Half[] HalfArrayPropield { get; set; } = null!;
    public int[] IntArrayPropield = null!;
    public uint[] UIntArrayPropield { get; set; } = null!;
    public float[] FloatArrayPropield = null!;
    public long[] LongArrayPropield { get; set; } = null!;
    public ulong[] ULongArrayPropield = null!;
    public double[] DoubleArrayPropield { get; set; } = null!;
    public decimal[] DecimalArrayPropield = null!;
    public Int128[] VeryLongArrayPropield { get; set; } = null!;
    public UInt128[] VeryUlongArrayPropield = null!;
    public BigInteger[] BigIntArrayPropield { get; set; } = null!;
    public Complex[] ComplexArrayPropield = null!;
    public DateTime[] DateTimeArrayPropield { get; set; } = null!;
    public DateOnly[] DateOnlyArrayPropield = null!;
    public TimeSpan[] TimeSpanArrayPropield { get; set; } = null!;
    public TimeOnly[] TimeOnlyArrayPropield = null!;
    public Rune[] RuneArrayPropield { get; set; } = null!;
    public Guid[] GuidArrayPropield = null!;
    public IPNetwork[] IpNetworkArrayPropield { get; set; } = null!;

    public byte?[] NullByteArrayPropield { get; set; } = null!;
    public sbyte?[] NullSByteArrayPropield = null!;
    public char?[] NullCharArrayPropield { get; set; } = null!;
    public short?[] NullShortArrayPropield = null!;
    public ushort?[] NullUShortArrayPropield { get; set; } = null!;
    public Half?[] NullHalfArrayPropield = null!;
    public int?[] NullIntArrayPropield { get; set; } = null!;
    public uint?[] NullUIntArrayPropield = null!;
    public float?[] NullFloatArrayPropield { get; set; } = null!;
    public long?[] NullLongArrayPropield = null!;
    public ulong?[] NullULongArrayPropield { get; set; } = null!;
    public double?[] NullDoubleArrayPropield = null!;
    public decimal?[] NullDecimalArrayPropield { get; set; } = null!;
    public Int128?[] NullVeryLongArrayPropield = null!;
    public UInt128?[] NullVeryUlongArrayPropield { get; set; } = null!;
    public BigInteger?[] NullBigIntArrayPropield = null!;

    public Complex?[] NullComplexArrayPropield = null!;
    public DateTime?[] NullDateTimeArrayPropield { get; set; } = null!;
    public DateOnly?[] NullDateOnlyArrayPropield = null!;
    public TimeSpan?[] NullTimeSpanArrayPropield { get; set; } = null!;
    public TimeOnly?[] NullTimeOnlyArrayPropield = null!;
    public Rune?[] NullRuneArrayPropield { get; set; } = null!;
    public Guid?[] NullGuidArrayPropield = null!;
    public IPNetwork?[] NullIpNetworkArrayPropield { get; set; } = null!;
    public string[] StringArrayPropield { get; set; } = null!;
    public StringBuilder[] StringBuilderArrayPropield = null!;
    public ICharSequence[] CharSequenceArrayPropield { get; set; } = null!;

    public Version[] VersionArrayPropield { get; set; } = null!;
    public IPAddress[] IntPtrArrayPropield = null!;
    public Uri[] UriArrayPropield { get; set; } = null!;

    public MySpanFormattableClass[] SpanFormattableArrayPropield = null!;

    public NoDefaultLongNoFlagsEnum[] NdLNfEnumArrayPropield { get; set; } = null!;
    public NoDefaultULongNoFlagsEnum[] NdUNfEnumArrayPropield = null!;
    public NoDefaultLongWithFlagsEnum[] NdLWfEnumArrayPropield { get; set; } = null!;
    public NoDefaultULongWithFlagsEnum[] NdUWfEnumArrayPropield = null!;

    public WithDefaultLongNoFlagsEnum[] WdLNfEnumArrayPropield = null!;
    public WithDefaultULongNoFlagsEnum[] WdUNfEnumArrayPropield { get; set; } = null!;
    public WithDefaultLongWithFlagsEnum[] WdLWfEnumArrayPropield = null!;
    public WithDefaultULongWithFlagsEnum[] WdUWfEnumArrayPropield { get; set; } = null!;

    public NoDefaultLongNoFlagsEnum?[] NullNdLNfEnumArrayPropield { get; set; } = null!;
    public NoDefaultULongNoFlagsEnum?[] NullNdUNfEnumArrayPropield = null!;
    public NoDefaultLongWithFlagsEnum?[] NullNdLWfEnumArrayPropield { get; set; } = null!;
    public NoDefaultULongWithFlagsEnum?[] NullNdUWfEnumArrayPropield = null!;

    public WithDefaultLongNoFlagsEnum?[] NullWdLNfEnumArrayPropield = null!;
    public WithDefaultULongNoFlagsEnum?[] NullWdUNfEnumArrayPropield { get; set; } = null!;
    public WithDefaultLongWithFlagsEnum?[] NullWdLWfEnumArrayPropield = null!;
    public WithDefaultULongWithFlagsEnum?[] NullWdUWfEnumArrayPropield { get; set; } = null!;

    public static IFilterRegistry FilterRegistry { get; set; } = new FilterRegistry(new AddOddRetrieveCountFactory());

    public static PalantírReveal<StandardArrayPropertyFieldStruct> SelectStateRevealer(TestCollectionFieldRevealMode testCollectionFieldRevealMode)
    {
        switch (testCollectionFieldRevealMode)
        {
            case TestCollectionFieldRevealMode.WhenPopulated:           return WhenPopulatedReveal;
            case TestCollectionFieldRevealMode.AlwaysFilter:            return AlwaysAddFiltered;
            case TestCollectionFieldRevealMode.WhenPopulatedWithFilter: return WhenPopulatedWithFilterReveal;
            case TestCollectionFieldRevealMode.AlwaysAddAll:
            default:
                return AlwaysRevealAllState;
        }
    }

    public static PalantírReveal<StandardArrayPropertyFieldStruct> AlwaysRevealAllState
    {
        get
        {
            return
                (sapfs, tos) =>
                {
                    using var ctb =
                        tos.StartComplexType(sapfs);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.ByteArrayPropield), sapfs.ByteArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.SByteArrayPropield), sapfs.SByteArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.CharArrayPropield), sapfs.CharArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.ShortArrayPropield), sapfs.ShortArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.UShortArrayPropield), sapfs.UShortArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.HalfArrayPropield), sapfs.HalfArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.IntArrayPropield), sapfs.IntArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.UIntArrayPropield), sapfs.UIntArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.FloatArrayPropield), sapfs.FloatArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.LongArrayPropield), sapfs.LongArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.ULongArrayPropield), sapfs.ULongArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.DoubleArrayPropield), sapfs.DoubleArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.DecimalArrayPropield), sapfs.DecimalArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.VeryLongArrayPropield), sapfs.VeryLongArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.VeryUlongArrayPropield), sapfs.VeryUlongArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.BigIntArrayPropield), sapfs.BigIntArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.ComplexArrayPropield), sapfs.ComplexArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.DateTimeArrayPropield), sapfs.DateTimeArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.DateOnlyArrayPropield), sapfs.DateOnlyArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.TimeSpanArrayPropield), sapfs.TimeSpanArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.TimeOnlyArrayPropield), sapfs.TimeOnlyArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.RuneArrayPropield), sapfs.RuneArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.GuidArrayPropield), sapfs.GuidArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.IpNetworkArrayPropield), sapfs.IpNetworkArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullByteArrayPropield), sapfs.NullByteArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullSByteArrayPropield), sapfs.NullSByteArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullCharArrayPropield), sapfs.NullCharArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullShortArrayPropield), sapfs.NullShortArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullUShortArrayPropield), sapfs.NullUShortArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullHalfArrayPropield), sapfs.NullHalfArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullIntArrayPropield), sapfs.NullIntArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullUIntArrayPropield), sapfs.NullUIntArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullFloatArrayPropield), sapfs.NullFloatArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullLongArrayPropield), sapfs.NullLongArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullULongArrayPropield), sapfs.NullULongArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullDoubleArrayPropield), sapfs.NullDoubleArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullDecimalArrayPropield), sapfs.NullDecimalArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullVeryLongArrayPropield), sapfs.NullVeryLongArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullVeryUlongArrayPropield), sapfs.NullVeryUlongArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullBigIntArrayPropield), sapfs.NullBigIntArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullComplexArrayPropield), sapfs.NullComplexArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullDateTimeArrayPropield), sapfs.NullDateTimeArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullDateOnlyArrayPropield), sapfs.NullDateOnlyArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullTimeSpanArrayPropield), sapfs.NullTimeSpanArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullTimeOnlyArrayPropield), sapfs.NullTimeOnlyArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullRuneArrayPropield), sapfs.NullRuneArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullGuidArrayPropield), sapfs.NullGuidArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullIpNetworkArrayPropield), sapfs.NullIpNetworkArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.StringArrayPropield), sapfs.StringArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.StringBuilderArrayPropield), sapfs.StringBuilderArrayPropield);
                    ctb.CollectionField.AlwaysAddAllCharSeq(nameof(sapfs.CharSequenceArrayPropield), sapfs.CharSequenceArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.VersionArrayPropield), sapfs.VersionArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.IntPtrArrayPropield), sapfs.IntPtrArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.UriArrayPropield), sapfs.UriArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.SpanFormattableArrayPropield), sapfs.SpanFormattableArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NdLNfEnumArrayPropield), sapfs.NdLNfEnumArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NdUNfEnumArrayPropield), sapfs.NdUNfEnumArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NdLWfEnumArrayPropield), sapfs.NdLWfEnumArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NdUWfEnumArrayPropield), sapfs.NdUWfEnumArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.WdLNfEnumArrayPropield), sapfs.WdLNfEnumArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.WdUNfEnumArrayPropield), sapfs.WdUNfEnumArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.WdLWfEnumArrayPropield), sapfs.WdLWfEnumArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.WdUWfEnumArrayPropield), sapfs.WdUWfEnumArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullNdLNfEnumArrayPropield), sapfs.NullNdLNfEnumArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullNdUNfEnumArrayPropield), sapfs.NullNdUNfEnumArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullNdLWfEnumArrayPropield), sapfs.NullNdLWfEnumArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullNdUWfEnumArrayPropield), sapfs.NullNdUWfEnumArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullWdLNfEnumArrayPropield), sapfs.NullWdLNfEnumArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullWdUNfEnumArrayPropield), sapfs.NullWdUNfEnumArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullWdLWfEnumArrayPropield), sapfs.NullWdLWfEnumArrayPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullWdUWfEnumArrayPropield), sapfs.NullWdUWfEnumArrayPropield);
                    return ctb.Complete();
                };
        }
    }

    public static PalantírReveal<StandardArrayPropertyFieldStruct> WhenPopulatedReveal
    {
        get
        {
            return
                (sapfs, tos) =>
                {
                    using var ctb =
                        tos.StartComplexType(sapfs);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.ByteArrayPropield), sapfs.ByteArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.SByteArrayPropield), sapfs.SByteArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.CharArrayPropield), sapfs.CharArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.ShortArrayPropield), sapfs.ShortArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.UShortArrayPropield), sapfs.UShortArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.HalfArrayPropield), sapfs.HalfArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.IntArrayPropield), sapfs.IntArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.UIntArrayPropield), sapfs.UIntArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.FloatArrayPropield), sapfs.FloatArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.LongArrayPropield), sapfs.LongArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.ULongArrayPropield), sapfs.ULongArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.DoubleArrayPropield), sapfs.DoubleArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.DecimalArrayPropield), sapfs.DecimalArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.VeryLongArrayPropield), sapfs.VeryLongArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.VeryUlongArrayPropield), sapfs.VeryUlongArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.BigIntArrayPropield), sapfs.BigIntArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.ComplexArrayPropield), sapfs.ComplexArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.DateTimeArrayPropield), sapfs.DateTimeArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.DateOnlyArrayPropield), sapfs.DateOnlyArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.TimeSpanArrayPropield), sapfs.TimeSpanArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.TimeOnlyArrayPropield), sapfs.TimeOnlyArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.RuneArrayPropield), sapfs.RuneArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.GuidArrayPropield), sapfs.GuidArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.IpNetworkArrayPropield), sapfs.IpNetworkArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullByteArrayPropield), sapfs.NullByteArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullSByteArrayPropield), sapfs.NullSByteArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullCharArrayPropield), sapfs.NullCharArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullShortArrayPropield), sapfs.NullShortArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullUShortArrayPropield), sapfs.NullUShortArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullHalfArrayPropield), sapfs.NullHalfArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullIntArrayPropield), sapfs.NullIntArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullUIntArrayPropield), sapfs.NullUIntArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullFloatArrayPropield), sapfs.NullFloatArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullLongArrayPropield), sapfs.NullLongArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullULongArrayPropield), sapfs.NullULongArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullDoubleArrayPropield), sapfs.NullDoubleArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullDecimalArrayPropield), sapfs.NullDecimalArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullVeryLongArrayPropield), sapfs.NullVeryLongArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullVeryUlongArrayPropield), sapfs.NullVeryUlongArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullBigIntArrayPropield), sapfs.NullBigIntArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullComplexArrayPropield), sapfs.NullComplexArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullDateTimeArrayPropield), sapfs.NullDateTimeArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullDateOnlyArrayPropield), sapfs.NullDateOnlyArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullTimeSpanArrayPropield), sapfs.NullTimeSpanArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullTimeOnlyArrayPropield), sapfs.NullTimeOnlyArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullRuneArrayPropield), sapfs.NullRuneArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullGuidArrayPropield), sapfs.NullGuidArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullIpNetworkArrayPropield), sapfs.NullIpNetworkArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.StringArrayPropield), sapfs.StringArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.StringBuilderArrayPropield), sapfs.StringBuilderArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAllCharSeq(nameof(sapfs.CharSequenceArrayPropield), sapfs.CharSequenceArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.VersionArrayPropield), sapfs.VersionArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.IntPtrArrayPropield), sapfs.IntPtrArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.UriArrayPropield), sapfs.UriArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.SpanFormattableArrayPropield), sapfs.SpanFormattableArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NdLNfEnumArrayPropield), sapfs.NdLNfEnumArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NdUNfEnumArrayPropield), sapfs.NdUNfEnumArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NdLWfEnumArrayPropield), sapfs.NdLWfEnumArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NdUWfEnumArrayPropield), sapfs.NdUWfEnumArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.WdLNfEnumArrayPropield), sapfs.WdLNfEnumArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.WdUNfEnumArrayPropield), sapfs.WdUNfEnumArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.WdLWfEnumArrayPropield), sapfs.WdLWfEnumArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.WdUWfEnumArrayPropield), sapfs.WdUWfEnumArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullNdLNfEnumArrayPropield), sapfs.NullNdLNfEnumArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullNdUNfEnumArrayPropield), sapfs.NullNdUNfEnumArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullNdLWfEnumArrayPropield), sapfs.NullNdLWfEnumArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullNdUWfEnumArrayPropield), sapfs.NullNdUWfEnumArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullWdLNfEnumArrayPropield), sapfs.NullWdLNfEnumArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullWdUNfEnumArrayPropield), sapfs.NullWdUNfEnumArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullWdLWfEnumArrayPropield), sapfs.NullWdLWfEnumArrayPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullWdUWfEnumArrayPropield), sapfs.NullWdUWfEnumArrayPropield);
                    return ctb.Complete();
                };
        }
    }

    public static PalantírReveal<StandardArrayPropertyFieldStruct> AlwaysAddFiltered
    {
        get
        {
            return
                (sapfs, tos) =>
                {
                    using var ctb = tos.StartComplexType(sapfs);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.ByteArrayPropield), sapfs.ByteArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.ByteArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.SByteArrayPropield), sapfs.SByteArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.SByteArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.CharArrayPropield), sapfs.CharArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.CharArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.ShortArrayPropield), sapfs.ShortArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.ShortArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.UShortArrayPropield), sapfs.UShortArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.UShortArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.HalfArrayPropield), sapfs.HalfArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.HalfArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.IntArrayPropield), sapfs.IntArrayPropield, FilterRegistry.OrderedCollectionFilterDefault
                                                              (sapfs.IntArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.UIntArrayPropield), sapfs.UIntArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.UIntArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.FloatArrayPropield), sapfs.FloatArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.FloatArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.LongArrayPropield), sapfs.LongArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.LongArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.ULongArrayPropield), sapfs.ULongArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.ULongArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.DoubleArrayPropield), sapfs.DoubleArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.DoubleArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.DecimalArrayPropield), sapfs.DecimalArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.DecimalArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.VeryLongArrayPropield), sapfs.VeryLongArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.VeryLongArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.VeryUlongArrayPropield), sapfs.VeryUlongArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.VeryUlongArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.BigIntArrayPropield), sapfs.BigIntArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.BigIntArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.ComplexArrayPropield), sapfs.ComplexArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.ComplexArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.DateTimeArrayPropield), sapfs.DateTimeArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.DateTimeArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.DateOnlyArrayPropield), sapfs.DateOnlyArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.DateOnlyArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.TimeSpanArrayPropield), sapfs.TimeSpanArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.TimeSpanArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.TimeOnlyArrayPropield), sapfs.TimeOnlyArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.TimeOnlyArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.RuneArrayPropield), sapfs.RuneArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.RuneArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.GuidArrayPropield), sapfs.GuidArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.GuidArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.IpNetworkArrayPropield), sapfs.IpNetworkArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.IpNetworkArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullByteArrayPropield), sapfs.NullByteArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NullByteArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullSByteArrayPropield), sapfs.NullSByteArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NullSByteArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullCharArrayPropield), sapfs.NullCharArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NullCharArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullShortArrayPropield), sapfs.NullShortArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NullShortArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullUShortArrayPropield), sapfs.NullUShortArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullUShortArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullHalfArrayPropield), sapfs.NullHalfArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NullHalfArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullIntArrayPropield), sapfs.NullIntArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NullIntArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullUIntArrayPropield), sapfs.NullUIntArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NullUIntArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullFloatArrayPropield), sapfs.NullFloatArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NullFloatArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullLongArrayPropield), sapfs.NullLongArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NullLongArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullULongArrayPropield), sapfs.NullULongArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NullULongArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullDoubleArrayPropield), sapfs.NullDoubleArrayPropield
                                                        , FilterRegistry.OrderedCollectionFilterDefault(sapfs.NullDoubleArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullDecimalArrayPropield), sapfs.NullDecimalArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullDecimalArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullVeryLongArrayPropield), sapfs.NullVeryLongArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullVeryLongArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullVeryUlongArrayPropield), sapfs.NullVeryUlongArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullVeryUlongArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullBigIntArrayPropield), sapfs.NullBigIntArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullBigIntArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullComplexArrayPropield), sapfs.NullComplexArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullComplexArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullDateTimeArrayPropield), sapfs.NullDateTimeArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullDateTimeArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullDateOnlyArrayPropield), sapfs.NullDateOnlyArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullDateOnlyArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullTimeSpanArrayPropield), sapfs.NullTimeSpanArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullTimeSpanArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullTimeOnlyArrayPropield), sapfs.NullTimeOnlyArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullTimeOnlyArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullRuneArrayPropield), sapfs.NullRuneArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NullRuneArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullGuidArrayPropield), sapfs.NullGuidArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NullGuidArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullIpNetworkArrayPropield), sapfs.NullIpNetworkArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullIpNetworkArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.StringArrayPropield), sapfs.StringArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.StringArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.StringBuilderArrayPropield), sapfs.StringBuilderArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.StringBuilderArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFilteredCharSeq(nameof(sapfs.CharSequenceArrayPropield), sapfs.CharSequenceArrayPropield, FilterRegistry
                                                                          .OrderedCollectionFilterDefault(sapfs.CharSequenceArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.VersionArrayPropield), sapfs.VersionArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.VersionArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.IntPtrArrayPropield), sapfs.IntPtrArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.IntPtrArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.UriArrayPropield), sapfs.UriArrayPropield, FilterRegistry.OrderedCollectionFilterDefault
                                                              (sapfs.UriArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.SpanFormattableArrayPropield), sapfs.SpanFormattableArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.SpanFormattableArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NdLNfEnumArrayPropield), sapfs.NdLNfEnumArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NdLNfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NdUNfEnumArrayPropield), sapfs.NdUNfEnumArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NdUNfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NdLWfEnumArrayPropield), sapfs.NdLWfEnumArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NdLWfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NdUWfEnumArrayPropield), sapfs.NdUWfEnumArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NdUWfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.WdLNfEnumArrayPropield), sapfs.WdLNfEnumArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.WdLNfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.WdUNfEnumArrayPropield), sapfs.WdUNfEnumArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.WdUNfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.WdLWfEnumArrayPropield), sapfs.WdLWfEnumArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.WdLWfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.WdUWfEnumArrayPropield), sapfs.WdUWfEnumArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.WdUWfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullNdLNfEnumArrayPropield), sapfs.NullNdLNfEnumArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullNdLNfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullNdUNfEnumArrayPropield), sapfs.NullNdUNfEnumArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullNdUNfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullNdLWfEnumArrayPropield), sapfs.NullNdLWfEnumArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullNdLWfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullNdUWfEnumArrayPropield), sapfs.NullNdUWfEnumArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullNdUWfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullWdLNfEnumArrayPropield), sapfs.NullWdLNfEnumArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullWdLNfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullWdUNfEnumArrayPropield), sapfs.NullWdUNfEnumArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullWdUNfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullWdLWfEnumArrayPropield), sapfs.NullWdLWfEnumArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullWdLWfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullWdUWfEnumArrayPropield), sapfs.NullWdUWfEnumArrayPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullWdUWfEnumArrayPropield).CheckPredicate);
                    return ctb.Complete();
                };
        }
    }

    public static PalantírReveal<StandardArrayPropertyFieldStruct> WhenPopulatedWithFilterReveal
    {
        get
        {
            return
                (sapfs, tos) =>
                {
                    using var ctb = tos.StartComplexType(sapfs);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.ByteArrayPropield), sapfs.ByteArrayPropield
                                                        , FilterRegistry.OrderedCollectionFilterDefault(sapfs.ByteArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.ByteArrayPropield), sapfs.ByteArrayPropield
                                                              , FilterRegistry.OrderedCollectionFilterDefault(sapfs.ByteArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.SByteArrayPropield), sapfs.SByteArrayPropield
                                                              , FilterRegistry.OrderedCollectionFilterDefault(sapfs.SByteArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.CharArrayPropield), sapfs.CharArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.CharArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.ShortArrayPropield), sapfs.ShortArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.ShortArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.UShortArrayPropield), sapfs.UShortArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.UShortArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.HalfArrayPropield), sapfs.HalfArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.HalfArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.IntArrayPropield), sapfs.IntArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs
                                                                                                        .IntArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.UIntArrayPropield), sapfs.UIntArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.UIntArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.FloatArrayPropield), sapfs.FloatArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.FloatArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.LongArrayPropield), sapfs.LongArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.LongArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.ULongArrayPropield), sapfs.ULongArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.ULongArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.DoubleArrayPropield), sapfs.DoubleArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.DoubleArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.DecimalArrayPropield), sapfs.DecimalArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.DecimalArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.VeryLongArrayPropield), sapfs.VeryLongArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.VeryLongArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.VeryUlongArrayPropield), sapfs.VeryUlongArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.VeryUlongArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.BigIntArrayPropield), sapfs.BigIntArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.BigIntArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.ComplexArrayPropield), sapfs.ComplexArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.ComplexArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.DateTimeArrayPropield), sapfs.DateTimeArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.DateTimeArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.DateOnlyArrayPropield), sapfs.DateOnlyArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.DateOnlyArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.TimeSpanArrayPropield), sapfs.TimeSpanArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.TimeSpanArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.TimeOnlyArrayPropield), sapfs.TimeOnlyArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.TimeOnlyArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.RuneArrayPropield), sapfs.RuneArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs
                                                                             .RuneArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.GuidArrayPropield), sapfs.GuidArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs
                                                                             .GuidArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.IpNetworkArrayPropield), sapfs.IpNetworkArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.IpNetworkArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullByteArrayPropield), sapfs.NullByteArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.NullByteArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullSByteArrayPropield), sapfs.NullSByteArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullSByteArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullCharArrayPropield), sapfs.NullCharArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.NullCharArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullShortArrayPropield), sapfs.NullShortArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullShortArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullUShortArrayPropield), sapfs.NullUShortArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullUShortArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullHalfArrayPropield), sapfs.NullHalfArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.NullHalfArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullIntArrayPropield), sapfs.NullIntArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.NullIntArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullUIntArrayPropield), sapfs.NullUIntArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.NullUIntArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullFloatArrayPropield), sapfs.NullFloatArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullFloatArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullLongArrayPropield), sapfs.NullLongArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.NullLongArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullULongArrayPropield), sapfs.NullULongArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullULongArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullDoubleArrayPropield), sapfs.NullDoubleArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullDoubleArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullDecimalArrayPropield), sapfs.NullDecimalArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullDecimalArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullVeryLongArrayPropield), sapfs.NullVeryLongArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullVeryLongArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullVeryUlongArrayPropield), sapfs.NullVeryUlongArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullVeryUlongArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullBigIntArrayPropield), sapfs.NullBigIntArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullBigIntArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullComplexArrayPropield), sapfs.NullComplexArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullComplexArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullDateTimeArrayPropield), sapfs.NullDateTimeArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullDateTimeArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullDateOnlyArrayPropield), sapfs.NullDateOnlyArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullDateOnlyArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullTimeSpanArrayPropield), sapfs.NullTimeSpanArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullTimeSpanArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullTimeOnlyArrayPropield), sapfs.NullTimeOnlyArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullTimeOnlyArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullRuneArrayPropield), sapfs.NullRuneArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.NullRuneArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullGuidArrayPropield), sapfs.NullGuidArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.NullGuidArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullIpNetworkArrayPropield), sapfs.NullIpNetworkArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullIpNetworkArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.StringArrayPropield), sapfs.StringArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.StringArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.StringBuilderArrayPropield), sapfs.StringBuilderArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.StringBuilderArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilterCharSeq(nameof(sapfs.CharSequenceArrayPropield), sapfs.CharSequenceArrayPropield
                                                                          , FilterRegistry
                                                                            .OrderedCollectionFilterDefault(sapfs.CharSequenceArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.VersionArrayPropield), sapfs.VersionArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.VersionArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.IntPtrArrayPropield), sapfs.IntPtrArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.IntPtrArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.UriArrayPropield), sapfs.UriArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs
                                                                                                        .UriArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.SpanFormattableArrayPropield), sapfs.SpanFormattableArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.SpanFormattableArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NdLNfEnumArrayPropield), sapfs.NdLNfEnumArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NdLNfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NdUNfEnumArrayPropield), sapfs.NdUNfEnumArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NdUNfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NdLWfEnumArrayPropield), sapfs.NdLWfEnumArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NdLWfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NdUWfEnumArrayPropield), sapfs.NdUWfEnumArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NdUWfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.WdLNfEnumArrayPropield), sapfs.WdLNfEnumArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.WdLNfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.WdUNfEnumArrayPropield), sapfs.WdUNfEnumArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.WdUNfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.WdLWfEnumArrayPropield), sapfs.WdLWfEnumArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.WdLWfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.WdUWfEnumArrayPropield), sapfs.WdUWfEnumArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.WdUWfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullNdLNfEnumArrayPropield), sapfs.NullNdLNfEnumArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullNdLNfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullNdUNfEnumArrayPropield), sapfs.NullNdUNfEnumArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullNdUNfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullNdLWfEnumArrayPropield), sapfs.NullNdLWfEnumArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullNdLWfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullNdUWfEnumArrayPropield), sapfs.NullNdUWfEnumArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullNdUWfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullWdLNfEnumArrayPropield), sapfs.NullWdLNfEnumArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullWdLNfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullWdUNfEnumArrayPropield), sapfs.NullWdUNfEnumArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullWdUNfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullWdLWfEnumArrayPropield), sapfs.NullWdLWfEnumArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullWdLWfEnumArrayPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullWdUWfEnumArrayPropield), sapfs.NullWdUWfEnumArrayPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullWdUWfEnumArrayPropield).CheckPredicate);

                    return ctb.Complete();
                };
        }
    }
}
