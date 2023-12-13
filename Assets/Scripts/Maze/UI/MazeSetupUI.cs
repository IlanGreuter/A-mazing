using UnityEngine;
using UnityEngine.UI;

namespace IlanGreuter.Maze.UI
{
    public class MazeSetupUI : MonoBehaviour
    {
        [SerializeField] private MazeGrid mazeGrid;
        [SerializeField] private Slider widthSlider, heightSlider;
        [SerializeField] private SpriteRenderer sizePreview;

        /// <summary> Places the grid tiles and sets up the generation </summary>
        public void StartMazeGeneration()
        {
            sizePreview.gameObject.SetActive(false);
            mazeGrid.StartGeneration(GetDesiredSize());
        }

        /// <summary> Show a preview to visualise the maze's size </summary>
        public void PreviewSize()
        {
            Vector2Int size = GetDesiredSize();

            sizePreview.gameObject.SetActive(true);
            sizePreview.size = size;
            sizePreview.transform.position = new Vector3(size.x, size.y) * 0.5f;
        }

        private Vector2Int GetDesiredSize()
            => new((int)widthSlider.value, (int)heightSlider.value);


        /// <summary> Set the generation algortithm </summary>
        public void OnAlgorithmChange(int algorithm) 
        {
            mazeGrid.UsedGenerationAlgoritm = algorithm switch
            {
                0 => MazeGrid.Algorithms.Prim,
                1 => MazeGrid.Algorithms.Wilson,
                _ => throw new System.ArgumentException($"Algorithm {algorithm} was not found!"),
            };
        }
    }
}