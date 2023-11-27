using UnityEngine;
using UnityEngine.Tilemaps;

namespace IlanGreuter
{
    public class TileSelector : MonoBehaviour
    {
        [SerializeField] private TileBase tile;
        [SerializeField] private Tilemap tilemap;

        [SerializeField] private Vector3Int hoveredTile;
        private Camera cam;

        public bool b;
        private void Awake()
        {
            if (b)
            {
                for (Vector3Int pos = new(-5, -5, 0); pos.y != 5; pos.y++)
                {
                    for (pos.x = -5; pos.x < 5; pos.x += 1)
                    {
                        tilemap.SetTile(pos, tile);
                        tilemap.SetColor(pos, ((pos.x) & 1) > 0 ? Color.gray : Color.white);
                    }
                }
            }

            cam = Camera.main;
        }

        private void Update()
        {
            Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            hoveredTile = tilemap.WorldToCell(mousePosition);
        }

        private void SetTileWalls(Vector3Int pos, int walls)
        {
            //Up & down = I
            //Left & Right = I rot90
            //All 4 = +
            //


        }
    }
}
