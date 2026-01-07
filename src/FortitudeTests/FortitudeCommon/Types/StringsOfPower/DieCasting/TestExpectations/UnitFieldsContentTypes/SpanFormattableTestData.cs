// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.LoggerViews;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.UnitFieldsContentTypes;

public static class SpanFormattableTestData
{
    private static IVersatileFLogger logger = null!;

    public static readonly Lazy<List<ISingleFieldExpectation>> AllSpanFormattableExpectations =
        new(() =>
        {

            logger = FLog.FLoggerForType.As<IVersatileFLogger>();

            return 
            //     new List<ISingleFieldExpectation>()
            // {
            //     NumberTestData.AllNumberExpectations.Value.First()
            // };
            [
                ..NumberTestData.AllNumberExpectations.Value
              , ..SpanFormattableStructTestData.SpanFormattableStructExpectations
              , ..SpanFormattableClassTestData.SpanFormattableClassExpectations
              , ..EnumTestData.EnumExpectations
            ];
        });
}
