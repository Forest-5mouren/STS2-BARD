using Forest_Sr.Bard;
using Godot;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Forest_Sr.BardCode.Powers;

[RegisterPower(Inherit = true)]  // Inherit = true 让所有子类自动注册
public abstract class BardPower : ModPowerTemplate
{
    // 统一能力图标路径（小型图标）
    public override PowerAssetProfile AssetProfile => new()
    {
        IconPath = GetIconPath(),
        BigIconPath = GetBigIconPath()
    };

    private string GetIconPath()
    {
        var cardName = GetPowerName();
        var path = $"res://{MainFile.ModId}/Images/Powers/{cardName}.png";
        return ResourceLoader.Exists(path) ? path : $"res://{MainFile.ModId}/Images/Powers/default.png";
    }

    private string GetBigIconPath()
    {
        var cardName = GetPowerName();
        var path = $"res://{MainFile.ModId}/Images/Powers/{cardName}.png";
        return ResourceLoader.Exists(path) ? path : $"res://{MainFile.ModId}/Images/Powers/default.png";
    }

    private string GetPowerName()
    {
        var entry = Id.Entry;
        // 移除前缀，例如 "BARD_POWER_BARD_POWER" -> "BARD_POWER"
        var prefix = "BARD_POWER_";
        var name = entry.StartsWith(prefix) ? entry[prefix.Length..] : entry;
        return name.ToLowerInvariant();
    }
}