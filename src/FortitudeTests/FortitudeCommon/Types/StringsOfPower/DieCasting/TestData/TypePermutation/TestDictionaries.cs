// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Net;
using System.Numerics;
using System.Text;
using FluentAssertions.Formatting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification.CollectionItemResult;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation;

public class TestDictionaries
{
    public static readonly Dictionary<bool, int> BoolIntMap = new()
    {
        { true, 1 }
      , { false, 0 }
    };

    public static KeyValuePredicate<bool, int> BoolIntMap_First_8        = (count, _, _) => StopOnFirstExclusion(count <= 8);
    
    public static KeyValuePredicate<bool, int> BoolIntMap_Second_5       = (count, _, _) => 
        BetweenRetrieveRange(count, 6, 11);
    
    public static KeyValuePredicate<bool, int> BoolIntMap_First_FalseKey = (_, key, _) => First(!key);

    public static readonly List<KeyValuePair<bool?, int?>> NullBoolNullIntKvpList = new()
    {
        new KeyValuePair<bool?, int?>(null, 0)
      , new KeyValuePair<bool?, int?>(true, 1)
      , new KeyValuePair<bool?, int?>(false, null)
    };
    public static KeyValuePredicate<bool?, int?> NullIntMap_Second_5 = (count, _, _) => 
        BetweenRetrieveRange(count, 6, 11);
    
    public static KeyValuePredicate<bool?, int?> NullIntMap_Skip_Odd_Index = (count, _, _) => 
        EvaluateIsIncludedAndContinue(((count - 1) % 2) == 0, 1);
    
    public static KeyValuePredicate<bool?, int?> NullIntMap_First_FalseKey = (_, key, _) => First(key is false);

    public static readonly Dictionary<double, ICharSequence> DoubleCharSequenceMap = new()
    {
        { Math.PI, new MutableString("Eating the crust edges of one pie means you have eaten the length of two pi") }
      , { 2 * Math.PI, new MutableString("You have now eaten only 1 pie length, but if it is blood pudding pie it will feel like 2") }
      , { Math.E, new CharArrayStringBuilder("One doesn't simply write Euler nature number.") }
      , { 2 * Math.E, new CharArrayStringBuilder("One doesn't even appear at the start of Euler nature number.") }
      , { Math.PI * Math.E, new CharArrayStringBuilder("Oiler and Euler are very different things.") }
      , { 1, new CharArrayStringBuilder("All for one and one for all.") }
      , { -1, new CharArrayStringBuilder("Imagine there's no tax havens, it's easy if you try") }
    };

    public static KeyValuePredicate<double, ICharSequence> DoubleCharSequenceMap_First_5  = (count, _, _) => 
        StopOnFirstExclusion(count <= 5);
    
    public static KeyValuePredicate<double, ICharSequence> DoubleCharSequenceMap_Second_5 = (count, _, _) => 
        BetweenRetrieveRange(count, 6, 11);
    
    public static KeyValuePredicate<double, ICharSequence> DoubleCharSequenceMap_First_2  = (count, _, _) => 
        StopOnFirstExclusion(count <= 2);

    public static KeyValuePredicate<double, ICharSequence> DoubleCharSequenceMap_Skip_Odd_Index = (_, _, _) => 
        EvaluateIsIncludedAndContinue(true, 1);
    
    public static KeyValuePredicate<double, ICharSequence> DoubleCharSequenceMap_FirstKey_Lt_5  = (_, key, _) => First(key < 5.0f);

    public static readonly List<KeyValuePair<double?, ICharSequence?>> NullDoubleNullCharSequence = new()
    {
        new KeyValuePair<double?, ICharSequence?>(null, null)
      , new KeyValuePair<double?, ICharSequence?>
            (Math.PI , new MutableString("Eating the crust edges of one pie means you have eaten the length of two pi"))
      , new KeyValuePair<double?, ICharSequence?>(Math.E, new CharArrayStringBuilder("One doesn't simply write Euler nature number."))
      , new KeyValuePair<double?, ICharSequence?>(1, new CharArrayStringBuilder("All for one and one for all."))
      , new KeyValuePair<double?, ICharSequence?>
            (null, new CharArrayStringBuilder("\"Your contract is null AND void\", apparently not the same thing or why say both."))
      , new KeyValuePair<double?, ICharSequence?>(-1, new CharArrayStringBuilder("Imagine there's no tax havens, it's easy if you try"))
    };

    public static KeyValuePredicate<double?, ICharSequence?>
        NullDoubleNullCharSequenceMap_First_2 = (count, _, _) => StopOnFirstExclusion(count <= 2);
    
