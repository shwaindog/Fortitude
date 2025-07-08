// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Config.Appending;

public enum FloggerAppenderType
{
    NotGiven
  , Forwarding // Includes  Filtering, Buffering and Async Dispatching
  , File
  , Console
  , Network
  , Database
  , Custom
}
