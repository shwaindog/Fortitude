#region

using System.Text;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeCommon.Types.Mutable.Strings;

public interface IMutableString : IReusableObject<IMutableString>, IStringBuilder, ITransferState<IFrozenString>, 
    IFrozenString, IFreezable<IFrozenString>
{
    new IMutableString Clone();

    StringBuilder BackingStringBuilder { get; }
}