    public static KeyValuePredicate<double?, ICharSequence?>
        NullDoubleNullCharSequenceMap_First_5 = (count, _, _) => StopOnFirstExclusion(count <= 5);
    
    public static KeyValuePredicate<double?, ICharSequence?> NullDoubleNullCharSequenceMap_Second_5
        = (count, _, _) => BetweenRetrieveRange(count, 6, 11);
    
    public static KeyValuePredicate<double?, ICharSequence?> NullDoubleNullCharSequenceMap_Skip_Odd_Index
        = (_, _, _) => EvaluateIsIncludedAndContinue(true, 1);
    
    public static KeyValuePredicate<double?, ICharSequence?> NullDoubleNullCharSequenceMap_FirstKey_Lt_5 = (_, key, _) => First(key is < 5.0f);


    public static readonly Dictionary<TimeSpan, char> TimeSpanCharMap = new()
    {
        { TimeSpan.Zero, 'z' }
      , { TimeSpan.FromDays(1), 'd' }
      , { TimeSpan.FromHours(1), 'h' }
      , { TimeSpan.FromMinutes(1), 'm' }
      , { TimeSpan.FromSeconds(1), 's' }
      , { TimeSpan.FromMilliseconds(1), 'f' }
    };

    public static readonly List<KeyValuePair<TimeSpan?, char?>> NullTimeSpanCharMap = new()
    {
        new KeyValuePair<TimeSpan?, char?>(null, null)
      , new KeyValuePair<TimeSpan?, char?>(null, '\0')
      , new KeyValuePair<TimeSpan?, char?>(TimeSpan.Zero, 'z')
      , new KeyValuePair<TimeSpan?, char?>(TimeSpan.FromDays(1), 'd')
      , new KeyValuePair<TimeSpan?, char?>(TimeSpan.FromHours(1), 'h')
      , new KeyValuePair<TimeSpan?, char?>(TimeSpan.FromMinutes(1), 'm')
    };

    public static readonly Dictionary<int, MySpanFormattableStruct> IntMySpanFormattableStructMap = new()
    {
        { 0, new MySpanFormattableStruct("MySpanFormattableStruct.ToString() => 0 ") }
      , { 3600 * 24, new MySpanFormattableStruct("MySpanFormattableStruct.ToString() => 1d ") }
      , { -1, default }
      , { 60, new MySpanFormattableStruct("MySpanFormattableStruct.ToString() => 1m") }
    };

    public static readonly List<KeyValuePair<int?, MySpanFormattableStruct?>> NullIntNullMySpanFormattableStructMap = new()
    {
        new KeyValuePair<int?, MySpanFormattableStruct?>(null, null)
      , new KeyValuePair<int?, MySpanFormattableStruct?>(0, new MySpanFormattableStruct("MySpanFormattableStruct.ToString() => \"0\" "))
      , new KeyValuePair<int?, MySpanFormattableStruct?>(3600 * 24, new MySpanFormattableStruct("MySpanFormattableStruct.ToString() => \"1d\" "))
      , new KeyValuePair<int?, MySpanFormattableStruct?>(-1, null)
      , new KeyValuePair<int?, MySpanFormattableStruct?>(60, new MySpanFormattableStruct("MySpanFormattableStruct.ToString() => \"1m\""))
    };

    public static readonly Dictionary<IPAddress, Uri> IPAddressUriMap = new()
    {
        { new IPAddress([0, 0, 0, 0]), new Uri("http://First-null.com") }
      , { new IPAddress([127, 0, 0, 1]), new Uri("localhost") }
      , { new IPAddress([255, 255, 255, 255]), new Uri("http://unknown.com") }
      , { new IPAddress([192, 168, 1, 1]), new Uri("tcp://Default-Gateway") }
    };

    public static readonly List<KeyValuePair<IPAddress?, Uri?>> NullIPAddressUriMap = new()
    {
        new KeyValuePair<IPAddress?, Uri?>(null, new Uri("http://First-null.com"))
      , new KeyValuePair<IPAddress?, Uri?>(new IPAddress([0, 0, 0, 0]), new Uri("localhost"))
      , new KeyValuePair<IPAddress?, Uri?>(new IPAddress([255, 255, 255, 255]), new Uri("http://unknown.com"))
      , new KeyValuePair<IPAddress?, Uri?>(new IPAddress([192, 168, 1, 1]), new Uri("tcp://Default-Gateway"))
      , new KeyValuePair<IPAddress?, Uri?>(null, null)
    };

