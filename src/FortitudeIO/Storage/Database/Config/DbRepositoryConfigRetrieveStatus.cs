// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeIO.Storage.Database.Config;

[Flags]
public enum DbRepositoryConfigRetrieveStatus : ushort
{
    NeverAttempted            = 0x00_00
  , InvalidAdapterType        = 0x00_01
  , InsufficientParameters    = 0x00_02
  , InvalidDbQuery            = 0x00_04
  , InvalidEntityQuery        = 0x00_08
  , DbUnavailable             = 0x00_10
  , DbPermissionsError        = 0x00_20
  , NoResultsReturned         = 0x00_40
  , MissingDbDefinition       = 0x00_80
  , UsedEmbeddedConfigResults = 0x01_00
  , UsedMustIncludedResults   = 0x02_00
  , MergedIncludeResults      = 0x04_00
  , DbRetrieveSucceeded       = 0x08_00
  , HasConfigResults          = 0x10_00
}

public static class DbRepositoryConfigRetrieveStatusExtensions
{
    public static bool IsNeverAttempted(this DbRepositoryConfigRetrieveStatus flags) => flags == DbRepositoryConfigRetrieveStatus.NeverAttempted;

    public static bool HasInvalidAdapterTypeFlag(this DbRepositoryConfigRetrieveStatus flags) =>
        (flags & DbRepositoryConfigRetrieveStatus.InvalidAdapterType) > 0;

    public static bool HasInsufficientParametersFlag(this DbRepositoryConfigRetrieveStatus flags) =>
        (flags & DbRepositoryConfigRetrieveStatus.InsufficientParameters) > 0;

    public static bool HasInvalidDbQueryFlag(this DbRepositoryConfigRetrieveStatus flags) =>
        (flags & DbRepositoryConfigRetrieveStatus.InvalidDbQuery) > 0;

    public static bool HasInvalidEntityQueryFlag(this DbRepositoryConfigRetrieveStatus flags) =>
        (flags & DbRepositoryConfigRetrieveStatus.InvalidEntityQuery) > 0;

    public static bool HasDbUnavailableFlag(this DbRepositoryConfigRetrieveStatus flags) =>
        (flags & DbRepositoryConfigRetrieveStatus.DbUnavailable) > 0;

    public static bool HasDbPermissionsErrorFlag(this DbRepositoryConfigRetrieveStatus flags) =>
        (flags & DbRepositoryConfigRetrieveStatus.DbPermissionsError) > 0;

    public static bool HasNoResultsReturnedFlag(this DbRepositoryConfigRetrieveStatus flags) =>
        (flags & DbRepositoryConfigRetrieveStatus.NoResultsReturned) > 0;

    public static bool HasMissingDbDefinitionFlag(this DbRepositoryConfigRetrieveStatus flags) =>
        (flags & DbRepositoryConfigRetrieveStatus.MissingDbDefinition) > 0;

    public static bool HasUsedEmbeddedConfigResultsFlag(this DbRepositoryConfigRetrieveStatus flags) =>
        (flags & DbRepositoryConfigRetrieveStatus.UsedEmbeddedConfigResults) > 0;

    public static bool HasUsedMustIncludedResultsFlag(this DbRepositoryConfigRetrieveStatus flags) =>
        (flags & DbRepositoryConfigRetrieveStatus.UsedMustIncludedResults) > 0;

    public static bool HasMergedIncludeResultsFlag(this DbRepositoryConfigRetrieveStatus flags) =>
        (flags & DbRepositoryConfigRetrieveStatus.MergedIncludeResults) > 0;

    public static bool HasDbRetrieveSucceededFlag(this DbRepositoryConfigRetrieveStatus flags) =>
        (flags & DbRepositoryConfigRetrieveStatus.DbRetrieveSucceeded) > 0;

    public static bool HasHasConfigResultsFlag(this DbRepositoryConfigRetrieveStatus flags) =>
        (flags & DbRepositoryConfigRetrieveStatus.HasConfigResults) > 0;

    public static DbRepositoryConfigRetrieveStatus Unset(this DbRepositoryConfigRetrieveStatus flags, DbRepositoryConfigRetrieveStatus toUnset) =>
        flags & ~toUnset;

    public static bool HasAllOf(this DbRepositoryConfigRetrieveStatus flags, DbRepositoryConfigRetrieveStatus checkAllFound) =>
        (flags & checkAllFound) == checkAllFound;

    public static bool HasNoneOf(this DbRepositoryConfigRetrieveStatus flags, DbRepositoryConfigRetrieveStatus checkNonAreSet) =>
        (flags & checkNonAreSet) == 0;

    public static bool HasAnyOf(this DbRepositoryConfigRetrieveStatus flags, DbRepositoryConfigRetrieveStatus checkAnyAreFound) =>
        (flags & checkAnyAreFound) > 0;

    public static bool IsExactly(this DbRepositoryConfigRetrieveStatus flags, DbRepositoryConfigRetrieveStatus checkAllFound) =>
        flags == checkAllFound;
}
