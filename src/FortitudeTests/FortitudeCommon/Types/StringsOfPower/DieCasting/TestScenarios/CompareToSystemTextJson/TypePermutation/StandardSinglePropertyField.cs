using System.Net;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PreferConcreteValueOverDefault
// A Single Property or Field becomes SingPropield

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Scenarios.CompareToSystemTextJson.TypePermutation;

public class StandardSinglePropertyFieldClass : IStringBearer
{
    public StandardSinglePropertyFieldClass()
    {
        InitializeAllSet();
    }

    public void InitializeAllSet()
    {
        ByteSingPropield      = byte.MaxValue;
        SByteSingPropield     = sbyte.MaxValue;
        CharSingPropield      = 'U';
        ShortSingPropield     = short.MinValue;
        UShortSingPropield    = ushort.MaxValue;
        HalfSetSingPropield   = Half.E;
        HalfNanSingPropield   = Half.NaN;
        IntSingPropield       = int.MinValue;
        UIntSingPropield      = uint.MaxValue;
        FloatSetSingPropield  = float.Pi;
        FloatNanSingPropield  = float.NaN;
        LongSingPropield      = long.MinValue;
        ULongSingPropield     = ulong.MaxValue;
        DoubleSetSingPropield = double.E;
        DoubleNanSingPropield = double.NaN;
        DecimalSingPropield   = decimal.MaxValue;
        VeryLongSingPropield  = Int128.MinValue;
        VeryUlongSingPropield = UInt128.MaxValue;
        BigIntSingPropield    = new BigInteger(decimal.MaxValue) * new BigInteger(100);

        ComplexSingPropield   = Complex.ImaginaryOne + 1;
        DateTimeSingPropield  = new(2009, 11, 12, 19, 49, 0, DateTimeKind.Utc);
        DateOnlySingPropield  = new(1981, 6, 16);
        TimeSpanSingPropield  = new TimeSpan(1, 2, 3, 4, 5).Add(TimeSpan.FromMicroseconds(6));
        TimeOnlySingPropield  = new(23, 59, 59, 999);
        RuneSingPropield      = Rune.GetRuneAt("𝄞", 0);
        GuidSingPropield      = Guid.ParseExact("BEEFCA4E-BEEF-CA4E-BEEF-C0FFEEBABE51", "D");
        IpNetworkSingPropield = new IPNetwork(IPAddress.Loopback, 32);

        NullByteSingPropield      = byte.MaxValue;
        NullSByteSingPropield     = sbyte.MaxValue;
        NullCharSingPropield      = 'U';
        NullShortSingPropield     = short.MinValue;
        NullUShortSingPropield    = ushort.MaxValue;
        NullHalfSingPropield      = Half.E;
        NullIntSingPropield       = int.MinValue;
        NullUIntSingPropield      = uint.MaxValue;
        NullFloatSingPropield     = float.Pi;
        NullLongSingPropield      = long.MinValue;
        NullULongSingPropield     = ulong.MaxValue;
        NullDoubleSingPropield    = double.E;
        NullDecimalSingPropield   = decimal.MaxValue;
        NullVeryLongSingPropield  = Int128.MinValue;
        NullVeryUlongSingPropield = UInt128.MaxValue;
        NullBigIntSingPropield    = new BigInteger(decimal.MaxValue) * new BigInteger(100);

        NullComplexSetSingPropield   = Complex.ImaginaryOne + 1;
        NullComplexUnsetSingPropield = Complex.ImaginaryOne + 1;
        NullDateTimeSingPropield     = new(2009, 11, 12, 19, 49, 0, DateTimeKind.Utc);
        NullDateOnlySingPropield     = new(1981, 6, 16);
        NullTimeSpanSingPropield     = new TimeSpan(1, 2, 3, 4, 5).Add(TimeSpan.FromMicroseconds(6));
        NullTimeOnlySingPropield     = new(23, 59, 59, 999);
        NullRuneSingPropield         = Rune.GetRuneAt("𝄞", 0);
        NullGuidSingPropield         = Guid.ParseExact("BEEFCA4E-BEEF-CA4E-BEEF-C0FFEEBABE51", "D");
        NullIpNetworkSingPropield    = new IPNetwork(IPAddress.Loopback, 32);

        StringSingPropield        = "stringSingPropield";
        StringBuilderSingPropield = new("stringBuilderSingPropield");
        var ms = new MutableString("charSequenceSingPropield");
        for (var i = 128; i < 256; i++)
        {
            ms.Append((char)i);
        }
        CharSequenceSingPropield  = ms;

        VersionSingPropield = Version.Parse("2.0.1");
        IpAddressSingPropield  = IPAddress.Parse("192.168.0.1");
        UriSingPropield     = new Uri("https://github.com/shwaindog/Fortitude");

        SpanFormattableSingPropield = new MySpanFormattableClass("SpanFormattableSingPropield", true);
        NdLNfEnumSingPropield       = NoDefaultLongNoFlagsEnum.NDLNFE_16;
        NdUNfEnumSingPropield       = NoDefaultULongNoFlagsEnum.NDUNFE_3;
        NdLWfEnumSingPropield =
            NoDefaultLongWithFlagsEnum.NDLWFE_2 | NoDefaultLongWithFlagsEnum.NDLWFE_15 | NoDefaultLongWithFlagsEnum.NDLWFE_23 |
            NoDefaultLongWithFlagsEnum.NDLWFE_33;
        NdUWfEnumSingPropield
            = NoDefaultULongWithFlagsEnum.NDUWFE_3 | NoDefaultULongWithFlagsEnum.NDUWFE_15 | NoDefaultULongWithFlagsEnum.NDUWFE_8 |
              NoDefaultULongWithFlagsEnum.NDUWFE_34;

        WdLNfEnumSingPropield = WithDefaultLongNoFlagsEnum.WDLNFE_33;
        WdUNfEnumSingPropield = WithDefaultULongNoFlagsEnum.WDUNFE_34;
        WdLWfEnumSingPropield =
            WithDefaultLongWithFlagsEnum.WDLWFE_4 | WithDefaultLongWithFlagsEnum.WDLWFE_6 |
            WithDefaultLongWithFlagsEnum.WDLWFE_11 | WithDefaultLongWithFlagsEnum.WDLWFE_13 |
            WithDefaultLongWithFlagsEnum.WDLWFE_29;
        WdUWfEnumSingPropield =
            WithDefaultULongWithFlagsEnum.WDUWFE_3 | WithDefaultULongWithFlagsEnum.WDUWFE_15 |
            WithDefaultULongWithFlagsEnum.WDUWFE_8 | WithDefaultULongWithFlagsEnum.WDUWFE_34;

        NullNdLNfEnumSingPropield = NoDefaultLongNoFlagsEnum.NDLNFE_16;
        NullNdUNfEnumSingPropield = NoDefaultULongNoFlagsEnum.NDUNFE_3;
        NullNdLWfEnumSingPropield =
            NoDefaultLongWithFlagsEnum.NDLWFE_2 | NoDefaultLongWithFlagsEnum.NDLWFE_15 | NoDefaultLongWithFlagsEnum.NDLWFE_23 |
            NoDefaultLongWithFlagsEnum.NDLWFE_33;
        NullNdUWfEnumSingPropield
            = NoDefaultULongWithFlagsEnum.NDUWFE_3 | NoDefaultULongWithFlagsEnum.NDUWFE_15 | NoDefaultULongWithFlagsEnum.NDUWFE_8 |
              NoDefaultULongWithFlagsEnum.NDUWFE_34;

        NullWdLNfEnumSingPropield = WithDefaultLongNoFlagsEnum.WDLNFE_33;
        NullWdUNfEnumSingPropield = WithDefaultULongNoFlagsEnum.WDUNFE_34;
        NullWdLWfEnumSingPropield =
            WithDefaultLongWithFlagsEnum.WDLWFE_4 | WithDefaultLongWithFlagsEnum.WDLWFE_6 |
            WithDefaultLongWithFlagsEnum.WDLWFE_11 | WithDefaultLongWithFlagsEnum.WDLWFE_13 |
            WithDefaultLongWithFlagsEnum.WDLWFE_29;
        NullWdUWfEnumSingPropield =
            WithDefaultULongWithFlagsEnum.WDUWFE_3 | WithDefaultULongWithFlagsEnum.WDUWFE_15 |
            WithDefaultULongWithFlagsEnum.WDUWFE_8 | WithDefaultULongWithFlagsEnum.WDUWFE_34;
    }

