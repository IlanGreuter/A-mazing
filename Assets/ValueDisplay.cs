using TMPro;
using UnityEngine;

namespace IlanGreuter.UI
{

    public class ValueDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textDisplay;

        public void SetDisplayValue(float value)
        {
            textDisplay.text = value.ToString();
        }

        private void Awake()
        {
            if (textDisplay == null) 
                textDisplay = GetComponent<TextMeshProUGUI>();
        }
    }
}
