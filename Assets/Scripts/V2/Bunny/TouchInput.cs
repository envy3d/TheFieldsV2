using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace Bunny
{
    public class TouchInput : MonoBehaviour
    {
        public LayerMask touchInputMask;
        public List<TouchRegion> touchRegions;
        public List<UnityEvent> touchRegionFunctions;

        private BunnyController bunnyCtrl;
        private Camera camera;
        private Ray ray;
        private RaycastHit hit;

        private bool isFirstTouchOnFrame = true;

        void OnEnable()
        {
            bunnyCtrl = GetComponent<BunnyController>();
            camera = Camera.main;
            Debug.Log("Touch supported: " + Input.touchSupported);
            Debug.Log("Multitouch supported: " + Input.multiTouchEnabled);
        }

        void Update()
        {
            isFirstTouchOnFrame = true;
            if (Input.touchCount == 1)
            {
                ray = camera.ScreenPointToRay(Input.GetTouch(0).position);
                //TestTouchLocation();
            }
            /*else if (Input.GetMouseButtonDown(0))
            {
                ray = camera.ScreenPointToRay(Input.mousePosition);
                TestTouchLocation();
            }*/
        }

         

        /*private void TestTouchLocation()
        {
            Physics.Raycast(ray, out hit, touchInputMask);
            
            TouchRegion recipient = hit.transform.GetComponent<TouchRegion>();
            TouchRegion.RegionInfo recipientInfo = recipient.regionInfo;
            if (recipientInfo == TouchRegion.RegionInfo.Center)
            {
                bunnyCtrl.Dig();
            }
            else if (recipientInfo == TouchRegion.RegionInfo.Up)
            {
                bunnyCtrl.Move(0, 1, false);
            }
            else if (recipientInfo == TouchRegion.RegionInfo.Down)
            {
                bunnyCtrl.Move(0, -1, false);
            }
            else if (recipientInfo == TouchRegion.RegionInfo.Left)
            {
                bunnyCtrl.Move(-1, 0, false);
            }
            else if (recipientInfo == TouchRegion.RegionInfo.Right)
            {
                bunnyCtrl.Move(1, 0, false);
            }
        }*/

        private bool isTouchValid()
        {
            if (isFirstTouchOnFrame)
            {
                isFirstTouchOnFrame = false;

                return true;
            }

            return false;
        }

        public void CenterZoneTouched()
        {
            if (isTouchValid())
                bunnyCtrl.Dig();
        }

        public void UpZoneTouched()
        {
            if (isTouchValid())
                bunnyCtrl.Move(0, 1, false);
        }

        public void DownZoneTouched()
        {
            if (isTouchValid())
                bunnyCtrl.Move(0, -1, false);
        }

        public void LeftZoneTouched()
        {
            if (isTouchValid())
                bunnyCtrl.Move(-1, 0, false);
        }

        public void RightZoneTouched()
        {
            if (isTouchValid())
                bunnyCtrl.Move(1, 0, false);
        }
    }

    [System.Serializable]
    public class TouchRegion {
        public Vector2 v1;
        public Vector2 v2;
        public Vector2 v3;
    }
}