namespace FortitudeCommon.Types.StringsOfPower.Options;

public enum CollectionPrettyStyleFormat
{
    Default
    , OneElementOnEveryLine
    , AllElementsOnOneLine
    , CollectionWidthWrap
    , FixedNumberPerLine  
}

public static class CollectionPrettyStyleFormatExtensions
{
    public static bool IsCollectionContentWidthWrap(this CollectionPrettyStyleFormat dateTimeStyle) =>
        dateTimeStyle is CollectionPrettyStyleFormat.CollectionWidthWrap;
    
    public static bool IsOneElementOnEveryLine(this CollectionPrettyStyleFormat dateTimeStyle) =>
        dateTimeStyle is CollectionPrettyStyleFormat.OneElementOnEveryLine;
    
    public static bool IsAllElementsOnOneLine(this CollectionPrettyStyleFormat dateTimeStyle) =>
        dateTimeStyle is CollectionPrettyStyleFormat.AllElementsOnOneLine;
    
    public static bool IsFixedNumberPerLine(this CollectionPrettyStyleFormat dateTimeStyle) =>
        dateTimeStyle is CollectionPrettyStyleFormat.FixedNumberPerLine;
}