    public void InitializeAllDefault()
    {
        ByteSingPropield      = default;
        SByteSingPropield     = default;
        CharSingPropield      = default;
        ShortSingPropield     = default;
        UShortSingPropield    = default;
        HalfSetSingPropield   = default;
        HalfNanSingPropield   = default;
        IntSingPropield       = default;
        UIntSingPropield      = default;
        FloatSetSingPropield  = default;
        FloatNanSingPropield  = default;
        LongSingPropield      = default;
        ULongSingPropield     = default;
        DoubleSetSingPropield = default;
        DoubleNanSingPropield = default;
        DecimalSingPropield   = default;
        VeryLongSingPropield  = default;
        VeryUlongSingPropield = default;
        BigIntSingPropield    = default;

        ComplexSingPropield   = default;
        DateTimeSingPropield  = default;
        DateOnlySingPropield  = default;
        TimeSpanSingPropield  = default;
        TimeOnlySingPropield  = default;
        RuneSingPropield      = default;
        GuidSingPropield      = default;
        IpNetworkSingPropield = default;

        NullByteSingPropield      = default;
        NullSByteSingPropield     = default;
        NullCharSingPropield      = default;
        NullShortSingPropield     = default;
        NullUShortSingPropield    = default;
        NullHalfSingPropield      = default;
        NullIntSingPropield       = default;
        NullUIntSingPropield      = default;
        NullFloatSingPropield     = default;
        NullLongSingPropield      = default;
        NullULongSingPropield     = default;
        NullDoubleSingPropield    = default;
        NullDecimalSingPropield   = default;
        NullVeryLongSingPropield  = default;
        NullVeryUlongSingPropield = default;
        NullBigIntSingPropield    = default;

        NullComplexSetSingPropield   = default;
        NullComplexUnsetSingPropield = default;
        NullDateTimeSingPropield     = default;
        NullDateOnlySingPropield     = default;
        NullTimeSpanSingPropield     = default;
        NullTimeOnlySingPropield     = default;
        NullRuneSingPropield         = default;
        NullGuidSingPropield         = default;
        NullIpNetworkSingPropield    = default;

        StringSingPropield        = null!;
        StringBuilderSingPropield = null!;
        CharSequenceSingPropield  = null!;

        VersionSingPropield = null!;
        IpAddressSingPropield  = null!;
        UriSingPropield     = null!;

        SpanFormattableSingPropield = null!;

        NdLNfEnumSingPropield = default;
        NdUNfEnumSingPropield = default;
        NdLWfEnumSingPropield = default;
        NdUWfEnumSingPropield = default;

        WdLNfEnumSingPropield = default;
        WdUNfEnumSingPropield = default;
        WdLWfEnumSingPropield = default;
        WdUWfEnumSingPropield = default;

        NullNdLNfEnumSingPropield = default;
        NullNdUNfEnumSingPropield = default;
        NullNdLWfEnumSingPropield = default;
        NullNdUWfEnumSingPropield = default;

        NullWdLNfEnumSingPropield = default;
        NullWdUNfEnumSingPropield = default;
        NullWdLWfEnumSingPropield = default;
        NullWdUWfEnumSingPropield = default;
    }

    public byte ByteSingPropield { get; set; }
    public sbyte SByteSingPropield;
    public char CharSingPropield { get; set; }
    public short ShortSingPropield;
    public ushort UShortSingPropield { get; set; }
    public Half HalfSetSingPropield;
    public Half HalfNanSingPropield { get; set; }
    public int IntSingPropield;
    public uint UIntSingPropield { get; set; }
    public float FloatSetSingPropield;
    public float FloatNanSingPropield { get; set; }
    public long LongSingPropield;
    public ulong ULongSingPropield { get; set; }
    public double DoubleSetSingPropield;
    public double DoubleNanSingPropield { get; set; }
    public decimal DecimalSingPropield;
    public Int128 VeryLongSingPropield { get; set; }
    public UInt128 VeryUlongSingPropield;
    public BigInteger BigIntSingPropield { get; set; }

    public Complex ComplexSingPropield { get; set; }
    public DateTime DateTimeSingPropield;
    public DateOnly DateOnlySingPropield { get; set; }
    public TimeSpan TimeSpanSingPropield;
    public TimeOnly TimeOnlySingPropield { get; set; }
    public Rune RuneSingPropield;
    public Guid GuidSingPropield { get; set; }
    public IPNetwork IpNetworkSingPropield;

    public byte? NullByteSingPropield;
    public sbyte? NullSByteSingPropield { get; set; }
    public char? NullCharSingPropield;
    public short? NullShortSingPropield { get; set; }
    public ushort? NullUShortSingPropield;
    public Half? NullHalfSingPropield { get; set; }
    public int? NullIntSingPropield;
    public uint? NullUIntSingPropield { get; set; }
    public float? NullFloatSingPropield;
    public long? NullLongSingPropield { get; set; }
    public ulong? NullULongSingPropield;
    public double? NullDoubleSingPropield { get; set; }
    public decimal? NullDecimalSingPropield;
    public Int128? NullVeryLongSingPropield { get; set; }
    public UInt128? NullVeryUlongSingPropield;
    public BigInteger? NullBigIntSingPropield { get; set; }

    public Complex? NullComplexSetSingPropield;
    public Complex? NullComplexUnsetSingPropield { get; set; }
    public DateTime? NullDateTimeSingPropield;
    public DateOnly? NullDateOnlySingPropield { get; set; }
    public TimeSpan? NullTimeSpanSingPropield;
    public TimeOnly? NullTimeOnlySingPropield { get; set; }
    public Rune? NullRuneSingPropield;
    public Guid? NullGuidSingPropield { get; set; }
    public IPNetwork? NullIpNetworkSingPropield;

    public string StringSingPropield { get; set; } = null!;
    public StringBuilder StringBuilderSingPropield = null!;
    public ICharSequence CharSequenceSingPropield { get; set; } = null!;

    public Version VersionSingPropield = null!;
    public IPAddress IpAddressSingPropield { get; set; } = null!;
    public Uri UriSingPropield = null!;

    public MySpanFormattableClass SpanFormattableSingPropield { get; set; } = null!;

    public NoDefaultLongNoFlagsEnum NdLNfEnumSingPropield;
    public NoDefaultULongNoFlagsEnum NdUNfEnumSingPropield { get; set; }
    public NoDefaultLongWithFlagsEnum NdLWfEnumSingPropield;
    public NoDefaultULongWithFlagsEnum NdUWfEnumSingPropield { get; set; }

    public WithDefaultLongNoFlagsEnum WdLNfEnumSingPropield { get; set; }
    public WithDefaultULongNoFlagsEnum WdUNfEnumSingPropield;
    public WithDefaultLongWithFlagsEnum WdLWfEnumSingPropield { get; set; }
    public WithDefaultULongWithFlagsEnum WdUWfEnumSingPropield;

    public NoDefaultLongNoFlagsEnum? NullNdLNfEnumSingPropield;
    public NoDefaultULongNoFlagsEnum? NullNdUNfEnumSingPropield { get; set; }
    public NoDefaultLongWithFlagsEnum? NullNdLWfEnumSingPropield;
    public NoDefaultULongWithFlagsEnum? NullNdUWfEnumSingPropield { get; set; }

    public WithDefaultLongNoFlagsEnum? NullWdLNfEnumSingPropield { get; set; }
    public WithDefaultULongNoFlagsEnum? NullWdUNfEnumSingPropield;
    public WithDefaultLongWithFlagsEnum? NullWdLWfEnumSingPropield { get; set; }
    public WithDefaultULongWithFlagsEnum? NullWdUWfEnumSingPropield;

    [JsonIgnore] public TestFieldRevealMode TestFieldRevealMode { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos)
    {
        switch (TestFieldRevealMode)
        {
            case TestFieldRevealMode.WhenNonDefault:          return WhenNonDefaultReveal(tos);
            case TestFieldRevealMode.WhenNonNull:             return WhenNonNullReveal(tos);
            case TestFieldRevealMode.WhenNonNullOrNonDefault: return WhenNonNullOrDefaultReveal(tos);
            case TestFieldRevealMode.AlwaysAll:
            default:
                return AlwaysRevealAll(tos);
        }
    }

