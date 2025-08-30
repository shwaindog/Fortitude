// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config.LoggersHierarchy.ActivationProfiles;

namespace FortitudeCommon.Logging.Core.ActivationProfiles;

[Flags]
public enum LoggerActivationFlags : uint
{
    Disabled                 = 0x00_00_00_00
  , MergeWithLoggerConfigProfile     = 0x00_00_00_01
  , WhenDebugBuildProfile    = 0x00_00_00_02
  , WhenReleaseBuildProfile  = 0x00_00_00_04
  , WhenAnyBuildProfile      = 0x00_00_00_06
  , WhenLocalMachineProfile  = 0x00_00_00_08
  , WhenSystemTestProfile    = 0x00_00_00_10
  , WhenSystemOrLocal        = 0x00_00_00_18
  , WhenUatTestProfile       = 0x00_00_00_20
  , WhenUatSystemOrLocal     = 0x00_00_00_38
  , WhenPerfTestProfile      = 0x00_00_00_40
  , WhenPerfUatSystemOrLocal = 0x00_00_00_78
  , WhenProdProfile          = 0x00_00_00_80
  , WhenAnyEnvironment       = 0x00_00_00_F8
  , WhenStopWatch            = 0x00_00_01_00
  , DefaultLogger            = 0x00_00_01_FE

  , PerLocationAttemptInterval  = 0x00_00_02_00
  , LoggerGlobalAttemptInterval = 0x00_00_04_00
  , PerCorrelationIdInterval    = 0x00_00_08_00
  , AddSkipCount                = 0x00_00_10_00
  , PerLocationTimeInterval     = 0x00_00_20_00
  , GlobalLoggerTimeInterval    = 0x00_00_40_00

  , NotDebugBuildProfile    = 0x00_02_00_01
  , NotReleaseBuildProfile  = 0x00_04_00_01
  , NotLocalMachineProfile  = 0x00_08_00_01
  , NotSystemTestProfile    = 0x00_10_00_01
  , NotSystemOrLocal        = 0x00_18_00_01
  , NotUatTestProfile       = 0x00_20_00_01
  , NotUatSystemOrLocal     = 0x00_38_00_01
  , NotPerfTestProfile      = 0x00_40_00_01
  , NotPerfUatSystemOrLocal = 0x00_78_00_01
  , NotProdProfile          = 0x00_80_00_01
  , NotAnyEnvironment       = 0x00_F8_00_01
  , NotStopWatch            = 0x01_00_00_01

  , IgnoreBuildTypeDeactivations      = 0x02_00_00_01
  , IgnoreDeploymentEnvDeactivations  = 0x04_00_00_01
  , IgnoreAllEnvironmentDeactivations = 0x06_00_00_01

  , LargeMessage     = 0x10_00_00_01
  , VeryLargeMessage = 0x20_00_00_01

  , PerLocationPercentilesAtInterval = 0x40_00_00_00
  , CacheBacktraceReleaseOnly        = 0x80_00_00_01
}

public static class LoggerActivationFlagsExtensions
{
    private const uint BuildTypesMask            = (uint)LoggerActivationFlags.WhenAnyBuildProfile;
    private const uint DeploymentEnvironmentMask = (uint)LoggerActivationFlags.WhenAnyEnvironment;

    public static bool ShouldActivate(this LoggerActivationFlags received, LoggerActivationFlags configActivationProfile
      , LoggerActivationFlags currentFLogEnvironment)
    {
        var currentEnvAsUint = (uint)currentFLogEnvironment;
        var shouldActive = received.ShouldActivate
            (configActivationProfile, (FLogBuildTypeProfile)(currentEnvAsUint & BuildTypesMask),
             (FlogDeploymentEnvironmentProfileType)(currentEnvAsUint & DeploymentEnvironmentMask));
        return shouldActive;
    }

