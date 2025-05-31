using System.Windows;
using System.Windows.Controls;
using BKEFTTools.Entity.EUserDB;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace BKEFTTools;

public partial class UserPresetCtrl : UserControl
{
    private UserWeaponBuild curSelectedPresetItem;
    
    public UserPresetCtrl()
    {
        InitializeComponent();
        WeaponPresetList.Children.Clear();
        DBManager.EAllDataLoaded += DBManagerOnEAllDataLoaded;    
    }

    private void DBManagerOnEAllDataLoaded()
    {
        LoadUserProfiles();
    }


    private void LoadUserProfiles()
    {
        UserProfileCB.Items.Clear();
        foreach (var keyValuePair in DBManager.userProfileDic)
        {
            var newCBItem = new ComboBoxItem();
            newCBItem.Content = keyValuePair.Key;
            UserProfileCB.Items.Add(newCBItem);
        }
    }

    private void LoadUserProfile_OnClick(object sender, RoutedEventArgs e)
    {
        if(UserProfileCB.SelectedItem == null)
            return;
        var selected = UserProfileCB.SelectedItem as ComboBoxItem;
        DBManager.LoadUserProfile(selected.Content.ToString());
        GenUserPresetList();
    }

    private void GenUserPresetList()
    {
        WeaponPresetList.Children.Clear();

        var wepPresetList = DBManager.curUserProfile.userbuilds.weaponBuilds;
        wepPresetList.Sort(((presetA, presetB) =>
        {
            return String.Compare(presetA.Name, presetB.Name, true);
        }));
        
        foreach (var wepPreset in wepPresetList)
        {
            var btn = new Button();
            btn.Content = wepPreset.Name;
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
        PresetName.Text = curSelectedPresetItem.Name;
        foreach (var presetItem in curSelectedPresetItem.Items)
        {
            var piictrl = new PresetInfoItemCtrl();

            piictrl.Preset_Item_TPL.Text = presetItem._tpl;
            piictrl.Preset_Item_SoltID.Text = "";
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