using UnityEngine;

namespace IlanGreuter.Maze.UI
{
    public class MazeIterationUI : MonoBehaviour
    {
        [SerializeField] private MazeGrid mazeGrid;

        /// <summary> Do this many iterations of the generator,
        /// with each iteration connecting 1 tile to the maze. </summary>
        public void GenerateForIterations(int iterations)
        {
            mazeGrid.GenerateForIterations(iterations);
        }
    }
}
