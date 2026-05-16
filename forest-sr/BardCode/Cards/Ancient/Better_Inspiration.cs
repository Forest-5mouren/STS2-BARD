using Forest_Sr.BardCode.Cards;
using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Relics;

/// <summary>
/// 更好的激励
/// 效果：每回合开始时，给全体友方提供 {vigor} 层活力。
/// </summary>
[RegisterRelic(typeof(BardRelicPool))]
public sealed class BetterInspiration : BardRelics
{

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<VigorPower>(6)
    ];

    // 额外悬停提示
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<VigorPower>()
    ];

    public override RelicRarity Rarity => RelicRarity.Starter;

    // 替换为诗人激励
    //public override RelicModel? GetUpgradeReplacement() => ModelDb.Relic<BardicInspiration>();

    // 每回合开始时触发
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        // 闪烁效果
        Flash();


        // 给全体友方施加活力
        await PowerCmd.Apply<VigorPower>(
            player.Creature.CombatState.Allies,
            DynamicVars["VigorPower"].IntValue,
            player.Creature,
            null
        );
    }
}