    public static readonly Dictionary<string, StringBuilder> StringStringBuilderMap = new()
    {
        { "", new StringBuilder("Empty Value") }
      , { "FirstKey", new StringBuilder("FirstValue") }
      , { "NullKey", null! }
      , { "SecondKey", new StringBuilder("SecondValue") }
    };

    public static readonly List<KeyValuePair<string?, StringBuilder?>> NullStringNullStringBuilderMap = new()
    {
        new KeyValuePair<string?, StringBuilder?>(null, new StringBuilder("null Key Value"))
      , new KeyValuePair<string?, StringBuilder?>("", new StringBuilder("empty key value"))
      , new KeyValuePair<string?, StringBuilder?>("NullValue", null)
      , new KeyValuePair<string?, StringBuilder?>("FirstKey", new StringBuilder("FirstValue"))
      , new KeyValuePair<string?, StringBuilder?>(null, null)
    };

    public static readonly Dictionary<ICharSequence, bool> CharSequenceBoolMap = new()
    {
        { new CharArrayStringBuilder(""), true }
      , { new MutableString("FirstKey"), false }
      , { new CharArrayStringBuilder("NullKey"), false }
      , { new MutableString("SecondKey"), true }
    };

    public static readonly List<KeyValuePair<ICharSequence?, bool?>> NullCharSequenceBoolMap = new()
    {
        new KeyValuePair<ICharSequence?, bool?>(new CharArrayStringBuilder(""), true)
      , new KeyValuePair<ICharSequence?, bool?>(new MutableString("FirstKey"), false)
      , new KeyValuePair<ICharSequence?, bool?>(new CharArrayStringBuilder("NullKey"), null)
      , new KeyValuePair<ICharSequence?, bool?>(null, null)
    };

    public static readonly Dictionary<object, object?> ObjToObjMap = new()
    {
        { true, 1 }
      , { 42, (bool?)false }
      , { "StringKey", new CharArrayStringBuilder("CharArrayStringBuilderValue") }
      , { new MySpanFormattableClass("MySpanFormattableClassKeyWithNullValue"), null }
      , { new MySpanFormattableStruct("MySpanFormattableStructKeyWithByteValue"), byte.MaxValue }
      , { new Version(1, 1, 1, 1), "NextReleaseStringValue" }
      , { "CharArrayKeyWithDoubleValue".ToCharArray(), Math.PI }
      , { new MutableString("MutableStringKeyBigIntegerValue"), ((BigInteger)UInt128.MaxValue + UInt128.MaxValue) }
      , { new StringBuilder("StringBuilderKeyIPAddressValue"), new IPAddress([127, 0, 0, 1]) }
    };

    public static readonly List<KeyValuePair<object?, object?>> NullObjNullObjMap = new()
    {
        new KeyValuePair<object?, object?>(true, 1)
      , new KeyValuePair<object?, object?>(42, (bool?)false)
      , new KeyValuePair<object?, object?>(null, "NullKey")
      , new KeyValuePair<object?, object?>("StringKey", new CharArrayStringBuilder("CharArrayStringBuilderValue"))
      , new KeyValuePair<object?, object?>(new MySpanFormattableClass("MySpanFormattableClassKeyWithNullValue"), null)
      , new KeyValuePair<object?, object?>(new MySpanFormattableStruct("MySpanFormattableStructKeyWithByteValue"), byte.MaxValue)
      , new KeyValuePair<object?, object?>(new Version(1, 1, 1, 1), "NextReleaseStringValue")
      , new KeyValuePair<object?, object?>("CharArrayKeyWithDoubleValue".ToCharArray(), Math.PI)
      , new KeyValuePair<object?, object?>(new MutableString("MutableStringKeyBigIntegerValue"), (UInt128.MaxValue + (BigInteger)UInt128.MaxValue))
      , new KeyValuePair<object?, object?>(new StringBuilder("StringBuilderKeyIPAddressValue"), new IPAddress([127, 0, 0, 1]))
      , new KeyValuePair<object?, object?>(null, null)
    };

    // 1. Start No Flags enums
    // 1. a) No Default No Flags
    public static readonly Dictionary<NoDefaultLongNoFlagsEnum, string?> EnumLongNdNfNullStringMap = new()
    {
        { NoDefaultLongNoFlagsEnum.NDLNFE_4, "" }
      , { NoDefaultLongNoFlagsEnum.NDLNFE_34, "NDLNFE_34_MapValue" }
      , { NoDefaultLongNoFlagsEnum.NDLNFE_1.Default(), "Zero chance" }
      , { NoDefaultLongNoFlagsEnum.NDLNFE_1, null }
      , { NoDefaultLongNoFlagsEnum.NDLNFE_13, "NDLNFE_Lucky_Number" }
      , { NoDefaultLongNoFlagsEnum.NDLNFE_2, null }
    };

