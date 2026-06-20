using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Rare;

/// <summary>
/// 绝不认输｜NeverGiveUp
/// 效果：乐曲。能力。每当你即将死亡时，改为回复到 {healAmount} 点生命，消耗此能力并抽 {drawAmount} 张牌。
/// 升级：费用 3 → 2，回复 1 → 2，抽牌 3 → 4
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class NeverGiveUp : BardCard
{
    private const string _healAmountKey = "healAmount";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar(_healAmountKey, 5),   // 回复生命值
    ];

    // 关键词
    protected override IEnumerable<string> RegisteredKeywordIds => [
        BardKeywords.Song,
    ];

    public override List<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];

    public NeverGiveUp() : base(3, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1); 
    }
    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        // 检查是不是自己打的牌
        if (cardPlay.Card.Owner != Owner)
        {
            return Task.CompletedTask;
        }

        // 检查打出的牌是否是乐曲牌（参考 Stomp 检查攻击牌的方式）
        if (!cardPlay.Card.HasModKeyword(BardKeywords.Song))
        {
            return Task.CompletedTask;
        }

        // 减少此牌费用（参考 Stomp.ReduceCostBy）
        ReduceCostBy(1);
        return Task.CompletedTask;
    }
    private void ReduceCostBy(int amount)
    {
        EnergyCost.AddThisTurn(-amount);
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 施加能力Power（层数为1，作为标记）
        await PowerCmd.Apply<NeverGiveUpPower>(
            Owner.Creature,
            1,
            Owner.Creature,
            this
        );
    }
}