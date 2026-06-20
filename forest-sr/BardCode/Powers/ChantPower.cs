using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System.Linq;
using System.Threading.Tasks;
using STS2RitsuLib.Keywords;

namespace Forest_Sr.BardCode.Powers;

/// <summary>
/// 吟唱基类：所有吟唱效果在下一回合开始时自动触发
/// </summary>
public abstract class ChantPower : BardPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    protected abstract Task OnChantTrigger(PlayerChoiceContext ctx);

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext ctx, Player player)
    {
        if (player != Owner?.Player) return;
        Flash();
        await OnChantTrigger(ctx);
        await PowerCmd.Remove(this);
    }
}

/// <summary>
/// 勇气之歌吟唱：下回合获得力量+活力
/// </summary>
public sealed class CourageSongChant : ChantPower
{
    public decimal StrengthAmount { get; set; } = 1;
    public decimal VigorAmount { get; set; } = 2;

    protected override async Task OnChantTrigger(PlayerChoiceContext ctx)
    {
        await PowerCmd.Apply<StrengthPower>(Owner, StrengthAmount, Owner, null);
        await PowerCmd.Apply<VigorPower>(Owner, VigorAmount, Owner, null);
    }
}

/// <summary>
/// 女妖之嚎吟唱：下回合全体敌人虚弱+易伤
/// </summary>
public sealed class BansheeWailChant : ChantPower
{
    public decimal WeakAmount { get; set; } = 1;
    public decimal VulnerableAmount { get; set; } = 1;

    protected override async Task OnChantTrigger(PlayerChoiceContext ctx)
    {
        foreach (Creature enemy in Owner.CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<WeakPower>(enemy, WeakAmount, Owner, null);
            await PowerCmd.Apply<VulnerablePower>(enemy, VulnerableAmount, Owner, null);
        }
    }
}

/// <summary>
/// 激昂旋律吟唱：下回合全体队友力量+敏捷
/// </summary>
public sealed class InspiringMelodyChant : ChantPower
{
    public decimal StrengthAmount { get; set; } = 1;
    public decimal DexterityAmount { get; set; } = 1;

    protected override async Task OnChantTrigger(PlayerChoiceContext ctx)
    {
        foreach (Creature ally in Owner.CombatState.Allies)
        {
            await PowerCmd.Apply<StrengthPower>(ally, StrengthAmount, Owner, null);
            await PowerCmd.Apply<DexterityPower>(ally, DexterityAmount, Owner, null);
        }
    }
}

/// <summary>
/// 清晰赞歌吟唱：下回合从抽牌堆搜索法术牌，获得易伤
/// </summary>
public sealed class ClearChantChant : ChantPower
{
    public int DrawAmount { get; set; } = 2;
    public decimal VulnerableAmount { get; set; } = 1;

    protected override async Task OnChantTrigger(PlayerChoiceContext ctx)
    {
        var player = Owner?.Player;
        if (player == null) return;

        // 从抽牌堆搜索法术牌
        var drawPile = PileType.Draw.GetPile(player);
        var spells = drawPile?.Cards
            .Where(c => c.HasModKeyword(Forest_Sr.BardCode.Cards.KeyWord.BardKeywords.Magic))
            .Take(DrawAmount)
            .ToList();

        if (spells != null && spells.Count > 0)
        {
            await CardPileCmd.Add(spells, PileType.Hand, source: null);
        }

        // 给予自己易伤
        await PowerCmd.Apply<VulnerablePower>(Owner, VulnerableAmount, Owner, null);
    }
}

/// <summary>
/// 宁静之歌吟唱：下回合全体队友回复生命
/// </summary>
public sealed class SereneSongChant : ChantPower
{
    public decimal HealAmount { get; set; } = 8;

    protected override async Task OnChantTrigger(PlayerChoiceContext ctx)
    {
        foreach (Creature ally in Owner.CombatState.Allies)
        {
            await CreatureCmd.Heal(ally, HealAmount);
        }
    }
}
