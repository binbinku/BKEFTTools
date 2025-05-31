namespace BKEFTTools;

public class ItemProp
{
    public string Name;
    public string ShortName;
    public string Description;

    public Dictionary<string, string> Prefab;

    public string GetPrefabPath()
    {
        if (Prefab.TryGetValue("path", out string path))
        {
            return path;
        }

        return null;
    }
    
}