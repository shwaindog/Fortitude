// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Core.Appending;

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

    public IFLogEntrySink Appender { get; init; }

    public void OnUpdated(IAppenderDefinitionConfig newConfig)
    {
        ConfigUpdated?.Invoke(newConfig);
    }

    public void Deconstruct(out IFLogEntrySink appender)
    {
        appender = Appender;
    }
}

public interface IFLogAppenderRegistry
{
    void RegistryAppenderInterest(NotifyAppenderHandler onRegisteredCallback, string appenderName);

    NotifyNewAppenderHandler RegisterAppenderCallback { get; }
}

public interface IMutableFLogAppenderRegistry : IFLogAppenderRegistry
{
    Dictionary<string, IMutableAppenderDefinitionConfig> DefinedAppenderConfigs { get; set; }
}

public class FLogAppenderRegistry : IMutableFLogAppenderRegistry
{
    private static readonly object SyncLock = new ();

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

    public virtual void AddAppenderCreated(IFLogAppender newlyCreatedAppender)
    {
        if (!embodiedAppenders.TryAdd(newlyCreatedAppender.AppenderName, newlyCreatedAppender))
        {
            Console.Out.WriteLine($"Warning appender with name {newlyCreatedAppender.AppenderName} already exists!");
            Console.Out.WriteLine("This appender may not receive forward requests as the the original will be bound to");
        }
    }

    public void RegistryAppenderInterest(NotifyAppenderHandler onRegisteredCallback, string appenderName)
    {
        // ReSharper disable once InconsistentlySynchronizedField
        if (embodiedAppenders.TryGetValue(appenderName, out var existingAppender))
        {
            onRegisteredCallback(existingAppender);
        }
        if (definedAppenderConfigs.TryGetValue(appenderName, out var requestedAppenderConfig))
        {
            lock (SyncLock)
            {
                if (embodiedAppenders.TryGetValue(appenderName, out var tryAgainExistingAppender))
                {
                    onRegisteredCallback(tryAgainExistingAppender);
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
                        onRegisteredCallback(newAppender);
                    }
                }
            }
        }
        else
        {
            Console.Out.WriteLine($"Returning NullAppender for Appender.Name {appenderName} no config definition found");
            var nullAppender = new NullAppender(new NullAppenderConfig(), FLogContext.Context);
            onRegisteredCallback(nullAppender);
        }
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
