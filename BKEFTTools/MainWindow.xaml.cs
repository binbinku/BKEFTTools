using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.IO;
using Path = System.IO.Path;

namespace BKEFTTools;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DBManager.EAllDataLoaded += DBManagerOnEAllDataLoaded;
    }

    private void DBManagerOnEAllDataLoaded()
    {
        SettingPage.Visibility = Visibility.Hidden;
        MainPage.Visibility = Visibility.Visible;
        MenuPageList.Children.Remove(SettingPage);
    }
}