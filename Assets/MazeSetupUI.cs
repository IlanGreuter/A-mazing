using IlanGreuter.Maze;
using UnityEngine;
using UnityEngine.UI;

namespace IlanGreuter
{
    public class MazeSetupUI : MonoBehaviour
    {
        [SerializeField] private MazeGrid mazeGrid;
        [SerializeField] private Slider widthSlider, heightSlider;
        [SerializeField] private SpriteRenderer sizePreview;

        public void StartMazeGeneration()
        {
            sizePreview.gameObject.SetActive(false);
            mazeGrid.StartGeneration(GetDesiredSize());
        }

        public void PreviewSize()
        {
            Vector2Int size = GetDesiredSize();

            sizePreview.gameObject.SetActive(true);
            sizePreview.size = size;
            sizePreview.transform.position = new Vector3(size.x, size.y) * 0.5f;
        }

        private Vector2Int GetDesiredSize() 
            => new((int)widthSlider.value, (int)heightSlider.value);
        
    }
}
