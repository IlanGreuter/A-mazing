using System.Collections;
using UnityEngine;

namespace IlanGreuter.Maze
{
    public class MazeIterator : MonoBehaviour
    {
        public float TimePerStep;
        public int IterationsPerStep;

        private Coroutine coroutine;
        [SerializeField] private MazeGrid mazeGrid;

        [ContextMenu("Start")]
        public void StartRunning()
        {
            coroutine = StartCoroutine(RunIterations());
        }

        [ContextMenu("Stop")]
        public void StopRunning()
        {
            StopCoroutine(coroutine);
        }

        private IEnumerator RunIterations()
        {
            while (!mazeGrid.GenerateForIterations(IterationsPerStep))
                yield return new WaitForSeconds(TimePerStep);
        }
    }
}
