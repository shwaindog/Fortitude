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

public class FLogEntryFormatter : ReusableObject<IFLogEntryFormatter>, IMutableFLogEntryFormatter
{
    private readonly IFLogFormattingAppender formattingAppender;
    private readonly List<ITemplatePart>     templateParts = new();

    private string formattingTemplate = "";

    public FLogEntryFormatter(string formattingTemplate, IFLogFormattingAppender formattingAppender)
    {
        this.formattingAppender = formattingAppender;
        FormattingTemplate      = formattingTemplate;
    }

    public FLogEntryFormatter(FLogEntryFormatter toClone)
    {
        formattingAppender = toClone.formattingAppender;
        FormattingTemplate = toClone.FormattingTemplate;
    }

    public int ApplyFormatting(IFLogEntry logEntry)
    {
        if (templateParts.Count == 0)
        {
            FormattingTemplate = IFormattingAppenderConfig.DefaultStringFormattingTemplate;
        }
        using var formatWriterResolver = formattingAppender.FormatWriterResolver;

        var  formatWriterRequestTimeouts = 0;
        var gotFormatWriter = false;
        do
        {
            using var formatWriter = formatWriterResolver.GetOrWaitForFormatWriter();

            if (formatWriter != null)
            {
                gotFormatWriter = true;
                foreach (var part in templateParts)
                {
                    part.Apply(formatWriter, logEntry);
                }
            }
            else
            {
                formatWriterRequestTimeouts++;
            }
        } while (!gotFormatWriter);
        return formatWriterRequestTimeouts;
    }

    public string FormattingTemplate
    {
        get => formattingTemplate;
        set
        {
            if (value == formattingTemplate) return;
            formattingTemplate = value;
            templateParts.Clear();
            BuildTemplateParts();
        }
    }

    protected virtual void BuildTemplateParts()
    {
        templateParts.Clear();

        TokenisedLogEntryFormatStringParser.Instance.BuildTemplateParts(formattingTemplate, templateParts);
        TokenisedLogEntryFormatStringParser.Instance.EnsureConsoleColorsReset(templateParts);
    }

    public override FLogEntryFormatter Clone() => new (this);

    public override FLogEntryFormatter CopyFrom
        (IFLogEntryFormatter source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        FormattingTemplate = source.FormattingTemplate;

        return this;
    }
}
