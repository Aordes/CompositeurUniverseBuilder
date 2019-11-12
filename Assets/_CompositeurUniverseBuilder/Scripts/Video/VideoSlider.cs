///-----------------------------------------------------------------
///   Author : Soren SZABO                    
///   Date   : 17/10/2019 17:59
///-----------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Com.Docaret.CompositeurUniverseBuilder.Video {

    [RequireComponent(typeof(Slider))]
    public class VideoSlider : MonoBehaviour, IPointerDownHandler {
        public Slider Slider { get; private set; }

        public event Action OnClick;

        private void Start () {
            Slider = GetComponent<Slider>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnClick?.Invoke();
        }
    }
}