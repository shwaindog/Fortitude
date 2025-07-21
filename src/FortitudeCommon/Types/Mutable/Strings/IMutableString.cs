#region

using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeCommon.Types.Mutable;

public interface IMutableString : IReusableObject<IMutableString>, IMutableStringBuilder<IMutableString>, ITransferState<IFrozenString>, 
    IFrozenString, IFreezable<IFrozenString>
{
    new int Length { get; set; }
    new char this[int index] { get; set; }

    IMutableString Substring(int startIndex);
    IMutableString Substring(int startIndex, int length);
    IMutableString Trim();
    IMutableString ToLower();
    IMutableString Remove(int startIndex);
    IMutableString ToUpper();

    IMutableString CopyFrom(string source);

    new IMutableString Clone();
}
