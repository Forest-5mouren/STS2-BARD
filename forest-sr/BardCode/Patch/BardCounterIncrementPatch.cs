using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers.Counters;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Keywords;

namespace Forest_Sr.BardCode.Patch;

/// <summary>
/// 隐藏计数器递增补丁
/// 在 Hook.AfterCardPlayed 和 Hook.AfterPlayerTurnStart 中更新/重置计数器
/// </summary>
[HarmonyPatch(typeof(Hook))]
public static class BardCounterIncrementPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Hook.AfterCardPlayed))]
    public static void AfterCardPlayedPostfix(ICombatState combatState, PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var owner = cardPlay.Card.Owner;
        if (owner?.Creature == null) return;

        // ── 乐曲牌计数器 ──
        if (cardPlay.Card.HasModKeyword(BardKeywords.Song))
        {
            var songPower = owner.Creature.GetPower<SongsPlayedCounter>();
            if (songPower == null)
            {
                songPower = (SongsPlayedCounter)ModelDb.Power<SongsPlayedCounter>().ToMutable();
                songPower.ApplyInternal(owner.Creature, 1, silent: true);
            }
            else
            {
                songPower.SetAmount(songPower.Amount + 1);
            }
        }

        // ── 卡牌类型计数器（去重） ──
        var typePower = owner.Creature.GetPower<CardTypesUsedCounter>();
        if (typePower == null)
        {
            typePower = (CardTypesUsedCounter)ModelDb.Power<CardTypesUsedCounter>().ToMutable();
            typePower.RecordedTypes = new HashSet<CardType> { cardPlay.Card.Type };
            typePower.ApplyInternal(owner.Creature, 1, silent: true);
        }
        else if (typePower.RecordedTypes.Add(cardPlay.Card.Type))
        {
            typePower.SetAmount(typePower.Amount + 1);
        }
    }

    /// <summary>
    /// 回合开始时不清零，但确保计数器存在（参考 Combo 的回合重置模式，但我们的计数器是跨回合的）
    /// 这里只做防御性存在检查。回合开始时若计数器不存在也不创建——没打过牌时就是 0。
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Hook.AfterPlayerTurnStart))]
    public static void AfterPlayerTurnStartPostfix(ICombatState combatState, PlayerChoiceContext choiceContext, Player player)
    {
        // 计数器跨战斗保留，由 Godot 的 Power 生命周期管理
        // 战斗结束后所有 Power 自动清除，所以无需手动重置
    }
}
