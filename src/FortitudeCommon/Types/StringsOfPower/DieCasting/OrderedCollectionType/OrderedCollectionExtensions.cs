// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;
using FortitudeCommon.Types.StringsOfPower.Options;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;

public static class OrderedCollectionExtensions
{
    public static bool ShouldDisplayCollectionTypeName(this StyleOptions styleOptions, Type maybeCollectionType)
    {
        var shouldShowTypeName = false;
        if (maybeCollectionType.IsKeyedCollection())
        {
            return styleOptions.ShouldDisplayKeyedCollectionTypeName(maybeCollectionType);
        }
        if (maybeCollectionType.IsIterable())
        {
            var collectionFullName  = maybeCollectionType.FullName ?? "";
            var elementType         = maybeCollectionType.GetIterableElementType()?.IfNullableGetUnderlyingTypeOrThis() ?? maybeCollectionType;
            var elementTypeFullName = elementType.FullName ?? "";

            shouldShowTypeName =
                !(styleOptions.LogSuppressDisplayCollectionNames.Any(s => collectionFullName.StartsWith(s))
               && styleOptions.LogSuppressDisplayCollectionElementNames.Any(s => elementTypeFullName.StartsWith(s)));
        }
        return shouldShowTypeName;
    }
}
