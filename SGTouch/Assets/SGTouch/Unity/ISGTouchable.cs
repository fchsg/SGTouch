namespace SGTouch.Unity
{
    public interface ISGTouchable
    {
        void AddTouchListener();
        void RemoveTouchListener();
        void OnTouchStart(SGTouchCover touchCover);
        void OnTouchStationary(SGTouchCover sgTouchCover);
        void OnTouchEnd(SGTouchCover touchCover);

    }
}