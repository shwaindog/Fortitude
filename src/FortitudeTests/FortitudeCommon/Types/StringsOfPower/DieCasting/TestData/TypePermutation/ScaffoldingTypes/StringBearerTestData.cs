// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Net;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.SingleFields;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

public static class StringBearerTestData
{
    public static readonly IFormatExpectation[] AllStringBearerExpectations =
    [
        // byte
        new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<byte>>(new FieldSpanFormattableAlwaysAddStringBearer<byte>
        {
            Value = 0
        }, "", true, new FieldSpanFormattableAlwaysAddStringBearer<byte>
        {
            Value = 0
        })
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" }
        }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<byte>>(new FieldSpanFormattableAlwaysAddStringBearer<byte>
        {
            Value = 255
        })
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "255" }
        }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<byte>>(new FieldSpanFormattableAlwaysAddStringBearer<byte>
            {
                Value = 128
            }, "C2")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "$128.00" } }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<byte>>(new FieldSpanFormattableAlwaysAddStringBearer<byte>
        {
            Value = 77
        }, "\"{0,-20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"77                  \"" }
        }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<byte>>(new FieldSpanFormattableAlwaysAddStringBearer<byte>
        {
            Value = 32
        }, "", true, new FieldSpanFormattableAlwaysAddStringBearer<byte>
        {
            Value = 32
        })
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "32" }
        }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<byte>>(new FieldSpanFormattableAlwaysAddStringBearer<byte>
            {
                Value = 255
            }, "{0[..1]}")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2" } }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<byte>>(new FieldSpanFormattableAlwaysAddStringBearer<byte>
            {
                Value = 255
            }, "{0[1..2]}")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "5" } }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<byte>>(new FieldSpanFormattableAlwaysAddStringBearer<byte>
            {
                Value = 255
            }, "{0[1..]}")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "55" } }
      
        // byte?
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<byte>>(new FieldNullableSpanFormattableAlwaysAddStringBearer<byte>
        {
            Value = 0
        }, "", true, new FieldNullableSpanFormattableAlwaysAddStringBearer<byte>
        {
            Value = 0
        })
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" }
        }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<byte>>(
                                                                                        new FieldNullableSpanFormattableAlwaysAddStringBearer<byte>
        {
            Value = 0
        }, "{0}", true, new FieldNullableSpanFormattableAlwaysAddStringBearer<byte>
        {
            Value = 0
        }) { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<byte>>
            (null, "null", true, new FieldNullableSpanFormattableAlwaysAddStringBearer<byte>
        {
            Value = 0
        })
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" }
        }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<byte>>
            (null, "null")
        {
            { AcceptsSpanFormattable | AlwaysWrites, "null" }
        }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<byte>>(new FieldNullableSpanFormattableAlwaysAddStringBearer<byte>
            {
                Value = 255
            })
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "255" } }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<byte>>(new FieldNullableSpanFormattableAlwaysAddStringBearer<byte>
        {
            Value = 128
        }, "C2")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "$128.00" }
        }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<byte>>(new FieldNullableSpanFormattableAlwaysAddStringBearer<byte>
        {
            Value = 144
        }, "\"{0,20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"                 144\"" }
        }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<byte>>(new FieldNullableSpanFormattableAlwaysAddStringBearer<byte>
        {
            Value = 64
        }, "", true, new FieldNullableSpanFormattableAlwaysAddStringBearer<byte>
        {
            Value = 64
        })
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "64" }
        }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<byte>>(new FieldNullableSpanFormattableAlwaysAddStringBearer<byte>
        {
            Value = 255
        }, "{0[..1]}")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2" }
        }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<byte>>(new FieldNullableSpanFormattableAlwaysAddStringBearer<byte>
        {
            Value = 255
        }, "{0[1..2]}")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "5" }
        }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<byte>>(new FieldNullableSpanFormattableAlwaysAddStringBearer<byte>
            {
                Value = 255
            }, "{0[1..]}")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "55" } }
      
        // DateTime
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<DateTime>>(new FieldSpanFormattableAlwaysAddStringBearer<DateTime>
            {
                Value = DateTime.MinValue
            }, "O", true, new FieldSpanFormattableAlwaysAddStringBearer<DateTime>
            {
                Value = DateTime.MinValue
            })
            { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0001-01-01T00:00:00.0000000" } }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<DateTime>>(new FieldSpanFormattableAlwaysAddStringBearer<DateTime>
            {
                Value = new DateTime(2000, 1, 1, 1, 1, 1).AddTicks(1111111)
            }, "o")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2000-01-01T01:01:01.1111111" } }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<DateTime>>
            (new FieldSpanFormattableAlwaysAddStringBearer<DateTime>
             {
                 Value = new DateTime(2020, 2, 2).AddTicks(2222222)
             }, "s", true
           , new FieldSpanFormattableAlwaysAddStringBearer<DateTime>
             {
                 Value = new DateTime(2020, 2, 2).AddTicks(2222222)
             })
            { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "2020-02-02T00:00:00" } }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<DateTime>>
            (new FieldSpanFormattableAlwaysAddStringBearer<DateTime>
            {
                Value = DateTime.MaxValue
            }, "'{0:u}'")
            {
                {
                    AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "'9999-12-31 23:59:59Z'"
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
                    AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
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
                    AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "'1980-07-31 11:48:13'"
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<DateTime>>
            (new FieldSpanFormattableAlwaysAddStringBearer<DateTime>
            {
                Value = new DateTime(2009, 11, 12, 19, 49, 0)
            }, "\"{0,-30:O}\"")
            {
                {
                    AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
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
            { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0001-01-01T00:00:00.0000000" } }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>>
            ( null, "null", true, new FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>
            {
                Value = new DateTime(2020, 2, 2).AddTicks(2222222)
            })
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" } }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>>
            (null, "null")
            { { AcceptsSpanFormattable | AlwaysWrites, "null" } }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>
             {
                 Value = new DateTime(2000, 1, 1, 1, 1, 1).AddTicks(1111111)
             }
           , "o")
            {
                {
                    AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "2000-01-01T01:01:01.1111111"
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
            { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "2020-02-02T00:00:00" } }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>
             {
                 Value = DateTime.MaxValue
             }
           , "'{0:u}'")
            {
                {
                    AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "'9999-12-31 23:59:59Z'"
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>
             {
                 Value = DateTime.MinValue
             }
           , "\"{0,30:u}\"", true, new FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>
             {
                 Value = new DateTime(2020, 1, 1)
             })
            {
                {
                    AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
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
                    AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "'1980-07-31 11:48:13'"
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>
            {
                Value = new DateTime(2009, 11, 12, 19, 49, 0)
            }, "\"{0,-30:O}\"")
            {
                {
                    AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
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
                { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0:00:00" }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (new FieldSpanFormattableAlwaysAddStringBearer<TimeSpan>
            {
                Value = new TimeSpan(1, 1, 1, 1, 111, 111)
            }, "c")
            {
                {
                    AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "1.01:01:01.1111110"
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
            { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "-2:22:22:22.2222220" } }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (new FieldSpanFormattableAlwaysAddStringBearer<TimeSpan>
            {
                Value = TimeSpan.MaxValue
            }, "'{0:G}'")
            {
                {
                    AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "'10675199:02:48:05.4775807'"
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
                    AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "\"    -10675199.02:48:05.4775808\""
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<TimeSpan>>(new FieldSpanFormattableAlwaysAddStringBearer<TimeSpan>
        {
            Value = new TimeSpan(3, 3, 33, 33, 333, 333)
        }, "'{0:dd\\-hh\\-mm\\-ss\\.fff}'")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'03-03-33-33.333'"
            }
        }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (new FieldSpanFormattableAlwaysAddStringBearer<TimeSpan>
            {
                Value = new TimeSpan(-4, -4, -44, -44, -444, -444)
            }, "\"{0,-30:G}\"")
            {
                {
                    AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
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
                { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0:00:00" }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (null, "null", true, new FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>
            {
                Value = TimeSpan.FromSeconds(1)
            })
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" } }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (null, "null")
            { { AcceptsSpanFormattable | AlwaysWrites, "null" } }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>
            {
                Value = new TimeSpan(1, 1, 1, 1, 111, 111)
            }, "c")
            {
                {
                    AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "1.01:01:01.1111110"
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
            { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "-2:22:22:22.2222220" } }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>
            {
                Value = TimeSpan.MaxValue
            }, "'{0:G}'")
            {
                {
                    AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "'10675199:02:48:05.4775807'"
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>
             {
                 Value = TimeSpan.MinValue
             }
           , "\"{0,30:c}\"", true, new FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>
             {
                 Value = TimeSpan.Zero
             })
            {
                {
                    AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "\"    -10675199.02:48:05.4775808\""
                }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>
            {
                Value = new TimeSpan(3, 3, 33, 33, 333, 333)
            }, "'{0:dd\\-hh\\-mm\\-ss\\.fff}'")
            {
                { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'03-03-33-33.333'" }
            }
      , new StringBearerExpect<FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (new FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>
            {
                Value = new TimeSpan(-4, -4, -44, -44, -444, -444)
            }, "\"{0,-30:G}\"")
            {
                {
                    AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
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
                    AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                  , "0.0.0.0"
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<IPAddress>>
            (null, "null", true, new FieldSpanFormattableAlwaysAddStringBearer<IPAddress>
            {
                Value = IPAddress.Loopback
            })
            {
                { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<IPAddress>>
            (null, "null")
            {
                { AcceptsSpanFormattable | AlwaysWrites, "null" }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<IPAddress>>
            (new FieldSpanFormattableAlwaysAddStringBearer<IPAddress>
            {
                Value = IPAddress.Loopback
            })
            {
                {
                    AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "127.0.0.1"
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
                { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "'192.168.0.1'" }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<IPAddress>>
            (new FieldSpanFormattableAlwaysAddStringBearer<IPAddress>
            {
                Value = IPAddress.Parse("255.255.255.254")
            }, "'{0}'")
            {
                {
                    AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "'255.255.255.254'"
                }
            }
      , new StringBearerExpect<FieldSpanFormattableAlwaysAddStringBearer<IPAddress>>
            (new FieldSpanFormattableAlwaysAddStringBearer<IPAddress>
            {
                Value = IPAddress.Parse("255.255.0.0")
            }, "\"{0,17}\"")
            {
                {
                    AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "\"      255.255.0.0\""
                }
            }
      
      , new StringBearerExpect<FieldStringAlwaysAddStringBearer>
            (new FieldStringAlwaysAddStringBearer()
             {
                 Value = "It began with the forging of the Great Strings."
             }
           , "[{0}]")
            {
                {
                    AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "[It began with the forging of the Great Strings.]"
                }
            }
      
      , new StringBearerExpect<FieldStringAlwaysAddStringBearer>(null, "{0}", true, new FieldStringAlwaysAddStringBearer()
        {
            Value = "not empty"
        })
        {
            {
                AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites
              , "null"
            }
        }
    ];
}
