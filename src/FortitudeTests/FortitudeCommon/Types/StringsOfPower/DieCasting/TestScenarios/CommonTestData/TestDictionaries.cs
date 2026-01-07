// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Net;
using System.Numerics;
using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.UnitField.FixtureScaffolding;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ValueTypeScaffolds;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification.CollectionItemResult;

// ReSharper disable UseUtf8StringLiteral
// ReSharper disable FormatStringProblem
// ReSharper disable InconsistentNaming

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Scenarios.CompareToSystemTextJson.TypePermutation;

public class TestDictionaries
{
    public static readonly Dictionary<bool, int> BoolIntMap = new()
    {
        { true, 1 }
      , { false, 0 }
    };

    public static readonly KeyValuePredicate<bool, int> BoolIntMap_First_1 = (count, _, _) => StopOnFirstExclusion(count <= 1);

    public static readonly KeyValuePredicate<bool, int> BoolIntMap_Second_1 = (count, _, _) =>
        BetweenRetrieveRange(count, 2, 31);

    public static readonly KeyValuePredicate<bool, int> BoolIntMap_First_FalseKey = (_, key, _) => First(!key);

    public static readonly List<bool> Bool_True_SubList = [true];

    public static readonly List<bool> Bool_False_SubList = [false];

    public static readonly PalantírReveal<bool> Bool_Reveal          = (b, tos) => tos.StartSimpleContentType(b).AsValue(b);
    public static readonly PalantírReveal<bool> Bool_Reveal_AsString = (b, tos) => tos.StartSimpleContentType(b).AsString(b);
    public static readonly PalantírReveal<bool> Bool_OneChar_Reveal  = (b, tos) => tos.StartSimpleContentType(b).AsValue(b, "{0[..1]}");

    public static readonly PalantírReveal<int> Int_Money_Reveal          = (i, tos) => tos.StartSimpleContentType(i).AsValue(i, "{0:C2}");
    public static readonly PalantírReveal<int> Int_NegativeString_Reveal = (i, tos) => tos.StartSimpleContentType(i).AsValue(-i, "\"{0}\"");

    public static readonly List<KeyValuePair<bool, int?>> BoolNullIntKvpList =
        [new KeyValuePair<bool, int?>(true, 1), new KeyValuePair<bool, int?>(false, null)];

    public static readonly List<KeyValuePair<bool?, int?>> NullBoolNullIntKvpList =
        [new KeyValuePair<bool?, int?>(null, 0), new KeyValuePair<bool?, int?>(true, 1), new KeyValuePair<bool?, int?>(false, null)];

    public static readonly KeyValuePredicate<bool?, int?> NullBoolIntMap_First_1 = (count, _, _) =>
        StopOnFirstExclusion(count <= 1);

    public static readonly KeyValuePredicate<bool?, int?> NullBoolIntMap_Second_1 = (count, _, _) =>
        BetweenRetrieveRange(count, 2, 3);

    public static readonly KeyValuePredicate<bool?, int?> NullBoolIntMap_KeyNonNull = (_, key, _) =>
        EvaluateIsIncludedAndContinue(key != null);

    public static readonly List<bool> Bool_TrueFalse_SubList = [true, false];


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

    public static readonly KeyValuePredicate<double, ICharSequence> DoubleCharSequenceMap_First_4 = (count, _, _) =>
        StopOnFirstExclusion(count <= 4);

    public static readonly KeyValuePredicate<double, ICharSequence> DoubleCharSequenceMap_Second_4 = (count, _, _) =>
        BetweenRetrieveRange(count, 5, 9);

    public static readonly KeyValuePredicate<double, ICharSequence> DoubleCharSequenceMap_First_2 = (count, _, _) =>
        StopOnFirstExclusion(count <= 2);

    public static readonly KeyValuePredicate<double, ICharSequence> DoubleCharSequenceMap_Skip_Odd_Index = (_, _, _) =>
        EvaluateIsIncludedAndContinue(true, 1);

    public static KeyValuePredicate<double, ICharSequence> DoubleCharSequenceMap_FirstKey_Lt_5 = (_, key, _) => First(key < 5.0f);

    public static readonly List<double> Double_First_SubList = [Math.PI];

    public static readonly List<double> Double_First_4_SubList = [Math.PI, 2 * Math.PI, Math.E, 2 * Math.E];

    public static readonly List<double> Double_Second_4_SubList = [Math.PI * Math.E, 1, -1, 100];

    public static readonly PalantírReveal<double> Double_Reveal            = (d, tos) => tos.StartSimpleContentType(d).AsValue(d);
    public static readonly PalantírReveal<double> Double_Reveal_1Dp        = (d, tos) => tos.StartSimpleContentType(d).AsValue(d, "N1");
    public static readonly PalantírReveal<double> Double_Reveal_Pad17      = (d, tos) => tos.StartSimpleContentType(d).AsValue(d, "{0,17}");
    public static readonly PalantírReveal<double> Double_Reveal_PadMinus17 = (d, tos) => tos.StartSimpleContentType(d).AsValue(d, "{0,-17}");

    public static readonly PalantírReveal<ICharSequence> CharSequenceMap_10Chars     = (cs, tos) => tos.StartSimpleContentType(cs).AsString(cs, "{0[..10]}");
    public static readonly PalantírReveal<ICharSequence> CharSequenceMap_Pad50       = (cs, tos) => tos.StartSimpleContentType(cs).AsString(cs, "{0,-50}");
    public static readonly PalantírReveal<ICharSequence> CharSequenceMap_Last10Chars = (cs, tos) => tos.StartSimpleContentType(cs).AsString(cs, "{0[^10..]}");

    public static readonly List<KeyValuePair<double, ICharSequence?>> DoubleNullCharSequence =
    [
        new (Math.PI, new MutableString("Eating the crust edges of one pie means you have eaten the length of two pi"))
      , new (Math.E, new CharArrayStringBuilder("One doesn't simply write Euler nature number."))
      , new (0, null)
      , new (1, new CharArrayStringBuilder("All for one and one for all."))
      , new (3.333, new CharArrayStringBuilder("\"Your contract is null AND void\", apparently not the same thing or why say both."))
      , new (-1, null)
    ];

    public static readonly List<KeyValuePair<double?, ICharSequence?>> NullDoubleNullCharSequence =
    [
        new (null, null)
      , new (Math.PI, new MutableString("Eating the crust edges of one pie means you have eaten the length of two pi"))
      , new (Math.E, new CharArrayStringBuilder("One doesn't simply write Euler nature number."))
      , new (1, new CharArrayStringBuilder("All for one and one for all."))
      , new (null, new CharArrayStringBuilder("\"Your contract is null AND void\", apparently not the same thing or why say both."))
      , new (-1, new CharArrayStringBuilder("Imagine there's no tax havens, it's easy if you try"))
    ];

    public static readonly KeyValuePredicate<double?, ICharSequence?> NullDoubleCharSequenceMap_First_4 = (count, _, _) =>
        StopOnFirstExclusion(count <= 4);

    public static readonly KeyValuePredicate<double?, ICharSequence?> NullDoubleCharSequenceMap_Second_4 = (count, _, _) =>
        BetweenRetrieveRange(count, 5, 9);

    public static readonly KeyValuePredicate<double?, ICharSequence?> NullDoubleCharSequenceMap_First_2 = (count, _, _) =>
        StopOnFirstExclusion(count <= 2);

    public static readonly KeyValuePredicate<double?, ICharSequence?> NullDoubleCharSequenceMap_Skip_Odd_Index = (_, _, _) =>
        EvaluateIsIncludedAndContinue(true, 1);

    public static readonly KeyValuePredicate<double?, ICharSequence?> NullDoubleCharSequenceMap_FirstKey_Lt_5 = (_, key, _) => First(key < 5.0f);

    public static readonly List<double> NullDouble_NullOrNeg1_SubList = [-1, 3.333, 1, 0, Math.E, Math.PI];

    public static KeyValuePredicate<double?, ICharSequence?>
        NullDoubleNullCharSequenceMap_First_2 = (count, _, _) => StopOnFirstExclusion(count <= 2);

    public static KeyValuePredicate<double?, ICharSequence?>
        NullDoubleNullCharSequenceMap_First_5 = (count, _, _) => StopOnFirstExclusion(count <= 5);

    public static KeyValuePredicate<double?, ICharSequence?> NullDoubleNullCharSequenceMap_Second_5
        = (count, _, _) => BetweenRetrieveRange(count, 6, 11);

    public static KeyValuePredicate<double?, ICharSequence?> NullDoubleNullCharSequenceMap_Skip_Odd_Index
        = (_, _, _) => EvaluateIsIncludedAndContinue(true, 1);

    public static KeyValuePredicate<double?, ICharSequence?> NullDoubleNullCharSequenceMap_FirstKey_Lt_5 = (_, key, _) => First(key is < 5.0f);


    public static readonly Dictionary<UInt128, BigInteger> VeryULongBigIntegerMap = new()
    {
        { UInt128.MinValue, BigInteger.Zero }
      , { UInt128.MaxValue / 2, UInt128.MaxValue / 2 }
      , { UInt128.One, BigInteger.One }
      , { UInt128.MaxValue / 3, UInt128.MaxValue / 3 }
      , { UInt128.MaxValue / 4, UInt128.MaxValue / 4 }
      , { UInt128.MaxValue, UInt128.MaxValue }
    };

    public static readonly List<UInt128> VeryULong_First_3_SubList = [UInt128.MinValue, UInt128.MaxValue / 2, UInt128.One];

    public static readonly List<UInt128> VeryULong_Second_3_SubList = [UInt128.MaxValue / 3, UInt128.MaxValue / 4, UInt128.MaxValue];

    public static readonly KeyValuePredicate<UInt128, BigInteger> VeryULongBigInteger_First_3 = (count, _, _) =>
        StopOnFirstExclusion(count <= 3);

    public static readonly List<KeyValuePair<UInt128, BigInteger?>> NullVeryULongBigIntegerMap =
    [
        new (UInt128.MinValue, BigInteger.Zero)
      , new (UInt128.MaxValue / 2, null)
      , new (UInt128.One, BigInteger.One)
      , new (UInt128.MaxValue / 3, UInt128.MaxValue / 3)
      , new (UInt128.MaxValue / 4, null)
      , new (UInt128.MaxValue, UInt128.MaxValue)
    ];
    
