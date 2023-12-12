using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace IlanGreuter.Maze.Generation
{
    [System.Serializable]
    public class WilsonMazeGenerator : MazeGeneratorBase
    {
        private readonly List<int> currentWalk = new();

        public WilsonMazeGenerator(MazeTile[] maze, int2 mazeSize, int start, int end) : base(maze, mazeSize, start, end)
        {
            maze[currentTile].IsVisited = true;
        }

        public override List<int> Step()
        {
            if (currentWalk.Count == 0)
            {
                currentTile = FindStartingPoint();
                if (currentTile < 0)
                    return new();

                maze[currentTile].IsVisited = true;
                currentWalk.Add(currentTile);
                Debug.Log($"Starting walk at {currentTile}");
            }

            RandomWalk();
            return changedTiles;
        }

        /// <summary> Find the first unvisited tile to start a random walk from </summary>
        private int FindStartingPoint()
        {
            for (int i = maze.Length - connectedTiles - 1; connectedTiles < maze.Length; connectedTiles++, i--)
                if (!maze[i].IsVisited)
                    return i;

            //We have checked every single tile, which means we are done
            return -1;
        }

        /// <summary> Randomly make a path, removing loops where they are found </summary>
        private void RandomWalk()
        {
            //Pick a random from each neighbour, removing the tile we just came from
            int lastWalk = currentWalk.Count > 1 ? currentWalk[^2] : -1;
            var neighbours = GetNeighbours(currentTile).Where(i => i.Item1 != lastWalk).ToList();
            var (neighbour, wall) = neighbours[UnityEngine.Random.Range(0, neighbours.Count)];

            changedTiles.Clear();

            //If not visited, connect it to current random walk path and continue
            if (!maze[neighbour].IsVisited)
            {
                changedTiles.Add(currentTile);
                changedTiles.Add(neighbour);

                maze[neighbour].IsVisited = true;
                ConnectTiles(CurrentTile, neighbour, wall);
                currentWalk.Add(neighbour);
                currentTile = neighbour;

            }
            else
            {
                //If the neighbour is in the current random walk, AKA we made a loop
                //Backtrack to the start of the loop and continue
                int index = currentWalk.IndexOf(neighbour);
                if (index >= 0)
                {
                    currentTile = neighbour;
                    for (int i = currentWalk.Count - 1; i >= index; i--)
                    {
                        var w = GetDirection(currentWalk[i], currentTile);
                        DisconnectTiles(currentWalk[i], currentTile, (Walls.Sides)w);
                        currentTile = currentWalk[i];
                        changedTiles.Add(currentTile);

                        //Since we want to continue the walk from the last tile, we do not fully reset that one
                        if (i != index)
                        {
                            maze[currentWalk[i]].IsVisited = false;
                            currentWalk.RemoveAt(i);
                        }
                    }
                }
                //else, connect to maze and end random walk
                else 
                {
                    changedTiles.Add(currentTile);
                    changedTiles.Add(neighbour);

                    ConnectTiles(CurrentTile, neighbour, wall);
                    currentWalk.Clear();
                }
            }
        }
    }
}