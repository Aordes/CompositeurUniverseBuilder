///-----------------------------------------------------------------
/// Author : #Arien Bordes#
/// Date : #01.09.2019#
///-----------------------------------------------------------------

using SFB;
using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace Com.Docaret.CompositeurUniverseBuilder
{
    public class Ui : MonoBehaviour
    {
        #region Fields
        [Header("Containers")]
        [SerializeField] Transform bottomFolderContainer;
        [SerializeField] SlidePanelButtonContainer leftButtonContainer;
        [SerializeField] SlidePanelButtonContainer rightButtonContainer;
        [SerializeField] GameObject uiContainer;

        [Header("Prefabs")]
        [SerializeField] GameObject folderPrefab;
        [SerializeField] GameObject newFolderButtonPrefab;

        [Header("Background")]
        [SerializeField] RawImage backgroundImage;

        [Header("ToolBar")]
        [SerializeField] ToolBar toolBar;

        [Header("DialogScreen")]
        [SerializeField] DialogScreen dialogScreen;

        private string universePath;
        private string compositeurFolderPath;
        private string path;

        private Button newFolderButton;
        private DirectoryInfo universeDirectory;
        private GameObject nameInputField;

        private event Action<bool> OnExitDialog;
        private event Action<bool,string> OnRenameUniverseDialog;
        #endregion

        #region Unity Methods

        //Get Directory Path information & AddListeners to SlideButtonContainers
        private void Start()
        {
            if (DirectoryData.openExistingProject)
            {
                StartCoroutine(FileImporter.ImportUniverse(DirectoryData.CurrentDirectory, CreateFolder));
            }

            OnExitDialog += ExitToHomeMenu;
            OnRenameUniverseDialog += ChangeUniverseName;

            UpdateDirectoryData();

            leftButtonContainer.universeName.text = DirectoryData.CurrentUniverseName;
            rightButtonContainer.universeName.text = DirectoryData.CurrentUniverseName;

            CreateNewFolderButton();

            SlideButtonContainerAddListeners(leftButtonContainer);
            SlideButtonContainerAddListeners(rightButtonContainer);
        }

        #endregion

        #region UI Elements Creation Methods

        //Create new folder & Event subscriptions
        private void OnCLick_CreateFolder()
        {
            DynamicGrid dynamicGrid;
            FolderStruct folderStruct = new FolderStruct();
            folderStruct.path = universePath + "/" + "New Folder" + (FileManager.folderList.Count + 1);

            DirectoryInfo folderDirectory = Directory.CreateDirectory(folderStruct.path);
            Destroy(newFolderButton.gameObject);

            folderStruct.directory = folderDirectory;
            folderStruct.folderInstance = Instantiate(folderPrefab, bottomFolderContainer.transform);
            folderStruct.button = folderStruct.folderInstance.GetComponent<Button>();

            folderStruct.folderScript = folderStruct.button.gameObject.GetComponent<FolderButton>();
            folderStruct.folderScript.toolbar = toolBar;
            folderStruct.folderScript.SetName(Path.GetDirectoryName(folderStruct.path));
            folderStruct.folderScript.onSelected += FileManager.FolderButton_OnSelected;

            folderStruct.image = folderStruct.button.gameObject.GetComponent<RawImage>();
            folderStruct.metaData = new MetaData();

            dynamicGrid = folderStruct.folderScript.fileContainer.GetComponent<DynamicGrid>();
            folderStruct.fileList = dynamicGrid.fileList;

            FileManager.folderList.Add(folderStruct);
            dynamicGrid.currentFolderStruct = folderStruct;

            CreateNewFolderButton();
        }

        public void CreateFolder(UniverseStruct universeStruct)
        {
            Destroy(newFolderButton.gameObject);
            Debug.Log(universeStruct.folders.Count);

            for (int i = 0; i < universeStruct.folders.Count; i++)
            {
                DynamicGrid dynamicGrid;
                FolderStruct folderStruct = new FolderStruct();
                folderStruct.path = universeStruct.folders[i].fileInfo.FullName;

                DirectoryInfo folderDirectory = Directory.CreateDirectory(folderStruct.path);

                folderStruct.directory = folderDirectory;
                folderStruct.folderInstance = Instantiate(folderPrefab, bottomFolderContainer.transform);
                folderStruct.button = folderStruct.folderInstance.GetComponent<Button>();

                folderStruct.folderScript = folderStruct.button.gameObject.GetComponent<FolderButton>();
                folderStruct.folderScript.toolbar = toolBar;
                folderStruct.folderScript.SetName(folderStruct.directory.Name);
                Debug.Log(folderStruct.directory.Name);
                folderStruct.folderScript.onSelected += FileManager.FolderButton_OnSelected;

                folderStruct.image = folderStruct.button.gameObject.GetComponent<RawImage>();
                folderStruct.metaData = new MetaData();

                dynamicGrid = folderStruct.folderScript.fileContainer.GetComponent<DynamicGrid>();
                folderStruct.fileList = dynamicGrid.fileList;

                FileManager.folderList.Add(folderStruct);
                dynamicGrid.currentFolderStruct = folderStruct;
            }

            CreateNewFolderButton();
        }

        //Instanciate new Create Folder Button
        private void CreateNewFolderButton()
        {
            newFolderButton = Instantiate(newFolderButtonPrefab, bottomFolderContainer.transform).GetComponent<Button>();
            newFolderButton.onClick.AddListener(OnCLick_CreateFolder);
        }

        #endregion

        #region Callback Methods

        private void OnChangeBackground()
        {
            string[] path = StandaloneFileBrowser.OpenFilePanel("Select a Background", "", FileManager.supportedImageExtantions, false);

            if (path.Length == 0) return;

            string newPath = universePath + "/_background.png";

            if (File.Exists(newPath))
            {
                File.Delete(newPath);
            }
            File.Copy(path[0], newPath);

            WWW www = new WWW(newPath);
            backgroundImage.texture = www.texture;
        }

        public void SetBackground (Texture2D background)
        {
            backgroundImage.texture = background;
        }

        private void OnChangeUniversePreview()
        {
            string[] path = StandaloneFileBrowser.OpenFilePanel("Select a Preview", "", FileManager.supportedImageExtantions, false);

            if (path.Length == 0) return;

            string newPath = universePath + "/_preview.png";

            if (File.Exists(newPath))
            {
                File.Delete(newPath);
            }
            File.Copy(path[0], newPath);

            WWW www = new WWW(newPath);

            leftButtonContainer.universePreview.texture = www.texture;
            rightButtonContainer.universePreview.texture = www.texture;
        }

        public void SetUniversePreview (Texture2D preview)
        {
            leftButtonContainer.universePreview.texture = preview;
            rightButtonContainer.universePreview.texture = preview;
        }


        private void OnOpenCompositeur()
        {
            Process.Start("cdux://");
        }

        private void OnChangeUniverseName()
        {
            dialogScreen.DisplayInputDialog(OnRenameUniverseDialog, "Rename", "Ok", "Cancel", "New name");
        }

        private void ChangeUniverseName(bool confirm, string newName)
        {
            if (!confirm || Directory.Exists(universePath + "/" + newName) || string.IsNullOrEmpty(newName)) return;

            universeDirectory.MoveTo(compositeurFolderPath + "/" + newName);

            DirectoryData.CurrentUniverseName = newName;
            DirectoryData.CurrentUniversePath = compositeurFolderPath + "/" + newName;

            leftButtonContainer.universeName.text = newName;
            rightButtonContainer.universeName.text = newName;
            UpdateDirectoryData();

            Destroy(nameInputField);
        }

        private void OnExitToHomeMenu ()
        {
            dialogScreen.DisplayDialog(OnExitDialog, "Exit", "Ok", "Are you sure you want to exit?", "Cancel");
        }

        private void ExitToHomeMenu (bool confirm)
        {
            if (!confirm) return;
            SceneManager.LoadScene("Home Menu_Soren");
        }

        #endregion

        #region Utils

        private void UpdateDirectoryData()
        {
            universePath = DirectoryData.CurrentUniversePath;
            universeDirectory = DirectoryData.CurrentDirectory;
            compositeurFolderPath = DirectoryData.CompositeurFolderPath;
        }

        private void SlideButtonContainerAddListeners(SlidePanelButtonContainer buttonContainer)
        {
            buttonContainer.changeBackgroundButton.onClick.AddListener(OnChangeBackground);
            buttonContainer.changeUniverseNameButton.onClick.AddListener(OnChangeUniverseName);
            buttonContainer.changeUniversePreviewButton.onClick.AddListener(OnChangeUniversePreview);
            buttonContainer.openCompositeurButton.onClick.AddListener(OnOpenCompositeur);
            buttonContainer.homeButton.onClick.AddListener(OnExitToHomeMenu);
        }

        #endregion
    }
}
