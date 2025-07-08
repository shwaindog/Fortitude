// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Config.Appending.Forwarding;

public interface IAppenderForwardingReferenceConfig : IAppenderReferenceConfig
{
    uint AppendProcessingOrder { get; }  // If Synchronised will send from lowest to highest order
}

public interface IMutableAppenderForwardingReferenceConfig : IAppenderForwardingReferenceConfig
{
    new uint AppendProcessingOrder { get; set; } 
}