    public static readonly PalantírReveal<UInt128> UInt128_Reveal_SglQt = (ui, tos) => tos.StartSimpleContentType(ui).AsValue(ui, "'{0}'");
    public static readonly PalantírReveal<UInt128> UInt128_Reveal_DblQtPadMinus45 = (ui, tos) => tos.StartSimpleContentType(ui).AsValue(ui, "\"{0,-45}\"");
    
    public static readonly PalantírReveal<BigInteger> BigInteger_Reveal_Negative = (bi, tos) => tos.StartSimpleContentType(bi).AsValue(-bi);
    public static readonly PalantírReveal<BigInteger> BigInteger_Reveal_Pad45 = (bi, tos) => tos.StartSimpleContentType(bi).AsValue(bi, "{0,45}");
    public static readonly PalantírReveal<BigInteger> BigInteger_DblQt_Pad4 = (bi, tos) => tos.StartSimpleContentType(bi).AsValue(bi, "\"{0,4}\"");
    public static readonly PalantírReveal<BigInteger> BigInteger_Separators = (bi, tos) => tos.StartSimpleContentType(bi).AsValue(bi, "{0:###,##0.0}");
    

    public static readonly KeyValuePredicate<UInt128, BigInteger?> NullVeryULongBigInteger_First_3 = (count, _, _) =>
        StopOnFirstExclusion(count <= 3);

    public static readonly KeyValuePredicate<UInt128, BigInteger?> NullVeryULongBigInteger_Second_3 = (count, _, _) =>
        BetweenRetrieveRange(count, 4, 7);


    public static readonly Dictionary<TimeSpan, char> TimeSpanCharMap = new()
    {
        { TimeSpan.Zero, 'z' }
      , { TimeSpan.FromDays(1), 'd' }
      , { TimeSpan.FromHours(1), 'h' }
      , { TimeSpan.FromMinutes(1), 'm' }
      , { TimeSpan.FromSeconds(1), 's' }
      , { TimeSpan.FromMilliseconds(1), 'f' }
    };

    public static readonly List<KeyValuePair<TimeSpan?, char?>> NullTimeSpanCharMap =
    [
        new (null, null)
      , new (null, '\0')
      , new (TimeSpan.Zero, 'z')
      , new (TimeSpan.FromDays(1), 'd')
      , new (TimeSpan.FromHours(1), 'h')
      , new (TimeSpan.FromMinutes(1), 'm')
    ];

    public static readonly Dictionary<int, MySpanFormattableStruct> IntMySpanFormattableStructMap = new()
    {
        { 0, new MySpanFormattableStruct("MySpanFormattableStruct.ToString() => 0 ") }
      , { 3600 * 24, new MySpanFormattableStruct("MySpanFormattableStruct.ToString() => 1d ") }
      , { -1, default }
      , { 60, new MySpanFormattableStruct("MySpanFormattableStruct.ToString() => 1m") }
    };

    public static readonly List<KeyValuePair<int?, MySpanFormattableStruct?>> NullIntNullMySpanFormattableStructMap =
    [
        new (null, null)
      , new (0, new MySpanFormattableStruct("MySpanFormattableStruct.ToString() => \"0\" "))
      , new (3600 * 24, new MySpanFormattableStruct("MySpanFormattableStruct.ToString() => \"1d\" "))
      , new (-1, null)
      , new (60, new MySpanFormattableStruct("MySpanFormattableStruct.ToString() => \"1m\""))
    ];

    public static readonly Dictionary<IPAddress, Uri> IPAddressUriMap = new()
    {
        { new IPAddress([0, 0, 0, 0]), new Uri("http://first-null.com") }
      , { new IPAddress([127, 0, 0, 1]), new Uri("tcp://localhost") }
      , { new IPAddress([255, 255, 255, 255]), new Uri("http://unknown.com") }
      , { new IPAddress([192, 168, 1, 1]), new Uri("tcp://default-gateway") }
    };

    public static readonly KeyValuePredicate<IPAddress, Uri> IPAddressUri_First_10 = (count, _, _) =>
        StopOnFirstExclusion(count <= 10);

    public static readonly KeyValuePredicate<IPAddress, Uri> IPAddressUri_First_3 = (count, _, _) =>
        StopOnFirstExclusion(count <= 3);

    public static readonly KeyValuePredicate<IPAddress, Uri> IPAddressUri_Second_3 = (count, _, _) =>
        BetweenRetrieveRange(count, 4, 7);
    
    public static readonly PalantírReveal<IPAddress>        IPAddress_Reveal_Pad18     = (u, tos) => tos.StartSimpleContentType(u).AsValue(u, "{0,18}");
    
    public static readonly PalantírReveal<Uri>        Uri_Reveal_RightArrow     = (u, tos) => tos.StartSimpleContentType(u).AsValue(u, "==> {0}");

    public static readonly List<KeyValuePair<IPAddress, Uri?>> IPAddressNullUriMap =
    [
        new (new IPAddress([0, 0, 0, 0]), null)
      , new (new IPAddress([127, 0, 0, 1]), new Uri("tcp://localhost"))
      , new (new IPAddress([192, 168, 1, 1]), new Uri("tcp://default-gateway"))
      , new (new IPAddress([255, 255, 255, 255]), null)
    ];

    public static readonly List<KeyValuePair<IPAddress?, Uri?>> NullIPAddressUriMap =
    [
        new (new IPAddress([0, 0, 0, 0]), new Uri("http://first-null.com"))
      , new (new IPAddress([127, 0, 0, 1]), new Uri("tcp://localhost"))
      , new (new IPAddress([192, 168, 1, 1]), new Uri("tcp://default-gateway"))
      , new (new IPAddress([255, 255, 255, 255]), null), new KeyValuePair<IPAddress?, Uri?>(null, null)
    ];

    public static readonly KeyValuePredicate<IPAddress?, Uri?> NullIPAddressUri_First_10 = (count, _, _) =>
        StopOnFirstExclusion(count <= 10);

    public static readonly KeyValuePredicate<IPAddress?, Uri?> NullIPAddressUri_First_3 = (count, _, _) =>
        StopOnFirstExclusion(count <= 3);

    public static readonly KeyValuePredicate<IPAddress?, Uri?> NullIPAddressUri_Second_3 = (count, _, _) =>
        BetweenRetrieveRange(count, 4, 7);

    public static readonly Dictionary<MySpanFormattableStruct, MySpanFormattableClass> MySpanFormattableStructClassMap = new()
    {
        { new MySpanFormattableStruct("First_SpanStruct"), new MySpanFormattableClass("First_SpanClass") }
      , { new MySpanFormattableStruct("Second_SpanStruct"), new MySpanFormattableClass("Second_SpanClass") }
      , { new MySpanFormattableStruct("Third_SpanStruct"), new MySpanFormattableClass("Third_SpanClass") }
      , { new MySpanFormattableStruct("Fourth_SpanStruct"), new MySpanFormattableClass("Fourth_SpanClass") }
      , { new MySpanFormattableStruct("Fifth_SpanStruct"), new MySpanFormattableClass("Fifth_SpanClass") }
      , { new MySpanFormattableStruct("Sixth_SpanStruct"), new MySpanFormattableClass("Sixth_SpanClass") }
    };

    public static readonly KeyValuePredicate<MySpanFormattableStruct, MySpanFormattableClass> MySpanFormattableStructClass_First_10 = (count, _, _) =>
        StopOnFirstExclusion(count <= 10);

    public static readonly KeyValuePredicate<MySpanFormattableStruct, MySpanFormattableClass> MySpanFormattableStructClass_First_3 = (count, _, _) =>
        StopOnFirstExclusion(count <= 3);

    public static readonly KeyValuePredicate<MySpanFormattableStruct, MySpanFormattableClass> MySpanFormattableStructClass_Second_3 = (count, _, _) =>
        BetweenRetrieveRange(count, 4, 7);

    public static readonly List<MySpanFormattableStruct> MySpanFormattableStruct_First_3_SubList = 
        [
            new ("First_SpanStruct")
          , new ("Second_SpanStruct")
          , new ("Third_SpanStruct")
        ];

    public static readonly List<MySpanFormattableStruct> MySpanFormattableStruct_Second_3_SubList = 
        [
            new ("Fourth_SpanStruct")
          , new ("Fifth_SpanStruct")
          , new ("Sixth_SpanStruct")
        ];
    
    public static readonly PalantírReveal<MySpanFormattableClass>        MySpanFormattableClass_Reveal_PadMinus20     = 
        (msfc, tos) => tos.StartSimpleContentType(msfc).AsValue(msfc, "{0,-20}");
    
    public static readonly PalantírReveal<MySpanFormattableClass>        MySpanFormattableClass_Reveal_Pad20     = 
        (msfc, tos) => tos.StartSimpleContentType(msfc).AsValue(msfc, "{0,20}");

    public static readonly List<KeyValuePair<MySpanFormattableStruct, MySpanFormattableClass?>> MySpanFormattableStructNullClassMap =
    [
        new (new MySpanFormattableStruct("First_SpanStruct"), new MySpanFormattableClass("First_SpanClass"))
      , new (new MySpanFormattableStruct("Second_SpanStruct"), null)
      , new (new MySpanFormattableStruct("Third_SpanStruct"), new MySpanFormattableClass("Third_SpanClass"))
      , new (new MySpanFormattableStruct("Fourth_SpanStruct"), null)
      , new (new MySpanFormattableStruct("Fifth_SpanStruct"), null)
    ];

    public static readonly List<KeyValuePair<MySpanFormattableStruct?, MySpanFormattableClass?>> NullMySpanFormattableStructClassMap=
    [
        new (new MySpanFormattableStruct("First_SpanStruct"), new MySpanFormattableClass("First_SpanClass"))
      , new (null, new MySpanFormattableClass("Second_SpanClass"))
      , new (new MySpanFormattableStruct("Third_SpanStruct"), new MySpanFormattableClass("Third_SpanClass"))
      , new (new MySpanFormattableStruct("Fourth_SpanStruct"), null)
      , new (null, null)
    ];

    public static readonly KeyValuePredicate<MySpanFormattableStruct?, MySpanFormattableClass?> NullMySpanFormattableStructClass_First_10 = (count, _, _) =>
        StopOnFirstExclusion(count <= 10);

    public static readonly KeyValuePredicate<MySpanFormattableStruct?, MySpanFormattableClass?> NullMySpanFormattableStructClass_First_3 = (count, _, _) =>
        StopOnFirstExclusion(count <= 3);

