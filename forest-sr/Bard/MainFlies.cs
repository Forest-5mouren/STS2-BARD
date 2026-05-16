using Forest_Sr.BardCode.Cards.Ancient;
using Forest_Sr.BardCode.Cards.Basic;
using Forest_Sr.BardCode.Character;
using Forest_Sr.BardCode.Relics;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models.Characters;
using STS2RitsuLib;
using STS2RitsuLib.Interop;
using STS2RitsuLib.Scaffolding.Content;
using System.Reflection;

namespace Forest_Sr.Bard;

[ModInitializer(nameof(Initialize))]
public class MainFile
{
    public const string ModId = "Bard";
    public static Logger Logger { get; private set; } = null!;

    public static void Initialize()
    {
        Logger = RitsuLibFramework.CreateLogger(ModId);

        // 注册程序集（启用注解式注册）
        ModTypeDiscoveryHub.RegisterModAssembly(ModId, Assembly.GetExecutingAssembly());

        // 可选：注册初始卡牌/遗物（使用注解方式则不需要）
        RitsuLibFramework.CreateContentPack(ModId)
            .Character<Forest_Sr.BardCode.Character.Bard>()
            .CharacterStarterCard<Forest_Sr.BardCode.Character.Bard, BardAttack>(4, order: 10)
            .CharacterStarterCard<Forest_Sr.BardCode.Character.Bard, BardBlock>(4, order: 20)
            .CharacterStarterCard<Forest_Sr.BardCode.Character.Bard, ViciousMockery>(1, order: 30)
            .CharacterStarterCard<Forest_Sr.BardCode.Character.Bard, BladeWard>(1, order: 40)
            .CharacterStarterRelic<Forest_Sr.BardCode.Character.Bard, BardicInspiration>(1, order: 0)
            .Apply();
        //升级遗物
        RitsuLibFramework.RegisterTouchOfOrobasRefinementMapping<BardicInspiration, BetterInspiration>();
        //升级初始卡
        RitsuLibFramework.RegisterArchaicToothTranscendenceMapping<ViciousMockery, PhantasmalKiller>();
        // Harmony 补丁（如果需要）
        var harmony = new Harmony(ModId);
        harmony.PatchAll();
        harmony.PatchAll(Assembly.GetExecutingAssembly());

        Logger.Info("Bard mod initialized with RitsuLib!");
    }
}