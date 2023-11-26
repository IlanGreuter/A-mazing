using Unity.Burst;
using Unity.Mathematics;

namespace IlanGreuter.Maze
{
    [BurstCompile]
    public static class TileMath
    {
        /// <summary> Calculate the distance in tiles between the two coordinates for square grids </summary>
        public static int CalculateSquareTileDistance(int x1, int y1, int x2, int y2, bool allowDiagonals)
        {
            int dx = math.abs(x1 - x2);
            int dy = math.abs(y1 - y2);

            if (allowDiagonals)
                return math.min(dx, dy) + math.abs(dx - dy);
            else
                return (dx + dy);
        }

        /// <summary> Calculate the distance in tiles between the two coordinates for square grids </summary>
        public static int CalculateSquareTileDistance(int2 a, int2 b, bool allowDiagonals) =>
            CalculateSquareTileDistance(a.x, a.y, b.x, b.y, allowDiagonals);

        /// <summary> Calculate the distance in tiles between the two coordinates for hex grids </summary>
        public static int CalculateHexTileDistance(int x1, int y1, int x2, int y2)
        {
            int dx = x2 - x1;
            int dy = y2 - y1;
            int adx = math.abs(dx);
            int ady = math.abs(dy);

            //Which tiles can be moved depends on how many rows you have to move and on what column you start on
            if ((dx < 0) ^ ((y1 & 1) == 1))
                adx = math.max(0, adx - (ady + 1) / 2);
            else
                adx = math.max(0, adx - (ady) / 2);

            return adx + ady;
        }

        /// <summary> Calculate the distance in tiles between the two coordinates for hex grids </summary>
        public static int CalculateHexTileDistance(int2 a, int2 b) =>
            CalculateHexTileDistance(a.x, a.y, b.x, b.y);
    }
}
