// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Rules;

#endregion

namespace PublishingRecordingAndReplaying.Step_1;

public class QuotePublisher(CommandLineOptions cmdCommandLineOptions) : Rule
{
    private readonly Venues venue;

    public override void Start()
    {
        Console.Out.WriteLine($"{DateTime.Now:hh:mm:ss.ffffff} - I say Hello");
    }

    public override void Stop()
    {
        Console.Out.WriteLine($"{DateTime.Now:hh:mm:ss.ffffff} - Stopping Quote Publisher for ");
    }
}
