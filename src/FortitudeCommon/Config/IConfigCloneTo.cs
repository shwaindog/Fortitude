// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Config;

public interface IConfigCloneTo<T> : ICloneable<T>
{
    T CloneConfigTo(IConfigurationRoot configRoot, string path);
}
