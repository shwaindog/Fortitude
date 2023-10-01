#region

using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Globalization;
using System.Text;
using FortitudeCommon.Chronometry;

#endregion

namespace FortitudeCommon.Monitoring.Logging.Diagnostics.Tracing;

public class TraceLogger : ITraceLogger
{
    private const bool DefaultTraceEnabled = false;
    private const string ActivationKeyWordAlwaysTrace = "alwaystrace";
    public static readonly ITimeContext ConsistentTimer = new HighPrecisionTimeContext();

    private static readonly ConcurrentDictionary<string, TraceLogger> KnownInstances = new();

    private static readonly object StaticSyncCntx = new();
    private static Action<IHierarchicalLogger, string>? settingTranslation;
    private readonly IndentedTextWriter indentWriter;
    private readonly IFLogger logger;
    private readonly StringBuilder sb = new(5120);
    private readonly List<TraceEntry> traceLog = new(256);
    private int currentDepth;
    private bool hasBeenReset;
    private bool writeTrace;

    public TraceLogger(string name, Type callingClass) : this(name, callingClass.FullName!) { }

    public TraceLogger(string name, string prefixName)
        : this(prefixName + "." + name) { }

    private TraceLogger(string name, IFLogger? loggerOverride = null)
    {
        var baseTextWriter = new StringWriter(sb);
        indentWriter = new IndentedTextWriter(baseTextWriter, "\t");
        FullNameOfLogger = name;
        logger = loggerOverride ?? FLoggerFactory.Instance.GetLogger("Trace." + FullNameOfLogger);
    }

    public static IEnumerable<string> ActivationKeywords =>
        HierarchicalConfigurationUpdater.ActivationKeyWords.Concat(new[] { ActivationKeyWordAlwaysTrace });

    public string FullNameOfLogger { get; set; }
    public bool Enabled { get; set; }

    public bool WriteTrace
    {
        get => writeTrace && Enabled;
        set => writeTrace = value;
    }

    public bool DefaultWriteTrace { get; set; }

    public Action<IHierarchicalLogger, string> SettingTranslation
    {
        get
        {
            return settingTranslation ?? (settingTranslation = (hl, setting) =>
            {
                if (!(hl is ITraceLogger tl) || setting == null) return;
                tl.DefaultWriteTrace = false;
                tl.WriteTrace = false;
                if (HierarchicalConfigurationUpdater.IsActivationKeyWord(setting))
                {
                    tl.Enabled = true;
                    return;
                }

                if (setting.Contains(ActivationKeyWordAlwaysTrace))
                {
                    tl.Enabled = true;
                    tl.DefaultWriteTrace = true;
                    tl.WriteTrace = true;
                    return;
                }

                tl.Enabled = false;
            });
        }
    }

    public string DefaultStringValue => DefaultTraceEnabled.ToString(CultureInfo.InvariantCulture);

    public void Start()
    {
        currentDepth = 0;
        traceLog.Clear();
        hasBeenReset = true;
    }

    public void Add<Tu>(string identifier, Tu subject)
    {
        if (Enabled) traceLog.Add(new TraceEntry(ConsistentTimer.UtcNow, identifier, subject, currentDepth));
    }

    public void Add(string identifier)
    {
        if (Enabled) traceLog.Add(new TraceEntry(ConsistentTimer.UtcNow, identifier, null, currentDepth));
    }

    public void Indent()
    {
        if (Enabled) currentDepth++;
    }

    public void Dedent()
    {
        if (Enabled)
        {
            currentDepth--;
            if (currentDepth < 0) currentDepth = 0;
        }
    }

    public List<TraceEntry> TraceFinished()
    {
        if (WriteTrace && hasBeenReset)
        {
            logger.Debug("{0}", ToString());
            WriteTrace = DefaultWriteTrace;
            hasBeenReset = false;
        }

        return traceLog;
    }

    public static TraceLogger GetWellKnownTraceLogger(string name, IFLogger? loggerOverride = null)
    {
        if (KnownInstances.TryGetValue(name, out var getInstance)) return getInstance;
        lock (StaticSyncCntx)
        {
            if (KnownInstances.TryGetValue(name, out getInstance)) return getInstance;
            getInstance = new TraceLogger(name, loggerOverride);
            HierarchicalLoggingConfigurator<ITraceLogger>.Register(getInstance);
            KnownInstances.TryAdd(name, getInstance);
        }

        return getInstance;
    }

    public void PreAppend(ITraceLogger append, int depthAdjust = 0, string? subjectOverride = null)
    {
        if (Enabled)
            traceLog.InsertRange(0,
                append.TraceFinished()
                    .Select(
                        te =>
                            new TraceEntry(te.Time, te.Identifier, subjectOverride ?? te.Subject,
                                te.Depth + depthAdjust)));
    }

    public void PostAppend(ITraceLogger append, int depthAdjust = 0, string? subjectOverride = null)
    {
        if (Enabled)
            traceLog.AddRange(
                append.TraceFinished()
                    .Select(
                        te =>
                            new TraceEntry(te.Time, te.Identifier, subjectOverride ?? te.Subject,
                                te.Depth + depthAdjust)));
    }

    public override string ToString()
    {
        if (!Enabled) return "TraceLogger Not Enabled";
        if (traceLog.Count == 0) return "No Trace entries.";

        sb.Clear();
        indentWriter.WriteLine("TraceLogger {0} started", FullNameOfLogger);
        foreach (var traceEntry in traceLog)
        {
            indentWriter.Indent = traceEntry.Depth;
            indentWriter.WriteLine(traceEntry.ToStringNoBoxing());
        }

        indentWriter.Indent = 0;
        var result = sb.ToString();
        return result;
    }
}
