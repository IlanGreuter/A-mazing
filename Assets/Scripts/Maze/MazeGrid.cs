using IlanGreuter.Maze.Generation;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IlanGreuter.Maze
{
    [RequireComponent(typeof(TilePlacer))]
    public class MazeGrid : MonoBehaviour
    {
        public Tilemap Tilemap;
        private MazeGeneratorBase generator;
        [SerializeField] private Vector3Int startPoint, endPoint;

        private TilePlacer tileBuilder;

        private void Awake()
        {
            tileBuilder = GetComponent<TilePlacer>();
        }

        public void StartGeneration(Vector2Int size)
        {
            CreateGrid(size);
            CreateGenerator(size);
        }

        public void CreateGrid(Vector2Int size)
        {
            Tilemap.ClearAllTiles();
            tileBuilder.tilemap = Tilemap;
            //Offset and increase size to add a border around the maze
            tileBuilder.FillBlock(new(-1, -1), new(size.x + 2, size.y + 2));
        }

        private void CreateGenerator(Vector2Int size)
        {
            int startIndex = startPoint.x + (startPoint.y * size.x);
            int endIndex = endPoint.x + (endPoint.y * size.x);

            //Instantiate the grid
            MazeTile[] maze = new MazeTile[size.x * size.y];
            for (int i = 0; i < maze.Length; i++)
            {
                int2 pos = new(i % size.x, i / size.x);
                maze[i] = new MazeTile(pos, i);
            }

            generator = new WilsonMazeGenerator(maze, size.ToInt2(), startIndex, endIndex);
        }

        public void GenerateForIterations(int iterations)
        {
            for (int i = 0; (i < iterations && !generator.HasFinished) || iterations < 0; i++)
                ProgressGeneration();
        }

        public List<Vector3Int> GetPath(Vector3Int endPoint)
        {
            if (generator == null || !generator.IsTileValid(endPoint.ToInt2())) return new();
            return generator.GetPath(endPoint);
        }

        [ContextMenu("Step")]
        private void ProgressGeneration()
        {
            if (generator == null || generator.HasFinished)
                return;

            foreach (var change in generator.Step())
                tileBuilder.UpdateTileVisual(generator.maze[change]);
        }

        public MazeTile GetTile(Vector3Int cell) =>
            generator.GetTile(cell);
    }
}