    public StateExtractStringRange AlwaysRevealAll(ITheOneString tos)
    {
        using var ctb =
            tos.StartComplexType(this);
        ctb.Field.AlwaysAdd(nameof(ByteSingPropield), ByteSingPropield);
        ctb.Field.AlwaysAdd(nameof(CharSingPropield), CharSingPropield);
        ctb.Field.AlwaysAdd(nameof(UShortSingPropield), UShortSingPropield);
        ctb.Field.AlwaysAdd(nameof(HalfNanSingPropield), HalfNanSingPropield, formatFlags: EnsureFormattedDelimited);
        ctb.Field.AlwaysAdd(nameof(UIntSingPropield), UIntSingPropield);
        ctb.Field.AlwaysAdd(nameof(FloatNanSingPropield), FloatNanSingPropield);
        ctb.Field.AlwaysAdd(nameof(ULongSingPropield), ULongSingPropield);
        ctb.Field.AlwaysAdd(nameof(DoubleNanSingPropield), DoubleNanSingPropield);
        ctb.Field.AlwaysAdd(nameof(VeryLongSingPropield), VeryLongSingPropield);
        ctb.Field.AlwaysAdd(nameof(BigIntSingPropield), BigIntSingPropield);
        ctb.Field.AlwaysAdd(nameof(ComplexSingPropield), ComplexSingPropield);
        ctb.Field.AlwaysAdd(nameof(DateOnlySingPropield), DateOnlySingPropield);
        ctb.Field.AlwaysAdd(nameof(TimeOnlySingPropield), TimeOnlySingPropield);
        ctb.Field.AlwaysAdd(nameof(GuidSingPropield), GuidSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullSByteSingPropield), NullSByteSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullShortSingPropield), NullShortSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullHalfSingPropield), NullHalfSingPropield, formatFlags: EnsureFormattedDelimited);
        ctb.Field.AlwaysAdd(nameof(NullUIntSingPropield), NullUIntSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullLongSingPropield), NullLongSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullDoubleSingPropield), NullDoubleSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullVeryLongSingPropield), NullVeryLongSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullBigIntSingPropield), NullBigIntSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullComplexUnsetSingPropield), NullComplexUnsetSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullDateOnlySingPropield), NullDateOnlySingPropield);
        ctb.Field.AlwaysAdd(nameof(NullTimeOnlySingPropield), NullTimeOnlySingPropield);
        ctb.Field.AlwaysAdd(nameof(NullGuidSingPropield), NullGuidSingPropield);
        ctb.Field.AlwaysAdd(nameof(StringSingPropield), StringSingPropield);
        ctb.Field.AlwaysAddCharSeq(nameof(CharSequenceSingPropield), CharSequenceSingPropield, formatFlags: AsCollection);
        ctb.Field.AlwaysAdd(nameof(IpAddressSingPropield), IpAddressSingPropield);
        ctb.Field.AlwaysAdd(nameof(SpanFormattableSingPropield), SpanFormattableSingPropield);
        ctb.Field.AlwaysAdd(nameof(NdUNfEnumSingPropield), NdUNfEnumSingPropield);
        ctb.Field.AlwaysAdd(nameof(NdUWfEnumSingPropield), NdUWfEnumSingPropield);
        ctb.Field.AlwaysAdd(nameof(WdLNfEnumSingPropield), WdLNfEnumSingPropield);
        ctb.Field.AlwaysAdd(nameof(WdLWfEnumSingPropield), WdLWfEnumSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullNdUNfEnumSingPropield), NullNdUNfEnumSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullNdUWfEnumSingPropield), NullNdUWfEnumSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullWdLNfEnumSingPropield), NullWdLNfEnumSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullWdLWfEnumSingPropield), NullWdLWfEnumSingPropield);
        ctb.Field.AlwaysAdd(nameof(SByteSingPropield), SByteSingPropield);
        ctb.Field.AlwaysAdd(nameof(ShortSingPropield), ShortSingPropield);
        ctb.Field.AlwaysAdd(nameof(HalfSetSingPropield), HalfSetSingPropield, formatFlags: EnsureFormattedDelimited);
        ctb.Field.AlwaysAdd(nameof(IntSingPropield), IntSingPropield);
        ctb.Field.AlwaysAdd(nameof(FloatSetSingPropield), FloatSetSingPropield);
        ctb.Field.AlwaysAdd(nameof(LongSingPropield), LongSingPropield);
        ctb.Field.AlwaysAdd(nameof(DoubleSetSingPropield), DoubleSetSingPropield);
        ctb.Field.AlwaysAdd(nameof(DecimalSingPropield), DecimalSingPropield);
        ctb.Field.AlwaysAdd(nameof(VeryUlongSingPropield), VeryUlongSingPropield);
        ctb.Field.AlwaysAdd(nameof(DateTimeSingPropield), DateTimeSingPropield);
        ctb.Field.AlwaysAdd(nameof(TimeSpanSingPropield), TimeSpanSingPropield);
        ctb.Field.AlwaysAdd(nameof(RuneSingPropield), RuneSingPropield);
        ctb.Field.AlwaysAdd(nameof(IpNetworkSingPropield), IpNetworkSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullByteSingPropield), NullByteSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullCharSingPropield), NullCharSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullUShortSingPropield), NullUShortSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullIntSingPropield), NullIntSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullFloatSingPropield), NullFloatSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullULongSingPropield), NullULongSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullDecimalSingPropield), NullDecimalSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullVeryUlongSingPropield), NullVeryUlongSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullComplexSetSingPropield), NullComplexSetSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullDateTimeSingPropield), NullDateTimeSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullTimeSpanSingPropield), NullTimeSpanSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullRuneSingPropield), NullRuneSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullIpNetworkSingPropield), NullIpNetworkSingPropield);
        ctb.Field.AlwaysAdd(nameof(StringBuilderSingPropield), StringBuilderSingPropield);
        ctb.Field.AlwaysAdd(nameof(VersionSingPropield), VersionSingPropield);
        ctb.Field.AlwaysAdd(nameof(UriSingPropield), UriSingPropield);
        ctb.Field.AlwaysAdd(nameof(NdLNfEnumSingPropield), NdLNfEnumSingPropield);
        ctb.Field.AlwaysAdd(nameof(NdLWfEnumSingPropield), NdLWfEnumSingPropield);
        ctb.Field.AlwaysAdd(nameof(WdUNfEnumSingPropield), WdUNfEnumSingPropield);
        ctb.Field.AlwaysAdd(nameof(WdUWfEnumSingPropield), WdUWfEnumSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullNdLNfEnumSingPropield), NullNdLNfEnumSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullNdLWfEnumSingPropield), NullNdLWfEnumSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullWdUNfEnumSingPropield), NullWdUNfEnumSingPropield);
        ctb.Field.AlwaysAdd(nameof(NullWdUWfEnumSingPropield), NullWdUWfEnumSingPropield);
        return ctb.Complete();
    }

    public StateExtractStringRange WhenNonDefaultReveal(ITheOneString tos)
    {
        using var ctb =
            tos.StartComplexType(this);
        ctb.Field.WhenNonDefaultAdd(nameof(ByteSingPropield), ByteSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(CharSingPropield), CharSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(UShortSingPropield), UShortSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(HalfNanSingPropield), HalfNanSingPropield, formatFlags: EnsureFormattedDelimited);
        ctb.Field.WhenNonDefaultAdd(nameof(UIntSingPropield), UIntSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(FloatNanSingPropield), FloatNanSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(ULongSingPropield), ULongSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(DoubleNanSingPropield), DoubleNanSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(VeryLongSingPropield), VeryLongSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(BigIntSingPropield), BigIntSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(ComplexSingPropield), ComplexSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(DateOnlySingPropield), DateOnlySingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(TimeOnlySingPropield), TimeOnlySingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(GuidSingPropield), GuidSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullSByteSingPropield), NullSByteSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullShortSingPropield), NullShortSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullHalfSingPropield), NullHalfSingPropield, formatFlags: EnsureFormattedDelimited);
        ctb.Field.WhenNonDefaultAdd(nameof(NullUIntSingPropield), NullUIntSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullLongSingPropield), NullLongSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullDoubleSingPropield), NullDoubleSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullVeryLongSingPropield), NullVeryLongSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullBigIntSingPropield), NullBigIntSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullComplexUnsetSingPropield), NullComplexUnsetSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullDateOnlySingPropield), NullDateOnlySingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullTimeOnlySingPropield), NullTimeOnlySingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullGuidSingPropield), NullGuidSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(StringSingPropield), StringSingPropield);
        ctb.Field.WhenNonDefaultAddCharSeq(nameof(CharSequenceSingPropield), CharSequenceSingPropield, formatFlags: AsCollection);
        ctb.Field.WhenNonDefaultAdd(nameof(IpAddressSingPropield), IpAddressSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(SpanFormattableSingPropield), SpanFormattableSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NdUNfEnumSingPropield), NdUNfEnumSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NdUWfEnumSingPropield), NdUWfEnumSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(WdLNfEnumSingPropield), WdLNfEnumSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(WdLWfEnumSingPropield), WdLWfEnumSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullNdUNfEnumSingPropield), NullNdUNfEnumSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullNdUWfEnumSingPropield), NullNdUWfEnumSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullWdLNfEnumSingPropield), NullWdLNfEnumSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullWdLWfEnumSingPropield), NullWdLWfEnumSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(SByteSingPropield), SByteSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(ShortSingPropield), ShortSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(HalfSetSingPropield), HalfSetSingPropield, formatFlags: EnsureFormattedDelimited);
        ctb.Field.WhenNonDefaultAdd(nameof(IntSingPropield), IntSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(FloatSetSingPropield), FloatSetSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(LongSingPropield), LongSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(DoubleSetSingPropield), DoubleSetSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(DecimalSingPropield), DecimalSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(VeryUlongSingPropield), VeryUlongSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(DateTimeSingPropield), DateTimeSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(TimeSpanSingPropield), TimeSpanSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(RuneSingPropield), RuneSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(IpNetworkSingPropield), IpNetworkSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullByteSingPropield), NullByteSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullCharSingPropield), NullCharSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullUShortSingPropield), NullUShortSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullIntSingPropield), NullIntSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullFloatSingPropield), NullFloatSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullULongSingPropield), NullULongSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullDecimalSingPropield), NullDecimalSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullVeryUlongSingPropield), NullVeryUlongSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullComplexSetSingPropield), NullComplexSetSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullDateTimeSingPropield), NullDateTimeSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullTimeSpanSingPropield), NullTimeSpanSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullRuneSingPropield), NullRuneSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullIpNetworkSingPropield), NullIpNetworkSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(StringBuilderSingPropield), StringBuilderSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(VersionSingPropield), VersionSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(UriSingPropield), UriSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NdLNfEnumSingPropield), NdLNfEnumSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NdLWfEnumSingPropield), NdLWfEnumSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(WdUNfEnumSingPropield), WdUNfEnumSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(WdUWfEnumSingPropield), WdUWfEnumSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullNdLNfEnumSingPropield), NullNdLNfEnumSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullNdLWfEnumSingPropield), NullNdLWfEnumSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullWdUNfEnumSingPropield), NullWdUNfEnumSingPropield);
        ctb.Field.WhenNonDefaultAdd(nameof(NullWdUWfEnumSingPropield), NullWdUWfEnumSingPropield);
        return ctb.Complete();
    }

    public StateExtractStringRange WhenNonNullReveal(ITheOneString tos)
    {
        using var ctb =
            tos.StartComplexType(this);
        ctb.Field.WhenNonNullAdd(nameof(ByteSingPropield), ByteSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(CharSingPropield), CharSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(UShortSingPropield), UShortSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(HalfNanSingPropield), HalfNanSingPropield, formatFlags: EnsureFormattedDelimited);
        ctb.Field.WhenNonNullAdd(nameof(UIntSingPropield), UIntSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(FloatNanSingPropield), FloatNanSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(ULongSingPropield), ULongSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(DoubleNanSingPropield), DoubleNanSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(VeryLongSingPropield), VeryLongSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(BigIntSingPropield), BigIntSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(ComplexSingPropield), ComplexSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(DateOnlySingPropield), DateOnlySingPropield);
        ctb.Field.WhenNonNullAdd(nameof(TimeOnlySingPropield), TimeOnlySingPropield);
        ctb.Field.WhenNonNullAdd(nameof(GuidSingPropield), GuidSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullSByteSingPropield), NullSByteSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullShortSingPropield), NullShortSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullHalfSingPropield), NullHalfSingPropield, formatFlags: EnsureFormattedDelimited);
        ctb.Field.WhenNonNullAdd(nameof(NullUIntSingPropield), NullUIntSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullLongSingPropield), NullLongSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullDoubleSingPropield), NullDoubleSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullVeryLongSingPropield), NullVeryLongSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullBigIntSingPropield), NullBigIntSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullComplexUnsetSingPropield), NullComplexUnsetSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullDateOnlySingPropield), NullDateOnlySingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullTimeOnlySingPropield), NullTimeOnlySingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullGuidSingPropield), NullGuidSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(StringSingPropield), StringSingPropield);
        ctb.Field.WhenNonNullAddCharSeq(nameof(CharSequenceSingPropield), CharSequenceSingPropield, formatFlags: AsCollection);
        ctb.Field.WhenNonNullAdd(nameof(IpAddressSingPropield), IpAddressSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(SpanFormattableSingPropield), SpanFormattableSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NdUNfEnumSingPropield), NdUNfEnumSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NdUWfEnumSingPropield), NdUWfEnumSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(WdLNfEnumSingPropield), WdLNfEnumSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(WdLWfEnumSingPropield), WdLWfEnumSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullNdUNfEnumSingPropield), NullNdUNfEnumSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullNdUWfEnumSingPropield), NullNdUWfEnumSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullWdLNfEnumSingPropield), NullWdLNfEnumSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullWdLWfEnumSingPropield), NullWdLWfEnumSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(SByteSingPropield), SByteSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(ShortSingPropield), ShortSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(HalfSetSingPropield), HalfSetSingPropield, formatFlags: EnsureFormattedDelimited);
        ctb.Field.WhenNonNullAdd(nameof(IntSingPropield), IntSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(FloatSetSingPropield), FloatSetSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(LongSingPropield), LongSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(DoubleSetSingPropield), DoubleSetSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(DecimalSingPropield), DecimalSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(VeryUlongSingPropield), VeryUlongSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(DateTimeSingPropield), DateTimeSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(TimeSpanSingPropield), TimeSpanSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(RuneSingPropield), RuneSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(IpNetworkSingPropield), IpNetworkSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullByteSingPropield), NullByteSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullCharSingPropield), NullCharSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullUShortSingPropield), NullUShortSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullIntSingPropield), NullIntSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullFloatSingPropield), NullFloatSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullULongSingPropield), NullULongSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullDecimalSingPropield), NullDecimalSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullVeryUlongSingPropield), NullVeryUlongSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullComplexSetSingPropield), NullComplexSetSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullDateTimeSingPropield), NullDateTimeSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullTimeSpanSingPropield), NullTimeSpanSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullRuneSingPropield), NullRuneSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullIpNetworkSingPropield), NullIpNetworkSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(StringBuilderSingPropield), StringBuilderSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(VersionSingPropield), VersionSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(UriSingPropield), UriSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NdLNfEnumSingPropield), NdLNfEnumSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NdLWfEnumSingPropield), NdLWfEnumSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(WdUNfEnumSingPropield), WdUNfEnumSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(WdUWfEnumSingPropield), WdUWfEnumSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullNdLNfEnumSingPropield), NullNdLNfEnumSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullNdLWfEnumSingPropield), NullNdLWfEnumSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullWdUNfEnumSingPropield), NullWdUNfEnumSingPropield);
        ctb.Field.WhenNonNullAdd(nameof(NullWdUWfEnumSingPropield), NullWdUWfEnumSingPropield);
        return ctb.Complete();
    }

    public StateExtractStringRange WhenNonNullOrDefaultReveal(ITheOneString tos)
    {
        using var ctb =
            tos.StartComplexType(this);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(ByteSingPropield), ByteSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(SByteSingPropield), SByteSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(CharSingPropield), CharSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(ShortSingPropield), ShortSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(UShortSingPropield), UShortSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(HalfSetSingPropield), HalfSetSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(HalfNanSingPropield), HalfNanSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(IntSingPropield), IntSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(UIntSingPropield), UIntSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(FloatSetSingPropield), FloatSetSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(FloatNanSingPropield), FloatNanSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(LongSingPropield), LongSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(ULongSingPropield), ULongSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(DoubleSetSingPropield), DoubleSetSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(DoubleNanSingPropield), DoubleNanSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(DecimalSingPropield), DecimalSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(VeryLongSingPropield), VeryLongSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(VeryUlongSingPropield), VeryUlongSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(BigIntSingPropield), BigIntSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(ComplexSingPropield), ComplexSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(DateTimeSingPropield), DateTimeSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(DateOnlySingPropield), DateOnlySingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(TimeSpanSingPropield), TimeSpanSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(TimeOnlySingPropield), TimeOnlySingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(RuneSingPropield), RuneSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(GuidSingPropield), GuidSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(IpNetworkSingPropield), IpNetworkSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullByteSingPropield), NullByteSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullSByteSingPropield), NullSByteSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullCharSingPropield), NullCharSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullShortSingPropield), NullShortSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullUShortSingPropield), NullUShortSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullHalfSingPropield), NullHalfSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullIntSingPropield), NullIntSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullUIntSingPropield), NullUIntSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullFloatSingPropield), NullFloatSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullLongSingPropield), NullLongSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullULongSingPropield), NullULongSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullDoubleSingPropield), NullDoubleSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullDecimalSingPropield), NullDecimalSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullVeryLongSingPropield), NullVeryLongSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullVeryUlongSingPropield), NullVeryUlongSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullBigIntSingPropield), NullBigIntSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullComplexSetSingPropield), NullComplexSetSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullComplexUnsetSingPropield), NullComplexUnsetSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullDateTimeSingPropield), NullDateTimeSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullDateOnlySingPropield), NullDateOnlySingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullTimeSpanSingPropield), NullTimeSpanSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullTimeOnlySingPropield), NullTimeOnlySingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullRuneSingPropield), NullRuneSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullGuidSingPropield), NullGuidSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullIpNetworkSingPropield), NullIpNetworkSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(StringSingPropield), StringSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(StringBuilderSingPropield), StringBuilderSingPropield);
        ctb.Field.WhenNonNullOrDefaultAddCharSeq(nameof(CharSequenceSingPropield), CharSequenceSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(VersionSingPropield), VersionSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(IpAddressSingPropield), IpAddressSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(UriSingPropield), UriSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(SpanFormattableSingPropield), SpanFormattableSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NdLNfEnumSingPropield), NdLNfEnumSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NdUNfEnumSingPropield), NdUNfEnumSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NdLWfEnumSingPropield), NdLWfEnumSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NdUWfEnumSingPropield), NdUWfEnumSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(WdLNfEnumSingPropield), WdLNfEnumSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(WdUNfEnumSingPropield), WdUNfEnumSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(WdLWfEnumSingPropield), WdLWfEnumSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(WdUWfEnumSingPropield), WdUWfEnumSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullNdLNfEnumSingPropield), NullNdLNfEnumSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullNdUNfEnumSingPropield), NullNdUNfEnumSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullNdLWfEnumSingPropield), NullNdLWfEnumSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullNdUWfEnumSingPropield), NullNdUWfEnumSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullWdLNfEnumSingPropield), NullWdLNfEnumSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullWdUNfEnumSingPropield), NullWdUNfEnumSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullWdLWfEnumSingPropield), NullWdLWfEnumSingPropield);
        ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullWdUWfEnumSingPropield), NullWdUWfEnumSingPropield);
        return ctb.Complete();
    }
}

