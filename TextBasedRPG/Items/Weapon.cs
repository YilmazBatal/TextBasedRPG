namespace TextBasedRPG
{
    public class Weapon : Item
    {
        public int WeaponATK { get; set; }
        public WeaponType WeaponType { get; set; }
        public int RequiredLevel { get; set; }
    }
}
