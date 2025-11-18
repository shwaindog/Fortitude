// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;
using FortitudeCommon.Types.StringsOfPower.Options;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public static class StyleFormattingSelector
{
    public static Func<ISecretStringOfPower, EncodingType, IStyledTypeFormatting> StyleFormattingResolverSelector { get; set; }
        = DefaultResolveStyleFormatter;
    
    public static IStyledTypeFormatting ResolveStyleFormatter(this ISecretStringOfPower theStringMaster
      , EncodingType encodingType = EncodingType.DefaultForStyle )
    {
        return StyleFormattingResolverSelector(theStringMaster, encodingType );
    }

    private static IStyledTypeFormatting DefaultResolveStyleFormatter(ISecretStringOfPower theStringMaster
      , EncodingType encodingType = EncodingType.DefaultForStyle)
    {
        
        return theStringMaster.Settings.Style switch
               {
                   StringStyle.Json | StringStyle.Compact =>
                       theStringMaster.Recycler.Borrow<CompactJsonTypeFormatting>()
                               .Initialize(theStringMaster.GraphBuilder, theStringMaster.Settings)
                 , StringStyle.Json | StringStyle.Pretty =>
                       theStringMaster.Recycler.Borrow<PrettyJsonTypeFormatting>()
                                      .Initialize(theStringMaster.GraphBuilder, theStringMaster.Settings)
                 , StringStyle.Log | StringStyle.Pretty =>
                       theStringMaster.Recycler.Borrow<PrettyLogTypeFormatting>()
                                      .Initialize(theStringMaster.GraphBuilder, theStringMaster.Settings)
                 , _ => Recycler.ThreadStaticRecycler.Borrow<CompactLogTypeFormatting>()
                                .Initialize(theStringMaster.GraphBuilder, theStringMaster.Settings)
               };
    }
}
