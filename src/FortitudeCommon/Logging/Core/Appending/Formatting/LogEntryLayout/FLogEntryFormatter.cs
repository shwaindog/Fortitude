// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

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
    private string formattingTemplate = "";

    private IFormatWriterResolver formatWriterResolver = null!;

    private ITokenFormattingValidator? tokenFormattingValidator = null;

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

    public List<ITemplatePart> TemplateParts { get; } = new();

    public int ApplyFormatting(IFLogEntry logEntry)
    {
        if (TemplateParts.Count == 0) FormattingTemplate = IFormattingAppenderConfig.DefaultStringFormattingTemplate;

        var       usedFormatWriter            = false;
        var       formatWriterRequestTimeouts = 0;

        while (!usedFormatWriter && formatWriterRequestTimeouts < 15)
        {
            using var formatWriterWaitHandle      = formatWriterResolver.FormatWriterResolver(logEntry);
            do
            {
                if (!formatWriterResolver.IsOpen) break;

                using var formatWriter = formatWriterWaitHandle.GetOrWaitForFormatWriter();

                if (formatWriter != null)
                {
                    if (formatWriter.NotifyStartEntryAppend(logEntry))
                    {
                        foreach (var part in TemplateParts) part.Apply(formatWriter, logEntry);
                        usedFormatWriter = true;
                        formatWriter.NotifyEntryAppendComplete();
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    formatWriterRequestTimeouts++;
                }
            } while (!usedFormatWriter && formatWriterRequestTimeouts % 3 == 0);
        }
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

    public FLogEntryFormatter Initialize(string template, IFormatWriterResolver writerResolver
      , ITokenFormattingValidator? formattingValidator = null)
    {
        tokenFormattingValidator = formattingValidator;
        if (template != FormattingTemplate) FormattingTemplate = template;
        formatWriterResolver = writerResolver;

        return this;
    }

    protected virtual void BuildTemplateParts()
    {
        TemplateParts.Clear();

        var tokenValidator = tokenFormattingValidator ?? DefaultLogEntryFormatting.Instance;

        TokenisedLogEntryFormatStringParser.Instance.BuildTemplateParts(formattingTemplate, TemplateParts, tokenValidator);
        TokenisedLogEntryFormatStringParser.Instance.EnsureConsoleColorsReset(TemplateParts);
    }

    public override FLogEntryFormatter Clone() => new(this);

    public override FLogEntryFormatter CopyFrom
        (IFLogEntryFormatter source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        FormattingTemplate = source.FormattingTemplate;

        return this;
    }
}
