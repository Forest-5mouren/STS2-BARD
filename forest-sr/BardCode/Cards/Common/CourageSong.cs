using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Common;

/// <summary>
/// 勇气之歌｜CourageSong
/// 效果：获得活力
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class CourageSong : BardCard
{
    private const int energyCost = 0;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Common;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;

    // 基础数值：活力层数（基础3，升级5）
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<VigorPower>(3)
    ];

    // 关键词
    protected override IEnumerable<string> RegisteredKeywordIds => [BardKeywords.Song];

    public CourageSong() : base(energyCost, type, rarity, targetType)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

        await PowerCmd.Apply<VigorPower>(
            Owner.Creature,                  // 目标（自己）
            DynamicVars["VigorPower"].IntValue,   // 层数
            Owner.Creature,                  // 来源（自己）
            this);                           // 关联卡牌
    }

    protected override void OnUpgrade()
    {
        DynamicVars["VigorPower"].UpgradeValueBy(2);  
    }
}