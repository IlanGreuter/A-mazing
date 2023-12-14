using UnityEngine;

namespace IlanGreuter.Maze.UI
{
    public class MazeIterationUI : MonoBehaviour
    {
        [SerializeField] private MazeGrid mazeGrid;
        [SerializeField] private MazeIterator iterator;

        /// <summary> Do this many iterations of the generator,
        /// with each iteration connecting 1 tile to the maze. </summary>
        public void GenerateForIterations(int iterations)
        {
            mazeGrid.GenerateForIterations(iterations);
        }

        /// <summary> Start automatically building the maze </summary>
        public void StartAutoIterate() =>
            iterator.StartRunning();

        /// <summary> Stop automatically building the maze </summary>
        public void StopAutoIterate() =>
            iterator.StopRunning();

        /// <summary> Set the number of iterations to run for every step </summary>
        public void SetIterationsPerStep(float iterationsPerStep) =>
            iterator.IterationsPerStep = (int)iterationsPerStep;

        /// <summary> Set the number of steps to do per second </summary>
        public void SetStepsPerSeconds(float stepsPerSecond) =>
            iterator.TimePerStep = 1 / Mathf.Max(stepsPerSecond, 1);
    }
}