using HarmonyLib;
using MegaCrit.Sts2.Core.Runs;

namespace Forest_Sr.BardCode.Patch;

/// <summary>
/// 补丁目标缺失（STS2 v107 中不存在 TheAncient 类型）
/// 原计划在某个与"古老"相关的对象方法中调用 SetLocalPlayerReady()
/// 目前作为安全占位，不修改任何游戏行为
/// </summary>
[HarmonyPatch]
internal static class TheAncient
{
    // 无活跃补丁 - 仅保留此类供 Harmony PatchAll 扫描
    // 实际不会修改任何方法
}
