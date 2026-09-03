
using System.IO;

namespace PostGetImage.Models;
public static class modFileAndDirecrty
{

    public static bool VerfingDirectionByPath(string Path)
    {
        if(!Directory.Exists(Path)) Directory.CreateDirectory(Path); 
        return Directory.Exists(Path);
    }

    public static bool SaveImageFromBytes(byte[] imageBytes, string filePath)
    {
        File.WriteAllBytes(filePath, imageBytes);
        return File.Exists(filePath);
    }

}