    public static readonly KeyValuePredicate<MySpanFormattableStruct?, MySpanFormattableClass?> NullMySpanFormattableStructClass_Second_3 = (count, _, _) =>
        BetweenRetrieveRange(count, 4, 7);
    
    public static readonly PalantírReveal<MySpanFormattableStruct>        MySpanFormattableStruct_Reveal_PadMinus20     = 
        (msfc, tos) => tos.StartSimpleContentType(msfc).AsValue(msfc, "{0,-20}");
    
    public static readonly PalantírReveal<MySpanFormattableStruct>        MySpanFormattableStruct_Reveal_Pad20     = 
        (msfc, tos) => tos.StartSimpleContentType(msfc).AsValue(msfc, "{0,20}");

    public static readonly Dictionary<MySpanFormattableClass, MySpanFormattableStruct> MySpanFormattableClassStructMap = new()
    {
        { new MySpanFormattableClass("First_SpanClass"), new MySpanFormattableStruct("First_SpanStruct") }
      , { new MySpanFormattableClass("Second_SpanClass"), new MySpanFormattableStruct("Second_SpanStruct") }
      , { new MySpanFormattableClass("Third_SpanClass"), new MySpanFormattableStruct("Third_SpanStruct") }
      , { new MySpanFormattableClass("Fourth_SpanClass"), new MySpanFormattableStruct("Fourth_SpanStruct") }
      , { new MySpanFormattableClass("Fifth_SpanClass"), new MySpanFormattableStruct("Fifth_SpanStruct") }
    };

    public static readonly KeyValuePredicate<MySpanFormattableClass, MySpanFormattableStruct> MySpanFormattableClassStruct_First_10 = (count, _, _) =>
        StopOnFirstExclusion(count <= 10);

    public static readonly KeyValuePredicate<MySpanFormattableClass, MySpanFormattableStruct> MySpanFormattableClassStruct_First_3 = (count, _, _) =>
        StopOnFirstExclusion(count <= 3);

    public static readonly KeyValuePredicate<MySpanFormattableClass, MySpanFormattableStruct> MySpanFormattableClassStruct_Second_3 = (count, _, _) =>
        BetweenRetrieveRange(count, 4, 7);

    public static readonly List<MySpanFormattableClass> MySpanFormattableClass_First_3_SubList = 
    [
        new ("First_SpanClass")
      , new ("Second_SpanClass")
      , new ("Third_SpanClass")
    ];

    public static readonly List<MySpanFormattableClass> MySpanFormattableClass_Second_3_SubList = 
    [
        new ("Fourth_SpanClass")
      , new ("Fifth_SpanClass")
    ];

    public static readonly List<KeyValuePair<MySpanFormattableClass, MySpanFormattableStruct?>> MySpanFormattableNullClassStructMap =
    [
        new (new MySpanFormattableClass("First_SpanClass"), new MySpanFormattableStruct("First_SpanStruct"))
      , new (new MySpanFormattableClass("Second_SpanClass"), null)
      , new (new MySpanFormattableClass("Third_SpanClass") , null)
      , new (new MySpanFormattableClass("Fourth_SpanClass"), null)
      , new (new MySpanFormattableClass("Fifth_SpanClass"), new MySpanFormattableStruct("Fifth_SpanStruct"))
    ];

    public static readonly List<KeyValuePair<MySpanFormattableClass?, MySpanFormattableStruct?>> NullMySpanFormattableClassStructMap =
    [
        new (new MySpanFormattableClass("First_SpanClass"), new MySpanFormattableStruct("First_SpanStruct"))
      , new (null, new MySpanFormattableStruct("Second_SpanStruct"))
      , new (new MySpanFormattableClass("Third_SpanClass"), new MySpanFormattableStruct("Third_SpanStruct"))
      , new (new MySpanFormattableClass("Fourth_SpanClass"), null)
      , new (null, null)
    ];

    public static readonly KeyValuePredicate<MySpanFormattableClass?, MySpanFormattableStruct?> NullMySpanFormattableClassStruct_First_10 = (count, _, _) =>
        StopOnFirstExclusion(count <= 10);

    public static readonly KeyValuePredicate<MySpanFormattableClass?, MySpanFormattableStruct?> NullMySpanFormattableClassStruct_First_3 = (count, _, _) =>
        StopOnFirstExclusion(count <= 3);

    public static readonly KeyValuePredicate<MySpanFormattableClass?, MySpanFormattableStruct?> NullMySpanFormattableClassStruct_Second_3 = (count, _, _) =>
        BetweenRetrieveRange(count, 4, 7);

    public static readonly List<MySpanFormattableClass> MySpanFormattableNullClass_First_3_SubList = 
    [
        new ("First_SpanClass")
      , new ("Second_SpanStruct")
      , new ("Third_SpanClass")
    ];

    public static readonly List<MySpanFormattableClass> MySpanFormattableNullClass_Second_3_SubList = 
    [
        new ("Fourth_SpanClass")
      , new ("Fifth_SpanClass")
    ];


    public static readonly Dictionary<ComplexStructContentAsValueSpanFormattable<decimal>
      , FieldSpanFormattableAlwaysAddStructStringBearer<Uri>> StructBearerToComplexBearerMap = new()
    {
        {
            new ComplexStructContentAsValueSpanFormattable<decimal>((decimal)Math.PI)
          , new FieldSpanFormattableAlwaysAddStructStringBearer<Uri>(new Uri("http://first-value.com"))
        }
       ,
        {
            new ComplexStructContentAsValueSpanFormattable<decimal>((decimal)Math.E)
          , new FieldSpanFormattableAlwaysAddStructStringBearer<Uri>(new Uri("http://second-value.com"))
        }
       ,
        {
            new ComplexStructContentAsValueSpanFormattable<decimal>((decimal)Math.PI * 10)
          , new FieldSpanFormattableAlwaysAddStructStringBearer<Uri>(new Uri("http://third-value.com"))
        }
       ,
        {
            new ComplexStructContentAsValueSpanFormattable<decimal>((decimal)Math.E * 10)
          , new FieldSpanFormattableAlwaysAddStructStringBearer<Uri>(new Uri("http://fourth-value.com"))
        }
    };

    public static KeyValuePredicate<ComplexStructContentAsValueSpanFormattable<decimal>
      , FieldSpanFormattableAlwaysAddStructStringBearer<Uri>> StructBearerToComplexBearer_First_10 = (count, _, _) =>
        StopOnFirstExclusion(count <= 10);

    public static readonly KeyValuePredicate<ComplexStructContentAsValueSpanFormattable<decimal>
      , FieldSpanFormattableAlwaysAddStructStringBearer<Uri>> StructBearerToComplexBearer_First_3 = (count, _, _) =>
        StopOnFirstExclusion(count <= 3);

    public static KeyValuePredicate<ComplexStructContentAsValueSpanFormattable<decimal>
      , FieldSpanFormattableAlwaysAddStructStringBearer<Uri>> StructBearerToComplexBearer_Second_3 = (count, _, _) =>
        BetweenRetrieveRange(count, 4, 7);

    public static readonly List<ComplexStructContentAsValueSpanFormattable<decimal>> StructBearer_First_3_SubList = 
    [
        new ((decimal)Math.PI)
      , new ((decimal)Math.E)
      , new ((decimal)Math.PI * 10)
    ];

    public static readonly List<ComplexStructContentAsValueSpanFormattable<decimal>> StructBearer_Second_3_SubList = 
    [
        new ((decimal)Math.E * 10)
    ];
    
    public static readonly PalantírReveal<ComplexStructContentAsValueSpanFormattable<decimal>> StructBearerDecimal_Reveal_N3     = 
        (msfc, tos) => tos.StartSimpleContentType(msfc).RevealAsValue(msfc, "N3");
    
    public static readonly PalantírReveal<FieldSpanFormattableAlwaysAddStructStringBearer<Uri>>        StructBearer_Reveal_Pad30     = 
        (msfc, tos) => tos.StartSimpleContentType(msfc).RevealAsValue(msfc, "{0,30}");

    public static readonly List<KeyValuePair<ComplexStructContentAsValueSpanFormattable<decimal>
      , FieldSpanFormattableAlwaysAddStructStringBearer<Uri>?>> StructBearerToNullComplexStructBearerMap =
    [
        new ( new ComplexStructContentAsValueSpanFormattable<decimal>((decimal)Math.PI),
             new FieldSpanFormattableAlwaysAddStructStringBearer<Uri>(new Uri("http://first-value.com")))
      , new ( new ComplexStructContentAsValueSpanFormattable<decimal>((decimal)Math.E)
           , new FieldSpanFormattableAlwaysAddStructStringBearer<Uri>(new Uri("http://second-value.com")))
      , new ( new ComplexStructContentAsValueSpanFormattable<decimal>((decimal)Math.PI * 10)
           , new FieldSpanFormattableAlwaysAddStructStringBearer<Uri>(new Uri("http://third-value.com")))
      , new ( new ComplexStructContentAsValueSpanFormattable<decimal>((decimal)Math.E * 10), null )
    ];

    public static readonly List<KeyValuePair<ComplexStructContentAsValueSpanFormattable<decimal>?
      , FieldSpanFormattableAlwaysAddStructStringBearer<Uri>?>> NullStructBearerToComplexBearerMap =
    [
        new (null, new FieldSpanFormattableAlwaysAddStructStringBearer<Uri>(new Uri("http://first-value.com")))
      , new ( new ComplexStructContentAsValueSpanFormattable<decimal>((decimal)Math.E)
           , new FieldSpanFormattableAlwaysAddStructStringBearer<Uri>(new Uri("http://second-value.com")))
      , new ( new ComplexStructContentAsValueSpanFormattable<decimal>((decimal)Math.PI * 10)
           , new FieldSpanFormattableAlwaysAddStructStringBearer<Uri>(new Uri("http://third-value.com")) )
      , new ( new ComplexStructContentAsValueSpanFormattable<decimal>((decimal)Math.E * 10), null )
      , new (null, null)
    ];

    public static KeyValuePredicate<ComplexStructContentAsValueSpanFormattable<decimal>?,
            FieldSpanFormattableAlwaysAddStructStringBearer<Uri>?>
        NullStructBearerToComplexBearerMap_First_10 = (count, _, _) => StopOnFirstExclusion(count <= 10);

