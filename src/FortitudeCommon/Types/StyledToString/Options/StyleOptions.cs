// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Types.StyledToString;


public struct StyleOptionsValue(StringBuildingStyle initial)
{
    private StyleOptions? fallbackOptions; 
        
    private StringBuildingStyle? style = initial;

    public StringBuildingStyle Style
    {
        readonly get => style ?? fallbackOptions?.Values.Style ?? StringBuildingStyle.Default;
        set => style = value;
    }

    public StyleOptions DefaultOptions
    {
        get => fallbackOptions;
        set => fallbackOptions =  value;
    }
}



public class StyleOptions(StyleOptionsValue initialValues)
{
    public StyleOptionsValue Values { get; set; } = initialValues;
}
