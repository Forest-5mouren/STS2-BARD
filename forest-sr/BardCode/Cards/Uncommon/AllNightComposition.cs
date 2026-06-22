using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 通宵谱曲｜All-Night Composition
/// 效果：获得 {energy} 点能量，往抽牌堆加入 1 张晕眩。
/// 升级：能量 2 → 3
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class AllNightComposition : BardCard
{

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new EnergyVar(2)
    ];

    // 额外悬停提示
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromCard<Dazed>()
    ];

    public AllNightComposition() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
    }

    // 升级：能量 2 → 3
    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 播放创作特效（紫色灵感光芒）
        NCombatRoom.Instance?.PlaySplashVfx(Owner.Creature, new Color("#9B59B6"));

        // 获得能量
        int energyToGain = DynamicVars.Energy.IntValue;
        await PlayerCmd.GainEnergy(energyToGain, Owner);

        // 塞眩晕
        CardModel card = CombatState.CreateCard<Dazed>(Owner);
        CardCmd.PreviewCardPileAdd(
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Draw, Owner, CardPilePosition.Random)
        );

        // 短暂延迟，让特效和动画有足够时间播放
        await Cmd.Wait(0.25f);
    }
}
