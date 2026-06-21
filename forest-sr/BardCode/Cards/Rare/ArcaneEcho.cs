using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Rare;

/// <summary>
/// 奥术回响｜ArcaneEcho
/// 效果：能力。每当和声达到 {threshold} 层时，从消耗堆随机回收一张法术牌。
/// 升级：阈值 4→3
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class ArcaneEcho : BardCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("threshold", 4)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Magic];

    public ArcaneEcho() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self) { }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await PowerCmd.Apply<ArcaneEchoPower>(ctx, 
            Owner.Creature,
            DynamicVars["threshold"].IntValue,  // Amount = threshold
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["threshold"].UpgradeValueBy(-1);  // 4→3
    }
}

