using UnityEngine;

public class TutorialController : PlayerController
{
    public bool CanUseDirectionalMovement { get; set; }
    public bool CanUseForwardMovement { get; set; }
    public bool CanUseDig { get; set; }
    public bool CanUseLongJump { get; set; }


    protected override void HandlePlayerInput()
    {
        if (ReadyForAction())
        {
            if (CanUseDig && Input.GetButton("Dig"))
            {
                Dig();
            }
            else
            {
                Vector2 moveDirection = Vector2.zero;
                if (CanUseForwardMovement && Input.GetAxisRaw("Vertical") > 0.2f)
                {
                    moveDirection = new Vector2(0, Input.GetAxisRaw("Vertical"));
                }
                else if (CanUseDirectionalMovement)
                {
                    moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                }

                if (CanUseLongJump && Input.GetButton("LongJump"))
                    jumpDistance = 2;
                else
                    jumpDistance = 1;
                if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y) && Mathf.Abs(moveDirection.x) >= 0.15f)
                {
                    moveDirection.y = 0.0f;
                    moveDirection.x = (moveDirection.x > 0) ? 1 : -1;
                    Move(moveDirection, jumpDistance);
                }
                else if (Mathf.Abs(moveDirection.x) < Mathf.Abs(moveDirection.y) && Mathf.Abs(moveDirection.y) >= 0.15f)
                {
                    moveDirection.x = 0.0f;
                    moveDirection.y = (moveDirection.y > 0) ? 1 : -1;
                    Move(moveDirection, jumpDistance);
                }
            }
        }
    }

}
