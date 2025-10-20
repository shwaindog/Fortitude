namespace FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;

public interface IFormattingOptions
{
    public const string DefaultNullString = "null";

    protected const string DefaultTrueString    = "true";
    protected const string DefaultFalseString   = "false";
    protected const string DefaultItemSeparator = ", ";


    string ItemSeparator { get; set; }

    bool NullWritesNothing { get; set; }

    bool NullWritesNullString { get; set; }

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
    protected ICustomStringFormatter? Stringformatter;

    private bool writeNullString = true;

    protected const char TokenClose = '}';
    protected const char TokenOpen  = '{';

    public FormattingOptions()
    {
        
    }
    
    public FormattingOptions(IFormattingOptions toClone)
    {
        ItemSeparator             = toClone.ItemSeparator;
        NullWritesNullString      = toClone.NullWritesNullString;
        EmptyCollectionWritesNull = toClone.EmptyCollectionWritesNull;
        IgnoreEmptyCollection     = toClone.IgnoreEmptyCollection;
        NullString                = toClone.NullString;
        True                      = toClone.True;
        False                     = toClone.False;
        EncodingTransfer          = toClone.EncodingTransfer;
        Formatter                 = toClone.Formatter;
    }

    public string ItemSeparator { get; set; } = IFormattingOptions.DefaultItemSeparator;

    public bool NullWritesNothing
    {
        get => !writeNullString;
        set => writeNullString = !value;
    }

    public bool NullWritesNullString
    {
        get => writeNullString;
        set => writeNullString = value;
    }

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