    public static readonly List<KeyValuePair<NoDefaultLongNoFlagsEnum?, string?>> NullEnumLongNdNfNullStringMap = new()
    {
        new KeyValuePair<NoDefaultLongNoFlagsEnum?, string?>(NoDefaultLongNoFlagsEnum.NDLNFE_4, "")
      , new KeyValuePair<NoDefaultLongNoFlagsEnum?, string?>
            (null , "\"Your contract is null AND void\", apparently not the same thing or why say both.")
      , new KeyValuePair<NoDefaultLongNoFlagsEnum?, string?>(NoDefaultLongNoFlagsEnum.NDLNFE_1, null)
      , new KeyValuePair<NoDefaultLongNoFlagsEnum?, string?>(NoDefaultLongNoFlagsEnum.NDLNFE_1.Default(), "Zero chance")
      , new KeyValuePair<NoDefaultLongNoFlagsEnum?, string?>(NoDefaultLongNoFlagsEnum.NDLNFE_13, "NDLNFE_Lucky_Number")
      , new KeyValuePair<NoDefaultLongNoFlagsEnum?, string?>(null, null)
    };

    public static readonly Dictionary<NoDefaultULongWithFlagsEnum, DateTime> EnumULongNdNfDateTimeMap = new()
    {
        { NoDefaultULongWithFlagsEnum.NDUWFE_4, new DateTime(4444, 4, 4, 4, 4, 4) }
      , { NoDefaultULongWithFlagsEnum.NDUWFE_34, DateTime.MaxValue }
      , { NoDefaultULongWithFlagsEnum.NDUWFE_1.Default(), new DateTime() }
      , { NoDefaultULongWithFlagsEnum.NDUWFE_1, new DateTime(2025, 11, 27, 8, 57, 22) }
      , { NoDefaultULongWithFlagsEnum.NDUWFE_13, new DateTime(1981, 7, 31, 13, 57, 12) }
      , { NoDefaultULongWithFlagsEnum.NDUWFE_2, new DateTime(2000, 01, 01, 0, 0, 0) }
    };

    public static readonly List<KeyValuePair<NoDefaultULongNoFlagsEnum?, DateTime?>> NullEnumULongNdNfNullStringMap = new()
    {
        new KeyValuePair<NoDefaultULongNoFlagsEnum?, DateTime?>
            (NoDefaultULongNoFlagsEnum.NDUNFE_4, new DateTime(4444, 4, 4, 4, 4, 4))
      , new KeyValuePair<NoDefaultULongNoFlagsEnum?, DateTime?>(null, DateTime.MaxValue)
      , new KeyValuePair<NoDefaultULongNoFlagsEnum?, DateTime?>(NoDefaultULongNoFlagsEnum.NDUNFE_1.Default(), new DateTime())
      , new KeyValuePair<NoDefaultULongNoFlagsEnum?, DateTime?>(NoDefaultULongNoFlagsEnum.NDUNFE_1, null)
      , new KeyValuePair<NoDefaultULongNoFlagsEnum?, DateTime?>
            (NoDefaultULongNoFlagsEnum.NDUNFE_13, new DateTime(2025, 11, 27, 8, 57, 22))
      , new KeyValuePair<NoDefaultULongNoFlagsEnum?, DateTime?>(null, null)
    };

    // 1. b) With Default No Flags
    public static readonly Dictionary<WithDefaultLongNoFlagsEnum, StringBuilder> EnumLongWdNfStringBuilderMap = new()
    {
        { WithDefaultLongNoFlagsEnum.WDLNFE_4, new StringBuilder() }
      , { WithDefaultLongNoFlagsEnum.WDLNFE_34, new StringBuilder("WDLWFE_34_Value") }
      , { WithDefaultLongNoFlagsEnum.Default, new StringBuilder("Zero chance") }
      , { WithDefaultLongNoFlagsEnum.WDLNFE_1, new StringBuilder("One is the loneliest number that you'll ever do") }
      , { WithDefaultLongNoFlagsEnum.WDLNFE_2, new StringBuilder("Two can be as bad as one") }
      , { WithDefaultLongNoFlagsEnum.WDLNFE_3, null! }
    };