    public static bool ShouldActivate(this LoggerActivationFlags received, LoggerActivationFlags configActivationProfile
      , FLogBuildTypeProfile currentFLogBuildTypeProfile, FlogDeploymentEnvironmentProfileType currentDeploymentProfile)
    {
        if (received.HasMergeInConfigProfile())
        {
            received |= configActivationProfile;
        }
        var buildProfilePass         = 
            received.BuildProfileMeetsCriteria(currentFLogBuildTypeProfile) || received.HasIgnoreBuildTypeDeactivations();
        var envDeploymentProfilePass = 
            received.DeploymentProfileMeetsCriteria(currentDeploymentProfile) || received.HasIgnoreDeploymentEnvDeactivations();

        return buildProfilePass && envDeploymentProfilePass;
    }

    public static bool DeploymentProfileMeetsCriteria(this LoggerActivationFlags loggerActivationFlags
      , FlogDeploymentEnvironmentProfileType currentBuildProfile)
    {
        var buildProfileUint    = (uint)currentBuildProfile;
        var buildProfileShifted = currentBuildProfile.AllToNoneOrOriginalAsUint() << 16;
        var activateFlagsUint   = (uint)loggerActivationFlags;
        return (buildProfileUint & activateFlagsUint) > 0 && (buildProfileShifted & activateFlagsUint) == 0;
    }

    public static bool BuildProfileMeetsCriteria(this LoggerActivationFlags loggerActivationFlags, FLogBuildTypeProfile currentFLogBuildTypeProfile)
    {
        var buildProfileUint    = (uint)currentFLogBuildTypeProfile;
        var buildProfileShifted = currentFLogBuildTypeProfile.AllToNoneOrOriginalAsUint() << 16;
        var activateFlagsUint   = (uint)loggerActivationFlags;
        return (buildProfileUint & activateFlagsUint) > 0 && (buildProfileShifted & activateFlagsUint) == 0;
    }

    public static bool HasMergeInConfigProfile(this LoggerActivationFlags flags)     => (flags & LoggerActivationFlags.MergeWithLoggerConfigProfile) > 0;
    public static bool HasWhenDebugBuildProfile(this LoggerActivationFlags flags)    => (flags & LoggerActivationFlags.WhenDebugBuildProfile) > 0;
    public static bool HasWhenReleaseBuildProfile(this LoggerActivationFlags flags)  => (flags & LoggerActivationFlags.WhenReleaseBuildProfile) > 0;
    public static bool HasWhenLocalMachineProfile(this LoggerActivationFlags flags)  => (flags & LoggerActivationFlags.WhenLocalMachineProfile) > 0;
    public static bool HasWhenSystemTestProfile(this LoggerActivationFlags flags)    => (flags & LoggerActivationFlags.WhenSystemTestProfile) > 0;
    public static bool HasWhenSystemOrLocal(this LoggerActivationFlags flags)        => (flags & LoggerActivationFlags.WhenSystemOrLocal) > 0;
    public static bool HasWhenUatTestProfile(this LoggerActivationFlags flags)       => (flags & LoggerActivationFlags.WhenUatTestProfile) > 0;
    public static bool HasWhenUatSystemOrLocal(this LoggerActivationFlags flags)     => (flags & LoggerActivationFlags.WhenUatSystemOrLocal) > 0;
    public static bool HasWhenPerfTestProfile(this LoggerActivationFlags flags)      => (flags & LoggerActivationFlags.WhenPerfTestProfile) > 0;
    public static bool HasWhenPerfUatSystemOrLocal(this LoggerActivationFlags flags) => (flags & LoggerActivationFlags.WhenPerfUatSystemOrLocal) > 0;
    public static bool HasWhenProdProfile(this LoggerActivationFlags flags)          => (flags & LoggerActivationFlags.WhenProdProfile) > 0;

    public static bool HasWhenAnyEnvironment(this LoggerActivationFlags flags) =>
        (flags & LoggerActivationFlags.WhenAnyEnvironment) == LoggerActivationFlags.WhenAnyEnvironment;

    public static bool HasWhenStopWatch(this LoggerActivationFlags flags) => (flags & LoggerActivationFlags.WhenStopWatch) > 0;

    public static bool HasDefaultLogger(this LoggerActivationFlags flags) =>
        (flags & LoggerActivationFlags.DefaultLogger) == LoggerActivationFlags.DefaultLogger;

    public static bool HasPerLocationAttemptInterval(this LoggerActivationFlags flags) =>
        (flags & LoggerActivationFlags.PerLocationAttemptInterval) > 0;

