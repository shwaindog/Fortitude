using System.Linq.Expressions;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;

namespace FortitudeCommon.DataStructures.Collections;


public interface ISortOrderComparer<T> : IComparer<T>
{
    string PropertyName { get; }

    string SortDescription { get; }

    ISortOrderComparer<T>? AndThenBy { get; set; }
}

public class SortOrderComparer<T, TProp>(PropertyExtractor<T, TProp> propertyExtractor, IComparer<TProp> propertyComparer, bool ascending = true)
    : ISortOrderComparer<T>
{
    public SortOrderComparer(Expression<Func<T?, TProp>> propertyGetExpression, IComparer<TProp> propertyComparer, bool ascending = true) 
        : this(new PropertyExtractor<T, TProp>(propertyGetExpression), propertyComparer, ascending) { }

    public ISortOrderComparer<T>? AndThenBy { get; set; }

    public int Compare(T? lhs, T? rhs)
    {
        var compare = ascending 
            ? propertyComparer.Compare(propertyExtractor.GetPropertyValue(lhs), propertyExtractor.GetPropertyValue(rhs)) 
            : propertyComparer.Compare(propertyExtractor.GetPropertyValue(rhs), propertyExtractor.GetPropertyValue(lhs));
        if (compare == 0 && AndThenBy != null)
        {
            return AndThenBy.Compare(lhs, rhs);
        }
        return compare;
    }

    public string PropertyName    => propertyExtractor.PropertyName;

    public string SortDescription
    {
        get
        {
            var sb = new StringBuilder();
            sb.Append(PropertyName);
            var nextSortOrderComparer = AndThenBy;
            while (nextSortOrderComparer != null)
            {
                sb.Append(" then by ");
                sb.Append(nextSortOrderComparer.PropertyName);
                nextSortOrderComparer = nextSortOrderComparer.AndThenBy;
            }
            return sb.ToString();
        }
    }
}

public class SortDateTimeOrderComparer<T>(PropertyExtractor<T, DateTime> propertyExtractor, IComparer<DateTime> propertyComparer, bool ascending = true)
    : SortOrderComparer<T, DateTime>(propertyExtractor, propertyComparer, ascending)
{
    public SortDateTimeOrderComparer(Expression<Func<T?, DateTime>> propertyGetExpression, IComparer<DateTime> propertyComparer, bool ascending = true) 
        : this(new PropertyExtractor<T, DateTime>(propertyGetExpression), propertyComparer, ascending) { }

    public SortDateTimeOrderComparer(Expression<Func<T?, DateTime>> propertyGetExpression, bool ascending = true) 
        : this(new PropertyExtractor<T, DateTime>(propertyGetExpression), ascending) { }

    public SortDateTimeOrderComparer(PropertyExtractor<T, DateTime> propertyExtractor, bool ascending = true) 
        : this(propertyExtractor, DateTimeExtensions.UtcComparer, ascending) { }
}

public class SortUIntOrderComparer<T>(PropertyExtractor<T, uint> propertyExtractor, bool ascending = true)
    : SortOrderComparer<T, uint>(propertyExtractor, NumberExtensions.UIntComparer, ascending)
{
    public SortUIntOrderComparer(Expression<Func<T?, uint>> propertyGetExpression, bool ascending = true) 
        : this(new PropertyExtractor<T, uint>(propertyGetExpression), ascending) { }
}

public class SortIntOrderComparer<T>(PropertyExtractor<T, int> propertyExtractor, bool ascending = true)
    : SortOrderComparer<T, int>(propertyExtractor, NumberExtensions.IntComparer, ascending)
{
    public SortIntOrderComparer(Expression<Func<T?, int>> propertyGetExpression, bool ascending = true) 
        : this(new PropertyExtractor<T, int>(propertyGetExpression), ascending) { }
}