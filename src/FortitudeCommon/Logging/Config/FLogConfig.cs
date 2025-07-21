using FortitudeCommon.Config;
using FortitudeCommon.Logging.Config.Visitor;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config;

public interface IFLogConfig 
{
    T Visit<T>(T visitor) where T : IFLogConfigVisitor<T>;

    IFLogConfig? ParentConfig { get; } 
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

    public abstract T Visit<T>(T visitor) where T : IFLogConfigVisitor<T>;
}
