using System.IO;
using BKEFTTools.Entity.EBundleDB;

namespace BKEFTTools;

public class PathDefine
{
    //Game
    public static string Bundle_Asset_Path = @"EscapeFromTarkov_Data\StreamingAssets\Windows";
    public static string Bundle_DB_Path = @"Windows.json";
    
    //AKI
    public static string AKI_DB_Path = @"Aki_Data\Server\database";
    public static string Global_DB_Path = @"globals.json";
    public static string Item_DB_Path = @"templates\items.json";
    
    public static string UserProfiles_Path = @"user\profiles";
    
    //Tools
    public static string AssetStudioCLI_EXE = @"AssetStudio.CLI.exe";
    
    //OutFolder
    public static string OutputFolder = @"D:\BKEFT";


    public static string GetGlobalDBPath()
    {
        return Path.Combine(DBManager.settingDB.EFT_Path,AKI_DB_Path,Global_DB_Path);
    }
    
    public static string GetItemDBPath()
    {
        return Path.Combine(DBManager.settingDB.EFT_Path,AKI_DB_Path,Item_DB_Path);
    }
    
    public static string GetBundleDBPath()
    {
        return Path.Combine(DBManager.settingDB.EFT_Path,Bundle_Asset_Path,Bundle_DB_Path);
    }

    public static string GetBundleFullPathByPath(string path)
    {
        return Path.Combine(DBManager.settingDB.EFT_Path,Bundle_Asset_Path,path.Replace('/', '\\'));
    }


    public static string GetPresetItemOutPath(string presetName, string soltId, string itemName)
    {
        return Path.Combine(OutputFolder,presetName,soltId, itemName);
    }

    public static string GetPresetOutPath(string presetName)
    {
        return Path.Combine(OutputFolder,presetName);
    }

    public static void SetOutputFolder(string outputFolder)
    {
        OutputFolder = outputFolder;
    }

    public static string GetAssetStudioCLIPath()
    {
        return Path.Combine(DBManager.settingDB.AssetStudioCLI_Path, AssetStudioCLI_EXE);
    }

    public static string GetUserProfileFolderPath()
    {
        return Path.Combine(DBManager.settingDB.EFT_Path, UserProfiles_Path);
    }
}