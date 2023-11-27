using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IlanGreuter.Maze
{
    public class MazeGrid : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        private MazeTile[] maze;
        private Vector2Int mazeSize;

        [Header("Tile Sprites")]
        [SerializeField] private TileBase full;
        [SerializeField] private TileBase end, straight, corner, split, plus;

        private void Awake()
        {
            CreateGrid(new(5, 5));
        }

        private void CreateGrid(Vector2Int size)
        {
            maze = new MazeTile[size.x * size.y];
            Walls w = new Walls(15);
            w.RemoveWall(Walls.Sides.Bottom);
            w.RemoveWall(Walls.Sides.Right);

            for (int i = 0; i < maze.Length; i++)
            {
                int2 pos = new(i % size.y, i / size.y);
                maze[i] = new MazeTile(pos, i);
                UpdateSpriteVisual(new(pos.x, pos.y), w);
            }
        }

        private void UpdateSpriteVisual(Vector3Int tilePos, Walls walls)
        {
            switch (walls.CountWalls())
            {
                case 0:
                    SetTile(tilePos, plus, 0);
                    break;
                case 1:
                    //Rotate 90 degrees CW to account for sprite rotation, then 90 CCW times the value of the side
                    SetTile(tilePos, split, -90 + (int)walls.GetClosedSide() * 90);
                    break;
                case 2:
                    Walls.Sides open = (Walls.Sides)walls.GetOpenSide();
                    if (walls.IsStraight(open))
                    {
                        //Rotate 90 degrees CCW times the value of the side
                        SetTile(tilePos, straight, (int)open * 90);
                    }
                    else
                    {
                        //Rotate 90 degrees CW for sprite rotation, then another 90 CW if the corner is CW (L shaped)
                        SetTile(tilePos, corner, (walls.IsCornerCW(open) ? -180 : -90) + (int)open * 90);
                    }
                    break;
                case 3:
                    //Rotate 90 degrees CCW to account for sprite rotation, then 90 CCW times the value of the side
                    SetTile(tilePos, end, 90 + (int)walls.GetOpenSide() * 90);
                    break;
                default:
                    SetTile(tilePos, full, 0);
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
