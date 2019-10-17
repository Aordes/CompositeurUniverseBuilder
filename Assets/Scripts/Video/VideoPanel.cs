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

namespace Com.Docaret.Video {

    public class VideoPanel : MonoBehaviour {

        public VideoPlayer videoPlayer;
        [SerializeField] private RawImage imgTarget;
        [SerializeField] private VideoSlider sliderProgress;

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

            sliderProgress.OnClick += SetModeScrub;
            SetModeUpdateInterface();
        }

        private void LateUpdate()
        {
            doAction?.Invoke();
        }

        private void SliderProgress_OnValueChanged(float ratio)
        {
            videoPlayer.frame = (long)(totalFrames * ratio);
        }

        public void SetModeUpdateInterface()
        {
            sliderProgress.Slider.onValueChanged.RemoveListener(SliderProgress_OnValueChanged);

            doAction = DoActionUpdateInterface;
        }

        public void SetModeScrub()
        {
            if (sliderProgress)
                sliderProgress.Slider.onValueChanged.AddListener(SliderProgress_OnValueChanged);

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
            sliderProgress.Slider.value = (float)videoPlayer.frame / totalFrames;
        }
    }
}