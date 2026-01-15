// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config.LoggersHierarchy.ActivationProfiles;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Types.StringsOfPower;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios;

[TestClass]
public class TargetStringBearerRevealStateTests
{
    [TestMethod]
    public void ClassWIthOverridenStyledToStringCallsBasePassBaseType()
    {
        var baseHasStyledToString = new FLoggerActivationConfig
        {
            LoggerActivationFlags = LoggerActivationFlags.GlobalLoggerTimeInterval | LoggerActivationFlags.StopWatch
          , DeploymentEnvironment = FlogDeploymentEnvironmentProfileType.PerfEnv | FlogDeploymentEnvironmentProfileType.PreReleaseEnv
          , BuildType             = FLogBuildTypeProfile.ReleaseBuild
        };

        var stsa = new TheOneString().Initialize();
        
        
        TargetStringBearerRevealState.CallBaseStyledToString<FLoggerActivationConfig, FLogBuildTypeAndDeployEnvConfig>(baseHasStyledToString, stsa);
        
        Console.Out.WriteLine("Without virtual dispatch = " + stsa.WriteBuffer.ToString());

        Assert.AreEqual(
                        """
                        FLoggerActivationConfig {
                         BuildType: FLogBuildTypeProfile.ReleaseBuild,
                         DeploymentEnvironment: FlogDeploymentEnvironmentProfileType.AnyEnv 
                        }
                        """.RemoveLineEndings()
                      , stsa.WriteBuffer.ToString());

        stsa.Clear();
        baseHasStyledToString.RevealState(stsa);
        
        Console.Out.WriteLine("With virtual dispatch = " + stsa.WriteBuffer.ToString());

        Assert.AreEqual(
                        """
                        FLoggerActivationConfig {
                         LoggerActivationFlags: LoggerActivationFlags.StopWatch | LoggerActivationFlags.GlobalLoggerTimeInterval,
                         BuildType: FLogBuildTypeProfile.ReleaseBuild,
                         DeploymentEnvironment: FlogDeploymentEnvironmentProfileType.AnyEnv 
                        }
                        """.RemoveLineEndings() 
                      , stsa.WriteBuffer.ToString());
        
    }
    
    [TestMethod]
    public void ClassWIthOverridenStyledToStringCallsBaseWithoutPassBaseType()
    {
        var baseHasStyledToString = new FLoggerActivationConfig
        {
            LoggerActivationFlags = LoggerActivationFlags.GlobalLoggerTimeInterval | LoggerActivationFlags.StopWatch
          , DeploymentEnvironment = FlogDeploymentEnvironmentProfileType.PerfEnv | FlogDeploymentEnvironmentProfileType.PreReleaseEnv
          , BuildType             = FLogBuildTypeProfile.ReleaseBuild
        };

        var stsa = new TheOneString().Initialize();
        
        
        TargetStringBearerRevealState.CallBaseStyledToStringIfSupported(baseHasStyledToString, stsa);
        
        Console.Out.WriteLine("Without virtual dispatch = " + stsa.WriteBuffer.ToString());

        Assert.AreEqual(
                        """
                        FLoggerActivationConfig {
                         BuildType: FLogBuildTypeProfile.ReleaseBuild,
                         DeploymentEnvironment: FlogDeploymentEnvironmentProfileType.AnyEnv 
                        }
                        """.RemoveLineEndings()
                      , stsa.WriteBuffer.ToString());

        stsa.Clear();
        baseHasStyledToString.RevealState(stsa);
        
        Console.Out.WriteLine("With virtual dispatch = " + stsa.WriteBuffer.ToString());

        Assert.AreEqual(
                        """
                        FLoggerActivationConfig {
                         LoggerActivationFlags: LoggerActivationFlags.StopWatch | LoggerActivationFlags.GlobalLoggerTimeInterval,
                         BuildType: FLogBuildTypeProfile.ReleaseBuild,
                         DeploymentEnvironment: FlogDeploymentEnvironmentProfileType.AnyEnv 
                        }
                        """.RemoveLineEndings()
                      , stsa.WriteBuffer.ToString());
        
    }
}