    public static bool HasLoggerGlobalAttemptInterval(this LoggerActivationFlags flags) =>
        (flags & LoggerActivationFlags.LoggerGlobalAttemptInterval) > 0;

    public static bool HasPerCorrelationIdInterval(this LoggerActivationFlags flags) => (flags & LoggerActivationFlags.PerCorrelationIdInterval) > 0;

    public static bool HasAddSkipCount(this LoggerActivationFlags flags)             => (flags & LoggerActivationFlags.AddSkipCount) > 0;
    public static bool HasPerLocationTimeInterval(this LoggerActivationFlags flags)  => (flags & LoggerActivationFlags.PerLocationTimeInterval) > 0;
    public static bool HasGlobalLoggerTimeInterval(this LoggerActivationFlags flags) => (flags & LoggerActivationFlags.GlobalLoggerTimeInterval) > 0;
    public static bool HasNotDebugBuildProfile(this LoggerActivationFlags flags)     => (flags & LoggerActivationFlags.NotDebugBuildProfile) > 0;
    public static bool HasNotReleaseBuildProfile(this LoggerActivationFlags flags)   => (flags & LoggerActivationFlags.NotReleaseBuildProfile) > 0;
    public static bool HasNotLocalMachineProfile(this LoggerActivationFlags flags)   => (flags & LoggerActivationFlags.NotLocalMachineProfile) > 0;
    public static bool HasNotSystemTestProfile(this LoggerActivationFlags flags)     => (flags & LoggerActivationFlags.NotSystemTestProfile) > 0;
    public static bool HasNotSystemOrLocal(this LoggerActivationFlags flags)         => (flags & LoggerActivationFlags.NotSystemOrLocal) > 0;
    public static bool HasNotUatTestProfile(this LoggerActivationFlags flags)        => (flags & LoggerActivationFlags.NotUatTestProfile) > 0;
    public static bool HasNotUatSystemOrLocal(this LoggerActivationFlags flags)      => (flags & LoggerActivationFlags.NotUatSystemOrLocal) > 0;
    public static bool HasNotPerfTestProfile(this LoggerActivationFlags flags)       => (flags & LoggerActivationFlags.NotPerfTestProfile) > 0;
    public static bool HasNotPerfUatSystemOrLocal(this LoggerActivationFlags flags)  => (flags & LoggerActivationFlags.NotPerfUatSystemOrLocal) > 0;
    public static bool HasNotProdProfile(this LoggerActivationFlags flags)           => (flags & LoggerActivationFlags.NotProdProfile) > 0;
    public static bool HasNotAnyEnvironment(this LoggerActivationFlags flags)        => (flags & LoggerActivationFlags.NotAnyEnvironment) > 0;
    public static bool HasNotStopWatch(this LoggerActivationFlags flags)             => (flags & LoggerActivationFlags.NotStopWatch) > 0;

    public static bool HasIgnoreBuildTypeDeactivations(this LoggerActivationFlags flags) =>
        (flags & LoggerActivationFlags.IgnoreBuildTypeDeactivations) > 0;

    public static bool HasIgnoreDeploymentEnvDeactivations(this LoggerActivationFlags flags) =>
        (flags & LoggerActivationFlags.IgnoreDeploymentEnvDeactivations) > 0;

    public static bool HasIgnoreAllEnvironmentDeactivations(this LoggerActivationFlags flags) =>
        (flags & LoggerActivationFlags.IgnoreAllEnvironmentDeactivations) > 0;

    public static bool HasLargeMessage(this LoggerActivationFlags flags)     => (flags & LoggerActivationFlags.LargeMessage) > 0;
    public static bool HasVeryLargeMessage(this LoggerActivationFlags flags) => (flags & LoggerActivationFlags.VeryLargeMessage) > 0;

    public static bool HasPerLocationPercentilesAtInterval(this LoggerActivationFlags flags) =>
        (flags & LoggerActivationFlags.PerLocationPercentilesAtInterval) > 0;

    public static bool HasCacheBacktraceReleaseOnly(this LoggerActivationFlags flags) =>
        (flags & LoggerActivationFlags.CacheBacktraceReleaseOnly) > 0;
}
