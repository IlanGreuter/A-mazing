using IlanGreuter.Maze.Generation;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IlanGreuter.Maze
{
    [RequireComponent(typeof(TilePlacer))]
    public class MazeGrid : MonoBehaviour
    {
        public Tilemap Tilemap;
        private MazeGeneratorBase generator;
        [SerializeField] private Vector3Int startPoint;
        public Algorithms UsedGenerationAlgoritm;

        private TilePlacer tileBuilder;

        private void Awake()
        {
            tileBuilder = GetComponent<TilePlacer>();
        }

        /// <summary> Initializes the grid and the generation </summary>
        public void StartGeneration(Vector2Int size)
        {
            CreateGrid(size);
            CreateGenerator(size);
        }

        /// <summary> Initializes the grid </summary>
        private void CreateGrid(Vector2Int size)
        {
            Tilemap.ClearAllTiles();
            tileBuilder.tilemap = Tilemap;
            //Offset and increase size to add a border around the maze
            tileBuilder.FillBlock(new(-1, -1), new(size.x + 2, size.y + 2));
        }

        private void CreateGenerator(Vector2Int size)
        {
            int startIndex = startPoint.x + (startPoint.y * size.x);

            switch (UsedGenerationAlgoritm)
            {
                case Algorithms.Prim:
                    generator = new PrimMazeGenerator(size.ToInt2(), startIndex);
                    break;
                case Algorithms.Wilson:
                    generator = new WilsonMazeGenerator(size.ToInt2(), startIndex);
                    break;
                default:
                    throw new System.NotImplementedException();
            }
        }

        /// <summary> Run this many iterations of the maze generation algorithm </summary>
        public void GenerateForIterations(int iterations)
        {
            for (int i = 0; (i < iterations || iterations < 0) && !generator.HasFinished; i++)
                ProgressGeneration();
        }

        /// <summary> Get the path from the start of the maze to this position </summary>
        public List<Vector3Int> GetPath(Vector3Int endPoint)
        {
            if (generator == null || !generator.IsTileValid(endPoint.ToInt2()))
                return new();

            return generator.GetPath(endPoint);
        }

        /// <summary> Do one iteration of the maze generator, and update the tiles </summary>
        [ContextMenu("Step")]
        private void ProgressGeneration()
        {
            if (generator == null || generator.HasFinished)
                return;

            foreach (var change in generator.Step())
                tileBuilder.UpdateTileVisual(generator.maze[change]);
        }

        /// <summary> Get the tile data at this position </summary>
        public MazeTile GetTile(Vector3Int cell) =>
            generator.GetTile(cell);

        public enum Algorithms
        {
            Undefined,
            Prim,
            Wilson
        }
    }
}