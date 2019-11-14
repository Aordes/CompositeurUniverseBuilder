///-----------------------------------------------------------------
/// Author : #Arien Bordes#
/// Date : #10.09.2019#
///-----------------------------------------------------------------

using SFB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Com.Docaret.CompositeurUniverseBuilder {
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

            btnCreateUniverse.GetComponent<Animator>().SetTrigger(ProjectItem.BTN_INIT_TRIGGER);
            btnOpenUniverse.GetComponent<Animator>().SetTrigger(ProjectItem.BTN_INIT_TRIGGER);

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
                StopAllCoroutines();
                //StopCoroutine(getProjectCoroutine);
                RemoveProjects();
            }

            getProjectCoroutine = StartCoroutine(GetProjects(wait));
        }

        private void RemoveProjects()
        {
            int nChildren = projectGrid.childCount;

            for (int i = nChildren - 1; i >= 0; i--)
            {
                Destroy(projectGrid.GetChild(i).gameObject);
            }
        }

        private void OpenProject(DirectoryInfo source)
        {
            Debug.Log("Loading " + source.FullName);
            universeDirectory = source;

            DirectoryData.CurrentUniverseName = source.Name;
            DirectoryData.CurrentDirectory = source;
            DirectoryData.CurrentUniversePath = source.FullName;
            DirectoryData.CompositeurFolderPath = compositeurFolderPath;
            DirectoryData.openExistingProject = true;

            StartCoroutine(AsyncLoadEditor());
        }

        private void CreateProject(string name)
        {
            universeDirectory = Directory.CreateDirectory(compositeurFolderPath + "/" + name);

            DirectoryData.CurrentUniverseName = name;
            DirectoryData.CurrentDirectory = universeDirectory;
            DirectoryData.CurrentUniversePath = universeDirectory.FullName;
            DirectoryData.CompositeurFolderPath = compositeurFolderPath;
            DirectoryData.openExistingProject = false;

            StartCoroutine(AsyncLoadEditor());
        }

        #region OnClick Methods
        private void ButtonCreateUniverse_OnClick()
        {
            dialogScreen.DisplayInputDialog(InputDialog_OnStatus, "Universe Name", "Create", "Cancel");
        }

        private void ButtonOpenUniverse_OnClick()
        {
            string[] folders = StandaloneFileBrowser.OpenFolderPanel("Select project", compositeurFolderPath, false);

            if (folders.Length == 0)
                return;

            universeDirectory = Directory.CreateDirectory(folders[0]);
            OpenProject(universeDirectory);
            StartCoroutine(AsyncLoadEditor());
        }

        private void ButtonReload_OnClick()
        {
            RemoveProjects();
            StartGetProjects();
        }

        private void InputDialog_OnStatus(bool validate, string output)
        {
            //dialogScreen.CloseScreen();
            if (!validate)
                return;

            string folder = Path.Combine(compositeurFolderPath, output);
            Debug.Log(folder);
            if (Directory.Exists(folder))
            {
                Debug.Log("project exists");
                DirectoryInfo directory = new DirectoryInfo(folder);
                OpenProject(directory);
                return;
            }

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

            List<ProjectItem> projectItems = new List<ProjectItem>();

            for (int i = 0; i < directories.Length; i++)
            {
                ////Debug.Log(directories[i]);
                //if (wait)
                //    yield return new WaitForSeconds(WAIT_TIME);

                yield return AddProject(directories[i], projectItems);
            }

            for (int i = 0; i < projectItems.Count; i++)
            {
                projectItems[i].Show();

                if (wait)
                    yield return new WaitForSeconds(WAIT_TIME);
            }

            //StopCoroutine(getProjectCoroutine);
        }

        private IEnumerator AddProject(DirectoryInfo directoryInfo, List<ProjectItem> projectItems)
        {
            ProjectItem instance = Instantiate(prefabProjectItem, projectGrid);

            Sprite preview = null;
            yield return FileImporter.GetItemPreview(directoryInfo, PREVIEW_FILE_NAME, (output) => { preview = output; });

            instance.Init(directoryInfo, preview);
            instance.OnClick += OpenProject;

            projectItems.Add(instance);
        }
        #endregion
    }
}
