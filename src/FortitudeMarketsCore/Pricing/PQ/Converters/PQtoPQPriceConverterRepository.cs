#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Converters;

public class PQtoPQPriceConverterRepository : ConverterRepository
{
    public PQtoPQPriceConverterRepository()
    {
        AddConverter(new PQLevel0ToPQLevel1Converter());
        AddConverter(new PQLevel0ToPQLevel2Converter());
        AddConverter(new PQLevel0ToPQLevel3Converter());
        AddConverter(new PQLevel1ToPQLevel2Converter());
        AddConverter(new PQLevel1ToPQLevel3Converter());
        AddConverter(new PQLevel2ToPQLevel3Converter());
        AddConverter(new PQLevel1ToPQLevel0Converter());
        AddConverter(new PQLevel2ToPQLevel0Converter());
        AddConverter(new PQLevel3ToPQLevel0Converter());
        AddConverter(new PQLevel2ToPQLevel1Converter());
        AddConverter(new PQLevel3ToPQLevel1Converter());
        AddConverter(new PQLevel3ToPQLevel2Converter());
    }
}
