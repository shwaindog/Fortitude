using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Logging.Config.Appending.Formatting;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.Mutable.Strings;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;

public interface IFLogEntryFormatter : IReusableObject<IFLogEntryFormatter>
{
    static readonly string[] TokenDelimiters = ["\"%", "%\""];

    string FormattingTemplate { get; }

    IMutableString ApplyFormatting(IFLogEntry logEntry);
}

public interface IMutableFLogEntryFormatter : IFLogEntryFormatter
{
    new string FormattingTemplate { get; set; }
}

public class FLogEntryFormatter : ReusableObject<IFLogEntryFormatter>, IMutableFLogEntryFormatter
{
    private readonly List<ITemplatePart> templateParts = new();

    private string formattingTemplate = "";

    public static FLogEntryFormatter NewDefaultInstance => new (IFormattingAppenderConfig.DefaultStringFormattingTemplate);

    public FLogEntryFormatter() { }

    public FLogEntryFormatter(string formattingTemplate)
    {
        FormattingTemplate = formattingTemplate;
    }

    public FLogEntryFormatter(FLogEntryFormatter toClone)
    {
        FormattingTemplate = toClone.FormattingTemplate;
    }

    public IMutableString ApplyFormatting(IFLogEntry logEntry)
    {
        if (templateParts.Count == 0)
        {
            FormattingTemplate = IFormattingAppenderConfig.DefaultStringFormattingTemplate;
        }
        var mutableString = Recycler?.Borrow<MutableString>().Clear() ?? new MutableString();

        // var sb = mutableString.BackingStringBuilder;
        //
        // fr (var i = 0; i < templateParts.Count; i++)
        // {
        //     var part = templateParts[i];
        //     part.Apply(sb, logEntry);
        // }
        // logEntry.DecrementRefCount();
        return mutableString;
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

    public override FLogEntryFormatter Clone() =>
        Recycler?.Borrow<FLogEntryFormatter>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new FLogEntryFormatter(this);

    public override FLogEntryFormatter CopyFrom
        (IFLogEntryFormatter source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        FormattingTemplate = source.FormattingTemplate;

        return this;
    }
}
