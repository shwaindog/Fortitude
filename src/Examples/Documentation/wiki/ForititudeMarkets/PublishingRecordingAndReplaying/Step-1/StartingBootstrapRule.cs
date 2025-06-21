// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Rules;

#endregion

namespace PublishingRecordingAndReplaying.Step_1;

public class StartingBootstrapRule(CommandLineOptions cmdLineOptions) : Rule
{
    private readonly bool   isPublisher = cmdLineOptions.RunAs == DistributionType.Publisher;
    private readonly Venues venue       = cmdLineOptions.Source;

    public override async ValueTask StartAsync() { }
}