[NoMatchingProductionClass]
public struct StandardSinglePropertyFieldStruct
{
    public StandardSinglePropertyFieldStruct()
    {
        InitializeAllSet();
    }

    public void InitializeAllSet()
    {
        ByteSingPropield      = byte.MaxValue;
        SByteSingPropield     = sbyte.MaxValue;
        CharSingPropield      = 'U';
        ShortSingPropield     = short.MinValue;
        UShortSingPropield    = ushort.MaxValue;
        HalfSetSingPropield   = Half.E;
        HalfNanSingPropield   = Half.NaN;
        IntSingPropield       = int.MinValue;
        UIntSingPropield      = uint.MaxValue;
        FloatSetSingPropield  = float.Pi;
        FloatNanSingPropield  = float.NaN;
        LongSingPropield      = long.MinValue;
        ULongSingPropield     = ulong.MaxValue;
        DoubleSetSingPropield = double.E;
        DoubleNanSingPropield = double.NaN;
        DecimalSingPropield   = decimal.MaxValue;
        VeryLongSingPropield  = Int128.MinValue;
        VeryUlongSingPropield = UInt128.MaxValue;
        BigIntSingPropield    = new BigInteger(decimal.MaxValue) * new BigInteger(100);

        ComplexSingPropield   = Complex.ImaginaryOne + 1;
        DateTimeSingPropield  = new(2009, 11, 12, 19, 49, 0, DateTimeKind.Utc);
        DateOnlySingPropield  = new(1981, 6, 16);
        TimeSpanSingPropield  = new TimeSpan(1, 2, 3, 4, 5).Add(TimeSpan.FromMicroseconds(6));
        TimeOnlySingPropield  = new(23, 59, 59, 999);
        RuneSingPropield      = Rune.GetRuneAt("𝄞", 0);
        GuidSingPropield      = Guid.ParseExact("BEEFCA4E-BEEF-CA4E-BEEF-COFFEEBABE51", "D");
        IpNetworkSingPropield = new IPNetwork(IPAddress.Loopback, 32);

        NullByteSingPropield      = byte.MaxValue;
        NullSByteSingPropield     = sbyte.MaxValue;
        NullCharSingPropield      = 'U';
        NullShortSingPropield     = short.MinValue;
        NullUShortSingPropield    = ushort.MaxValue;
        NullHalfSingPropield      = Half.E;
        NullIntSingPropield       = int.MinValue;
        NullUIntSingPropield      = uint.MaxValue;
        NullFloatSingPropield     = float.Pi;
        NullLongSingPropield      = long.MinValue;
        NullULongSingPropield     = ulong.MaxValue;
        NullDoubleSingPropield    = double.E;
        NullDecimalSingPropield   = decimal.MaxValue;
        NullVeryLongSingPropield  = Int128.MinValue;
        NullVeryUlongSingPropield = UInt128.MaxValue;
        NullBigIntSingPropield    = new BigInteger(decimal.MaxValue) * new BigInteger(100);

        NullComplexSetSingPropield   = Complex.ImaginaryOne + 1;
        NullComplexUnsetSingPropield = Complex.ImaginaryOne + 1;
        NullDateTimeSingPropield     = new(2009, 11, 12, 19, 49, 0, DateTimeKind.Utc);
        NullDateOnlySingPropield     = new(1981, 6, 16);
        NullTimeSpanSingPropield     = new TimeSpan(1, 2, 3, 4, 5).Add(TimeSpan.FromMicroseconds(6));
        NullTimeOnlySingPropield     = new(23, 59, 59, 999);
        NullRuneSingPropield         = Rune.GetRuneAt("𝄞", 0);
        NullGuidSingPropield         = Guid.ParseExact("BEEFCA4E-BEEF-CA4E-BEEF-COFFEEBABE51", "D");
        NullIpNetworkSingPropield    = new IPNetwork(IPAddress.Loopback, 32);

        StringSingPropield        = "stringSingPropield";
        StringBuilderSingPropield = new("stringBuilderSingPropield");
        CharSequenceSingPropield  = new MutableString("charSequenceSingPropield");

        VersionSingPropield = Version.Parse("2.0.1");
        IntPtrSingPropield  = IPAddress.Parse("192.168.0.1");
        UriSingPropield     = new Uri("https://github.com/shwaindog/Fortitude");

        SpanFormattableClassSingPropield = new MySpanFormattableClass("SpanFormattableSingPropield");
        NdLNfEnumSingPropield       = NoDefaultLongNoFlagsEnum.NDLNFE_16;
        NdUNfEnumSingPropield       = NoDefaultULongNoFlagsEnum.NDUNFE_3;
        NdLWfEnumSingPropield =
            NoDefaultLongWithFlagsEnum.NDLWFE_2 | NoDefaultLongWithFlagsEnum.NDLWFE_15 | NoDefaultLongWithFlagsEnum.NDLWFE_23 |
            NoDefaultLongWithFlagsEnum.NDLWFE_33;
        NdUWfEnumSingPropield
            = NoDefaultULongWithFlagsEnum.NDUWFE_3 | NoDefaultULongWithFlagsEnum.NDUWFE_15 | NoDefaultULongWithFlagsEnum.NDUWFE_8 |
              NoDefaultULongWithFlagsEnum.NDUWFE_34;

        WdLNfEnumSingPropield = WithDefaultLongNoFlagsEnum.WDLNFE_33;
        WdUNfEnumSingPropield = WithDefaultULongNoFlagsEnum.WDUNFE_34;
        WdLWfEnumSingPropield =
            WithDefaultLongWithFlagsEnum.WDLWFE_4 | WithDefaultLongWithFlagsEnum.WDLWFE_6 |
            WithDefaultLongWithFlagsEnum.WDLWFE_11 | WithDefaultLongWithFlagsEnum.WDLWFE_13 |
            WithDefaultLongWithFlagsEnum.WDLWFE_29;
        WdUWfEnumSingPropield =
            WithDefaultULongWithFlagsEnum.WDUWFE_3 | WithDefaultULongWithFlagsEnum.WDUWFE_15 |
            WithDefaultULongWithFlagsEnum.WDUWFE_8 | WithDefaultULongWithFlagsEnum.WDUWFE_34;

        NullNdLNfEnumSingPropield = NoDefaultLongNoFlagsEnum.NDLNFE_16;
        NullNdUNfEnumSingPropield = NoDefaultULongNoFlagsEnum.NDUNFE_3;
        NullNdLWfEnumSingPropield =
            NoDefaultLongWithFlagsEnum.NDLWFE_2 | NoDefaultLongWithFlagsEnum.NDLWFE_15 | NoDefaultLongWithFlagsEnum.NDLWFE_23 |
            NoDefaultLongWithFlagsEnum.NDLWFE_33;
        NullNdUWfEnumSingPropield
            = NoDefaultULongWithFlagsEnum.NDUWFE_3 | NoDefaultULongWithFlagsEnum.NDUWFE_15 | NoDefaultULongWithFlagsEnum.NDUWFE_8 |
              NoDefaultULongWithFlagsEnum.NDUWFE_34;

        NullWdLNfEnumSingPropield = WithDefaultLongNoFlagsEnum.WDLNFE_33;
        NullWdUNfEnumSingPropield = WithDefaultULongNoFlagsEnum.WDUNFE_34;
        NullWdLWfEnumSingPropield =
            WithDefaultLongWithFlagsEnum.WDLWFE_4 | WithDefaultLongWithFlagsEnum.WDLWFE_6 |
            WithDefaultLongWithFlagsEnum.WDLWFE_11 | WithDefaultLongWithFlagsEnum.WDLWFE_13 |
            WithDefaultLongWithFlagsEnum.WDLWFE_29;
        NullWdUWfEnumSingPropield =
            WithDefaultULongWithFlagsEnum.WDUWFE_3 | WithDefaultULongWithFlagsEnum.WDUWFE_15 |
            WithDefaultULongWithFlagsEnum.WDUWFE_8 | WithDefaultULongWithFlagsEnum.WDUWFE_34;
    }

