using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Types.Mutable;

namespace FortitudeCommon.Logging.Core.Appending.Formatting;

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
    static readonly string[] TokenReplaceDelimiters = ["{", "}"];

    private readonly List<ITemplatePart> templateParts = new();

    private string formattingTemplate = "";

    public static FLogEntryFormatter NewDefaultInstance => new (IAppenderDefinitionConfig.DefaultStringFormattingTemplate);

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
            FormattingTemplate = IAppenderDefinitionConfig.DefaultStringFormattingTemplate;
        }
        var mutableString = Recycler?.Borrow<MutableString>().Clear() ?? new MutableString();

        var sb = mutableString.BackingStringBuilder;

        for (var i = 0; i < templateParts.Count; i++)
        {
            var part = templateParts[i];
            part.Apply(sb, logEntry);
        }
        logEntry.DecrementRefCount();
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
        var formattingSpan = formattingTemplate.AsSpan();
        var stringParts    = formattingSpan.TokenSplit(IFLogEntryFormatter.TokenDelimiters, TokenReplaceDelimiters);
        foreach (var part in stringParts)
        {
            if (part[0] == '{' && part[^1] == '}')
            {
                templateParts.Add(new TokenKeyTemplatePart(part));
            }
            else
            {
                templateParts.Add(new StringConstantTemplatePart(part));
            }
        }
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