    public static readonly List<KeyValuePair<WithDefaultLongNoFlagsEnum?, StringBuilder?>> NullEnumULongWdNfNullStringBuilderMap = new()
    {
        new KeyValuePair<WithDefaultLongNoFlagsEnum?, StringBuilder?>(WithDefaultLongNoFlagsEnum.WDLNFE_4, new StringBuilder())
      , new KeyValuePair<WithDefaultLongNoFlagsEnum?, StringBuilder?>
            (null , new StringBuilder("\"Your contract is null AND void\", apparently not the same thing or why say both."))
      , new KeyValuePair<WithDefaultLongNoFlagsEnum?, StringBuilder?>(WithDefaultLongNoFlagsEnum.Default, new StringBuilder("Zero chance"))
      , new KeyValuePair<WithDefaultLongNoFlagsEnum?, StringBuilder?>(WithDefaultLongNoFlagsEnum.WDLNFE_1, null)
      , new KeyValuePair<WithDefaultLongNoFlagsEnum?, StringBuilder?>(WithDefaultLongNoFlagsEnum.WDLNFE_13, new StringBuilder("WDLWFE_Lucky_Number"))
      , new KeyValuePair<WithDefaultLongNoFlagsEnum?, StringBuilder?>(null, null)
    };

    public static readonly Dictionary<WithDefaultULongNoFlagsEnum, Version> EnumULongWdNfNullVersionMap = new()
    {
        { WithDefaultULongNoFlagsEnum.WDUNFE_2, null! }
      , { WithDefaultULongNoFlagsEnum.WDUNFE_4, new Version(4, 4) }
      , { WithDefaultULongNoFlagsEnum.WDUNFE_34, new Version(34, 34, 34, 34) }
      , { WithDefaultULongNoFlagsEnum.Default, new Version() }
      , { WithDefaultULongNoFlagsEnum.WDUNFE_13, new Version(13, 31) }
    };

    public static readonly List<KeyValuePair<WithDefaultULongNoFlagsEnum?, Version?>> NullEnumULongWdNfNullStringMap = new()
    {
        new KeyValuePair<WithDefaultULongNoFlagsEnum?, Version?>(WithDefaultULongNoFlagsEnum.WDUNFE_4, null!)
      , new KeyValuePair<WithDefaultULongNoFlagsEnum?, Version?>(null, new Version(4, 4))
      , new KeyValuePair<WithDefaultULongNoFlagsEnum?, Version?>
            (WithDefaultULongNoFlagsEnum.WDUNFE_4, new Version(34, 34, 34, 34))
      , new KeyValuePair<WithDefaultULongNoFlagsEnum?, Version?>(WithDefaultULongNoFlagsEnum.Default, new Version())
      , new KeyValuePair<WithDefaultULongNoFlagsEnum?, Version?>(null, new Version(13, 31))
    };


    //  2. Start With Flags enums
    //  2. a)   No Default With Flags
    public static readonly Dictionary<NoDefaultLongWithFlagsEnum, char[]> EnumLongNdWfDateTimeMap = new()
    {
        { NoDefaultLongWithFlagsEnum.NDLWFE_4, "NDLWFE_4".ToCharArray() }
      , { NoDefaultLongWithFlagsEnum.NDLWFE_34, "NDLWFE_34".ToCharArray() }
      , { NoDefaultLongWithFlagsEnum.NDLWFE_1.First8MinusFlag2Mask(), "NDLWFE_First8MinusFlag2Mask".ToCharArray() }
      , { NoDefaultLongWithFlagsEnum.NDLWFE_1.First8AndLast2Mask(), "NDLWFE_First8AndLast2Mask".ToCharArray() }
      , { NoDefaultLongWithFlagsEnum.NDLWFE_1.First8AndLast2MaskPlusUnnamed(), "NDLWFE_First8AndLast2MaskPlusUnnamed".ToCharArray() }
      , { NoDefaultLongWithFlagsEnum.NDLWFE_1.Default(), "Zero chance".ToCharArray() }
      , { NoDefaultLongWithFlagsEnum.NDLWFE_1, "NDLWFE_1".ToCharArray() }
      , { NoDefaultLongWithFlagsEnum.NDLWFE_1.First4Mask(), "NDLWFE_First4Mask".ToCharArray() }
      , { NoDefaultLongWithFlagsEnum.NDLWFE_2, "".ToCharArray() }
    };