    public void InitializeAllDefault()
    {
        ByteSingPropield      = default;
        SByteSingPropield     = default;
        CharSingPropield      = default;
        ShortSingPropield     = default;
        UShortSingPropield    = default;
        HalfSetSingPropield   = default;
        HalfNanSingPropield   = default;
        IntSingPropield       = default;
        UIntSingPropield      = default;
        FloatSetSingPropield  = default;
        FloatNanSingPropield  = default;
        LongSingPropield      = default;
        ULongSingPropield     = default;
        DoubleSetSingPropield = default;
        DoubleNanSingPropield = default;
        DecimalSingPropield   = default;
        VeryLongSingPropield  = default;
        VeryUlongSingPropield = default;
        BigIntSingPropield    = default;

        ComplexSingPropield   = default;
        DateTimeSingPropield  = default;
        DateOnlySingPropield  = default;
        TimeSpanSingPropield  = default;
        TimeOnlySingPropield  = default;
        RuneSingPropield      = default;
        GuidSingPropield      = default;
        IpNetworkSingPropield = default;

        NullByteSingPropield      = default;
        NullSByteSingPropield     = default;
        NullCharSingPropield      = default;
        NullShortSingPropield     = default;
        NullUShortSingPropield    = default;
        NullHalfSingPropield      = default;
        NullIntSingPropield       = default;
        NullUIntSingPropield      = default;
        NullFloatSingPropield     = default;
        NullLongSingPropield      = default;
        NullULongSingPropield     = default;
        NullDoubleSingPropield    = default;
        NullDecimalSingPropield   = default;
        NullVeryLongSingPropield  = default;
        NullVeryUlongSingPropield = default;
        NullBigIntSingPropield    = default;

        NullComplexSetSingPropield   = default;
        NullComplexUnsetSingPropield = default;
        NullDateTimeSingPropield     = default;
        NullDateOnlySingPropield     = default;
        NullTimeSpanSingPropield     = default;
        NullTimeOnlySingPropield     = default;
        NullRuneSingPropield         = default;
        NullGuidSingPropield         = default;
        NullIpNetworkSingPropield    = default;

        StringSingPropield        = null!;
        StringBuilderSingPropield = null!;
        CharSequenceSingPropield  = null!;

        VersionSingPropield = null!;
        IntPtrSingPropield  = null!;
        UriSingPropield     = null!;

        SpanFormattableClassSingPropield = null!;

        NdLNfEnumSingPropield = default;
        NdUNfEnumSingPropield = default;
        NdLWfEnumSingPropield = default;
        NdUWfEnumSingPropield = default;

        WdLNfEnumSingPropield = default;
        WdUNfEnumSingPropield = default;
        WdLWfEnumSingPropield = default;
        WdUWfEnumSingPropield = default;

        NullNdLNfEnumSingPropield = default;
        NullNdUNfEnumSingPropield = default;
        NullNdLWfEnumSingPropield = default;
        NullNdUWfEnumSingPropield = default;

        NullWdLNfEnumSingPropield = default;
        NullWdUNfEnumSingPropield = default;
        NullWdLWfEnumSingPropield = default;
        NullWdUWfEnumSingPropield = default;
    }

