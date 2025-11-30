// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;
using FortitudeCommon.Types.StringsOfPower.Options;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public static class StyleFormattingSelector
{
    public static Func<ISecretStringOfPower, IStyledTypeFormatting> StyleFormattingResolverSelector { get; set; }
        = DefaultResolveStyleFormatter;
    
    public static IStyledTypeFormatting ResolveStyleFormatter(this ISecretStringOfPower theStringMaster)
    {
        return StyleFormattingResolverSelector(theStringMaster);
    }

    private static IStyledTypeFormatting DefaultResolveStyleFormatter(ISecretStringOfPower theStringMaster)
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
    
    public static Func<ISecretStringOfPower, EncodingType, IEncodingTransfer> StyleEncoderResolverSelector { get; set; }
        = DefaultResolveStyleEncoder;
    
    public static IEncodingTransfer ResolveStyleEncoder(this ISecretStringOfPower theStringMaster, EncodingType encodingType)
    {
        return StyleEncoderResolverSelector(theStringMaster, encodingType);
    }

    private static IEncodingTransfer DefaultResolveStyleEncoder(ISecretStringOfPower theStringMaster, EncodingType encodingType) =>
        encodingType switch
        {
            EncodingType.PassThrough =>
                PassThroughEncodingTransfer.Instance
          , EncodingType.JsonEncoding =>
                theStringMaster.Recycler.Borrow<JsonEscapingEncodingTransfer>()
                               .Initialize(theStringMaster.Settings, theStringMaster.Settings.CachedMappingFactoryRanges)
          , _ => PassThroughEncodingTransfer.Instance                      
        };
}
