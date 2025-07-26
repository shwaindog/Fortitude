// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Types.Mutable.Strings;

namespace FortitudeCommon.Logging.Core.LoggerVisitors;

public class SourceOrCreateLoggerVisitor(string loggerFullName,  IFLogContext flogContext) : LoggerVisitor<SourceOrCreateLoggerVisitor>
{
    private readonly MutableString loggerNameScratch  = new();

    private static readonly char[] NamePartDelimiter = ['.'];

    public IFLogger? SourcedLogger { get; private set; }

    public override SourceOrCreateLoggerVisitor Accept(IMutableFLoggerRoot node)
    {
        foreach (var childLogger in node.ImmediateEmbodiedChildren)
        {
            if (WalkDownTree((IMutableFLoggerCommon)childLogger)) return this;
        }
        WalkDownTree(node);
        return this;
    }

    protected bool WalkDownTree(IFLoggerCommon ancestorLogger)
    {
        var ancestorFullName = ancestorLogger.FullName;
        if (!loggerFullName.StartsWith(ancestorFullName))
        {
            return false;
        }
        loggerNameScratch.Clear();
        loggerNameScratch.Append(loggerFullName).Replace(ancestorFullName, "");
        if (ancestorFullName.IsNotNullOrEmpty())
        {
            loggerNameScratch.Remove(0);
        }
        var firstNamePart = loggerNameScratch.SplitFirstAsString(NamePartDelimiter);
        var isPathPart    = firstNamePart.Length < loggerNameScratch.Length;
    
        var foundChild = ancestorLogger.ImmediateEmbodiedChildren.FirstOrDefault(fld => fld.Name == firstNamePart);
        
        if (foundChild == null)
        {
            var subLoggerName  = loggerNameScratch.Clear().Append(ancestorLogger.FullName).Append(".").Append(firstNamePart).ToString();
            var explicitConfig = flogContext.ConfigRegistry.FindLoggerConfigIfGiven(subLoggerName) ?? 
                                 FLogCreate.MakeClonedDescendantLoggerConfig(ancestorLogger.ResolvedConfig);
            var subLogger       = FLogCreate.MakeLogger(explicitConfig, ancestorLogger, flogContext.LoggerRegistry);
            ((IMutableFLoggerCommon)ancestorLogger).AddDirectChild(subLogger);
            if (isPathPart)
            {
                return WalkDownTree(subLogger);
            }
            SourcedLogger = subLogger;
            return true;
        }
        if (isPathPart)
        {
            WalkDownTree(foundChild);
        }
        else
        {
            SourcedLogger = foundChild;
            return true;
        }
        return false;
    }
           
    public override SourceOrCreateLoggerVisitor Accept(IMutableFLoggerDescendant node) => this;
}
