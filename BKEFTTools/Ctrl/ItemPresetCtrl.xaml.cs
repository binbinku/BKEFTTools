using System.Windows;
using System.Windows.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace BKEFTTools;

public partial class ItemPresetCtrl : UserControl
{
    private ItemPreset curSelectedPresetItem;
    
    public ItemPresetCtrl()
    {
        InitializeComponent();
        DBManager.EAllDataLoaded += DBManagerOnEAllDataLoaded;
    }

    private void DBManagerOnEAllDataLoaded()
    {
        GenWeaponPresetList();
    }

    private void GenWeaponPresetList()
    {
        WeaponPresetList.Children.Clear();

        var wepPresetList = new List<ItemPreset>(DBManager.globalDB.ItemPresets.Values);
        wepPresetList.Sort(((presetA, presetB) =>
        {
            return String.Compare(presetA._name, presetB._name, true);
        }));
        
        foreach (var wepPreset in wepPresetList)
        {
            if(wepPreset._name.Contains("Helmet") ||
               wepPreset._name.Contains("Body armor") ||
               wepPreset._name.Contains("Vest"))
                continue;
            
            var btn = new Button();
            btn.Content = wepPreset._name;
            btn.Height = 20;
            btn.FontSize = 14;
            btn.HorizontalContentAlignment = HorizontalAlignment.Left;

            btn.Click += (sender, args) =>
            {
                curSelectedPresetItem = wepPreset;
                GenPresetInfo();
            };
            
            WeaponPresetList.Children.Add(btn);
        }
    }

    private void GenPresetInfo()
    {
        PresetInfoList.Children.Clear();
        PresetName.Text = curSelectedPresetItem._name;
        foreach (var presetItem in curSelectedPresetItem._items)
        {
            var piictrl = new PresetInfoItemCtrl();

            piictrl.Preset_Item_TPL.Text = presetItem._tpl;
            piictrl.Preset_Item_SoltID.Text = presetItem.slotId;
            if (DBManager.itemDB.TryGetValue(presetItem._tpl, out var item))
            {
                piictrl.Preset_Item_Name.Text = item._name;
            }
            else
            {
                piictrl.Preset_Item_Name.Text = "Empty";
            }
            PresetInfoList.Children.Add(piictrl);
        }
        
    }

    private void SelectOutFolder_OnClick(object sender, RoutedEventArgs e)
    {
        // 创建 CommonOpenFileDialog
        var dialog = new CommonOpenFileDialog
        {
            Title = "选择输出文件夹",
            IsFolderPicker = true,
            EnsurePathExists = true,
            AllowNonFileSystemItems = false,
            Multiselect = false
        };

        dialog.InitialDirectory = PathDefine.OutputFolder;

        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            PathDefine.SetOutputFolder(dialog.FileName);
            OutputFolderTB.Text = dialog.FileName;
        }
    }

    private void ExportPreset_OnClick(object sender, RoutedEventArgs e)
    {
        if (curSelectedPresetItem == null)
        {
            MessageBox.Show("Please select a preset");
            return;
        }
        ExportManager.ExportItemPreset(curSelectedPresetItem);
    }
}