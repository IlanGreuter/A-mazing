using UnityEngine;
using UnityEngine.Tilemaps;

namespace IlanGreuter.Maze
{
    public class TilePlacer : MonoBehaviour
    {
        public Tilemap tilemap;

        [Header("Tile Sprites")]
        [SerializeField] private TileBase fullTile;
        [SerializeField] private TileBase endTile, straightTile, cornerTile, splitTile, plusTile;

        /// <summary> Fills an area with tiles. Can also place a border around the area </summary>
        public void FillBlock(Vector3Int offset, Vector3Int size)
        {
            //Place 2 corners first to expand the tilemap, then place all the tiles
            tilemap.SetTile(offset, fullTile);
            tilemap.SetTile(offset + new Vector3Int(size.x -1, size.y -1), fullTile);
            tilemap.BoxFill(offset + new Vector3Int(1, 1), fullTile, offset.x, offset.y, offset.x + size.x -1, offset.y + size.y -1);
        }

        /// <summary> Update the image and the rotation of the sprite based on where the walls are </summary>
        public void UpdateTileVisual(MazeTile mazeTile) =>
            UpdateTileVisual(mazeTile.Pos.ToVec3Int(), mazeTile.Walls);

        /// <summary> Update the image and the rotation of the sprite based on where the walls are </summary>
        public void UpdateTileVisual(Vector3Int tilePos, Walls walls)
        {
            switch (walls.CountWalls())
            {
                case 0:
                    SetTile(tilePos, plusTile, 0);
                    break;
                case 1:
                    //Rotate 90 degrees CW to account for sprite rotation, then 90 CCW times the value of the closed side
                    SetTile(tilePos, splitTile, -90 + (int)walls.GetClosedSide() * 90);
                    break;
                case 2:
                    Walls.Sides open = (Walls.Sides)walls.GetOpenSide();
                    if (walls.IsStraight(open))
                        //Rotate 90 degrees CCW times the value of the side
                        SetTile(tilePos, straightTile, (int)open * 90);
                    else
                        //Rotate 90 degrees CW for sprite rotation, then another 90 CW if the corner is CW (L shaped)
                        SetTile(tilePos, cornerTile, (walls.IsCornerCW(open) ? -180 : -90) + (int)open * 90);
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
