#region

using System.Text;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeCommon.Types.StringsOfPower.Forge;

public interface IMutableString : IReusableObject<IMutableString>, IStringBuilder, ITransferState<IFrozenString>, 
    IFrozenString, IFreezable<IFrozenString>
{
    new IMutableString Clone();

    StringBuilder BackingStringBuilder { get; }
}
