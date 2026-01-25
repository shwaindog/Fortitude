// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using static FortitudeCommon.Types.StringsOfPower.WriteMethodType;

namespace FortitudeCommon.Types.StringsOfPower;

public enum WriteMethodType
{
    None
  , RawContent
  , MoldSimpleContentType
  , MoldComplexContentType
  , MoldComplexType
  , MoldSimpleCollectionType
  , MoldComplexCollectionType
  , MoldExplicitCollectionType
  , MoldKeyedCollectionType
  , MoldExplicitKeyedCollectionType
}


public static class WriteMethodTypeExtensions
{
    public static bool IsRawContent(this WriteMethodType value)                      => value == RawContent; 
    public static bool IsdSimpleContentType(this WriteMethodType value)              => value == MoldSimpleContentType; 
    public static bool IsComplexContentType(this WriteMethodType value)              => value == MoldComplexContentType; 
    public static bool IsComplexType(this WriteMethodType value)                     => value == MoldComplexType; 
    public static bool IsSimpleCollectionType(this WriteMethodType value)        => value == MoldSimpleCollectionType; 
    public static bool IsComplexCollectionType(this WriteMethodType value)       => value == MoldComplexCollectionType; 
    public static bool IsExplicitCollectionType(this WriteMethodType value)      => value == MoldExplicitCollectionType; 
    public static bool IsKeyedCollectionType(this WriteMethodType value)         => value == MoldKeyedCollectionType; 
    public static bool IsExplicitKeyedCollectionType(this WriteMethodType value) => value == MoldExplicitKeyedCollectionType;


    public static bool SupportsMultipleFields(this WriteMethodType value) =>
        value switch
        {
            MoldComplexContentType          => true
          , MoldComplexType                 => true
          , MoldComplexCollectionType       => true
          , MoldKeyedCollectionType         => true
          , MoldExplicitKeyedCollectionType => true
          , _                               => false
        };
    
    public static WriteMethodType ToMultiFieldEquivalent(this WriteMethodType value) =>
        value switch
        {
            RawContent                      => MoldComplexType
          , MoldSimpleContentType           => MoldComplexContentType
          , MoldSimpleCollectionType        => MoldComplexCollectionType
          , MoldExplicitCollectionType      => MoldComplexCollectionType
          , _                               => value
        };
    
    public static WriteMethodType ToNoFieldEquivalent(this WriteMethodType value) =>
        value switch
        {
            RawContent                 => MoldComplexType
          , MoldComplexContentType     => MoldSimpleContentType
          , MoldComplexCollectionType  => MoldSimpleContentType
          , _                          => value
        };
    
    public static bool NoFieldNames(this WriteMethodType value) =>
        value switch
        {
            RawContent               => true
          , MoldSimpleContentType    => true
          , MoldSimpleCollectionType => true
          , _                        => false
        };
}
