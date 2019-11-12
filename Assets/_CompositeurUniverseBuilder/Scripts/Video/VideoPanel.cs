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

namespace Com.Docaret.CompositeurUniverseBuilder.Video {

    public class VideoPanel : MonoBehaviour {

        [Header("Video")]
        public VideoPlayer videoPlayer;
        [SerializeField] private RawImage imgTarget;
        [SerializeField] private VideoSlider sliderProgress;

        [Header("Toggles")]
        [SerializeField] private Toggle togglePlay;
        [SerializeField] private Image imageTogglePlay;
        [SerializeField] private Toggle toggleMute;
        [SerializeField] private Image imageToggleMute;

        [Header("Sprites")]
        [SerializeField] private Sprite spritePlay;
        [SerializeField] private Sprite spritePause;
        [SerializeField] private Sprite spriteMute;
        [SerializeField] private Sprite spriteSpeaker;

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
            togglePlay.onValueChanged.AddListener(TogglePlay_OnValueChanged);
            toggleMute.onValueChanged.AddListener(ToggleMute_OnValueChanged);

            videoPlayer.Play();

            sliderProgress.OnClick += SetModeScrub;
            SetModeUpdateInterface();
        }

        private void LateUpdate()
        {
            doAction?.Invoke();
        }

        #region UI Callbacks
        private void SliderProgress_OnValueChanged(float ratio)
        {
            videoPlayer.frame = (long)(totalFrames * ratio);
        }

        private void TogglePlay_OnValueChanged(bool isPlaying)
        {
            if (isPlaying)
            {
                imageTogglePlay.sprite = spritePlay;
                videoPlayer.Play();
            }
            else
            {
                imageTogglePlay.sprite = spritePause;
                videoPlayer.Pause();
            }
        }

        private void ToggleMute_OnValueChanged(bool value)
        {
            Debug.Log(videoPlayer.audioTrackCount);
            videoPlayer.SetDirectAudioMute(0, value);

            imageToggleMute.sprite = value ? spriteMute : spriteSpeaker;
        }
        #endregion

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