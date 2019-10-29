///-----------------------------------------------------------------
/// Author : #Arien Bordes#
/// Date : #20.09.2019#
///-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;

namespace Com.Docaret.CompositeurUniverseBuilder
{
	public class SlidePanelButtonContainer : MonoBehaviour {

        [Header("Buttons")]
        public Button openCompositeurButton;
        public Button changeBackgroundButton;
        public Button changeUniversePreviewButton;
        public Button changeUniverseNameButton;
        public Button homeButton;

        [Header("Universe Preview & Name")]
        public TextMeshProUGUI universeName;
        public RawImage universePreview;

    }
}