    public byte ByteSingPropield;
    public sbyte SByteSingPropield { get; set; }
    public char CharSingPropield;
    public short ShortSingPropield { get; set; }
    public ushort UShortSingPropield;
    public Half HalfSetSingPropield { get; set; }
    public Half HalfNanSingPropield;
    public int IntSingPropield { get; set; }
    public uint UIntSingPropield;
    public float FloatSetSingPropield { get; set; }
    public float FloatNanSingPropield;
    public long LongSingPropield { get; set; }
    public ulong ULongSingPropield;
    public double DoubleSetSingPropield { get; set; }
    public double DoubleNanSingPropield;
    public decimal DecimalSingPropield { get; set; }
    public Int128 VeryLongSingPropield;
    public UInt128 VeryUlongSingPropield { get; set; }
    public BigInteger BigIntSingPropield;

    public Complex ComplexSingPropield;
    public DateTime DateTimeSingPropield { get; set; }
    public DateOnly DateOnlySingPropield;
    public TimeSpan TimeSpanSingPropield { get; set; }
    public TimeOnly TimeOnlySingPropield;
    public Rune RuneSingPropield { get; set; }
    public Guid GuidSingPropield;
    public IPNetwork IpNetworkSingPropield { get; set; }

    public byte? NullByteSingPropield { get; set; }
    public sbyte? NullSByteSingPropield;
    public char? NullCharSingPropield { get; set; }
    public short? NullShortSingPropield;
    public ushort? NullUShortSingPropield { get; set; }
    public Half? NullHalfSingPropield;
    public int? NullIntSingPropield { get; set; }
    public uint? NullUIntSingPropield;
    public float? NullFloatSingPropield { get; set; }
    public long? NullLongSingPropield;
    public ulong? NullULongSingPropield { get; set; }
    public double? NullDoubleSingPropield;
    public decimal? NullDecimalSingPropield { get; set; }
    public Int128? NullVeryLongSingPropield;
    public UInt128? NullVeryUlongSingPropield { get; set; }
    public BigInteger? NullBigIntSingPropield;

    public Complex? NullComplexSetSingPropield { get; set; }
    public Complex? NullComplexUnsetSingPropield;
    public DateTime? NullDateTimeSingPropield { get; set; }
    public DateOnly? NullDateOnlySingPropield;
    public TimeSpan? NullTimeSpanSingPropield { get; set; }
    public TimeOnly? NullTimeOnlySingPropield;
    public Rune? NullRuneSingPropield { get; set; }
    public Guid? NullGuidSingPropield;
    public IPNetwork? NullIpNetworkSingPropield { get; set; }

    public string StringSingPropield = null!;
    public StringBuilder StringBuilderSingPropield { get; set; } = null!;
    public ICharSequence CharSequenceSingPropield = null!;

    public Version VersionSingPropield { get; set; } = null!;
    public IPAddress IntPtrSingPropield = null!;
    public Uri UriSingPropield { get; set; } = null!;

    public MySpanFormattableClass SpanFormattableClassSingPropield = null!;

    public NoDefaultLongNoFlagsEnum NdLNfEnumSingPropield { get; set; }
    public NoDefaultULongNoFlagsEnum NdUNfEnumSingPropield;
    public NoDefaultLongWithFlagsEnum NdLWfEnumSingPropield { get; set; }
    public NoDefaultULongWithFlagsEnum NdUWfEnumSingPropield;

    public WithDefaultLongNoFlagsEnum WdLNfEnumSingPropield;
    public WithDefaultULongNoFlagsEnum WdUNfEnumSingPropield { get; set; }
    public WithDefaultLongWithFlagsEnum WdLWfEnumSingPropield;
    public WithDefaultULongWithFlagsEnum WdUWfEnumSingPropield { get; set; }

    public NoDefaultLongNoFlagsEnum? NullNdLNfEnumSingPropield { get; set; }
    public NoDefaultULongNoFlagsEnum? NullNdUNfEnumSingPropield;
    public NoDefaultLongWithFlagsEnum? NullNdLWfEnumSingPropield { get; set; }
    public NoDefaultULongWithFlagsEnum? NullNdUWfEnumSingPropield;

    public WithDefaultLongNoFlagsEnum? NullWdLNfEnumSingPropield;
    public WithDefaultULongNoFlagsEnum? NullWdUNfEnumSingPropield { get; set; }
    public WithDefaultLongWithFlagsEnum? NullWdLWfEnumSingPropield;
    public WithDefaultULongWithFlagsEnum? NullWdUWfEnumSingPropield { get; set; }

    public static PalantírReveal<StandardSinglePropertyFieldStruct> SelectStateRevealer(TestFieldRevealMode fieldRevealMode)
    {
        switch (fieldRevealMode)
        {
            case TestFieldRevealMode.WhenNonDefault:          return WhenNonDefaultRevealState;
            case TestFieldRevealMode.WhenNonNull:             return WhenNonNullRevealState;
            case TestFieldRevealMode.WhenNonNullOrNonDefault: return WhenNonNullOrDefaultRevealState;
            case TestFieldRevealMode.AlwaysAll:
            default:
                return AlwaysRevealAllState;
        }
    }

