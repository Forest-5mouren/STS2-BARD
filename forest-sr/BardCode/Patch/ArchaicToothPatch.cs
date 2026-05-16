using Forest_Sr.BardCode.Cards.Ancient;
using Forest_Sr.BardCode.Cards.Basic;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using System.Collections.Generic;

namespace Forest_Sr.BardCode.Patch;

/// <summary>
/// 为 ArchaicTooth 添加 Bard 初始卡牌的先古升级映射
/// </summary>
[HarmonyPatch]
public static class ArchaicToothPatch
{
    /// <summary>
    /// 在 ArchaicTooth 的 TranscendenceUpgrades 属性被访问后，动态添加 Bard 卡牌的映射
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ArchaicTooth), "get_TranscendenceUpgrades")]
    public static void ArchaicTooth_TranscendenceUpgrades_Postfix(ref Dictionary<ModelId, CardModel> __result)
    {
        if (__result == null) return;

        // 获取 ViciousMockery 和 PhantasmalKiller 的 ModelId
        var viciousMockeryId = ModelDb.Card<ViciousMockery>().Id;
        var phantasmalKiller = ModelDb.Card<PhantasmalKiller>();

        // 如果字典中还没有这个映射，则添加
        if (!__result.ContainsKey(viciousMockeryId))
        {
            __result[viciousMockeryId] = phantasmalKiller;
        }
    }
}