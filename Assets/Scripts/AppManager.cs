///-----------------------------------------------------------------
/// Author : #Arien Bordes#
/// Date : #10.09.2019#
///-----------------------------------------------------------------

using Com.Docaret.CompositeurUniverseBuilder;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Com.Docaret.UniverseBuilder
{
    public class AppManager : MonoBehaviour
    {
        //[SerializeField] private TMPro.TMP_InputField universeNameInputField;
        [Header("Buttons")]
        [SerializeField] private Button btnCreateUniverse;
        [SerializeField] private Button btnOpenUniverse;

        [Header("Projects")]
        [SerializeField] private Transform projectGrid;
        [SerializeField] private ProjectItem prefabProjectItem;

        private static string PREVIEW_FILE_NAME = "_preview.png";
        private static Rect IMAGE_RECT = new Rect(0, 0, 256, 256);

        private string compositeurFolderPath;
        private DirectoryInfo compositeurDirectory;
        private DirectoryInfo universeDirectory;

        #region Unity Methods
        void Start()
        {
            //universeNameInputField.gameObject.SetActive(false);
            //universeNameInputField.onEndEdit.AddListener(OnEndEdit_LoadScene);
            compositeurFolderPath = "C:/Users/" + Environment.UserName + "/Documents/Compositeur Digital UX";

            if (!Directory.Exists(compositeurFolderPath))
            {
                compositeurDirectory = Directory.CreateDirectory(compositeurFolderPath);
            }

            if (btnCreateUniverse)
                btnCreateUniverse.onClick.AddListener(OnClick_CreateUniverse);
            if (btnOpenUniverse)
            btnOpenUniverse.onClick.AddListener(OnClick_OpenUniverse);

            GetProjects();
        }
        #endregion

        private void GetProjects()
        {
            DirectoryInfo info = new DirectoryInfo(compositeurFolderPath);
            DirectoryInfo[] files = info.GetDirectories();

            for (int i = 0; i < files.Length; i++)
            {
                AddProject(files[i]);
                Debug.Log(files[i]);
            }
        }

        private void AddProject(DirectoryInfo directoryInfo)
        {
            ProjectItem instance = Instantiate(prefabProjectItem, projectGrid);
            Sprite preview = null;

            string previewPath = Path.Combine(directoryInfo.FullName, PREVIEW_FILE_NAME);

            if (File.Exists(previewPath))
            {
                byte[] data = File.ReadAllBytes(previewPath);

                Texture2D texture2D = new Texture2D(2, 2);
                texture2D.LoadImage(data);

                preview = Sprite.Create(texture2D, IMAGE_RECT, Vector2.one * 0.5f);
            }

            instance.Init(directoryInfo.FullName, directoryInfo.Name, preview);
            instance.OnClick += ProjectItem_OnClick;
        }

        private void ProjectItem_OnClick(string obj)
        {
            Debug.Log("Loading " + obj);
        }


        #region OnClick Methods
        private void OnClick_CreateUniverse()
        {
            //universeNameInputField.gameObject.SetActive(true);
        }

        private void OnClick_OpenUniverse()
        {
        
        }

        private void OnEndEdit_LoadScene(string name)
        {
            universeDirectory = Directory.CreateDirectory(compositeurFolderPath + "/" + name);

            DirectoryData.CurrentUniverseName = name;
            DirectoryData.CurrentDirectory = universeDirectory;
            DirectoryData.CurrentUniversePath = universeDirectory.FullName;
            DirectoryData.CompositeurFolderPath = compositeurFolderPath;

            StartCoroutine(LoadYourAsyncScene());
        }
        #endregion

        #region Coroutine
        private IEnumerator LoadYourAsyncScene()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Universe Builder");

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            StopCoroutine(LoadYourAsyncScene());
        }
        #endregion
    }
}
