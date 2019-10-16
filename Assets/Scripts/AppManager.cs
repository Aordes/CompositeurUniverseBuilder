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

        private static string PREVIEW_FILE_NAME = "_preview.*";

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
            //if (btnOpenUniverse)
            //btnOpenUniverse.onClick.AddListener(OnClick_OpenUniverse);

            GetProjects();
        }
        #endregion

        private void GetProjects()
        {
            DirectoryInfo info = new DirectoryInfo(compositeurFolderPath);
            DirectoryInfo[] directories = info.GetDirectories();

            for (int i = 0; i < directories.Length; i++)
            {
                AddProject(directories[i]);
                //Debug.Log(directories[i]);
            }
        }

        private void AddProject(DirectoryInfo directoryInfo)
        {
            ProjectItem instance = Instantiate(prefabProjectItem, projectGrid);
            Sprite preview = null;

            string previewPath = Path.Combine(directoryInfo.FullName, PREVIEW_FILE_NAME);
            FileInfo[] files = directoryInfo.GetFiles(PREVIEW_FILE_NAME);

            if (files.Length != 0)
            {
                Debug.Log(files[0].FullName);
                byte[] data = File.ReadAllBytes(files[0].FullName);

                Texture2D texture2D = new Texture2D(2, 2);
                texture2D.LoadImage(data);

                float minSize = Mathf.Min(texture2D.width, texture2D.height);
                preview = Sprite.Create(texture2D, new Rect((texture2D.width - minSize) / 2, (texture2D.height - minSize) / 2, minSize, minSize), Vector2.zero);
            }

            //if (File.Exists(previewPath))
            //{
            //    byte[] data = File.ReadAllBytes(previewPath);

            //    Texture2D texture2D = new Texture2D(2, 2);
            //    texture2D.LoadImage(data);

            //    float minSize = Mathf.Min(texture2D.width, texture2D.height);
            //    preview = Sprite.Create(texture2D, new Rect((texture2D.width - minSize) / 2, (texture2D.height - minSize) / 2, minSize, minSize), Vector2.zero);
            //}

            instance.Init(directoryInfo, preview);
            instance.OnClick += ProjectItem_OnClick;
        }

        private void ProjectItem_OnClick(DirectoryInfo source)
        {
            Debug.Log("Loading " + source.FullName);
            universeDirectory = source;

            DirectoryData.CurrentUniverseName = source.Name;
            DirectoryData.CurrentDirectory = source;
            DirectoryData.CurrentUniversePath = source.FullName;
            DirectoryData.CompositeurFolderPath = compositeurFolderPath;

            StartCoroutine(AsyncLoadEditor());
        }


        #region OnClick Methods
        private void OnClick_CreateUniverse()
        {
            //universeNameInputField.gameObject.SetActive(true);
        }

        private void OnEndEdit_LoadScene(string name)
        {
            universeDirectory = Directory.CreateDirectory(compositeurFolderPath + "/" + name);

            DirectoryData.CurrentUniverseName = name;
            DirectoryData.CurrentDirectory = universeDirectory;
            DirectoryData.CurrentUniversePath = universeDirectory.FullName;
            DirectoryData.CompositeurFolderPath = compositeurFolderPath;

            StartCoroutine(AsyncLoadEditor());
        }
        #endregion

        #region Coroutine
        private IEnumerator AsyncLoadEditor()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Universe Builder");

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            StopCoroutine(AsyncLoadEditor());
        }
        #endregion
    }
}
