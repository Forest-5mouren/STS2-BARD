using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Basic;

/// <summary>
/// 刺耳尖叫｜Bard Strike
/// 效果：造成 5(8) 点伤害，施加 1 层易伤。乐曲。
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class BardAttack : BardCard
{
    private const int energyCost = 1;
    private const CardType type = CardType.Attack;
    private const CardRarity rarity = CardRarity.Basic;
    private const TargetType targetType = TargetType.AnyEnemy;

    // 乐曲标签
    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Song];
    protected override HashSet<CardTag> CanonicalTags => new() { CardTag.Strike };

    // 基础数值：伤害 + 易伤
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(5, ValueProp.Move),
        new PowerVar<VulnerablePower>(1)
    ];

    public BardAttack() : base(energyCost, type, rarity, targetType) { }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        // 造成伤害
        await DamageCmd.Attack(DynamicVars.Damage.IntValue)
            .FromCard(this)
            .Targeting(cardPlay.Target!)
            .Execute(ctx);

        // 施加易伤
        if (cardPlay.Target != null)
        {
            await PowerCmd.Apply<VulnerablePower>(ctx, 
                cardPlay.Target,
                DynamicVars["VulnerablePower"].IntValue,
                Owner.Creature,
                this);
        }
    }

    // 升级：伤害 5 → 8
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }
}


