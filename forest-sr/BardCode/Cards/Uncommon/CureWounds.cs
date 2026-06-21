using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Forest_Sr.BardCode.Cards.Uncommon;
/// <summary>
/// 疗伤术｜CureWounds
/// 效果：回复 {heal} 点生命。消耗。法术。
/// 升级：回复 7 → 10
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class CureWounds : BardCard{
    private const string _healKey = "heal";

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar(_healKey, 7)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Magic, CardKeyword.Exhaust];

    public CureWounds() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyPlayer) { }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        Creature target = cardPlay.Target ?? Owner.Creature;
        int healAmount = DynamicVars[_healKey].IntValue;
        await CreatureCmd.Heal(target, healAmount, playAnim: true);
    }

    protected override void OnUpgrade()
    {
        DynamicVars[_healKey].UpgradeValueBy(3);
    }
}
