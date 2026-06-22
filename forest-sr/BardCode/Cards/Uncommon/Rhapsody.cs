using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 狂想曲｜Rhapsody
/// 效果：抽 {DynamicVars.Cards.IntValue} 张牌。每抽到一张法术牌，获得 {energy} 点能量。
/// 升级：抽牌数 2 → 3
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class Rhapsody : BardCard
{



    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CardsVar( 2),   // 抽牌数
        new EnergyVar(1)       // 每张法术回复能量
    ];

    // 关键词：乐曲
    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        BardKeywords.Song
    ];

    public Rhapsody() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
    }

    // 升级：抽牌数 2 → 3
    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {



        // 获取手牌列表
        var handCards = Owner.PlayerCombatState?.Hand?.Cards;
        if (handCards == null) return;

        // 记录抽牌前的手牌
        var beforeHand = new HashSet<CardModel>(handCards);

        // 抽牌
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);

        // 重新获取手牌
        handCards = Owner.PlayerCombatState?.Hand?.Cards;
        if (handCards == null) return;

        // ✅ 修正：使用 HasModKeyword 检查法术牌
        int spellCount = handCards
            .Where(c => !beforeHand.Contains(c) && c.HasModKeyword(BardKeywords.Magic))
            .Count();

        if (spellCount > 0)
        {
            await PlayerCmd.GainEnergy(spellCount * DynamicVars.Energy.IntValue, Owner);
        }
    }
}