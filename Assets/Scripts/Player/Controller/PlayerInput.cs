using UnityEngine;
namespace Assets.Scripts.Player
{
    public class PlayerInput
    {
        public Vector3 HandleMovement()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            return new Vector3(h, 0, v);
        }

        public bool HandleJump()
        {
            return Input.GetButtonDown("Jump");
        }
        public bool HasInput()
        {
            return Input.anyKey;
        }
        public bool HandleAttack()
        {
            return Input.GetMouseButtonDown(0);
        }
    }
}