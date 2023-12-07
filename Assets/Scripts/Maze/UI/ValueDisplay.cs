using TMPro;
using UnityEngine;

namespace IlanGreuter.Maze.UI
{
    public class ValueDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textDisplay;

        /// <summary> Set the displayed value. Will only display whole numbers </summary>
        public void SetDisplayValue(float value)
        {
            textDisplay.text = value.ToString("0");
        }

        private void Awake()
        {
            if (textDisplay == null) 
                textDisplay = GetComponent<TextMeshProUGUI>();
        }
    }
}
