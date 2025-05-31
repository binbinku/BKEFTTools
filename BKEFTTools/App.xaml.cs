using System.Configuration;
using System.Data;
using System.Windows;

namespace BKEFTTools;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private App()
    {
        DBManager.Init();
    }
}