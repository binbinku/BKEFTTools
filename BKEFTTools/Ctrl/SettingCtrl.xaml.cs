using System.Windows;
using System.Windows.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace BKEFTTools;

public partial class SettingCtrl : UserControl
{
    public SettingCtrl()
    {
        InitializeComponent();
        LoadSettingData();
    }

    private void LoadSettingData()
    {
        EFT_PathTB.Text = DBManager.settingDB.EFT_Path;
        AssetStudioCLI_PathTB.Text = DBManager.settingDB.AssetStudioCLI_Path;
    }

    private void LoadDataBtn_OnClick(object sender, RoutedEventArgs e)
    {
        DBManager.LoadData();
    }

    private void SelectEFTPath_OnClick(object sender, RoutedEventArgs e)
    {
        var dialog = new CommonOpenFileDialog
        {
            Title = "Select EFT Root Path",
            IsFolderPicker = true,
            EnsurePathExists = true,
            AllowNonFileSystemItems = false,
            Multiselect = false
        };

        dialog.InitialDirectory = PathDefine.OutputFolder;

        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            DBManager.settingDB.EFT_Path = dialog.FileName;
            EFT_PathTB.Text = dialog.FileName;
        }
    }

    private void SelectAssetStudioCLIPath_OnClick(object sender, RoutedEventArgs e)
    {
        var dialog = new CommonOpenFileDialog
        {
            Title = "Select AssetStudioCLI Root Path",
            IsFolderPicker = true,
            EnsurePathExists = true,
            AllowNonFileSystemItems = false,
            Multiselect = false
        };

        dialog.InitialDirectory = PathDefine.OutputFolder;

        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            DBManager.settingDB.AssetStudioCLI_Path = dialog.FileName;
            AssetStudioCLI_PathTB.Text = dialog.FileName;
        }
    }
}