# SGTouch
## Simple unity touch editor & mobile
## For mobile optimization
## Easy to use
## Unity version 2018.4

# Usage
## 1.Open your projcet /Packages/manifest.json
## 2.Add "https://github.com/fchsg/SGTouch.git#upm" 

# example code

``` c#

using SGTouch.Core.Interface;
using SGTouch.Core.UnityTouch;
using UnityEngine;
using UnityEngine.UI;

namespace SGTouch.Test
{
    public class TestSGTouch : MonoBehaviour, ISGTouchListener, ISGOnTouchBegan, ISGOnTouchStationary, ISGOnTouchEnd
    {
        private SGTouchController _sgTouchController;

        void Start()
        {
            // Add touch collider and set layer "TouchLayer" 
            
            const int colliderLayer = 1 << 9;
            var sortingTouchLayers = new LayerMask[] { colliderLayer};
            _sgTouchController = new SGTouchController(Camera.main, sortingTouchLayers);
            SGTouchController.SetMultiTouchEnabled(true);  //option

            AddTouchListener();
            
            //UI Open Or Closed
            var canvas = GameObject.Find("Canvas").transform;
            var openBtn = canvas.Find("Open").GetComponent<Button>();
            openBtn.onClick.AddListener(OnOpenTouch);
            var closeBtn =canvas.Find("Closed").GetComponent<Button>();    
            closeBtn.onClick.AddListener(OnCloseTouch);
            
            Debug.Log("SGTouch Initial Success");
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
        
        public void AddTouchListener()
        {
            _sgTouchController.AddTouchListener(OnTouchBegan, SGTouchPhase.Began); 
            _sgTouchController.AddTouchListener(OnTouchStationary, SGTouchPhase.Stationary);
            _sgTouchController.AddTouchListener(OnTouchEnd, SGTouchPhase.End);
        }

        public void RemoveTouchListener()
        {
            _sgTouchController.RemoveTouchListener(OnTouchBegan, SGTouchPhase.Began); 
            _sgTouchController.RemoveTouchListener(OnTouchStationary, SGTouchPhase.Stationary);
            _sgTouchController.RemoveTouchListener(OnTouchEnd, SGTouchPhase.End);
        }

        public void OnTouchBegan(SGTouchCover sgTouchCover)
        {
            Debug.Log($"OnTouchBegan{sgTouchCover.Position}");
        }

        public void OnTouchStationary(SGTouchCover sgTouchCover)
        {
            Debug.Log($"OnTouchStationary{sgTouchCover.Position}");
        }

        public void OnTouchEnd(SGTouchCover sgTouchCover)
        {
            Debug.Log($"OnTouchEnd{sgTouchCover.Position}");
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


```


