using SGTouch.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace Test
{
    public class Test : MonoBehaviour
    {
        private SGTouchController _sgTouchController;

        void Start()
        {
            // Add touch collider and set layer "TouchLayer" 
            
            const int colliderLayer = 1 << 9;
            var sortingTouchLayers = new LayerMask[] { colliderLayer};
            _sgTouchController = new SGTouchController(Camera.main, sortingTouchLayers);
            _sgTouchController.SetMultiTouchEnabled(true);  //option

            AddTouchListener();
            
            //UI Open Or Closed
            var canvas = GameObject.Find("Canvas").transform;
            var openBtn = canvas.Find("Open").GetComponent<Button>();
            openBtn.onClick.AddListener(OnOpenTouch);
            var closeBtn =canvas.Find("Closed").GetComponent<Button>();    
            closeBtn.onClick.AddListener(OnCloseTouch);
        }

        private void OnOpenTouch()
        {
            _sgTouchController?.Resume();
            Debug.Log($"Open Touch");
        }

        private void OnCloseTouch()
        {
            _sgTouchController?.Pause();
            Debug.Log($"Closed Touch");
        }
        
        private void AddTouchListener()
        {
            _sgTouchController.AddTouchListener(OnTouchBegan, SGTouchPhase.Began); 
            _sgTouchController.AddTouchListener(OnTouchStationary, SGTouchPhase.Stationary);
            _sgTouchController.AddTouchListener(OnTouchEnd, SGTouchPhase.End);
        }

        private void RemoveTouchListener()
        {
            _sgTouchController.RemoveTouchListener(OnTouchBegan, SGTouchPhase.Began); 
            _sgTouchController.RemoveTouchListener(OnTouchStationary, SGTouchPhase.Stationary);
            _sgTouchController.RemoveTouchListener(OnTouchEnd, SGTouchPhase.End);
        }

        private static void OnTouchBegan(SGTouchCover touchCover)
        {
            Debug.Log($"_onTouchStart{touchCover.Position}");
        }

        private static void OnTouchStationary(SGTouchCover touchCover)
        {
            Debug.Log($"OnTouchStationary{touchCover.Position}");
        }

        private static void OnTouchEnd(SGTouchCover touchCover)
        {
            Debug.Log($"OnTouchEnd{touchCover.Position}");
        }

        private void Update()
        {
            _sgTouchController?.Update();
        }

        private void OnDestroy()
        {
            RemoveTouchListener();

           _sgTouchController?.Destroy();
        }
    }
}