    public static PalantírReveal<StandardSinglePropertyFieldStruct> AlwaysRevealAllState
    {
        get
        {
            return
                (ssps, tos) =>
                {
                    using var ctb =
                        tos.StartComplexType(ssps);
                    ctb.Field.AlwaysAdd(nameof(ByteSingPropield), ssps.ByteSingPropield);
                    ctb.Field.AlwaysAdd(nameof(SByteSingPropield), ssps.SByteSingPropield);
                    ctb.Field.AlwaysAdd(nameof(CharSingPropield), ssps.CharSingPropield);
                    ctb.Field.AlwaysAdd(nameof(ShortSingPropield), ssps.ShortSingPropield);
                    ctb.Field.AlwaysAdd(nameof(UShortSingPropield), ssps.UShortSingPropield);
                    ctb.Field.AlwaysAdd(nameof(HalfSetSingPropield), ssps.HalfSetSingPropield);
                    ctb.Field.AlwaysAdd(nameof(HalfNanSingPropield), ssps.HalfNanSingPropield);
                    ctb.Field.AlwaysAdd(nameof(IntSingPropield), ssps.IntSingPropield);
                    ctb.Field.AlwaysAdd(nameof(UIntSingPropield), ssps.UIntSingPropield);
                    ctb.Field.AlwaysAdd(nameof(FloatSetSingPropield), ssps.FloatSetSingPropield);
                    ctb.Field.AlwaysAdd(nameof(FloatNanSingPropield), ssps.FloatNanSingPropield);
                    ctb.Field.AlwaysAdd(nameof(LongSingPropield), ssps.LongSingPropield);
                    ctb.Field.AlwaysAdd(nameof(ULongSingPropield), ssps.ULongSingPropield);
                    ctb.Field.AlwaysAdd(nameof(DoubleSetSingPropield), ssps.DoubleSetSingPropield);
                    ctb.Field.AlwaysAdd(nameof(DoubleNanSingPropield), ssps.DoubleNanSingPropield);
                    ctb.Field.AlwaysAdd(nameof(DecimalSingPropield), ssps.DecimalSingPropield);
                    ctb.Field.AlwaysAdd(nameof(VeryLongSingPropield), ssps.VeryLongSingPropield);
                    ctb.Field.AlwaysAdd(nameof(VeryUlongSingPropield), ssps.VeryUlongSingPropield);
                    ctb.Field.AlwaysAdd(nameof(BigIntSingPropield), ssps.BigIntSingPropield);
                    ctb.Field.AlwaysAdd(nameof(ComplexSingPropield), ssps.ComplexSingPropield);
                    ctb.Field.AlwaysAdd(nameof(DateTimeSingPropield), ssps.DateTimeSingPropield);
                    ctb.Field.AlwaysAdd(nameof(DateOnlySingPropield), ssps.DateOnlySingPropield);
                    ctb.Field.AlwaysAdd(nameof(TimeSpanSingPropield), ssps.TimeSpanSingPropield);
                    ctb.Field.AlwaysAdd(nameof(TimeOnlySingPropield), ssps.TimeOnlySingPropield);
                    ctb.Field.AlwaysAdd(nameof(RuneSingPropield), ssps.RuneSingPropield);
                    ctb.Field.AlwaysAdd(nameof(GuidSingPropield), ssps.GuidSingPropield);
                    ctb.Field.AlwaysAdd(nameof(IpNetworkSingPropield), ssps.IpNetworkSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullByteSingPropield), ssps.NullByteSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullSByteSingPropield), ssps.NullSByteSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullCharSingPropield), ssps.NullCharSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullShortSingPropield), ssps.NullShortSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullUShortSingPropield), ssps.NullUShortSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullHalfSingPropield), ssps.NullHalfSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullIntSingPropield), ssps.NullIntSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullUIntSingPropield), ssps.NullUIntSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullFloatSingPropield), ssps.NullFloatSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullLongSingPropield), ssps.NullLongSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullULongSingPropield), ssps.NullULongSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullDoubleSingPropield), ssps.NullDoubleSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullDecimalSingPropield), ssps.NullDecimalSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullVeryLongSingPropield), ssps.NullVeryLongSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullVeryUlongSingPropield), ssps.NullVeryUlongSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullBigIntSingPropield), ssps.NullBigIntSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullComplexSetSingPropield), ssps.NullComplexSetSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullComplexUnsetSingPropield), ssps.NullComplexUnsetSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullDateTimeSingPropield), ssps.NullDateTimeSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullDateOnlySingPropield), ssps.NullDateOnlySingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullTimeSpanSingPropield), ssps.NullTimeSpanSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullTimeOnlySingPropield), ssps.NullTimeOnlySingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullRuneSingPropield), ssps.NullRuneSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullGuidSingPropield), ssps.NullGuidSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullIpNetworkSingPropield), ssps.NullIpNetworkSingPropield);
                    ctb.Field.AlwaysAdd(nameof(StringSingPropield), ssps.StringSingPropield);
                    ctb.Field.AlwaysAdd(nameof(StringBuilderSingPropield), ssps.StringBuilderSingPropield);
                    ctb.Field.AlwaysAddCharSeq(nameof(CharSequenceSingPropield), ssps.CharSequenceSingPropield);
                    ctb.Field.AlwaysAdd(nameof(VersionSingPropield), ssps.VersionSingPropield);
                    ctb.Field.AlwaysAdd(nameof(IntPtrSingPropield), ssps.IntPtrSingPropield);
                    ctb.Field.AlwaysAdd(nameof(UriSingPropield), ssps.UriSingPropield);
                    ctb.Field.AlwaysAdd(nameof(SpanFormattableClassSingPropield), ssps.SpanFormattableClassSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NdLNfEnumSingPropield), ssps.NdLNfEnumSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NdUNfEnumSingPropield), ssps.NdUNfEnumSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NdLWfEnumSingPropield), ssps.NdLWfEnumSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NdUWfEnumSingPropield), ssps.NdUWfEnumSingPropield);
                    ctb.Field.AlwaysAdd(nameof(WdLNfEnumSingPropield), ssps.WdLNfEnumSingPropield);
                    ctb.Field.AlwaysAdd(nameof(WdUNfEnumSingPropield), ssps.WdUNfEnumSingPropield);
                    ctb.Field.AlwaysAdd(nameof(WdLWfEnumSingPropield), ssps.WdLWfEnumSingPropield);
                    ctb.Field.AlwaysAdd(nameof(WdUWfEnumSingPropield), ssps.WdUWfEnumSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullNdLNfEnumSingPropield), ssps.NullNdLNfEnumSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullNdUNfEnumSingPropield), ssps.NullNdUNfEnumSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullNdLWfEnumSingPropield), ssps.NullNdLWfEnumSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullNdUWfEnumSingPropield), ssps.NullNdUWfEnumSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullWdLNfEnumSingPropield), ssps.NullWdLNfEnumSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullWdUNfEnumSingPropield), ssps.NullWdUNfEnumSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullWdLWfEnumSingPropield), ssps.NullWdLWfEnumSingPropield);
                    ctb.Field.AlwaysAdd(nameof(NullWdUWfEnumSingPropield), ssps.NullWdUWfEnumSingPropield);
                    return ctb.Complete();
                };
        }
    }

    public static PalantírReveal<StandardSinglePropertyFieldStruct> WhenNonDefaultRevealState
    {
        get
        {
            return
                (ssps, tos) =>
                {
                    using var ctb =
                        tos.StartComplexType(ssps);
                    ctb.Field.WhenNonDefaultAdd(nameof(ByteSingPropield), ssps.ByteSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(SByteSingPropield), ssps.SByteSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(CharSingPropield), ssps.CharSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(ShortSingPropield), ssps.ShortSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(UShortSingPropield), ssps.UShortSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(HalfSetSingPropield), ssps.HalfSetSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(HalfNanSingPropield), ssps.HalfNanSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(IntSingPropield), ssps.IntSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(UIntSingPropield), ssps.UIntSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(FloatSetSingPropield), ssps.FloatSetSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(FloatNanSingPropield), ssps.FloatNanSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(LongSingPropield), ssps.LongSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(ULongSingPropield), ssps.ULongSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(DoubleSetSingPropield), ssps.DoubleSetSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(DoubleNanSingPropield), ssps.DoubleNanSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(DecimalSingPropield), ssps.DecimalSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(VeryLongSingPropield), ssps.VeryLongSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(VeryUlongSingPropield), ssps.VeryUlongSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(BigIntSingPropield), ssps.BigIntSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(ComplexSingPropield), ssps.ComplexSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(DateTimeSingPropield), ssps.DateTimeSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(DateOnlySingPropield), ssps.DateOnlySingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(TimeSpanSingPropield), ssps.TimeSpanSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(TimeOnlySingPropield), ssps.TimeOnlySingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(RuneSingPropield), ssps.RuneSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(GuidSingPropield), ssps.GuidSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(IpNetworkSingPropield), ssps.IpNetworkSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullByteSingPropield), ssps.NullByteSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullSByteSingPropield), ssps.NullSByteSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullCharSingPropield), ssps.NullCharSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullShortSingPropield), ssps.NullShortSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullUShortSingPropield), ssps.NullUShortSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullHalfSingPropield), ssps.NullHalfSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullIntSingPropield), ssps.NullIntSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullUIntSingPropield), ssps.NullUIntSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullFloatSingPropield), ssps.NullFloatSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullLongSingPropield), ssps.NullLongSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullULongSingPropield), ssps.NullULongSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullDoubleSingPropield), ssps.NullDoubleSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullDecimalSingPropield), ssps.NullDecimalSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullVeryLongSingPropield), ssps.NullVeryLongSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullVeryUlongSingPropield), ssps.NullVeryUlongSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullBigIntSingPropield), ssps.NullBigIntSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullComplexSetSingPropield), ssps.NullComplexSetSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullComplexUnsetSingPropield), ssps.NullComplexUnsetSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullDateTimeSingPropield), ssps.NullDateTimeSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullDateOnlySingPropield), ssps.NullDateOnlySingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullTimeSpanSingPropield), ssps.NullTimeSpanSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullTimeOnlySingPropield), ssps.NullTimeOnlySingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullRuneSingPropield), ssps.NullRuneSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullGuidSingPropield), ssps.NullGuidSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullIpNetworkSingPropield), ssps.NullIpNetworkSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(StringSingPropield), ssps.StringSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(StringBuilderSingPropield), ssps.StringBuilderSingPropield);
                    ctb.Field.WhenNonDefaultAddCharSeq(nameof(CharSequenceSingPropield), ssps.CharSequenceSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(VersionSingPropield), ssps.VersionSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(IntPtrSingPropield), ssps.IntPtrSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(UriSingPropield), ssps.UriSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(SpanFormattableClassSingPropield), ssps.SpanFormattableClassSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NdLNfEnumSingPropield), ssps.NdLNfEnumSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NdUNfEnumSingPropield), ssps.NdUNfEnumSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NdLWfEnumSingPropield), ssps.NdLWfEnumSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NdUWfEnumSingPropield), ssps.NdUWfEnumSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(WdLNfEnumSingPropield), ssps.WdLNfEnumSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(WdUNfEnumSingPropield), ssps.WdUNfEnumSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(WdLWfEnumSingPropield), ssps.WdLWfEnumSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(WdUWfEnumSingPropield), ssps.WdUWfEnumSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullNdLNfEnumSingPropield), ssps.NullNdLNfEnumSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullNdUNfEnumSingPropield), ssps.NullNdUNfEnumSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullNdLWfEnumSingPropield), ssps.NullNdLWfEnumSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullNdUWfEnumSingPropield), ssps.NullNdUWfEnumSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullWdLNfEnumSingPropield), ssps.NullWdLNfEnumSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullWdUNfEnumSingPropield), ssps.NullWdUNfEnumSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullWdLWfEnumSingPropield), ssps.NullWdLWfEnumSingPropield);
                    ctb.Field.WhenNonDefaultAdd(nameof(NullWdUWfEnumSingPropield), ssps.NullWdUWfEnumSingPropield);
                    return ctb.Complete();
                };
        }
    }

    public static PalantírReveal<StandardSinglePropertyFieldStruct> WhenNonNullRevealState
    {
        get
        {
            return
                (ssps, tos) =>
                {
                    using var ctb =
                        tos.StartComplexType(ssps);
                    ctb.Field.WhenNonNullAdd(nameof(ByteSingPropield), ssps.ByteSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(SByteSingPropield), ssps.SByteSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(CharSingPropield), ssps.CharSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(ShortSingPropield), ssps.ShortSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(UShortSingPropield), ssps.UShortSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(HalfSetSingPropield), ssps.HalfSetSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(HalfNanSingPropield), ssps.HalfNanSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(IntSingPropield), ssps.IntSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(UIntSingPropield), ssps.UIntSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(FloatSetSingPropield), ssps.FloatSetSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(FloatNanSingPropield), ssps.FloatNanSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(LongSingPropield), ssps.LongSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(ULongSingPropield), ssps.ULongSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(DoubleSetSingPropield), ssps.DoubleSetSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(DoubleNanSingPropield), ssps.DoubleNanSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(DecimalSingPropield), ssps.DecimalSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(VeryLongSingPropield), ssps.VeryLongSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(VeryUlongSingPropield), ssps.VeryUlongSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(BigIntSingPropield), ssps.BigIntSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(ComplexSingPropield), ssps.ComplexSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(DateTimeSingPropield), ssps.DateTimeSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(DateOnlySingPropield), ssps.DateOnlySingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(TimeSpanSingPropield), ssps.TimeSpanSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(TimeOnlySingPropield), ssps.TimeOnlySingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(RuneSingPropield), ssps.RuneSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(GuidSingPropield), ssps.GuidSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(IpNetworkSingPropield), ssps.IpNetworkSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullByteSingPropield), ssps.NullByteSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullSByteSingPropield), ssps.NullSByteSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullCharSingPropield), ssps.NullCharSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullShortSingPropield), ssps.NullShortSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullUShortSingPropield), ssps.NullUShortSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullHalfSingPropield), ssps.NullHalfSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullIntSingPropield), ssps.NullIntSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullUIntSingPropield), ssps.NullUIntSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullFloatSingPropield), ssps.NullFloatSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullLongSingPropield), ssps.NullLongSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullULongSingPropield), ssps.NullULongSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullDoubleSingPropield), ssps.NullDoubleSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullDecimalSingPropield), ssps.NullDecimalSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullVeryLongSingPropield), ssps.NullVeryLongSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullVeryUlongSingPropield), ssps.NullVeryUlongSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullBigIntSingPropield), ssps.NullBigIntSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullComplexSetSingPropield), ssps.NullComplexSetSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullComplexUnsetSingPropield), ssps.NullComplexUnsetSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullDateTimeSingPropield), ssps.NullDateTimeSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullDateOnlySingPropield), ssps.NullDateOnlySingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullTimeSpanSingPropield), ssps.NullTimeSpanSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullTimeOnlySingPropield), ssps.NullTimeOnlySingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullRuneSingPropield), ssps.NullRuneSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullGuidSingPropield), ssps.NullGuidSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullIpNetworkSingPropield), ssps.NullIpNetworkSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(StringSingPropield), ssps.StringSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(StringBuilderSingPropield), ssps.StringBuilderSingPropield);
                    ctb.Field.WhenNonNullAddCharSeq(nameof(CharSequenceSingPropield), ssps.CharSequenceSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(VersionSingPropield), ssps.VersionSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(IntPtrSingPropield), ssps.IntPtrSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(UriSingPropield), ssps.UriSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(SpanFormattableClassSingPropield), ssps.SpanFormattableClassSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NdLNfEnumSingPropield), ssps.NdLNfEnumSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NdUNfEnumSingPropield), ssps.NdUNfEnumSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NdLWfEnumSingPropield), ssps.NdLWfEnumSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NdUWfEnumSingPropield), ssps.NdUWfEnumSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(WdLNfEnumSingPropield), ssps.WdLNfEnumSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(WdUNfEnumSingPropield), ssps.WdUNfEnumSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(WdLWfEnumSingPropield), ssps.WdLWfEnumSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(WdUWfEnumSingPropield), ssps.WdUWfEnumSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullNdLNfEnumSingPropield), ssps.NullNdLNfEnumSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullNdUNfEnumSingPropield), ssps.NullNdUNfEnumSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullNdLWfEnumSingPropield), ssps.NullNdLWfEnumSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullNdUWfEnumSingPropield), ssps.NullNdUWfEnumSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullWdLNfEnumSingPropield), ssps.NullWdLNfEnumSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullWdUNfEnumSingPropield), ssps.NullWdUNfEnumSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullWdLWfEnumSingPropield), ssps.NullWdLWfEnumSingPropield);
                    ctb.Field.WhenNonNullAdd(nameof(NullWdUWfEnumSingPropield), ssps.NullWdUWfEnumSingPropield);
                    return ctb.Complete();
                };
        }
    }

    public static PalantírReveal<StandardSinglePropertyFieldStruct> WhenNonNullOrDefaultRevealState
    {
        get
        {
            return
                (ssps, tos) =>
                {
                    using var ctb =
                        tos.StartComplexType(ssps);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(ByteSingPropield), ssps.ByteSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(SByteSingPropield), ssps.SByteSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(CharSingPropield), ssps.CharSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(ShortSingPropield), ssps.ShortSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(UShortSingPropield), ssps.UShortSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(HalfSetSingPropield), ssps.HalfSetSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(HalfNanSingPropield), ssps.HalfNanSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(IntSingPropield), ssps.IntSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(UIntSingPropield), ssps.UIntSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(FloatSetSingPropield), ssps.FloatSetSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(FloatNanSingPropield), ssps.FloatNanSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(LongSingPropield), ssps.LongSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(ULongSingPropield), ssps.ULongSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(DoubleSetSingPropield), ssps.DoubleSetSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(DoubleNanSingPropield), ssps.DoubleNanSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(DecimalSingPropield), ssps.DecimalSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(VeryLongSingPropield), ssps.VeryLongSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(VeryUlongSingPropield), ssps.VeryUlongSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(BigIntSingPropield), ssps.BigIntSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(ComplexSingPropield), ssps.ComplexSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(DateTimeSingPropield), ssps.DateTimeSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(DateOnlySingPropield), ssps.DateOnlySingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(TimeSpanSingPropield), ssps.TimeSpanSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(TimeOnlySingPropield), ssps.TimeOnlySingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(RuneSingPropield), ssps.RuneSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(GuidSingPropield), ssps.GuidSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(IpNetworkSingPropield), ssps.IpNetworkSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullByteSingPropield), ssps.NullByteSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullSByteSingPropield), ssps.NullSByteSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullCharSingPropield), ssps.NullCharSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullShortSingPropield), ssps.NullShortSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullUShortSingPropield), ssps.NullUShortSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullHalfSingPropield), ssps.NullHalfSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullIntSingPropield), ssps.NullIntSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullUIntSingPropield), ssps.NullUIntSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullFloatSingPropield), ssps.NullFloatSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullLongSingPropield), ssps.NullLongSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullULongSingPropield), ssps.NullULongSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullDoubleSingPropield), ssps.NullDoubleSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullDecimalSingPropield), ssps.NullDecimalSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullVeryLongSingPropield), ssps.NullVeryLongSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullVeryUlongSingPropield), ssps.NullVeryUlongSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullBigIntSingPropield), ssps.NullBigIntSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullComplexSetSingPropield), ssps.NullComplexSetSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullComplexUnsetSingPropield), ssps.NullComplexUnsetSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullDateTimeSingPropield), ssps.NullDateTimeSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullDateOnlySingPropield), ssps.NullDateOnlySingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullTimeSpanSingPropield), ssps.NullTimeSpanSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullTimeOnlySingPropield), ssps.NullTimeOnlySingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullRuneSingPropield), ssps.NullRuneSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullGuidSingPropield), ssps.NullGuidSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullIpNetworkSingPropield), ssps.NullIpNetworkSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(StringSingPropield), ssps.StringSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(StringBuilderSingPropield), ssps.StringBuilderSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAddCharSeq(nameof(CharSequenceSingPropield), ssps.CharSequenceSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(VersionSingPropield), ssps.VersionSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(IntPtrSingPropield), ssps.IntPtrSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(UriSingPropield), ssps.UriSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(SpanFormattableClassSingPropield), ssps.SpanFormattableClassSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NdLNfEnumSingPropield), ssps.NdLNfEnumSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NdUNfEnumSingPropield), ssps.NdUNfEnumSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NdLWfEnumSingPropield), ssps.NdLWfEnumSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NdUWfEnumSingPropield), ssps.NdUWfEnumSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(WdLNfEnumSingPropield), ssps.WdLNfEnumSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(WdUNfEnumSingPropield), ssps.WdUNfEnumSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(WdLWfEnumSingPropield), ssps.WdLWfEnumSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(WdUWfEnumSingPropield), ssps.WdUWfEnumSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullNdLNfEnumSingPropield), ssps.NullNdLNfEnumSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullNdUNfEnumSingPropield), ssps.NullNdUNfEnumSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullNdLWfEnumSingPropield), ssps.NullNdLWfEnumSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullNdUWfEnumSingPropield), ssps.NullNdUWfEnumSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullWdLNfEnumSingPropield), ssps.NullWdLNfEnumSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullWdUNfEnumSingPropield), ssps.NullWdUNfEnumSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullWdLWfEnumSingPropield), ssps.NullWdLWfEnumSingPropield);
                    ctb.Field.WhenNonNullOrDefaultAdd(nameof(NullWdUWfEnumSingPropield), ssps.NullWdUWfEnumSingPropield);
                    return ctb.Complete();
                };
        }
    }
}
