using System.Collections.Generic;
using Unity.Mathematics;

namespace IlanGreuter.Maze.Generation
{
    public abstract class MazeGeneratorBase
    {
        public readonly MazeTile[] maze;
        public readonly int2 MazeSize;

        internal readonly int Start, End;
        internal int currentTile;
        internal int processedTiles;

        /// <summary> Has the algorithm finished completely? </summary>
        public bool HasFinished =>
            processedTiles == maze.Length;

        public MazeGeneratorBase(MazeTile[] grid, int2 mazeSize, int start, int end)
        {
            maze = grid;
            MazeSize = mazeSize;
            Start = start;
            End = end;
        }

        /// <summary> Run one iteration of this algorithm's loop </summary>
        public abstract void Step();

        /// <summary> Is this tile within the tilemap? </summary>
        public bool IsTileValid(int2 tilePos) =>
            IsTileValid(tilePos.y * MazeSize.x + tilePos.x);
        /// <summary> Is this tile within the tilemap? </summary>
        public bool IsTileValid(int tileIndex) =>
           tileIndex > 0 && tileIndex < maze.Length;

        // Since we use a 1D array to represent a 2D grid,
        // We use the following logic to move through the grid
        // Moving horizontally requires adding -1 or 1 to move left or right
        // Moving vertically requres adding -w or w to move up or down, where w is the width of the 2d grid.
        /// <summary> Get a list of all neighbours of the tile that would be on the grid </summary>
        internal IEnumerable<(int, Walls.Sides)> GetNeighbours(int tile)
        {
            if (tile % MazeSize.x == 0) //If not first column
                yield return (tile - 1, Walls.Sides.Left);
            if (tile % MazeSize.x == (MazeSize.x - 1)) //If not last Column
                yield return (tile + 1, Walls.Sides.Right);

            if (tile >= MazeSize.x) //If not first row
                yield return (tile - MazeSize.x, Walls.Sides.Top);
            if (tile < (maze.Length - MazeSize.x)) //If not last row
                yield return (tile + MazeSize.x, Walls.Sides.Bottom);
        }
    }
}
