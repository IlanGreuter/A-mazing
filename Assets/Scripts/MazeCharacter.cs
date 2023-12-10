using IlanGreuter.Maze;
using UnityEngine;

namespace IlanGreuter
{
    public class MazeCharacter : MonoBehaviour
    {
        [SerializeField] private MazeGrid mazeGrid;
        Vector3Int currentCell;
        float moveSpeed;
        bool isMoving;

        void Update()
        {
            if (isMoving)
                Vector3.MoveTowards(transform.position, currentCell, moveSpeed * Time.deltaTime);
            //Get direction from input
        }

        private bool CanMove(Walls.Sides direction) =>
            !mazeGrid.GetTile(currentCell).Walls.HasWallInDirection(direction);
    }
}
