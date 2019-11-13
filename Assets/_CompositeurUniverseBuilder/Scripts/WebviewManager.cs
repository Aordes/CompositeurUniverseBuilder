///-----------------------------------------------------------------
/// Author : Soren SZABO
/// Date : 16:55
///-----------------------------------------------------------------

using System.IO;
using UnityEngine;

namespace Com.Docaret.CompositeurUniverseBuilder
{
	public class WebviewManager : MonoBehaviour {

        private struct WebviewModel
        {
            public string name;
            public string path;
        }

        private static WebviewModel currentWebview;

        public static void Create(FolderStruct folder)
        {
            currentWebview = new WebviewModel
            {
                path = folder.path
            };

            DialogScreen.Instance.DisplayInputDialog(OnName, "Create Webview", "Continue", "Cancel", "Enter name");
        }

        private static void OnName(bool complete, string output)
        {
            if (!complete)
                return;

            currentWebview.path = Path.Combine(currentWebview.path, output);
            DialogScreen.Instance.DisplayInputDialog(OnUrl, "Create Webview", "Create", "Cancel", "Enter url");
        }

        private static void OnUrl(bool complete, string output)
        {
            if (!complete)
                return;

            currentWebview.path += FileTypes.CDURL;

            FileManager.CreateFile(currentWebview.path);
            using (StreamWriter sw = File.CreateText(currentWebview.path))
            {
                sw.WriteLine(output);
            }
        }
}
}