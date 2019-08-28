using UnityEngine;

namespace SGTouch.Core.UnityTouch
{
    public enum SGTouchPhase : byte
    {
        None,
        Began,
        Stationary,
        End
    }
    
    public class SGTouchCover
    {
        private int _fingerId;
        public SGTouchPhase Phase;
        public Vector2 Position;
        private int _instanceId;
        
        public SGTouchCover()
        {
            Reset();
        }

        public void Reset()
        {
            _fingerId = -1;
            Phase = SGTouchPhase.None;
            Position = Vector2.zero;
            _instanceId = -1;
        }

        public bool IsCanUse()
        {
            return _fingerId == -1;
        }

        public bool IsTouched(int instanceId)
        {
            return _instanceId == instanceId;
        }

        public void SetFingerId(int fingerId)
        {
            _fingerId = fingerId;
        }

        public bool IsEqualFingerId(int fingerId)
        {
            return _fingerId == fingerId;
        }
        
        public void SetInstanceId(int instanceId)
        {
            _instanceId = instanceId;
        }

        public void ConvertSGTouch(Touch touch)
        {
            Phase = ConverTouchPhase(touch.phase);
            Position = touch.position;
        }
        
        public void ConvertSGTouch(TouchPhase phase, Vector2 pos)
        {
            Phase = ConverTouchPhase(phase);
            Position = pos;
        }

        private static SGTouchPhase ConverTouchPhase(TouchPhase touchPhase)
        {
            var phase = SGTouchPhase.None;
            if (touchPhase == TouchPhase.Began)
            {
                phase = SGTouchPhase.Began;
            }
            else if (touchPhase == TouchPhase.Moved || touchPhase == TouchPhase.Stationary)
            {
                phase = SGTouchPhase.Stationary;
            }
            else if (touchPhase == TouchPhase.Ended || touchPhase == TouchPhase.Canceled)
            {
                phase = SGTouchPhase.End;
            }

            return phase;
        }

    }
}