    public static readonly KeyValuePredicate<ComplexStructContentAsValueSpanFormattable<decimal>?,
            FieldSpanFormattableAlwaysAddStructStringBearer<Uri>?>
        NullStructBearerToComplexBearerMap_First_3 = (count, _, _) => StopOnFirstExclusion(count <= 3);

    public static KeyValuePredicate<ComplexStructContentAsValueSpanFormattable<decimal>?,
            FieldSpanFormattableAlwaysAddStructStringBearer<Uri>?>
        NullStructBearerToComplexBearerMap_Second_3 = (count, _, _) => BetweenRetrieveRange(count, 4, 7);

    public static readonly Dictionary<ComplexContentAsValueSpanFormattable<decimal>
      , FieldSpanFormattableAlwaysAddStringBearer<Uri>> ClassBearerToComplexBearerMap = new()
    {
        {
            new ComplexContentAsValueSpanFormattable<decimal>((decimal)Math.PI)
          , new FieldSpanFormattableAlwaysAddStringBearer<Uri>(new Uri("http://first-value.com"))
        }
       ,
        {
            new ComplexContentAsValueSpanFormattable<decimal>((decimal)Math.E)
          , new FieldSpanFormattableAlwaysAddStringBearer<Uri>(new Uri("http://second-value.com"))
        }
       ,
        {
            new ComplexContentAsValueSpanFormattable<decimal>((decimal)Math.PI * 10)
          , new FieldSpanFormattableAlwaysAddStringBearer<Uri>(new Uri("http://third-value.com"))
        }
       ,
        {
            new ComplexContentAsValueSpanFormattable<decimal>((decimal)Math.E * 10)
          , new FieldSpanFormattableAlwaysAddStringBearer<Uri>(new Uri("http://fourth-value.com"))
        }
    };

    public static KeyValuePredicate<ComplexContentAsValueSpanFormattable<decimal>
      , FieldSpanFormattableAlwaysAddStringBearer<Uri>> ClassBearerToComplexBearer_First_10 = (count, _, _) =>
        StopOnFirstExclusion(count <= 10);

    public static readonly KeyValuePredicate<ComplexContentAsValueSpanFormattable<decimal>
      , FieldSpanFormattableAlwaysAddStringBearer<Uri>> ClassBearerToComplexBearer_First_3 = (count, _, _) =>
        StopOnFirstExclusion(count <= 3);

    public static KeyValuePredicate<ComplexContentAsValueSpanFormattable<decimal>
      , FieldSpanFormattableAlwaysAddStringBearer<Uri>> ClassBearerToComplexBearer_Second_3 = (count, _, _) =>
        BetweenRetrieveRange(count, 4, 7);

    public static readonly List<ComplexContentAsValueSpanFormattable<decimal>> ClassBearer_First_3_SubList = 
    [
        new ((decimal)Math.PI)
      , new ((decimal)Math.E)
      , new ((decimal)Math.PI * 10)
    ];

    public static readonly List<ComplexContentAsValueSpanFormattable<decimal>> ClassBearer_Second_3_SubList = 
    [
        new ((decimal)Math.E * 10)
    ];
    
    public static readonly PalantírReveal<ComplexContentAsValueSpanFormattable<decimal>> ClassBearerDecimal_Reveal_N3 = 
        (msfc, tos) => tos.StartSimpleContentType(msfc).RevealAsValue(msfc, "N3");
    
    public static readonly PalantírReveal<FieldSpanFormattableAlwaysAddStringBearer<Uri>> ClassBearer_Reveal_Pad30 = 
        (msfc, tos) => tos.StartSimpleContentType(msfc).RevealAsValue(msfc, "{0,30}");

    public static readonly List<KeyValuePair<ComplexContentAsValueSpanFormattable<decimal>
      , FieldSpanFormattableAlwaysAddStructStringBearer<Uri>?>> ClassBearerToNullStructComplexBearerMap =
    [
        new (new ComplexContentAsValueSpanFormattable<decimal>((decimal)Math.PI), null)
      , new (new ComplexContentAsValueSpanFormattable<decimal>((decimal)Math.E)
           , new FieldSpanFormattableAlwaysAddStructStringBearer<Uri>(new Uri("http://second-value.com")))
      , new (new ComplexContentAsValueSpanFormattable<decimal>((decimal)Math.PI * 10)
           , new FieldSpanFormattableAlwaysAddStructStringBearer<Uri>(new Uri("http://third-value.com")))
      , new (new ComplexContentAsValueSpanFormattable<decimal>((decimal)Math.E * 10), null)
    ];

    public static readonly List<KeyValuePair<ComplexContentAsValueSpanFormattable<decimal>?
      , FieldSpanFormattableAlwaysAddStructStringBearer<Uri>?>> NullClassBearerToComplexBearerMap =
    [
        new ( null, new FieldSpanFormattableAlwaysAddStructStringBearer<Uri>(new Uri("http://first-value.com")) )
      , new ( new ComplexContentAsValueSpanFormattable<decimal>((decimal)Math.E)
           , new FieldSpanFormattableAlwaysAddStructStringBearer<Uri>(new Uri("http://second-value.com")) )
      , new ( new ComplexContentAsValueSpanFormattable<decimal>((decimal)Math.PI * 10)
           , new FieldSpanFormattableAlwaysAddStructStringBearer<Uri>(new Uri("http://third-value.com")) )
      , new ( new ComplexContentAsValueSpanFormattable<decimal>((decimal)Math.E * 10), null )
      , new (null, null)
    ];

    public static KeyValuePredicate<ComplexContentAsValueSpanFormattable<decimal>?,
            FieldSpanFormattableAlwaysAddStructStringBearer<Uri>?>
        NullClassBearerToComplexBearer_First_10 = (count, _, _) => StopOnFirstExclusion(count <= 10);

    public static readonly KeyValuePredicate<ComplexContentAsValueSpanFormattable<decimal>?,
            FieldSpanFormattableAlwaysAddStructStringBearer<Uri>?>
        NullClassBearerToComplexBearer_First_3 = (count, _, _) => StopOnFirstExclusion(count <= 3);

    public static KeyValuePredicate<ComplexContentAsValueSpanFormattable<decimal>?,
            FieldSpanFormattableAlwaysAddStructStringBearer<Uri>?>
        NullClassBearerToComplexBearer_Second_3 = (count, _, _) => BetweenRetrieveRange(count, 4, 7);

    public static readonly Dictionary<string, StringBuilder> StringStringBuilderMap = new()
    {
        { "", new StringBuilder("Empty Value") }
      , { "FirstKey", new StringBuilder("FirstValue") }
      , { "NullKey", null! }
      , { "SecondKey", new StringBuilder("SecondValue") }
    };

    public static readonly List<KeyValuePair<string?, StringBuilder?>> NullStringNullStringBuilderMap =
    [
        new (null, new StringBuilder("null Key Value"))
      , new ("", new StringBuilder("empty key value"))
      , new ("NullValue", null)
      , new ("FirstKey", new StringBuilder("FirstValue")), new KeyValuePair<string?, StringBuilder?>(null, null)
    ];

    public static readonly Dictionary<ICharSequence, bool> CharSequenceBoolMap = new()
    {
        { new CharArrayStringBuilder(""), true }
      , { new MutableString("FirstKey"), false }
      , { new CharArrayStringBuilder("NullKey"), false }
      , { new MutableString("SecondKey"), true }
    };

    public static readonly List<KeyValuePair<ICharSequence?, bool?>> NullCharSequenceBoolMap =
    [
        new (new CharArrayStringBuilder(""), true)
      , new (new MutableString("FirstKey"), false)
      , new (new CharArrayStringBuilder("NullKey"), null), new KeyValuePair<ICharSequence?, bool?>(null, null)
    ];


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

    public static readonly List<KeyValuePair<object?, object?>> NullObjNullObjMap =
    [
        new (true, 1), new KeyValuePair<object?, object?>(42, (bool?)false)
      , new (null, "NullKey")
      , new ("StringKey", new CharArrayStringBuilder("CharArrayStringBuilderValue"))
      , new (new MySpanFormattableClass("MySpanFormattableClassKeyWithNullValue"), null)
      , new (new MySpanFormattableStruct("MySpanFormattableStructKeyWithByteValue"), byte.MaxValue)
      , new (new Version(1, 1, 1, 1), "NextReleaseStringValue")
      , new ("CharArrayKeyWithDoubleValue".ToCharArray(), Math.PI)
      , new (new MutableString("MutableStringKeyBigIntegerValue"), (UInt128.MaxValue + (BigInteger)UInt128.MaxValue))
      , new (new StringBuilder("StringBuilderKeyIPAddressValue"), new IPAddress([127, 0, 0, 1]))
      , new (null, null)
    ];

    // 1. Start No Flags enums
    // 1. a) No Default No Flags
    public static readonly Dictionary<NoDefaultLongNoFlagsEnum, WithDefaultLongWithFlagsEnum> EnumLongNdNfToWdWfMap = new()
    {
        { NoDefaultLongNoFlagsEnum.NDLNFE_4, WithDefaultLongWithFlagsEnum.WDLWFE_4 }
      , { NoDefaultLongNoFlagsEnum.NDLNFE_34, WithDefaultLongWithFlagsEnum.WDLWFE_34 }
      , { NoDefaultLongNoFlagsEnum.NDLNFE_1.Default(), WithDefaultLongWithFlagsEnum.Default  }
      , { NoDefaultLongNoFlagsEnum.NDLNFE_1, WithDefaultLongWithFlagsEnum.WDLWFE_1.First8Last2MaskMinusFlag1() }
      , { NoDefaultLongNoFlagsEnum.NDLNFE_13, WithDefaultLongWithFlagsEnum.WDLWFE_13 | WithDefaultLongWithFlagsEnum.WDLWFE_23 }
      , { NoDefaultLongNoFlagsEnum.NDLNFE_2, WithDefaultLongWithFlagsEnum.WDLWFE_2 | WithDefaultLongWithFlagsEnum.WDLWFE_5 }
    };

    public static KeyValuePredicate<NoDefaultLongNoFlagsEnum, WithDefaultLongWithFlagsEnum>
        EnumLongNdNfToWdWf_First_10 = (count, _, _) => StopOnFirstExclusion(count <= 10);

    public static readonly KeyValuePredicate<NoDefaultLongNoFlagsEnum, WithDefaultLongWithFlagsEnum>
        EnumLongNdNfToWdWf_First_3 = (count, _, _) => StopOnFirstExclusion(count <= 3);

