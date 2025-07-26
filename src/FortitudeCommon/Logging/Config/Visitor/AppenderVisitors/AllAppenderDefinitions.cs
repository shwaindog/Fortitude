using FortitudeCommon.Logging.Config.Appending;

namespace FortitudeCommon.Logging.Config.Visitor.AppenderVisitors;

public class AllAppenderDefinitions : VisitAllAppenderDefinitionsCollectOnCriteria<AllAppenderDefinitions, IMutableAppenderDefinitionConfig>
{
}