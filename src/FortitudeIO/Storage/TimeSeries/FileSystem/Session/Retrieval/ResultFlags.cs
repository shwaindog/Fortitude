// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeIO.Storage.TimeSeries.FileSystem.Session.Retrieval;

[Flags]
public enum ResultFlags
{
    None                   = 0x00_00
  , CountOnly              = 0x00_01
  , AsManyAsPossible       = 0x00_02
  , CaptureFirstResult     = 0x00_04
  , CaptureLastResult      = 0x00_08
  , LimitCount             = 0x00_40
  , AsEnumerable           = 0x00_20
  , CopyToList             = 0x00_10
  , PublishToBlockingQueue = 0x00_80
  , PublishOnObserver      = 0x01_00
  , RunCallback            = 0x02_00
  , SampleResults          = 0x08_00
  , WriteResultsToBuffer   = 0x10_00
}

public static class ResultFlagsExtensions
{
    public static bool HasCountOnlyFlag(this ResultFlags flags)                   => (flags & ResultFlags.CountOnly) > 0;
    public static bool HasAsManyAsPossibleFlag(this ResultFlags flags)            => (flags & ResultFlags.AsManyAsPossible) > 0;
    public static bool HasCaptureFirstResultFlag(this ResultFlags flags)          => (flags & ResultFlags.CaptureFirstResult) > 0;
    public static bool HasCaptureLastResultFlag(this ResultFlags flags)           => (flags & ResultFlags.CaptureLastResult) > 0;
    public static bool HasCopyToListFlag(this ResultFlags flags)                  => (flags & ResultFlags.CopyToList) > 0;
    public static bool HasAsEnumerableFlag(this ResultFlags flags)                => (flags & ResultFlags.AsEnumerable) > 0;
    public static bool HasLimitCountFlag(this ResultFlags flags)                  => (flags & ResultFlags.LimitCount) > 0;
    public static bool HasReusableResultBlockingQueueFlag(this ResultFlags flags) => (flags & ResultFlags.PublishToBlockingQueue) > 0;
    public static bool HasPublishOnObserverFlag(this ResultFlags flags)           => (flags & ResultFlags.PublishOnObserver) > 0;
    public static bool HasRunCallbackFlag(this ResultFlags flags)                 => (flags & ResultFlags.RunCallback) > 0;
    public static bool HasSampleResultsFlag(this ResultFlags flags)               => (flags & ResultFlags.SampleResults) > 0;
    public static bool HasWriteResultsToBufferFlag(this ResultFlags flags)        => (flags & ResultFlags.WriteResultsToBuffer) > 0;

    public static bool        HasNoneOf(this ResultFlags flags, ResultFlags ensureMissing) => (flags & ensureMissing) == 0;
    public static bool        HasAnyOf(this ResultFlags flags, ResultFlags check)          => (flags & check) > 0;
    public static ResultFlags Unset(this ResultFlags flags, ResultFlags unsetThese)        => flags & ~unsetThese;
}
