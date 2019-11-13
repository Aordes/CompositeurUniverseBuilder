///-----------------------------------------------------------------
/// Author : Adrien Bordes
/// Date : 16/10/2019 17:47
///-----------------------------------------------------------------

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Docaret.CompositeurUniverseBuilder {
	public class FileSelection : MonoBehaviour {

        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI fileName;
        [SerializeField] private GameObject outline;

        public ToolBar toolBar;

        private void Start()
        {
            Debug.Log("FileInstance");
            button.onClick.AddListener(SetCurrentSelection_OnClick);
        }

        private void SetCurrentSelection_OnClick()
        {
            toolBar.CurrentSelection = button;
            ShowOutline();
        }

        public void SetName(string name)
        {
            fileName.text = name;
        }

        public void ShowOutline()
        {
            outline.SetActive(true);
            FileManager.DeselectFiles(button);
        }

        public void HideOutline()
        {
            outline.SetActive(false);
        }

    }
}