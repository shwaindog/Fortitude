// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeCommon.Types;

public enum EquivalenceCheckFlags
{
    AllSupportedValuesMatch      = 0
  , TypesMatch                   = 1
  , AllUpdatedFlagsMatch         = 2
  , ExactMatch                   = 3
  , RelevantUpdatedFlagsMatch    = 4
  , WhollyContainsAllValuesMatch = 8
}

public interface IInterfacesComparable<in T>
{
    bool AreEquivalent(T? other, bool exactTypes = false);
}
