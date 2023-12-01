using IlanGreuter.Maze.Generation;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IlanGreuter.Maze
{
    public class MazeGrid : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        private MazeGeneratorBase generator;
        public Vector2Int Size = new(5, 5);
        public Vector3Int startPoint, endPoint;

        [Header("Tile Sprites")]
        [SerializeField] private TileBase fullTile;
        [SerializeField] private TileBase endTile, straightTile, cornerTile, splitTile, plusTile;

        private void Awake()
        {
            CreateGrid(Size);
            CreateGenerator(Size);
        }

        private void CreateGrid(Vector2Int size)
        {
            //Expand the tilemap, then fill the area (plus a border around it) with blocks
            tilemap.SetTile(new(-1, -1), fullTile);
            tilemap.SetTile(new(size.x, size.y), fullTile);
            tilemap.BoxFill(new(0, 0), fullTile, -1, -1, size.x, size.y);
        }

        private void CreateGenerator(Vector2Int size)
        {
            int startIndex = startPoint.x + (startPoint.y * size.x);
            int endIndex = endPoint.x + (endPoint.y * size.x);

            //Instantiate the grid
            MazeTile[] maze = new MazeTile[size.x * size.y];
            for (int i = 0; i < maze.Length; i++)
            {
                int2 pos = new(i % size.x, size.y - 1 - (i / size.x));
                maze[i] = new MazeTile(pos, i);
            }

            generator = new PrimMazeGenerator(maze, size.ToInt2(), startIndex, endIndex);
        }

        [ContextMenu("Step")]
        private void ProgressGeneration()
        {
            int toUpdate = generator.Step();
            UpdateTileVisual(generator.maze[toUpdate]);
            foreach (var (n, _) in generator.GetNeighbours(toUpdate))
                UpdateTileVisual(generator.maze[n]);
        }

        private void UpdateTileVisual(MazeTile mazeTile) =>
            UpdateTileVisual(mazeTile.Pos.ToVec3Int(), mazeTile.Walls);

        private void UpdateTileVisual(Vector3Int tilePos, Walls walls)
        {
            switch (walls.CountWalls())
            {
                case 0:
                    SetTile(tilePos, plusTile, 0);
                    break;
                case 1:
                    //Rotate 90 degrees CW to account for sprite rotation, then 90 CCW times the value of the side
                    SetTile(tilePos, splitTile, -90 + (int)walls.GetClosedSide() * 90);
                    break;
                case 2:
                    Walls.Sides open = (Walls.Sides)walls.GetOpenSide();
                    if (walls.IsStraight(open))
                    {
                        //Rotate 90 degrees CCW times the value of the side
                        SetTile(tilePos, straightTile, (int)open * 90);
                    }
                    else
                    {
                        //Rotate 90 degrees CW for sprite rotation, then another 90 CW if the corner is CW (L shaped)
                        SetTile(tilePos, cornerTile, (walls.IsCornerCW(open) ? -180 : -90) + (int)open * 90);
                    }
                    break;
                case 3:
                    //Rotate 90 degrees CCW to account for sprite rotation, then 90 CCW times the value of the side
                    SetTile(tilePos, endTile, 90 + (int)walls.GetOpenSide() * 90);
                    break;
                default:
                    SetTile(tilePos, fullTile, 0);
                    break;
            }
        }

        private void SetTile(Vector3Int tilePos, TileBase tile, float rotation)
        {
            Matrix4x4 matrix = Matrix4x4.identity;
            matrix.SetTRS(Vector3.zero, Quaternion.AngleAxis(rotation, Vector3.forward), Vector3.one);
            tilemap.SetTile(new(tilePos, tile, Color.white, matrix), false);
        }
    }
}
