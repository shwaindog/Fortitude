// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding;

public interface IForwardingAppendersLookupConfig : INamedAppendersLookupConfig<IAppenderForwardingReferenceConfig>
{
}

public interface IAppendableForwardingAppendersLookupConfig : IForwardingAppendersLookupConfig, IAppendableNamedAppendersLookupConfig<IMutableAppenderForwardingReferenceConfig, IAppenderForwardingReferenceConfig>
{
}

public class ForwardingAppendersLookupConfig 
    : NamedAppendersLookupConfig<IMutableAppenderForwardingReferenceConfig, IAppenderForwardingReferenceConfig>
      , IAppendableForwardingAppendersLookupConfig
{
    public ForwardingAppendersLookupConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public ForwardingAppendersLookupConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public ForwardingAppendersLookupConfig(params IMutableAppenderForwardingReferenceConfig[] toAdd)
        : this(InMemoryConfigRoot, InMemoryPath, toAdd) { }

    public ForwardingAppendersLookupConfig
        (IConfigurationRoot root, string path, params IMutableAppenderForwardingReferenceConfig[] toAdd) : base(root, path, toAdd) { }

    public ForwardingAppendersLookupConfig(IForwardingAppendersLookupConfig toClone, IConfigurationRoot root, string path) 
        : base(toClone, root, path) { }

    public ForwardingAppendersLookupConfig(IForwardingAppendersLookupConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    public override ForwardingAppendersLookupConfig Clone() => new(this);

    public override StyledTypeBuildResult ToString(IStyledTypeStringAppender sbc)
    {
        return
            sbc.StartKeyedCollectionType(nameof(ForwardingAppendersLookupConfig))
               .AddAll(AppendersByName)
               .Complete();
    }
}
