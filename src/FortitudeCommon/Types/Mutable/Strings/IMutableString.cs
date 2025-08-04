#region

using System.Text;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeCommon.Types.Mutable.Strings;

public interface IStringBuilder : ICharSequence, IMutableStringBuilder<IStringBuilder>, ICloneable<IStringBuilder>
{
    new int Length { get; set; }
    new char this[int index] { get; set; }

    IStringBuilder Substring(int startIndex);
    IStringBuilder Substring(int startIndex, int length);
    IStringBuilder Trim();
    IStringBuilder ToLower();
    IStringBuilder Remove(int startIndex);
    IStringBuilder ToUpper();

    IStringBuilder CopyFrom(string source);

    new IStringBuilder Clone();
}



public interface IMutableString : IReusableObject<IMutableString>, IStringBuilder, ITransferState<IFrozenString>, 
    IFrozenString, IFreezable<IFrozenString>
{
    new IMutableString Clone();

    StringBuilder BackingStringBuilder { get; }
}
