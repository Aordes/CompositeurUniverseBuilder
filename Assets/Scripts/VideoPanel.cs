///-----------------------------------------------------------------
///   Author : Soren SZABO                    
///   Date   : 17/10/2019 16:52
///-----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Com.Docaret {

    public class VideoPanel : MonoBehaviour, IPointerDownHandler {

        public VideoPlayer videoPlayer;
        [SerializeField] private RawImage imgTarget;
        [SerializeField] private Slider sliderProgress;

        private ulong totalFrames;
        private Action doAction;

        private void Start () {
            Init(videoPlayer);
        }

        public async void Init(VideoPlayer player)
        {
            await Task.Delay(1000);
            videoPlayer = player;
            imgTarget.texture = videoPlayer.texture;

            totalFrames = videoPlayer.frameCount;
            videoPlayer.Play();

            SetModeUpdateInterface();
        }

        private void Update()
        {
            doAction?.Invoke();
        }

        private void SliderProgress_OnValueChanged(float ratio)
        {
            if (doAction == DoActionUpdateInterface)
                doAction = null;

            videoPlayer.frame = (long)(totalFrames * ratio);
        }

        public void SetModeUpdateInterface()
        {
            sliderProgress.onValueChanged.RemoveListener(SliderProgress_OnValueChanged);

            doAction = DoActionUpdateInterface;
        }

        public void SetModeScrub()
        {
            if (sliderProgress)
                sliderProgress.onValueChanged.AddListener(SliderProgress_OnValueChanged);

            doAction = DoActionScrub;
        }

        private void DoActionScrub()
        {
            if (!Input.GetMouseButton(0))
            {
                videoPlayer.Play();
                SetModeUpdateInterface();
            }
        }

        private void DoActionUpdateInterface()
        {
            sliderProgress.value = (float)videoPlayer.frame / totalFrames;

            if (Input.GetMouseButtonDown(0)) {
                SetModeScrub();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            
            Debug.Log(eventData.lastPress + " Was Clicked.");
        }
    }
}