// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Net;
using System.Numerics;
using System.Text;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

public static class SpanFormattableTestData
{
    public static readonly IFormatExpectation[] AllSpanFormattableExpectations =
    [
        // byte
        new FieldExpect<byte>(0, "") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new FieldExpect<byte>(255) { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "255" } }
      , new FieldExpect<byte>(128, "C2")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "$128.00" } }
      , new FieldExpect<byte>(77, "\"{0,-20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"77                  \"" }
        }
      , new FieldExpect<byte>(32, "", true, 32)
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "32" }
        }
      , new FieldExpect<byte>(255, "{0[..1]}")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2" } }
      , new FieldExpect<byte>(255, "{0[1..2]}")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "5" } }
      , new FieldExpect<byte>(255, "{0[1..]}")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "55" } }
      
        // byte?
      , new FieldExpect<byte?>(0, "{0}") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new FieldExpect<byte?>(null, "null", true)
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" }
        }
      , new FieldExpect<byte?>(255)
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "255" } }
      , new FieldExpect<byte?>(128, "C2")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "$128.00" }
        }
      , new FieldExpect<byte?>(144, "\"{0,20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"                 144\"" }
        }
      , new FieldExpect<byte?>(64, "", true, 64)
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "64" }
        }
      , new FieldExpect<byte?>(255, "{0[..1]}")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2" }
        }
      , new FieldExpect<byte?>(255, "{0[1..2]}")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "5" }
        }
      , new FieldExpect<byte?>(255, "{0[1..]}")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "55" } }
      
        // char
      , new FieldExpect<char>('\0', "") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "\0" } }
      , new FieldExpect<char>('A') { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "A" } }
      , new FieldExpect<char>(' ', "'{0}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "' '" } }
      , new FieldExpect<char>('z', "\"{0,-20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"z                   \"" }
        }
      
        // char?
      , new FieldExpect<char?>('\0', "") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "\0" } }
      , new FieldExpect<char?>(null, "null", true) { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" } }
      , new FieldExpect<char?>('A')
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "A" } }
      , new FieldExpect<char?>(' ', "'{0}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "' '" } }
      , new FieldExpect<char?>('z', "\"{0,20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"                   z\"" }
        }
      
        // short
      , new FieldExpect<short>(0, "") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new FieldExpect<short>(32000, "N2")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "32,000.00" } }
      , new FieldExpect<short>(32, "C0", true, 32) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "$32" } }
      , new FieldExpect<short>(-16328, "'{0}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'-16328'" } }
      , new FieldExpect<short>(55, "\"{0,-20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"55                  \"" }
        }
      
        // short?
      , new FieldExpect<short?>(0, "") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new FieldExpect<short?>(null, "null", true) { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" } }
      , new FieldExpect<short?>(32000, "N2")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "32,000.00" } }
      , new FieldExpect<short?>(32, "C0", true, 32)
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "$32" }
        }
      , new FieldExpect<short?>(-16328, "'{0}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'-16328'" } }
      , new FieldExpect<short?>(55, "\"{0,20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"                  55\"" }
        }
      
        // ushort
      , new FieldExpect<ushort>(0, "") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new FieldExpect<ushort>(32000, "N2")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "32,000.00" } }
      , new FieldExpect<ushort>(32, "C0", true, 32) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "$32" } }
      , new FieldExpect<ushort>(ushort.MaxValue, "'{0:B16}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'1111111111111111'" } }
      , new FieldExpect<ushort>(55, "\"{0,-20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"55                  \"" }
        }
      
        // ushort?
      , new FieldExpect<ushort?>(0, "") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new FieldExpect<ushort?>(null, "null", true) { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" } }
      , new FieldExpect<ushort?>(32000, "N2")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "32,000.00" } }
      , new FieldExpect<ushort?>(32, "C8", true, 32)
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "$32.00000000" }
        }
      , new FieldExpect<ushort?>(ushort.MaxValue, "'{0:B16}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'1111111111111111'" } }
      , new FieldExpect<ushort?>(55, "\"{0,20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"                  55\"" }
        }

        // Half
      , new FieldExpect<Half>(Half.Zero) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new FieldExpect<Half>(Half.MinValue / (Half)2.0, "R")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "-32750" }
        }
      , new FieldExpect<Half>(Half.One, "", true, Half.One) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "1" } }
      , new FieldExpect<Half>(Half.NaN, "", true, Half.NaN) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "NaN" } }
      , new FieldExpect<Half>(Half.NaN, "\"{0}\"")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"NaN\"" } }
      , new FieldExpect<Half>(Half.MaxValue, "'{0:G}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'65500'" } }
      , new FieldExpect<Half>(Half.MinValue, "'{0:c}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'-$65,504.00'" } }
      , new FieldExpect<Half>((Half)(Math.E * 10.0), "N0")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "27" }
        }
      , new FieldExpect<float>((float)Math.PI, "\"{0,-20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"3.1415927           \"" }
        }

        // Half?
      , new FieldExpect<Half?>(Half.Zero) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new FieldExpect<Half?>(null, "null", true) { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" } }
      , new FieldExpect<Half?>(Half.MinValue / (Half)2.0, "R")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "-32750" }
        }
      , new FieldExpect<Half?>(Half.One, "", true, Half.One) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "1" } }
      , new FieldExpect<Half?>(Half.NaN, "", true, Half.NaN) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "NaN" } }
      , new FieldExpect<Half?>(Half.NaN, "\"{0}\"")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"NaN\"" } }
      , new FieldExpect<Half?>(Half.MaxValue, "'{0:G}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'65500'" } }
      , new FieldExpect<Half?>(Half.MinValue, "'{0:c}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'-$65,504.00'" } }
      , new FieldExpect<Half?>((Half)(Math.E * 10.0), "N0")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "27" }
        }
      , new FieldExpect<float?>((float)Math.PI, "\"{0,-20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"3.1415927           \"" }
        }

        // int
      , new FieldExpect<int>(0, "") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new FieldExpect<int>(32000, "0x{0:X}")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0x7D00" } }
      , new FieldExpect<int>(32, "C0", true, 32) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "$32" } }
      , new FieldExpect<int>(int.MaxValue, "'{0:X8}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'7FFFFFFF'" } }
      , new FieldExpect<int>(int.MinValue, "'{0:X9}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'080000000'" } }
      , new FieldExpect<int>(55, "\"{0,-20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"55                  \"" }
        }

        // int?
      , new FieldExpect<int?>(0, "") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new FieldExpect<int?>(null, "null", true) { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" } }
      , new FieldExpect<int?>(32000, "0x{0:X}")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0x7D00" } }
      , new FieldExpect<int?>(32, "C8", true, 32) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "$32.00000000" } }
      , new FieldExpect<int?>(int.MaxValue, "'{0:X8}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'7FFFFFFF'" } }
      , new FieldExpect<int?>(int.MinValue, "'{0:X9}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'080000000'" } }
      , new FieldExpect<int?>(55, "\"{0,20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"                  55\"" }
        }

        // uint
      , new FieldExpect<uint>(0, "") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new FieldExpect<uint>(32000, "0x{0:X}")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0x7D00" } }
      , new FieldExpect<uint>(32, "C0", true, 32) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "$32" } }
      , new FieldExpect<uint>(uint.MaxValue, "'{0:X8}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'FFFFFFFF'" } }
      , new FieldExpect<uint>(uint.MinValue, "'{0:X9}'", true, 100)
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'000000000'" } }
      , new FieldExpect<uint>(55, "\"{0,-20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"55                  \"" }
        }

        // uint?
      , new FieldExpect<uint?>(0, "") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new FieldExpect<uint?>(null, "null", true) { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" } }
      , new FieldExpect<uint?>(32000, "0x{0:X}")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0x7D00" } }
      , new FieldExpect<uint?>(32, "C8", true, 32) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "$32.00000000" } }
      , new FieldExpect<uint?>(uint.MaxValue, "'{0:X8}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'FFFFFFFF'" } }
      , new FieldExpect<uint?>(uint.MinValue, "'{0:X9}'", true, 100)
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'000000000'" } }
      , new FieldExpect<uint?>(55, "\"{0,20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"                  55\"" }
        }

        // float
      , new FieldExpect<float>(0, "") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new FieldExpect<float>(1 - float.MinValue, "R")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "3.4028235E+38" } }
      , new FieldExpect<float>(1, "", true, 1) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "1" } }
      , new FieldExpect<float>(float.NaN, "", true, float.NaN) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "NaN" } }
      , new FieldExpect<float>(float.NaN, "\"{0}\"")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"NaN\"" } }
      , new FieldExpect<float>(float.MaxValue, "'{0:G}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'3.4028235E+38'" } }
      , new FieldExpect<float>(float.MinValue, "'{0:c}'")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'-$340,282,346,638,528,859,811,704,183,484,516,925,440.00'"
            }
        }
      , new FieldExpect<float>((float)Math.E * 1_000_000, "N0")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2,718,282" }
        }
      , new FieldExpect<float>((float)Math.PI, "\"{0,-20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"3.1415927           \"" }
        }

        // float?
      , new FieldExpect<float?>(0, "") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new FieldExpect<float?>(null, "null", true) { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" } }
      , new FieldExpect<float?>(1 - float.MinValue, "R")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "3.4028235E+38" } }
      , new FieldExpect<float?>(1, "", true, 1) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "1" } }
      , new FieldExpect<float?>(float.NaN, "", true, float.NaN) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "NaN" } }
      , new FieldExpect<float?>(float.NaN, "\"{0}\"")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"NaN\"" } }
      , new FieldExpect<float?>(float.MaxValue, "'{0:G}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'3.4028235E+38'" } }
      , new FieldExpect<float?>(float.MinValue, "'{0:c}'")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'-$340,282,346,638,528,859,811,704,183,484,516,925,440.00'"
            }
        }
      , new FieldExpect<float?>((float)Math.E * 1_000_000, "N0")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2,718,282" }
        }
      , new FieldExpect<float?>((float)Math.PI, "\"{0,-20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"3.1415927           \"" }
        }

        // long
      , new FieldExpect<long>(0, "") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new FieldExpect<long>(32000, "0x{0:X}")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0x7D00" } }
      , new FieldExpect<long>(32, "C0", true, 32) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "$32" } }
      , new FieldExpect<long>(long.MaxValue, "'{0:X16}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'7FFFFFFFFFFFFFFF'" } }
      , new FieldExpect<long>(long.MinValue, "'{0:X17}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'08000000000000000'" } }
      , new FieldExpect<long>(55, "\"{0,-20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"55                  \"" }
        }

        // long?
      , new FieldExpect<long?>(0, "") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new FieldExpect<long?>(null, "null", true) { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" } }
      , new FieldExpect<long?>(32000, "0x{0:X}")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0x7D00" } }
      , new FieldExpect<long?>(32, "C8", true, 32) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "$32.00000000" } }
      , new FieldExpect<long?>(long.MaxValue, "'{0:X16}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'7FFFFFFFFFFFFFFF'" } }
      , new FieldExpect<long?>(long.MinValue, "'{0:X17}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'08000000000000000'" } }
      , new FieldExpect<long?>(55, "\"{0,20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"                  55\"" }
        }

        // ulong
      , new FieldExpect<ulong>(0, "") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new FieldExpect<ulong>(32000, "0x{0:X}")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0x7D00" } }
      , new FieldExpect<ulong>(32, "C0", true, 32) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "$32" } }
      , new FieldExpect<ulong>(ulong.MaxValue, "'{0:X16}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'FFFFFFFFFFFFFFFF'" } }
      , new FieldExpect<ulong>(ulong.MinValue, "'{0:X17}'", true, 100)
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'00000000000000000'" } }
      , new FieldExpect<ulong>(55, "\"{0,-20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"55                  \"" }
        }

        // ulong?
      , new FieldExpect<ulong?>(0, "") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new FieldExpect<ulong?>(null, "null", true) { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" } }
      , new FieldExpect<ulong?>(32000, "0x{0:X}")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0x7D00" } }
      , new FieldExpect<ulong?>(32, "C8", true, 32) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "$32.00000000" } }
      , new FieldExpect<ulong?>(ulong.MaxValue, "'{0:X16}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'FFFFFFFFFFFFFFFF'" } }
      , new FieldExpect<ulong?>(ulong.MinValue, "'{0:X17}'", true, 100)
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'00000000000000000'" } }
      , new FieldExpect<ulong?>(55, "\"{0,20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"                  55\"" }
        }

        // double
      , new FieldExpect<double>(0, "") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new FieldExpect<double>(1 - double.MinValue, "R")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "1.7976931348623157E+308" } }
      , new FieldExpect<double>(1, "", true, 1) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "1" } }
      , new FieldExpect<double>(double.NaN, "", true, double.NaN) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "NaN" } }
      , new FieldExpect<double>(double.NaN, "\"{0}\"")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"NaN\"" } }
      , new FieldExpect<double>(double.MaxValue, "'{0:G}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'1.7976931348623157E+308'" } }
      , new FieldExpect<double>(double.MinValue, "'{0:c}'")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'-$179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299,881,250,404,026,184,124,858,368.00'"
            }
        }
      , new FieldExpect<double>(Math.E * 1_000_000, "N0")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2,718,282" }
        }
      , new FieldExpect<double>(Math.PI, "\"{0,-20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"3.141592653589793   \"" }
        }

        // double?
      , new FieldExpect<double?>(0, "") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new FieldExpect<double?>(null, "null", true) { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" } }
      , new FieldExpect<double?>(1 - double.MinValue, "R")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "1.7976931348623157E+308" } }
      , new FieldExpect<double?>(1, "", true, 1) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "1" } }
      , new FieldExpect<double?>(double.NaN, "", true, double.NaN) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "NaN" } }
      , new FieldExpect<double?>(double.NaN, "\"{0}\"")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"NaN\"" } }
      , new FieldExpect<double?>(double.MaxValue, "'{0:G}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'1.7976931348623157E+308'" } }
      , new FieldExpect<double?>(double.MinValue, "'{0:c}'")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'-$179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299,881,250,404,026,184,124,858,368.00'"
            }
        }
      , new FieldExpect<double?>(Math.E * 1_000_000, "N0")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2,718,282" }
        }
      , new FieldExpect<double?>(Math.PI, "\"{0,-20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"3.141592653589793   \"" }
        }

        // decimal
      , new FieldExpect<decimal>(0, "") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new FieldExpect<decimal>(decimal.MinValue, "R")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "-79228162514264337593543950335" }
        }
      , new FieldExpect<decimal>(1, "", true, 1) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "1" } }
      , new FieldExpect<decimal>(decimal.MaxValue, "'{0:G}'")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'79228162514264337593543950335'" }
        }
      , new FieldExpect<decimal>(decimal.MinValue, "'{0:c}'")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'-$79,228,162,514,264,337,593,543,950,335.00'"
            }
        }
      , new FieldExpect<decimal>((decimal)Math.E * 1_000_000, "N0")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2,718,282" }
        }
      , new FieldExpect<decimal>((decimal)Math.PI, "\"{0,-20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"3.14159265358979    \"" }
        }

        // decimal?
      , new FieldExpect<decimal?>(0, "") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new FieldExpect<decimal?>(null, "null", true) { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" } }
      , new FieldExpect<decimal?>(decimal.MinValue, "R")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "-79228162514264337593543950335" }
        }
      , new FieldExpect<decimal?>(1, "", true, 1) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "1" } }
      , new FieldExpect<decimal?>(decimal.MaxValue, "'{0:G}'")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'79228162514264337593543950335'" }
        }
      , new FieldExpect<decimal?>(decimal.MinValue, "'{0:c}'")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'-$79,228,162,514,264,337,593,543,950,335.00'"
            }
        }
      , new FieldExpect<decimal?>((decimal)Math.E * 1_000_000, "N0")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2,718,282" }
        }
      , new FieldExpect<decimal?>((decimal)Math.PI, "\"{0,-20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"3.14159265358979    \"" }
        }

        // Int128
      , new FieldExpect<Int128>(0, "") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new FieldExpect<Int128>(32000, "0x{0:X}")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0x7D00" } }
      , new FieldExpect<Int128>(32, "C0", true, 32) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "$32" } }
      , new FieldExpect<Int128>(Int128.MaxValue, "'{0:X32}'")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF'"
            }
        }
      , new FieldExpect<Int128>(Int128.MinValue, "'{0:X33}'")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'080000000000000000000000000000000'"
            }
        }
      , new FieldExpect<Int128>(Int128.MaxValue, "\"{0,-52:N0}\"")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"170,141,183,460,469,231,731,687,303,715,884,105,727 \""
            }
        }

        // Int128?
      , new FieldExpect<Int128?>(0, "") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new FieldExpect<Int128?>(null, "null", true) { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" } }
      , new FieldExpect<Int128?>(32000, "0x{0:X}")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0x7D00" } }
      , new FieldExpect<Int128?>(32, "C0", true, 32) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "$32" } }
      , new FieldExpect<Int128?>(Int128.MaxValue, "'{0:X32}'")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF'"
            }
        }
      , new FieldExpect<Int128?>(Int128.MinValue, "'{0:X33}'")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'080000000000000000000000000000000'"
            }
        }
      , new FieldExpect<Int128?>(Int128.MaxValue, "\"{0,-52:N0}\"")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"170,141,183,460,469,231,731,687,303,715,884,105,727 \""
            }
        }

        // UInt128
      , new FieldExpect<UInt128>(0, "") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new FieldExpect<UInt128>(32000, "0x{0:X}")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0x7D00" } }
      , new FieldExpect<UInt128>(32, "C0", true, 32) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "$32" } }
      , new FieldExpect<UInt128>(UInt128.MaxValue, "'{0:X32}'")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF'"
            }
        }
      , new FieldExpect<UInt128>(UInt128.MinValue, "'{0:X33}'", true, (UInt128)100)
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'000000000000000000000000000000000'"
            }
        }
      , new FieldExpect<UInt128>(UInt128.MaxValue, "\"{0,-52:N0}\"")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"340,282,366,920,938,463,463,374,607,431,768,211,455 \""
            }
        }

        // UInt128?
      , new FieldExpect<UInt128?>(0, "") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new FieldExpect<UInt128?>(null, "null", true) { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" } }
      , new FieldExpect<UInt128?>(32000, "0x{0:X}")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0x7D00" } }
      , new FieldExpect<UInt128?>(32, "C0", true, 32) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "$32" } }
      , new FieldExpect<UInt128?>(UInt128.MaxValue, "'{0:X32}'")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF'"
            }
        }
      , new FieldExpect<UInt128?>(UInt128.MinValue, "'{0:X33}'", true, (UInt128)100)
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'000000000000000000000000000000000'"
            }
        }
      , new FieldExpect<UInt128?>(UInt128.MaxValue, "\"{0,-52:N0}\"")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"340,282,366,920,938,463,463,374,607,431,768,211,455 \""
            }
        }

        // BigInteger
      , new FieldExpect<BigInteger>(0, "") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new FieldExpect<BigInteger>(32000, "0x{0:X}")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0x7D00" } }
      , new FieldExpect<BigInteger>(32, "C0", true, 32) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "$32" } }
      , new FieldExpect<BigInteger>(UInt128.MaxValue * (BigInteger)50, "'{0:X32}'")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'31FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFCE'"
            }
        }
      , new FieldExpect<BigInteger>(Int128.MinValue * (BigInteger)50, "'{0:X33}'")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'E700000000000000000000000000000000'"
            }
        }
      , new FieldExpect<BigInteger>(UInt128.MaxValue * (BigInteger)100, "\"{0,-56:N0}\"")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"34,028,236,692,093,846,346,337,460,743,176,821,145,500  \""
            }
        }

        // BigInteger?
      , new FieldExpect<BigInteger?>(0, "") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new FieldExpect<BigInteger?>(null, "null", true) { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" } }
      , new FieldExpect<BigInteger?>(32000, "0x{0:X}")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0x7D00" } }
      , new FieldExpect<BigInteger?>(32, "C0", true, 32) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "$32" } }
      , new FieldExpect<BigInteger?>(UInt128.MaxValue * (BigInteger)50, "'{0:X32}'")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'31FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFCE'"
            }
        }
      , new FieldExpect<BigInteger?>(Int128.MinValue * (BigInteger)50, "'{0:X33}'")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'E700000000000000000000000000000000'"
            }
        }
      , new FieldExpect<BigInteger?>(UInt128.MaxValue * (BigInteger)100, "\"{0,-56:N0}\"")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"34,028,236,692,093,846,346,337,460,743,176,821,145,500  \""
            }
        }

        // Complex
      , new FieldExpect<Complex>(0, "") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "<0; 0>" } }
      , new FieldExpect<Complex>(32000, "{0:N0}")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "<32,000; 0>" }
        }
      , new FieldExpect<Complex>(new Complex(32.0d, 1), "N0", true, new Complex(32.0d, 1))
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "<32; 1>" }
        }
      , new FieldExpect<Complex>(new Complex(999999.999, 999999.999), "'{0:N2}'")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'<1,000,000.00; 1,000,000.00>'" }
        }
      , new FieldExpect<Complex>(new Complex(double.MinValue, double.MinValue), "'{0:N9}'")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'<-179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632" +
                ",766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389" +
                ",328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299" +
                ",881,250,404,026,184,124,858,368.000000000; -179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476" +
                ",803,157,260,780,028,538,760,589,558,632,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986" +
                ",049,910,576,551,282,076,245,490,090,389,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797" +
                ",826,204,144,723,168,738,177,180,919,299,881,250,404,026,184,124,858,368.000000000>'"
            }
        }
      , new FieldExpect<Complex>(new Complex(Math.PI, Math.E), "\"{0-20}\"")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"<3.141592653589793; 2.718281828459045>\""
            }
        }

        // Complex?
      , new FieldExpect<Complex?>(0, "") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "<0; 0>" } }
      , new FieldExpect<Complex?>(null, "null", true) { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" } }
      , new FieldExpect<Complex?>(32000, "{0:N0}")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "<32,000; 0>" }
        }
      , new FieldExpect<Complex?>(new Complex(32.0d, 1), "N0", true, new Complex(32.0d, 1))
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "<32; 1>" }
        }
      , new FieldExpect<Complex?>(new Complex(999999.999, 999999.999), "'{0:N2}'")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'<1,000,000.00; 1,000,000.00>'" }
        }
      , new FieldExpect<Complex?>(new Complex(double.MinValue, double.MinValue), "'{0:N9}'")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'<-179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632" +
                ",766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389" +
                ",328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299" +
                ",881,250,404,026,184,124,858,368.000000000; -179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476" +
                ",803,157,260,780,028,538,760,589,558,632,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986" +
                ",049,910,576,551,282,076,245,490,090,389,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797" +
                ",826,204,144,723,168,738,177,180,919,299,881,250,404,026,184,124,858,368.000000000>'"
            }
        }
      , new FieldExpect<Complex?>(new Complex(Math.PI, Math.E), "\"{0-20}\"")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"<3.141592653589793; 2.718281828459045>\""
            }
        }

        // DateTime
      , new FieldExpect<DateTime>(DateTime.MinValue, "O")
            { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0001-01-01T00:00:00.0000000" } }
      , new FieldExpect<DateTime>(new DateTime(2000, 1, 1, 1, 1, 1).AddTicks(1111111), "o")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2000-01-01T01:01:01.1111111" } }
      , new FieldExpect<DateTime>(new DateTime(2020, 2, 2)
                                      .AddTicks(2222222), "s", true
                                , new DateTime(2020, 2, 2).AddTicks(2222222))
            { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "2020-02-02T00:00:00" } }
      , new FieldExpect<DateTime>(DateTime.MaxValue, "'{0:u}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'9999-12-31 23:59:59Z'" } }
      , new FieldExpect<DateTime>(DateTime.MinValue, "\"{0,30:u}\"", true, new DateTime(2020, 1, 1))
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"          0001-01-01 00:00:00Z\""
            }
        }
      , new FieldExpect<DateTime>(new DateTime(1980, 7, 31, 11, 48, 13), "'{0:yyyy-MM-dd HH:mm:ss}'")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'1980-07-31 11:48:13'" }
        }
      , new FieldExpect<DateTime>(new DateTime(2009, 11, 12, 19, 49, 0), "\"{0,-30:O}\"")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"2009-11-12T19:49:00.0000000   \""
            }
        }

        // DateTime?
      , new FieldExpect<DateTime?>(DateTime.MinValue, "O")
            { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0001-01-01T00:00:00.0000000" } }
      , new FieldExpect<DateTime?>(null, "null", true) { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" } }
      , new FieldExpect<DateTime?>(new DateTime(2000, 1, 1, 1, 1, 1).AddTicks(1111111), "o")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2000-01-01T01:01:01.1111111" } }
      , new FieldExpect<DateTime?>(new DateTime(2020, 2, 2)
                                                    .AddTicks(2222222), "s", true
                                              , new DateTime(2020, 2, 2).AddTicks(2222222))
            { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "2020-02-02T00:00:00" } }
      , new FieldExpect<DateTime?>(DateTime.MaxValue, "'{0:u}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'9999-12-31 23:59:59Z'" } }
      , new FieldExpect<DateTime?>(DateTime.MinValue, "\"{0,30:u}\"", true, new DateTime(2020, 1, 1))
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"          0001-01-01 00:00:00Z\""
            }
        }
      , new FieldExpect<DateTime?>(new DateTime(1980, 7, 31, 11, 48, 13), "'{0:yyyy-MM-dd HH:mm:ss}'")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'1980-07-31 11:48:13'" }
        }
      , new FieldExpect<DateTime?>(new DateTime(2009, 11, 12, 19, 49, 0), "\"{0,-30:O}\"")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"2009-11-12T19:49:00.0000000   \""
            }
        }

        // TimeSpan
      , new FieldExpect<TimeSpan>(TimeSpan.Zero, "g") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0:00:00" } }
      , new FieldExpect<TimeSpan>(new TimeSpan(1, 1, 1, 1, 111, 111), "c")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "1.01:01:01.1111110" } }
      , new FieldExpect<TimeSpan>(new TimeSpan(-2, -22, -22, -22, -222, -222), "G", true
                                , new TimeSpan(-2, -22, -22, -22, -222, -222))
            { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "-2:22:22:22.2222220" } }
      , new FieldExpect<TimeSpan>(TimeSpan.MaxValue, "'{0:G}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'10675199:02:48:05.4775807'" } }
      , new FieldExpect<TimeSpan>(TimeSpan.MinValue, "\"{0,30:c}\"", true, TimeSpan.Zero)
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"    -10675199.02:48:05.4775808\""
            }
        }
      , new FieldExpect<TimeSpan>(new TimeSpan(3, 3, 33, 33, 333, 333),
                                  "'{0:dd\\-hh\\-mm\\-ss\\.fff}'")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'03-03-33-33.333'" }
        }
      , new FieldExpect<TimeSpan>(new TimeSpan(-4, -4, -44, -44, -444, -444), "\"{0,-30:G}\"")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"-4:04:44:44.4444440           \""
            }
        }

        // TimeSpan?
      , new FieldExpect<TimeSpan?>(TimeSpan.Zero, "g") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0:00:00" } }
      , new FieldExpect<TimeSpan?>(null, "null", true) { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" } }
      , new FieldExpect<TimeSpan?>(new TimeSpan(1, 1, 1, 1, 111, 111), "c")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "1.01:01:01.1111110" } }
      , new FieldExpect<TimeSpan?>(new TimeSpan(-2, -22, -22, -22, -222, -222), "G", true
                                              , new TimeSpan(-2, -22, -22, -22, -222, -222))
            { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "-2:22:22:22.2222220" } }
      , new FieldExpect<TimeSpan?>(TimeSpan.MaxValue, "'{0:G}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'10675199:02:48:05.4775807'" } }
      , new FieldExpect<TimeSpan?>(TimeSpan.MinValue, "\"{0,30:c}\"", true, TimeSpan.Zero)
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"    -10675199.02:48:05.4775808\""
            }
        }
      , new FieldExpect<TimeSpan?>(new TimeSpan(3, 3, 33, 33, 333, 333),
                                                "'{0:dd\\-hh\\-mm\\-ss\\.fff}'")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'03-03-33-33.333'" }
        }
      , new FieldExpect<TimeSpan?>(new TimeSpan(-4, -4, -44, -44, -444, -444)
                                              , "\"{0,-30:G}\"")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"-4:04:44:44.4444440           \""
            }
        }

        // DateOnly
      , new FieldExpect<DateOnly>(DateOnly.MinValue, "o")
            { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0001-01-01" } }
      , new FieldExpect<DateOnly>(new DateOnly(2000, 1, 1), "o")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2000-01-01" } }
      , new FieldExpect<DateOnly>(new DateOnly(2020, 2, 2), "o", true
                                , new DateOnly(2020, 2, 2))
            { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "2020-02-02" } }
      , new FieldExpect<DateOnly>(DateOnly.MaxValue, "'{0:o}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'9999-12-31'" } }
      , new FieldExpect<DateOnly>(DateOnly.MinValue, "\"{0,30:o}\"", true
                                , new DateOnly(2020, 1, 1))
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"                    0001-01-01\""
            }
        }
      , new FieldExpect<DateOnly>(new DateOnly(1980, 7, 31), "'{0:yyyy\\\\MM\\\\dd}'")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'1980\\07\\31'" }
        }
      , new FieldExpect<DateOnly>(new DateOnly(2009, 11, 12), "\"{0,-30:o}\"")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"2009-11-12                    \""
            }
        }

        // DateOnly?
      , new FieldExpect<DateOnly?>(DateOnly.MinValue, "o")
            { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0001-01-01" } }
      , new FieldExpect<DateOnly?>(null, "null", true) { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" } }
      , new FieldExpect<DateOnly?>(new DateOnly(2000, 1, 1), "o")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2000-01-01" } }
      , new FieldExpect<DateOnly?>(new DateOnly(2020, 2, 2), "o", true
                                              , new DateOnly(2020, 2, 2))
            { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "2020-02-02" } }
      , new FieldExpect<DateOnly?>(DateOnly.MaxValue, "'{0:o}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'9999-12-31'" } }
      , new FieldExpect<DateOnly?>(DateOnly.MinValue, "\"{0,30:o}\"", true, new DateOnly(2020, 1, 1))
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"                    0001-01-01\""
            }
        }
      , new FieldExpect<DateOnly?>(new DateOnly(1980, 7, 31), "'{0:yyyy\\\\MM\\\\dd}'")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'1980\\07\\31'" }
        }
      , new FieldExpect<DateOnly?>(new DateOnly(2009, 11, 12), "\"{0,-30:o}\"")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"2009-11-12                    \""
            }
        }

        // TimeOnly
      , new FieldExpect<TimeOnly>(TimeOnly.FromTimeSpan(TimeSpan.Zero), "r") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "00:00:00" } }
      , new FieldExpect<TimeOnly>(new TimeOnly(1, 1, 1, 111, 111), "o")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "01:01:01.1111110" } }
      , new FieldExpect<TimeOnly>(new TimeOnly(22, 22, 22, 222, 222), "O", true
                                , new TimeOnly(22, 22, 22, 222, 222))
            { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "22:22:22.2222220" } }
      , new FieldExpect<TimeOnly>(TimeOnly.MaxValue, "'{0:o}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'23:59:59.9999999'" } }
      , new FieldExpect<TimeOnly>(TimeOnly.MinValue, "\"{0,30:r}\"", true
                                , TimeOnly.FromTimeSpan(TimeSpan.FromHours(1)))
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"                      00:00:00\""
            }
        }
      , new FieldExpect<TimeOnly>(new TimeOnly(3, 33, 33, 333, 333),
                                  "'{0:hh\\-mm\\-ss\\.fff}'")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'03-33-33.333'" }
        }
      , new FieldExpect<TimeOnly>(new TimeOnly(4, 44, 44, 444, 444), "\"{0,-30:O}\"")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"04:44:44.4444440              \""
            }
        }

        // TimeOnly?
      , new FieldExpect<TimeOnly?>(TimeOnly.FromTimeSpan(TimeSpan.Zero), "r")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "00:00:00" }
        }
      , new FieldExpect<TimeOnly?>(null, "null", true) { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" } }
      , new FieldExpect<TimeOnly?>(new TimeOnly(1, 1, 1, 111, 111), "o")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "01:01:01.1111110" } }
      , new FieldExpect<TimeOnly?>(new TimeOnly(22, 22, 22, 222, 222), "O", true
                                              , new TimeOnly(22, 22, 22, 222, 222))
            { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "22:22:22.2222220" } }
      , new FieldExpect<TimeOnly?>(TimeOnly.MaxValue, "'{0:o}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'23:59:59.9999999'" } }
      , new FieldExpect<TimeOnly?>(TimeOnly.MinValue, "\"{0,30:r}\"", true
                                              , TimeOnly.FromTimeSpan(TimeSpan.FromHours(1)))
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"                      00:00:00\""
            }
        }
      , new FieldExpect<TimeOnly?>(new TimeOnly(3, 33, 33, 333, 333),
                                                "'{0:hh\\-mm\\-ss\\.fff}'")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'03-33-33.333'" }
        }
      , new FieldExpect<TimeOnly?>(new TimeOnly(4, 44, 44, 444, 444), "\"{0,-30:O}\"")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"04:44:44.4444440              \""
            }
        }

        // Rune
      , new FieldExpect<Rune>(Rune.GetRuneAt("\0", 0)) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "\0" } }
      , new FieldExpect<Rune>(Rune.GetRuneAt("𝄞", 0))
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "𝄞" }
        }
      , new FieldExpect<Rune>(Rune.GetRuneAt("𝄢", 0), "'{0}'")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'𝄢'" }
        }
      , new FieldExpect<Rune>(Rune.GetRuneAt("𝅘𝅥𝅮", 0), "\"{0,-20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"𝅘𝅥𝅮                  \"" }
        }

        // Rune?
      , new FieldExpect<Rune?>(Rune.GetRuneAt("\0", 0)) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "\0" } }
      , new FieldExpect<Rune?>(null, "null", true) { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" } }
      , new FieldExpect<Rune?>(Rune.GetRuneAt("𝄞", 0))
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "𝄞" }
        }
      , new FieldExpect<Rune?>(Rune.GetRuneAt("𝄢", 0), "'{0}'")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'𝄢'" }
        }
      , new FieldExpect<Rune?>(Rune.GetRuneAt("𝅘𝅥𝅮", 0), "\"{0,-20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"𝅘𝅥𝅮                  \"" }
        }

        // Guid
      , new FieldExpect<Guid>(Guid.Empty) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "00000000-0000-0000-0000-000000000000" } }
      , new FieldExpect<Guid>(Guid.ParseExact("BEEFCA4E-BEEF-CA4E-BEEF-C0FFEEBABE51", "D"))
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "beefca4e-beef-ca4e-beef-c0ffeebabe51"
            }
        }
      , new FieldExpect<Guid>(Guid.ParseExact("C0FFEEFE-BEEF-CA4E-BEEF-C0FFEEBABE51", "D"), "'{0}'")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'c0ffeefe-beef-ca4e-beef-c0ffeebabe51'"
            }
        }
      , new FieldExpect<Guid>(Guid.ParseExact("BEEEEEEF-BEEF-BEEF-BEEF-CAAAAAAAAA4E", "D"), "\"{0,40}\"")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"    beeeeeef-beef-beef-beef-caaaaaaaaa4e\""
            }
        }

        // Guid?
      , new FieldExpect<Guid?>(Guid.Empty)
            { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "00000000-0000-0000-0000-000000000000" } }
      , new FieldExpect<Guid?>(null, "null", true) { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" } }
      , new FieldExpect<Guid?>(Guid.ParseExact("BEEFCA4E-BEEF-CA4E-BEEF-C0FFEEBABE51", "D"))
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "beefca4e-beef-ca4e-beef-c0ffeebabe51"
            }
        }
      , new FieldExpect<Guid?>(Guid.ParseExact("C0FFEEFE-BEEF-CA4E-BEEF-C0FFEEBABE51", "D"), "'{0}'")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'c0ffeefe-beef-ca4e-beef-c0ffeebabe51'"
            }
        }
      , new FieldExpect<Guid?>(Guid.ParseExact("BEEEEEEF-BEEF-BEEF-BEEF-CAAAAAAAAA4E", "D"), "\"{0,40}\"")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"    beeeeeef-beef-beef-beef-caaaaaaaaa4e\""
            }
        }

        // IPNetwork
      , new FieldExpect<IPNetwork>(new IPNetwork())
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0.0.0.0/0" }
        }
      , new FieldExpect<IPNetwork>(new IPNetwork(IPAddress.Loopback, 32))
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "127.0.0.1/32" }
        }
      , new FieldExpect<IPNetwork>(IPNetwork.Parse("255.255.255.254/31"), "'{0}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'255.255.255.254/31'" } }
      , new FieldExpect<IPNetwork>(IPNetwork.Parse("255.255.0.0/16"), "\"{0,17}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"   255.255.0.0/16\"" }
        }

        // IPNetwork?
      , new FieldExpect<IPNetwork?>(new IPNetwork())
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0.0.0.0/0" }
        }
      , new FieldExpect<IPNetwork?>(null, "null", true) { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" } }
      , new FieldExpect<IPNetwork?>(new IPNetwork(IPAddress.Loopback, 32))
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "127.0.0.1/32" }
        }
      , new FieldExpect<IPNetwork?>(IPNetwork.Parse("255.255.255.254/31"), "'{0}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'255.255.255.254/31'" } }
      , new FieldExpect<IPNetwork?>(IPNetwork.Parse("255.255.0.0/16"), "\"{0,17}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"   255.255.0.0/16\"" }
        }

        // Version and Version?  (Class)
      , new FieldExpect<Version>(new Version())
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0.0" }
        }
      , new FieldExpect<Version>(null, "null", true) { { AcceptsSpanFormattable | AlwaysWrites, "null" } }
      , new FieldExpect<Version>(new Version(1, 1))
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "1.1" }
        }
      , new FieldExpect<Version>(new Version("1.2.3.4"), "'{0}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'1.2.3.4'" } }
      , new FieldExpect<Version>(null, "null", true) { { AcceptsSpanFormattable | AlwaysWrites, "null" } }
      , new FieldExpect<Version>(new Version(1, 0), "'{0}'", true, new Version(1, 0))
            { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "'1.0'" } }
      , new FieldExpect<Version>(new Version("5.6.7.8"), "\"{0,17}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"          5.6.7.8\"" }
        }

        //  IPAddress and IPAddress?
      , new FieldExpect<IPAddress>(new IPAddress("\0\0\0\0"u8))
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0.0.0.0" }
        }
      , new FieldExpect<IPAddress>(null, "null", true) { { AcceptsSpanFormattable | AlwaysWrites, "null" } }
      , new FieldExpect<IPAddress>(IPAddress.Loopback)
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "127.0.0.1" }
        }
      , new FieldExpect<IPAddress>(new IPAddress([192, 168, 0, 1]), "'{0}'", true, new IPAddress([192, 168, 0, 1]))
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "'192.168.0.1'" }
        }
      , new FieldExpect<IPAddress>(IPAddress.Parse("255.255.255.254"), "'{0}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'255.255.255.254'" } }
      , new FieldExpect<IPAddress>(IPAddress.Parse("255.255.0.0"), "\"{0,17}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"      255.255.0.0\"" }
        }

        //  Uri and Uri?
      , new FieldExpect<Uri>(new Uri("https://learn.microsoft.com/en-us/dotnet/api"))
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "https://learn.microsoft.com/en-us/dotnet/api"
            }
        }
      , new FieldExpect<Uri>(null, "null", true) { { AcceptsSpanFormattable | AlwaysWrites, "null" } }
      , new FieldExpect<Uri>(new Uri("https://github.com/shwaindog/Fortitude"), "'{0}'", true, new Uri("https://github.com/shwaindog/Fortitude"))
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "'https://github.com/shwaindog/Fortitude'" }
        }
      , new
            FieldExpect<Uri>(new
                                 Uri("https://github.com/shwaindog/Fortitude/tree/main/src/FortitudeTests/FortitudeCommon/Types/StringsOfPower/DieCasting/TestData")
                           , "{0[..38]}")
            {
                {
                    AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "https://github.com/shwaindog/Fortitude"
                }
            }
      , new FieldExpect<Uri>(new Uri("https://en.wikipedia.org/wiki/Rings_of_Power"), "'{0,-40}'")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'https://en.wikipedia.org/wiki/Rings_of_Power'"
            }
        }
    ];
}
