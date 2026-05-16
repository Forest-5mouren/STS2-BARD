using System.IO;
using Forest_Sr.Bard;

namespace Forest_Sr.BardCode.Extensions;

// Mostly utilities to get asset paths.
public static class StringExtensions
{
    private static string ModId => MainFile.ModId;

    public static string ImagePath(this string path)
    {
        return Path.Join(ModId, "Images", path);
    }

    public static string CardImagePath(this string path)
    {
        return Path.Join(ModId, "Images", "Cards", path);
    }

    public static string BigCardImagePath(this string path)
    {
        return Path.Join(ModId, "Images", "Cards", "Big", path);
    }

    public static string PowerImagePath(this string path)
    {
        return Path.Join(ModId, "Images", "Powers", path);
    }

    public static string BigPowerImagePath(this string path)
    {
        return Path.Join(ModId, "Images", "Powers", "Big", path);
    }

    public static string RelicImagePath(this string path)
    {
        return Path.Join(ModId, "Images", "Relics", path);
    }

    public static string BigRelicImagePath(this string path)
    {
        return Path.Join(ModId, "Images", "Relics", "Big", path);
    }

    public static string CharacterUiPath(this string path)
    {
        return Path.Join(ModId, "Images", "Charui", path);
    }
}