    public static KeyValuePredicate<NoDefaultLongNoFlagsEnum, WithDefaultLongWithFlagsEnum>
        EnumLongNdNfToWdWf_Second_3 = (count, _, _) => BetweenRetrieveRange(count, 4, 7);

    public static readonly List<NoDefaultLongNoFlagsEnum> EnumLongNdNfToWdWf_First_3_SubList = 
    [
        NoDefaultLongNoFlagsEnum.NDLNFE_4
      , NoDefaultLongNoFlagsEnum.NDLNFE_34
      , NoDefaultLongNoFlagsEnum.NDLNFE_1.Default()
    ];

    public static readonly List<NoDefaultLongNoFlagsEnum> EnumLongNdNfToWdWf_Second_3_SubList = 
    [
        NoDefaultLongNoFlagsEnum.NDLNFE_1
      , NoDefaultLongNoFlagsEnum.NDLNFE_13
      , NoDefaultLongNoFlagsEnum.NDLNFE_2
    ];
    
    public static readonly PalantírReveal<NoDefaultLongNoFlagsEnum> NoDefaultLongNoFlags_Reveal  = (e, tos) => tos.StartSimpleContentType(e).AsValue(e);

    public static readonly List<KeyValuePair<NoDefaultLongNoFlagsEnum, WithDefaultLongWithFlagsEnum?>> EnumLongNdNfToNullWdWfMap =
    [
        new (NoDefaultLongNoFlagsEnum.NDLNFE_4, null)
      , new (NoDefaultLongNoFlagsEnum.NDLNFE_34, WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask)
      , new (NoDefaultLongNoFlagsEnum.NDLNFE_1.Default(), WithDefaultLongWithFlagsEnum.WDLWFE_All)
      , new (NoDefaultLongNoFlagsEnum.NDLNFE_1, null)
      , new (NoDefaultLongNoFlagsEnum.NDLNFE_13 , WithDefaultLongWithFlagsEnum.WDLWFE_13)
    ];
    
    public static readonly List<KeyValuePair<NoDefaultLongNoFlagsEnum?, WithDefaultLongWithFlagsEnum?>> NullEnumLongNdNfToWdWfMap =
    [
        new (NoDefaultLongNoFlagsEnum.NDLNFE_4, null)
      , new (null, WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask)
      , new (NoDefaultLongNoFlagsEnum.NDLNFE_1, null)
      , new (NoDefaultLongNoFlagsEnum.NDLNFE_1.Default() , WithDefaultLongWithFlagsEnum.WDLWFE_All)
      , new (NoDefaultLongNoFlagsEnum.NDLNFE_13 , WithDefaultLongWithFlagsEnum.WDLWFE_13)
      , new (null, null)
    ];

    public static KeyValuePredicate<NoDefaultLongNoFlagsEnum?, WithDefaultLongWithFlagsEnum?>
        NullEnumLongNdNfToWdWf_First_10 = (count, _, _) => StopOnFirstExclusion(count <= 10);

    public static readonly KeyValuePredicate<NoDefaultLongNoFlagsEnum?, WithDefaultLongWithFlagsEnum?>
        NullEnumLongNdNfToWdWf_First_3 = (count, _, _) => StopOnFirstExclusion(count <= 3);

    public static KeyValuePredicate<NoDefaultLongNoFlagsEnum?, WithDefaultLongWithFlagsEnum?>
        NullEnumLongNdNfToWdWf_Second_3 = (count, _, _) => BetweenRetrieveRange(count, 4, 7);

    public static readonly Dictionary<NoDefaultULongNoFlagsEnum, WithDefaultULongWithFlagsEnum> EnumULongNdNfToWdwfMap = new()
    {
        { NoDefaultULongNoFlagsEnum.NDUNFE_4, WithDefaultULongWithFlagsEnum.WDUWFE_4 }
      , { NoDefaultULongNoFlagsEnum.NDUNFE_34, WithDefaultULongWithFlagsEnum.WDUWFE_34 }
      , { NoDefaultULongNoFlagsEnum.NDUNFE_1.Default(), WithDefaultULongWithFlagsEnum.Default }
      , { NoDefaultULongNoFlagsEnum.NDUNFE_1, WithDefaultULongWithFlagsEnum.WDUWFE_1 }
      , { NoDefaultULongNoFlagsEnum.NDUNFE_13, WithDefaultULongWithFlagsEnum.WDUWFE_13 }
      , { NoDefaultULongNoFlagsEnum.NDUNFE_2, WithDefaultULongWithFlagsEnum.WDUWFE_2 }
    };

    public static readonly KeyValuePredicate<NoDefaultULongNoFlagsEnum, WithDefaultULongWithFlagsEnum>
        EnumULongNdNfToWdWf_First_10 = (count, _, _) => StopOnFirstExclusion(count <= 10);

    public static readonly KeyValuePredicate<NoDefaultULongNoFlagsEnum, WithDefaultULongWithFlagsEnum>
        EnumULongNdNfToWdWf_First_3 = (count, _, _) => StopOnFirstExclusion(count <= 3);

    public static readonly KeyValuePredicate<NoDefaultULongNoFlagsEnum, WithDefaultULongWithFlagsEnum>
        EnumULongNdNfToWdWf_Second_3 = (count, _, _) => BetweenRetrieveRange(count, 4, 7);

    public static readonly List<NoDefaultULongNoFlagsEnum> EnumULongNdNfDateTime_First_3_SubList = 
    [
        NoDefaultULongNoFlagsEnum.NDUNFE_4
      , NoDefaultULongNoFlagsEnum.NDUNFE_34
      , NoDefaultULongNoFlagsEnum.NDUNFE_1.Default()
    ];

    public static readonly List<NoDefaultULongNoFlagsEnum> EnumULongNdNfDateTime_Second_3_SubList = 
    [
        NoDefaultULongNoFlagsEnum.NDUNFE_1
      , NoDefaultULongNoFlagsEnum.NDUNFE_13
      , NoDefaultULongNoFlagsEnum.NDUNFE_2
    ];
    
    public static readonly PalantírReveal<NoDefaultULongNoFlagsEnum> NoDefaultULongNoFlags_Reveal  = (e, tos) => tos.StartSimpleContentType(e).AsValue(e);

    public static readonly List<KeyValuePair<NoDefaultULongNoFlagsEnum, WithDefaultULongWithFlagsEnum?>> EnumULongNdNfToNullWdWfMap =
    [
        new (NoDefaultULongNoFlagsEnum.NDUNFE_4 , WithDefaultULongWithFlagsEnum.WDUWFE_4)
      , new (NoDefaultULongNoFlagsEnum.NDUNFE_34 , WithDefaultULongWithFlagsEnum.WDUWFE_34)
      , new (NoDefaultULongNoFlagsEnum.NDUNFE_1.Default(), null)
      , new (NoDefaultULongNoFlagsEnum.NDUNFE_1 , WithDefaultULongWithFlagsEnum.WDUWFE_1)
      , new (NoDefaultULongNoFlagsEnum.NDUNFE_13 , WithDefaultULongWithFlagsEnum.WDUWFE_13)
      , new (NoDefaultULongNoFlagsEnum.NDUNFE_2, null)
    ];

    public static readonly List<KeyValuePair<NoDefaultULongNoFlagsEnum?, WithDefaultULongWithFlagsEnum?>> NullEnumULongNdNfToWdWfMap =
    [
        new (NoDefaultULongNoFlagsEnum.NDUNFE_4, null)
      , new (null, WithDefaultULongWithFlagsEnum.WDUWFE_34)
      , new (NoDefaultULongNoFlagsEnum.NDUNFE_1.Default() , WithDefaultULongWithFlagsEnum.Default)
      , new (NoDefaultULongNoFlagsEnum.NDUNFE_1, null)
      , new (NoDefaultULongNoFlagsEnum.NDUNFE_13 , WithDefaultULongWithFlagsEnum.WDUWFE_13)
      , new (null, null)
    ];

    public static readonly KeyValuePredicate<NoDefaultULongNoFlagsEnum?, WithDefaultULongWithFlagsEnum?>
        NullEnumULongNdNfToWdWf_First_10 = (count, _, _) => StopOnFirstExclusion(count <= 10);

    public static readonly KeyValuePredicate<NoDefaultULongNoFlagsEnum?, WithDefaultULongWithFlagsEnum?>
        NullEnumULongNdNfToWdWf_First_3 = (count, _, _) => StopOnFirstExclusion(count <= 3);

    public static readonly KeyValuePredicate<NoDefaultULongNoFlagsEnum?, WithDefaultULongWithFlagsEnum?>
        NullEnumULongNdNfToWdWf_Second_3 = (count, _, _) => BetweenRetrieveRange(count, 4, 7);

    // 1. b) With Default No Flags
    public static readonly Dictionary<WithDefaultLongNoFlagsEnum, NoDefaultLongWithFlagsEnum> EnumLongWdNfToNdWfMap = new()
    {
        { WithDefaultLongNoFlagsEnum.WDLNFE_4, NoDefaultLongWithFlagsEnum.NDLWFE_4 }
      , { WithDefaultLongNoFlagsEnum.WDLNFE_34, NoDefaultLongWithFlagsEnum.NDLWFE_34 }
      , { WithDefaultLongNoFlagsEnum.Default, NoDefaultLongWithFlagsEnum.NDLWFE_1.Default() }
      , { WithDefaultLongNoFlagsEnum.WDLNFE_1, NoDefaultLongWithFlagsEnum.NDLWFE_1 }
      , { WithDefaultLongNoFlagsEnum.WDLNFE_2, NoDefaultLongWithFlagsEnum.NDLWFE_2 }
      , { WithDefaultLongNoFlagsEnum.WDLNFE_3, NoDefaultLongWithFlagsEnum.NDLWFE_3 }
    };

    public static readonly KeyValuePredicate<WithDefaultLongNoFlagsEnum, NoDefaultLongWithFlagsEnum>
        EnumLongWdNfToNdWf_First_10 = (count, _, _) => StopOnFirstExclusion(count <= 10);

    public static readonly KeyValuePredicate<WithDefaultLongNoFlagsEnum, NoDefaultLongWithFlagsEnum>
        EnumLongWdNfToNdWf_First_3 = (count, _, _) => StopOnFirstExclusion(count <= 3);

