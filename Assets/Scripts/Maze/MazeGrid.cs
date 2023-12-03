using IlanGreuter.Maze.Generation;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IlanGreuter.Maze
{
    [RequireComponent(typeof(TilePlacer))]
    public class MazeGrid : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        private MazeGeneratorBase generator;
        private Vector2Int size = new(5, 5);
        [SerializeField] private Vector2Int offset;
        public Vector3Int startPoint, endPoint;

        private TilePlacer tileBuilder;

        private void Awake()
        {
            tileBuilder = GetComponent<TilePlacer>();
            StartGeneration();
            while (!generator.HasFinished)
                ProgressGeneration();
        }

        public void StartGeneration()
        {
            CreateGrid(size);
            CreateGenerator(size);
        }

        private void CreateGrid(Vector2Int size)
        {
            tileBuilder.tilemap = tilemap;
            //Offset and increase size to add a border around the maze
            tileBuilder.FillBlock(new(offset.x -1,offset.y -1), new(size.x + 2, size.y + 2));
        }

        private void CreateGenerator(Vector2Int size)
        {
            int startIndex = startPoint.x + (startPoint.y * size.x);
            int endIndex = endPoint.x + (endPoint.y * size.x);
            int2 posOffset = offset.ToInt2();

            //Instantiate the grid
            MazeTile[] maze = new MazeTile[size.x * size.y];
            for (int i = 0; i < maze.Length; i++)
            {
                int2 pos = new(i % size.x, size.y - 1 - (i / size.x));
                maze[i] = new MazeTile(pos + posOffset, i);
            }

            generator = new PrimMazeGenerator(maze, size.ToInt2(), startIndex, endIndex);
        }

        [ContextMenu("Step")]
        private void ProgressGeneration()
        {
            int toUpdate = generator.Step();
            tileBuilder.UpdateTileVisual(generator.maze[toUpdate]);
            foreach (var (n, _) in generator.GetNeighbours(toUpdate))
                tileBuilder.UpdateTileVisual(generator.maze[n]);
        }
    }
}
