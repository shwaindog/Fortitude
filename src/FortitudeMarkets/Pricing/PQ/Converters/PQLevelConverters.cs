// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Converters;

public class PQTickInstantToPQLevel1QuoteConverter : ConverterBase<PQTickInstant, PQLevel1Quote>
{
    public override PQLevel1Quote Convert(PQTickInstant original, IRecycler? recycler = null) => (PQLevel1Quote)original;
}

public class PQTickInstantToPQLevel2QuoteConverter : ConverterBase<PQTickInstant, PQLevel2Quote>
{
    public override PQLevel2Quote Convert(PQTickInstant original, IRecycler? recycler = null) => (PQLevel2Quote)original;
}

public class PQTickInstantToPQLevel3QuoteConverter : ConverterBase<PQTickInstant, PQLevel3Quote>
{
    public override PQLevel3Quote Convert(PQTickInstant original, IRecycler? recycler = null) => (PQLevel3Quote)original;
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

public class PQLevel1QuoteToPQTickInstantConverter : ConverterBase<PQLevel1Quote, PQTickInstant>
{
    public override PQTickInstant Convert(PQLevel1Quote original, IRecycler? recycler = null) => original;
}

public class PQLevel2QuoteToPQTickInstantConverter : ConverterBase<PQLevel2Quote, PQTickInstant>
{
    public override PQTickInstant Convert(PQLevel2Quote original, IRecycler? recycler = null) => original;
}

public class PQLevel3QuoteToPQTickInstantConverter : ConverterBase<PQLevel3Quote, PQTickInstant>
{
    public override PQTickInstant Convert(PQLevel3Quote original, IRecycler? recycler = null) => original;
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
