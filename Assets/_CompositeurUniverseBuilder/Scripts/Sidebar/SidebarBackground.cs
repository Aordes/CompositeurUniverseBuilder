///-----------------------------------------------------------------
///   Author : Soren SZABO                    
///   Date   : 30/10/2019 15:30
///-----------------------------------------------------------------

using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace Com.Docaret.CompositeurUniverseBuilder.Sidebar
{

    public class SidebarBackground : MonoBehaviour
    {

        [SerializeField] protected Button button;
        [SerializeField] protected Sprite sprite;
        [SerializeField] protected Image image;

        [SerializeField, HideInInspector] protected string assetPath;

        public event Action<Sprite, string> OnSelect;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (sprite != null)
            {
                //string path = AssetDatabase.GetAssetPath(sprite);
                string separator = "/";
                string[] path = (AssetDatabase.GetAssetPath(sprite).Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries));
                assetPath = "Backgrounds/" + path[path.Length - 1];

                string fullPath = Path.Combine(Application.streamingAssetsPath, assetPath);
                if (!File.Exists(fullPath))
                {
                    Debug.LogWarning("File not found in " + fullPath + ", copying");
                    File.Copy(string.Join(separator, path), fullPath);
                }

                //Debug.Log("Updating sprite " + assetPath);
                if (image)
                    image.sprite = sprite;
            }
        }
#endif

        private void Start()
        {
            button.onClick.AddListener(Button_OnClick);
        }

        private void Button_OnClick()
        {
            OnSelect?.Invoke(sprite, assetPath);
        }
    }
}