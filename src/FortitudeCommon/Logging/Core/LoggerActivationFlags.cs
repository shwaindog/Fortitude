// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Runtime.CompilerServices;
using FortitudeCommon.Logging.Config.LoggersHierarchy.ActivationProfiles;

namespace FortitudeCommon.Logging.Core;

[Flags]
public enum LoggerActivationFlags : uint
{
    Disabled                      = 0x00_00_00_00
  , MergeConfigBuildType          = 0x00_00_00_01
  , MergeConfigDeployEnv          = 0x00_00_00_02
  , MergeLoggerActivationConfig   = 0x00_00_00_03
  , DebugBuild                    = 0x00_00_00_04
  , DebugBuildAnyDeployEnv        = 0x08_00_00_04
  , ReleaseBuild                  = 0x00_00_00_08
  , ReleaseBuildAnyDeployEnv      = 0x08_00_00_08
  , AnyBuildType                  = 0x00_00_00_0C
  , DevEnv                        = 0x00_00_00_10
  , DevEnvBuildType               = 0x04_00_00_10
  , TestEnv                       = 0x00_00_00_20
  , TestEnvAnyBuildType           = 0x04_00_00_20
  , TestEnvOrLower                = 0x00_00_00_30
  , TestEnvOrLowerAnyBuildType    = 0x04_00_00_30
  , PreReleaseEnv                 = 0x00_00_00_40
  , PreReleaseAnyBuildType        = 0x04_00_00_40
  , PreReleaseOrLower             = 0x00_00_00_70
  , PreReleaseOrLowerAnyBuildType = 0x04_00_00_70
  , PerfEnv                       = 0x00_00_00_80
  , PerfEnvAnyBuildType           = 0x04_00_00_80
  , PerfEnvOrLower                = 0x00_00_00_F0
  , PerfEnvOrLowerAnyBuildType    = 0x04_00_00_F0
  , ProdEnv                       = 0x00_00_01_00
  , ProdEnvAnyBuildType           = 0x04_00_01_00
  , AnyDeployEnv                  = 0x00_00_01_F0
  , AnyDeployEnvAnyBuildType      = 0x0C_00_01_F0
  , StopWatch                     = 0x00_00_02_00
  , AlwaysForFLogLevel            = 0x0C_00_03_FC

  , AtIntervalPerCallLocation = 0x00_00_04_00
  , AtIntervalLoggerGlobal    = 0x00_00_08_00
  , PerCorrelationIdInterval  = 0x00_00_10_00
  , AddSkipCount              = 0x00_00_20_00
  , PerLocationTimeInterval   = 0x00_00_40_00
  , GlobalLoggerTimeInterval  = 0x00_00_80_00

  , LargeMessage     = 0x00_01_00_00
  , VeryLargeMessage = 0x00_02_00_00

  , NotDebugBuild        = 0x00_04_00_03
  , NotReleaseBuild      = 0x00_08_00_03
  , NotDevEnv            = 0x00_10_00_03
  , NotTestEnv           = 0x00_20_00_03
  , NotTestEnvOrLower    = 0x00_30_00_03
  , NotPreRelease        = 0x00_40_00_03
  , NotPreReleaseOrLower = 0x00_70_00_03
  , NotPerfEnv           = 0x00_80_00_03
  , NotPerfEnvOrLower    = 0x00_F0_00_03
  , NotProdEnv           = 0x01_00_00_03
  , NotAnyDeployEnv      = 0x01_F0_00_00
  , NotStopWatch         = 0x02_00_00_03

  , NoBuildTypeExclusions  = 0x04_00_00_03
  , NoDeployEnvExclusions  = 0x08_00_00_03
  , NoActivationExclusions = 0x0C_00_00_03

  , PerLocationPercentilesAtInterval = 0x10_00_00_00
  , CacheBacktraceReleaseOnly        = 0x20_00_00_03
  , CheckMemberCallConfig            = 0x40_00_00_00
  , IgnoreLogLevelExclusions         = 0x80_00_00_00
}

