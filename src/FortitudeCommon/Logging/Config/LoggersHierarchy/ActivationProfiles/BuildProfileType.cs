// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Config.LoggersHierarchy.ActivationProfiles;


// Aligns with LoggerActivationFlags Bits
public enum FLogBuildTypeProfile : byte
{
    None = 0
  , DebugBuild = 2
  , ReleaseBuild = 4
  , AllBuildTypes = 6
}

public static class FLogBuildProfileTypeExtensions
{
    public static  uint AllToNoneOrOriginalAsUint(this FLogBuildTypeProfile fLogBuildTypeProfile) => 
        fLogBuildTypeProfile == FLogBuildTypeProfile.AllBuildTypes ? (uint)FLogBuildTypeProfile.None : (uint)fLogBuildTypeProfile;
    
    
}