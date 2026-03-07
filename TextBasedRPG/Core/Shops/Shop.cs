namespace TextBasedRPG.Core.Shops
{
    public class Shop
    {
        public string ID { get; init; }
        public string ShopName { get; init; }
        public List<string> Items { get; init; }

        public Shop(string id, string shopName, List<string> items)
        {
            ID = id;
            ShopName = shopName;
            Items = items;
        }
    }
}
