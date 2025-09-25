namespace FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;

public interface IFormattingOptions
{
    public const    string DefaultNullString  = "null";
    
    protected const string DefaultTrueString  = "true";
    protected const string DefaultFalseString = "false";
    protected const string DefaultItemSeparator = ", ";
    
    
    string ItemSeparator { get; set; }
    
    bool SkipNullableNullWritesNull { get; set; }
    
    bool EmptyCollectionWritesNull { get; set; }
    
    bool IgnoreEmptyCollection { get; set; }
    
    string NullString { get; set; }
    
    string True { get; set; }
    
    string False { get; set; }
    
    IEncodingTransfer EncodingTransfer { get; set; }
    
    ICustomStringFormatter Formatter { get; set; }
}

public class FormattingOptions : IFormattingOptions
{
    protected IEncodingTransfer?      CurrentEncodingTransfer;
    protected   ICustomStringFormatter? Stringformatter;

    protected const char TokenClose = '}';
    protected const char TokenOpen  = '{';
    
    public string ItemSeparator { get; set; } = IFormattingOptions.DefaultItemSeparator;

    public bool SkipNullableNullWritesNull { get; set; } = false;

    public bool EmptyCollectionWritesNull { get; set; } = false;

    public bool IgnoreEmptyCollection { get; set; } = false;

    public string NullString { get; set; } = IFormattingOptions.DefaultNullString;

    public string True { get; set; } = IFormattingOptions.DefaultTrueString;
    public string False { get; set; } = IFormattingOptions.DefaultFalseString;

    public virtual IEncodingTransfer EncodingTransfer
    {
        get => CurrentEncodingTransfer ??= new PassThroughEncodingTransfer();
        set => CurrentEncodingTransfer = value;
    }

    public virtual ICustomStringFormatter Formatter
    {
        get => Stringformatter ??= new DefaultStringFormatter();
        set => Stringformatter = value;
    }
}
