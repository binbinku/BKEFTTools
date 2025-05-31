using System.Diagnostics;
using System.IO;
using System.Windows;
using BKEFTTools.Entity.EUserDB;

namespace BKEFTTools;

public class ExportManager
{
    private static List<Task> _runningTasks = new List<Task>();

    private static bool isAsync = true;



    public static void ExportItemPreset(UserWeaponBuild build)
    {
        var newPreset = new ItemPreset();
        newPreset._name = build.Name;
        newPreset._items = build.Items;
        ExportItemPreset(newPreset);
    }
    
    public static void ExportItemPreset(ItemPreset preset)
    {
        var presetName = preset._name.Replace(" ", "_");
        
        Console.WriteLine($"Exporting [Item Preset {presetName}] Start\n");

        _runningTasks.Clear();
        
        var presetOutPath = PathDefine.GetPresetOutPath(presetName);
        if (Directory.Exists(presetOutPath))
            Directory.Delete(presetOutPath, true);
        
        foreach (var presetItem in preset._items)
        {
            Console.WriteLine($"[{presetItem.slotId}] Start\n");
            
            if (DBManager.itemDB.TryGetValue(presetItem._tpl, out var item))
            {
                Console.WriteLine($"Exporting Item: {item._name}");
                
                //Create Item Out Path
                var fSlotId = String.IsNullOrEmpty(presetItem.slotId ) ? "weapon_root" : presetItem.slotId;
                var fullOutPath = PathDefine.GetPresetItemOutPath(presetName,fSlotId, item._name);
                if (Directory.Exists(fullOutPath) == false)
                {
                    Directory.CreateDirectory(fullOutPath);
                    Console.WriteLine($"CreateOutputPath: {fullOutPath}");
                }
                
                //Start UpPack
                var prefabPath = item._props.GetPrefabPath();
                if (prefabPath != null && DBManager.bundleDB.TryGetValue(prefabPath, out var bundle))
                {
                    //1.Origin Bundle
                    var fullBundlePath = PathDefine.GetBundleFullPathByPath(prefabPath);
                    if (File.Exists(fullBundlePath) == false)
                        throw new Exception();
                    Console.WriteLine($"[Main]: {fullBundlePath}");

                    if (isAsync)
                        _runningTasks.Add(Task.Run((() => { UnpackBundle(fullBundlePath, fullOutPath); })));
                    else 
                        UnpackBundle(fullBundlePath, fullOutPath);
                    
                    //2.Dep Bundle
                    foreach (var bundleDepPath in bundle.Dependencies)
                    {
                        if(bundleDepPath.Contains("animations") || bundleDepPath.Contains("additional_hands") )
                            continue;
                        
                        if (bundleDepPath.Contains("content/weapons") || bundleDepPath.Contains("content/items"))
                        {
                            var fullBundleDepPath = PathDefine.GetBundleFullPathByPath(bundleDepPath);

                            if (File.Exists(fullBundleDepPath) == false)
                                throw new Exception();
                            
                            Console.WriteLine($"[Dep]: {fullBundleDepPath}");
                            if (isAsync)
                                _runningTasks.Add(Task.Run((() => { UnpackBundle(fullBundleDepPath, fullOutPath); })));
                            else
                                UnpackBundle(fullBundleDepPath, fullOutPath);
                        }
                    }
                }
                else
                {
                    continue;
                }
            }
            else
            {
                continue;
            }
            
            Console.WriteLine($"\n[{presetItem.slotId}] End\n");
            
        }
        
        Task.WaitAll(_runningTasks.ToArray());
        
        Console.WriteLine($"Exporting [Item Preset {presetName}] End\n");

        MessageBox.Show("Export Finished!");
    }
    public static void UnpackBundle(string bundlePath, string outPath)
    {
        if(bundlePath.Contains("client_assets"))
            UnpackWeaponBuild(bundlePath, outPath, "", new List<string>() { "Animator" });
        else
            UnpackWeaponBuild(bundlePath, outPath, ".*lod0.*");
    }
    public static void UnpackWeaponBuild(string inputPath, string outputPath, string patten = "", List<string> types = null)
    {
        //StartInfo
        ProcessStartInfo processStartInfo = new ProcessStartInfo();
        processStartInfo.FileName = PathDefine.GetAssetStudioCLIPath();
        processStartInfo.CreateNoWindow = true;

        //SetArgs
        string args = $" {inputPath} {outputPath} --game Normal --silent";
        if (patten != "") args += $" --names {patten}";
        if (types != null)
        {
            args += " --types ";

            foreach (var type in types)
            {
                args += $"{type} ";
            }
        }
        processStartInfo.Arguments = args;
        
        processStartInfo.RedirectStandardOutput = true;


        //Log Args
        Console.WriteLine($"AssetStudioCLI Args: {args}");
        
        //Start Process
        Process process = new Process();
        process.StartInfo = processStartInfo;
        
        process.Start();
        
        Console.WriteLine($"[AssetStudioCLI Log]: {process.StandardOutput.ReadToEnd()}");
        
        process.WaitForExit();
    }
}