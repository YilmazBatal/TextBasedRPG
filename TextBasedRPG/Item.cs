namespace TextBasedRPG
{
    public abstract class Item
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public Rarity Rarity { get; set; }
    }

    public enum Rarity { Common, Uncommon, Rare, Epic, Legendary }
}