    public static readonly List<KeyValuePair<NoDefaultLongWithFlagsEnum?, char[]?>> NullEnumLongNdWfNullStringMap = new()
    {
        new KeyValuePair<NoDefaultLongWithFlagsEnum?, char[]?>(NoDefaultLongWithFlagsEnum.NDLWFE_4, "NDLWFE_4".ToCharArray())
      , new KeyValuePair<NoDefaultLongWithFlagsEnum?, char[]?>(null, "NDLWFE_34".ToCharArray())
      , new KeyValuePair<NoDefaultLongWithFlagsEnum?, char[]?>
            (NoDefaultLongWithFlagsEnum.NDLWFE_1.First8MinusFlag2Mask() , "NDLWFE_First8MinusFlag2Mask".ToCharArray())
      , new KeyValuePair<NoDefaultLongWithFlagsEnum?, char[]?>
            (NoDefaultLongWithFlagsEnum.NDLWFE_1.First8AndLast2Mask() , "NDLWFE_First8AndLast2Mask".ToCharArray())
      , new KeyValuePair<NoDefaultLongWithFlagsEnum?, char[]?>
            (NoDefaultLongWithFlagsEnum.NDLWFE_1.First8AndLast2MaskPlusUnnamed(), "NDLWFE_First8AndLast2MaskPlusUnnamed".ToCharArray())
      , new KeyValuePair<NoDefaultLongWithFlagsEnum?, char[]?>(NoDefaultLongWithFlagsEnum.NDLWFE_1.Default(), "Zero chance".ToCharArray())
      , new KeyValuePair<NoDefaultLongWithFlagsEnum?, char[]?>(NoDefaultLongWithFlagsEnum.NDLWFE_1, "NDLWFE_1".ToCharArray())
      , new KeyValuePair<NoDefaultLongWithFlagsEnum?, char[]?>(NoDefaultLongWithFlagsEnum.NDLWFE_13, "NDLWFE_Lucky_Number".ToCharArray())
      , new KeyValuePair<NoDefaultLongWithFlagsEnum?, char[]?>(null, null)
    };

    public static readonly Dictionary<NoDefaultULongWithFlagsEnum, ICharSequence> EnumULongNdWfStringBuilderMap = new()
    {
        { NoDefaultULongWithFlagsEnum.NDUWFE_4, new MutableString("NDUWFE_4") }
      , { NoDefaultULongWithFlagsEnum.NDUWFE_1, new CharArrayStringBuilder() }
      , { NoDefaultULongWithFlagsEnum.NDUWFE_34, new MutableString("NDUWFE_34") }
      , { NoDefaultULongWithFlagsEnum.NDUWFE_1.First8MinusFlag2Mask(), new CharArrayStringBuilder("NDUWFE_First8MinusFlag2Mask") }
      , { NoDefaultULongWithFlagsEnum.NDUWFE_1.First8AndLast2Mask(), new MutableString("NDUWFE_First8AndLast2Mask") }
      , { NoDefaultULongWithFlagsEnum.NDUWFE_1.First8AndLast2MaskPlusUnnamed(), new CharArrayStringBuilder("NDUWFE_First8AndLast2MaskPlusUnnamed") }
      , { NoDefaultULongWithFlagsEnum.NDUWFE_1.Default(), new CharArrayStringBuilder("Zero chance") }
      , { NoDefaultULongWithFlagsEnum.NDUWFE_2, new MutableString("NDUWFE_1") }
      , { NoDefaultULongWithFlagsEnum.NDUWFE_13, new CharArrayStringBuilder("NDUWFE_Lucky_Number") }
      , { NoDefaultULongWithFlagsEnum.NDUWFE_3, null! }
    };

