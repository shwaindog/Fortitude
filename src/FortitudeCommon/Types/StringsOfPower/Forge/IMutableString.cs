#region

using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeCommon.Types.StringsOfPower.Forge;

public interface IMutableString : IReusableObject<IMutableString>, IStringBuilder, ITransferState<IFrozenString>, 
    IFrozenString, IFreezable<IFrozenString>
{
    new IMutableString Clone();

    StringBuilder BackingStringBuilder { get; }
}
