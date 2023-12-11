using System.Collections.Generic;
using Unity.Mathematics;

namespace IlanGreuter.Maze.Generation
{
    public class PrimMazeGenerator : MazeGeneratorBase
    {
        private readonly List<int> adjecentTiles = new();

        public PrimMazeGenerator(MazeTile[] maze, int2 mazeSize, int start, int end) : base(maze, mazeSize, start, end)
        {
            maze[currentTile].IsVisited = true;
            connectedTiles = 1;
            ExpandTile(currentTile);
        }

        public override List<int> Step()
        {
            UpdateCurrent();
            ExpandTile(currentTile);
            ConnectTileToMaze(currentTile);
            return changedTiles;
        }

        /// <summary> Connect the tile to another tile that is already part of the maze </summary>
        private void ConnectTileToMaze(int tile)
        {
            List<(int, Walls.Sides)> neighbours = new(4);

            //Find all neighbour tiles that have already been visited
            foreach (var neighbour in GetNeighbours(tile))
                if (IsTileValid(neighbour.Item1) && maze[neighbour.Item1].IsVisited)
                    neighbours.Add(neighbour);

            //Connect a random visited neighbour
            (int index, Walls.Sides wall) = neighbours[UnityEngine.Random.Range(0, neighbours.Count)];
            ConnectTiles(tile, index, wall);
            connectedTiles++;

            changedTiles.Clear();
            changedTiles.Add(tile);
            changedTiles.Add(index);
        }

        /// <summary> Draws a new currentTile from the list of adjecentTiles </summary>
        private void UpdateCurrent()
        {
            int i = UnityEngine.Random.Range(0, adjecentTiles.Count);
            currentTile = adjecentTiles[i];
            maze[currentTile].IsVisited = true;
            adjecentTiles.RemoveAt(i);
        }

        /// <summary> Add each neighbour to the list of tiles we can access </summary>
        private void ExpandTile(int tileIndex)
        {
            //TODO: use hashmap when inserting instead of checking if in list
            foreach (var (neighbour, _) in GetNeighbours(tileIndex))
                if (IsTileValid(neighbour) && !maze[neighbour].IsVisited && !adjecentTiles.Contains(neighbour))
                    adjecentTiles.Add(neighbour);
        }
    }
}