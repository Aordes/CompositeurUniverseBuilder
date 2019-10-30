///-----------------------------------------------------------------
/// Author : #Arien Bordes#
/// Date : #20.09.2019#
///-----------------------------------------------------------------

using Com.Docaret.CompositeurUniverseBuilder.Sidebar;
using SFB;
using System;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Docaret.CompositeurUniverseBuilder
{
    public class SlidePanelButtonContainer : MonoBehaviour {

        [Header("Buttons")]
        public Button openCompositeurButton;
        public Button changeBackgroundButton;
        public Button browseBackgroundButton;
        public Button changeUniversePreviewButton;
        public Button changeUniverseNameButton;
        public Button homeButton;

        [Header("Content")]
        [SerializeField] protected GameObject backgroundContainer;
        [SerializeField] protected SidebarBackground[] backgroundButtons;

        [Header("Universe Preview & Name")]
        public TextMeshProUGUI universeName;
        public RawImage universePreview;

        public Action<string> OnBackgroundChange;
        protected Background background;

        private void Start()
        {
            if (changeBackgroundButton)
                changeBackgroundButton.onClick.AddListener(ButtonChangeBackground_OnClick);
            if (browseBackgroundButton)
                browseBackgroundButton.onClick.AddListener(ButtonBrowseBackground_OnClick);

            backgroundContainer.SetActive(false);
            background = Background.Instance;

            for (int i = 0; i < backgroundButtons.Length; i++)
            {
                backgroundButtons[i].OnSelect += SlidePanelBackgroundButton_OnSelect;
            }
        }


        private void ButtonBrowseBackground_OnClick()
        {
            string[] path = StandaloneFileBrowser.OpenFilePanel("Select a Background", "", FileManager.supportedImageExtantions, false);

            if (path.Length == 0) 
                return;

            OnBackgroundChange?.Invoke(path[0]);
            StartCoroutine(FileImporter.ImportTexture(new FileInfo(path[0]), OnBackground));
        }

        private void OnBackground(Texture2D texture)
        {
            background.ChangeTexture(texture);
        }

        #region Button Callbacks
        private void SlidePanelBackgroundButton_OnSelect(Sprite sprite, string path)
        {
            path = Path.Combine(Application.streamingAssetsPath, path);
            OnBackgroundChange?.Invoke(path);
            OnBackground(sprite.texture);
        }

        private void ButtonChangeBackground_OnClick()
        {
            backgroundContainer.SetActive(!backgroundContainer.activeInHierarchy);
        }
        #endregion
    }
}