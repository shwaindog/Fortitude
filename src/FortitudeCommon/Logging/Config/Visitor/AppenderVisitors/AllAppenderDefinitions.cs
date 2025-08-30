// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config.Appending;

namespace FortitudeCommon.Logging.Config.Visitor.AppenderVisitors;

public class AllAppenderDefinitions : VisitAllAppenderDefinitionsCollectOnCriteria<AllAppenderDefinitions, IMutableAppenderDefinitionConfig> { }
