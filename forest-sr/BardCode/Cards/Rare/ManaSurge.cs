using System.Collections.Generic;
using System.Threading.Tasks;
using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Forest_Sr.BardCode.Cards.Common;

/// <summary>
/// 法力充沛｜ManaSurge
/// 效果：能力。每回合第一次释放法术时，抽 {DynamicVars.Cards.IntValue} 张牌。
/// 升级：抽牌 1 → 2
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class ManaSurge : BardCard
{
    

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CardsVar( 1)   // 抽牌数量
    ];

    public ManaSurge() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
    }

    // 升级：抽牌 1 → 2
    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        

        // 施加法力充沛能力Power
        await PowerCmd.Apply<ManaSurgePower>(
            Owner.Creature,
            DynamicVars.Cards.IntValue,
            Owner.Creature,
            this
        );
    }
}