using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeCommon.Config;

public interface IExternallySourcedConfig<out TConfig>
    where TConfig : class
{
    TConfig? RetrievedExternalConfig { get; }

    bool ResolvedExternalConfig();
}

public interface IExternallySourcedWithGuaranteedConfig<TConfig> : IExternallySourcedConfig<TConfig>
    where TConfig : class
{
    bool DisableIncludeGuaranteedResults { get; set; }

    TConfig? GuaranteedResults { get; set; }

    TConfig? ResolvedConfig { get; }
}