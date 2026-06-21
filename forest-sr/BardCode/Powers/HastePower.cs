using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Powers;

public sealed class HastePower : BardPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override bool AllowNegative => false;

    protected override List<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.Static(StaticHoverTip.Energy),
        HoverTipFactory.Static(StaticHoverTip.CardReward)
    ];

    public override decimal ModifyMaxEnergy(Player player, decimal amount)
    {
        if (player != base.Owner?.Player) return amount;
        return amount + 2m;
    }

    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        if (player != base.Owner?.Player) return count;
        return count + 2m;
    }

    public override async Task AfterEnergyReset(Player player)
    {
        if (player == base.Owner.Player)
        {
            await PowerCmd.Decrement(this);
        }
        if (Amount <= 0)
        {
            Flash();
            var bctx = new BlockingPlayerChoiceContext();
            await PowerCmd.Apply<VulnerablePower>(bctx, base.Owner, 2m, base.Owner, null);
            await PowerCmd.Apply<WeakPower>(bctx, base.Owner, 2m, base.Owner, null);
            await PowerCmd.Remove(this);
        }
    }
}
