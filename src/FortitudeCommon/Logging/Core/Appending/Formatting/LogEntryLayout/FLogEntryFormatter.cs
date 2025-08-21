using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Logging.Config.Appending.Formatting;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Types.Mutable;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;

public interface IFLogEntryFormatter : IReusableObject<IFLogEntryFormatter>
{
    static readonly (string Open, string Close) TokenDelimiters = ("'%", "%'");

    string FormattingTemplate { get; }

    int ApplyFormatting(IFLogEntry logEntry);
}

public interface IMutableFLogEntryFormatter : IFLogEntryFormatter
{
    new string FormattingTemplate { get; set; }
}

public interface ITokenFormattingValidator
{
    ReadOnlySpan<char> ValidateFormattingToken(string tokenName, ReadOnlySpan<char> tokenValue);
}

public class FLogEntryFormatter : ReusableObject<IFLogEntryFormatter>, IMutableFLogEntryFormatter
{
    private readonly List<ITemplatePart> templateParts = new();

    private IFormatWriterResolver formatWriterResolver = null!;

    private string formattingTemplate = "";

    private ITokenFormattingValidator? tokenFormattingValidator;

    public FLogEntryFormatter() { }

    public FLogEntryFormatter(string template, IFormatWriterResolver writerResolver)
    {
        formatWriterResolver = writerResolver;
        FormattingTemplate   = template;
    }

    public FLogEntryFormatter(FLogEntryFormatter toClone)
    {
        formatWriterResolver = toClone.formatWriterResolver;
        FormattingTemplate   = toClone.FormattingTemplate;
    }

    public FLogEntryFormatter Initialize(string template, IFormatWriterResolver writerResolver
      , ITokenFormattingValidator? tokenFormattingValidator = null)
    {
        if (template != FormattingTemplate)
        {
            FormattingTemplate = template;
        }
        formatWriterResolver = writerResolver;

        return this;
    }

    public List<ITemplatePart> TemplateParts => templateParts;

    public int ApplyFormatting(IFLogEntry logEntry)
    {
        if (templateParts.Count == 0)
        {
            FormattingTemplate = IFormattingAppenderConfig.DefaultStringFormattingTemplate;
        }
        
        var usedFormatWriter             = false;
        var formatWriterRequestTimeouts = 0;
        do
        {
            if (!formatWriterResolver.IsOpen) break;
            using var formatWriterWaitHandle = formatWriterResolver.FormatWriterResolver(logEntry);
            
            using var formatWriter = formatWriterWaitHandle.GetOrWaitForFormatWriter();

            if (formatWriter != null)
            {
                if (formatWriter.NotifyStartEntryAppend(logEntry))
                {
                    foreach (var part in templateParts)
                    {
                        part.Apply(formatWriter, logEntry);
                    }
                    usedFormatWriter = true;
                    formatWriter.NotifyEntryAppendComplete();
                }
            }
            else
            {
                formatWriterRequestTimeouts++;
            }
        } while (!usedFormatWriter);
        return formatWriterRequestTimeouts;
    }

    public string FormattingTemplate
    {
        get => formattingTemplate;
        set
        {
            if (value == formattingTemplate) return;
            formattingTemplate = value;
            BuildTemplateParts();
        }
    }

    protected virtual void BuildTemplateParts()
    {
        templateParts.Clear();

        TokenisedLogEntryFormatStringParser.Instance.BuildTemplateParts(formattingTemplate, templateParts, tokenFormattingValidator);
        TokenisedLogEntryFormatStringParser.Instance.EnsureConsoleColorsReset(templateParts);
    }

    public override FLogEntryFormatter Clone() => new(this);

    public override FLogEntryFormatter CopyFrom
        (IFLogEntryFormatter source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        FormattingTemplate = source.FormattingTemplate;

        return this;
    }
}
