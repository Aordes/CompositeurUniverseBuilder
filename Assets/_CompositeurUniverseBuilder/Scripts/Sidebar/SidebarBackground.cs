///-----------------------------------------------------------------
///   Author : Soren SZABO                    
///   Date   : 30/10/2019 15:30
///-----------------------------------------------------------------

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Docaret.CompositeurUniverseBuilder.Sidebar
{

    public class SidebarBackground : MonoBehaviour {

        [SerializeField] protected Button button;
        [SerializeField] protected Sprite sprite;
        [SerializeField] protected Image image;

        protected string assetPath;
        public event Action<Sprite, string> OnSelect;

        private void OnValidate()
        {
            if (sprite != null) {
                string path = AssetDatabase.GetAssetPath(sprite);
                assetPath = path.Split(new string[] { "StreamingAssets/" }, StringSplitOptions.RemoveEmptyEntries)[1];

                if (image)
                    image.sprite = sprite;
            }
        }

        private void Start () {
            Debug.Log(assetPath);
            button.onClick.AddListener(Button_OnClick);
        }

        private void Button_OnClick()
        {
            OnSelect?.Invoke(sprite, assetPath);
        }
    }
}