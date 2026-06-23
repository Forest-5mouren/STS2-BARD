using Forest_Sr.BardCode.Cards;
using Forest_Sr.BardCode.Relics;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Nodes.Combat;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Characters;
using STS2RitsuLib.Scaffolding.Godot;

namespace Forest_Sr.BardCode.Character;

[RegisterCharacter]
public sealed class Bard : ModCharacterTemplate<BardCardPool, BardRelicPool, SharedPotionPool>
{
    public const string CharacterId = "Bard";

    // 角色名称颜色
    public override Color NameColor => new("#FFD1DC");
    // 能量图标轮廓颜色
    public override Color EnergyLabelOutlineColor => new("#FFD1DC");
    // 地图绘制颜色
    public override Color MapDrawingColor => new("#FFD1DC");

    // 人物性别
    public override CharacterGender Gender => CharacterGender.Masculine;

    // 初始血量和金币
    public override int StartingHp => 78;
    public override int StartingGold => 99;

    // 角色资源配置
    public override CharacterAssetProfile AssetProfile => CharacterAssetProfiles.Merge(
        CharacterAssetProfiles.Ironclad(),
        new(
            Scenes: new(
                // 人物模型tscn路径
                VisualsPath: "res://Bard/Scenes/BardVisual.tscn",
                // 能量表盘tscn路径
                EnergyCounterPath: "res://Bard/Scenes/bard_energy_counter.tscn",
                // 商店人物场景
                MerchantAnimPath: "res://Bard/Scenes/bard_merchant.tscn",
                //火堆
                RestSiteAnimPath: "res://Bard/Scenes/bard_rest_site.tscn"
            ),
            Ui: new(
                // 游戏左上角头像、角色统计页头像、每日挑战角色头像。这个是场景而不是图片。参考下方附赠资源搭建。
                IconPath: "res://Bard/Images/Charui/character_icon_bard.tscn",
                // 人物头像路径
                IconTexturePath: "res://Bard/Images/Charui/character_icon_bard.tscn",
                // 人物选择背景
                CharacterSelectBgPath: "res://Bard/Scenes/char_select_bg_bard.tscn",
                // 人物选择图标
                CharacterSelectIconPath: "res://Bard/Images/Charui/char_select_bard.png",
                // 人物选择图标-锁定状态
                CharacterSelectLockedIconPath: "res://Bard/Images/Charui/char_select_locked.png",
                // 地图上的角色标记图标
                MapMarkerPath: "res://Bard/Images/Charui/map_marker_bard.png"
            ),
            Vfx: new(
            // 卡牌拖尾场景
            // TrailPath: "res://Bard/vfx/card_trail_bard.tscn"
            ),
            Audio: new(
            // 攻击音效
            // AttackSfx: "event:/bard/attack",
            // 施法音效
            // CastSfx: "event:/bard/cast",
            // 死亡音效
            // DeathSfx: "event:/bard/death",
            // 角色选择音效
            // CharacterSelectSfx: "event:/bard/select",
            // 过渡音效
            // CharacterTransitionSfx: "event:/sfx/ui/wipe_bard"
            ),
            Multiplayer: new(
            // 多人模式-手指
            ArmPointingTexturePath: "res://Bard/Images/Charui/mp_pointing.png",
            // 多人模式剪刀石头布-石头
            ArmRockTexturePath: "res://Bard/Images/Charui/mp_rock.png",
            //多人模式剪刀石头布-布
            ArmPaperTexturePath: "res://Bard/Images/Charui/mp_paper.png",
            // 多人模式剪刀石头布-剪刀
            ArmScissorsTexturePath: "res://Bard/Images/Charui/mp_scissors.png"
            )
        // 其余如果有需要自行取消注释使用
        // Spine: null,
        // VisualCues: null, // 帧动画静态图人物使用，查看角色动画一章
        // WorldProceduralVisuals: null,
        // 以下为让遗物根据你的人物展现不同的图像资源，在列表里添加即可
        // VanillaCardVisualOverrides: [],
        // VanillaRelicVisualOverrides: [
        //     new (CharacterOwnedVanillaRelicModelId.YummyCookie, new("res://icon.svg")) // 美味饼干覆盖
        // ],
        // VanillaPotionVisualOverrides: []
        )
    );

    // 攻击和施法动画延迟
    public override float AttackAnimDelay => 0.25f;
    public override float CastAnimDelay => 0.25f;

    // 角色是否需要时间线小故事
    public override bool RequiresEpochAndTimeline => false;

    // 是否从原版角色选择列表隐藏
    public override bool HideFromVanillaCharacterSelect => false;

    // 是否允许被随机角色选中
    public override bool AllowInVanillaRandomCharacterSelect => true;

    // 是否在卡牌图鉴中隐藏角色卡池筛选
    public override bool HideInCardLibraryCompendium => false;

    // 自动转换人物场景
    protected override NCreatureVisuals? TryCreateCreatureVisuals() =>
        RitsuGodotNodeFactories.CreateFromScenePath<NCreatureVisuals>(AssetProfile.Scenes!.VisualsPath!);

    // 攻击建筑师的攻击特效列表
    public override List<string> GetArchitectAttackVfx() => new()
    {
        "vfx/vfx_attack_slash",
        "vfx/vfx_attack_blunt",
        "vfx/vfx_magic_blast"
    };
}