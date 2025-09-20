// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config.LoggersHierarchy.ActivationProfiles;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Options;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower;

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

        var stsa = new TheOneString().Initialize(StringStyle.PlainText);
        
        
        TargetStringBearerRevealState.CallBaseStyledToString<FLoggerActivationConfig, FLogBuildTypeAndDeployEnvConfig>(baseHasStyledToString, stsa);
        
        Console.Out.WriteLine("Without virtual dispatch = " + stsa.WriteBuffer.ToString());

        Assert.AreEqual("FLogBuildTypeAndDeployEnvConfig { BuildType: ReleaseBuild, DeploymentEnvironment: AnyEnv }", stsa.WriteBuffer.ToString());

        stsa.Clear();
        baseHasStyledToString.RevealState(stsa);
        
        Console.Out.WriteLine("With virtual dispatch = " + stsa.WriteBuffer.ToString());

        Assert.AreEqual("FLoggerActivationConfig { LoggerActivationFlags: StopWatch, GlobalLoggerTimeInterval, " +
                        "BuildType: ReleaseBuild, DeploymentEnvironment: AnyEnv }" 
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

        var stsa = new TheOneString().Initialize(StringStyle.PlainText);
        
        
        TargetStringBearerRevealState.CallBaseStyledToStringIfSupported(baseHasStyledToString, stsa);
        
        Console.Out.WriteLine("Without virtual dispatch = " + stsa.WriteBuffer.ToString());

        Assert.AreEqual("FLogBuildTypeAndDeployEnvConfig { BuildType: ReleaseBuild, DeploymentEnvironment: AnyEnv }", stsa.WriteBuffer.ToString());

        stsa.Clear();
        baseHasStyledToString.RevealState(stsa);
        
        Console.Out.WriteLine("With virtual dispatch = " + stsa.WriteBuffer.ToString());

        Assert.AreEqual("FLoggerActivationConfig { LoggerActivationFlags: StopWatch, GlobalLoggerTimeInterval, " +
                        "BuildType: ReleaseBuild, DeploymentEnvironment: AnyEnv }", stsa.WriteBuffer.ToString());
        
    }
}
