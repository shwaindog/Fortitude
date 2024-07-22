// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Converters;

public class PQtoPQPriceConverterRepository : ConverterRepository
{
    public PQtoPQPriceConverterRepository()
    {
        AddConverter(new PQTickInstantToPQLevel1QuoteConverter());
        AddConverter(new PQTickInstantToPQLevel2QuoteConverter());
        AddConverter(new PQTickInstantToPQLevel3QuoteConverter());
        AddConverter(new PQLevel1ToPQLevel2Converter());
        AddConverter(new PQLevel1ToPQLevel3Converter());
        AddConverter(new PQLevel2ToPQLevel3Converter());
        AddConverter(new PQLevel1QuoteToPQTickInstantConverter());
        AddConverter(new PQLevel2QuoteToPQTickInstantConverter());
        AddConverter(new PQLevel3QuoteToPQTickInstantConverter());
        AddConverter(new PQLevel2ToPQLevel1Converter());
        AddConverter(new PQLevel3ToPQLevel1Converter());
        AddConverter(new PQLevel3ToPQLevel2Converter());
    }
}
