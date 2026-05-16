using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 格挡反击｜ParryRiposte
/// 效果：获得 {block} 点格挡。如果敌人意图攻击，造成 {damage} 点伤害。如果上一张是技能牌，获得1点能量。
/// 升级：格挡 6→8，伤害 6→8
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class ParryRiposte : BardCard
{
    private const string _blockKey = "block";
    
    

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar( 6, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),    // 格挡值
        new DamageVar( 6, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),   // 伤害值
        new EnergyVar(1)    // 回费显示
    ];

    public ParryRiposte() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

    // 高亮条件：上一张是技能牌
    protected override bool ShouldGlowGoldInternal => WasLastCardSkill;

    // 判断上一张打出的牌是否是技能牌
    private bool WasLastCardSkill
    {
        get
        {
            if (CombatState == null) return false;

            var entry = CombatManager.Instance.History
                .CardPlaysStarted
                .LastOrDefault(e => e.CardPlay.Card.Owner == Owner
                                    && e.HappenedThisTurn(CombatState)
                                    && e.CardPlay.Card != this);
            return entry?.CardPlay.Card.Type == CardType.Skill;
        }
    }

    // 升级：格挡 6→8，伤害 6→8
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2);
        DynamicVars.Damage.UpgradeValueBy(2);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        // 获得格挡
        
        await CreatureCmd.GainBlock(Owner.Creature, new BlockVar(DynamicVars.Block.IntValue, ValueProp.Move), cardPlay);

        // 如果敌人意图攻击，造成伤害
        if (cardPlay.Target.Monster.IntendsToAttack)
        {
            
            await DamageCmd.Attack(DynamicVars.Damage.IntValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash", null, "sword_riposte.mp3")
                .Execute(choiceContext);
        }

        // 如果上一张是技能牌，获得1点能量
        if (WasLastCardSkill)
        {
            await PlayerCmd.GainEnergy(1, Owner);
        }
    }
}