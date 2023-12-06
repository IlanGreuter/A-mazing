using IlanGreuter.Maze;
using UnityEngine;

namespace IlanGreuter
{
    public class MazeIterationUI : MonoBehaviour
    {
        [SerializeField] private MazeGrid mazeGrid;

        public void GenerateForIterations(int iterations)
        {
            mazeGrid.GenerateForIterations(iterations);
        }
    }
}