    public static readonly List<KeyValuePair<NoDefaultULongWithFlagsEnum?, ICharSequence?>> NullEnumULongNdWfNullStringBuilderMap = new()
    {
        new KeyValuePair<NoDefaultULongWithFlagsEnum?, ICharSequence?>(NoDefaultULongWithFlagsEnum.NDUWFE_4, new MutableString("NDUWFE_4"))
      , new KeyValuePair<NoDefaultULongWithFlagsEnum?, ICharSequence?>(NoDefaultULongWithFlagsEnum.NDUWFE_1, new CharArrayStringBuilder())
      , new KeyValuePair<NoDefaultULongWithFlagsEnum?, ICharSequence?>(NoDefaultULongWithFlagsEnum.NDUWFE_34, new MutableString("NDUWFE_34"))
      , new KeyValuePair<NoDefaultULongWithFlagsEnum?, ICharSequence?>
            (NoDefaultULongWithFlagsEnum.NDUWFE_1.First8MinusFlag2Mask(), new CharArrayStringBuilder("NDUWFE_First8MinusFlag2Mask"))
      , new KeyValuePair<NoDefaultULongWithFlagsEnum?, ICharSequence?>
            (NoDefaultULongWithFlagsEnum.NDUWFE_1.First8AndLast2Mask(), new MutableString("NDUWFE_First8AndLast2Mask"))
      , new KeyValuePair<NoDefaultULongWithFlagsEnum?, ICharSequence?>
            (NoDefaultULongWithFlagsEnum.NDUWFE_1.First8AndLast2MaskPlusUnnamed(), new CharArrayStringBuilder("NDUWFE_First8AndLast2MaskPlusUnnamed"))
      , new KeyValuePair<NoDefaultULongWithFlagsEnum?, ICharSequence?>
            (NoDefaultULongWithFlagsEnum.NDUWFE_1.Default(), new CharArrayStringBuilder("Zero chance"))
      , new KeyValuePair<NoDefaultULongWithFlagsEnum?, ICharSequence?>(NoDefaultULongWithFlagsEnum.NDUWFE_2, new MutableString("NDLWFE_1"))
      , new KeyValuePair<NoDefaultULongWithFlagsEnum?, ICharSequence?>
            (NoDefaultULongWithFlagsEnum.NDUWFE_13, new CharArrayStringBuilder("NDLWFE_Lucky_Number"))
      , new KeyValuePair<NoDefaultULongWithFlagsEnum?, ICharSequence?>(NoDefaultULongWithFlagsEnum.NDUWFE_3, null!)
    };

    // 2. b) With Default With Flags
    public static readonly Dictionary<WithDefaultLongWithFlagsEnum, MyOtherTypeClass> EnumLongWdWfDateTimeMap = new()
    {
        { WithDefaultLongWithFlagsEnum.WDLWFE_4, new MyOtherTypeClass("WDLWFE_4_Via_ToString") }
      , { WithDefaultLongWithFlagsEnum.WDLWFE_34, new MyOtherTypeClass("WDLWFE_34_Via_ToString") }
      , { WithDefaultLongWithFlagsEnum.WDLWFE_1.First8MinusFlag2Mask(), new MyOtherTypeClass("WDLWFE_First8MinusFlag2Mask") }
      , { WithDefaultLongWithFlagsEnum.WDLWFE_1.First8AndLast2Mask(), new MyOtherTypeClass("WDLWFE_First8AndLast2Mask") }
      , { WithDefaultLongWithFlagsEnum.WDLWFE_1.First8AndLast2MaskPlusUnnamed(), new MyOtherTypeClass("WDLWFE_First8AndLast2MaskPlusUnnamed") }
      , { WithDefaultLongWithFlagsEnum.Default, new MyOtherTypeClass("Zero chance") }
      , { WithDefaultLongWithFlagsEnum.WDLWFE_1, new MyOtherTypeClass("WDLWFE_1_Via_ToString") }
      , { WithDefaultLongWithFlagsEnum.WDLWFE_13, new MyOtherTypeClass("WDLWFE_13_Via_ToString") }
      , { WithDefaultLongWithFlagsEnum.WDLWFE_2, new MyOtherTypeClass("WDLWFE_2_Via_ToString") }
    };

    public static readonly List<KeyValuePair<WithDefaultLongWithFlagsEnum?, MyOtherTypeClass?>> NullEnumLongWdWfNullStringMap = new()
    {
        new KeyValuePair<WithDefaultLongWithFlagsEnum?, MyOtherTypeClass?>(WithDefaultLongWithFlagsEnum.WDLWFE_4, new MyOtherTypeClass(""))
      , new KeyValuePair<WithDefaultLongWithFlagsEnum?, MyOtherTypeClass?>(null, new MyOtherTypeClass("WDLWFE_34_MapValue"))
      , new KeyValuePair<WithDefaultLongWithFlagsEnum?, MyOtherTypeClass?>
            (WithDefaultLongWithFlagsEnum.WDLWFE_1.First8MinusFlag2Mask(), new MyOtherTypeClass("WDLWFE_First8MinusFlag2Mask"))
      , new KeyValuePair<WithDefaultLongWithFlagsEnum?, MyOtherTypeClass?>
            (WithDefaultLongWithFlagsEnum.WDLWFE_1.First8AndLast2Mask(), new MyOtherTypeClass("WDLWFE_First8AndLast2Mask"))
      , new KeyValuePair<WithDefaultLongWithFlagsEnum?, MyOtherTypeClass?>
            (WithDefaultLongWithFlagsEnum.WDLWFE_1.First8AndLast2MaskPlusUnnamed(), new MyOtherTypeClass("WDLWFE_First8AndLast2MaskPlusUnnamed"))
      , new KeyValuePair<WithDefaultLongWithFlagsEnum?, MyOtherTypeClass?>
            (WithDefaultLongWithFlagsEnum.Default, new MyOtherTypeClass("Zero chance"))
      , new KeyValuePair<WithDefaultLongWithFlagsEnum?, MyOtherTypeClass?>
            (WithDefaultLongWithFlagsEnum.WDLWFE_13, new MyOtherTypeClass("WDLWFE_Lucky_Number"))
      , new KeyValuePair<WithDefaultLongWithFlagsEnum?, MyOtherTypeClass?>(null, null)
    };

