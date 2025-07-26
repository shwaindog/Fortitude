#region

using System.Text;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeCommon.Types.Mutable.Strings;

public interface IStringBuilder<out T> : ICharSequence, IMutableStringBuilder<T>, ICloneable<IStringBuilder<T>>
where T : IStringBuilder<T>, IMutableStringBuilder<T>
{
    new int Length { get; set; }
    new char this[int index] { get; set; }

    T Substring(int startIndex);
    T Substring(int startIndex, int length);
    T Trim();
    T ToLower();
    T Remove(int startIndex);
    T ToUpper();

    T CopyFrom(string source);

    new T Clone();
}


public interface IMutableString : IReusableObject<IMutableString>, IStringBuilder<IMutableString>, ITransferState<IFrozenString>, 
    IFrozenString, IFreezable<IFrozenString>
{
    new IMutableString Clone();

    StringBuilder BackingStringBuilder { get; }
}
