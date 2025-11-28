using System.Net;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Extensions;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation;

public class StandardListPropertyFieldClass : IStringBearer
{
    public StandardListPropertyFieldClass()
    {
        InitializeAllSet();
    }

    public void InitializeAllSet()
    {
        ByteListPropield      = [byte.MinValue, 0, byte.MaxValue];
        SByteListPropield     = [sbyte.MinValue, 0, sbyte.MaxValue];
        CharListPropield      = [' ', '\u0000', '\uFFFF'];
        ShortListPropield     = [short.MinValue, 0, short.MaxValue];
        UShortListPropield    = [ushort.MinValue, 0, ushort.MaxValue];
        HalfListPropield      = [Half.MinValue, default, Half.NaN, Half.MaxValue];
        IntListPropield       = [int.MinValue, 0, int.MaxValue];
        UIntListPropield      = [uint.MinValue, 0, uint.MaxValue];
        FloatListPropield     = [float.MinValue, 0f, float.NaN, float.MaxValue];
        LongListPropield      = [long.MinValue, 0, long.MaxValue];
        ULongListPropield     = [ulong.MinValue, 0, ulong.MaxValue];
        DoubleListPropield    = [double.MinValue, 0d, double.NaN, double.MaxValue];
        DecimalListPropield   = [decimal.MinValue, 0m, decimal.MaxValue];
        VeryLongListPropield  = [Int128.MinValue, default, Int128.MaxValue];
        VeryUlongListPropield = [UInt128.MinValue, default, UInt128.MaxValue];
        BigIntListPropield    = [BigInteger.Parse("-99999999999999999999999999"), default, BigInteger.Parse("99999999999999999999999999")];
        ComplexListPropield   = [new Complex(double.MaxValue * -1.0, double.MaxValue * -1), default, new Complex(double.MaxValue, double.MaxValue)];
        DateTimeListPropield  = [DateTime.MinValue, default, DateTime.MaxValue];
        DateOnlyListPropield  = [DateOnly.MinValue, default, DateOnly.MaxValue];
        TimeSpanListPropield  = [TimeSpan.MinValue, TimeSpan.Zero, TimeSpan.MaxValue];
        TimeOnlyListPropield  = [TimeOnly.MinValue, default, TimeOnly.MaxValue];
        RuneListPropield      = [Rune.GetRuneAt("\U00010000", 0), default, Rune.GetRuneAt("\U0010FFFF", 0)];
        GuidListPropield =
            [Guid.ParseExact("00000000-0000-0000-0000-000000000000", "X"), Guid.Empty, Guid.ParseExact("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", "X")];
        IpNetworkListPropield = [new IPNetwork(new IPAddress("\0\0\0\0"u8.ToArray()), 0), default, IPNetwork.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff")];

        NullByteListPropield      = [byte.MinValue, 0, null, byte.MaxValue];
        NullSByteListPropield     = [sbyte.MinValue, 0, null, sbyte.MaxValue];
        NullCharListPropield      = [' ', '\u0000', null, '\uFFFF'];
        NullShortListPropield     = [short.MinValue, 0, null, short.MaxValue];
        NullUShortListPropield    = [ushort.MinValue, 0, null, ushort.MaxValue];
        NullHalfListPropield      = [Half.MinValue, Half.Zero, Half.NaN, null, Half.MaxValue];
        NullIntListPropield       = [int.MinValue, 0, null, int.MaxValue];
        NullUIntListPropield      = [uint.MinValue, 0, null, uint.MaxValue];
        NullFloatListPropield     = [float.MinValue, 0f, float.NaN, null, float.MaxValue];
        NullLongListPropield      = [long.MinValue, 0, null, long.MaxValue];
        NullULongListPropield     = [ulong.MinValue, 0, null, ulong.MaxValue];
        NullDoubleListPropield    = [double.MinValue, 0d, double.NaN, null, double.MaxValue];
        NullDecimalListPropield   = [decimal.MinValue, 0m, null, decimal.MaxValue];
        NullVeryLongListPropield  = [Int128.MinValue, Int128.Zero, null, Int128.MaxValue];
        NullVeryUlongListPropield = [UInt128.MinValue, UInt128.Zero, null, UInt128.MaxValue];
        NullBigIntListPropield    = [BigInteger.Parse("-99999999999999999999999999"), BigInteger.Zero, null, BigInteger.Parse("99999999999999999999999999")];
        NullComplexListPropield =
            [new Complex(double.MaxValue * -1.0, double.MaxValue * -1), Complex.Zero, null, new Complex(double.MaxValue, double.MaxValue)];
        NullDateTimeListPropield = [DateTime.MinValue, new DateTime(), null, DateTime.MaxValue];
        NullDateOnlyListPropield = [DateOnly.MinValue, new DateOnly(), null, DateOnly.MaxValue];
        NullTimeSpanListPropield = [TimeSpan.MinValue, TimeSpan.Zero, null, TimeSpan.MaxValue];
        NullTimeOnlyListPropield = [TimeOnly.MinValue, null, TimeOnly.MaxValue];
        NullRuneListPropield     = [Rune.GetRuneAt("\U00010000", 0), Rune.GetRuneAt("\u0000", 0), null, Rune.GetRuneAt("\U0010FFFF", 0)];
        NullGuidListPropield =
            [Guid.ParseExact("00000000-0000-0000-0000-000000000000", "X"), Guid.Empty, null, Guid.ParseExact("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", "X")];
        NullIpNetworkListPropield =
            [new IPNetwork(new IPAddress("\0\0\0\0"u8.ToArray()), 0), new IPNetwork(), null, IPNetwork.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff")];

        StringListPropield        = ["stringListPropield_1", "", null!, "stringListPropield_4"];
        StringBuilderListPropield = [new("stringBuilderListPropield_1"), new StringBuilder(), null!, new StringBuilder("stringBuilderListPropield_4")];
        CharSequenceListPropield =
            [new MutableString("charSequenceListPropield_1"), new MutableString(), null!, new MutableString("charSequenceListPropield_4")];

        VersionListPropield = [new Version(0, 0), null!, new Version(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue)];
        IntPtrListPropield  = [new IPAddress("\0\0\0\0"u8.ToArray()), null!, IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff")];
        UriListPropield     = [new Uri(""), null!, new Uri("https://github.com/shwaindog/Fortitude")];

        SpanFormattableListPropield = [new MySpanFormattableClass(""), null!, new MySpanFormattableClass("SpanFormattableSingPropield")];
        NdLNfEnumListPropield       = [NoDefaultLongNoFlagsEnum.NDLNFE_1, default, NoDefaultLongNoFlagsEnum.NDLNFE_34];
        NdUNfEnumListPropield       = [NoDefaultULongNoFlagsEnum.NDUNFE_1, default, NoDefaultULongNoFlagsEnum.NDUNFE_34];
        NdLWfEnumListPropield =
        [
            NoDefaultLongWithFlagsEnum.NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_2, default
          , NoDefaultLongWithFlagsEnum.NDLWFE_33 | NoDefaultLongWithFlagsEnum.NDLWFE_34
        ];
        NdUWfEnumListPropield =
        [
            NoDefaultULongWithFlagsEnum.NDUWFE_1 | NoDefaultULongWithFlagsEnum.NDUWFE_2, default
          , NoDefaultULongWithFlagsEnum.NDUWFE_33 | NoDefaultULongWithFlagsEnum.NDUWFE_34
        ];

        WdLNfEnumListPropield = [WithDefaultLongNoFlagsEnum.WDLNFE_1, default, WithDefaultLongNoFlagsEnum.WDLNFE_34];
        WdUNfEnumListPropield = [WithDefaultULongNoFlagsEnum.WDUNFE_1, default, WithDefaultULongNoFlagsEnum.WDUNFE_34];
        WdLWfEnumListPropield =
        [
            WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_2, default
          , WithDefaultLongWithFlagsEnum.WDLWFE_33 | WithDefaultLongWithFlagsEnum.WDLWFE_34
        ];
        WdUWfEnumListPropield =
        [
            WithDefaultULongWithFlagsEnum.WDUWFE_1 | WithDefaultULongWithFlagsEnum.WDUWFE_2, default
          , WithDefaultULongWithFlagsEnum.WDUWFE_33 | WithDefaultULongWithFlagsEnum.WDUWFE_34
        ];

        NullNdLNfEnumListPropield = [NoDefaultLongNoFlagsEnum.NDLNFE_1, default(NoDefaultLongNoFlagsEnum), null, NoDefaultLongNoFlagsEnum.NDLNFE_34];
        NullNdUNfEnumListPropield = [NoDefaultULongNoFlagsEnum.NDUNFE_1, default(NoDefaultULongNoFlagsEnum), null, NoDefaultULongNoFlagsEnum.NDUNFE_34];
        NullNdLWfEnumListPropield =
        [
            NoDefaultLongWithFlagsEnum.NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_2, default(NoDefaultLongWithFlagsEnum), null
          , NoDefaultLongWithFlagsEnum.NDLWFE_33 | NoDefaultLongWithFlagsEnum.NDLWFE_34
        ];
        NullNdUWfEnumListPropield =
        [
            NoDefaultULongWithFlagsEnum.NDUWFE_1 | NoDefaultULongWithFlagsEnum.NDUWFE_2, default(NoDefaultULongWithFlagsEnum), null
          , NoDefaultULongWithFlagsEnum.NDUWFE_33 | NoDefaultULongWithFlagsEnum.NDUWFE_34
        ];

        NullWdLNfEnumListPropield = [WithDefaultLongNoFlagsEnum.WDLNFE_1, default(WithDefaultLongNoFlagsEnum), null, WithDefaultLongNoFlagsEnum.WDLNFE_34];
        NullWdUNfEnumListPropield = [WithDefaultULongNoFlagsEnum.WDUNFE_1, default(WithDefaultULongNoFlagsEnum), null, WithDefaultULongNoFlagsEnum.WDUNFE_34];
        NullWdLWfEnumListPropield =
        [
            WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_2, default(WithDefaultLongWithFlagsEnum), null
          , WithDefaultLongWithFlagsEnum.WDLWFE_33 | WithDefaultLongWithFlagsEnum.WDLWFE_34
        ];
        NullWdUWfEnumListPropield =
        [
            WithDefaultULongWithFlagsEnum.WDUWFE_1 | WithDefaultULongWithFlagsEnum.WDUWFE_2, default(WithDefaultULongWithFlagsEnum), null
          , WithDefaultULongWithFlagsEnum.WDUWFE_33 | WithDefaultULongWithFlagsEnum.WDUWFE_34
        ];
    }


    public void InitializeAtSize(int numberToGenerate)
    {
        ByteListPropield      = NumberTestDataGenerator.GenRandomNumberRange<byte>(numberToGenerate).ToList();
        SByteListPropield     = NumberTestDataGenerator.GenRandomNumberRange<sbyte>(numberToGenerate).ToList();
        CharListPropield      = NumberTestDataGenerator.GenRandomNumberRange<char>(numberToGenerate).ToList();
        ShortListPropield     = NumberTestDataGenerator.GenRandomNumberRange<short>(numberToGenerate).ToList();
        UShortListPropield    = NumberTestDataGenerator.GenRandomNumberRange<ushort>(numberToGenerate).ToList();
        HalfListPropield      = NumberTestDataGenerator.GenRandomNumberRange<Half>(numberToGenerate).ToList();
        IntListPropield       = NumberTestDataGenerator.GenRandomNumberRange<int>(numberToGenerate).ToList();
        UIntListPropield      = NumberTestDataGenerator.GenRandomNumberRange<uint>(numberToGenerate).ToList();
        FloatListPropield     = NumberTestDataGenerator.GenRandomNumberRange<float>(numberToGenerate).ToList();
        LongListPropield      = NumberTestDataGenerator.GenRandomNumberRange<long>(numberToGenerate).ToList();
        ULongListPropield     = NumberTestDataGenerator.GenRandomNumberRange<ulong>(numberToGenerate).ToList();
        DoubleListPropield    = NumberTestDataGenerator.GenRandomNumberRange<double>(numberToGenerate).ToList();
        DecimalListPropield   = NumberTestDataGenerator.GenRandomNumberRange<decimal>(numberToGenerate).ToList();
        VeryLongListPropield  = NumberTestDataGenerator.GenRandomNumberRange<Int128>(numberToGenerate).ToList();
        VeryUlongListPropield = NumberTestDataGenerator.GenRandomNumberRange<UInt128>(numberToGenerate).ToList();
        BigIntListPropield    = NumberTestDataGenerator.GenRandomNumberRange<BigInteger>(numberToGenerate).ToList();
        ComplexListPropield   = NumberTestDataGenerator.GenRandomNumberRange<Complex>(numberToGenerate).ToList();
        DateTimeListPropield  = DateTimeTestDataGenerator.GenRandomDateTimeRange(numberToGenerate).ToList();
        DateOnlyListPropield  = DateTimeTestDataGenerator.GenRandomDateOnlyRange(numberToGenerate).ToList();
        TimeSpanListPropield  = DateTimeTestDataGenerator.GenRandomTimeSpanRange(numberToGenerate).ToList();
        TimeOnlyListPropield  = DateTimeTestDataGenerator.GenRandomTimeOnlyRange(numberToGenerate).ToList();
        RuneListPropield      = NumberTestDataGenerator.GenRandomNumberRange<Rune>(numberToGenerate).ToList();
        GuidListPropield =
            NumberTestDataGenerator
                .GenRandomNumberRange<byte>(numberToGenerate)
                .Select(_ =>
                            new Guid(NumberTestDataGenerator
                                     .GenRandomNumberRange<
                                         byte>(16).ToArray()))
                .ToList();
        IpNetworkListPropield =
            NumberTestDataGenerator
                .GenRandomNumberRange<byte>(numberToGenerate)
                .Select(b =>
                {
                    if(b % 2 == 0){
                        return new IPNetwork
                            (new IPAddress
                                 (NumberTestDataGenerator.GenRandomNumberRange<byte>(4).ToArray()), (b % 32));
                    }
                    return new IPNetwork
                        (new IPAddress
                             (NumberTestDataGenerator.GenRandomNumberRange<byte>(16).ToArray()), (b % 128));
                }) .ToList();

        NullByteListPropield      = NumberTestDataGenerator.GenRandomNullableNumberRange<byte>(numberToGenerate).ToList();
        NullSByteListPropield     = NumberTestDataGenerator.GenRandomNullableNumberRange<sbyte>(numberToGenerate).ToList();
        NullCharListPropield      = NumberTestDataGenerator.GenRandomNullableNumberRange<char>(numberToGenerate).ToList();
        NullShortListPropield     = NumberTestDataGenerator.GenRandomNullableNumberRange<short>(numberToGenerate).ToList();
        NullUShortListPropield    = NumberTestDataGenerator.GenRandomNullableNumberRange<ushort>(numberToGenerate).ToList();
        NullHalfListPropield      = NumberTestDataGenerator.GenRandomNullableNumberRange<Half>(numberToGenerate).ToList();
        NullIntListPropield       = NumberTestDataGenerator.GenRandomNullableNumberRange<int>(numberToGenerate).ToList();
        NullUIntListPropield      = NumberTestDataGenerator.GenRandomNullableNumberRange<uint>(numberToGenerate).ToList();
        NullFloatListPropield     = NumberTestDataGenerator.GenRandomNullableNumberRange<float>(numberToGenerate).ToList();
        NullLongListPropield      = NumberTestDataGenerator.GenRandomNullableNumberRange<long>(numberToGenerate).ToList();
        NullULongListPropield     = NumberTestDataGenerator.GenRandomNullableNumberRange<ulong>(numberToGenerate).ToList();
        NullDoubleListPropield    = NumberTestDataGenerator.GenRandomNullableNumberRange<double>(numberToGenerate).ToList();
        NullDecimalListPropield   = NumberTestDataGenerator.GenRandomNullableNumberRange<decimal>(numberToGenerate).ToList();
        NullVeryLongListPropield  = NumberTestDataGenerator.GenRandomNullableNumberRange<Int128>(numberToGenerate).ToList();
        NullVeryUlongListPropield = NumberTestDataGenerator.GenRandomNullableNumberRange<UInt128>(numberToGenerate).ToList();
        NullBigIntListPropield    = NumberTestDataGenerator.GenRandomNullableNumberRange<BigInteger>(numberToGenerate).ToList();
        NullComplexListPropield   = NumberTestDataGenerator.GenRandomNullableNumberRange<Complex>(numberToGenerate).ToList();
        NullDateTimeListPropield  = DateTimeTestDataGenerator.GenRandomNullableDateTimeRange(numberToGenerate).ToList();
        NullDateOnlyListPropield  = DateTimeTestDataGenerator.GenRandomNullableDateOnlyRange(numberToGenerate).ToList();
        NullTimeSpanListPropield  = DateTimeTestDataGenerator.GenRandomNullableTimeSpanRange(numberToGenerate).ToList();
        NullTimeOnlyListPropield  = DateTimeTestDataGenerator.GenRandomNullableTimeOnlyRange(numberToGenerate).ToList();
        NullRuneListPropield      = NumberTestDataGenerator.GenRandomNullableNumberRange<Rune>(numberToGenerate).ToList();
        
        NullGuidListPropield      = 
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<byte>(numberToGenerate)
                .Select(b =>
                        {
                            if(b == null) return null;
                            return  new Guid?(new Guid(NumberTestDataGenerator
                                                       .GenRandomNumberRange<
                                                           byte>(16).ToArray()));
                        }
                       )
                .ToList();
        NullIpNetworkListPropield = 
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<byte>(numberToGenerate)
                .Select(b =>
                {
                    if(b == null) return null;
                    if(b % 2 == 0){
                        return new IPNetwork?( new IPNetwork
                            (new IPAddress
                                 (NumberTestDataGenerator.GenRandomNumberRange<byte>(4).ToArray()), (b.Value % 32)));
                    }
                    return new IPNetwork( new IPAddress
                                             (NumberTestDataGenerator.GenRandomNumberRange<byte>(16).ToArray()), (b.Value % 128));
                }) .ToList();

        StringListPropield        = 
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<int>(numberToGenerate)
                .Select(num =>
                {
                    if(num == null) return null!;
                    return "stringListPropield_" + num;
                }) .ToList();
        StringBuilderListPropield = 
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<int>(numberToGenerate)
                .Select(num =>
                {
                    if(num == null) return null!;
                    return new StringBuilder("stringBuilderListPropield_1" + num);
                }) .ToList();
        CharSequenceListPropield =
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<int>(numberToGenerate)
                .Select(num =>
                {
                    if(num == null) return null!;
                    return (ICharSequence)new MutableString("charSequenceListPropield_1" + num);
                }) .ToList();

        VersionListPropield = 
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<int>(numberToGenerate)
                .Select(num =>
                {
                    if(num == null) return null!;
                    var otherThree = NumberTestDataGenerator
                        .GenRandomNumberRange<int>(numberToGenerate).Select(Math.Abs).ToList();
                    return new Version(Math.Abs(num.Value), otherThree[0], otherThree[1], otherThree[2]);
                }) .ToList();
        
        IntPtrListPropield  = 
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<byte>(numberToGenerate)
                .Select(b =>
                {
                    if(b == null) return null!;
                    if(b % 2 == 0){
                        return new IPAddress(NumberTestDataGenerator.GenRandomNumberRange<byte>(4).ToArray());
                    }
                    return  new IPAddress(NumberTestDataGenerator.GenRandomNumberRange<byte>(16).ToArray());
                }) .ToList();
        UriListPropield     = 
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<int>(numberToGenerate)
                .Select(num =>
                {
                    if(num == null) return null!;
                    return new Uri("https://www.someWebAddress_" + num + ".net");
                }) .ToList(); 

        SpanFormattableListPropield = 
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<int>(numberToGenerate)
                .Select(num =>
                {
                    if(num == null) return null!;
                    return new MySpanFormattableClass(" SpanFormattableListPropield_" + num);
                }) .ToList();
        
        NdLNfEnumListPropield = EnumTestDataGenerator.GenRandomEnumValues<NoDefaultLongNoFlagsEnum>(numberToGenerate).ToList();
        NdUNfEnumListPropield = EnumTestDataGenerator.GenRandomEnumValues<NoDefaultULongNoFlagsEnum>(numberToGenerate).ToList();
        NdLWfEnumListPropield = EnumTestDataGenerator.GenRandomEnumMultiFlagValues<NoDefaultLongWithFlagsEnum>(numberToGenerate).ToList();
        NdUWfEnumListPropield = EnumTestDataGenerator.GenRandomEnumMultiFlagValues<NoDefaultULongWithFlagsEnum>(numberToGenerate).ToList();

        WdLNfEnumListPropield = EnumTestDataGenerator.GenRandomEnumValues<WithDefaultLongNoFlagsEnum>(numberToGenerate).ToList();
        WdUNfEnumListPropield = EnumTestDataGenerator.GenRandomEnumValues<WithDefaultULongNoFlagsEnum>(numberToGenerate).ToList();
        WdLWfEnumListPropield = EnumTestDataGenerator.GenRandomEnumMultiFlagValues<WithDefaultLongWithFlagsEnum>(numberToGenerate).ToList();
        WdUWfEnumListPropield = EnumTestDataGenerator.GenRandomEnumMultiFlagValues<WithDefaultULongWithFlagsEnum>(numberToGenerate).ToList();

        NullNdLNfEnumListPropield = EnumTestDataGenerator.GenRandomNullableEnumValues<NoDefaultLongNoFlagsEnum>(numberToGenerate).ToList();
        NullNdUNfEnumListPropield = EnumTestDataGenerator.GenRandomNullableEnumValues<NoDefaultULongNoFlagsEnum>(numberToGenerate).ToList();
        NullNdLWfEnumListPropield = EnumTestDataGenerator.GenRandomNullableEnumMultiFlagValues<NoDefaultLongWithFlagsEnum>(numberToGenerate).ToList();
        NullNdUWfEnumListPropield = EnumTestDataGenerator.GenRandomNullableEnumMultiFlagValues<NoDefaultULongWithFlagsEnum>(numberToGenerate).ToList();

        NullWdLNfEnumListPropield = EnumTestDataGenerator.GenRandomNullableEnumValues<WithDefaultLongNoFlagsEnum>(numberToGenerate).ToList();
        NullWdUNfEnumListPropield = EnumTestDataGenerator.GenRandomNullableEnumValues<WithDefaultULongNoFlagsEnum>(numberToGenerate).ToList();
        NullWdLWfEnumListPropield = EnumTestDataGenerator.GenRandomNullableEnumMultiFlagValues<WithDefaultLongWithFlagsEnum>(numberToGenerate).ToList();
        NullWdUWfEnumListPropield = EnumTestDataGenerator.GenRandomNullableEnumMultiFlagValues<WithDefaultULongWithFlagsEnum>(numberToGenerate).ToList();
    }

    public void InitializeAllNull()
    {
        ByteListPropield      = null!;
        SByteListPropield     = null!;
        CharListPropield      = null!;
        ShortListPropield     = null!;
        UShortListPropield    = null!;
        HalfListPropield      = null!;
        IntListPropield       = null!;
        UIntListPropield      = null!;
        FloatListPropield     = null!;
        LongListPropield      = null!;
        ULongListPropield     = null!;
        DoubleListPropield    = null!;
        DecimalListPropield   = null!;
        VeryLongListPropield  = null!;
        VeryUlongListPropield = null!;
        BigIntListPropield    = null!;

        ComplexListPropield   = null!;
        DateTimeListPropield  = null!;
        DateOnlyListPropield  = null!;
        TimeSpanListPropield  = null!;
        TimeOnlyListPropield  = null!;
        RuneListPropield      = null!;
        GuidListPropield      = null!;
        IpNetworkListPropield = null!;

        NullByteListPropield      = null!;
        NullSByteListPropield     = null!;
        NullCharListPropield      = null!;
        NullShortListPropield     = null!;
        NullUShortListPropield    = null!;
        NullHalfListPropield      = null!;
        NullIntListPropield       = null!;
        NullUIntListPropield      = null!;
        NullFloatListPropield     = null!;
        NullLongListPropield      = null!;
        NullULongListPropield     = null!;
        NullDoubleListPropield    = null!;
        NullDecimalListPropield   = null!;
        NullVeryLongListPropield  = null!;
        NullVeryUlongListPropield = null!;
        NullBigIntListPropield    = null!;

        NullComplexListPropield   = null!;
        NullDateTimeListPropield  = null!;
        NullDateOnlyListPropield  = null!;
        NullTimeSpanListPropield  = null!;
        NullTimeOnlyListPropield  = null!;
        NullRuneListPropield      = null!;
        NullGuidListPropield      = null!;
        NullIpNetworkListPropield = null!;

        StringListPropield        = null!;
        StringBuilderListPropield = null!;
        CharSequenceListPropield  = null!;

        VersionListPropield = null!;
        IntPtrListPropield  = null!;
        UriListPropield     = null!;

        SpanFormattableListPropield = null!;

        NdLNfEnumListPropield = null!;
        NdUNfEnumListPropield = null!;
        NdLWfEnumListPropield = null!;
        NdUWfEnumListPropield = null!;

        WdLNfEnumListPropield = null!;
        WdUNfEnumListPropield = null!;
        WdLWfEnumListPropield = null!;
        WdUWfEnumListPropield = null!;

        NullNdLNfEnumListPropield = null!;
        NullNdUNfEnumListPropield = null!;
        NullNdLWfEnumListPropield = null!;
        NullNdUWfEnumListPropield = null!;

        NullWdLNfEnumListPropield = null!;
        NullWdUNfEnumListPropield = null!;
        NullWdLWfEnumListPropield = null!;
        NullWdUWfEnumListPropield = null!;
    }


    public  List<byte> ByteListPropield { get; set; } = null!;
    public List<sbyte> SByteListPropield = null!;
    public List<char> CharListPropield { get; set; } = null!;
    public List<short> ShortListPropield = null!;
    public List<ushort> UShortListPropield { get; set; } = null!;
    public List<Half> HalfListPropield = null!;
    public List<int> IntListPropield { get; set; } = null!;
    public List<uint> UIntListPropield = null!;
    public List<float> FloatListPropield { get; set; } = null!;
    public List<long> LongListPropield = null!;
    public List<ulong> ULongListPropield { get; set; } = null!;
    public List<double> DoubleListPropield = null!;
    public List<decimal> DecimalListPropield { get; set; } = null!;
    public List<Int128> VeryLongListPropield = null!;
    public List<UInt128> VeryUlongListPropield { get; set; } = null!;
    public List<BigInteger> BigIntListPropield = null!;

    public List<Complex> ComplexListPropield { get; set; } = null!;
    public List<DateTime> DateTimeListPropield = null!;
    public List<DateOnly> DateOnlyListPropield { get; set; } = null!;
    public List<TimeSpan> TimeSpanListPropield = null!;
    public List<TimeOnly> TimeOnlyListPropield { get; set; } = null!;
    public List<Rune> RuneListPropield = null!;
    public List<Guid> GuidListPropield { get; set; } = null!;
    public List<IPNetwork> IpNetworkListPropield = null!;

    public List<byte?> NullByteListPropield = null!;
    public List<sbyte?> NullSByteListPropield { get; set; } = null!;
    public List<char?> NullCharListPropield = null!;
    public List<short?> NullShortListPropield { get; set; } = null!;
    public List<ushort?> NullUShortListPropield = null!;
    public List<Half?> NullHalfListPropield { get; set; } = null!;
    public List<int?> NullIntListPropield = null!;
    public List<uint?> NullUIntListPropield { get; set; } = null!;
    public List<float?> NullFloatListPropield = null!;
    public List<long?> NullLongListPropield { get; set; } = null!;
    public List<ulong?> NullULongListPropield = null!;
    public List<double?> NullDoubleListPropield { get; set; } = null!;
    public List<decimal?> NullDecimalListPropield = null!;
    public List<Int128?> NullVeryLongListPropield { get; set; } = null!;
    public List<UInt128?> NullVeryUlongListPropield = null!;
    public List<BigInteger?> NullBigIntListPropield { get; set; } = null!;

    public List<Complex?> NullComplexListPropield { get; set; } = null!;
    public List<DateTime?> NullDateTimeListPropield = null!;
    public List<DateOnly?> NullDateOnlyListPropield { get; set; } = null!;
    public List<TimeSpan?> NullTimeSpanListPropield = null!;
    public List<TimeOnly?> NullTimeOnlyListPropield { get; set; } = null!;
    public List<Rune?> NullRuneListPropield = null!;
    public List<Guid?> NullGuidListPropield { get; set; } = null!;
    public List<IPNetwork?> NullIpNetworkListPropield = null!;

    public List<string> StringListPropield { get; set; } = null!;
    public List<StringBuilder> StringBuilderListPropield = null!;
    public List<ICharSequence> CharSequenceListPropield { get; set; } = null!;

    public List<Version> VersionListPropield = null!;
    public List<IPAddress> IntPtrListPropield { get; set; } = null!;
    public List<Uri> UriListPropield = null!;

    public List<MySpanFormattableClass> SpanFormattableListPropield { get; set; } = null!;

    public List<NoDefaultLongNoFlagsEnum> NdLNfEnumListPropield = null!;
    public List<NoDefaultULongNoFlagsEnum> NdUNfEnumListPropield { get; set; } = null!;
    public List<NoDefaultLongWithFlagsEnum> NdLWfEnumListPropield = null!;
    public List<NoDefaultULongWithFlagsEnum> NdUWfEnumListPropield { get; set; } = null!;

    public List<WithDefaultLongNoFlagsEnum> WdLNfEnumListPropield { get; set; } = null!;
    public List<WithDefaultULongNoFlagsEnum> WdUNfEnumListPropield = null!;
    public List<WithDefaultLongWithFlagsEnum> WdLWfEnumListPropield { get; set; } = null!;
    public List<WithDefaultULongWithFlagsEnum> WdUWfEnumListPropield = null!;

    public List<NoDefaultLongNoFlagsEnum?> NullNdLNfEnumListPropield = null!;
    public List<NoDefaultULongNoFlagsEnum?> NullNdUNfEnumListPropield { get; set; } = null!;
    public List<NoDefaultLongWithFlagsEnum?> NullNdLWfEnumListPropield = null!;
    public List<NoDefaultULongWithFlagsEnum?> NullNdUWfEnumListPropield { get; set; } = null!;

    public List<WithDefaultLongNoFlagsEnum?> NullWdLNfEnumListPropield { get; set; } = null!;
    public List<WithDefaultULongNoFlagsEnum?> NullWdUNfEnumListPropield = null!;
    public List<WithDefaultLongWithFlagsEnum?> NullWdLWfEnumListPropield { get; set; } = null!;
    public List<WithDefaultULongWithFlagsEnum?> NullWdUWfEnumListPropield = null!;

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
        ctb.CollectionField.AlwaysAddAll(nameof(ByteListPropield), ByteListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(SByteListPropield), SByteListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(CharListPropield), CharListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(ShortListPropield), ShortListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(UShortListPropield), UShortListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(HalfListPropield), HalfListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(IntListPropield), IntListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(UIntListPropield), UIntListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(FloatListPropield), FloatListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(LongListPropield), LongListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(ULongListPropield), ULongListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(DoubleListPropield), DoubleListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(DecimalListPropield), DecimalListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(VeryLongListPropield), VeryLongListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(VeryUlongListPropield), VeryUlongListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(BigIntListPropield), BigIntListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(ComplexListPropield), ComplexListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(DateTimeListPropield), DateTimeListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(DateOnlyListPropield), DateOnlyListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(TimeSpanListPropield), TimeSpanListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(TimeOnlyListPropield), TimeOnlyListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(RuneListPropield), RuneListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(GuidListPropield), GuidListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(IpNetworkListPropield), IpNetworkListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullByteListPropield), NullByteListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullSByteListPropield), NullSByteListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullCharListPropield), NullCharListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullShortListPropield), NullShortListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullUShortListPropield), NullUShortListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullHalfListPropield), NullHalfListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullIntListPropield), NullIntListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullUIntListPropield), NullUIntListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullFloatListPropield), NullFloatListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullLongListPropield), NullLongListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullULongListPropield), NullULongListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullDoubleListPropield), NullDoubleListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullDecimalListPropield), NullDecimalListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullVeryLongListPropield), NullVeryLongListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullVeryUlongListPropield), NullVeryUlongListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullBigIntListPropield), NullBigIntListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullComplexListPropield), NullComplexListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullDateTimeListPropield), NullDateTimeListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullDateOnlyListPropield), NullDateOnlyListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullTimeSpanListPropield), NullTimeSpanListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullTimeOnlyListPropield), NullTimeOnlyListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullRuneListPropield), NullRuneListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullGuidListPropield), NullGuidListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullIpNetworkListPropield), NullIpNetworkListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(StringListPropield), StringListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(StringBuilderListPropield), StringBuilderListPropield);
        ctb.CollectionField.AlwaysAddAllCharSeq(nameof(CharSequenceListPropield), CharSequenceListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(VersionListPropield), VersionListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(IntPtrListPropield), IntPtrListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(UriListPropield), UriListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(SpanFormattableListPropield), SpanFormattableListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NdLNfEnumListPropield), NdLNfEnumListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NdUNfEnumListPropield), NdUNfEnumListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NdLWfEnumListPropield), NdLWfEnumListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NdUWfEnumListPropield), NdUWfEnumListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(WdLNfEnumListPropield), WdLNfEnumListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(WdUNfEnumListPropield), WdUNfEnumListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(WdLWfEnumListPropield), WdLWfEnumListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(WdUWfEnumListPropield), WdUWfEnumListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullNdLNfEnumListPropield), NullNdLNfEnumListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullNdUNfEnumListPropield), NullNdUNfEnumListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullNdLWfEnumListPropield), NullNdLWfEnumListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullNdUWfEnumListPropield), NullNdUWfEnumListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullWdLNfEnumListPropield), NullWdLNfEnumListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullWdUNfEnumListPropield), NullWdUNfEnumListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullWdLWfEnumListPropield), NullWdLWfEnumListPropield);
        ctb.CollectionField.AlwaysAddAll(nameof(NullWdUWfEnumListPropield), NullWdUWfEnumListPropield);
        return ctb.Complete();
    }

    public StateExtractStringRange WhenPopulatedReveal(ITheOneString tos)
    {
        using var ctb =
            tos.StartComplexType(this);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(ByteListPropield), ByteListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(SByteListPropield), SByteListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(CharListPropield), CharListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(ShortListPropield), ShortListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(UShortListPropield), UShortListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(HalfListPropield), HalfListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(IntListPropield), IntListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(UIntListPropield), UIntListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(FloatListPropield), FloatListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(LongListPropield), LongListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(ULongListPropield), ULongListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(DoubleListPropield), DoubleListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(DecimalListPropield), DecimalListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(VeryLongListPropield), VeryLongListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(VeryUlongListPropield), VeryUlongListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(BigIntListPropield), BigIntListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(ComplexListPropield), ComplexListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(DateTimeListPropield), DateTimeListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(DateOnlyListPropield), DateOnlyListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(TimeSpanListPropield), TimeSpanListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(TimeOnlyListPropield), TimeOnlyListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(RuneListPropield), RuneListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(GuidListPropield), GuidListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(IpNetworkListPropield), IpNetworkListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullByteListPropield), NullByteListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullSByteListPropield), NullSByteListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullCharListPropield), NullCharListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullShortListPropield), NullShortListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullUShortListPropield), NullUShortListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullHalfListPropield), NullHalfListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullIntListPropield), NullIntListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullUIntListPropield), NullUIntListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullFloatListPropield), NullFloatListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullLongListPropield), NullLongListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullULongListPropield), NullULongListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullDoubleListPropield), NullDoubleListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullDecimalListPropield), NullDecimalListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullVeryLongListPropield), NullVeryLongListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullVeryUlongListPropield), NullVeryUlongListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullBigIntListPropield), NullBigIntListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullComplexListPropield), NullComplexListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullDateTimeListPropield), NullDateTimeListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullDateOnlyListPropield), NullDateOnlyListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullTimeSpanListPropield), NullTimeSpanListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullTimeOnlyListPropield), NullTimeOnlyListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullRuneListPropield), NullRuneListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullGuidListPropield), NullGuidListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullIpNetworkListPropield), NullIpNetworkListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(StringListPropield), StringListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(StringBuilderListPropield), StringBuilderListPropield);
        ctb.CollectionField.WhenPopulatedAddAllCharSeq(nameof(CharSequenceListPropield), CharSequenceListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(VersionListPropield), VersionListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(IntPtrListPropield), IntPtrListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(UriListPropield), UriListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(SpanFormattableListPropield), SpanFormattableListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NdLNfEnumListPropield), NdLNfEnumListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NdUNfEnumListPropield), NdUNfEnumListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NdLWfEnumListPropield), NdLWfEnumListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NdUWfEnumListPropield), NdUWfEnumListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(WdLNfEnumListPropield), WdLNfEnumListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(WdUNfEnumListPropield), WdUNfEnumListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(WdLWfEnumListPropield), WdLWfEnumListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(WdUWfEnumListPropield), WdUWfEnumListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullNdLNfEnumListPropield), NullNdLNfEnumListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullNdUNfEnumListPropield), NullNdUNfEnumListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullNdLWfEnumListPropield), NullNdLWfEnumListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullNdUWfEnumListPropield), NullNdUWfEnumListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullWdLNfEnumListPropield), NullWdLNfEnumListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullWdUNfEnumListPropield), NullWdUNfEnumListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullWdLWfEnumListPropield), NullWdLWfEnumListPropield);
        ctb.CollectionField.WhenPopulatedAddAll(nameof(NullWdUWfEnumListPropield), NullWdUWfEnumListPropield);
        return ctb.Complete();
    }

    public StateExtractStringRange AlwaysAddFiltered(ITheOneString tos)
    {
        using var ctb =
            tos.StartComplexType(this);
        ctb.CollectionField.AlwaysAddFiltered(nameof(ByteListPropield), ByteListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(ByteListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(SByteListPropield), SByteListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(SByteListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(CharListPropield), CharListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(CharListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(ShortListPropield), ShortListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(ShortListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(UShortListPropield), UShortListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(UShortListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(HalfListPropield), HalfListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(HalfListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(IntListPropield), IntListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(IntListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(UIntListPropield), UIntListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(UIntListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(FloatListPropield), FloatListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(FloatListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(LongListPropield), LongListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(LongListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(ULongListPropield), ULongListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(ULongListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(DoubleListPropield), DoubleListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(DoubleListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(DecimalListPropield), DecimalListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(DecimalListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(VeryLongListPropield), VeryLongListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(VeryLongListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(VeryUlongListPropield), VeryUlongListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(VeryUlongListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(BigIntListPropield), BigIntListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(BigIntListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(ComplexListPropield), ComplexListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(ComplexListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(DateTimeListPropield), DateTimeListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(DateTimeListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(DateOnlyListPropield), DateOnlyListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(DateOnlyListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(TimeSpanListPropield), TimeSpanListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(TimeSpanListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(TimeOnlyListPropield), TimeOnlyListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(TimeOnlyListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(RuneListPropield), RuneListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(RuneListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(GuidListPropield), GuidListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(GuidListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(IpNetworkListPropield), IpNetworkListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(IpNetworkListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullByteListPropield), NullByteListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullByteListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullSByteListPropield), NullSByteListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullSByteListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullCharListPropield), NullCharListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullCharListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullShortListPropield), NullShortListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullShortListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullUShortListPropield), NullUShortListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullUShortListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullHalfListPropield), NullHalfListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullHalfListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullIntListPropield), NullIntListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullIntListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullUIntListPropield), NullUIntListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullUIntListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullFloatListPropield), NullFloatListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullFloatListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullLongListPropield), NullLongListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullLongListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullULongListPropield), NullULongListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullULongListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullDoubleListPropield), NullDoubleListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullDoubleListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullDecimalListPropield), NullDecimalListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullDecimalListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullVeryLongListPropield), NullVeryLongListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullVeryLongListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullVeryUlongListPropield), NullVeryUlongListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullVeryUlongListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullBigIntListPropield), NullBigIntListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullBigIntListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullComplexListPropield), NullComplexListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullComplexListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullDateTimeListPropield), NullDateTimeListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullDateTimeListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullDateOnlyListPropield), NullDateOnlyListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullDateOnlyListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullTimeSpanListPropield), NullTimeSpanListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullTimeSpanListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullTimeOnlyListPropield), NullTimeOnlyListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullTimeOnlyListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullRuneListPropield), NullRuneListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullRuneListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullGuidListPropield), NullGuidListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullGuidListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullIpNetworkListPropield), NullIpNetworkListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullIpNetworkListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(StringListPropield), StringListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(StringListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(StringBuilderListPropield), StringBuilderListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(StringBuilderListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFilteredCharSeq(nameof(CharSequenceListPropield), CharSequenceListPropield
                                                        , FilterRegistry.OrderedCollectionFilterDefault(CharSequenceListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(VersionListPropield), VersionListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(VersionListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(IntPtrListPropield), IntPtrListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(IntPtrListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(UriListPropield), UriListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(UriListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(SpanFormattableListPropield), SpanFormattableListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(SpanFormattableListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NdLNfEnumListPropield), NdLNfEnumListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NdLNfEnumListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NdUNfEnumListPropield), NdUNfEnumListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NdUNfEnumListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NdLWfEnumListPropield), NdLWfEnumListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NdLWfEnumListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NdUWfEnumListPropield), NdUWfEnumListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NdUWfEnumListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(WdLNfEnumListPropield), WdLNfEnumListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(WdLNfEnumListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(WdUNfEnumListPropield), WdUNfEnumListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(WdUNfEnumListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(WdLWfEnumListPropield), WdLWfEnumListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(WdLWfEnumListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(WdUWfEnumListPropield), WdUWfEnumListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(WdUWfEnumListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullNdLNfEnumListPropield), NullNdLNfEnumListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullNdLNfEnumListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullNdUNfEnumListPropield), NullNdUNfEnumListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullNdUNfEnumListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullNdLWfEnumListPropield), NullNdLWfEnumListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullNdLWfEnumListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullNdUWfEnumListPropield), NullNdUWfEnumListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullNdUWfEnumListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullWdLNfEnumListPropield), NullWdLNfEnumListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullWdLNfEnumListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullWdUNfEnumListPropield), NullWdUNfEnumListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullWdUNfEnumListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullWdLWfEnumListPropield), NullWdLWfEnumListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullWdLWfEnumListPropield).CheckPredicate);
        ctb.CollectionField.AlwaysAddFiltered(nameof(NullWdUWfEnumListPropield), NullWdUWfEnumListPropield
                                            , FilterRegistry.OrderedCollectionFilterDefault(NullWdUWfEnumListPropield).CheckPredicate);
        return ctb.Complete();
    }

    public StateExtractStringRange WhenPopulatedWithFilterReveal(ITheOneString tos)
    {
        using var ctb = tos.StartComplexType(this);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(ByteListPropield), ByteListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(ByteListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(SByteListPropield), SByteListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(SByteListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(CharListPropield), CharListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(CharListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(ShortListPropield), ShortListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(ShortListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(UShortListPropield), UShortListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(UShortListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(HalfListPropield), HalfListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(HalfListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(IntListPropield), IntListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(IntListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(UIntListPropield), UIntListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(UIntListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(FloatListPropield), FloatListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(FloatListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(LongListPropield), LongListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(LongListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(ULongListPropield), ULongListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(ULongListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(DoubleListPropield), DoubleListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(DoubleListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(DecimalListPropield), DecimalListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(DecimalListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(VeryLongListPropield), VeryLongListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(VeryLongListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(VeryUlongListPropield), VeryUlongListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(VeryUlongListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(BigIntListPropield), BigIntListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(BigIntListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(ComplexListPropield), ComplexListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(ComplexListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(DateTimeListPropield), DateTimeListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(DateTimeListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(DateOnlyListPropield), DateOnlyListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(DateOnlyListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(TimeSpanListPropield), TimeSpanListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(TimeSpanListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(TimeOnlyListPropield), TimeOnlyListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(TimeOnlyListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(RuneListPropield), RuneListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(RuneListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(GuidListPropield), GuidListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(GuidListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(IpNetworkListPropield), IpNetworkListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(IpNetworkListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullByteListPropield), NullByteListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullByteListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullSByteListPropield), NullSByteListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullSByteListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullCharListPropield), NullCharListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullCharListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullShortListPropield), NullShortListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullShortListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullUShortListPropield), NullUShortListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullUShortListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullHalfListPropield), NullHalfListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullHalfListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullIntListPropield), NullIntListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullIntListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullUIntListPropield), NullUIntListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullUIntListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullFloatListPropield), NullFloatListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullFloatListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullLongListPropield), NullLongListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullLongListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullULongListPropield), NullULongListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullULongListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullDoubleListPropield), NullDoubleListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullDoubleListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullDecimalListPropield), NullDecimalListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullDecimalListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullVeryLongListPropield), NullVeryLongListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullVeryLongListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullVeryUlongListPropield), NullVeryUlongListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullVeryUlongListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullBigIntListPropield), NullBigIntListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullBigIntListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullComplexListPropield), NullComplexListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullComplexListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullDateTimeListPropield), NullDateTimeListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullDateTimeListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullDateOnlyListPropield), NullDateOnlyListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullDateOnlyListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullTimeSpanListPropield), NullTimeSpanListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullTimeSpanListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullTimeOnlyListPropield), NullTimeOnlyListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullTimeOnlyListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullRuneListPropield), NullRuneListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullRuneListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullGuidListPropield), NullGuidListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullGuidListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullIpNetworkListPropield), NullIpNetworkListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullIpNetworkListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(StringListPropield), StringListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(StringListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(StringBuilderListPropield), StringBuilderListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(StringBuilderListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilterCharSeq(nameof(CharSequenceListPropield), CharSequenceListPropield
                                                              , FilterRegistry.OrderedCollectionFilterDefault(CharSequenceListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(VersionListPropield), VersionListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(VersionListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(IntPtrListPropield), IntPtrListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(IntPtrListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(UriListPropield), UriListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(UriListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(SpanFormattableListPropield), SpanFormattableListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(SpanFormattableListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NdLNfEnumListPropield), NdLNfEnumListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NdLNfEnumListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NdUNfEnumListPropield), NdUNfEnumListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NdUNfEnumListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NdLWfEnumListPropield), NdLWfEnumListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NdLWfEnumListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NdUWfEnumListPropield), NdUWfEnumListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NdUWfEnumListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(WdLNfEnumListPropield), WdLNfEnumListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(WdLNfEnumListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(WdUNfEnumListPropield), WdUNfEnumListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(WdUNfEnumListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(WdLWfEnumListPropield), WdLWfEnumListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(WdLWfEnumListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(WdUWfEnumListPropield), WdUWfEnumListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(WdUWfEnumListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullNdLNfEnumListPropield), NullNdLNfEnumListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullNdLNfEnumListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullNdUNfEnumListPropield), NullNdUNfEnumListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullNdUNfEnumListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullNdLWfEnumListPropield), NullNdLWfEnumListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullNdLWfEnumListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullNdUWfEnumListPropield), NullNdUWfEnumListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullNdUWfEnumListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullWdLNfEnumListPropield), NullWdLNfEnumListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullWdLNfEnumListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullWdUNfEnumListPropield), NullWdUNfEnumListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullWdUNfEnumListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullWdLWfEnumListPropield), NullWdLWfEnumListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullWdLWfEnumListPropield).CheckPredicate);
        ctb.CollectionField.WhenPopulatedWithFilter(nameof(NullWdUWfEnumListPropield), NullWdUWfEnumListPropield
                                                  , FilterRegistry.OrderedCollectionFilterDefault(NullWdUWfEnumListPropield).CheckPredicate);
        return ctb.Complete();
    }
}

