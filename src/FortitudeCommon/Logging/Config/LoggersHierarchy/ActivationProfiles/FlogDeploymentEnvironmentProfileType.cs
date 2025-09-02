// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Config.LoggersHierarchy.ActivationProfiles;

// Aligns with LoggerActivationFlags Bits
[Flags]
public enum FlogDeploymentEnvironmentProfileType : ushort
{
    None              = 0x00_00
  , DevEnv            = 0x00_10 // local machine logging, or (unit tests, build machine, developer machine) could go here 
  , TestEnv           = 0x00_20 // or here
  , TestEnvOrLower    = 0x00_30
  , PreReleaseEnv        = 0x00_40 // User Acceptance Test (UAT) Environment, Development Test, Pre-Prod etc...
  , PreReleaseEnvOrLower = 0x00_70 // 
  , PerfEnv           = 0x00_80 // Performance Test Environment, Pre-Prod
  , PerfEnvOrLower    = 0x00_F0 //  
  , ProdEnv           = 0x01_00 // Production Environment
  , AnyEnv            = 0x01_F0
}

public static class FlogDeploymentEnvironmentProfileTypeExtensions
{
    public static uint AllToNoneOrOriginalAsUint(this FlogDeploymentEnvironmentProfileType flogDeploymentEnvironmentProfile) =>
        flogDeploymentEnvironmentProfile == FlogDeploymentEnvironmentProfileType.AnyEnv
            ? (uint)FlogDeploymentEnvironmentProfileType.None
            : (uint)flogDeploymentEnvironmentProfile;
}
