// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Types.Mutable;

public interface IShowsEmpty
{
    bool IsEmpty { get; }
}


public interface IEmptyable : IShowsEmpty
{
    new bool IsEmpty { get; set; }
}