    public static readonly KeyValuePredicate<WithDefaultLongNoFlagsEnum, NoDefaultLongWithFlagsEnum>
        EnumLongWdNfToNdWf_Second_3 = (count, _, _) => BetweenRetrieveRange(count, 4, 7);

    public static readonly List<WithDefaultLongNoFlagsEnum> EnumLongWdNfToNdWf_First_3_SubList = 
    [
        WithDefaultLongNoFlagsEnum.WDLNFE_4
      , WithDefaultLongNoFlagsEnum.WDLNFE_34
      , WithDefaultLongNoFlagsEnum.Default.Default()
    ];

    public static readonly List<WithDefaultLongNoFlagsEnum> EnumLongWdNfToNdWf_Second_3_SubList = 
    [
        WithDefaultLongNoFlagsEnum.WDLNFE_1
      , WithDefaultLongNoFlagsEnum.WDLNFE_2
      , WithDefaultLongNoFlagsEnum.WDLNFE_3
    ];
    
    public static readonly PalantírReveal<WithDefaultLongNoFlagsEnum> WithDefaultLongNoFlags_Reveal  = (e, tos) => tos.StartSimpleContentType(e).AsValue(e);

    public static readonly List<KeyValuePair<WithDefaultLongNoFlagsEnum, NoDefaultLongWithFlagsEnum?>> EnumLongWdNfToNullNdWfMap =
    [
        new (WithDefaultLongNoFlagsEnum.WDLNFE_4, null)
      , new (WithDefaultLongNoFlagsEnum.WDLNFE_34 , NoDefaultLongWithFlagsEnum.NDLWFE_1.First8Last2MaskMinusFlag1())
      , new (WithDefaultLongNoFlagsEnum.Default, null)
      , new (WithDefaultLongNoFlagsEnum.WDLNFE_1, null)
      , new (WithDefaultLongNoFlagsEnum.WDLNFE_2 , NoDefaultLongWithFlagsEnum.NDLWFE_2)
      , new (WithDefaultLongNoFlagsEnum.WDLNFE_13 , NoDefaultLongWithFlagsEnum.NDLWFE_13)
    ];

    public static readonly List<KeyValuePair<WithDefaultLongNoFlagsEnum?, NoDefaultLongWithFlagsEnum?>> NullEnumLongWdNfToNdWfMap =
    [
        new (WithDefaultLongNoFlagsEnum.WDLNFE_4, null)
      , new (null , NoDefaultLongWithFlagsEnum.NDLWFE_1.First8Last2MaskMinusFlag1())
      , new (WithDefaultLongNoFlagsEnum.Default, NoDefaultLongWithFlagsEnum.NDLWFE_1.Default())
      , new (WithDefaultLongNoFlagsEnum.WDLNFE_1, null)
      , new (WithDefaultLongNoFlagsEnum.WDLNFE_13, NoDefaultLongWithFlagsEnum.NDLWFE_13)
      , new (null, null)
    ];

    public static KeyValuePredicate<WithDefaultLongNoFlagsEnum?, NoDefaultLongWithFlagsEnum?>
        NullEnumLongWdNfToNdWf_First_10 = (count, _, _) => StopOnFirstExclusion(count <= 10);

    public static readonly KeyValuePredicate<WithDefaultLongNoFlagsEnum?, NoDefaultLongWithFlagsEnum?>
        NullEnumLongWdNfToNdWf_First_3 = (count, _, _) => StopOnFirstExclusion(count <= 3);

    public static KeyValuePredicate<WithDefaultLongNoFlagsEnum?, NoDefaultLongWithFlagsEnum?>
        NullEnumLongWdNfToNdWf_Second_3 = (count, _, _) => BetweenRetrieveRange(count, 4, 7);

    public static readonly Dictionary<WithDefaultULongNoFlagsEnum, NoDefaultULongWithFlagsEnum> EnumULongWdNfToNdWfMap = new()
    {
        { WithDefaultULongNoFlagsEnum.WDUNFE_2, NoDefaultULongWithFlagsEnum.NDUWFE_2 }
      , { WithDefaultULongNoFlagsEnum.WDUNFE_4, NoDefaultULongWithFlagsEnum.NDUWFE_4 }
      , { WithDefaultULongNoFlagsEnum.WDUNFE_34, NoDefaultULongWithFlagsEnum.NDUWFE_34 }
      , { WithDefaultULongNoFlagsEnum.Default, NoDefaultULongWithFlagsEnum.NDUWFE_1.Default() }
      , { WithDefaultULongNoFlagsEnum.WDUNFE_13, NoDefaultULongWithFlagsEnum.NDUWFE_13 }
    };

    public static readonly KeyValuePredicate<WithDefaultULongNoFlagsEnum, NoDefaultULongWithFlagsEnum>
        EnumULongWdNfToNdWf_First_10 = (count, _, _) => StopOnFirstExclusion(count <= 10);

    public static readonly KeyValuePredicate<WithDefaultULongNoFlagsEnum, NoDefaultULongWithFlagsEnum>
        EnumULongWdNfToNdWf_First_3 = (count, _, _) => StopOnFirstExclusion(count <= 3);

    public static readonly KeyValuePredicate<WithDefaultULongNoFlagsEnum, NoDefaultULongWithFlagsEnum>
        EnumULongWdNfToNdWf_Second_3 = (count, _, _) => BetweenRetrieveRange(count, 4, 7);

    public static readonly List<WithDefaultULongNoFlagsEnum> EnumULongWdNfToNdWf_First_3_SubList = 
    [
        WithDefaultULongNoFlagsEnum.WDUNFE_2
      , WithDefaultULongNoFlagsEnum.WDUNFE_4
      , WithDefaultULongNoFlagsEnum.WDUNFE_34
    ];

    public static readonly List<WithDefaultULongNoFlagsEnum> EnumULongWdNfToNdWf_Second_3_SubList = 
    [
        WithDefaultULongNoFlagsEnum.Default
      , WithDefaultULongNoFlagsEnum.WDUNFE_13
    ];
    
    public static readonly PalantírReveal<WithDefaultULongNoFlagsEnum> WithDefaultULongNoFlags_Reveal  = 
        (e, tos) => tos.StartSimpleContentType(e).AsValue(e);

    public static readonly List<KeyValuePair<WithDefaultULongNoFlagsEnum, NoDefaultULongWithFlagsEnum?>> EnumULongWdNfToNullNdWfMap =
    [
        new (WithDefaultULongNoFlagsEnum.WDUNFE_2, null)
      , new (WithDefaultULongNoFlagsEnum.WDUNFE_4, NoDefaultULongWithFlagsEnum.NDUWFE_2)
      , new (WithDefaultULongNoFlagsEnum.WDUNFE_34, NoDefaultULongWithFlagsEnum.NDUWFE_34)
      , new (WithDefaultULongNoFlagsEnum.Default, NoDefaultULongWithFlagsEnum.NDUWFE_1.Default())
      , new (WithDefaultULongNoFlagsEnum.WDUNFE_13, NoDefaultULongWithFlagsEnum.NDUWFE_13)
    ];

    public static readonly List<KeyValuePair<WithDefaultULongNoFlagsEnum?, NoDefaultULongWithFlagsEnum?>> NullEnumULongWdNfToNdWfMap =
    [
        new (WithDefaultULongNoFlagsEnum.WDUNFE_4, null)
      , new (null, NoDefaultULongWithFlagsEnum.NDUWFE_2)
      , new (WithDefaultULongNoFlagsEnum.Default, NoDefaultULongWithFlagsEnum.NDUWFE_1.Default())
      , new (WithDefaultULongNoFlagsEnum.WDUNFE_13, NoDefaultULongWithFlagsEnum.NDUWFE_13)
      , new (null, NoDefaultULongWithFlagsEnum.NDUWFE_All)
    ];

    public static readonly KeyValuePredicate<WithDefaultULongNoFlagsEnum?, NoDefaultULongWithFlagsEnum?>
        NullEnumULongWdNfToNdWf_First_10 = (count, _, _) => StopOnFirstExclusion(count <= 10);

    public static readonly KeyValuePredicate<WithDefaultULongNoFlagsEnum?, NoDefaultULongWithFlagsEnum?>
        NullEnumULongWdNfToNdWf_First_3 = (count, _, _) => StopOnFirstExclusion(count <= 3);

    public static readonly KeyValuePredicate<WithDefaultULongNoFlagsEnum?, NoDefaultULongWithFlagsEnum?>
        NullEnumULongWdNfToNdWf_Second_3 = (count, _, _) => BetweenRetrieveRange(count, 4, 7);


    //  2. Start With Flags enums
    //  2. a)   No Default With Flags
    public static readonly Dictionary<NoDefaultLongWithFlagsEnum, WithDefaultLongNoFlagsEnum> EnumLongNdWfToWdNfMap = new()
    {
        { NoDefaultLongWithFlagsEnum.NDLWFE_4, WithDefaultLongNoFlagsEnum.WDLNFE_4 }
      , { NoDefaultLongWithFlagsEnum.NDLWFE_1.First8MinusFlag6Mask(), WithDefaultLongNoFlagsEnum.WDLNFE_6 }
      , { NoDefaultLongWithFlagsEnum.NDLWFE_1.Default(), WithDefaultLongNoFlagsEnum.Default }
      , { NoDefaultLongWithFlagsEnum.NDLWFE_1.First8AndLast2Mask(), WithDefaultLongNoFlagsEnum.WDLNFE_3 }
      , { NoDefaultLongWithFlagsEnum.NDLWFE_22, WithDefaultLongNoFlagsEnum.WDLNFE_22 }
      , { NoDefaultLongWithFlagsEnum.NDLWFE_34, WithDefaultLongNoFlagsEnum.WDLNFE_34 }
    };

    public static readonly KeyValuePredicate<NoDefaultLongWithFlagsEnum, WithDefaultLongNoFlagsEnum>
        EnumLongNdWfToWdNf_First_10 = (count, _, _) => StopOnFirstExclusion(count <= 10);

    public static readonly KeyValuePredicate<NoDefaultLongWithFlagsEnum, WithDefaultLongNoFlagsEnum>
        EnumLongNdWfToWdNf_First_3 = (count, _, _) => StopOnFirstExclusion(count <= 3);

    public static readonly KeyValuePredicate<NoDefaultLongWithFlagsEnum, WithDefaultLongNoFlagsEnum>
        EnumLongNdWfToWdNf_Second_3 = (count, _, _) => BetweenRetrieveRange(count, 4, 7);

