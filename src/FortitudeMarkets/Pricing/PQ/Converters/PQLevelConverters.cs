// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Converters;

public class PQTickInstantToPQLevel1QuoteConverter : ConverterBase<PQPublishableTickInstant, PQPublishableLevel1Quote>
{
    public override PQPublishableLevel1Quote Convert(PQPublishableTickInstant original, IRecycler? recycler = null) => (PQPublishableLevel1Quote)original;
}

public class PQTickInstantToPQLevel2QuoteConverter : ConverterBase<PQPublishableTickInstant, PQPublishableLevel2Quote>
{
    public override PQPublishableLevel2Quote Convert(PQPublishableTickInstant original, IRecycler? recycler = null) => (PQPublishableLevel2Quote)original;
}

public class PQTickInstantToPQLevel3QuoteConverter : ConverterBase<PQPublishableTickInstant, PQPublishableLevel3Quote>
{
    public override PQPublishableLevel3Quote Convert(PQPublishableTickInstant original, IRecycler? recycler = null) => (PQPublishableLevel3Quote)original;
}

public class PQLevel1ToPQLevel2Converter : ConverterBase<PQPublishableLevel1Quote, PQPublishableLevel2Quote>
{
    public override PQPublishableLevel2Quote Convert(PQPublishableLevel1Quote original, IRecycler? recycler = null) => (PQPublishableLevel2Quote)original;
}

public class PQLevel1ToPQLevel3Converter : ConverterBase<PQPublishableLevel1Quote, PQPublishableLevel3Quote>
{
    public override PQPublishableLevel3Quote Convert(PQPublishableLevel1Quote original, IRecycler? recycler = null) => (PQPublishableLevel3Quote)original;
}

public class PQLevel2ToPQLevel3Converter : ConverterBase<PQPublishableLevel2Quote, PQPublishableLevel3Quote>
{
    public override PQPublishableLevel3Quote Convert(PQPublishableLevel2Quote original, IRecycler? recycler = null) => (PQPublishableLevel3Quote)original;
}

public class PQLevel1QuoteToPQTickInstantConverter : ConverterBase<PQPublishableLevel1Quote, PQPublishableTickInstant>
{
    public override PQPublishableTickInstant Convert(PQPublishableLevel1Quote original, IRecycler? recycler = null) => original;
}

public class PQLevel2QuoteToPQTickInstantConverter : ConverterBase<PQPublishableLevel2Quote, PQPublishableTickInstant>
{
    public override PQPublishableTickInstant Convert(PQPublishableLevel2Quote original, IRecycler? recycler = null) => original;
}

public class PQLevel3QuoteToPQTickInstantConverter : ConverterBase<PQPublishableLevel3Quote, PQPublishableTickInstant>
{
    public override PQPublishableTickInstant Convert(PQPublishableLevel3Quote original, IRecycler? recycler = null) => original;
}

public class PQLevel2ToPQLevel1Converter : ConverterBase<PQPublishableLevel2Quote, PQPublishableLevel1Quote>
{
    public override PQPublishableLevel1Quote Convert(PQPublishableLevel2Quote original, IRecycler? recycler = null) => original;
}

public class PQLevel3ToPQLevel1Converter : ConverterBase<PQPublishableLevel3Quote, PQPublishableLevel1Quote>
{
    public override PQPublishableLevel1Quote Convert(PQPublishableLevel3Quote original, IRecycler? recycler = null) => original;
}

public class PQLevel3ToPQLevel2Converter : ConverterBase<PQPublishableLevel3Quote, PQPublishableLevel2Quote>
{
    public override PQPublishableLevel2Quote Convert(PQPublishableLevel3Quote original, IRecycler? recycler = null) => original;
}
