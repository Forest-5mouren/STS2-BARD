using Forest_Sr.BardCode.Relics;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

namespace Forest_Sr.BardCode.Patches;

/// <summary>
/// 为 TouchOfOrobas 添加 Bard 遗物的升级映射
/// </summary>
[HarmonyPatch]
public static class TouchOfOrobasRefinementPatch
{
    /// <summary>
    /// 在 TouchOfOrobas 的 RefinementUpgrades 属性被访问后，动态添加 Bard 遗物的映射
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(TouchOfOrobas), "get_RefinementUpgrades")]
    public static void TouchOfOrobas_RefinementUpgrades_Postfix(ref Dictionary<ModelId, RelicModel> __result)
    {
        if (__result == null) return;

        // 获取 BardicInspiration 和 BetterInspiration 的 ModelId
        var bardicInspirationId = ModelDb.Relic<BardicInspiration>().Id;
        var betterInspiration = ModelDb.Relic<BetterInspiration>();

        // 如果字典中还没有这个映射，则添加
        if (!__result.ContainsKey(bardicInspirationId))
        {
            __result[bardicInspirationId] = betterInspiration;
        }
    }
}