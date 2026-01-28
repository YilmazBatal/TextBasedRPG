namespace TextBasedRPG
{
    public class Material : Item
    {
        public int Quantity { get; set; }
        public int MaxStack { get; set; } = 64;
    }
}
