using UnityEngine;
using System.Collections;

namespace Bunny
{ 
    public class KeyboardInput : MonoBehaviour
    {
        //private Vector2 movement;
        private BunnyController bunnyCtrl;

        void OnEnable()
        {
            bunnyCtrl = GetComponent<BunnyController>();
        }

        void Update()
        {
            if (Input.GetButtonDown("Dig"))
            {
                bunnyCtrl.Dig();
            }
            
            Vector2 directionalInput = new Vector2(Mathf.RoundToInt(Input.GetAxis("Horizontal")), Mathf.RoundToInt(Input.GetAxis("Vertical")));

            if (directionalInput.x != 0 || directionalInput.y != 0)
            {
                bunnyCtrl.Move((int)directionalInput.x, (int)directionalInput.y, Input.GetButton("LongJump"));
            }


        }
    }
}