using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Common;

/// <summary>
/// 灵感共鸣｜InspirationResonance
/// 效果：消耗。本回合内，每打出一张乐曲牌，回复 {energy} 点能量。
/// 升级：回复能量 1→2，获得保留
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class InspirationResonance : BardCard
{
    

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new EnergyVar(1)
    ];

    // 关键词
    protected override IEnumerable<string> RegisteredKeywordIds => ["EXHAUST"];
    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }

    public InspirationResonance() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int energyPerSong = DynamicVars.Energy.IntValue;

        // 不需要 choiceContext
        await PowerCmd.Apply<InspirationResonancePower>(
            Owner.Creature,
            energyPerSong,
            Owner.Creature,
            this
        );
    }
}