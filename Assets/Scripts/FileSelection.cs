///-----------------------------------------------------------------
/// Author : Adrien Bordes
/// Date : 16/10/2019 17:47
///-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace Com.Docaret.UniverseBuilder
{
	public class FileSelection : MonoBehaviour {

        [SerializeField] private Button button;

        public ToolBar toolBar;

        private void Start()
        {
            button.onClick.AddListener(SetCurrentSelection_OnClick);
        }

        private void SetCurrentSelection_OnClick()
        {
            toolBar.CurrentSelection = button;
        }

    }
}