// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Config;
using FortitudeCommon.Logging.Config.Visitor;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config;

public interface IFLogConfig
{
    string ConfigSubPath { get; }

    IFLogConfig? ParentConfig { get; }
    T Visit<T>(T visitor) where T : IFLogConfigVisitor<T>;
}

public interface IMutableFLogConfig : IFLogConfig
{
    new IFLogConfig? ParentConfig { get; set; }
}

public abstract class FLogConfig : ConfigSection, IMutableFLogConfig
{
    protected FLogConfig() { }

    protected FLogConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public IFLogConfig? ParentConfig { get; set; }

    public string ConfigSubPath => Path;

    public abstract T Visit<T>(T visitor) where T : IFLogConfigVisitor<T>;
}
