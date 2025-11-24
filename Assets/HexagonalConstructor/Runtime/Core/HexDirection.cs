namespace HexDungeon
{
    public enum HexDirection
    {
        NorthEast = 0, East = 1, SouthEast = 2,
        SouthWest = 3, West = 4, NorthWest = 5
    }

    public static class HexDirectionExtensions
    {
        public static HexDirection Opposite(this HexDirection dir) => (HexDirection)(((int)dir + 3) % 6); 
        public static HexDirection Next(this HexDirection dir) => (HexDirection)(((int)dir + 1) % 6);
        public static HexDirection Previous(this HexDirection dir) => (HexDirection)(((int)dir - 1) % 6);
    }
}
