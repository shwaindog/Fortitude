// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Formatting;

public interface IInheritingAppendersLookupConfig : INamedAppendersLookupConfig<IFormattingAppenderConfig>
{
}

public interface IAppendableInheritingAppendersLookupConfig : IInheritingAppendersLookupConfig
  , IAppendableNamedAppendersLookupConfig<IMutableFormattingAppenderConfig, IFormattingAppenderConfig>
{
    new IAppendableInheritingAppendersLookupConfig Clone();
}

public class InheritingAppendersLookupConfig : NamedAppendersLookupConfig<IMutableFormattingAppenderConfig, IFormattingAppenderConfig>
  , IAppendableInheritingAppendersLookupConfig
{
    public InheritingAppendersLookupConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public InheritingAppendersLookupConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public InheritingAppendersLookupConfig(params IMutableFormattingAppenderConfig[] toAdd)
        : this(InMemoryConfigRoot, InMemoryPath, toAdd) { }

    public InheritingAppendersLookupConfig
        (IConfigurationRoot root, string path, params IMutableFormattingAppenderConfig[] toAdd) : base(root, path, toAdd) { }

    public InheritingAppendersLookupConfig(IInheritingAppendersLookupConfig toClone, IConfigurationRoot root, string path)
        : base(toClone, root, path) { }

    public InheritingAppendersLookupConfig(IInheritingAppendersLookupConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    IAppendableInheritingAppendersLookupConfig IAppendableInheritingAppendersLookupConfig.Clone() => Clone();

    public override InheritingAppendersLookupConfig Clone() => new(this);

    public override StyledTypeBuildResult ToString(IStyledTypeStringAppender sbc)
    {
        return
            sbc.StartKeyedCollectionType(nameof(InheritingAppendersLookupConfig))
               .AddAll(AppendersByName)
               .Complete();
    }
}
