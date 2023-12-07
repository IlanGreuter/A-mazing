using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace IlanGreuter.Maze.Generation
{
    public abstract class MazeGeneratorBase
    {
        public readonly MazeTile[] maze;
        public readonly int2 MazeSize;

        protected readonly int Start, End;
        protected int currentTile;
        protected int processedTiles;

        public int CurrentTile => currentTile;
        public int RemainingTiles =>
            maze.Length - processedTiles;
        public bool HasFinished =>
            processedTiles == maze.Length;

        public MazeGeneratorBase(MazeTile[] grid, int2 mazeSize, int start, int end)
        {
            maze = grid;
            MazeSize = mazeSize;
            Start = start;
            End = end;

            currentTile = start;
        }

        /// <summary> Run one iteration of this algorithm's loop </summary>
        /// <returns> Tuple of 2 indexes. The first is of the tile that was last connected,
        /// the second is of the tile the first was conected to. </returns>
        public abstract (int, int) Step();

        /// <summary> Is this tile within the tilemap? </summary>
        public bool IsTileValid(int2 tilePos) =>
            IsTileValid(tilePos.y * MazeSize.x + tilePos.x);
        /// <summary> Is this tile within the tilemap? </summary>
        public bool IsTileValid(int tileIndex) =>
           tileIndex >= 0 && tileIndex < maze.Length;
        
        /// <summary> Get a list of all neighbours of the tile that would be on the grid </summary>
        public IEnumerable<(int, Walls.Sides)> GetNeighbours(int tile)
        {
            if (tile % MazeSize.x != 0) //If not first column
                yield return (tile - 1, Walls.Sides.Left);
            if (tile % MazeSize.x != (MazeSize.x - 1)) //If not last Column
                yield return (tile + 1, Walls.Sides.Right);

            if (tile >= MazeSize.x) //If not first row
                yield return (tile - MazeSize.x, Walls.Sides.Bottom);
            if (tile < (maze.Length - MazeSize.x)) //If not last row
                yield return (tile + MazeSize.x, Walls.Sides.Top);
        }

        public List<Vector3Int> GetPath(Vector3Int target)
        {
            if (!HasFinished || !IsTileValid(target.ToInt2())) return new();

            int next = ToIndex(target);
            List<Vector3Int> path = new() { target };
            while (IsTileValid(next) && next != Start)
            {
                next = maze[next].PreviousTile;
                path.Add(maze[next].ToVec3Int());
            }

            path.Reverse();
            return path;
        }

        private int ToIndex(Vector3Int position) =>
            position.y * MazeSize.x + position.x;
    }
}
