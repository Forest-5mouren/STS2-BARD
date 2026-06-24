using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Audio;

namespace Forest_Sr.BardCode.Cards.Rare;

/// <summary>
/// 黑暗喧嚣｜DarkCacophony
/// 效果：能力。每回合开始时，所有敌人失去 {damage} 点生命。乐曲。
/// 升级：伤害 3→5
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class DarkCacophony : BardCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(3, ValueProp.Unblockable | ValueProp.Unpowered)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Song];

    public DarkCacophony() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self) { }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await PowerCmd.Apply<DarkCacophonyPower>(ctx, Owner.Creature, DynamicVars.Damage.IntValue, Owner.Creature, this);
                SfxCmd.Play("event:/Bard/sfx/DarkCacophony");
    }

 protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }
}