    public static readonly List<NoDefaultLongWithFlagsEnum> EnumLongNdWfToWdNf_First_3_SubList = 
    [
        NoDefaultLongWithFlagsEnum.NDLWFE_4
      , NoDefaultLongWithFlagsEnum.NDLWFE_1.First8MinusFlag6Mask()
      , NoDefaultLongWithFlagsEnum.NDLWFE_1.Default()
    ];

    public static readonly List<NoDefaultLongWithFlagsEnum> EnumLongNdWfToWdNf_Second_3_SubList = 
    [
        NoDefaultLongWithFlagsEnum.NDLWFE_1.First8AndLast2Mask()
      , NoDefaultLongWithFlagsEnum.NDLWFE_22
      , NoDefaultLongWithFlagsEnum.NDLWFE_34
    ];
    
    public static readonly PalantírReveal<NoDefaultLongWithFlagsEnum> NoDefaultLongWithFlags_Reveal  = 
        (e, tos) => tos.StartSimpleContentType(e).AsValue(e);

    public static readonly List<KeyValuePair<NoDefaultLongWithFlagsEnum, WithDefaultLongNoFlagsEnum?>> EnumLongNdWfToNullWdNfMap =
    [
        new (NoDefaultLongWithFlagsEnum.NDLWFE_4, WithDefaultLongNoFlagsEnum.WDLNFE_4)
      , new (NoDefaultLongWithFlagsEnum.NDLWFE_1.First8MinusFlag6Mask(), null)
      , new (NoDefaultLongWithFlagsEnum.NDLWFE_1.Default(), WithDefaultLongNoFlagsEnum.Default)
      , new (NoDefaultLongWithFlagsEnum.NDLWFE_1.First8AndLast2Mask(), null)
      , new (NoDefaultLongWithFlagsEnum.NDLWFE_22, WithDefaultLongNoFlagsEnum.WDLNFE_22)
      , new (NoDefaultLongWithFlagsEnum.NDLWFE_34, null)
    ];

    public static readonly List<KeyValuePair<NoDefaultLongWithFlagsEnum?, WithDefaultLongNoFlagsEnum?>> NullEnumLongNdWfToWdNfMap =
    [
        new (null, null)
      , new (NoDefaultLongWithFlagsEnum.NDLWFE_4, WithDefaultLongNoFlagsEnum.WDLNFE_4)
      , new (NoDefaultLongWithFlagsEnum.NDLWFE_1.First8MinusFlag6Mask(), null)
      , new (NoDefaultLongWithFlagsEnum.NDLWFE_1.Default(), WithDefaultLongNoFlagsEnum.Default)
      , new (NoDefaultLongWithFlagsEnum.NDLWFE_1.First8AndLast2Mask(), WithDefaultLongNoFlagsEnum.WDLNFE_3)
      , new (NoDefaultLongWithFlagsEnum.NDLWFE_22, WithDefaultLongNoFlagsEnum.WDLNFE_22)
      , new (NoDefaultLongWithFlagsEnum.NDLWFE_34, null)
    ];

    public static readonly KeyValuePredicate<NoDefaultLongWithFlagsEnum?, WithDefaultLongNoFlagsEnum?>
        NullEnumLongNdWfToWdNf_First_10 = (count, _, _) => StopOnFirstExclusion(count <= 10);

    public static readonly KeyValuePredicate<NoDefaultLongWithFlagsEnum?, WithDefaultLongNoFlagsEnum?>
        NullEnumLongNdWfToWdNf_First_3 = (count, _, _) => StopOnFirstExclusion(count <= 3);

    public static readonly KeyValuePredicate<NoDefaultLongWithFlagsEnum?, WithDefaultLongNoFlagsEnum?>
        NullEnumLongNdWfToWdNf_Second_3 = (count, _, _) => BetweenRetrieveRange(count, 4, 7);

    public static readonly Dictionary<NoDefaultULongWithFlagsEnum, WithDefaultULongNoFlagsEnum> EnumULongNdWfToWdNfMap = new()
    {
        { NoDefaultULongWithFlagsEnum.NDUWFE_4, WithDefaultULongNoFlagsEnum.WDUNFE_4 }
      , { NoDefaultULongWithFlagsEnum.NDUWFE_1.First8MinusFlag6Mask(), WithDefaultULongNoFlagsEnum.WDUNFE_1 }
      , { NoDefaultULongWithFlagsEnum.NDUWFE_34, WithDefaultULongNoFlagsEnum.WDUNFE_34 }
      , { NoDefaultULongWithFlagsEnum.NDUWFE_1.Default(), WithDefaultULongNoFlagsEnum.Default }
      , { NoDefaultULongWithFlagsEnum.NDUWFE_1.First8AndLast2Mask(), WithDefaultULongNoFlagsEnum.WDUNFE_8 }
      , { NoDefaultULongWithFlagsEnum.NDUWFE_22, WithDefaultULongNoFlagsEnum.WDUNFE_22 }
    };

    public static readonly KeyValuePredicate<NoDefaultULongWithFlagsEnum, WithDefaultULongNoFlagsEnum>
        EnumULongNdWfToWdNfM_First_10 = (count, _, _) => StopOnFirstExclusion(count <= 10);

    public static readonly KeyValuePredicate<NoDefaultULongWithFlagsEnum, WithDefaultULongNoFlagsEnum>
        EnumULongNdWfToWdNfM_First_3 = (count, _, _) => StopOnFirstExclusion(count <= 3);

    public static readonly KeyValuePredicate<NoDefaultULongWithFlagsEnum, WithDefaultULongNoFlagsEnum>
        EnumULongNdWfToWdNfM_Second_3 = (count, _, _) => BetweenRetrieveRange(count, 4, 7);

    public static readonly List<NoDefaultULongWithFlagsEnum> EnumULongNdWfToWdNf_First_3_SubList = 
    [
        NoDefaultULongWithFlagsEnum.NDUWFE_4
      , NoDefaultULongWithFlagsEnum.NDUWFE_1.First8MinusFlag6Mask()
      , NoDefaultULongWithFlagsEnum.NDUWFE_34
    ];

    public static readonly List<NoDefaultULongWithFlagsEnum> EnumULongNdWfToWdNf_Second_3_SubList = 
    [
        NoDefaultULongWithFlagsEnum.NDUWFE_1.Default()
      , NoDefaultULongWithFlagsEnum.NDUWFE_1.First8AndLast2Mask()
      , NoDefaultULongWithFlagsEnum.NDUWFE_22
    ];
    
    public static readonly PalantírReveal<NoDefaultULongWithFlagsEnum> NoDefaultULongWithFlags_Reveal  = 
        (e, tos) => tos.StartSimpleContentType(e).AsValue(e);

    public static readonly List<KeyValuePair<NoDefaultULongWithFlagsEnum, WithDefaultULongNoFlagsEnum?>> EnumULongNdWfToNullWdNfMap =
    [
        new (NoDefaultULongWithFlagsEnum.NDUWFE_4, WithDefaultULongNoFlagsEnum.WDUNFE_4)
      , new (NoDefaultULongWithFlagsEnum.NDUWFE_1.First8MinusFlag6Mask(), null)
      , new (NoDefaultULongWithFlagsEnum.NDUWFE_34, WithDefaultULongNoFlagsEnum.WDUNFE_34)
      , new (NoDefaultULongWithFlagsEnum.NDUWFE_1.Default(), WithDefaultULongNoFlagsEnum.Default)
      , new (NoDefaultULongWithFlagsEnum.NDUWFE_1.First8AndLast2Mask(), WithDefaultULongNoFlagsEnum.WDUNFE_8)
      , new (NoDefaultULongWithFlagsEnum.NDUWFE_22, WithDefaultULongNoFlagsEnum.WDUNFE_22)
    ];

    public static readonly List<KeyValuePair<NoDefaultULongWithFlagsEnum?, WithDefaultULongNoFlagsEnum?>> NullEnumULongNdWfToWdNfMap =
    [
        new (null, null)
      , new (NoDefaultULongWithFlagsEnum.NDUWFE_1.First8MinusFlag6Mask(), WithDefaultULongNoFlagsEnum.WDUNFE_1)
      , new (NoDefaultULongWithFlagsEnum.NDUWFE_34, WithDefaultULongNoFlagsEnum.WDUNFE_34)
      , new (NoDefaultULongWithFlagsEnum.NDUWFE_1.Default(), WithDefaultULongNoFlagsEnum.Default)
      , new (NoDefaultULongWithFlagsEnum.NDUWFE_1.First8AndLast2Mask(), WithDefaultULongNoFlagsEnum.WDUNFE_8)
      , new (NoDefaultULongWithFlagsEnum.NDUWFE_22, WithDefaultULongNoFlagsEnum.WDUNFE_22)
    ];

    public static readonly KeyValuePredicate<NoDefaultULongWithFlagsEnum?, WithDefaultULongNoFlagsEnum?>
        NullEnumULongNdWfToWdNf_First_10 = (count, _, _) => StopOnFirstExclusion(count <= 10);

    public static readonly KeyValuePredicate<NoDefaultULongWithFlagsEnum?, WithDefaultULongNoFlagsEnum?>
        NullEnumULongNdWfToWdNf_First_3 = (count, _, _) => StopOnFirstExclusion(count <= 3);

    public static readonly KeyValuePredicate<NoDefaultULongWithFlagsEnum?, WithDefaultULongNoFlagsEnum?>
        NullEnumULongNdWfToWdNf_Second_3 = (count, _, _) => BetweenRetrieveRange(count, 4, 7);

    // 2. b) With Default With Flags
    public static readonly Dictionary<WithDefaultLongWithFlagsEnum, NoDefaultLongNoFlagsEnum> EnumLongWdWfToNdNfMap = new()
    {
        { WithDefaultLongWithFlagsEnum.WDLWFE_4, NoDefaultLongNoFlagsEnum.NDLNFE_4 }
      , { WithDefaultLongWithFlagsEnum.WDLWFE_1.First8MinusFlag2Mask(), NoDefaultLongNoFlagsEnum.NDLNFE_8 }
      , { WithDefaultLongWithFlagsEnum.Default, NoDefaultLongNoFlagsEnum.NDLNFE_1.Default() }
      , { WithDefaultLongWithFlagsEnum.WDLWFE_1.First8AndLast2Mask(), NoDefaultLongNoFlagsEnum.NDLNFE_6 }
      , { WithDefaultLongWithFlagsEnum.WDLWFE_22, NoDefaultLongNoFlagsEnum.NDLNFE_22 }
      , { WithDefaultLongWithFlagsEnum.WDLWFE_32, NoDefaultLongNoFlagsEnum.NDLNFE_32 }
    };

