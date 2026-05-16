using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Powers;

/// <summary>
/// 英雄气概能力
/// 效果：每回合开始时获得固定格挡
/// </summary>
[RegisterPower]
public sealed class HeroismPower : BardPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    // 基础数值：格挡值（由 SetDynamicVars.Block.IntValue 设置）
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(5,ValueProp.Unpowered)
    ];

    // 额外提示文本
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.Block)
    ];

    /// <summary>
    /// 设置每回合获得的格挡值
    /// </summary>

    /// <summary>
    /// 每回合开始时获得格挡
    /// </summary>
    public override async Task AfterBlockCleared(Creature creature)
    {
        if (creature == Owner)
        {
            Flash();
            await CreatureCmd.GainBlock(Owner, DynamicVars.Block.IntValue, ValueProp.Unpowered, null);
            await PowerCmd.ModifyAmount(this, -1m, null, null);
        }
    }
}