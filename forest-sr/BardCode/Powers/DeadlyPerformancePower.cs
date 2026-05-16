using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.GameInfo.Objects;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using System.Linq;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Powers;

/// <summary>
/// 夺命演奏能力
/// 效果：每当你打出带有魔法标签的牌时，对随机敌人造成等同于层数的伤害
/// </summary>
[RegisterPower]  // 注册能力
public sealed class DeadlyPerformancePower : BardPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    // 卡牌打出后触发
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        // 检查是否是魔法牌（使用字符串 ID 或常量）
        if (!cardPlay.Card.HasModKeyword(BardKeywords.Magic)) return;
        if (Owner == null) return;


        // 等待一小段时间，让卡牌动画播放
        await Cmd.CustomScaledWait(0.1f, 0.2f);

        // 随机选择一个敌人
        Creature target = Owner.Player.RunState.Rng.CombatTargets.NextItem(Owner.CombatState.HittableEnemies);

        if (target != null)
        {
            // 播放攻击特效
            VfxCmd.PlayOnCreatureCenter(target, "vfx/vfx_attack_slash");

            // 造成伤害（注意：需要传入 context）
            await CreatureCmd.Damage(context, target, base.Amount , ValueProp.Unpowered, Owner);
        }
    }
}