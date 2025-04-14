// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace PublishingRecordingAndReplaying;

public class CommandLineOptions(DistributionType distributionType, Venues source)
{
    private const string PublisherCmdLnOption                = "--publisher";
    private const string SubscriberCmdLnOption               = "--subscriber";
    private const string VenueCmdLnOption                    = "--venue=";
    private const string PublisherDistinctQuotesCmdLnOption  = "--numDistinctQuotes=";
    private const string PublisherRepeatCmdLnOption          = "--repeat=";
    private const string PublisherQuoteIntervalMsCmdLnOption = "--publishIntervalMs=";

    public Venues           Source                 { get; private set; }
    public DistributionType RuntAs                 { get; private set; }
    public uint             DistinctQuotes         { get; private set; } = 10;
    public uint             AverageQuoteIntervalMs { get; private set; } = 2_000;
    public uint             RepeatTimes            { get; private set; } = 10_000_000;

    public bool ShouldExit => Source == Venues.NotSet || RuntAs == DistributionType.NotSet;

    public static CommandLineOptions ParseCommandLine(string[] args)
    {
        var cmdLineOpts = new CommandLineOptions(DistributionType.NotSet, Venues.NotSet);

        foreach (var cla in args)
            try
            {
                switch (cla)
                {
                    case PublisherCmdLnOption:  cmdLineOpts.RuntAs = DistributionType.Publisher; break;
                    case SubscriberCmdLnOption: cmdLineOpts.RuntAs = DistributionType.Subscriber; break;
                    default:
                        if (cla.Contains(VenueCmdLnOption))
                        {
                            var venueStr = cla.Replace(VenueCmdLnOption, "");
                            cmdLineOpts.Source = Enum.Parse<Venues>(venueStr);
                        }
                        else if (cla.Contains(PublisherDistinctQuotesCmdLnOption))
                        {
                            var distinctQuotes = cla.Replace(PublisherDistinctQuotesCmdLnOption, "");
                            cmdLineOpts.DistinctQuotes = uint.Parse(distinctQuotes);
                        }
                        else if (cla.Contains(PublisherRepeatCmdLnOption))
                        {
                            var repeatTimes = cla.Replace(PublisherRepeatCmdLnOption, "");
                            cmdLineOpts.RepeatTimes = uint.Parse(repeatTimes);
                        }
                        else if (cla.Contains(PublisherQuoteIntervalMsCmdLnOption))
                        {
                            var repeatTimes = cla.Replace(PublisherQuoteIntervalMsCmdLnOption, "");
                            cmdLineOpts.AverageQuoteIntervalMs = uint.Parse(repeatTimes);
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine($"Caught error parsing {cla}.  Got {e}");
                Console.Out.WriteLine();
                DisplayCommandLineOptions();
                return new CommandLineOptions(DistributionType.NotSet, Venues.NotSet);
            }
        if (cmdLineOpts.ShouldExit) DisplayCommandLineOptions();
        return cmdLineOpts;
    }

    private static void DisplayCommandLineOptions()
    {
        Console.Out.WriteLine("This example application requires at least two launches of the application.");
        Console.Out.WriteLine("One as a publisher and another as a subscriber to the same venue.");
        Console.Out.WriteLine("");
        Console.Out.WriteLine("Usage: > examplePubSub.exe (--publisher | --subscriber) --venue=<Venue> [--numDistinctQuotes=###] [--repeat=###] [--publishIntervalMs=###]");
        Console.Out.WriteLine();
        Console.Out.WriteLine("Venue Options:");
        Console.Out.WriteLine("                 ExampleFuturesExchange");
        Console.Out.WriteLine("                 ExampleStockExchange1");
        Console.Out.WriteLine("                 ExampleStockExchange2");
        Console.Out.WriteLine("                 ExampleCurrencyExchange");
        Console.Out.WriteLine("                 ExampleBank1");
        Console.Out.WriteLine("                 ExampleBank2");
        Console.Out.WriteLine("                 ExampleBank3");
        Console.Out.WriteLine();
        Console.Out.WriteLine("Additional Publisher Options:");
        Console.Out.WriteLine("  numDistinctQuotes     :     Number of distinct quotes to publish before repeating the sequence.  Default value 10.");
        Console.Out.WriteLine("  repeat                :     Number of times to repeat publishing before exiting the publisher.  Default 10,000,000");
        Console.Out.WriteLine("  publishIntervalMs     :     Average time between publishing each quote.  Default 2,000 ms");
        Console.Out.WriteLine();
    }
}
