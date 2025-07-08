// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Maps;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering;

public interface IMatchOperatorCollectionConfig : IStickyKeyValueDictionary<uint, IMatchOperatorExpressionConfig> { }

public interface IMatchOperatorExpressionConfig
{
    uint EvaluateOrder { get; }

    IMatchOperatorCollectionConfig? Any { get; }

    IMatchOperatorCollectionConfig? All { get; }

    IMatchOperatorExpressionConfig? And { get; }

    IMatchOperatorExpressionConfig? Or { get; }

    IMatchConditionConfig? IsTrue { get; }

    IMatchConditionConfig? IsFalse { get; }
}
