using System.Collections.Concurrent;
using Forest_Sr.Bard;
using Godot;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Forest_Sr.BardCode.Powers;

[RegisterPower(Inherit = true)]  // Inherit = true 让所有子类自动注册
public abstract class BardPower : ModPowerTemplate
{
    /// <summary>
    /// 能力图标路径缓存：key = power ID entry（如 allegro_power），value = 解析后的完整路径
    /// ResourceLoader.Exists 只在第一次访问时调用一次
    /// </summary>
    private static readonly ConcurrentDictionary<string, string> IconPathCache = new();

    // 统一能力图标路径（小型图标）
    public override PowerAssetProfile AssetProfile => new()
    {
        IconPath = GetCachedIconPath(),
        BigIconPath = GetCachedIconPath()
    };

    private string GetCachedIconPath()
    {
        var key = GetPowerName();

        return IconPathCache.GetOrAdd(key, k =>
        {
            var path = $"res://{MainFile.ModId}/Images/Powers/{k}.png";
            return ResourceLoader.Exists(path)
                ? path
                : $"res://{MainFile.ModId}/Images/Powers/default.png";
        });
    }

    private string GetPowerName()
    {
        var entry = Id.Entry;
        // 移除前缀，例如 "BARD_POWER_ALLEGRO_POWER" -> "ALLEGRO_POWER"
        var prefix = "BARD_POWER_";
        var name = entry.StartsWith(prefix) ? entry[prefix.Length..] : entry;
        return name.ToLowerInvariant();
    }
}
