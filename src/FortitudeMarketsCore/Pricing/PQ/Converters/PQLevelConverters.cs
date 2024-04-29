#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Converters;

public class PQLevel0ToPQLevel1Converter : ConverterBase<PQLevel0Quote, PQLevel1Quote>
{
    public override PQLevel1Quote Convert(PQLevel0Quote original, IRecycler? recycler = null) => (PQLevel1Quote)original;
}

public class PQLevel0ToPQLevel2Converter : ConverterBase<PQLevel0Quote, PQLevel2Quote>
{
    public override PQLevel2Quote Convert(PQLevel0Quote original, IRecycler? recycler = null) => (PQLevel2Quote)original;
}

public class PQLevel0ToPQLevel3Converter : ConverterBase<PQLevel0Quote, PQLevel3Quote>
{
    public override PQLevel3Quote Convert(PQLevel0Quote original, IRecycler? recycler = null) => (PQLevel3Quote)original;
}

public class PQLevel1ToPQLevel2Converter : ConverterBase<PQLevel1Quote, PQLevel2Quote>
{
    public override PQLevel2Quote Convert(PQLevel1Quote original, IRecycler? recycler = null) => (PQLevel2Quote)original;
}

public class PQLevel1ToPQLevel3Converter : ConverterBase<PQLevel1Quote, PQLevel3Quote>
{
    public override PQLevel3Quote Convert(PQLevel1Quote original, IRecycler? recycler = null) => (PQLevel3Quote)original;
}

public class PQLevel2ToPQLevel3Converter : ConverterBase<PQLevel2Quote, PQLevel3Quote>
{
    public override PQLevel3Quote Convert(PQLevel2Quote original, IRecycler? recycler = null) => (PQLevel3Quote)original;
}

public class PQLevel1ToPQLevel0Converter : ConverterBase<PQLevel1Quote, PQLevel0Quote>
{
    public override PQLevel0Quote Convert(PQLevel1Quote original, IRecycler? recycler = null) => original;
}

public class PQLevel2ToPQLevel0Converter : ConverterBase<PQLevel2Quote, PQLevel0Quote>
{
    public override PQLevel0Quote Convert(PQLevel2Quote original, IRecycler? recycler = null) => original;
}

public class PQLevel3ToPQLevel0Converter : ConverterBase<PQLevel3Quote, PQLevel0Quote>
{
    public override PQLevel0Quote Convert(PQLevel3Quote original, IRecycler? recycler = null) => original;
}

public class PQLevel2ToPQLevel1Converter : ConverterBase<PQLevel2Quote, PQLevel1Quote>
{
    public override PQLevel1Quote Convert(PQLevel2Quote original, IRecycler? recycler = null) => original;
}

public class PQLevel3ToPQLevel1Converter : ConverterBase<PQLevel3Quote, PQLevel1Quote>
{
    public override PQLevel1Quote Convert(PQLevel3Quote original, IRecycler? recycler = null) => original;
}

public class PQLevel3ToPQLevel2Converter : ConverterBase<PQLevel3Quote, PQLevel2Quote>
{
    public override PQLevel2Quote Convert(PQLevel3Quote original, IRecycler? recycler = null) => original;
}
