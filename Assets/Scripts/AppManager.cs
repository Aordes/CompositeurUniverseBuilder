///-----------------------------------------------------------------
/// Author : #Arien Bordes#
/// Date : #10.09.2019#
///-----------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Com.Docaret.UniverseBuilder
{
    public class AppManager : MonoBehaviour
    {
        [SerializeField] private Button createUniverseButton;
        [SerializeField] private Button openUniverseButton;
        [SerializeField] private TMPro.TMP_InputField universeNameInputField;

        private string compositeurFolderPath;
        private DirectoryInfo compositeurDirectory;
        private DirectoryInfo universeDirectory;

        #region Unity Methods
        void Start()
        {
            universeNameInputField.gameObject.SetActive(false);
            universeNameInputField.onEndEdit.AddListener(OnEndEdit_LoadScene);
            compositeurFolderPath = "C:/Users/" + Environment.UserName + "/Documents/Compositeur Digital UX";

            if (!Directory.Exists(compositeurFolderPath))
            {
                compositeurDirectory = Directory.CreateDirectory(compositeurFolderPath);
            }

            createUniverseButton.onClick.AddListener(OnClick_CreateUniverse);
            openUniverseButton.onClick.AddListener(OnClick_OpenUniverse);
        }
        #endregion

        #region OnClick Methods
        private void OnClick_CreateUniverse()
        {
            universeNameInputField.gameObject.SetActive(true);
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
