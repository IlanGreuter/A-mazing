using System.Collections.Generic;
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
        protected int connectedTiles;

        protected List<int> changedTiles = new();

        public int CurrentTile => currentTile;
        public int RemainingTiles =>
            maze.Length - connectedTiles;
        public bool HasFinished =>
            connectedTiles >= maze.Length;

        public MazeGeneratorBase(MazeTile[] grid, int2 mazeSize, int start, int end)
        {
            maze = grid;
            MazeSize = mazeSize;
            Start = start;
            End = end;

            currentTile = start;
        }

        /// <summary> Run one iteration of this algorithm's loop </summary>
        /// <returns> List of all indexes that were changed this step </returns>
        public abstract List<int> Step();

        /// <summary> Get a path from the start of a maze to this cell </summary>
        public List<Vector3Int> GetPath(Vector3Int cell)
        {
            if (!HasFinished || !IsTileValid(cell.ToInt2())) return new();

            //Starting from the final cell, go to the previous cell, add to list and repeat until we reach the start
            int next = ToIndex(cell);
            List<Vector3Int> path = new() { cell };
            while (IsTileValid(next) && next != Start)
            {
                next = maze[next].PreviousTile;
                path.Add(maze[next].ToVec3Int());
            }

            //The path is end to start, so we need to reverse it first.
            path.Reverse();
            return path;
        }

        /// <summary> Connect from one tile to another tile, removing the wall between </summary>
        /// <param name="connectFrom"> The tile to connect from </param>
        /// <param name="connectTo"> The tile to connect to. This is typically the previous tile in a maze </param>
        /// <param name="direction"> The direction the second tile is from the first tile </param>
        protected void ConnectTiles(int connectFrom, int connectTo, Walls.Sides direction)
        {
            maze[connectFrom].OpenWall(direction);
            maze[connectFrom].PreviousTile = connectTo;
            maze[connectTo].OpenWall(Walls.GetOpposite(direction));
        }

        /// <summary> Disconnect one tile from another tile, placing a wall between </summary>
        /// <param name="disconnectTile"> The tile to disconnect </param>
        /// <param name="disconnectFrom"> The tile to disconnect from </param>
        /// <param name="direction"> The direction the second tile is from the first tile </param>
        protected void DisconnectTiles(int disconnectTile, int disconnectFrom, Walls.Sides direction)
        {
            maze[disconnectTile].PlaceWall(direction);
            maze[disconnectTile].PreviousTile = -1;
            maze[disconnectFrom].PlaceWall(Walls.GetOpposite(direction));
        }

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

        /// <summary> Gets the direction the second tile is from the first tile.
        /// This assumes the tiles are both in the same row/column.</summary>
        protected Walls.Sides? GetDirection(int tile, int directionTo)
        {
            foreach (var (index, wall) in GetNeighbours(tile))
                if (index == directionTo)
                    return wall;
            return null;
        }

        /// <summary> Is this tile within the tilemap? </summary>
        public bool IsTileValid(int2 tilePos) =>
            IsTileValid(tilePos.y * MazeSize.x + tilePos.x);
        /// <summary> Is this tile within the tilemap? </summary>
        public bool IsTileValid(int tileIndex) =>
           tileIndex >= 0 && tileIndex < maze.Length;

        /// <summary> Get the MazeTile at this position </summary>
        public MazeTile GetTile(Vector3Int cell) =>
            GetTile(ToIndex(cell));
        /// <summary> Get the MazeTile at this index </summary>
        public MazeTile GetTile(int cellIndex) =>
            maze[cellIndex];

        /// <summary> Get the index a tile at this position would be at </summary>
        public int ToIndex(Vector3Int position) =>
            position.y * MazeSize.x + position.x;
    }
}