public static class LoggerActivationFlagsExtensions
{
    private const uint BuildTypesMask            = (uint)LoggerActivationFlags.AnyBuildType;
    private const uint DeploymentEnvironmentMask = (uint)LoggerActivationFlags.AnyDeployEnv;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasActivationExclusion(this LoggerActivationFlags requestActivationFlags, LoggerActivationFlags fLogEnv)
    {
        var currentEnvAsUint = (uint)fLogEnv;
        var shouldActive = requestActivationFlags.HasActivationExclusion
            ((FLogBuildTypeProfile)(currentEnvAsUint & BuildTypesMask),
             (FlogDeploymentEnvironmentProfileType)(currentEnvAsUint & DeploymentEnvironmentMask));
        return shouldActive;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasActivationExclusion(this LoggerActivationFlags requestActivationFlags, FLogBuildTypeProfile fLogEnvBuildType
      , FlogDeploymentEnvironmentProfileType flogEnvDeployEnv)
    {
        var buildProfilePass = requestActivationFlags.BuildProfileMeetsCriteria(fLogEnvBuildType) ||
                               requestActivationFlags.HasNoBuildTypeExclusions();
        var deployEnvProfilePass = requestActivationFlags.DeploymentProfileMeetsCriteria(flogEnvDeployEnv) ||
                                   requestActivationFlags.HasNoDeployEnvExclusions();

        return !(buildProfilePass && deployEnvProfilePass);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool DeploymentProfileMeetsCriteria(this LoggerActivationFlags loggerActivationFlags
      , FlogDeploymentEnvironmentProfileType currentBuildProfile)
    {
        var buildProfileUint    = (uint)currentBuildProfile;
        var buildProfileShifted = currentBuildProfile.AllToNoneOrOriginalAsUint() << 16;
        var activateFlagsUint   = (uint)loggerActivationFlags;
        return (buildProfileUint & activateFlagsUint) > 0 && (buildProfileShifted & activateFlagsUint) == 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool BuildProfileMeetsCriteria(this LoggerActivationFlags loggerActivationFlags, FLogBuildTypeProfile currentFLogBuildTypeProfile)
    {
        var buildProfileUint    = (uint)currentFLogBuildTypeProfile;
        var buildProfileShifted = currentFLogBuildTypeProfile.AllToNoneOrOriginalAsUint() << 16;
        var activateFlagsUint   = (uint)loggerActivationFlags;
        return (buildProfileUint & activateFlagsUint) > 0 && (buildProfileShifted & activateFlagsUint) == 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LoggerActivationFlags MergeWithConfigIfAllowed(this LoggerActivationFlags received, LoggerActivationFlags configActivationProfile)
    {
        if (received.HasMergeInConfigProfile())
        {
            var activationProfile                                   = (uint)configActivationProfile;
            if (received.HasMergeBuildConfigProfile()) received     |= (LoggerActivationFlags)(activationProfile & BuildTypesMask);
            if (received.HasMergeDeployEnvConfigProfile()) received |= (LoggerActivationFlags)(activationProfile & DeploymentEnvironmentMask);
        }
        return received;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasMergeBuildConfigProfile(this LoggerActivationFlags flags) => (flags & LoggerActivationFlags.MergeConfigBuildType) > 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasMergeDeployEnvConfigProfile(this LoggerActivationFlags flags) => (flags & LoggerActivationFlags.MergeConfigDeployEnv) > 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasMergeInConfigProfile(this LoggerActivationFlags flags) => (flags & LoggerActivationFlags.MergeLoggerActivationConfig) > 0;

    public static bool HasDebugBuild(this LoggerActivationFlags flags)   => (flags & LoggerActivationFlags.DebugBuild) > 0;
    public static bool HasReleaseBuild(this LoggerActivationFlags flags) => (flags & LoggerActivationFlags.ReleaseBuild) > 0;
    public static bool HasDevEnv(this LoggerActivationFlags flags)       => (flags & LoggerActivationFlags.DevEnv) > 0;
    public static bool HasTestEnv(this LoggerActivationFlags flags)      => (flags & LoggerActivationFlags.TestEnv) > 0;

    public static bool HasTestEnvOrLower(this LoggerActivationFlags flags) =>
        (flags & LoggerActivationFlags.TestEnvOrLower) == LoggerActivationFlags.TestEnvOrLower;

    public static bool HasPreReleaseEnv(this LoggerActivationFlags flags) => (flags & LoggerActivationFlags.PreReleaseEnv) > 0;

    public static bool HasPreReleaseOrLower(this LoggerActivationFlags flags) =>
        (flags & LoggerActivationFlags.PreReleaseOrLower) == LoggerActivationFlags.PreReleaseOrLower;

    public static bool HasPerfEnv(this LoggerActivationFlags flags) => (flags & LoggerActivationFlags.PerfEnv) > 0;

    public static bool HasPerfEnvOrLower(this LoggerActivationFlags flags) =>
        (flags & LoggerActivationFlags.PerfEnvOrLower) == LoggerActivationFlags.PerfEnvOrLower;

    public static bool HasProdEnv(this LoggerActivationFlags flags) => (flags & LoggerActivationFlags.ProdEnv) > 0;

    public static bool HasAnyDeployEnv(this LoggerActivationFlags flags) =>
        (flags & LoggerActivationFlags.AnyDeployEnv) == LoggerActivationFlags.AnyDeployEnv;

    public static bool HasStopWatch(this LoggerActivationFlags flags) => (flags & LoggerActivationFlags.StopWatch) > 0;

    public static bool HasAlwaysForFLogLevel(this LoggerActivationFlags flags) =>
        (flags & LoggerActivationFlags.AlwaysForFLogLevel) == LoggerActivationFlags.AlwaysForFLogLevel;


    public static bool HasAtIntervalPerLocation(this LoggerActivationFlags flags)    => (flags & LoggerActivationFlags.AtIntervalPerCallLocation) > 0;
    public static bool HasAtIntervalLoggerGlobal(this LoggerActivationFlags flags)   => (flags & LoggerActivationFlags.AtIntervalLoggerGlobal) > 0;
    public static bool HasPerCorrelationIdInterval(this LoggerActivationFlags flags) => (flags & LoggerActivationFlags.PerCorrelationIdInterval) > 0;
    public static bool HasAddSkipCount(this LoggerActivationFlags flags)             => (flags & LoggerActivationFlags.AddSkipCount) > 0;
    public static bool HasPerLocationTimeInterval(this LoggerActivationFlags flags)  => (flags & LoggerActivationFlags.PerLocationTimeInterval) > 0;
    public static bool HasGlobalLoggerTimeInterval(this LoggerActivationFlags flags) => (flags & LoggerActivationFlags.GlobalLoggerTimeInterval) > 0;

    public static bool HasLargeMessage(this LoggerActivationFlags flags)         => (flags & LoggerActivationFlags.LargeMessage) > 0;
    public static bool HasVeryLargeMessage(this LoggerActivationFlags flags)     => (flags & LoggerActivationFlags.VeryLargeMessage) > 0;
    public static bool HasNotDebugBuild(this LoggerActivationFlags flags)        => (flags & LoggerActivationFlags.NotDebugBuild) > 0;
    public static bool HasNotReleaseBuild(this LoggerActivationFlags flags)      => (flags & LoggerActivationFlags.NotReleaseBuild) > 0;
    public static bool HasNotDevEnv(this LoggerActivationFlags flags)            => (flags & LoggerActivationFlags.NotDevEnv) > 0;
    public static bool HasNotTestEnv(this LoggerActivationFlags flags)           => (flags & LoggerActivationFlags.NotTestEnv) > 0;
    public static bool HasNotTestEnvOrLower(this LoggerActivationFlags flags)    => (flags & LoggerActivationFlags.NotTestEnvOrLower) > 0;
    public static bool HasNotPreRelease(this LoggerActivationFlags flags)        => (flags & LoggerActivationFlags.NotPreRelease) > 0;
    public static bool HasNotPreReleaseOrLower(this LoggerActivationFlags flags) => (flags & LoggerActivationFlags.NotPreReleaseOrLower) > 0;
    public static bool HasNotPerfEnv(this LoggerActivationFlags flags)           => (flags & LoggerActivationFlags.NotPerfEnv) > 0;
    public static bool HasNotPerfEnvOrLower(this LoggerActivationFlags flags)    => (flags & LoggerActivationFlags.NotPerfEnvOrLower) > 0;
    public static bool HasNotProdEnv(this LoggerActivationFlags flags)           => (flags & LoggerActivationFlags.NotProdEnv) > 0;
    public static bool HasNotAnyDeployEnv(this LoggerActivationFlags flags)      => (flags & LoggerActivationFlags.NotAnyDeployEnv) > 0;
    public static bool HasNotStopWatch(this LoggerActivationFlags flags)         => (flags & LoggerActivationFlags.NotStopWatch) > 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasNoBuildTypeExclusions(this LoggerActivationFlags flags) => (flags & LoggerActivationFlags.NoBuildTypeExclusions) > 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasNoDeployEnvExclusions(this LoggerActivationFlags flags) => (flags & LoggerActivationFlags.NoDeployEnvExclusions) > 0;

    public static bool HasNoActivationExclusions(this LoggerActivationFlags flags) => (flags & LoggerActivationFlags.NoActivationExclusions) > 0;

    public static bool HasPerLocationPercentilesAtInterval(this LoggerActivationFlags flags) =>
        (flags & LoggerActivationFlags.PerLocationPercentilesAtInterval) > 0;

    public static bool HasCacheBacktraceReleaseOnly(this LoggerActivationFlags flags) =>
        (flags & LoggerActivationFlags.CacheBacktraceReleaseOnly) > 0;

    public static bool HasCheckMemberCallConfig(this LoggerActivationFlags flags) => (flags & LoggerActivationFlags.CheckMemberCallConfig) > 0;

    public static bool HasIgnoreLogLevelFilter(this LoggerActivationFlags flags) => (flags & LoggerActivationFlags.IgnoreLogLevelExclusions) > 0;

    public static bool LogLevelExclusionsEnabled(this LoggerActivationFlags flags) => (flags & LoggerActivationFlags.IgnoreLogLevelExclusions) == 0;
}
