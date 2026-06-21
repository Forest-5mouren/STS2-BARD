// This power is no longer used. Negative StrengthPower/DexterityPower are applied directly from WeaknessShriek card.
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Powers;

public sealed class WeaknessShriekPower : BardPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Single;
}
