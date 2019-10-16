///-----------------------------------------------------------------
///   Author : Soren SZABO                    
///   Date   : 16/10/2019 16:56
///-----------------------------------------------------------------

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Com.Docaret.CompositeurUniverseBuilder {

    public class RawImageVideo : MonoBehaviour {
        [Header("Video")]
        [SerializeField] protected RawImage targetImage;
        [SerializeField] protected VideoPlayer videoPlayer;

        [Header("Item")]
        [SerializeField] protected Text txtName;
        [SerializeField] protected Button btnPlay;

        public event Action OnPlay;

        private void Start () {
            if (btnPlay)
                btnPlay.onClick.AddListener(ButtonPlay_OnClick);

            StartCoroutine(PreparePlayer());
        }

        private IEnumerator PreparePlayer()
        {
            videoPlayer.Prepare();

            while (!videoPlayer.isPrepared)
            {
                yield return null;
            }

            targetImage.texture = videoPlayer.texture;
            videoPlayer.Play();
        }

        private void ButtonPlay_OnClick()
        {
            OnPlay?.Invoke();
        }
    }
}