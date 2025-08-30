// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Config.LoggersHierarchy.ActivationProfiles;

// Aligns with LoggerActivationFlags Bits
[Flags]
public enum FlogDeploymentEnvironmentProfileType : ushort
{
    None                 = 0x00_00
  , LocalMachine         = 0x00_08 // unit tests, build machine, developer machine
  , SystemTest           = 0x00_10
  , SystemOrLocal        = 0x00_18 // SystemTest or LocalMachine Environment
  , Uat                  = 0x00_20 // User Acceptance Test (UAT) Environment
  , UatSystemOrLocal     = 0x00_38 // UAT Test or System Test or Local Machine Enironment
  , PerfTest             = 0x00_40 // Performance Test Environment
  , PerfUatSystemOrLocal = 0x00_78 // Performance or UAT or System Test or Local Machine Environment 
  , Production           = 0x00_80 
  , AllEnvironments      = 0x00_F8
}

public static class FlogDeploymentEnvironmentProfileTypeExtensions
{
    public static uint AllToNoneOrOriginalAsUint(this FlogDeploymentEnvironmentProfileType flogDeploymentEnvironmentProfile) =>
        flogDeploymentEnvironmentProfile == FlogDeploymentEnvironmentProfileType.AllEnvironments
            ? (uint)FlogDeploymentEnvironmentProfileType.None
            : (uint)flogDeploymentEnvironmentProfile;
}
