// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.Appending.Formatting.Console;
using FortitudeCommon.Logging.Core.Appending;
using FortitudeCommon.Logging.Core.Appending.Formatting.ConsoleOut;
using FortitudeCommon.Logging.Core.Appending.Forwarding;

namespace FortitudeCommon.Logging.Core.Hub;

public delegate void HandleAppenderConfigUpdate(IAppenderDefinitionConfig appenderConfig);

public delegate void NotifyAppenderHandler(IFLogAppender appender);

public delegate void NotifyNewAppenderHandler(IMutableFLogAppender appender);

public record AppenderContainer
{
    public AppenderContainer(IMutableFLogAppender Appender)
    {
        this.Appender = Appender;

        ConfigUpdated += Appender.HandleConfigUpdate;
    }

    public event HandleAppenderConfigUpdate? ConfigUpdated;

    public IMutableFLogAppender Appender { get; init; }

    public void OnUpdated(IAppenderDefinitionConfig newConfig)
    {
        ConfigUpdated?.Invoke(newConfig);
    }

    public void Deconstruct(out IMutableFLogAppender appender)
    {
        appender = Appender;
    }
}

public interface IFLogAppenderRegistry
{
    IAppenderClient GetAppenderClient(string appenderName, IFLoggerCommon logger);
    IAppenderClient GetAppenderClient(string appenderName, IFLogForwardingAppender forwardingAppender);

    NotifyNewAppenderHandler RegisterAppenderCallback { get; }

    IAppenderClient FailAppender { get; }
}

public interface IMutableFLogAppenderRegistry : IFLogAppenderRegistry
{
    Dictionary<string, IMutableAppenderDefinitionConfig> DefinedAppenderConfigs { get; set; }
}

public class FLogAppenderRegistry : IMutableFLogAppenderRegistry
{
    private static readonly object SyncLock = new ();

    private Lazy<IFLogAppender?> failAppender       = 
        new ( ()=> new FLogConsoleAppender(ConsoleAppenderConfig.DefaultConsoleAppenderConfig, FLogContext.Context));

    private IAppenderClient?     failAppenderClient;

    private readonly ConcurrentDictionary<string, IFLogAppender> embodiedAppenders = new();

    private Dictionary<string, IMutableAppenderDefinitionConfig> definedAppenderConfigs;

    public FLogAppenderRegistry(Dictionary<string, IMutableAppenderDefinitionConfig> appenderConfigs)
    {
        definedAppenderConfigs = appenderConfigs;

        RegisterAppenderCallback = AddAppenderCreated;
    }

    Dictionary<string, IMutableAppenderDefinitionConfig> IMutableFLogAppenderRegistry.DefinedAppenderConfigs
    {
        get => definedAppenderConfigs;
        set => definedAppenderConfigs = value;
    }

    public NotifyNewAppenderHandler RegisterAppenderCallback { get; }

    public IAppenderClient FailAppender
    {
        get => failAppenderClient ??= failAppender.Value!.CreateAppenderClientFor(FLogContext.Context.LoggerRegistry.Root);
        set => failAppenderClient = value;
    }

    public virtual void AddAppenderCreated(IFLogAppender newlyCreatedAppender)
    {
        if (!embodiedAppenders.TryAdd(newlyCreatedAppender.AppenderName, newlyCreatedAppender))
        {
            Console.Out.WriteLine($"Warning appender with name {newlyCreatedAppender.AppenderName} already exists!");
            Console.Out.WriteLine("This appender may not receive forward requests as the the original will be bound to");
        }
    }

    public IAppenderClient GetAppenderClient(string appenderName, IFLoggerCommon logger)
    {
        var appender = GetAppender(appenderName);
        return appender.CreateAppenderClientFor(logger);
    }

    public IAppenderClient GetAppenderClient(string appenderName, IFLogForwardingAppender forwardingAppender)
    {
        var appender = GetAppender(appenderName);
        return appender.CreateAppenderClientFor(forwardingAppender);
    }

    protected virtual IFLogAppender GetAppender(string appenderName)
    {
        // ReSharper disable once InconsistentlySynchronizedField
        if (embodiedAppenders.TryGetValue(appenderName, out var existingAppender))
        {
            return existingAppender;
        }
        if (definedAppenderConfigs.TryGetValue(appenderName, out var requestedAppenderConfig))
        {
            lock (SyncLock)
            {
                if (embodiedAppenders.TryGetValue(appenderName, out var tryAgainExistingAppender))
                {
                    return tryAgainExistingAppender;
                }
                else
                {
                    var newAppender = FLogCreate.MakeAppender(requestedAppenderConfig, FLogContext.Context);
                    if (newAppender != null)
                    {
                        embodiedAppenders.TryAdd(newAppender.AppenderName, newAppender);
                    }
                    else
                    {
                        Console.Out.WriteLine($"Returning NullAppender for Appender.Name {appenderName} and config {requestedAppenderConfig}");
                        newAppender = new NullAppender(new NullAppenderConfig(), FLogContext.Context);
                        return newAppender;
                    }
                }
            }
        }
        Console.Out.WriteLine($"Returning NullAppender for Appender.Name {appenderName} no config definition found");
        var nullAppender = new NullAppender(new NullAppenderConfig(), FLogContext.Context);
        return nullAppender;
    }
}

public static class FLogAppenderRegistryExtensions
{
    public static IMutableFLogAppenderRegistry? UpdateConfig
    (
        this IFLogAppenderRegistry? maybeCreated
      , Dictionary<string, IMutableAppenderDefinitionConfig> appenderConfig)
    {
        if (maybeCreated is IMutableFLogAppenderRegistry maybeMutable)
        {
            maybeMutable.DefinedAppenderConfigs = appenderConfig;
            return maybeMutable;
        }
        return null;
    }
}
