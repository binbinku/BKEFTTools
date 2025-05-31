using System.Windows;
using System.Windows.Controls;

namespace BKEFTTools;

public partial class ItemDBCtrl : UserControl
{
    private Item curSelectedItem;
    
    public ItemDBCtrl()
    {
        InitializeComponent();
        DBManager.EAllDataLoaded += DBManagerOnEAllDataLoaded;
    }

    private void DBManagerOnEAllDataLoaded()
    {
        GenItemList();
    }


    private void GenItemList()
    {
        ItemList.Children.Clear();
        
        foreach (var itemPair in DBManager.itemDB)
        {
            var btn = new Button();
            btn.Content = itemPair.Value._name;
            btn.Height = 20;
            btn.FontSize = 14;
            btn.HorizontalContentAlignment = HorizontalAlignment.Left;
            btn.Click += (sender, args) =>
            {
                curSelectedItem = itemPair.Value;
                GenItemInfo();
            };
            ItemList.Children.Add(btn);
        }
    }

    private void GenItemInfo()
    {
        ItemName.Text = curSelectedItem._name;
        ItemType.Text = curSelectedItem._type;
        ItemProp_Name.Text = curSelectedItem._props.Name;
        ItemProp_ShortName.Text = curSelectedItem._props.ShortName;
        ItemProp_Description.Text = curSelectedItem._props.Description;
        if (curSelectedItem._props.Prefab != null && curSelectedItem._props.Prefab.ContainsKey("path"))
            ItemProp_Prefab.Text = curSelectedItem._props.Prefab["path"];
        else
            ItemProp_Prefab.Text = "NULL";
    }
}