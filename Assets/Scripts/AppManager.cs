///-----------------------------------------------------------------
/// Author : #Arien Bordes#
/// Date : #10.09.2019#
///-----------------------------------------------------------------

using Com.Docaret.CompositeurUniverseBuilder;
using SFB;
using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
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
        [SerializeField] private Button btnReload;

        [Header("References")]
        [SerializeField] private Transform projectGrid;
        [SerializeField] private ProjectItem prefabProjectItem;
        [SerializeField] private DialogScreen dialogScreen;

        private static string PREVIEW_FILE_NAME = "_preview.*";
        private static float WAIT_TIME = 0.025f;

        private string compositeurFolderPath;
        private DirectoryInfo compositeurDirectory;
        private DirectoryInfo universeDirectory;

        private Coroutine getProjectCoroutine;

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
                btnCreateUniverse.onClick.AddListener(ButtonCreateUniverse_OnClick);
            if (btnOpenUniverse)
                btnOpenUniverse.onClick.AddListener(ButtonOpenUniverse_OnClick);
            if (btnReload)
                btnReload.onClick.AddListener(ButtonReload_OnClick);

            StartGetProjects();
        }
        #endregion

        private void StartGetProjects(bool wait = true)
        {
            if (getProjectCoroutine != null)
            {
                StopCoroutine(getProjectCoroutine);
                RemoveProjects();
            }

            getProjectCoroutine = StartCoroutine(GetProjects(wait));
        }

        private void AddProject(DirectoryInfo directoryInfo)
        {
            ProjectItem instance = Instantiate(prefabProjectItem, projectGrid);
            Sprite preview = null;

            string previewPath = Path.Combine(directoryInfo.FullName, PREVIEW_FILE_NAME);
            FileInfo[] files = directoryInfo.GetFiles(PREVIEW_FILE_NAME);

            if (files.Length != 0)
            {
                //Debug.Log(files[0].FullName);
                preview = FileImporter.CreateSquareSprite(FileImporter.ImportImage(files[0]));
            }

            instance.Init(directoryInfo, preview);
            instance.OnClick += ProjectItem_OnClick;
        }

        private void RemoveProjects()
        {
            int nChildren = projectGrid.childCount;

            for (int i = nChildren - 1; i >= 0; i--)
            {
                Destroy(projectGrid.GetChild(i).gameObject);
            }
        }

        private void ProjectItem_OnClick(DirectoryInfo source)
        {
            Debug.Log("Loading " + source.FullName);
            universeDirectory = source;

            DirectoryData.CurrentUniverseName = source.Name;
            DirectoryData.CurrentDirectory = source;
            DirectoryData.CurrentUniversePath = source.FullName;
            DirectoryData.CompositeurFolderPath = compositeurFolderPath;

            FileImporter.ImportUniverse(source);
            //StartCoroutine(AsyncLoadEditor());

        }

        private void CreateProject(string name)
        {
            universeDirectory = Directory.CreateDirectory(compositeurFolderPath + "/" + name);

            DirectoryData.CurrentUniverseName = name;
            DirectoryData.CurrentDirectory = universeDirectory;
            DirectoryData.CurrentUniversePath = universeDirectory.FullName;
            DirectoryData.CompositeurFolderPath = compositeurFolderPath;

            StartCoroutine(AsyncLoadEditor());
        }

        #region OnClick Methods
        private void ButtonCreateUniverse_OnClick()
        {
            dialogScreen.DisplayInputDialog(InputDialog_OnStatus, "Universe Name", "Create", "Cancel");
            //universeNameInputField.gameObject.SetActive(true);
        }

        private void ButtonOpenUniverse_OnClick()
        {
            string[] folder = StandaloneFileBrowser.OpenFolderPanel("Select project", compositeurFolderPath, false);

            if (folder.Length == 0)
                return;

            Debug.Log(folder[0]);
            StartCoroutine(AsyncLoadEditor());
        }

        private void ButtonReload_OnClick()
        {
            RemoveProjects();
            StartGetProjects();
        }

        private void InputDialog_OnStatus(bool state, string output)
        {
            dialogScreen.CloseScreen();

            if (state)
                CreateProject(output);
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

        private IEnumerator GetProjects(bool wait)
        {

            DirectoryInfo info = new DirectoryInfo(compositeurFolderPath);
            DirectoryInfo[] directories = info.GetDirectories();

            for (int i = 0; i < directories.Length; i++)
            {
                //Debug.Log(directories[i]);
                if (wait)
                    yield return new WaitForSeconds(WAIT_TIME);

                AddProject(directories[i]);
            }

            StopCoroutine(getProjectCoroutine);
        }
        #endregion
    }
}
