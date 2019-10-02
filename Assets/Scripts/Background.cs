///-----------------------------------------------------------------
/// Author : #Arien Bordes#
/// Date : #02.09.2019#
///-----------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Com.Docaret.UniverseBuilder
{
    public class Background : MonoBehaviour, IPointerDownHandler
    {
        public event Action OnChangeBackground;

        #region Unity Methods
        public void OnPointerDown(PointerEventData eventData)
        {
            if (Input.GetMouseButtonDown(1))
            {
                OnChangeBackground?.Invoke();
            }
        }
        #endregion
    }
}
