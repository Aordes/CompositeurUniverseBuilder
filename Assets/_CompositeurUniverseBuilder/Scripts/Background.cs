///-----------------------------------------------------------------
/// Author : #Arien Bordes#
/// Date : #02.09.2019#
///-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace Com.Docaret.CompositeurUniverseBuilder
{
    public class Background : MonoBehaviour
    {
        public static Background Instance { get; private set; }

        [SerializeField] protected RawImage background;

        #region Unity Methods
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }
        #endregion

        public void ChangeTexture(Texture2D texture)
        {
            Debug.Log(texture);
            background.texture = texture;
        }
    }
}
