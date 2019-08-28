using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SGTouch.Core.UnityTouch
{
    public class SGTouchOverUGUI
    {
        private EventSystem _UIEventSystem;
        private PointerEventData _UIPointerEventData;
        private readonly List<RaycastResult> _UIRayCastResultCache = new List<RaycastResult>();
        private bool _isValid;

        public void Initial()
        {
            _UIEventSystem = EventSystem.current;
            _UIPointerEventData = new PointerEventData(_UIEventSystem);
            _isValid = _UIEventSystem != null && _UIPointerEventData != null;
        }
        
        public bool IsTouchedOverUGUI(Vector2 position)
        {
            if (_isValid)
            {
                _UIPointerEventData.position = position;
                _UIEventSystem.RaycastAll( _UIPointerEventData, _UIRayCastResultCache);
                
                return _UIRayCastResultCache.Count > 0;
            }

            return false;
        }
    }
}
