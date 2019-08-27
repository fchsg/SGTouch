using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SGTouch.Core.UnityTouch
{
    public class SGTouchController
    {
        private const int MAX_TOUCH_COUNT = 10;

        public delegate void OnTouchDelegate(SGTouchCover sgTouchCover);
        
        private event OnTouchDelegate _onTouchBegan;
        private event OnTouchDelegate _onTouchStationary;
        private event OnTouchDelegate _onTouchEnd;
         
        private readonly SGTouchCover[] _touchCovers;
        private readonly SGTouchOverUGUI _touchOverUGUI;
        private readonly SGTouchRayCast2D _touchRaycast2D;
        private readonly string _chainClickName;
        
        private bool _isPause;
        
        public SGTouchController(Camera camera, LayerMask[] sortinglayerMasks, string chainClickName = "")
        {
            _touchOverUGUI = new SGTouchOverUGUI();
            _touchOverUGUI.Initial();
            
            _touchRaycast2D = new SGTouchRayCast2D(camera, sortinglayerMasks);

            _touchCovers = new SGTouchCover[MAX_TOUCH_COUNT];
            for (var i = 0; i < MAX_TOUCH_COUNT; ++i)
            {
                _touchCovers[i] = new SGTouchCover();
            }

            _isPause = false;
            _chainClickName = chainClickName;
        }

        public static void SetMultiTouchEnabled(bool enabled)
        {
            Input.multiTouchEnabled = enabled;
        }
        
        public void AddTouchListener(OnTouchDelegate onTouchDelegate, SGTouchPhase phase)
        {
            switch (phase)
            {
                case SGTouchPhase.Began:
                {
                    _onTouchBegan += onTouchDelegate;
                }
                    break;
                case SGTouchPhase.Stationary:
                {
                    _onTouchStationary += onTouchDelegate;
                }
                    break;
                case SGTouchPhase.End:
                {
                    _onTouchEnd += onTouchDelegate;
                }
                    break;
            }
        }

        public void RemoveTouchListener(OnTouchDelegate onTouchDelegate, SGTouchPhase phase)
        {
            switch (phase)
            {
                case SGTouchPhase.Began:
                {
                    if(_onTouchBegan != null)
                        _onTouchBegan -= onTouchDelegate;
                }
                    break;
                case SGTouchPhase.Stationary:
                {
                    if(_onTouchStationary != null)
                        _onTouchStationary -= onTouchDelegate;
                }
                    break;
                case SGTouchPhase.End:
                {  
                    if(_onTouchEnd != null)
                        _onTouchEnd -= onTouchDelegate;
                }
                    break;
            }
        }
        
        public void Resume()
        {
            _isPause = false;
        }

        public void Pause()
        {
            _isPause = true;
        }

        public void Destroy()
        {
            _onTouchBegan = null;
            _onTouchStationary = null;
            _onTouchEnd = null;
            
            for (var i = 0; i < MAX_TOUCH_COUNT; ++i)
            {
                _touchCovers[i].Reset();
            }
        }
        
        public void Update()
        {
            if (!_isPause)
            {
#if (((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR))
            UpdateInputTouch();
#else
            UpdateTouchMouse();
#endif
            }
        }

        private void UpdateInputTouch()
        {
            if (Input.touchCount > 0)
            {
                var count = Mathf.Min(Input.touchCount, MAX_TOUCH_COUNT);
                for (var i = 0; i < count; i++)
                {
                    var touch = Input.GetTouch(i);
                    if (touch.phase == TouchPhase.Began)
                    {
                        var newTouchCover = GetCanUseTouchCover();
                        newTouchCover?.SetFingerId(touch.fingerId);
                    }

                    var touchCover = GetTouchCoverByFingerId(touch.fingerId);
                    if (touchCover != null)
                    {
                        touchCover.ConvertSGTouch(touch);
                        ProcessTouchCover(touchCover);
                    }
                }    
            }
        }
        
        private SGTouchCover GetCanUseTouchCover()
        {
            for (var i = 0; i < MAX_TOUCH_COUNT; ++i)
            {
                if (_touchCovers[i].IsCanUse())
                {
                   return _touchCovers[i];
                }
            }

            return null;
        }

        private SGTouchCover GetTouchCoverByFingerId(int fingerId)
        {
            for (var i = 0; i < MAX_TOUCH_COUNT; ++i)
            {
                if (_touchCovers[i].IsEqualFingerId(fingerId))
                {
                    return _touchCovers[i];
                }
            }

            return null;
        }
        
        private void UpdateTouchMouse()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var touchCover = GetCanUseTouchCover();
                touchCover.ConvertSGTouch(TouchPhase.Began, Input.mousePosition);
                touchCover.SetFingerId(0);
            }
            else if (Input.GetMouseButton(0))
            {
                var touchCover = GetTouchCoverByFingerId(0);
                touchCover?.ConvertSGTouch(TouchPhase.Stationary, Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                var touchCover = GetTouchCoverByFingerId(0);
                touchCover?.ConvertSGTouch(TouchPhase.Ended, Input.mousePosition);
            }

            var sgTouchCover = GetTouchCoverByFingerId(0);
            if (sgTouchCover != null && sgTouchCover.Phase != SGTouchPhase.None)
            {
                ProcessTouchCover(sgTouchCover);
            }
        }

        private void ProcessTouchCover(SGTouchCover touchCover)
        {
            if (_touchOverUGUI.IsTouchedOverUGUI(touchCover.Position))
            {
                touchCover.Reset();
                return;    
            }

            var pickedObj = _touchRaycast2D.TryGetRaycastObject(touchCover);
            if (pickedObj != null)
            {
                switch (touchCover.Phase)
                {
                    case SGTouchPhase.Began:
                    {
                        touchCover.SetInstanceId(pickedObj.GetInstanceID());
                        _onTouchBegan?.Invoke(touchCover);
                    }
                        break;
                    case SGTouchPhase.Stationary:
                    {
                        if (touchCover.IsTouched(pickedObj.GetInstanceID()) || 
                            IsPolygonCollider(pickedObj,_chainClickName)) // 按住连续点击
                        {
                            _onTouchStationary?.Invoke(touchCover);
                        }
                        else
                        {
                            TouchEnd(touchCover);
                        }
                    }
                        break;
                    case SGTouchPhase.End:
                    {
                        TouchEnd(touchCover);
                    }
                        break;
                }
            }
            else
            {
                TouchEnd(touchCover);
            }
        }

        private void TouchEnd(SGTouchCover touchCover)
        {
            _onTouchEnd?.Invoke(touchCover);
            touchCover.Reset();
        }


        private static bool IsPolygonCollider(Object go, string name)
        {
           return string.Compare(go.name, name, StringComparison.Ordinal) == 0;
        }

    }
}

