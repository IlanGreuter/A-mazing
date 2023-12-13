using IlanGreuter.Maze;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IlanGreuter
{
    public class PathDrawer : MonoBehaviour
    {
        [SerializeField] private MazeGrid maze;
        [SerializeField] private LineRenderer lineRenderer;
        private Camera cam;

        private void Awake()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int hoveredTile = maze.Tilemap.WorldToCell(mousePosition);

                var path = maze.GetPath(hoveredTile).Select(vInt => maze.Tilemap.GetCellCenterWorld(vInt)).ToArray();
                lineRenderer.positionCount = path.Length;
                lineRenderer.SetPositions(path);
            }
        }
    }
}
