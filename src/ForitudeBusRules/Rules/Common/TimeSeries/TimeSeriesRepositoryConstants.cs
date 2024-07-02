// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeBusRules.Rules.Common.TimeSeries;

public static class TimeSeriesRepositoryConstants
{
    public const string TimeSeriesRepositoryBase = "Repository.TimeSeries";

    public const string TimeSeriesAllAvailableInstrumentsPublishRequest  = $"{TimeSeriesRepositoryBase}.Instrument.List.PublishRequest";
    public const string TimeSeriesAllAvailableInstrumentsRequestResponse = $"{TimeSeriesRepositoryBase}.Instrument.List.RequestResponse";
    public const string TimeSeriesInstrumentFileInfoRequestResponse      = $"{TimeSeriesRepositoryBase}.Instrument.FileInfo.RequestReponse";
    public const string TimeSeriesInstrumentEntryFileInfoRequestResponse = $"{TimeSeriesRepositoryBase}.Instrument.FileEntryInfo.RequestResponse";

    public const string TimeSeriesAllAvailableInstrumentsDefaultResponse = $"{TimeSeriesRepositoryBase}.Instrument.List.All";
}
