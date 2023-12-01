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
            ExpandTile(currentTile);
        }

        public override int Step()
        {
            UpdateCurrent();
            ConnectTileToMaze(currentTile);
            ExpandTile(currentTile);
            return currentTile;
        }

        //Connect the tile to another tile that is already part of the maze
        private void ConnectTileToMaze(int tile)
        {
            List<(int, Walls.Sides)> neighbours = new(4);

            //Find all neighbour tiles that have already been visited
            foreach (var neighbour in GetNeighbours(tile))
                if (IsTileValid(neighbour.Item1) && maze[neighbour.Item1].IsVisited)
                    neighbours.Add(neighbour);

            //Connect a random visited neighbour
            (int index, Walls.Sides wall) = neighbours[UnityEngine.Random.Range(0, neighbours.Count)];
            maze[tile].OpenWall(wall);
            maze[tile].PreviousTile = index;
            maze[index].OpenWall(Walls.GetOpposite(wall));
            processedTiles++;
        }

        //Draws a new currentTile from the list of adjecentTiles
        private void UpdateCurrent()
        {
            int i = UnityEngine.Random.Range(0, adjecentTiles.Count);
            currentTile = adjecentTiles[i];
            maze[currentTile].IsVisited = true;
            adjecentTiles.RemoveAt(i);
        }

        //Add each neighbour to the list of tiles we can access
        private void ExpandTile(int tileIndex)
        {
            //TODO: use hashmap when inserting instead of checking if in list
            foreach (var (neighbour, _) in GetNeighbours(tileIndex))
                if (IsTileValid(neighbour) && !maze[neighbour].IsVisited && !adjecentTiles.Contains(neighbour))
                    adjecentTiles.Add(neighbour);
        }
    }
}
