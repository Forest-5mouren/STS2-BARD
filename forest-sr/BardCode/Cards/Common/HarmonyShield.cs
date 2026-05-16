using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Forest_Sr.BardCode.Cards.Common;

/// <summary>
/// 和谐之盾｜HarmonyShield
/// 效果：获得6点格挡，获得2点临时敏捷（回合结束消失）。
/// 升级：临时敏捷 2→4
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class HarmonyShield : BardCard
{
    private const int energyCost = 1;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Common;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;

    // 基础数值：格挡值 + 临时敏捷层数
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(6,ValueProp.Move),
        ModCardVars.Int("dexterity", 2)
    ];

    // 额外提示文本
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<DexterityPower>()
    ];

    // 关键词
    protected override IEnumerable<string> RegisteredKeywordIds => [BardKeywords.Song];

    public HarmonyShield() : base(energyCost, type, rarity, targetType)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int dexterityAmount = DynamicVars["dexterity"].IntValue;

        

        // 1. 施加临时敏捷（使用 HarmonyShieldPower 或直接使用 DexterityPower 并设置持续时间）
        await PowerCmd.Apply<HarmonyShieldPower>(
            Owner.Creature,
            dexterityAmount,
            Owner.Creature,
            this);

        // 2. 获得格挡
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block.IntValue, ValueProp.Move, cardPlay);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["dexterity"].UpgradeValueBy(2);  // 2 → 4
    }
}