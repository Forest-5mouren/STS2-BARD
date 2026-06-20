using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Forest_Sr.BardCode.Powers;

namespace Forest_Sr.BardCode.Cards.Common;

/// <summary>
/// 即兴｜Improvise
/// 效果：能力。每回合开始时，随机获得一首乐曲。
/// 升级：获得固有（战斗开始时在手牌中）
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class Improvise : BardCard
{
    private const string _cardsKey = "cards";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar(_cardsKey, 1)   // 每回合获得1张牌
    ];
    

    public Improvise() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int amount = DynamicVars[_cardsKey].IntValue;

        // 施加 ImprovisePower，每回合随机获得一首乐曲
        await PowerCmd.Apply<ImprovisePower>(
            
            Owner.Creature,
            amount,  // 获得数量（默认1）
            Owner.Creature,
            this
        );
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}
