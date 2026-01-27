namespace TextBasedRPG
{
    public class Weapon : Item
    {
        public int weaponATK { get; set; }
        public WeaponType weaponType { get; set; }
        public int requiredLevel { get; set; }
    }
}
