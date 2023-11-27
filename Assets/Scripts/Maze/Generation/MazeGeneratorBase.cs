using Unity.Mathematics;

namespace IlanGreuter.Maze.Generation
{
    public abstract class MazeGeneratorBase
    {
        public MazeTile[] maze;
        public int2 mazeSize;

        internal int2 start, end;
        internal int2 currentTile;

        public MazeGeneratorBase(MazeTile[] maze, int2 mazeSize, int2 start, int2 end)
        {
            this.maze = maze;
            this.mazeSize = mazeSize;
            this.start = start;
            this.end = end;

            currentTile = start;
        }

        public void Step()
        {
        }

        /// <summary> Is this tile within the tilemap? </summary>
        public bool IsTileValid(int2 tilePos) =>
            IsTileValid(tilePos.y * mazeSize.x + tilePos.x);
        /// <summary> Is this tile within the tilemap? </summary>
        public bool IsTileValid(int tileIndex) =>
           tileIndex > 0 && tileIndex < maze.Length;
    }
}
