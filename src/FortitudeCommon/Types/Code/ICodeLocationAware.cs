// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Types.Code;

public interface ICodeLocationAware
{
    Type? ContainingType { get; }
    
    string? ContainingTypeNameFullName { get; }
    
    string? MemberName { get; }
    
    Uri? CodePath { get; }
}
