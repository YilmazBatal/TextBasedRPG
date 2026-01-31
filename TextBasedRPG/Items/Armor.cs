namespace TextBasedRPG
{
    public class Armor : Item
    {
        public int ArmorDef { get; set; }
        public int ExtraHP { get; set; } = 0;
        public int RequiredLevel { get; set; }
    }
}
