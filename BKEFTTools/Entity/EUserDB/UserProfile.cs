namespace BKEFTTools.Entity.EUserDB;

public class UserProfile
{
    public UserBuilds userbuilds;
}

public class UserBuilds
{
    public List<UserWeaponBuild> weaponBuilds;
}

public class UserWeaponBuild
{
    public string Name;

    public List<PresetItem> Items;
}
