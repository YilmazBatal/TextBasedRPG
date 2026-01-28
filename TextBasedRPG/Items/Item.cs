namespace TextBasedRPG
{
    public abstract class Item
    {
        public ItemType Type { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public Rarity Rarity { get; set; }
    }
}
