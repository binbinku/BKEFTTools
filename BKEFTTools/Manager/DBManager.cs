using System.IO;
using System.Windows;
using BKEFTTools.Entity;
using BKEFTTools.Entity.EBundleDB;
using BKEFTTools.Entity.EUserDB;
using Newtonsoft.Json;

namespace BKEFTTools;

public class DBManager
{
    public static event Action EAllDataLoaded;
    
    public static GlobalDB globalDB;

    public static Dictionary<string, Item> itemDB;
    
    public static Dictionary<string, Bundle> bundleDB;

    public static SettingData settingDB;

    //User
    public static Dictionary<string, string> userProfileDic;
    
    public static UserProfile curUserProfile;
    
    public static void Init()
    {
        LoadSettingDB();
    }

    private static void LoadUserProfileList()
    {
        userProfileDic = new Dictionary<string, string>();
        var userProfileFolderPath = PathDefine.GetUserProfileFolderPath();
        var upfs = new DirectoryInfo(userProfileFolderPath).GetFiles();
        foreach (var fileInfo in upfs)
        {
            userProfileDic.Add(fileInfo.Name.Replace(".json",""), fileInfo.FullName);
        }
    }

    public static void LoadUserProfile(string profileName)
    {
        var path = userProfileDic[profileName];
        curUserProfile = JsonConvert.DeserializeObject<UserProfile>(File.ReadAllText(path));
    }
    
    public static void LoadData()
    {
        if (Directory.Exists(settingDB.EFT_Path) == false)
        {
            MessageBox.Show("EFT Path not found");
            return;
        }
        
        if (Directory.Exists(settingDB.AssetStudioCLI_Path) == false)
        {
            MessageBox.Show("AssetStudioCLI Path not found");
            return;
        }
        
        try
        {
            LoadGlobalDB();
            LoadItemDB();
            LoadBundleDB();
            LoadUserProfileList();
            MessageBox.Show("Data Load Finished");
            SaveSettingDB();
            EAllDataLoaded?.Invoke();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
            throw;
        }
    }
    
    
    private static void LoadGlobalDB()
    {
        Console.WriteLine("Loading [Global DB] Start");
        
        globalDB = JsonConvert.DeserializeObject<GlobalDB>(File.ReadAllText(PathDefine.GetGlobalDBPath()));
        
        Console.WriteLine("Loading [Global DB] End");
    }
    
    private static void LoadItemDB()
    {
        Console.WriteLine("Loading [Item DB] Start");
        
        itemDB = JsonConvert.DeserializeObject<Dictionary<string, Item>>( File.ReadAllText(PathDefine.GetItemDBPath()));
        
        Console.WriteLine("Loading [Item DB] End");
    }
    
    private static void LoadBundleDB()
    {
        Console.WriteLine("Loading [Bundle DB] Start");
        
        bundleDB = JsonConvert.DeserializeObject<Dictionary<string, Bundle>>( File.ReadAllText(PathDefine.GetBundleDBPath()));
        
        Console.WriteLine("Loading [Bundle DB] End");
    }
    
    
    private static void LoadSettingDB()
    {
        Console.WriteLine("Loading [Setting DB] Start");

        if (File.Exists("Settings.json"))
        {
            settingDB = JsonConvert.DeserializeObject<SettingData>(File.ReadAllText("Settings.json"));
        }
        else
        {
            var newSetting = new SettingData();
            newSetting.EFT_Path = "Please Enter EFT Root Folder Absolute Path";
            newSetting.AssetStudioCLI_Path = "Please Enter AssetStudioCLI Root Folder Absolute Path";
            settingDB = newSetting;
            var settingJsonStr = JsonConvert.SerializeObject(newSetting);
            File.WriteAllText("Settings.json", settingJsonStr);
        }
        
        Console.WriteLine("Loading [Setting DB] End");
    }

    public static void SaveSettingDB()
    {
        Console.WriteLine("Saving [Setting DB] Start");
        var settingJsonStr = JsonConvert.SerializeObject(settingDB);
        File.WriteAllText("Settings.json", settingJsonStr);
        Console.WriteLine("Saving [Setting DB] End");
    }
}