[NoMatchingProductionClass]
public struct StandardListPropertyFieldStruct
{
    public StandardListPropertyFieldStruct()
    {
        InitializeAllSet();
    }

    public void InitializeAllSet()
    {
        ByteListPropield      = [byte.MinValue, 0, byte.MaxValue];
        SByteListPropield     = [sbyte.MinValue, 0, sbyte.MaxValue];
        CharListPropield      = [' ', '\u0000', '\uFFFF'];
        ShortListPropield     = [short.MinValue, 0, short.MaxValue];
        UShortListPropield    = [ushort.MinValue, 0, ushort.MaxValue];
        HalfListPropield      = [Half.MinValue, default, Half.NaN, Half.MaxValue];
        IntListPropield       = [int.MinValue, 0, int.MaxValue];
        UIntListPropield      = [uint.MinValue, 0, uint.MaxValue];
        FloatListPropield     = [float.MinValue, 0, float.NaN, float.MaxValue];
        LongListPropield      = [long.MinValue, 0, long.MaxValue];
        ULongListPropield     = [ulong.MinValue, 0, ulong.MaxValue];
        DoubleListPropield    = [double.MinValue, 0, double.NaN, double.MaxValue];
        DecimalListPropield   = [decimal.MinValue, 0, decimal.MaxValue];
        VeryLongListPropield  = [Int128.MinValue, default, Int128.MaxValue];
        VeryUlongListPropield = [UInt128.MinValue, default, UInt128.MaxValue];
        BigIntListPropield    = [BigInteger.Parse("-99999999999999999999999999"), default, BigInteger.Parse("99999999999999999999999999")];
        ComplexListPropield   = [new Complex(double.MaxValue * -1.0, double.MaxValue * -1), default, new Complex(double.MaxValue, double.MaxValue)];
        DateTimeListPropield  = [DateTime.MinValue, default, DateTime.MaxValue];
        DateOnlyListPropield  = [DateOnly.MinValue, default, DateOnly.MaxValue];
        TimeSpanListPropield  = [TimeSpan.MinValue, TimeSpan.Zero, TimeSpan.MaxValue];
        TimeOnlyListPropield  = [TimeOnly.MinValue, default, TimeOnly.MaxValue];
        RuneListPropield      = [Rune.GetRuneAt("\U00010000", 0), default, Rune.GetRuneAt("\U0010FFFF", 0)];
        GuidListPropield =
            [Guid.ParseExact("00000000-0000-0000-0000-000000000000", "X"), Guid.Empty, Guid.ParseExact("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", "X")];
        IpNetworkListPropield = [new IPNetwork(new IPAddress("\0\0\0\0"u8.ToArray()), 0), default, IPNetwork.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff")];

        NullByteListPropield      = [byte.MinValue, 0, null, byte.MaxValue];
        NullSByteListPropield     = [sbyte.MinValue, 0, null, sbyte.MaxValue];
        NullCharListPropield      = [' ', '\u0000', null, '\uFFFF'];
        NullShortListPropield     = [short.MinValue, 0, null, short.MaxValue];
        NullUShortListPropield    = [ushort.MinValue, 0, null, ushort.MaxValue];
        NullHalfListPropield      = [Half.MinValue, Half.Zero, Half.NaN, null, Half.MaxValue];
        NullIntListPropield       = [int.MinValue, 0, null, int.MaxValue];
        NullUIntListPropield      = [uint.MinValue, 0, null, uint.MaxValue];
        NullFloatListPropield     = [float.MinValue, 0f, float.NaN, null, float.MaxValue];
        NullLongListPropield      = [long.MinValue, 0, null, long.MaxValue];
        NullULongListPropield     = [ulong.MinValue, 0, null, ulong.MaxValue];
        NullDoubleListPropield    = [double.MinValue, 0d, double.NaN, null, double.MaxValue];
        NullDecimalListPropield   = [decimal.MinValue, 0m, null, decimal.MaxValue];
        NullVeryLongListPropield  = [Int128.MinValue, Int128.Zero, null, Int128.MaxValue];
        NullVeryUlongListPropield = [UInt128.MinValue, UInt128.Zero, null, UInt128.MaxValue];
        NullBigIntListPropield    = [BigInteger.Parse("-99999999999999999999999999"), BigInteger.Zero, null, BigInteger.Parse("99999999999999999999999999")];
        NullComplexListPropield =
            [new Complex(double.MaxValue * -1.0, double.MaxValue * -1), Complex.Zero, null, new Complex(double.MaxValue, double.MaxValue)];
        NullDateTimeListPropield = [DateTime.MinValue, new DateTime(), null, DateTime.MaxValue];
        NullDateOnlyListPropield = [DateOnly.MinValue, new DateOnly(), null, DateOnly.MaxValue];
        NullTimeSpanListPropield = [TimeSpan.MinValue, TimeSpan.Zero, null, TimeSpan.MaxValue];
        NullTimeOnlyListPropield = [TimeOnly.MinValue, null, TimeOnly.MaxValue];
        NullRuneListPropield     = [Rune.GetRuneAt("\U00010000", 0), Rune.GetRuneAt("\u0000", 0), null, Rune.GetRuneAt("\U0010FFFF", 0)];
        NullGuidListPropield =
            [Guid.ParseExact("00000000-0000-0000-0000-000000000000", "X"), Guid.Empty, null, Guid.ParseExact("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", "X")];
        NullIpNetworkListPropield =
            [new IPNetwork(new IPAddress("\0\0\0\0"u8.ToArray()), 0), new IPNetwork(), null, IPNetwork.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff")];

        StringListPropield        = ["stringListPropield_1", "", null!, "stringListPropield_4"];
        StringBuilderListPropield = [new("stringBuilderListPropield_1"), new StringBuilder(), null!, new StringBuilder("stringBuilderListPropield_4")];
        CharSequenceListPropield =
            [new MutableString("charSequenceListPropield_1"), new MutableString(), null!, new MutableString("charSequenceListPropield_4")];

        VersionListPropield = [new Version(0, 0), null!, new Version(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue)];
        IntPtrListPropield  = [new IPAddress("\0\0\0\0"u8.ToArray()), null!, IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff")];
        UriListPropield     = [new Uri(""), null!, new Uri("https://github.com/shwaindog/Fortitude")];

        SpanFormattableListPropield = [new MySpanFormattableClass(""), null!, new MySpanFormattableClass("SpanFormattableSingPropield")];
        NdLNfEnumListPropield       = [NoDefaultLongNoFlagsEnum.NDLNFE_1, default, NoDefaultLongNoFlagsEnum.NDLNFE_34];
        NdUNfEnumListPropield       = [NoDefaultULongNoFlagsEnum.NDUNFE_1, default, NoDefaultULongNoFlagsEnum.NDUNFE_34];
        NdLWfEnumListPropield =
        [
            NoDefaultLongWithFlagsEnum.NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_2, default
          , NoDefaultLongWithFlagsEnum.NDLWFE_33 | NoDefaultLongWithFlagsEnum.NDLWFE_34
        ];
        NdUWfEnumListPropield =
        [
            NoDefaultULongWithFlagsEnum.NDUWFE_1 | NoDefaultULongWithFlagsEnum.NDUWFE_2, default
          , NoDefaultULongWithFlagsEnum.NDUWFE_33 | NoDefaultULongWithFlagsEnum.NDUWFE_34
        ];

        WdLNfEnumListPropield = [WithDefaultLongNoFlagsEnum.WDLNFE_1, default, WithDefaultLongNoFlagsEnum.WDLNFE_34];
        WdUNfEnumListPropield = [WithDefaultULongNoFlagsEnum.WDUNFE_1, default, WithDefaultULongNoFlagsEnum.WDUNFE_34];
        WdLWfEnumListPropield =
        [
            WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_2, default
          , WithDefaultLongWithFlagsEnum.WDLWFE_33 | WithDefaultLongWithFlagsEnum.WDLWFE_34
        ];
        WdUWfEnumListPropield =
        [
            WithDefaultULongWithFlagsEnum.WDUWFE_1 | WithDefaultULongWithFlagsEnum.WDUWFE_2, default
          , WithDefaultULongWithFlagsEnum.WDUWFE_33 | WithDefaultULongWithFlagsEnum.WDUWFE_34
        ];

        NullNdLNfEnumListPropield = [NoDefaultLongNoFlagsEnum.NDLNFE_1, default(NoDefaultLongNoFlagsEnum), null, NoDefaultLongNoFlagsEnum.NDLNFE_34];
        NullNdUNfEnumListPropield = [NoDefaultULongNoFlagsEnum.NDUNFE_1, default(NoDefaultULongNoFlagsEnum), null, NoDefaultULongNoFlagsEnum.NDUNFE_34];
        NullNdLWfEnumListPropield =
        [
            NoDefaultLongWithFlagsEnum.NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_2, default(NoDefaultLongWithFlagsEnum), null
          , NoDefaultLongWithFlagsEnum.NDLWFE_33 | NoDefaultLongWithFlagsEnum.NDLWFE_34
        ];
        NullNdUWfEnumListPropield =
        [
            NoDefaultULongWithFlagsEnum.NDUWFE_1 | NoDefaultULongWithFlagsEnum.NDUWFE_2, default(NoDefaultULongWithFlagsEnum), null
          , NoDefaultULongWithFlagsEnum.NDUWFE_33 | NoDefaultULongWithFlagsEnum.NDUWFE_34
        ];

        NullWdLNfEnumListPropield = [WithDefaultLongNoFlagsEnum.WDLNFE_1, default(WithDefaultLongNoFlagsEnum), null, WithDefaultLongNoFlagsEnum.WDLNFE_34];
        NullWdUNfEnumListPropield = [WithDefaultULongNoFlagsEnum.WDUNFE_1, default(WithDefaultULongNoFlagsEnum), null, WithDefaultULongNoFlagsEnum.WDUNFE_34];
        NullWdLWfEnumListPropield =
        [
            WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_2, default(WithDefaultLongWithFlagsEnum), null
          , WithDefaultLongWithFlagsEnum.WDLWFE_33 | WithDefaultLongWithFlagsEnum.WDLWFE_34
        ];
        NullWdUWfEnumListPropield =
        [
            WithDefaultULongWithFlagsEnum.WDUWFE_1 | WithDefaultULongWithFlagsEnum.WDUWFE_2, default(WithDefaultULongWithFlagsEnum), null
          , WithDefaultULongWithFlagsEnum.WDUWFE_33 | WithDefaultULongWithFlagsEnum.WDUWFE_34
        ];
    }

    public void InitializeAtSize(int numberToGenerate)
    {
        ByteListPropield      = NumberTestDataGenerator.GenRandomNumberRange<byte>(numberToGenerate).ToList();
        SByteListPropield     = NumberTestDataGenerator.GenRandomNumberRange<sbyte>(numberToGenerate).ToList();
        CharListPropield      = NumberTestDataGenerator.GenRandomNumberRange<char>(numberToGenerate).ToList();
        ShortListPropield     = NumberTestDataGenerator.GenRandomNumberRange<short>(numberToGenerate).ToList();
        UShortListPropield    = NumberTestDataGenerator.GenRandomNumberRange<ushort>(numberToGenerate).ToList();
        HalfListPropield      = NumberTestDataGenerator.GenRandomNumberRange<Half>(numberToGenerate).ToList();
        IntListPropield       = NumberTestDataGenerator.GenRandomNumberRange<int>(numberToGenerate).ToList();
        UIntListPropield      = NumberTestDataGenerator.GenRandomNumberRange<uint>(numberToGenerate).ToList();
        FloatListPropield     = NumberTestDataGenerator.GenRandomNumberRange<float>(numberToGenerate).ToList();
        LongListPropield      = NumberTestDataGenerator.GenRandomNumberRange<long>(numberToGenerate).ToList();
        ULongListPropield     = NumberTestDataGenerator.GenRandomNumberRange<ulong>(numberToGenerate).ToList();
        DoubleListPropield    = NumberTestDataGenerator.GenRandomNumberRange<double>(numberToGenerate).ToList();
        DecimalListPropield   = NumberTestDataGenerator.GenRandomNumberRange<decimal>(numberToGenerate).ToList();
        VeryLongListPropield  = NumberTestDataGenerator.GenRandomNumberRange<Int128>(numberToGenerate).ToList();
        VeryUlongListPropield = NumberTestDataGenerator.GenRandomNumberRange<UInt128>(numberToGenerate).ToList();
        BigIntListPropield    = NumberTestDataGenerator.GenRandomNumberRange<BigInteger>(numberToGenerate).ToList();
        ComplexListPropield   = NumberTestDataGenerator.GenRandomNumberRange<Complex>(numberToGenerate).ToList();
        DateTimeListPropield  = DateTimeTestDataGenerator.GenRandomDateTimeRange(numberToGenerate).ToList();
        DateOnlyListPropield  = DateTimeTestDataGenerator.GenRandomDateOnlyRange(numberToGenerate).ToList();
        TimeSpanListPropield  = DateTimeTestDataGenerator.GenRandomTimeSpanRange(numberToGenerate).ToList();
        TimeOnlyListPropield  = DateTimeTestDataGenerator.GenRandomTimeOnlyRange(numberToGenerate).ToList();
        RuneListPropield      = NumberTestDataGenerator.GenRandomNumberRange<Rune>(numberToGenerate).ToList();
        GuidListPropield =
            NumberTestDataGenerator
                .GenRandomNumberRange<byte>(numberToGenerate)
                .Select(_ =>
                            new Guid(NumberTestDataGenerator
                                     .GenRandomNumberRange<
                                         byte>(16).ToArray()))
                .ToList();
        IpNetworkListPropield =
            NumberTestDataGenerator
                .GenRandomNumberRange<byte>(numberToGenerate)
                .Select(b =>
                {
                    if(b % 2 == 0){
                        return new IPNetwork
                            (new IPAddress
                                 (NumberTestDataGenerator.GenRandomNumberRange<byte>(4).ToArray()), (b % 32));
                    }
                    return new IPNetwork
                        (new IPAddress
                             (NumberTestDataGenerator.GenRandomNumberRange<byte>(16).ToArray()), (b % 128));
                }) .ToList();

        NullByteListPropield      = NumberTestDataGenerator.GenRandomNullableNumberRange<byte>(numberToGenerate).ToList();
        NullSByteListPropield     = NumberTestDataGenerator.GenRandomNullableNumberRange<sbyte>(numberToGenerate).ToList();
        NullCharListPropield      = NumberTestDataGenerator.GenRandomNullableNumberRange<char>(numberToGenerate).ToList();
        NullShortListPropield     = NumberTestDataGenerator.GenRandomNullableNumberRange<short>(numberToGenerate).ToList();
        NullUShortListPropield    = NumberTestDataGenerator.GenRandomNullableNumberRange<ushort>(numberToGenerate).ToList();
        NullHalfListPropield      = NumberTestDataGenerator.GenRandomNullableNumberRange<Half>(numberToGenerate).ToList();
        NullIntListPropield       = NumberTestDataGenerator.GenRandomNullableNumberRange<int>(numberToGenerate).ToList();
        NullUIntListPropield      = NumberTestDataGenerator.GenRandomNullableNumberRange<uint>(numberToGenerate).ToList();
        NullFloatListPropield     = NumberTestDataGenerator.GenRandomNullableNumberRange<float>(numberToGenerate).ToList();
        NullLongListPropield      = NumberTestDataGenerator.GenRandomNullableNumberRange<long>(numberToGenerate).ToList();
        NullULongListPropield     = NumberTestDataGenerator.GenRandomNullableNumberRange<ulong>(numberToGenerate).ToList();
        NullDoubleListPropield    = NumberTestDataGenerator.GenRandomNullableNumberRange<double>(numberToGenerate).ToList();
        NullDecimalListPropield   = NumberTestDataGenerator.GenRandomNullableNumberRange<decimal>(numberToGenerate).ToList();
        NullVeryLongListPropield  = NumberTestDataGenerator.GenRandomNullableNumberRange<Int128>(numberToGenerate).ToList();
        NullVeryUlongListPropield = NumberTestDataGenerator.GenRandomNullableNumberRange<UInt128>(numberToGenerate).ToList();
        NullBigIntListPropield    = NumberTestDataGenerator.GenRandomNullableNumberRange<BigInteger>(numberToGenerate).ToList();
        NullComplexListPropield   = NumberTestDataGenerator.GenRandomNullableNumberRange<Complex>(numberToGenerate).ToList();
        NullDateTimeListPropield  = DateTimeTestDataGenerator.GenRandomNullableDateTimeRange(numberToGenerate).ToList();
        NullDateOnlyListPropield  = DateTimeTestDataGenerator.GenRandomNullableDateOnlyRange(numberToGenerate).ToList();
        NullTimeSpanListPropield  = DateTimeTestDataGenerator.GenRandomNullableTimeSpanRange(numberToGenerate).ToList();
        NullTimeOnlyListPropield  = DateTimeTestDataGenerator.GenRandomNullableTimeOnlyRange(numberToGenerate).ToList();
        NullRuneListPropield      = NumberTestDataGenerator.GenRandomNullableNumberRange<Rune>(numberToGenerate).ToList();
        
        NullGuidListPropield      = 
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<byte>(numberToGenerate)
                .Select(b =>
                        {
                            if(b == null) return null;
                            return  new Guid?(new Guid(NumberTestDataGenerator
                                                       .GenRandomNumberRange<
                                                           byte>(16).ToArray()));
                        }
                       )
                .ToList();
        NullIpNetworkListPropield = 
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<byte>(numberToGenerate)
                .Select(b =>
                {
                    if(b == null) return null;
                    if(b % 2 == 0){
                        return new IPNetwork?( new IPNetwork
                            (new IPAddress
                                 (NumberTestDataGenerator.GenRandomNumberRange<byte>(4).ToArray()), (b.Value % 32)));
                    }
                    return new IPNetwork( new IPAddress
                                             (NumberTestDataGenerator.GenRandomNumberRange<byte>(16).ToArray()), (b.Value % 128));
                }) .ToList();

        StringListPropield        = 
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<int>(numberToGenerate)
                .Select(num =>
                {
                    if(num == null) return null!;
                    return "stringListPropield_" + num;
                }) .ToList();
        StringBuilderListPropield = 
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<int>(numberToGenerate)
                .Select(num =>
                {
                    if(num == null) return null!;
                    return new StringBuilder("stringBuilderListPropield_1" + num);
                }) .ToList();
        CharSequenceListPropield =
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<int>(numberToGenerate)
                .Select(num =>
                {
                    if(num == null) return null!;
                    return (ICharSequence)new MutableString("charSequenceListPropield_1" + num);
                }) .ToList();

        VersionListPropield = 
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<int>(numberToGenerate)
                .Select(num =>
                {
                    if(num == null) return null!;
                    var otherThree = NumberTestDataGenerator
                        .GenRandomNumberRange<int>(numberToGenerate).Select(Math.Abs).ToList();
                    return new Version(Math.Abs(num.Value), otherThree[0], otherThree[1], otherThree[2]);
                }) .ToList();
        
        IntPtrListPropield  = 
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<byte>(numberToGenerate)
                .Select(b =>
                {
                    if(b == null) return null!;
                    if(b % 2 == 0){
                        return new IPAddress(NumberTestDataGenerator.GenRandomNumberRange<byte>(4).ToArray());
                    }
                    return  new IPAddress(NumberTestDataGenerator.GenRandomNumberRange<byte>(16).ToArray());
                }) .ToList();
        UriListPropield     = 
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<int>(numberToGenerate)
                .Select(num =>
                {
                    if(num == null) return null!;
                    return new Uri("https://www.someWebAddress_" + num + ".net");
                }) .ToList(); 

        SpanFormattableListPropield = 
            NumberTestDataGenerator
                .GenRandomNullableNumberRange<int>(numberToGenerate)
                .Select(num =>
                {
                    if(num == null) return null!;
                    return new MySpanFormattableClass(" SpanFormattableListPropield_" + num);
                }) .ToList();
        
        NdLNfEnumListPropield = EnumTestDataGenerator.GenRandomEnumValues<NoDefaultLongNoFlagsEnum>(numberToGenerate).ToList();
        NdUNfEnumListPropield = EnumTestDataGenerator.GenRandomEnumValues<NoDefaultULongNoFlagsEnum>(numberToGenerate).ToList();
        NdLWfEnumListPropield = EnumTestDataGenerator.GenRandomEnumMultiFlagValues<NoDefaultLongWithFlagsEnum>(numberToGenerate).ToList();
        NdUWfEnumListPropield = EnumTestDataGenerator.GenRandomEnumMultiFlagValues<NoDefaultULongWithFlagsEnum>(numberToGenerate).ToList();

        WdLNfEnumListPropield = EnumTestDataGenerator.GenRandomEnumValues<WithDefaultLongNoFlagsEnum>(numberToGenerate).ToList();
        WdUNfEnumListPropield = EnumTestDataGenerator.GenRandomEnumValues<WithDefaultULongNoFlagsEnum>(numberToGenerate).ToList();
        WdLWfEnumListPropield = EnumTestDataGenerator.GenRandomEnumMultiFlagValues<WithDefaultLongWithFlagsEnum>(numberToGenerate).ToList();
        WdUWfEnumListPropield = EnumTestDataGenerator.GenRandomEnumMultiFlagValues<WithDefaultULongWithFlagsEnum>(numberToGenerate).ToList();

        NullNdLNfEnumListPropield = EnumTestDataGenerator.GenRandomNullableEnumValues<NoDefaultLongNoFlagsEnum>(numberToGenerate).ToList();
        NullNdUNfEnumListPropield = EnumTestDataGenerator.GenRandomNullableEnumValues<NoDefaultULongNoFlagsEnum>(numberToGenerate).ToList();
        NullNdLWfEnumListPropield = EnumTestDataGenerator.GenRandomNullableEnumMultiFlagValues<NoDefaultLongWithFlagsEnum>(numberToGenerate).ToList();
        NullNdUWfEnumListPropield = EnumTestDataGenerator.GenRandomNullableEnumMultiFlagValues<NoDefaultULongWithFlagsEnum>(numberToGenerate).ToList();

        NullWdLNfEnumListPropield = EnumTestDataGenerator.GenRandomNullableEnumValues<WithDefaultLongNoFlagsEnum>(numberToGenerate).ToList();
        NullWdUNfEnumListPropield = EnumTestDataGenerator.GenRandomNullableEnumValues<WithDefaultULongNoFlagsEnum>(numberToGenerate).ToList();
        NullWdLWfEnumListPropield = EnumTestDataGenerator.GenRandomNullableEnumMultiFlagValues<WithDefaultLongWithFlagsEnum>(numberToGenerate).ToList();
        NullWdUWfEnumListPropield = EnumTestDataGenerator.GenRandomNullableEnumMultiFlagValues<WithDefaultULongWithFlagsEnum>(numberToGenerate).ToList();
    }


    public void InitializeAllNull()
    {
        ByteListPropield      = null!;
        SByteListPropield     = null!;
        CharListPropield      = null!;
        ShortListPropield     = null!;
        UShortListPropield    = null!;
        HalfListPropield      = null!;
        IntListPropield       = null!;
        UIntListPropield      = null!;
        FloatListPropield     = null!;
        LongListPropield      = null!;
        ULongListPropield     = null!;
        DoubleListPropield    = null!;
        DecimalListPropield   = null!;
        VeryLongListPropield  = null!;
        VeryUlongListPropield = null!;
        BigIntListPropield    = null!;

        ComplexListPropield   = null!;
        DateTimeListPropield  = null!;
        DateOnlyListPropield  = null!;
        TimeSpanListPropield  = null!;
        TimeOnlyListPropield  = null!;
        RuneListPropield      = null!;
        GuidListPropield      = null!;
        IpNetworkListPropield = null!;

        NullByteListPropield      = null!;
        NullSByteListPropield     = null!;
        NullCharListPropield      = null!;
        NullShortListPropield     = null!;
        NullUShortListPropield    = null!;
        NullHalfListPropield      = null!;
        NullIntListPropield       = null!;
        NullUIntListPropield      = null!;
        NullFloatListPropield     = null!;
        NullLongListPropield      = null!;
        NullULongListPropield     = null!;
        NullDoubleListPropield    = null!;
        NullDecimalListPropield   = null!;
        NullVeryLongListPropield  = null!;
        NullVeryUlongListPropield = null!;
        NullBigIntListPropield    = null!;

        NullComplexListPropield   = null!;
        NullDateTimeListPropield  = null!;
        NullDateOnlyListPropield  = null!;
        NullTimeSpanListPropield  = null!;
        NullTimeOnlyListPropield  = null!;
        NullRuneListPropield      = null!;
        NullGuidListPropield      = null!;
        NullIpNetworkListPropield = null!;

        StringListPropield        = null!;
        StringBuilderListPropield = null!;
        CharSequenceListPropield  = null!;

        VersionListPropield = null!;
        IntPtrListPropield  = null!;
        UriListPropield     = null!;

        SpanFormattableListPropield = null!;

        NdLNfEnumListPropield = null!;
        NdUNfEnumListPropield = null!;
        NdLWfEnumListPropield = null!;
        NdUWfEnumListPropield = null!;

        WdLNfEnumListPropield = null!;
        WdUNfEnumListPropield = null!;
        WdLWfEnumListPropield = null!;
        WdUWfEnumListPropield = null!;

        NullNdLNfEnumListPropield = null!;
        NullNdUNfEnumListPropield = null!;
        NullNdLWfEnumListPropield = null!;
        NullNdUWfEnumListPropield = null!;

        NullWdLNfEnumListPropield = null!;
        NullWdUNfEnumListPropield = null!;
        NullWdLWfEnumListPropield = null!;
        NullWdUWfEnumListPropield = null!;
    }

    public List<byte> ByteListPropield = null!;
    public List<sbyte> SByteListPropield { get; set; } = null!;
    public List<char> CharListPropield = null!;
    public List<short> ShortListPropield { get; set; } = null!;
    public List<ushort> UShortListPropield = null!;
    public List<Half> HalfListPropield { get; set; } = null!;
    public List<int> IntListPropield = null!;
    public List<uint> UIntListPropield { get; set; } = null!;
    public List<float> FloatListPropield = null!;
    public List<long> LongListPropield { get; set; } = null!;
    public List<ulong> ULongListPropield = null!;
    public List<double> DoubleListPropield { get; set; } = null!;
    public List<decimal> DecimalListPropield = null!;
    public List<Int128> VeryLongListPropield { get; set; } = null!;
    public List<UInt128> VeryUlongListPropield = null!;
    public List<BigInteger> BigIntListPropield { get; set; } = null!;
    public List<Complex> ComplexListPropield = null!;
    public List<DateTime> DateTimeListPropield { get; set; } = null!;
    public List<DateOnly> DateOnlyListPropield = null!;
    public List<TimeSpan> TimeSpanListPropield { get; set; } = null!;
    public List<TimeOnly> TimeOnlyListPropield = null!;
    public List<Rune> RuneListPropield { get; set; } = null!;
    public List<Guid> GuidListPropield = null!;
    public List<IPNetwork> IpNetworkListPropield { get; set; } = null!;

    public List<byte?> NullByteListPropield { get; set; } = null!;
    public List<sbyte?> NullSByteListPropield = null!;
    public List<char?> NullCharListPropield { get; set; } = null!;
    public List<short?> NullShortListPropield = null!;
    public List<ushort?> NullUShortListPropield { get; set; } = null!;
    public List<Half?> NullHalfListPropield = null!;
    public List<int?> NullIntListPropield { get; set; } = null!;
    public List<uint?> NullUIntListPropield = null!;
    public List<float?> NullFloatListPropield { get; set; } = null!;
    public List<long?> NullLongListPropield = null!;
    public List<ulong?> NullULongListPropield { get; set; } = null!;
    public List<double?> NullDoubleListPropield = null!;
    public List<decimal?> NullDecimalListPropield { get; set; } = null!;
    public List<Int128?> NullVeryLongListPropield = null!;
    public List<UInt128?> NullVeryUlongListPropield { get; set; } = null!;
    public List<BigInteger?> NullBigIntListPropield = null!;

    public List<Complex?> NullComplexListPropield = null!;
    public List<DateTime?> NullDateTimeListPropield { get; set; } = null!;
    public List<DateOnly?> NullDateOnlyListPropield = null!;
    public List<TimeSpan?> NullTimeSpanListPropield { get; set; } = null!;
    public List<TimeOnly?> NullTimeOnlyListPropield = null!;
    public List<Rune?> NullRuneListPropield { get; set; } = null!;
    public List<Guid?> NullGuidListPropield = null!;
    public List<IPNetwork?> NullIpNetworkListPropield { get; set; } = null!;
    public List<string> StringListPropield { get; set; } = null!;
    public List<StringBuilder> StringBuilderListPropield = null!;
    public List<ICharSequence> CharSequenceListPropield { get; set; } = null!;

    public List<Version> VersionListPropield { get; set; } = null!;
    public List<IPAddress> IntPtrListPropield = null!;
    public List<Uri> UriListPropield { get; set; } = null!;

    public List<MySpanFormattableClass> SpanFormattableListPropield = null!;

    public List<NoDefaultLongNoFlagsEnum> NdLNfEnumListPropield { get; set; } = null!;
    public List<NoDefaultULongNoFlagsEnum> NdUNfEnumListPropield = null!;
    public List<NoDefaultLongWithFlagsEnum> NdLWfEnumListPropield { get; set; } = null!;
    public List<NoDefaultULongWithFlagsEnum> NdUWfEnumListPropield = null!;

    public List<WithDefaultLongNoFlagsEnum> WdLNfEnumListPropield = null!;
    public List<WithDefaultULongNoFlagsEnum> WdUNfEnumListPropield { get; set; } = null!;
    public List<WithDefaultLongWithFlagsEnum> WdLWfEnumListPropield = null!;
    public List<WithDefaultULongWithFlagsEnum> WdUWfEnumListPropield { get; set; } = null!;

    public List<NoDefaultLongNoFlagsEnum?> NullNdLNfEnumListPropield { get; set; } = null!;
    public List<NoDefaultULongNoFlagsEnum?> NullNdUNfEnumListPropield = null!;
    public List<NoDefaultLongWithFlagsEnum?> NullNdLWfEnumListPropield { get; set; } = null!;
    public List<NoDefaultULongWithFlagsEnum?> NullNdUWfEnumListPropield = null!;

    public List<WithDefaultLongNoFlagsEnum?> NullWdLNfEnumListPropield = null!;
    public List<WithDefaultULongNoFlagsEnum?> NullWdUNfEnumListPropield { get; set; } = null!;
    public List<WithDefaultLongWithFlagsEnum?> NullWdLWfEnumListPropield = null!;
    public List<WithDefaultULongWithFlagsEnum?> NullWdUWfEnumListPropield { get; set; } = null!;

    public static IFilterRegistry FilterRegistry { get; set; } = new FilterRegistry(new AddOddRetrieveCountFactory());

    public static PalantírReveal<StandardListPropertyFieldStruct> SelectStateRevealer(TestCollectionFieldRevealMode testCollectionFieldRevealMode)
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

    public static PalantírReveal<StandardListPropertyFieldStruct> AlwaysRevealAllState
    {
        get
        {
            return
                (sapfs, tos) =>
                {
                    using var ctb =
                        tos.StartComplexType(sapfs);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.ByteListPropield), sapfs.ByteListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.SByteListPropield), sapfs.SByteListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.CharListPropield), sapfs.CharListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.ShortListPropield), sapfs.ShortListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.UShortListPropield), sapfs.UShortListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.HalfListPropield), sapfs.HalfListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.IntListPropield), sapfs.IntListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.UIntListPropield), sapfs.UIntListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.FloatListPropield), sapfs.FloatListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.LongListPropield), sapfs.LongListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.ULongListPropield), sapfs.ULongListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.DoubleListPropield), sapfs.DoubleListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.DecimalListPropield), sapfs.DecimalListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.VeryLongListPropield), sapfs.VeryLongListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.VeryUlongListPropield), sapfs.VeryUlongListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.BigIntListPropield), sapfs.BigIntListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.ComplexListPropield), sapfs.ComplexListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.DateTimeListPropield), sapfs.DateTimeListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.DateOnlyListPropield), sapfs.DateOnlyListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.TimeSpanListPropield), sapfs.TimeSpanListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.TimeOnlyListPropield), sapfs.TimeOnlyListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.RuneListPropield), sapfs.RuneListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.GuidListPropield), sapfs.GuidListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.IpNetworkListPropield), sapfs.IpNetworkListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullByteListPropield), sapfs.NullByteListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullSByteListPropield), sapfs.NullSByteListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullCharListPropield), sapfs.NullCharListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullShortListPropield), sapfs.NullShortListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullUShortListPropield), sapfs.NullUShortListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullHalfListPropield), sapfs.NullHalfListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullIntListPropield), sapfs.NullIntListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullUIntListPropield), sapfs.NullUIntListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullFloatListPropield), sapfs.NullFloatListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullLongListPropield), sapfs.NullLongListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullULongListPropield), sapfs.NullULongListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullDoubleListPropield), sapfs.NullDoubleListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullDecimalListPropield), sapfs.NullDecimalListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullVeryLongListPropield), sapfs.NullVeryLongListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullVeryUlongListPropield), sapfs.NullVeryUlongListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullBigIntListPropield), sapfs.NullBigIntListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullComplexListPropield), sapfs.NullComplexListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullDateTimeListPropield), sapfs.NullDateTimeListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullDateOnlyListPropield), sapfs.NullDateOnlyListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullTimeSpanListPropield), sapfs.NullTimeSpanListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullTimeOnlyListPropield), sapfs.NullTimeOnlyListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullRuneListPropield), sapfs.NullRuneListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullGuidListPropield), sapfs.NullGuidListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullIpNetworkListPropield), sapfs.NullIpNetworkListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.StringListPropield), sapfs.StringListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.StringBuilderListPropield), sapfs.StringBuilderListPropield);
                    ctb.CollectionField.AlwaysAddAllCharSeq(nameof(sapfs.CharSequenceListPropield), sapfs.CharSequenceListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.VersionListPropield), sapfs.VersionListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.IntPtrListPropield), sapfs.IntPtrListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.UriListPropield), sapfs.UriListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.SpanFormattableListPropield), sapfs.SpanFormattableListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NdLNfEnumListPropield), sapfs.NdLNfEnumListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NdUNfEnumListPropield), sapfs.NdUNfEnumListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NdLWfEnumListPropield), sapfs.NdLWfEnumListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NdUWfEnumListPropield), sapfs.NdUWfEnumListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.WdLNfEnumListPropield), sapfs.WdLNfEnumListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.WdUNfEnumListPropield), sapfs.WdUNfEnumListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.WdLWfEnumListPropield), sapfs.WdLWfEnumListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.WdUWfEnumListPropield), sapfs.WdUWfEnumListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullNdLNfEnumListPropield), sapfs.NullNdLNfEnumListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullNdUNfEnumListPropield), sapfs.NullNdUNfEnumListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullNdLWfEnumListPropield), sapfs.NullNdLWfEnumListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullNdUWfEnumListPropield), sapfs.NullNdUWfEnumListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullWdLNfEnumListPropield), sapfs.NullWdLNfEnumListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullWdUNfEnumListPropield), sapfs.NullWdUNfEnumListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullWdLWfEnumListPropield), sapfs.NullWdLWfEnumListPropield);
                    ctb.CollectionField.AlwaysAddAll(nameof(sapfs.NullWdUWfEnumListPropield), sapfs.NullWdUWfEnumListPropield);
                    return ctb.Complete();
                };
        }
    }

    public static PalantírReveal<StandardListPropertyFieldStruct> WhenPopulatedReveal
    {
        get
        {
            return
                (sapfs, tos) =>
                {
                    using var ctb =
                        tos.StartComplexType(sapfs);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.ByteListPropield), sapfs.ByteListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.SByteListPropield), sapfs.SByteListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.CharListPropield), sapfs.CharListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.ShortListPropield), sapfs.ShortListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.UShortListPropield), sapfs.UShortListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.HalfListPropield), sapfs.HalfListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.IntListPropield), sapfs.IntListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.UIntListPropield), sapfs.UIntListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.FloatListPropield), sapfs.FloatListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.LongListPropield), sapfs.LongListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.ULongListPropield), sapfs.ULongListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.DoubleListPropield), sapfs.DoubleListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.DecimalListPropield), sapfs.DecimalListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.VeryLongListPropield), sapfs.VeryLongListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.VeryUlongListPropield), sapfs.VeryUlongListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.BigIntListPropield), sapfs.BigIntListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.ComplexListPropield), sapfs.ComplexListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.DateTimeListPropield), sapfs.DateTimeListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.DateOnlyListPropield), sapfs.DateOnlyListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.TimeSpanListPropield), sapfs.TimeSpanListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.TimeOnlyListPropield), sapfs.TimeOnlyListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.RuneListPropield), sapfs.RuneListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.GuidListPropield), sapfs.GuidListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.IpNetworkListPropield), sapfs.IpNetworkListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullByteListPropield), sapfs.NullByteListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullSByteListPropield), sapfs.NullSByteListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullCharListPropield), sapfs.NullCharListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullShortListPropield), sapfs.NullShortListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullUShortListPropield), sapfs.NullUShortListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullHalfListPropield), sapfs.NullHalfListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullIntListPropield), sapfs.NullIntListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullUIntListPropield), sapfs.NullUIntListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullFloatListPropield), sapfs.NullFloatListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullLongListPropield), sapfs.NullLongListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullULongListPropield), sapfs.NullULongListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullDoubleListPropield), sapfs.NullDoubleListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullDecimalListPropield), sapfs.NullDecimalListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullVeryLongListPropield), sapfs.NullVeryLongListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullVeryUlongListPropield), sapfs.NullVeryUlongListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullBigIntListPropield), sapfs.NullBigIntListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullComplexListPropield), sapfs.NullComplexListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullDateTimeListPropield), sapfs.NullDateTimeListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullDateOnlyListPropield), sapfs.NullDateOnlyListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullTimeSpanListPropield), sapfs.NullTimeSpanListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullTimeOnlyListPropield), sapfs.NullTimeOnlyListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullRuneListPropield), sapfs.NullRuneListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullGuidListPropield), sapfs.NullGuidListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullIpNetworkListPropield), sapfs.NullIpNetworkListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.StringListPropield), sapfs.StringListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.StringBuilderListPropield), sapfs.StringBuilderListPropield);
                    ctb.CollectionField.WhenPopulatedAddAllCharSeq(nameof(sapfs.CharSequenceListPropield), sapfs.CharSequenceListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.VersionListPropield), sapfs.VersionListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.IntPtrListPropield), sapfs.IntPtrListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.UriListPropield), sapfs.UriListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.SpanFormattableListPropield), sapfs.SpanFormattableListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NdLNfEnumListPropield), sapfs.NdLNfEnumListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NdUNfEnumListPropield), sapfs.NdUNfEnumListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NdLWfEnumListPropield), sapfs.NdLWfEnumListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NdUWfEnumListPropield), sapfs.NdUWfEnumListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.WdLNfEnumListPropield), sapfs.WdLNfEnumListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.WdUNfEnumListPropield), sapfs.WdUNfEnumListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.WdLWfEnumListPropield), sapfs.WdLWfEnumListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.WdUWfEnumListPropield), sapfs.WdUWfEnumListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullNdLNfEnumListPropield), sapfs.NullNdLNfEnumListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullNdUNfEnumListPropield), sapfs.NullNdUNfEnumListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullNdLWfEnumListPropield), sapfs.NullNdLWfEnumListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullNdUWfEnumListPropield), sapfs.NullNdUWfEnumListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullWdLNfEnumListPropield), sapfs.NullWdLNfEnumListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullWdUNfEnumListPropield), sapfs.NullWdUNfEnumListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullWdLWfEnumListPropield), sapfs.NullWdLWfEnumListPropield);
                    ctb.CollectionField.WhenPopulatedAddAll(nameof(sapfs.NullWdUWfEnumListPropield), sapfs.NullWdUWfEnumListPropield);
                    return ctb.Complete();
                };
        }
    }

    public static PalantírReveal<StandardListPropertyFieldStruct> AlwaysAddFiltered
    {
        get
        {
            return
                (sapfs, tos) =>
                {
                    using var ctb = tos.StartComplexType(sapfs);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.ByteListPropield), sapfs.ByteListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.ByteListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.SByteListPropield), sapfs.SByteListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.SByteListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.CharListPropield), sapfs.CharListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.CharListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.ShortListPropield), sapfs.ShortListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.ShortListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.UShortListPropield), sapfs.UShortListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.UShortListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.HalfListPropield), sapfs.HalfListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.HalfListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.IntListPropield), sapfs.IntListPropield, FilterRegistry.OrderedCollectionFilterDefault
                                                              (sapfs.IntListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.UIntListPropield), sapfs.UIntListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.UIntListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.FloatListPropield), sapfs.FloatListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.FloatListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.LongListPropield), sapfs.LongListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.LongListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.ULongListPropield), sapfs.ULongListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.ULongListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.DoubleListPropield), sapfs.DoubleListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.DoubleListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.DecimalListPropield), sapfs.DecimalListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.DecimalListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.VeryLongListPropield), sapfs.VeryLongListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.VeryLongListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.VeryUlongListPropield), sapfs.VeryUlongListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.VeryUlongListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.BigIntListPropield), sapfs.BigIntListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.BigIntListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.ComplexListPropield), sapfs.ComplexListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.ComplexListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.DateTimeListPropield), sapfs.DateTimeListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.DateTimeListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.DateOnlyListPropield), sapfs.DateOnlyListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.DateOnlyListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.TimeSpanListPropield), sapfs.TimeSpanListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.TimeSpanListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.TimeOnlyListPropield), sapfs.TimeOnlyListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.TimeOnlyListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.RuneListPropield), sapfs.RuneListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.RuneListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.GuidListPropield), sapfs.GuidListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.GuidListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.IpNetworkListPropield), sapfs.IpNetworkListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.IpNetworkListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullByteListPropield), sapfs.NullByteListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NullByteListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullSByteListPropield), sapfs.NullSByteListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NullSByteListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullCharListPropield), sapfs.NullCharListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NullCharListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullShortListPropield), sapfs.NullShortListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NullShortListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullUShortListPropield), sapfs.NullUShortListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullUShortListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullHalfListPropield), sapfs.NullHalfListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NullHalfListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullIntListPropield), sapfs.NullIntListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NullIntListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullUIntListPropield), sapfs.NullUIntListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NullUIntListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullFloatListPropield), sapfs.NullFloatListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NullFloatListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullLongListPropield), sapfs.NullLongListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NullLongListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullULongListPropield), sapfs.NullULongListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NullULongListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullDoubleListPropield), sapfs.NullDoubleListPropield
                                                        , FilterRegistry.OrderedCollectionFilterDefault(sapfs.NullDoubleListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullDecimalListPropield), sapfs.NullDecimalListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullDecimalListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullVeryLongListPropield), sapfs.NullVeryLongListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullVeryLongListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullVeryUlongListPropield), sapfs.NullVeryUlongListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullVeryUlongListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullBigIntListPropield), sapfs.NullBigIntListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullBigIntListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullComplexListPropield), sapfs.NullComplexListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullComplexListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullDateTimeListPropield), sapfs.NullDateTimeListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullDateTimeListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullDateOnlyListPropield), sapfs.NullDateOnlyListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullDateOnlyListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullTimeSpanListPropield), sapfs.NullTimeSpanListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullTimeSpanListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullTimeOnlyListPropield), sapfs.NullTimeOnlyListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullTimeOnlyListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullRuneListPropield), sapfs.NullRuneListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NullRuneListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullGuidListPropield), sapfs.NullGuidListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NullGuidListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullIpNetworkListPropield), sapfs.NullIpNetworkListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullIpNetworkListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.StringListPropield), sapfs.StringListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.StringListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.StringBuilderListPropield), sapfs.StringBuilderListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.StringBuilderListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFilteredCharSeq(nameof(sapfs.CharSequenceListPropield), sapfs.CharSequenceListPropield, FilterRegistry
                                                                          .OrderedCollectionFilterDefault(sapfs.CharSequenceListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.VersionListPropield), sapfs.VersionListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.VersionListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.IntPtrListPropield), sapfs.IntPtrListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.IntPtrListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.UriListPropield), sapfs.UriListPropield, FilterRegistry.OrderedCollectionFilterDefault
                                                              (sapfs.UriListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.SpanFormattableListPropield), sapfs.SpanFormattableListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.SpanFormattableListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NdLNfEnumListPropield), sapfs.NdLNfEnumListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NdLNfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NdUNfEnumListPropield), sapfs.NdUNfEnumListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NdUNfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NdLWfEnumListPropield), sapfs.NdLWfEnumListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NdLWfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NdUWfEnumListPropield), sapfs.NdUWfEnumListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.NdUWfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.WdLNfEnumListPropield), sapfs.WdLNfEnumListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.WdLNfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.WdUNfEnumListPropield), sapfs.WdUNfEnumListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.WdUNfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.WdLWfEnumListPropield), sapfs.WdLWfEnumListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.WdLWfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.WdUWfEnumListPropield), sapfs.WdUWfEnumListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault
                                                                  (sapfs.WdUWfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullNdLNfEnumListPropield), sapfs.NullNdLNfEnumListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullNdLNfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullNdUNfEnumListPropield), sapfs.NullNdUNfEnumListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullNdUNfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullNdLWfEnumListPropield), sapfs.NullNdLWfEnumListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullNdLWfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullNdUWfEnumListPropield), sapfs.NullNdUWfEnumListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullNdUWfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullWdLNfEnumListPropield), sapfs.NullWdLNfEnumListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullWdLNfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullWdUNfEnumListPropield), sapfs.NullWdUNfEnumListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullWdUNfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullWdLWfEnumListPropield), sapfs.NullWdLWfEnumListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullWdLWfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.NullWdUWfEnumListPropield), sapfs.NullWdUWfEnumListPropield, FilterRegistry
                                                              .OrderedCollectionFilterDefault(sapfs.NullWdUWfEnumListPropield).CheckPredicate);
                    return ctb.Complete();
                };
        }
    }

    public static PalantírReveal<StandardListPropertyFieldStruct> WhenPopulatedWithFilterReveal
    {
        get
        {
            return
                (sapfs, tos) =>
                {
                    using var ctb = tos.StartComplexType(sapfs);
                    ctb.CollectionField.AlwaysAddFiltered(nameof(sapfs.ByteListPropield), sapfs.ByteListPropield
                                                        , FilterRegistry.OrderedCollectionFilterDefault(sapfs.ByteListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.ByteListPropield), sapfs.ByteListPropield
                                                              , FilterRegistry.OrderedCollectionFilterDefault(sapfs.ByteListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.SByteListPropield), sapfs.SByteListPropield
                                                              , FilterRegistry.OrderedCollectionFilterDefault(sapfs.SByteListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.CharListPropield), sapfs.CharListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.CharListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.ShortListPropield), sapfs.ShortListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.ShortListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.UShortListPropield), sapfs.UShortListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.UShortListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.HalfListPropield), sapfs.HalfListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.HalfListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.IntListPropield), sapfs.IntListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs
                                                                                                        .IntListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.UIntListPropield), sapfs.UIntListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.UIntListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.FloatListPropield), sapfs.FloatListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.FloatListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.LongListPropield), sapfs.LongListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.LongListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.ULongListPropield), sapfs.ULongListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.ULongListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.DoubleListPropield), sapfs.DoubleListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.DoubleListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.DecimalListPropield), sapfs.DecimalListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.DecimalListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.VeryLongListPropield), sapfs.VeryLongListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.VeryLongListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.VeryUlongListPropield), sapfs.VeryUlongListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.VeryUlongListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.BigIntListPropield), sapfs.BigIntListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.BigIntListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.ComplexListPropield), sapfs.ComplexListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.ComplexListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.DateTimeListPropield), sapfs.DateTimeListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.DateTimeListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.DateOnlyListPropield), sapfs.DateOnlyListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.DateOnlyListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.TimeSpanListPropield), sapfs.TimeSpanListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.TimeSpanListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.TimeOnlyListPropield), sapfs.TimeOnlyListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.TimeOnlyListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.RuneListPropield), sapfs.RuneListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs
                                                                             .RuneListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.GuidListPropield), sapfs.GuidListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs
                                                                             .GuidListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.IpNetworkListPropield), sapfs.IpNetworkListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.IpNetworkListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullByteListPropield), sapfs.NullByteListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.NullByteListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullSByteListPropield), sapfs.NullSByteListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullSByteListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullCharListPropield), sapfs.NullCharListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.NullCharListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullShortListPropield), sapfs.NullShortListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullShortListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullUShortListPropield), sapfs.NullUShortListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullUShortListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullHalfListPropield), sapfs.NullHalfListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.NullHalfListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullIntListPropield), sapfs.NullIntListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.NullIntListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullUIntListPropield), sapfs.NullUIntListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.NullUIntListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullFloatListPropield), sapfs.NullFloatListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullFloatListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullLongListPropield), sapfs.NullLongListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.NullLongListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullULongListPropield), sapfs.NullULongListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullULongListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullDoubleListPropield), sapfs.NullDoubleListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullDoubleListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullDecimalListPropield), sapfs.NullDecimalListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullDecimalListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullVeryLongListPropield), sapfs.NullVeryLongListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullVeryLongListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullVeryUlongListPropield), sapfs.NullVeryUlongListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullVeryUlongListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullBigIntListPropield), sapfs.NullBigIntListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullBigIntListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullComplexListPropield), sapfs.NullComplexListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullComplexListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullDateTimeListPropield), sapfs.NullDateTimeListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullDateTimeListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullDateOnlyListPropield), sapfs.NullDateOnlyListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullDateOnlyListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullTimeSpanListPropield), sapfs.NullTimeSpanListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullTimeSpanListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullTimeOnlyListPropield), sapfs.NullTimeOnlyListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullTimeOnlyListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullRuneListPropield), sapfs.NullRuneListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.NullRuneListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullGuidListPropield), sapfs.NullGuidListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.NullGuidListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullIpNetworkListPropield), sapfs.NullIpNetworkListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullIpNetworkListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.StringListPropield), sapfs.StringListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.StringListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.StringBuilderListPropield), sapfs.StringBuilderListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.StringBuilderListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilterCharSeq(nameof(sapfs.CharSequenceListPropield), sapfs.CharSequenceListPropield
                                                                          , FilterRegistry
                                                                            .OrderedCollectionFilterDefault(sapfs.CharSequenceListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.VersionListPropield), sapfs.VersionListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.VersionListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.IntPtrListPropield), sapfs.IntPtrListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault
                                                                        (sapfs.IntPtrListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.UriListPropield), sapfs.UriListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs
                                                                                                        .UriListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.SpanFormattableListPropield), sapfs.SpanFormattableListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.SpanFormattableListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NdLNfEnumListPropield), sapfs.NdLNfEnumListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NdLNfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NdUNfEnumListPropield), sapfs.NdUNfEnumListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NdUNfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NdLWfEnumListPropield), sapfs.NdLWfEnumListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NdLWfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NdUWfEnumListPropield), sapfs.NdUWfEnumListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NdUWfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.WdLNfEnumListPropield), sapfs.WdLNfEnumListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.WdLNfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.WdUNfEnumListPropield), sapfs.WdUNfEnumListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.WdUNfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.WdLWfEnumListPropield), sapfs.WdLWfEnumListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.WdLWfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.WdUWfEnumListPropield), sapfs.WdUWfEnumListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.WdUWfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullNdLNfEnumListPropield), sapfs.NullNdLNfEnumListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullNdLNfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullNdUNfEnumListPropield), sapfs.NullNdUNfEnumListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullNdUNfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullNdLWfEnumListPropield), sapfs.NullNdLWfEnumListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullNdLWfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullNdUWfEnumListPropield), sapfs.NullNdUWfEnumListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullNdUWfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullWdLNfEnumListPropield), sapfs.NullWdLNfEnumListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullWdLNfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullWdUNfEnumListPropield), sapfs.NullWdUNfEnumListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullWdUNfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullWdLWfEnumListPropield), sapfs.NullWdLWfEnumListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullWdLWfEnumListPropield).CheckPredicate);
                    ctb.CollectionField.WhenPopulatedWithFilter(nameof(sapfs.NullWdUWfEnumListPropield), sapfs.NullWdUWfEnumListPropield, FilterRegistry
                                                                    .OrderedCollectionFilterDefault(sapfs.NullWdUWfEnumListPropield).CheckPredicate);

                    return ctb.Complete();
                };
        }
    }
}
