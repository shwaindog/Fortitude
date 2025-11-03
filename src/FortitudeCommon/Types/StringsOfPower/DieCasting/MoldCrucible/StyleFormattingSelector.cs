// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;
using FortitudeCommon.Types.StringsOfPower.Options;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public static class StyleFormattingSelector
{
    public static Func<IRecycler, StyleOptions, EncodingType, IStyledTypeFormatting> StyleFormattingResolverSelector { get; set; }
        = DefaultResolveStyleFormatter;
    
    public static IStyledTypeFormatting ResolveStyleFormatter(this IRecycler recycler, StyleOptions previousOptions, 
        EncodingType                    encodingType = EncodingType.DefaultForStyle)
    {
        return StyleFormattingResolverSelector(recycler, previousOptions, encodingType);
    }

    private static IStyledTypeFormatting DefaultResolveStyleFormatter(this IRecycler recycler, StyleOptions styleOptions
      , EncodingType encodingType = EncodingType.DefaultForStyle)
    {
        
        return styleOptions.Style switch
               {
                   StringStyle.Json | StringStyle.Compact =>
                       recycler.Borrow<CompactJsonTypeFormatting>()
                               .Initialize(styleOptions)
                 , StringStyle.Json | StringStyle.Pretty =>
                       recycler.Borrow<PrettyJsonTypeFormatting>()
                               .Initialize(styleOptions)
                 , StringStyle.Log | StringStyle.Pretty =>
                       recycler.Borrow<PrettyLogTypeFormatting>()
                               .Initialize(styleOptions)
                 , _ => Recycler.ThreadStaticRecycler.Borrow<CompactLogTypeFormatting>()
                                .Initialize(styleOptions)
               };
    }
}
