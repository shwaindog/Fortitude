using System.Text;
using FortitudeCommon.DataStructures.Memory;

namespace FortitudeCommon.Types;


public interface IStyledToStringObject
{
    void ToString(IStyledTypeStringAppender sbc);
}

public static class StyledToStringObjectExtensions
{
    public static string DefaultToString(this IStyledToStringObject makeString, IRecycler? recycler = null)
    {
        var styledStringBuilder = recycler?.Borrow<StyledTypeStringAppender>() ?? new StyledTypeStringAppender();
        styledStringBuilder.ClearSetStyle(StringBuildingStyle.Default);
        makeString.ToString(styledStringBuilder);
        return styledStringBuilder.ToString();
    }

    public static string JsonCompactString(this IStyledToStringObject makeString, IRecycler? recycler = null)
    {
        var styledStringBuilder = recycler?.Borrow<StyledTypeStringAppender>() ?? new StyledTypeStringAppender();
        styledStringBuilder.ClearSetStyle(StringBuildingStyle.JsonCompact);
        makeString.ToString(styledStringBuilder);
        return styledStringBuilder.ToString();
    }

    public static string JsonPrettyString(this IStyledToStringObject makeString, IRecycler? recycler = null)
    {
        var styledStringBuilder = recycler?.Borrow<StyledTypeStringAppender>() ?? new StyledTypeStringAppender();
        styledStringBuilder.ClearSetStyle(StringBuildingStyle.JsonPretty);
        makeString.ToString(styledStringBuilder);
        return styledStringBuilder.ToString();
    }
}