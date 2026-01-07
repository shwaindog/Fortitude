// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Net;
using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.UnitField.FixtureScaffolding;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.UnitFieldsContentTypes;

public static class StringBearerTestData
{
    private static PositionUpdatingList<ISingleFieldExpectation>? allStringBearerExpectations;  
    
    public static PositionUpdatingList<ISingleFieldExpectation> AllStringBearerExpectations => allStringBearerExpectations ??=
        new PositionUpdatingList<ISingleFieldExpectation>(typeof(StringBearerTestData))
        {
        // byte
        new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<byte>>
            (new FieldSpanFormattableAlwaysAddStringBearer<byte>
            {
                Value = 0
            }, "", true, new FieldSpanFormattableAlwaysAddStringBearer<byte>
            {
                Value = 0
            })
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "0" }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 0
            }, "", true, new FieldSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 0
            })
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "0" }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 0
            }, "", true, new FieldSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 0
            })
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "0" }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<byte>>
            (new FieldSpanFormattableAlwaysAddStringBearer<byte>
            {
                Value = 255
            })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "255"
                }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 255
            })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "255"
                }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 255
            })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "255"
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<byte>>
            (new FieldSpanFormattableAlwaysAddStringBearer<byte>
            {
                Value = 128
            }, "C2")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "$128.00"
                }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 128
            }, "C2")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "$128.00"
                }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 128
            }, "C2")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "$128.00"
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<byte>>
            (new FieldSpanFormattableAlwaysAddStringBearer<byte>
            {
                Value = 77
            }, "\"{0,-20}\"")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"77                  \""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 77
            }, "\"{0,-20}\"")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"77                  \""
                }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 77
            }, "\"{0,-20}\"")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"77                  \""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<byte>>
            (new FieldSpanFormattableAlwaysAddStringBearer<byte>
            {
                Value = 32
            }, "", true, new FieldSpanFormattableAlwaysAddStringBearer<byte>
            {
                Value = 32
            })
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "32" }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 32
            }, "", true, new FieldSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 32
            })
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "32" }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 32
            }, "", true, new FieldSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 32
            })
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "32" }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<byte>>
            (new FieldSpanFormattableAlwaysAddStringBearer<byte>
            {
                Value = 255
            }, "{0[..1]}")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "2"
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 255
            }, "{0[..1]}")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "2"
                }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 255
            }, "{0[..1]}")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "2"
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<byte>>
            (new FieldSpanFormattableAlwaysAddStringBearer<byte>
            {
                Value = 255
            }, "{0[1..2]}")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "5"
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 255
            }, "{0[1..2]}")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "5"
                }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 255
            }, "{0[1..2]}")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "5"
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<byte>>
            (new FieldSpanFormattableAlwaysAddStringBearer<byte>
            {
                Value = 255
            }, "{0[1..]}")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "55"
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 255
            }, "{0[1..]}")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "55"
                }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 255
            }, "{0[1..]}")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "55"
                }
            }

        // byte?
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<byte>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<byte>
            {
                Value = 0
            }, "", true, new FieldNullableSpanFormattableAlwaysAddStringBearer<byte>
            {
                Value = 0
            })
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "0" }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 0
            }, "{0}", true, new FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 0
            })
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "0" }
            }
      , new NullableStringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 0
            }, "{0}", true, new FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 0
            })
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "0" }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<byte>>
            (null, "", true, new FieldNullableSpanFormattableAlwaysAddStringBearer<byte>
            {
                Value = 1
            })
            {
                { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString
                       , AnyLog ) , "FieldNullableSpanFormattableAlwaysAddStringBearer<byte>(1)" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue  | DefaultBecomesFallbackString
                       , AnyLog ) , "\"FieldNullableSpanFormattableAlwaysAddStringBearer<byte>(1)\"" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)
                  , "FieldNullableSpanFormattableAlwaysAddStringBearer\\u003cbyte\\u003e(1)" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue) 
                  , "\"FieldNullableSpanFormattableAlwaysAddStringBearer\\u003cbyte\\u003e(1)\"" }
                // uses Default value which is re-encoded escaping \u003c to \\u003c etc...
              , { new EK(IsContentType | CallsViaMatch  | DefaultBecomesFallbackString) 
                  , "FieldNullableSpanFormattableAlwaysAddStringBearer\\\\u003cbyte\\\\u003e(1)" }
              , { new EK(IsContentType | CallsViaMatch ), "null" }
              , { new EK(IsContentType | AcceptsNonNullableStructStringBearer | DefaultBecomesEmpty) , "" }
              , { new EK(IsContentType | AcceptsNonNullableStructStringBearer | DefaultBecomesZero) , "0" }
              , { new EK(IsContentType | AcceptsNonNullableStructStringBearer | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue, AnyLog)
                  , "FieldNullableSpanFormattableAlwaysAddStringBearer<byte>(1)" }
              , { new EK(IsContentType | AcceptsNonNullableStructStringBearer | DefaultBecomesFallbackValue | DefaultBecomesFallbackString
                       , AnyJson)
                  , "FieldNullableSpanFormattableAlwaysAddStringBearer\\u003cbyte\\u003e(1)" }
              // , { new EK(IsContentType | AcceptsNonNullableStructStringBearer | DefaultBecomesEmpty) , "\"\"" }
              // , { new EK(IsContentType | AcceptsNonNullableStructStringBearer | DefaultBecomesZero) , "\"0\"" }
              // , { new EK(IsContentType | AcceptsNonNullableStructStringBearer | DefaultBecomesFallbackValue| DefaultBecomesFallbackString
              //          , AnyLog) , "\"FieldNullableSpanFormattableAlwaysAddStringBearer<byte>(1)\"" }
              // , { new EK(IsContentType | AcceptsNonNullableStructStringBearer | DefaultBecomesFallbackValue, AnyJson)
              //     , "\"FieldNullableSpanFormattableAlwaysAddStringBearer\\u003cbyte\\u003e(1)\"" }
              // , { new EK(IsContentType | AcceptsNonNullableStructStringBearer | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue 
              //          | DefaultBecomesFallbackString, AnyLog ) , "\"FieldNullableSpanFormattableAlwaysAddStringBearer<byte>(1)\"" }
              // , { new EK(IsContentType | AcceptsNonNullableStructStringBearer | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
              //          | DefaultBecomesFallbackString) , "FieldNullableSpanFormattableAlwaysAddStringBearer\\u003cbyte\\u003e(1)" }
              // , { new EK(IsContentType | AcceptsNonNullableStructStringBearer | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue 
              //          | DefaultBecomesFallbackString) , "\"FieldNullableSpanFormattableAlwaysAddStringBearer\\u003cbyte\\u003e(1)\"" }
              , { new EK(AcceptsNonNullableStructStringBearer | AlwaysWrites | NonDefaultWrites), "null" }
            }
      , new NullableStringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>>
            (null, "", true, new FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 1
            })
            {
                { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue| DefaultBecomesFallbackString
                       , AnyLog ) , "FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>(1)" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString
                       , AnyLog ) , "\"FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>(1)\"" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue) 
                  , "FieldNullableSpanFormattableAlwaysAddStructStringBearer\\u003cbyte\\u003e(1)" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue) 
                  , "\"FieldNullableSpanFormattableAlwaysAddStructStringBearer\\u003cbyte\\u003e(1)\"" }
                // uses Default value which is re-encoded escaping \u003c to \\u003c etc...
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackString) 
                  , "FieldNullableSpanFormattableAlwaysAddStructStringBearer\\\\u003cbyte\\\\u003e(1)" }
              , { new EK(IsContentType | CallsViaMatch ), "null" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsValueOut | DefaultBecomesEmpty, AnyLog) , "" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsValueOut | DefaultBecomesZero, AnyLog) , "0" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesEmpty, AnyLog) , "\"\"" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesZero, AnyLog) , "\"0\"" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultBecomesEmpty) , "" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultBecomesZero) , "0" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString, AnyLog ) , "FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>(1)" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString, AnyLog ) , "\"FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>(1)\"" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString), "FieldNullableSpanFormattableAlwaysAddStructStringBearer\\u003cbyte\\u003e(1)" }
              , { new EK(AcceptsAnyNullableStruct | AlwaysWrites | NonDefaultWrites), "null" }
            }
      , new NullableStringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>>
            (null, "{0}", true, new FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 1
            })
            {
                { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString
                       , AnyLog ), "FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>(1)" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString
                       , AnyLog ), "\"FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>(1)\"" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue) 
                  , "FieldNullableSpanFormattableAlwaysAddStructStringBearer\\u003cbyte\\u003e(1)" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue) 
                  , "\"FieldNullableSpanFormattableAlwaysAddStructStringBearer\\u003cbyte\\u003e(1)\"" }
                // uses Default value which is re-encoded escaping \u003c to \\u003c etc...
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackString) 
                  , "FieldNullableSpanFormattableAlwaysAddStructStringBearer\\\\u003cbyte\\\\u003e(1)" }
              , { new EK(IsContentType | CallsViaMatch ), "null" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsValueOut | DefaultBecomesEmpty, AnyLog) , "" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsValueOut | DefaultBecomesZero, AnyLog) , "0" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesEmpty, AnyLog) , "\"\"" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesZero, AnyLog) , "\"0\"" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultBecomesEmpty) , "" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultBecomesZero) , "0" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString, AnyLog ), "FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>(1)" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString, AnyLog ), "\"FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>(1)\"" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString), "FieldNullableSpanFormattableAlwaysAddStructStringBearer\\u003cbyte\\u003e(1)" }
                //   // uses Default value which is re-encoded escaping \u003c to \\u003c etc...
                // , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultBecomesFallbackString) 
                //     , "FieldNullableSpanFormattableAlwaysAddStructStringBearer\\\\u003cbyte\\\\u003e(1)" }
              , { new EK(AcceptsAnyNullableStruct | AlwaysWrites | NonDefaultWrites), "null" }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 255
            })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "255"
                }
            }
      , new NullableStringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 255
            })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "255"
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<byte>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<byte>
            {
                Value = 128
            }, "C2")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "$128.00"
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 128
            }, "C2")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "$128.00"
                }
            }
      , new NullableStringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 128
            }, "C2")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "$128.00"
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<byte>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<byte>
            {
                Value = 144
            }, "\"{0,20}\"")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"                 144\""
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 144
            }, "\"{0,20}\"")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"                 144\""
                }
            }
      , new NullableStringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 144
            }, "\"{0,20}\"")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"                 144\""
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<byte>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<byte>
            {
                Value = 64
            }, "", true, new FieldNullableSpanFormattableAlwaysAddStringBearer<byte>
            {
                Value = 64
            })
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "64" }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 64
            }, "", true, new FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 64
            })
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "64" }
            }
      , new NullableStringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 64
            }, "", true, new FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 64
            })
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "64" }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<byte>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<byte>
            {
                Value = 255
            }, "{0[..1]}")
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites), "2" }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 255
            }, "{0[..1]}")
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites), "2" }
            }
      , new NullableStringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 255
            }, "{0[..1]}")
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites), "2" }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<byte>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<byte>
            {
                Value = 255
            }, "{0[1..2]}")
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites), "5" }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 255
            }, "{0[1..2]}")
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites), "5" }
            }
      , new NullableStringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 255
            }, "{0[1..2]}")
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites), "5" }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<byte>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<byte>
            {
                Value = 255
            }, "{0[1..]}")
            { { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites), "55" } }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 255
            }, "{0[1..]}")
            { { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites), "55" } }
      , new NullableStringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>
            {
                Value = 255
            }, "{0[1..]}")
            { { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites), "55" } }

        // DateTime
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<DateTime>>
            (new FieldSpanFormattableAlwaysAddStringBearer<DateTime>
            {
                Value = DateTime.MinValue
            }, "O", true, new FieldSpanFormattableAlwaysAddStringBearer<DateTime>
            {
                Value = DateTime.MinValue
            })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, AnyLog
                          )
                  , "0001-01-01T00:00:00.0000000"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty
                          )
                  , "\"0001-01-01T00:00:00.0000000\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>
            {
                Value = DateTime.MinValue
            }, "O", true, new FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>
            {
                Value = DateTime.MinValue
            })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, AnyLog
                          )
                  , "0001-01-01T00:00:00.0000000"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty
                          )
                  , "\"0001-01-01T00:00:00.0000000\""
                }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>
            {
                Value = DateTime.MinValue
            }, "O", true, new FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>
            {
                Value = DateTime.MinValue
            })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, AnyLog
                          )
                  , "0001-01-01T00:00:00.0000000"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty
                          )
                  , "\"0001-01-01T00:00:00.0000000\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<DateTime>>
            (new FieldSpanFormattableAlwaysAddStringBearer<DateTime>
            {
                Value = new DateTime(2000, 1, 1, 1, 1, 1).AddTicks(1111111)
            }, "o")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "2000-01-01T01:01:01.1111111"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"2000-01-01T01:01:01.1111111\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>
            {
                Value = new DateTime(2000, 1, 1, 1, 1, 1).AddTicks(1111111)
            }, "o")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "2000-01-01T01:01:01.1111111"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"2000-01-01T01:01:01.1111111\""
                }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>
            {
                Value = new DateTime(2000, 1, 1, 1, 1, 1).AddTicks(1111111)
            }, "o")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "2000-01-01T01:01:01.1111111"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"2000-01-01T01:01:01.1111111\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<DateTime>>
            (new FieldSpanFormattableAlwaysAddStringBearer<DateTime>
             {
                 Value = new DateTime(2020, 2, 2).AddTicks(2222222)
             }, "s", true
           , new FieldSpanFormattableAlwaysAddStringBearer<DateTime>
             {
                 Value = new DateTime(2020, 2, 2).AddTicks(2222222)
             })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , AnyLog)
                  , "2020-02-02T00:00:00"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , Json | Compact | Pretty)
                  , "\"2020-02-02T00:00:00\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>
             {
                 Value = new DateTime(2020, 2, 2).AddTicks(2222222)
             }, "s", true
           , new FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>
             {
                 Value = new DateTime(2020, 2, 2).AddTicks(2222222)
             })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , AnyLog)
                  , "2020-02-02T00:00:00"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , Json | Compact | Pretty)
                  , "\"2020-02-02T00:00:00\""
                }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>
             {
                 Value = new DateTime(2020, 2, 2).AddTicks(2222222)
             }, "s", true
           , new FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>
             {
                 Value = new DateTime(2020, 2, 2).AddTicks(2222222)
             })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , AnyLog)
                  , "2020-02-02T00:00:00"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , Json | Compact | Pretty)
                  , "\"2020-02-02T00:00:00\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<DateTime>>
            (new FieldSpanFormattableAlwaysAddStringBearer<DateTime>
            {
                Value = DateTime.MaxValue
            }, "'{0:u}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "'9999-12-31 23:59:59Z'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'9999-12-31 23:59:59Z'\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>
            {
                Value = DateTime.MaxValue
            }, "'{0:u}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "'9999-12-31 23:59:59Z'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'9999-12-31 23:59:59Z'\""
                }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>
            {
                Value = DateTime.MaxValue
            }, "'{0:u}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "'9999-12-31 23:59:59Z'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'9999-12-31 23:59:59Z'\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<DateTime>>
            (new FieldSpanFormattableAlwaysAddStringBearer<DateTime>
             {
                 Value = DateTime.MinValue
             }
           , "\"{0,30:u}\"", true,
             new FieldSpanFormattableAlwaysAddStringBearer<DateTime>
             {
                 Value = new DateTime(2020, 1, 1)
             })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"          0001-01-01 00:00:00Z\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>
             {
                 Value = DateTime.MinValue
             }
           , "\"{0,30:u}\"", true,
             new FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>
             {
                 Value = new DateTime(2020, 1, 1)
             })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"          0001-01-01 00:00:00Z\""
                }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>
             {
                 Value = DateTime.MinValue
             }
           , "\"{0,30:u}\"", true,
             new FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>
             {
                 Value = new DateTime(2020, 1, 1)
             })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"          0001-01-01 00:00:00Z\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<DateTime>>
            (new FieldSpanFormattableAlwaysAddStringBearer<DateTime>
            {
                Value = new DateTime(1980, 7, 31, 11, 48, 13)
            }, "'{0:yyyy-MM-dd HH:mm:ss}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "'1980-07-31 11:48:13'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'1980-07-31 11:48:13'\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>
            {
                Value = new DateTime(1980, 7, 31, 11, 48, 13)
            }, "'{0:yyyy-MM-dd HH:mm:ss}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "'1980-07-31 11:48:13'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'1980-07-31 11:48:13'\""
                }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>
            {
                Value = new DateTime(1980, 7, 31, 11, 48, 13)
            }, "'{0:yyyy-MM-dd HH:mm:ss}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "'1980-07-31 11:48:13'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'1980-07-31 11:48:13'\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<DateTime>>
            (new FieldSpanFormattableAlwaysAddStringBearer<DateTime>
            {
                Value = new DateTime(2009, 11, 12, 19, 49, 0)
            }, "\"{0,-30:O}\"")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"2009-11-12T19:49:00.0000000   \""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>
            {
                Value = new DateTime(2009, 11, 12, 19, 49, 0)
            }, "\"{0,-30:O}\"")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"2009-11-12T19:49:00.0000000   \""
                }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>
            {
                Value = new DateTime(2009, 11, 12, 19, 49, 0)
            }, "\"{0,-30:O}\"")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"2009-11-12T19:49:00.0000000   \""
                }
            }

        // DateTime?
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>
             {
                 Value = DateTime.MinValue
             }
           , "O", true, new FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>
             {
                 Value = DateTime.MinValue
             })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , AnyLog)
                  , "0001-01-01T00:00:00.0000000"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , Json | Compact | Pretty)
                  , "\"0001-01-01T00:00:00.0000000\""
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>
             {
                 Value = DateTime.MinValue
             }
           , "O", true, new FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>
             {
                 Value = DateTime.MinValue
             })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , AnyLog)
                  , "0001-01-01T00:00:00.0000000"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , Json | Compact | Pretty)
                  , "\"0001-01-01T00:00:00.0000000\""
                }
            }
      , new NullableStringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>
             {
                 Value = DateTime.MinValue
             }
           , "O", true, new FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>
             {
                 Value = DateTime.MinValue
             })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , AnyLog)
                  , "0001-01-01T00:00:00.0000000"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , Json | Compact | Pretty)
                  , "\"0001-01-01T00:00:00.0000000\""
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>>
            (null, "", true
           , new FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>
             {
                 Value = new DateTime(2020, 2, 2).AddTicks(2222222)
             })
            {
                { new EK(IsContentType | CallsViaMatch | DefaultBecomesNull) , "null" }
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackString
                       , AnyLog ) , "FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>(2/2/2020 12:00:00 AM)" }
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackValue | DefaultTreatedAsStringOut | DefaultBecomesFallbackString
                       , AnyLog ) , "\"FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>(2/2/2020 12:00:00 AM)\"" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue ) 
                  , "FieldNullableSpanFormattableAlwaysAddStringBearer\\u003cDateTime\\u003e(2/2/2020 12:00:00 AM)" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue ) 
                  , "\"FieldNullableSpanFormattableAlwaysAddStringBearer\\u003cDateTime\\u003e(2/2/2020 12:00:00 AM)\"" }
                // uses Default value which is re-encoded escaping \u003c to \\u003c etc...
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackString) 
                  , "FieldNullableSpanFormattableAlwaysAddStringBearer\\\\u003cDateTime\\\\u003e(2/2/2020 12:00:00 AM)" }
              , { new EK(IsContentType | CallsViaMatch), "null" }
              , { new EK(IsContentType | AcceptsNonNullableStructStringBearer | DefaultBecomesEmpty) , "" }
              , { new EK(IsContentType | AcceptsNonNullableStructStringBearer | DefaultBecomesZero) , "0" }
              , { new EK(IsContentType | AcceptsNonNullableStructStringBearer | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                       | DefaultBecomesFallbackString, AnyLog )
                  , "FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>(2/2/2020 12:00:00 AM)" }
              , { new EK(IsContentType | AcceptsNonNullableStructStringBearer | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString, AnyLog ) , "\"FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>(2/2/2020 12:00:00 AM)\"" }
              , { new EK(IsContentType | AcceptsNonNullableStructStringBearer | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString) 
                  , "FieldNullableSpanFormattableAlwaysAddStringBearer\\u003cDateTime\\u003e(2/2/2020 12:00:00 AM)" }
              , { new EK(IsContentType | AcceptsNonNullableStructStringBearer | DefaultBecomesEmpty), "" }
              , { new EK(AcceptsNonNullableStructStringBearer | AlwaysWrites | NonDefaultWrites), "null" }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (default, "", true
           , new FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>
             {
                 Value = new DateTime(2020, 2, 2).AddTicks(2222222)
             })
            {
                { new EK(IsContentType | CallsViaMatch) , "null" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultBecomesEmpty | DefaultBecomesNull ) , "null" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultBecomesZero) , "0" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesNull, AnyLog ) , "\"FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>(2/2/2020 12:00:00 AM)\"" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultBecomesFallbackValue | DefaultBecomesNull) 
                  , "FieldNullableSpanFormattableAlwaysAddStringBearer\\u003cDateTime\\u003e(2/2/2020 12:00:00 AM)" }
              //   // uses Default value which is re-encoded escaping \u003c to \\u003c etc...
              // , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultBecomesFallbackString) 
              //     , "FieldNullableSpanFormattableAlwaysAddStringBearer\\\\u003cDateTime\\\\u003e(2/2/2020 12:00:00 AM)" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultBecomesNull) , "null" }  
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultBecomesFallbackValue  | DefaultBecomesFallbackString)
                  , "null" }  
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultBecomesEmpty), "" }
              , { new EK(AcceptsAnyExceptNullableStruct | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites), "null" }
            }
      , new NullableStringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (null, "", true
           , new FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>
             {
                 Value = new DateTime(2020, 2, 2).AddTicks(2222222)
             })
            {
                { new EK(IsContentType | CallsViaMatch | DefaultBecomesNull) , "null" }
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackString
                       , AnyLog ) , "FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>(2/2/2020 12:00:00 AM)" }
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackValue | DefaultTreatedAsStringOut | DefaultBecomesFallbackString
                       , AnyLog ) , "\"FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>(2/2/2020 12:00:00 AM)\"" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue) 
                  , "FieldNullableSpanFormattableAlwaysAddStructStringBearer\\u003cDateTime\\u003e(2/2/2020 12:00:00 AM)" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue) 
                  , "\"FieldNullableSpanFormattableAlwaysAddStructStringBearer\\u003cDateTime\\u003e(2/2/2020 12:00:00 AM)\"" }
                // uses Default value which is re-encoded escaping \u003c to \\u003c etc...
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackString) 
                  , "FieldNullableSpanFormattableAlwaysAddStructStringBearer\\\\u003cDateTime\\\\u003e(2/2/2020 12:00:00 AM)" }
              , { new EK(IsContentType | CallsViaMatch), "null" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsValueOut | DefaultBecomesEmpty, AnyLog) , "" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsValueOut | DefaultBecomesZero, AnyLog) , "0" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesEmpty, AnyLog) , "\"\"" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesZero, AnyLog) , "\"0\"" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultBecomesEmpty) , "" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultBecomesZero) , "0" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                       | DefaultBecomesFallbackString, AnyLog )
                  , "FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>(2/2/2020 12:00:00 AM)" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString, AnyLog ) , "\"FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>(2/2/2020 12:00:00 AM)\"" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString) 
                  , "FieldNullableSpanFormattableAlwaysAddStructStringBearer\\u003cDateTime\\u003e(2/2/2020 12:00:00 AM)" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultBecomesEmpty), "" }
              , { new EK(AcceptsAnyNullableStruct | AlwaysWrites | NonDefaultWrites), "null" }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>>
            (null)
            {
                { new EK(IsContentType | CallsViaMatch), "null" }
              , { new EK(IsContentType | AcceptsNonNullableStructStringBearer | DefaultBecomesEmpty) , "" }
              , { new EK(AcceptsNonNullableStructStringBearer | AlwaysWrites), "null" }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (default)
            {
                { new EK(IsContentType | CallsViaMatch), "null" }
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultBecomesFallbackValue) , "null" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "null" }
            }
      , new NullableStringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (null, null, true, new FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>())
            {
                { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackString
                       , AnyLog ) , "FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>()" }
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackValue | DefaultTreatedAsStringOut | DefaultBecomesFallbackString
                       , AnyLog ) , "\"FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>()\"" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue) 
                  , "FieldNullableSpanFormattableAlwaysAddStructStringBearer\\u003cDateTime\\u003e()" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue) 
                  , "\"FieldNullableSpanFormattableAlwaysAddStructStringBearer\\u003cDateTime\\u003e()\"" }
                // uses Default value which is re-encoded escaping \u003c to \\u003c etc...
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackString) 
                  , "FieldNullableSpanFormattableAlwaysAddStructStringBearer\\\\u003cDateTime\\\\u003e()" }
              , { new EK(IsContentType | CallsViaMatch), "null" }
              , { new EK(IsContentType | AcceptsNullableStructStringBearer | DefaultTreatedAsValueOut | DefaultBecomesEmpty, AnyLog) , "" }
              , { new EK(IsContentType | AcceptsNullableStructStringBearer | DefaultTreatedAsValueOut | DefaultBecomesZero, AnyLog) , "0" }
              , { new EK(IsContentType | AcceptsNullableStructStringBearer | DefaultTreatedAsStringOut | DefaultBecomesEmpty, AnyLog) , "\"\"" }
              , { new EK(IsContentType | AcceptsNullableStructStringBearer | DefaultTreatedAsStringOut | DefaultBecomesZero, AnyLog) , "\"0\"" }
              , { new EK(IsContentType | AcceptsNullableStructStringBearer | DefaultBecomesEmpty) , "" }
              , { new EK(IsContentType | AcceptsNullableStructStringBearer | DefaultBecomesZero) , "0" }
              , { new EK(IsContentType | AcceptsNullableStructStringBearer | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                       | DefaultBecomesFallbackString, AnyLog )
                  , "FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>()" }
              , { new EK(IsContentType | AcceptsNullableStructStringBearer | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString, AnyLog ) , "\"FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>()\"" }
              , { new EK(IsContentType | AcceptsNullableStructStringBearer | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString) 
                  , "FieldNullableSpanFormattableAlwaysAddStructStringBearer\\u003cDateTime\\u003e()" }
              , { new EK(IsContentType | AcceptsNullableStructStringBearer | DefaultBecomesEmpty), "" }
              , { new EK(AcceptsNullableStructStringBearer | AlwaysWrites | NonDefaultWrites), "null" }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>
             {
                 Value = new DateTime(2000, 1, 1, 1, 1, 1).AddTicks(1111111)
             }
           , "o")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "2000-01-01T01:01:01.1111111"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"2000-01-01T01:01:01.1111111\""
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>
             {
                 Value = new DateTime(2000, 1, 1, 1, 1, 1).AddTicks(1111111)
             }
           , "o")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "2000-01-01T01:01:01.1111111"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"2000-01-01T01:01:01.1111111\""
                }
            }
      , new NullableStringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>
             {
                 Value = new DateTime(2000, 1, 1, 1, 1, 1).AddTicks(1111111)
             }
           , "o")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "2000-01-01T01:01:01.1111111"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"2000-01-01T01:01:01.1111111\""
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>
             {
                 Value = new DateTime(2020, 2, 2).AddTicks(2222222)
             }
           , "s", true
           , new FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>
             {
                 Value = new DateTime(2020, 2, 2).AddTicks(2222222)
             })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , AnyLog)
                  , "2020-02-02T00:00:00"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , Json | Compact | Pretty)
                  , "\"2020-02-02T00:00:00\""
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>
             {
                 Value = new DateTime(2020, 2, 2).AddTicks(2222222)
             }
           , "s", true
           , new FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>
             {
                 Value = new DateTime(2020, 2, 2).AddTicks(2222222)
             })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , AnyLog)
                  , "2020-02-02T00:00:00"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , Json | Compact | Pretty)
                  , "\"2020-02-02T00:00:00\""
                }
            }
      , new NullableStringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>
             {
                 Value = new DateTime(2020, 2, 2).AddTicks(2222222)
             }
           , "s", true
           , new FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>
             {
                 Value = new DateTime(2020, 2, 2).AddTicks(2222222)
             })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , AnyLog)
                  , "2020-02-02T00:00:00"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , Json | Compact | Pretty)
                  , "\"2020-02-02T00:00:00\""
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>
             {
                 Value = DateTime.MaxValue
             }
           , "'{0:u}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "'9999-12-31 23:59:59Z'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'9999-12-31 23:59:59Z'\""
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>
             {
                 Value = DateTime.MaxValue
             }
           , "'{0:u}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "'9999-12-31 23:59:59Z'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'9999-12-31 23:59:59Z'\""
                }
            }
      , new NullableStringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>
             {
                 Value = DateTime.MaxValue
             }
           , "'{0:u}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "'9999-12-31 23:59:59Z'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'9999-12-31 23:59:59Z'\""
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>
             {
                 Value = DateTime.MinValue
             }
           , "\"{0,30:u}\"", true
           , new FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>
             {
                 Value = new DateTime(2020, 1, 1)
             })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"          0001-01-01 00:00:00Z\""
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>
             {
                 Value = DateTime.MinValue
             }
           , "\"{0,30:u}\"", true
           , new FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>
             {
                 Value = new DateTime(2020, 1, 1)
             })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"          0001-01-01 00:00:00Z\""
                }
            }
      , new NullableStringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>
             {
                 Value = DateTime.MinValue
             }
           , "\"{0,30:u}\"", true
           , new FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>
             {
                 Value = new DateTime(2020, 1, 1)
             })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"          0001-01-01 00:00:00Z\""
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>
             {
                 Value = new DateTime(1980, 7, 31, 11, 48, 13)
             }
           , "'{0:yyyy-MM-dd HH:mm:ss}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "'1980-07-31 11:48:13'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'1980-07-31 11:48:13'\""
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>
             {
                 Value = new DateTime(1980, 7, 31, 11, 48, 13)
             }
           , "'{0:yyyy-MM-dd HH:mm:ss}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "'1980-07-31 11:48:13'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'1980-07-31 11:48:13'\""
                }
            }
      , new NullableStringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>
             {
                 Value = new DateTime(1980, 7, 31, 11, 48, 13)
             }
           , "'{0:yyyy-MM-dd HH:mm:ss}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "'1980-07-31 11:48:13'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'1980-07-31 11:48:13'\""
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>
            {
                Value = new DateTime(2009, 11, 12, 19, 49, 0)
            }, "\"{0,-30:O}\"")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"2009-11-12T19:49:00.0000000   \""
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>
            {
                Value = new DateTime(2009, 11, 12, 19, 49, 0)
            }, "\"{0,-30:O}\"")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"2009-11-12T19:49:00.0000000   \""
                }
            }
      , new NullableStringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>
            {
                Value = new DateTime(2009, 11, 12, 19, 49, 0)
            }, "\"{0,-30:O}\"")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"2009-11-12T19:49:00.0000000   \""
                }
            }

        // TimeSpan
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (new FieldSpanFormattableAlwaysAddStringBearer<TimeSpan>
            {
                Value = TimeSpan.Zero
            }, "g", true, new FieldSpanFormattableAlwaysAddStringBearer<TimeSpan>
            {
                Value = TimeSpan.Zero
            })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , AnyLog)
                  , "0:00:00"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , Json | Compact | Pretty)
                  , "\"0:00:00\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
            {
                Value = TimeSpan.Zero
            }, "g", true, new FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
            {
                Value = TimeSpan.Zero
            })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , AnyLog)
                  , "0:00:00"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , Json | Compact | Pretty)
                  , "\"0:00:00\""
                }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
            {
                Value = TimeSpan.Zero
            }, "g", true, new FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
            {
                Value = TimeSpan.Zero
            })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , AnyLog)
                  , "0:00:00"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , Json | Compact | Pretty)
                  , "\"0:00:00\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (new FieldSpanFormattableAlwaysAddStringBearer<TimeSpan>
            {
                Value = new TimeSpan(1, 1, 1, 1, 111, 111)
            }, "c")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "1.01:01:01.1111110"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"1.01:01:01.1111110\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
            {
                Value = new TimeSpan(1, 1, 1, 1, 111, 111)
            }, "c")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "1.01:01:01.1111110"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"1.01:01:01.1111110\""
                }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
            {
                Value = new TimeSpan(1, 1, 1, 1, 111, 111)
            }, "c")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "1.01:01:01.1111110"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"1.01:01:01.1111110\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (new FieldSpanFormattableAlwaysAddStringBearer<TimeSpan>
             {
                 Value = new TimeSpan(-2, -22, -22, -22, -222, -222)
             }, "G", true
           , new FieldSpanFormattableAlwaysAddStringBearer<TimeSpan>
             {
                 Value = new TimeSpan(-2, -22, -22, -22, -222, -222)
             })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , AnyLog)
                  , "-2:22:22:22.2222220"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , Json | Compact | Pretty)
                  , "\"-2:22:22:22.2222220\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
             {
                 Value = new TimeSpan(-2, -22, -22, -22, -222, -222)
             }, "G", true
           , new FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
             {
                 Value = new TimeSpan(-2, -22, -22, -22, -222, -222)
             })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , AnyLog)
                  , "-2:22:22:22.2222220"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , Json | Compact | Pretty)
                  , "\"-2:22:22:22.2222220\""
                }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
             {
                 Value = new TimeSpan(-2, -22, -22, -22, -222, -222)
             }, "G", true
           , new FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
             {
                 Value = new TimeSpan(-2, -22, -22, -22, -222, -222)
             })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , AnyLog)
                  , "-2:22:22:22.2222220"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , Json | Compact | Pretty)
                  , "\"-2:22:22:22.2222220\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (new FieldSpanFormattableAlwaysAddStringBearer<TimeSpan>
            {
                Value = TimeSpan.MaxValue
            }, "'{0:G}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "'10675199:02:48:05.4775807'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'10675199:02:48:05.4775807'\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
            {
                Value = TimeSpan.MaxValue
            }, "'{0:G}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "'10675199:02:48:05.4775807'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'10675199:02:48:05.4775807'\""
                }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
            {
                Value = TimeSpan.MaxValue
            }, "'{0:G}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "'10675199:02:48:05.4775807'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'10675199:02:48:05.4775807'\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (new FieldSpanFormattableAlwaysAddStringBearer<TimeSpan>
            {
                Value = TimeSpan.MinValue
            }, "\"{0,30:c}\"", true, new FieldSpanFormattableAlwaysAddStringBearer<TimeSpan>
            {
                Value = TimeSpan.Zero
            })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"    -10675199.02:48:05.4775808\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
            {
                Value = TimeSpan.MinValue
            }, "\"{0,30:c}\"", true, new FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
            {
                Value = TimeSpan.Zero
            })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"    -10675199.02:48:05.4775808\""
                }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
            {
                Value = TimeSpan.MinValue
            }, "\"{0,30:c}\"", true, new FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
            {
                Value = TimeSpan.Zero
            })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"    -10675199.02:48:05.4775808\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (new FieldSpanFormattableAlwaysAddStringBearer<TimeSpan>
            {
                Value = new TimeSpan(3, 3, 33, 33, 333, 333)
            }, "'{0:dd\\-hh\\-mm\\-ss\\.fff}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "'03-03-33-33.333'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'03-03-33-33.333'\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
            {
                Value = new TimeSpan(3, 3, 33, 33, 333, 333)
            }, "'{0:dd\\-hh\\-mm\\-ss\\.fff}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "'03-03-33-33.333'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'03-03-33-33.333'\""
                }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
            {
                Value = new TimeSpan(3, 3, 33, 33, 333, 333)
            }, "'{0:dd\\-hh\\-mm\\-ss\\.fff}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "'03-03-33-33.333'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'03-03-33-33.333'\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (new FieldSpanFormattableAlwaysAddStringBearer<TimeSpan>
            {
                Value = new TimeSpan(-4, -4, -44, -44, -444, -444)
            }, "\"{0,-30:G}\"")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"-4:04:44:44.4444440           \""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
            {
                Value = new TimeSpan(-4, -4, -44, -44, -444, -444)
            }, "\"{0,-30:G}\"")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"-4:04:44:44.4444440           \""
                }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
            {
                Value = new TimeSpan(-4, -4, -44, -44, -444, -444)
            }, "\"{0,-30:G}\"")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"-4:04:44:44.4444440           \""
                }
            }

        // TimeSpan?
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>
             {
                 Value = TimeSpan.Zero
             }
           , "g", true, new FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>
             {
                 Value = TimeSpan.Zero
             })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , AnyLog)
                  , "0:00:00"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , Json | Compact | Pretty)
                  , "\"0:00:00\""
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
             {
                 Value = TimeSpan.Zero
             }
           , "g", true, new FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
             {
                 Value = TimeSpan.Zero
             })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , AnyLog)
                  , "0:00:00"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , Json | Compact | Pretty)
                  , "\"0:00:00\""
                }
            }
      , new NullableStringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
             {
                 Value = TimeSpan.Zero
             }
           , "g", true, new FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
             {
                 Value = TimeSpan.Zero
             })
            {
                {
                    new EK(AcceptsAnyNullableStruct | AlwaysWrites | NonNullWrites
                         , AnyLog)
                  , "0:00:00"
                }
               ,
                {
                    new EK(AcceptsAnyNullableStruct | AlwaysWrites | NonNullWrites
                         , Json | Compact | Pretty)
                  , "\"0:00:00\""
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (null, "", true
           , new FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>
             {
                 Value = TimeSpan.FromSeconds(1)
             })
            {
                { new EK(IsContentType | CallsViaMatch | DefaultBecomesNull) , "null" }
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackValue | DefaultTreatedAsValueOut 
                       | DefaultBecomesFallbackString, AnyLog ), "FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>(00:00:01)" }
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackValue | DefaultTreatedAsStringOut 
                       | DefaultBecomesFallbackString, AnyLog ), "\"FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>(00:00:01)\"" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue) 
                  , "FieldNullableSpanFormattableAlwaysAddStringBearer\\u003cTimeSpan\\u003e(00:00:01)" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue) 
                  , "\"FieldNullableSpanFormattableAlwaysAddStringBearer\\u003cTimeSpan\\u003e(00:00:01)\"" }
                // uses Default value which is re-encoded escaping \u003c to \\u003c etc...
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackString) 
                  , "FieldNullableSpanFormattableAlwaysAddStringBearer\\\\u003cTimeSpan\\\\u003e(00:00:01)" }
                
              , { new EK(IsContentType | CallsViaMatch), "null" }
              , { new EK(IsContentType | AcceptsNonNullableStructStringBearer | DefaultBecomesEmpty) , "" }
              , { new EK(IsContentType | AcceptsNonNullableStructStringBearer | DefaultBecomesZero) , "0" }
              , { new EK(IsContentType | AcceptsNonNullableStructStringBearer | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                       | DefaultBecomesFallbackString, AnyLog )
                  , "FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>(00:00:01)" }
              , { new EK(IsContentType | AcceptsNonNullableStructStringBearer | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString, AnyLog ) , "\"FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>(00:00:01)\"" }
              , { new EK(IsContentType | AcceptsNonNullableStructStringBearer | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString) 
                  , "FieldNullableSpanFormattableAlwaysAddStringBearer\\u003cTimeSpan\\u003e(00:00:01)" }
              , { new EK(IsContentType | AcceptsNonNullableStructStringBearer | DefaultBecomesEmpty), "" }
              , { new EK(IsContentType | AcceptsNonNullableStructStringBearer | DefaultBecomesEmpty)
                  , "" }
              , { new EK(AcceptsNonNullableStructStringBearer | AlwaysWrites | NonDefaultWrites), "null" }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (default, "", true
           , new FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
             {
                 Value = TimeSpan.FromSeconds(1)
             })
            {
                { new EK(IsContentType | CallsViaMatch), "null" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultBecomesEmpty) , "null" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultBecomesZero) , "0" }
              , { new EK(AcceptsAnyExceptNullableStruct | AlwaysWrites | NonDefaultWrites | NonNullWrites 
                       | NonNullAndPopulatedWrites), "null" }
            }
      , new NullableStringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (null, "", true
           , new FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
             {
                 Value = TimeSpan.FromSeconds(1)
             })
            {
               { new EK(IsContentType | CallsViaMatch | DefaultBecomesNull) , "null" }
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackString
                       , AnyLog ) , "FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>(00:00:01)" }
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackValue | DefaultTreatedAsStringOut | DefaultBecomesFallbackString
                       , AnyLog ) , "\"FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>(00:00:01)\"" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue) 
                  , "FieldNullableSpanFormattableAlwaysAddStructStringBearer\\u003cTimeSpan\\u003e(00:00:01)" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue) 
                  , "\"FieldNullableSpanFormattableAlwaysAddStructStringBearer\\u003cTimeSpan\\u003e(00:00:01)\"" }
                // uses Default value which is re-encoded escaping \u003c to \\u003c etc...
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackString) 
                  , "FieldNullableSpanFormattableAlwaysAddStructStringBearer\\\\u003cTimeSpan\\\\u003e(00:00:01)" }
              , { new EK(IsContentType | CallsViaMatch), "null" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesEmpty, AnyLog) , "\"\"" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesZero, AnyLog) , "\"0\"" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultTreatedAsValueOut | DefaultBecomesEmpty) , "" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultTreatedAsValueOut | DefaultBecomesZero) , "0" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                       | DefaultBecomesFallbackString, AnyLog )
                  , "FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>(00:00:01)" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString, AnyLog ) 
                  , "\"FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>(00:00:01)\"" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString) 
                  , "FieldNullableSpanFormattableAlwaysAddStructStringBearer\\u003cTimeSpan\\u003e(00:00:01)" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultBecomesEmpty), "" }
              , { new EK( AcceptsAnyNullableStruct | AlwaysWrites | NonDefaultWrites), "null" }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (null, "")
            {
                { new EK(IsContentType | CallsViaMatch), "null" }
              , { new EK(IsContentType | AcceptsNonNullableStructStringBearer | DefaultBecomesEmpty) , "" }
              , { new EK(IsContentType | AcceptsNonNullableStructStringBearer | DefaultBecomesZero) , "0" }
              , { new EK( AcceptsNonNullableStructStringBearer | AlwaysWrites), "null" }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (default, "")
            {
                { new EK(IsContentType | CallsViaMatch), "null" }
              , { new EK(IsContentType | AcceptsNullableStructStringBearer | DefaultBecomesEmpty) , "null" }
              , { new EK(IsContentType | AcceptsNullableStructStringBearer | DefaultBecomesZero) , "0" }
              , { new EK(IsContentType | AcceptsNullableStructStringBearer | DefaultBecomesEmpty)
                  , "null" }
              , { new EK(AcceptsNullableStructStringBearer | AlwaysWrites | NonNullWrites), "null" }
            }
      , new NullableStringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (null, "", true
           , new FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
             {
                 Value = null
             })
            {
                { new EK(IsContentType | CallsViaMatch | DefaultBecomesNull) , "null" }
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackString
                       , AnyLog ) , "FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>()" }
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackValue | DefaultTreatedAsStringOut | DefaultBecomesFallbackString
                       , AnyLog ) , "\"FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>()\"" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue) 
                  , "FieldNullableSpanFormattableAlwaysAddStructStringBearer\\u003cTimeSpan\\u003e()" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue) 
                  , "\"FieldNullableSpanFormattableAlwaysAddStructStringBearer\\u003cTimeSpan\\u003e()\"" }
                // uses Default value which is re-encoded escaping \u003c to \\u003c etc...
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackString) 
                  , "FieldNullableSpanFormattableAlwaysAddStructStringBearer\\\\u003cTimeSpan\\\\u003e()" }
              , { new EK(IsContentType | CallsViaMatch), "null" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesEmpty, AnyLog) , "\"\"" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesZero, AnyLog) , "\"0\"" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultTreatedAsValueOut | DefaultBecomesEmpty) , "" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultTreatedAsValueOut | DefaultBecomesZero) , "0" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                       | DefaultBecomesFallbackString, AnyLog )
                  , "FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>()" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString, AnyLog ) , "\"FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>()\"" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultBecomesFallbackValue
                       | DefaultBecomesFallbackString) 
                  , "FieldNullableSpanFormattableAlwaysAddStructStringBearer\\u003cTimeSpan\\u003e()" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultBecomesEmpty), "" }
              , { new EK(AcceptsAnyNullableStruct | AlwaysWrites | NonDefaultWrites), "null" }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>
            {
                Value = new TimeSpan(1, 1, 1, 1, 111, 111)
            }, "c")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "1.01:01:01.1111110"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"1.01:01:01.1111110\""
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
            {
                Value = new TimeSpan(1, 1, 1, 1, 111, 111)
            }, "c")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "1.01:01:01.1111110"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"1.01:01:01.1111110\""
                }
            }
      , new NullableStringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
            {
                Value = new TimeSpan(1, 1, 1, 1, 111, 111)
            }, "c")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "1.01:01:01.1111110"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"1.01:01:01.1111110\""
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>
             {
                 Value = new TimeSpan(-2, -22, -22, -22, -222, -222)
             }
           , "G", true
           , new FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>
             {
                 Value = new TimeSpan(-2, -22, -22, -22, -222, -222)
             })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , AnyLog)
                  , "-2:22:22:22.2222220"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , Json | Compact | Pretty)
                  , "\"-2:22:22:22.2222220\""
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
             {
                 Value = new TimeSpan(-2, -22, -22, -22, -222, -222)
             }
           , "G", true
           , new FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
             {
                 Value = new TimeSpan(-2, -22, -22, -22, -222, -222)
             })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , AnyLog)
                  , "-2:22:22:22.2222220"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , Json | Compact | Pretty)
                  , "\"-2:22:22:22.2222220\""
                }
            }
      , new NullableStringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
             {
                 Value = new TimeSpan(-2, -22, -22, -22, -222, -222)
             }
           , "G", true
           , new FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
             {
                 Value = new TimeSpan(-2, -22, -22, -22, -222, -222)
             })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , AnyLog)
                  , "-2:22:22:22.2222220"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , Json | Compact | Pretty)
                  , "\"-2:22:22:22.2222220\""
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>
            {
                Value = TimeSpan.MaxValue
            }, "'{0:G}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "'10675199:02:48:05.4775807'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'10675199:02:48:05.4775807'\""
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
            {
                Value = TimeSpan.MaxValue
            }, "'{0:G}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "'10675199:02:48:05.4775807'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'10675199:02:48:05.4775807'\""
                }
            }
      , new NullableStringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
            {
                Value = TimeSpan.MaxValue
            }, "'{0:G}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "'10675199:02:48:05.4775807'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'10675199:02:48:05.4775807'\""
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>
             {
                 Value = TimeSpan.MinValue
             }
           , "\"{0,30:c}\"", true
           , new FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>
             {
                 Value = TimeSpan.Zero
             })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"    -10675199.02:48:05.4775808\""
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
             {
                 Value = TimeSpan.MinValue
             }
           , "\"{0,30:c}\"", true
           , new FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
             {
                 Value = TimeSpan.Zero
             })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"    -10675199.02:48:05.4775808\""
                }
            }
      , new NullableStringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
             {
                 Value = TimeSpan.MinValue
             }
           , "\"{0,30:c}\"", true
           , new FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
             {
                 Value = TimeSpan.Zero
             })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"    -10675199.02:48:05.4775808\""
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>
            {
                Value = new TimeSpan(3, 3, 33, 33, 333, 333)
            }, "'{0:dd\\-hh\\-mm\\-ss\\.fff}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "'03-03-33-33.333'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'03-03-33-33.333'\""
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
            {
                Value = new TimeSpan(3, 3, 33, 33, 333, 333)
            }, "'{0:dd\\-hh\\-mm\\-ss\\.fff}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "'03-03-33-33.333'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'03-03-33-33.333'\""
                }
            }
      , new NullableStringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
            {
                Value = new TimeSpan(3, 3, 33, 33, 333, 333)
            }, "'{0:dd\\-hh\\-mm\\-ss\\.fff}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "'03-03-33-33.333'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'03-03-33-33.333'\""
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>
            {
                Value = new TimeSpan(-4, -4, -44, -44, -444, -444)
            }, "\"{0,-30:G}\"")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"-4:04:44:44.4444440           \""
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
            {
                Value = new TimeSpan(-4, -4, -44, -44, -444, -444)
            }, "\"{0,-30:G}\"")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"-4:04:44:44.4444440           \""
                }
            }
      , new NullableStringBearerExpect<FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>
            {
                Value = new TimeSpan(-4, -4, -44, -44, -444, -444)
            }, "\"{0,-30:G}\"")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"-4:04:44:44.4444440           \""
                }
            }

        //  IPAddress and IPAddress?
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<IPAddress>>
            (new FieldSpanFormattableAlwaysAddStringBearer<IPAddress>
            {
                Value = new IPAddress("\0\0\0\0"u8)
            }, null, true, new FieldSpanFormattableAlwaysAddStringBearer<IPAddress>
            {
                Value = new IPAddress("\0\0\0\0"u8)
            })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , AnyLog)
                  , "0.0.0.0"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , Json | Compact | Pretty)
                  , "\"0.0.0.0\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>
            {
                Value = new IPAddress("\0\0\0\0"u8)
            }, null, true, new FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>
            {
                Value = new IPAddress("\0\0\0\0"u8)
            })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , AnyLog)
                  , "0.0.0.0"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , Json | Compact | Pretty)
                  , "\"0.0.0.0\""
                }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>
            {
                Value = new IPAddress("\0\0\0\0"u8)
            }, null, true, new FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>
            {
                Value = new IPAddress("\0\0\0\0"u8)
            })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , AnyLog)
                  , "0.0.0.0"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , Json | Compact | Pretty)
                  , "\"0.0.0.0\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<IPAddress>>
            (null, "", true, new FieldSpanFormattableAlwaysAddStringBearer<IPAddress>())
            {
                { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackValue | DefaultBecomesFallbackString
                       , AnyLog ) , "FieldSpanFormattableAlwaysAddStringBearer<IPAddress>()" }
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackValue | DefaultTreatedAsStringOut | DefaultBecomesFallbackString
                       , AnyLog ) , "\"FieldSpanFormattableAlwaysAddStringBearer<IPAddress>()\"" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue) 
                  , "FieldSpanFormattableAlwaysAddStringBearer\\u003cIPAddress\\u003e()" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue) 
                  , "\"FieldSpanFormattableAlwaysAddStringBearer\\u003cIPAddress\\u003e()\"" }
                // uses Default value which is re-encoded escaping \u003c to \\u003c etc...
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackString) 
                  , "FieldSpanFormattableAlwaysAddStringBearer\\\\u003cIPAddress\\\\u003e()" }
              , { new EK(IsContentType | CallsViaMatch) , "null" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultBecomesEmpty) , "" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultBecomesZero) , "0" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString, AnyLog ), "FieldSpanFormattableAlwaysAddStringBearer<IPAddress>()" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue  
                       | DefaultBecomesFallbackString, AnyLog ) , "\"FieldSpanFormattableAlwaysAddStringBearer<IPAddress>()\"" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString), "FieldSpanFormattableAlwaysAddStringBearer\\u003cIPAddress\\u003e()" }
              , { new EK(AcceptsAnyExceptNullableStruct | AlwaysWrites | NonDefaultWrites), "null" }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>>
            (null, "", true, new FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>())
            {
               { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue 
                      | DefaultBecomesFallbackString, AnyLog ) 
                 , "FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>()" }
              , { new EK(IsContentType | CallsViaMatch  | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString , AnyLog ) 
                  , "\"FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>()\"" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue) 
                  , "FieldSpanFormattableAlwaysAddStructStringBearer\\u003cIPAddress\\u003e()" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue) 
                  , "\"FieldSpanFormattableAlwaysAddStructStringBearer\\u003cIPAddress\\u003e()\"" }
                // uses Default value which is re-encoded escaping \u003c to \\u003c etc...
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackString) 
                  , "FieldSpanFormattableAlwaysAddStructStringBearer\\\\u003cIPAddress\\\\u003e()" }
              , { new EK(IsContentType | CallsViaMatch) , "null" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesEmpty, AnyLog) , "\"\"" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesZero, AnyLog) , "\"0\"" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultBecomesEmpty) , "" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultBecomesZero) , "0" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                       | DefaultBecomesFallbackString, AnyLog )
                  , "FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>()" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString, AnyLog ) , "\"FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>()\"" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString) 
                  , "FieldSpanFormattableAlwaysAddStructStringBearer\\u003cIPAddress\\u003e()" }
              , { new EK(AcceptsAnyExceptNullableStruct | AlwaysWrites | NonDefaultWrites), "null" }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>>
            (null, null, true, new FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>())
            {
                { new EK(IsContentType | CallsViaMatch  | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                       | DefaultBecomesFallbackString, AnyLog ) 
                  , "FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>()" }
              , { new EK(IsContentType | CallsViaMatch  | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString , AnyLog ) 
                  , "\"FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>()\"" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue) 
                  , "FieldSpanFormattableAlwaysAddStructStringBearer\\u003cIPAddress\\u003e()" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue) 
                  , "\"FieldSpanFormattableAlwaysAddStructStringBearer\\u003cIPAddress\\u003e()\"" }
                // uses Default value which is re-encoded escaping \u003c to \\u003c etc...
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackString) 
                  , "FieldSpanFormattableAlwaysAddStructStringBearer\\\\u003cIPAddress\\\\u003e()" }
              , { new EK(IsContentType | CallsViaMatch) , "null" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesEmpty, AnyLog) , "\"\"" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesZero, AnyLog) , "\"0\"" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultBecomesEmpty) , "" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultBecomesZero) , "0" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                       | DefaultBecomesFallbackString, AnyLog )
                  , "FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>()" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString, AnyLog ) , "\"FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>()\"" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString) 
                  , "FieldSpanFormattableAlwaysAddStructStringBearer\\u003cIPAddress\\u003e()" }
              , { new EK(AcceptsAnyNullableStruct | AlwaysWrites | NonDefaultWrites), "null" }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<IPAddress>>
            (null, "", true
           , new FieldSpanFormattableAlwaysAddStringBearer<IPAddress>
             {
                 Value = IPAddress.Loopback
             })
            {
                { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackValue | DefaultBecomesFallbackString
                       , AnyLog ) , "FieldSpanFormattableAlwaysAddStringBearer<IPAddress>(127.0.0.1)" }
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackValue | DefaultTreatedAsStringOut | DefaultBecomesFallbackString
                       , AnyLog ) , "\"FieldSpanFormattableAlwaysAddStringBearer<IPAddress>(127.0.0.1)\"" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue) 
                  , "FieldSpanFormattableAlwaysAddStringBearer\\u003cIPAddress\\u003e(127.0.0.1)" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue) 
                  , "FieldSpanFormattableAlwaysAddStringBearer\\u003cIPAddress\\u003e(127.0.0.1)" }
                // uses Default value which is re-encoded escaping \u003c to \\u003c etc...
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackString) 
                  , "FieldSpanFormattableAlwaysAddStringBearer\\\\u003cIPAddress\\\\u003e(127.0.0.1)" }
              , { new EK(IsContentType | CallsViaMatch) , "null" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultBecomesEmpty) , "" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultBecomesZero) , "0" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString
                       , AnyLog )
                  , "FieldSpanFormattableAlwaysAddStringBearer<IPAddress>(127.0.0.1)" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue  | DefaultBecomesFallbackString
                       , AnyLog ) , "\"FieldSpanFormattableAlwaysAddStringBearer<IPAddress>(127.0.0.1)\"" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString) 
                  , "FieldSpanFormattableAlwaysAddStringBearer\\u003cIPAddress\\u003e(127.0.0.1)" }
              //   // uses Default value which is re-encoded escaping \u003c to \\u003c etc...
              // , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultBecomesFallbackString) 
              //     , "FieldSpanFormattableAlwaysAddStringBearer\\\\u003cIPAddress\\\\u003e(127.0.0.1)" }
              , { new EK(AcceptsAnyExceptNullableStruct | AlwaysWrites | NonDefaultWrites), "null" }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>>
            (default, "", true
           , new FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>
             {
                 Value = IPAddress.Loopback
             })
            {
              //   { new EK(ContentType | CallsViaMatch | DefaultBecomesNull) , "null" }
              // , { new EK(ContentType | CallsViaMatch | DefaultBecomesFallbackValue | DefaultBecomesFallbackString) , "null" }
                { new EK(IsContentType | CallsViaMatch) , "null" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultTreatedAsValueOut | DefaultBecomesEmpty) , "" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultTreatedAsValueOut | DefaultBecomesZero) , "0" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesEmpty) 
                  , "null" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesZero) , "\"0\"" }
              , { new EK(AcceptsAnyExceptNullableStruct | AlwaysWrites | NonDefaultWrites | NonNullWrites 
                       | NonNullAndPopulatedWrites), "null" }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>>
            (null, "", true
           , new FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>
             {
                 Value = IPAddress.Loopback
             })
            {
                { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut |  DefaultBecomesFallbackValue
                       | DefaultBecomesFallbackString, AnyLog ) 
                  , "FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>(127.0.0.1)" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue 
                        | DefaultBecomesFallbackString, AnyLog ) 
                  , "\"FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>(127.0.0.1)\"" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue) 
                  , "FieldSpanFormattableAlwaysAddStructStringBearer\\u003cIPAddress\\u003e(127.0.0.1)" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue) 
                  , "\"FieldSpanFormattableAlwaysAddStructStringBearer\\u003cIPAddress\\u003e(127.0.0.1)\"" }
                // uses Default value which is re-encoded escaping \u003c to \\u003c etc...
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackString) 
                  , "FieldSpanFormattableAlwaysAddStructStringBearer\\\\u003cIPAddress\\\\u003e(127.0.0.1)" }
              , { new EK(IsContentType | CallsViaMatch) , "null" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesEmpty, AnyLog) , "\"\"" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesZero, AnyLog) , "\"0\"" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultBecomesEmpty) , "" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultBecomesZero) , "0" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                       | DefaultBecomesFallbackString, AnyLog )
                  , "FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>(127.0.0.1)" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString, AnyLog ) , "\"FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>(127.0.0.1)\"" }
              , { new EK(IsContentType | AcceptsAnyNullableStruct | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString) 
                  , "FieldSpanFormattableAlwaysAddStructStringBearer\\u003cIPAddress\\u003e(127.0.0.1)" }
              , { new EK(AcceptsAnyNullableStruct | AlwaysWrites | NonDefaultWrites), "null" }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<IPAddress>>
            (new FieldSpanFormattableAlwaysAddStringBearer<IPAddress>
            {
                Value = IPAddress.Loopback
            })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "127.0.0.1"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"127.0.0.1\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>
            {
                Value = IPAddress.Loopback
            })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "127.0.0.1"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"127.0.0.1\""
                }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>
            {
                Value = IPAddress.Loopback
            })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "127.0.0.1"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"127.0.0.1\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<IPAddress>>
            (new FieldSpanFormattableAlwaysAddStringBearer<IPAddress>
            {
                Value = new IPAddress([192, 168, 0, 1])
            }, "'{0}'", true, new FieldSpanFormattableAlwaysAddStringBearer<IPAddress>
            {
                Value = new IPAddress([192, 168, 0, 1])
            })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , AnyLog)
                  , "'192.168.0.1'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , Json | Compact | Pretty)
                  , "\"'192.168.0.1'\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>
            {
                Value = new IPAddress([192, 168, 0, 1])
            }, "'{0}'", true, new FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>
            {
                Value = new IPAddress([192, 168, 0, 1])
            })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , AnyLog)
                  , "'192.168.0.1'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , Json | Compact | Pretty)
                  , "\"'192.168.0.1'\""
                }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>
            {
                Value = new IPAddress([192, 168, 0, 1])
            }, "'{0}'", true, new FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>
            {
                Value = new IPAddress([192, 168, 0, 1])
            })
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , AnyLog)
                  , "'192.168.0.1'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , Json | Compact | Pretty)
                  , "\"'192.168.0.1'\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<IPAddress>>
            (new FieldSpanFormattableAlwaysAddStringBearer<IPAddress>
            {
                Value = IPAddress.Parse("255.255.255.254")
            }, "'{0}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "'255.255.255.254'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'255.255.255.254'\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>
            {
                Value = IPAddress.Parse("255.255.255.254")
            }, "'{0}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "'255.255.255.254'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'255.255.255.254'\""
                }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>
            {
                Value = IPAddress.Parse("255.255.255.254")
            }, "'{0}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , AnyLog)
                  , "'255.255.255.254'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'255.255.255.254'\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<IPAddress>>
            (new FieldSpanFormattableAlwaysAddStringBearer<IPAddress>
            {
                Value = IPAddress.Parse("255.255.0.0")
            }, "\"{0,17}\"")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"      255.255.0.0\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>
            {
                Value = IPAddress.Parse("255.255.0.0")
            }, "\"{0,17}\"")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"      255.255.0.0\""
                }
            }
      , new NullableStringBearerExpect<FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>>
            (new FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>
            {
                Value = IPAddress.Parse("255.255.0.0")
            }, "\"{0,17}\"")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"      255.255.0.0\""
                }
            }

      , new StringBearerExpect<FieldStringAlwaysAddStringBearer>
            (new FieldStringAlwaysAddStringBearer
             {
                 Value = "It began with the forging of the Great Strings."
             }
           , "[{0}]")
            {
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"[It began with the forging of the Great Strings.]\""
                }
            }
      , new StringBearerExpect<FieldStringAlwaysAddStructStringBearer>
            (new FieldStringAlwaysAddStructStringBearer
             {
                 Value = "It began with the forging of the Great Strings."
             }
           , "[{0}]")
            {
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"[It began with the forging of the Great Strings.]\""
                }
            }
      , new NullableStringBearerExpect<FieldStringAlwaysAddStructStringBearer>
            (new FieldStringAlwaysAddStructStringBearer
             {
                 Value = "It began with the forging of the Great Strings."
             }
           , "[{0}]")
            {
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"[It began with the forging of the Great Strings.]\""
                }
            }

      , new StringBearerExpect<FieldStringAlwaysAddStringBearer>
            (null, "{0}", true, new FieldStringAlwaysAddStringBearer
            {
                Value = "non null expect non null bearer not empty"
            })
            {
                { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString, AnyLog) , "FieldStringAlwaysAddStringBearer(non null expect non null bearer not empty)" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString, AnyLog) , "\"FieldStringAlwaysAddStringBearer(non null expect non null bearer not empty)\"" }
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString) , "FieldStringAlwaysAddStringBearer(non null expect non null bearer not empty)" }
              , { new EK(IsContentType | CallsViaMatch), "null" }
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString, AnyLog), "FieldStringAlwaysAddStringBearer(non null expect non null bearer not empty)" }
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString, AnyLog), "\"FieldStringAlwaysAddStringBearer(non null expect non null bearer not empty)\"" }
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString), "FieldStringAlwaysAddStringBearer(non null expect non null bearer not empty)" }
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesEmpty, AnyLog), "" }
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesEmpty, AnyLog), "\"\"" }
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultBecomesEmpty), "" }
              , { new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonDefaultWrites), "null" }
            }
      , new StringBearerExpect<FieldStringAlwaysAddStructStringBearer>
            (default, "", true, new FieldStringAlwaysAddStructStringBearer
            {
                Value = "non null expect null bearer not empty"
            })
            {
                { new EK(IsContentType | CallsViaMatch), "null" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct), "null" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultTreatedAsValueOut | DefaultBecomesEmpty, AnyLog), "" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultTreatedAsStringOut | DefaultBecomesEmpty, AnyLog), "\"\"" }
              , { new EK(IsContentType | AcceptsAnyExceptNullableStruct | DefaultBecomesEmpty), "" }
              , {
                    new EK(AcceptsAnyExceptNullableStruct | CallsAsReadOnlySpan | AlwaysWrites | NonDefaultWrites 
                         | NonNullWrites | NonNullAndPopulatedWrites), "null"
                }
            }
      , new NullableStringBearerExpect<FieldStringAlwaysAddStructStringBearer>
            (null, "{0}", true, new FieldStringAlwaysAddStructStringBearer
            {
                Value = "null expect null bearer not empty"
            })
            {
                { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                       | DefaultBecomesFallbackString, AnyLog)
                  , "FieldStringAlwaysAddStructStringBearer(null expect null bearer not empty)" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString, AnyLog )
                  , "\"FieldStringAlwaysAddStructStringBearer(null expect null bearer not empty)\"" }
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackValue | DefaultBecomesFallbackString)
                  , "FieldStringAlwaysAddStructStringBearer(null expect null bearer not empty)" }
              , { new EK(IsContentType | CallsViaMatch) , "null" }
              , { new EK(IsContentType | AcceptsNullableStructStringBearer | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                | DefaultBecomesFallbackString, AnyLog)
                  , "FieldStringAlwaysAddStructStringBearer(null expect null bearer not empty)" }
              , { new EK(IsContentType | AcceptsNullableStructStringBearer | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString, AnyLog )
                  , "\"FieldStringAlwaysAddStructStringBearer(null expect null bearer not empty)\"" }
              , { new EK(IsContentType | AcceptsNullableStructStringBearer | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString)
                  , "FieldStringAlwaysAddStructStringBearer(null expect null bearer not empty)" }
              , { new EK(IsContentType | AcceptsNullableStructStringBearer | DefaultTreatedAsStringOut | DefaultBecomesEmpty, AnyLog) , "\"\"" }
              , { new EK(IsContentType | AcceptsNullableStructStringBearer | DefaultTreatedAsStringOut | DefaultBecomesZero, AnyLog) , "\"0\"" }
              , { new EK(IsContentType | AcceptsNullableStructStringBearer | DefaultBecomesEmpty) , "" }
              , { new EK(IsContentType | AcceptsNullableStructStringBearer | DefaultBecomesZero) , "0" }
              , {
                    new EK(AcceptsNullableStructStringBearer | CallsAsReadOnlySpan | AlwaysWrites | NonDefaultWrites), "null"
                }
            }
    };
}
