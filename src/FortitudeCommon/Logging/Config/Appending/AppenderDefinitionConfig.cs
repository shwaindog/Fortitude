// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Configuration;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending;

public interface IAppenderDefinitionConfig : IAppenderReferenceConfig
{
    string? InheritFromAppenderName { get; }

    bool IsTemplateOnlyDefinition { get; }

    int RunOnAsyncQueueNumber { get; }

    new IAppenderDefinitionConfig Clone();
}

public interface IMutableAppenderDefinitionConfig : IAppenderDefinitionConfig, IMutableAppenderReferenceConfig
{
    new bool IsTemplateOnlyDefinition { get; set; }

    new string? InheritFromAppenderName { get; set; }

    new int RunOnAsyncQueueNumber { get; set; }

    IMutableAppenderReferenceConfig GenerateReferenceToThis(bool deactivateHere = false);
}

public abstract class AppenderDefinitionConfig : AppenderReferenceConfig, IMutableAppenderDefinitionConfig
{
    protected AppenderDefinitionConfig(IConfigurationRoot root, string path) : base(root, path) { }

    protected AppenderDefinitionConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    protected AppenderDefinitionConfig
    (string appenderName, string appenderType, int runOnAsyncQueueNumber = 0, string? inheritFromAppenderName = null
      , bool isTemplateOnlyDefinition = false, bool deactivateHere = false)
        : this(InMemoryConfigRoot, InMemoryPath, appenderName, appenderType, runOnAsyncQueueNumber
             , inheritFromAppenderName, isTemplateOnlyDefinition, deactivateHere) { }

    protected AppenderDefinitionConfig
    (string appenderName, int runOnAsyncQueueNumber = 0, string? inheritFromAppenderName = null
      , bool isTemplateOnlyDefinition = false, bool deactivateHere = false)
        : this(InMemoryConfigRoot, InMemoryPath, appenderName, runOnAsyncQueueNumber
             , inheritFromAppenderName, isTemplateOnlyDefinition, deactivateHere) { }

    protected AppenderDefinitionConfig
    (IConfigurationRoot root, string path, string appenderName, string appenderType, int runOnAsyncQueueNumber = 0
      , string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false, bool deactivateHere = false)
        : base(root, path, appenderName, appenderType, deactivateHere)
    {
        RunOnAsyncQueueNumber    = runOnAsyncQueueNumber;
        InheritFromAppenderName  = inheritFromAppenderName;
        IsTemplateOnlyDefinition = isTemplateOnlyDefinition;
    }

    protected AppenderDefinitionConfig
    (IConfigurationRoot root, string path, string appenderName, int runOnAsyncQueueNumber = 0
      , string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false, bool deactivateHere = false)
        : base(root, path, appenderName, deactivateHere)
    {
        RunOnAsyncQueueNumber    = runOnAsyncQueueNumber;
        InheritFromAppenderName  = inheritFromAppenderName;
        IsTemplateOnlyDefinition = isTemplateOnlyDefinition;
    }

    protected AppenderDefinitionConfig(IAppenderDefinitionConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path)
    {
        InheritFromAppenderName  = toClone.InheritFromAppenderName;
        IsTemplateOnlyDefinition = toClone.IsTemplateOnlyDefinition;
        RunOnAsyncQueueNumber    = toClone.RunOnAsyncQueueNumber;
    }

    protected AppenderDefinitionConfig(IAppenderDefinitionConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public override string AppenderType
    {
        get => this[nameof(AppenderType)] ?? throw new ConfigurationErrorsException("Expected config to have set an AppenderType");
        set => this[nameof(AppenderType)] = value;
    }

    public string? InheritFromAppenderName
    {
        get => this[nameof(InheritFromAppenderName)];
        set => this[nameof(InheritFromAppenderName)] = value;
    }

    public bool IsTemplateOnlyDefinition
    {
        get => bool.TryParse(this[nameof(IsTemplateOnlyDefinition)], out var timePart) && timePart;
        set => this[nameof(IsTemplateOnlyDefinition)] = value.ToString();
    }

    public int RunOnAsyncQueueNumber
    {
        get => int.TryParse(this[nameof(RunOnAsyncQueueNumber)], out var timePart) ? timePart : 0;
        set => this[nameof(RunOnAsyncQueueNumber)] = value.ToString();
    }

    public IMutableAppenderReferenceConfig GenerateReferenceToThis(bool deactivateHere = false)
    {
        var appenderRef = new AppenderReferenceConfig(AppenderName, $"{nameof(FLoggerBuiltinAppenderType.Ref)}", deactivateHere);
        return appenderRef;
    }

    public override T Visit<T>(T visitor) => throw new NotImplementedException("Derived classes must override this method");

    object ICloneable.Clone() => Clone();

    IAppenderDefinitionConfig IAppenderDefinitionConfig.Clone() => Clone();

    public override bool AreEquivalent(IAppenderReferenceConfig? other, bool exactTypes = false)
    {
        if (other is not IAppenderDefinitionConfig appenderDefCfg) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var isTemplateSame = IsTemplateOnlyDefinition == appenderDefCfg.IsTemplateOnlyDefinition;
        var queueNumSame   = RunOnAsyncQueueNumber == appenderDefCfg.RunOnAsyncQueueNumber;

        var allAreSame = baseSame && isTemplateSame && queueNumSame;

        return allAreSame;
    }

    public override AppenderDefinitionConfig Clone() => throw new NotImplementedException("Derived classes must override this method");

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IAppenderReferenceConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ (InheritFromAppenderName?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ IsTemplateOnlyDefinition.GetHashCode();
            hashCode = (hashCode * 397) ^ RunOnAsyncQueueNumber.GetHashCode();
            return hashCode;
        }
    }

    public override StyledTypeBuildResult ToString(IStyledTypeStringAppender stsa) =>
        stsa.StartComplexType(this)
           .AddBaseStyledToStringFields(this)
           .Field.WhenNonDefaultAdd(nameof(RunOnAsyncQueueNumber), RunOnAsyncQueueNumber)
           .Field.WhenNonNullOrDefaultAdd(nameof(InheritFromAppenderName), InheritFromAppenderName)
           .Field.WhenNonDefaultAdd(nameof(IsTemplateOnlyDefinition), IsTemplateOnlyDefinition).Complete();

    public override string ToString() => this.DefaultToString();
}
