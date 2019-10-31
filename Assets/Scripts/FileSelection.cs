///-----------------------------------------------------------------
/// Author : Adrien Bordes
/// Date : 16/10/2019 17:47
///-----------------------------------------------------------------

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Docaret.CompositeurUniverseBuilder
{
	public class FileSelection : MonoBehaviour {

        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI fileName;

        public ToolBar toolBar;

        private void Start()
        {
            Debug.Log("FileInstance");
            button.onClick.AddListener(SetCurrentSelection_OnClick);
        }

        private void SetCurrentSelection_OnClick()
        {
            toolBar.CurrentSelection = button;
        }

        public void SetName(string name)
        {
            fileName.text = name;
        }

    }
}