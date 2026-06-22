using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
namespace Forest_Sr.BardCode.Cards.Rare;
/// <summary>
/// 安定心神｜CalmEmotions
/// 效果：清除所有负面效果（Debuff）。
/// 升级：费用 3 → 2
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class CalmEmotions : BardCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Magic, CardKeyword.Exhaust];

    public CalmEmotions() : base(2, CardType.Skill, CardRarity.Rare, TargetType.AnyPlayer) { }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Creature target = cardPlay.Target ?? Owner.Creature;
        await ClearDebuffs(target);
    }

    private async Task ClearDebuffs(Creature target)
    {
        var powers = target.Powers.ToList();
        foreach (var power in powers)
        {
            if (power.Type == PowerType.Debuff)
            {
                await PowerCmd.Remove(power);
            }
        }
    }
}
