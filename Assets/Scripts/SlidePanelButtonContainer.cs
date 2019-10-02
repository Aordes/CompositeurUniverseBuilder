///-----------------------------------------------------------------
/// Author : #Arien Bordes#
/// Date : #20.09.2019#
///-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;

namespace Com.Docaret.UniverseBuilder
{
	public class SlidePanelButtonContainer : MonoBehaviour {

        public Button openCompositeurButton;
        public Button changeBackgroundButton;
        public Button changeUniversePreviewButton;
        public Button changeUniverseNameButton;

        public TextMeshProUGUI universeName;
        public RawImage universePreview;

        private void Start()
        {
            openCompositeurButton.onClick.AddListener(OpenCompositeur);
        }

        private void OpenCompositeur()
        {
            Process.Start("cdux://");
        }

    }
}