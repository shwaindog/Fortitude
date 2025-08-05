using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Types.StyledToString;


[Obsolete("Generates temporary heap object. Consider implementing IStyledToStringObject for this type.")]
public class CallsObjectToString : System.Attribute { }

[Obsolete("Generates temporary heap object")]
public class CreatesParamsObjectArray : System.Attribute { }



public interface IStyledToStringObject
{
    StyledTypeBuildResult ToString(IStyledTypeStringAppender sbc);
}

public interface IOverridesStyledToStringObject : IStyledToStringObject
{
    StyledTypeBuildResult CallBaseStyledToString(IStyledTypeStringAppender sbc);
}

public interface IReceivesNotificationOfStyledToString : IStyledToStringObject
{
    void StartingStyledToString(IStyledTypeStringAppender sbc);

    void CompletedStyledToString(IStyledTypeStringAppender sbc);
}

public static class StyledToStringObjectExtensions
{
    public static string DefaultToString(this IStyledToStringObject makeString, IRecycler? recycler = null)
    {
        var styledStringBuilder = recycler?.Borrow<StyledTypeStringAppender>() ?? new StyledTypeStringAppender();
        styledStringBuilder.ClearAndReinitialize(StringBuildingStyle.Default);
        makeString.ToString(styledStringBuilder);
        return styledStringBuilder.ToString();
    }

    public static string JsonCompactString(this IStyledToStringObject makeString, IRecycler? recycler = null)
    {
        var styledStringBuilder = recycler?.Borrow<StyledTypeStringAppender>() ?? new StyledTypeStringAppender();
        styledStringBuilder.ClearAndReinitialize(StringBuildingStyle.Compact | StringBuildingStyle.Json);
        makeString.ToString(styledStringBuilder);
        return styledStringBuilder.ToString();
    }

    public static string JsonPrettyString(this IStyledToStringObject makeString, IRecycler? recycler = null)
    {
        var styledStringBuilder = recycler?.Borrow<StyledTypeStringAppender>() ?? new StyledTypeStringAppender();
        styledStringBuilder.ClearAndReinitialize(StringBuildingStyle.Pretty | StringBuildingStyle.Json);
        makeString.ToString(styledStringBuilder);
        return styledStringBuilder.ToString();
    }
}