using UnityEngine;

namespace SGTouch.Unity
{
    public class SGTouchRayCast2D
    {
        private const float raycastDistance = 0x14;
        
        private readonly Camera _camera;
        private readonly LayerMask[] _sortinglayerMasks;
        
        private Ray _screenPointToRay;
        private readonly RaycastHit2D[] _raycastHit2Ds;
        
        public SGTouchRayCast2D(Camera camera, LayerMask[] sortinglayerMasks)
        {
            _camera = camera;
            _sortinglayerMasks = sortinglayerMasks;
            _raycastHit2Ds = new RaycastHit2D[1];
        }

        public GameObject TryGetRaycastObject(SGTouchCover touchCover)
        {
            _screenPointToRay =  _camera.ScreenPointToRay(touchCover.Position);
            var n = _sortinglayerMasks.Length;
            for (var i = 0; i < n; ++i)
            {
                if (Physics2D.GetRayIntersectionNonAlloc(_screenPointToRay, 
                        _raycastHit2Ds, raycastDistance, _sortinglayerMasks[i]) > 0)
                {
                    return _raycastHit2Ds[0].collider.gameObject;
                }    
            }

            return null;
        }
    }
}