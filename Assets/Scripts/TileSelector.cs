using IlanGreuter.Maze;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IlanGreuter
{
    public class TileSelector : MonoBehaviour
    {
        [SerializeField] private MazeGrid maze;
        [SerializeField] private Tilemap tilemap;

        [SerializeField] private Vector3Int hoveredTile;
        [SerializeField] Vector3[] path;
        [SerializeField] private LineRenderer lineRenderer;
        private Camera cam;

        private void Awake()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            hoveredTile = tilemap.WorldToCell(mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                path = maze.GetPath(hoveredTile).Select(vInt => tilemap.GetCellCenterWorld(vInt)).ToArray();
                lineRenderer.positionCount = path.Length;
                lineRenderer.SetPositions(path);
            }
        }
    }
}
