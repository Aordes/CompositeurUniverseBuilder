///-----------------------------------------------------------------
/// Author : #Arien Bordes#
/// Date : #01.09.2019#
///-----------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Networking;
using SFB;
using TMPro;

namespace Com.Docaret.UniverseBuilder
{
    public class Ui : MonoBehaviour
    {
        #region Fields
        [SerializeField] Transform bottomFolderContainer;
        [SerializeField] GameObject folderPrefab;
        [SerializeField] Button newFolderButton;
        [SerializeField] GameObject newFolderButtonPrefab;
        [SerializeField] Background background;
        [SerializeField] RawImage backgroundImage;
        [SerializeField] SlidePanelButtonContainer leftButtonContainer;
        [SerializeField] SlidePanelButtonContainer rightButtonContainer;
        [SerializeField] GameObject inputFieldPrefab;
        [SerializeField] GameObject uiContainer;

        private string universePath;
        private string compositeurFolderPath;
        private DirectoryInfo universeDirectory;
        private List<FolderStruct> folderList = new List<FolderStruct>();
        private string path;
        private ExtensionFilter[] supportedImageExtantions = new[] { new ExtensionFilter("Image Files", "png", "jpg", "jpeg") };
        private GameObject nameInputField;

        private struct FolderStruct
        {
            public DirectoryInfo directory;
            public Button button;
            public FolderButton folderScript;
            public string path;
            public RawImage image;
            public GameObject folderInstance;
        }
        #endregion

        #region Unity Methods
        private void Start()
        {
            universePath = DirectoryData.CurrentUniversePath;
            universeDirectory = DirectoryData.CurrentDirectory;
            compositeurFolderPath = DirectoryData.CompositeurFolderPath;
            CreateNewFolderButton();

            leftButtonContainer.changeBackgroundButton.onClick.AddListener(Background_OnChangeBackground);
            leftButtonContainer.changeUniverseNameButton.onClick.AddListener(CreateInputField);
            leftButtonContainer.changeUniversePreviewButton.onClick.AddListener(delegate{OnChangeUniversePreview(leftButtonContainer.universePreview);});

            rightButtonContainer.changeBackgroundButton.onClick.AddListener(Background_OnChangeBackground);
            rightButtonContainer.changeUniverseNameButton.onClick.AddListener(CreateInputField);
            rightButtonContainer.changeUniversePreviewButton.onClick.AddListener(delegate{OnChangeUniversePreview(rightButtonContainer.universePreview);});
        }
        #endregion

        #region UI Elements Creation Methods
        private void OnCLick_CreateFolder()
        {
            FolderStruct folderStruct = new FolderStruct();
            folderStruct.path = universePath + "/" + "New Folder" + (folderList.Count + 1);

            DirectoryInfo folderDirectory = Directory.CreateDirectory(folderStruct.path);
            Destroy(newFolderButton.gameObject);

            folderStruct.directory = folderDirectory;
            folderStruct.folderInstance = Instantiate(folderPrefab, bottomFolderContainer.transform);
            folderStruct.button = folderStruct.folderInstance.GetComponent<Button>();
            folderStruct.folderScript = folderStruct.button.gameObject.GetComponent<FolderButton>();
            folderStruct.image = folderStruct.button.gameObject.GetComponent<RawImage>();

            folderStruct.folderScript.onEndEditFolderName += FolderButton_RenameFolder;
            folderStruct.folderScript.onChangePreview += FolderButton_OnChangePreview;
            folderStruct.folderScript.onChangeDirectoryContent += FolderButton_OnChangeDirectoryContent;
            folderStruct.folderScript.onDeleteDirectory += FolderButton_OnDeleteDirectory;

            folderList.Add(folderStruct);

            CreateNewFolderButton();
        }

        private void CreateNewFolderButton()
        {
            newFolderButton = Instantiate(newFolderButtonPrefab, bottomFolderContainer.transform).GetComponent<Button>();
            newFolderButton.onClick.AddListener(OnCLick_CreateFolder);
        }
        #endregion

        #region Delegate & Action Methods
        private void Background_OnChangeBackground()
        {
            string[] path = StandaloneFileBrowser.OpenFilePanel("Select a Background", "", supportedImageExtantions, false);

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

        private void FolderButton_OnChangePreview(Button button)
        {
            string[] path = StandaloneFileBrowser.OpenFilePanel("Select a Preview", "", supportedImageExtantions, false);

            if (path.Length == 0) return;

            FolderStruct folder = GetFolderStructFromFolderButton(button);
            string newPath = folder.directory.FullName + "/_preview.png";

            if (File.Exists(newPath))
            {
                File.Delete(newPath);
            }
            File.Copy(path[0], newPath);

            WWW www = new WWW(newPath);
            folder.image.texture = www.texture;
        }

        private void FolderButton_OnChangeDirectoryContent(Button button)
        {
            string[] path = StandaloneFileBrowser.OpenFilePanel("Select Documents", "", "", false);
            if (path.Length == 0) return;

            FolderStruct folder = GetFolderStructFromFolderButton(button);
            File.Copy(path[0], folder.directory.FullName + "/" + Path.GetFileName(path[0]));
        }

        private void FolderButton_OnDeleteDirectory(Button button)
        {
            FolderStruct folder = GetFolderStructFromFolderButton(button);

            folder.directory.Delete(true);
            Destroy(folder.folderInstance);
        }

        private void FolderButton_RenameFolder(string newName, Button button)
        {
            if (Directory.Exists(universePath + "/" + newName) || string.IsNullOrEmpty(newName)) return;

            FolderStruct folder = GetFolderStructFromFolderButton(button);
            folder.directory.MoveTo(universePath + "/" + newName);
        }
        #endregion

        #region Callback Methods
        private void OnChangeUniversePreview(RawImage preview)
        {
            string[] path = StandaloneFileBrowser.OpenFilePanel("Select a Preview", "", supportedImageExtantions, false);

            if (path.Length == 0) return;

            string newPath = universePath + "/_preview.png";

            if (File.Exists(newPath))
            {
                File.Delete(newPath);
            }
            File.Copy(path[0], newPath);

            WWW www = new WWW(newPath);
            preview.texture = www.texture;
        }

        private void OnChangeUniverseName(string newName)
        {
            if (Directory.Exists(universePath + "/" + newName) || string.IsNullOrEmpty(newName)) return;

            universeDirectory.MoveTo(compositeurFolderPath + "/" + newName);
            leftButtonContainer.universeName.text = newName;
            rightButtonContainer.universeName.text = newName;

            Destroy(nameInputField);
        }
        #endregion

        #region Utils
        private FolderStruct GetFolderStructFromFolderButton(Button button)
        {
            for (int i = 0; i < folderList.Count; i++)
            {
                if (folderList[i].button == button)
                {
                    return folderList[i];
                }
            }
            return folderList[0];
        }

        private void CreateInputField()
        {
            nameInputField = Instantiate(inputFieldPrefab, uiContainer.transform);
            nameInputField.GetComponentInChildren<TMP_InputField>().onEndEdit.AddListener(OnChangeUniverseName);
        }
        #endregion
    }
}
