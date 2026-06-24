using STS2RitsuLib.Audio;

namespace Forest_Sr.BardCode.Audio;

/// <summary>
/// 全部音频资源的路径常量及播放辅助方法。
/// 音频文件位于 res://Bard/Audio/，已通过 .godotignore 排除 Godot 导入处理，
/// FMOD 可直接加载原始 mp3 文件。
/// </summary>
public static class BardAudio
{
    // ── 路径常量（对应 Bard/Audio/ 目录下的 mp3 文件） ───

    /// <summary>Accelerando / 渐速音 — 无对应卡牌</summary>
    public const string Accelerando = "res://Bard/Audio/Accelerando.mp3";
    /// <summary>Allegro / 快板 — 对应 Allegro 卡牌</summary>
    public const string Allegro = "res://Bard/Audio/Allegro.mp3";
    /// <summary>BansheeWail / 女妖之嚎 — 对应 BansheeWail 卡牌</summary>
    public const string BansheeWail = "res://Bard/Audio/BansheeWail.mp3";
    /// <summary>BardBlock (Harmony Shield) / 和谐之盾 — 对应 BardBlock 卡牌</summary>
    public const string BardBlock = "res://Bard/Audio/BardBlock.mp3";
    /// <summary>BriskBeat / 轻快节拍 — 对应 BriskBeat 卡牌</summary>
    public const string BriskBeat = "res://Bard/Audio/BriskBeat.mp3";
    /// <summary>ChaosNoise / 混乱噪音 — 无对应卡牌</summary>
    public const string ChaosNoise = "res://Bard/Audio/ChaosNoise.mp3";
    /// <summary>ClearChant / 清晰赞歌 — 对应 ClearChant 卡牌</summary>
    public const string ClearChant = "res://Bard/Audio/ClearChant.mp3";
    /// <summary>CourageSong / 勇气之歌 — 对应 CourageSong 卡牌</summary>
    public const string CourageSong = "res://Bard/Audio/CourageSong.mp3";
    /// <summary>DarkCacophony / 黑暗喧嚣 — 对应 DarkCacophony 卡牌</summary>
    public const string DarkCacophony = "res://Bard/Audio/DarkCacophony.mp3";
    /// <summary>EarPiercingShriek / 尖锐刺耳 — 对应 EarPiercingShriek 卡牌</summary>
    public const string EarPiercingShriek = "res://Bard/Audio/EarPiercingShriek.mp3";
    /// <summary>FreeChord / 自由和弦 — 无对应卡牌</summary>
    public const string FreeChord = "res://Bard/Audio/FreeChord.mp3";
    /// <summary>InspiringMelody / 激昂旋律 — 对应 InspiringMelody 卡牌</summary>
    public const string InspiringMelody = "res://Bard/Audio/InspiringMelody.mp3";
    /// <summary>LightMelody / 轻快旋律 — 无对应卡牌</summary>
    public const string LightMelody = "res://Bard/Audio/LightMelody.mp3";
    /// <summary>MakePeace / 缔造和平 — 无对应卡牌</summary>
    public const string MakePeace = "res://Bard/Audio/MakePeace.mp3";
    /// <summary>SereneSong / 宁静赞歌 — 对应 SereneSong 卡牌</summary>
    public const string SereneSong = "res://Bard/Audio/SereneSong.mp3";
    /// <summary>ShadowSong / 暗影之歌 — 无对应卡牌</summary>
    public const string ShadowSong = "res://Bard/Audio/ShadowSong.mp3";
    /// <summary>SilenceSong / 沉默之歌 — 无对应卡牌</summary>
    public const string SilenceSong = "res://Bard/Audio/SilenceSong.mp3";
    /// <summary>TiredElegy / 疲倦挽歌 — 无对应卡牌</summary>
    public const string TiredElegy = "res://Bard/Audio/TiredElegy.mp3";
    /// <summary>WeaknessShriek / 虚弱尖叫 — 对应 WeaknessShriek 卡牌</summary>
    public const string WeaknessShriek = "res://Bard/Audio/WeaknessShriek.mp3";

    /// <summary>
    /// 全部音频文件路径列表，用于批量预加载。
    /// </summary>
    private static readonly string[] AllPaths =
    [
        Accelerando, Allegro, BansheeWail, BardBlock, BriskBeat,
        ChaosNoise, ClearChant, CourageSong, DarkCacophony, EarPiercingShriek,
        FreeChord, InspiringMelody, LightMelody, MakePeace,
        SereneSong, ShadowSong, SilenceSong, TiredElegy, WeaknessShriek,
    ];

    // ── 预加载 ────────────────────────────────────────

    /// <summary>
    /// 在初始化时调用，预加载所有音频到 FMOD 运行时缓存，
    /// 避免首次播放时出现卡顿。
    /// </summary>
    public static void PreloadAll()
    {
        foreach (var path in AllPaths)
        {
            FmodStudioStreamingFiles.TryPreloadAsSound(path);
        }
    }

    // ── 播放方法 ──────────────────────────────────────

    /// <summary>
    /// 播放一次指定路径的音频（旧式 API，简洁快速）。
    /// </summary>
    public static bool Play(string path, float volume = 1f, float pitch = 1f)
    {
        return FmodStudioStreamingFiles.TryPlaySoundFile(path, volume, pitch);
    }

    /// <summary>
    /// 播放一次指定路径的音频（新式 API，支持完整选项）。
    /// </summary>
    public static AudioPlayResult PlayOneShot(string path, AudioPlaybackOptions? options = null)
    {
        return GameAudioService.Shared.PlayOneShot(AudioSource.File(path), options);
    }
}
