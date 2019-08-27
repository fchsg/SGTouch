﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SGTouch.Unity
{
    public class SGTouchOverUGUI
    {
        private EventSystem _UIEventSystem;
        private PointerEventData _UIPointerEventData;
        private readonly List<RaycastResult> _UIRayCastResultCache = new List<RaycastResult>();

        public void Initial()
        {
            _UIEventSystem = EventSystem.current;
            _UIPointerEventData = new PointerEventData(_UIEventSystem);
        }
        
        public bool IsTouchedOverUGUI(Vector2 position)
        {
            if (_UIEventSystem != null && _UIPointerEventData != null)
            {
                _UIPointerEventData.position = position;
                _UIEventSystem.RaycastAll( _UIPointerEventData, _UIRayCastResultCache);
                
                return _UIRayCastResultCache.Count > 0;
            }

            return false;
        }
    }
}