    public static readonly KeyValuePredicate<WithDefaultLongWithFlagsEnum, NoDefaultLongNoFlagsEnum>
        EnumLongWdWfToNdNf_First_10 = (count, _, _) => StopOnFirstExclusion(count <= 10);

    public static readonly KeyValuePredicate<WithDefaultLongWithFlagsEnum, NoDefaultLongNoFlagsEnum>
        EnumLongWdWfToNdNf_First_3 = (count, _, _) => StopOnFirstExclusion(count <= 3);

    public static readonly KeyValuePredicate<WithDefaultLongWithFlagsEnum, NoDefaultLongNoFlagsEnum>
        EnumLongWdWfToNdNf_Second_3 = (count, _, _) => BetweenRetrieveRange(count, 4, 7);

    public static readonly List<WithDefaultLongWithFlagsEnum> EnumLongWdWfToNdNf_First_3_SubList = 
    [
        WithDefaultLongWithFlagsEnum.WDLWFE_4
      , WithDefaultLongWithFlagsEnum.WDLWFE_1.First8MinusFlag2Mask()
      , WithDefaultLongWithFlagsEnum.Default
    ];

    public static readonly List<WithDefaultLongWithFlagsEnum> EnumLongWdWfToNdNf_Second_3_SubList = 
    [
        WithDefaultLongWithFlagsEnum.WDLWFE_1.First8AndLast2Mask()
      , WithDefaultLongWithFlagsEnum.WDLWFE_22
      , WithDefaultLongWithFlagsEnum.WDLWFE_32
    ];
    
    public static readonly PalantírReveal<WithDefaultLongWithFlagsEnum> WithDefaultLongWithFlags_Reveal  = 
        (e, tos) => tos.StartSimpleContentType(e).AsValue(e);

    public static readonly List<KeyValuePair<WithDefaultLongWithFlagsEnum, NoDefaultLongNoFlagsEnum?>> EnumLongWdWfToNullNdNfMap =
    [
        new (WithDefaultLongWithFlagsEnum.WDLWFE_4, NoDefaultLongNoFlagsEnum.NDLNFE_4)
      , new (WithDefaultLongWithFlagsEnum.WDLWFE_1.First8MinusFlag2Mask(), NoDefaultLongNoFlagsEnum.NDLNFE_8)
      , new (WithDefaultLongWithFlagsEnum.Default, null)
      , new (WithDefaultLongWithFlagsEnum.WDLWFE_1.First8AndLast2Mask(), NoDefaultLongNoFlagsEnum.NDLNFE_6)
      , new (WithDefaultLongWithFlagsEnum.WDLWFE_22, NoDefaultLongNoFlagsEnum.NDLNFE_22)
      , new (WithDefaultLongWithFlagsEnum.WDLWFE_32, null)
    ];

    public static readonly List<KeyValuePair<WithDefaultLongWithFlagsEnum?, NoDefaultLongNoFlagsEnum?>> NullEnumLongWdWfToNdNfMap =
    [
        new (null, null)
      , new (WithDefaultLongWithFlagsEnum.WDLWFE_1.First8MinusFlag2Mask(), NoDefaultLongNoFlagsEnum.NDLNFE_8)
      , new (WithDefaultLongWithFlagsEnum.Default, NoDefaultLongNoFlagsEnum.NDLNFE_1.Default())
      , new (WithDefaultLongWithFlagsEnum.WDLWFE_1.First8AndLast2Mask(), NoDefaultLongNoFlagsEnum.NDLNFE_6)
      , new (WithDefaultLongWithFlagsEnum.WDLWFE_22, NoDefaultLongNoFlagsEnum.NDLNFE_22)
      , new (WithDefaultLongWithFlagsEnum.WDLWFE_32, null)
    ];

    public static readonly KeyValuePredicate<WithDefaultLongWithFlagsEnum?, NoDefaultLongNoFlagsEnum?>
        NullEnumLongWdWfToNdNf_First_10 = (count, _, _) => StopOnFirstExclusion(count <= 10);

    public static readonly KeyValuePredicate<WithDefaultLongWithFlagsEnum?, NoDefaultLongNoFlagsEnum?>
        NullEnumLongWdWfToNdNf_First_3 = (count, _, _) => StopOnFirstExclusion(count <= 3);

    public static readonly KeyValuePredicate<WithDefaultLongWithFlagsEnum?, NoDefaultLongNoFlagsEnum?>
        NullEnumLongWdWfToNdNf_Second_3 = (count, _, _) => BetweenRetrieveRange(count, 4, 7);

    public static readonly Dictionary<WithDefaultULongWithFlagsEnum, NoDefaultULongNoFlagsEnum> EnumULongWdWfToNdNfMap = new()
    {
        { WithDefaultULongWithFlagsEnum.WDUWFE_4, NoDefaultULongNoFlagsEnum.NDUNFE_4 }
      , { WithDefaultULongWithFlagsEnum.WDUWFE_1.First8MinusFlag2Mask(), NoDefaultULongNoFlagsEnum.NDUNFE_8 }
      , { WithDefaultULongWithFlagsEnum.Default, NoDefaultULongNoFlagsEnum.NDUNFE_1.Default() }
      , { WithDefaultULongWithFlagsEnum.WDUWFE_1.First8AndLast2Mask(), NoDefaultULongNoFlagsEnum.NDUNFE_6 }
      , { WithDefaultULongWithFlagsEnum.WDUWFE_22, NoDefaultULongNoFlagsEnum.NDUNFE_22 }
      , { WithDefaultULongWithFlagsEnum.WDUWFE_32, NoDefaultULongNoFlagsEnum.NDUNFE_32 }
    };

    public static readonly KeyValuePredicate<WithDefaultULongWithFlagsEnum, NoDefaultULongNoFlagsEnum>
        EnumULongWdWfToNdNf_First_10 = (count, _, _) => StopOnFirstExclusion(count <= 10);

    public static readonly KeyValuePredicate<WithDefaultULongWithFlagsEnum, NoDefaultULongNoFlagsEnum>
        EnumULongWdWfToNdNf_First_3 = (count, _, _) => StopOnFirstExclusion(count <= 3);

    public static readonly KeyValuePredicate<WithDefaultULongWithFlagsEnum, NoDefaultULongNoFlagsEnum>
        EnumULongWdWfToNdNf_Second_3 = (count, _, _) => BetweenRetrieveRange(count, 4, 7);

    public static readonly List<WithDefaultULongWithFlagsEnum> EnumULongWdWfToNdNf_First_3_SubList = 
    [
        WithDefaultULongWithFlagsEnum.WDUWFE_4
      , WithDefaultULongWithFlagsEnum.WDUWFE_1.First8MinusFlag2Mask()
      , WithDefaultULongWithFlagsEnum.Default
    ];

    public static readonly List<WithDefaultULongWithFlagsEnum> EnumULongWdWfToNdNf_Second_3_SubList = 
    [
        WithDefaultULongWithFlagsEnum.WDUWFE_1.First8AndLast2Mask()
      , WithDefaultULongWithFlagsEnum.WDUWFE_22
      , WithDefaultULongWithFlagsEnum.WDUWFE_32
    ];
    
    public static readonly PalantírReveal<WithDefaultULongWithFlagsEnum> WithDefaultULongWithFlags_Reveal  = 
        (e, tos) => tos.StartSimpleContentType(e).AsValue(e);

    public static readonly List<KeyValuePair<WithDefaultULongWithFlagsEnum, NoDefaultULongNoFlagsEnum?>> EnumULongWdWfToNullNdNfMap =
    [
        new (WithDefaultULongWithFlagsEnum.WDUWFE_4, null)
      , new (WithDefaultULongWithFlagsEnum.WDUWFE_1.First8MinusFlag2Mask(), NoDefaultULongNoFlagsEnum.NDUNFE_8)
      , new (WithDefaultULongWithFlagsEnum.Default, null)
      , new (WithDefaultULongWithFlagsEnum.WDUWFE_1.First8AndLast2Mask(), NoDefaultULongNoFlagsEnum.NDUNFE_6)
      , new (WithDefaultULongWithFlagsEnum.WDUWFE_22, NoDefaultULongNoFlagsEnum.NDUNFE_22)
      , new (WithDefaultULongWithFlagsEnum.WDUWFE_32, NoDefaultULongNoFlagsEnum.NDUNFE_32)
    ];

    public static readonly List<KeyValuePair<WithDefaultULongWithFlagsEnum?, NoDefaultULongNoFlagsEnum?>> NullEnumULongWdWfToNdNfMap =
    [
        new (null, null)
      , new (WithDefaultULongWithFlagsEnum.WDUWFE_1.First8MinusFlag2Mask(), NoDefaultULongNoFlagsEnum.NDUNFE_8)
      , new (WithDefaultULongWithFlagsEnum.Default, NoDefaultULongNoFlagsEnum.NDUNFE_1.Default())
      , new (WithDefaultULongWithFlagsEnum.WDUWFE_1.First8AndLast2Mask(), NoDefaultULongNoFlagsEnum.NDUNFE_6)
      , new (WithDefaultULongWithFlagsEnum.WDUWFE_22, NoDefaultULongNoFlagsEnum.NDUNFE_22)
      , new (WithDefaultULongWithFlagsEnum.WDUWFE_32, null)
    ];

    public static readonly KeyValuePredicate<WithDefaultULongWithFlagsEnum?, NoDefaultULongNoFlagsEnum?>
        NullEnumULongWdWfToNdNf_First_10 = (count, _, _) => StopOnFirstExclusion(count <= 10);

    public static readonly KeyValuePredicate<WithDefaultULongWithFlagsEnum?, NoDefaultULongNoFlagsEnum?>
        NullEnumULongWdWfToNdNf_First_3 = (count, _, _) => StopOnFirstExclusion(count <= 3);

    public static readonly KeyValuePredicate<WithDefaultULongWithFlagsEnum?, NoDefaultULongNoFlagsEnum?>
        NullEnumULongWdWfToNdNf_Second_3 = (count, _, _) => BetweenRetrieveRange(count, 4, 7);
}
