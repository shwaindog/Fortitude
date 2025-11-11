// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.DataStructures.Lists.PositionAware;

public interface IOffsetAwareListItem
{
    int AtIndex { get; set; }
}

public interface ICodeLocationAwareListItem : IOffsetAwareListItem
{
    Type? ListOwningType { get; set; }
    
    string? ListMemberName { get; set; }
    
    string? ItemCodePath { get; }
}
