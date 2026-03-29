// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Options;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;

public static class MapCollectionExtensions
{
    public static bool ShouldDisplayKeyedCollectionTypeName(this StyleOptions styleOptions, Type maybeCollectionType)
    {
        var shouldShowTypeName = false;
        if (maybeCollectionType.IsKeyedCollection())
        {
            var collectionFullName  = maybeCollectionType.FullName ?? "";
            shouldShowTypeName = !styleOptions.LogSuppressDisplayCollectionNames.Any(s => collectionFullName.StartsWith(s));
            if (!shouldShowTypeName)
            {
                var kvpTypes = maybeCollectionType.GetKeyedCollectionTypes()!.Value;
                
                var keyType    = kvpTypes.Key;
                var valueType  = kvpTypes.Value;
                
                var keyFullName = keyType.FullName ?? "";
                var valueFullName = valueType.FullName ?? "";

                var showKey = !styleOptions.LogSuppressDisplayCollectionElementNames.Any(s => keyFullName.StartsWith(s));
                var showValue = !styleOptions.LogSuppressDisplayCollectionElementNames.Any(s => valueFullName.StartsWith(s));

                shouldShowTypeName = showKey | showValue;
            }
        }
        return shouldShowTypeName;
    }
}
