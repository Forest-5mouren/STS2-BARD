using Forest_Sr.BardCode.Cards.KeyWord;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using STS2RitsuLib.Interop.AutoRegistration;
namespace Forest_Sr.BardCode.Cards.Uncommon;
/// <summary>
/// 变巨术｜Enlarge
/// 效果：使一个友方获得巨化效果（下一次强化攻击造成3倍伤害）
/// 升级：费用 2 → 1
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class Enlarge : BardCard
{
    private const string _gigantificationKey = "gigantification";
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar(_gigantificationKey, 1)   // 巨化层数
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Magic,CardKeyword.Exhaust];
    public Enlarge() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }
    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        // 确定目标（如果没有指定，则选择自己）
        Creature target = cardPlay.Target ?? Owner.Creature;

        // 播放红色特效
        NCombatRoom.Instance?.PlaySplashVfx(target, new Color(Colors.Red));
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

        // 施加巨化能力
        int amount = DynamicVars[_gigantificationKey].IntValue;
        await PowerCmd.Apply<GigantificationPower>(
            ctx,
            target,
            amount,
            Owner.Creature,
            this
            );
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
