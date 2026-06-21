using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Character;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Basic;

/// <summary>
/// 和谐之盾｜Bard Block
/// 效果：获得 5(7) 点格挡。获得 1(2) 层临时敏捷（回合结束消失）。乐曲。
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class BardBlock : BardCard
{
    private const int energyCost = 1;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Basic;
    private const TargetType targetType = TargetType.Self;

    // 乐曲标签
    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Song];
    protected override HashSet<CardTag> CanonicalTags => new() { CardTag.Defend };

    // 基础数值：格挡 + 临时敏捷
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(5, ValueProp.Move),
        new DynamicVar("tempDexterity", 1)
    ];

    public BardBlock() : base(energyCost, type, rarity, targetType) { }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        // 获得格挡
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block.IntValue, ValueProp.Move, cardPlay);

        // 获得临时敏捷（回合结束消失）
        await PowerCmd.Apply<SlideStepPower>(ctx, 
            Owner.Creature,
            DynamicVars["tempDexterity"].IntValue,
            Owner.Creature,
            this);
    }

    // 升级：格挡 5→7，临时敏捷 1→2
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2);
        DynamicVars["tempDexterity"].UpgradeValueBy(1);
    }
}


