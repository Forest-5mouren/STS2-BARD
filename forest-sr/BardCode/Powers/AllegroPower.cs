using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Powers;

/// <summary>
/// 快板能力：每回合开始时获得1点能量，持续若干回合
/// </summary>
[RegisterPower]
public sealed class AllegroPower : BardPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    // 额外提示文本
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.Energy)
    ];

    /// <summary>
    /// 修改最大能量（每回合开始时触发）
    /// </summary>
    public override decimal ModifyMaxEnergy(Player player, decimal amount)
    {
        if (player != Owner?.Player) return amount;
        return amount + 1m;  // +1最大能量
    }

    /// <summary>
    /// 能量重置后（回合开始时）减少持续时间
    /// </summary>
    public override async Task AfterEnergyReset(Player player)
    {
        if (player == Owner?.Player)
        {
            await PowerCmd.ModifyAmount(new BlockingPlayerChoiceContext(), this, -1m, null, null, false);
        }

        if (Amount <= 0)
        {
            Flash();
            await PowerCmd.Remove(this);
        }
    }
}