    public static readonly Dictionary<WithDefaultULongWithFlagsEnum, MySpanFormattableStruct> EnumULongWdWfStringBuilderMap = new()
    {
        { WithDefaultULongWithFlagsEnum.WDUWFE_4, new MySpanFormattableStruct() }
      , { WithDefaultULongWithFlagsEnum.WDUWFE_34, new MySpanFormattableStruct("WDUWFE_34_Value") }
      , { WithDefaultULongWithFlagsEnum.WDUWFE_1.First8MinusFlag2Mask(), new MySpanFormattableStruct("WDUWFE_First8MinusFlag2Mask") }
      , { WithDefaultULongWithFlagsEnum.WDUWFE_1.First8AndLast2Mask(), new MySpanFormattableStruct("WDUWFE_First8AndLast2Mask") }
       ,
        {
            WithDefaultULongWithFlagsEnum.WDUWFE_1.First8AndLast2MaskPlusUnnamed()
          , new MySpanFormattableStruct("WDUWFE_First8AndLast2MaskPlusUnnamed")
        }
      , { WithDefaultULongWithFlagsEnum.Default, new MySpanFormattableStruct("Zero chance") }
      , { WithDefaultULongWithFlagsEnum.WDUWFE_1, new MySpanFormattableStruct("One is the loneliest number that you'll ever do") }
      , { WithDefaultULongWithFlagsEnum.WDUWFE_2, new MySpanFormattableStruct("Two can be as bad as one") }
      , { WithDefaultULongWithFlagsEnum.WDUWFE_3, default }
    };

    public static readonly List<KeyValuePair<WithDefaultULongWithFlagsEnum?, MySpanFormattableStruct?>> NullEnumULongWdWfNullStringBuilderMap = new()
    {
        new KeyValuePair<WithDefaultULongWithFlagsEnum?, MySpanFormattableStruct?>
            (WithDefaultULongWithFlagsEnum.WDUWFE_4, new MySpanFormattableStruct())
      , new KeyValuePair<WithDefaultULongWithFlagsEnum?, MySpanFormattableStruct?>
            (null , new MySpanFormattableStruct("\"Your contract is null AND void\", apparently not the same thing or why say both."))
      , new KeyValuePair<WithDefaultULongWithFlagsEnum?, MySpanFormattableStruct?>
            (WithDefaultULongWithFlagsEnum.WDUWFE_1.First8MinusFlag2Mask(), new MySpanFormattableStruct("WDUWFE_First8MinusFlag2Mask"))
      , new KeyValuePair<WithDefaultULongWithFlagsEnum?, MySpanFormattableStruct?>
            (WithDefaultULongWithFlagsEnum.WDUWFE_1.First8AndLast2Mask(), new MySpanFormattableStruct("WDUWFE_First8AndLast2Mask"))
      , new KeyValuePair<WithDefaultULongWithFlagsEnum?, MySpanFormattableStruct?>
            (WithDefaultULongWithFlagsEnum.WDUWFE_1.First8AndLast2MaskPlusUnnamed() 
           , new MySpanFormattableStruct("WDUWFE_First8AndLast2MaskPlusUnnamed"))
      , new KeyValuePair<WithDefaultULongWithFlagsEnum?, MySpanFormattableStruct?>
            (WithDefaultULongWithFlagsEnum.Default, new MySpanFormattableStruct("Zero chance"))
      , new KeyValuePair<WithDefaultULongWithFlagsEnum?, MySpanFormattableStruct?>(WithDefaultULongWithFlagsEnum.WDUWFE_1, null)
      , new KeyValuePair<WithDefaultULongWithFlagsEnum?, MySpanFormattableStruct?>
            (WithDefaultULongWithFlagsEnum.WDUWFE_13, new MySpanFormattableStruct("WDUWFE_Lucky_Number"))
      , new KeyValuePair<WithDefaultULongWithFlagsEnum?, MySpanFormattableStruct?>(null, null)
    };
}
