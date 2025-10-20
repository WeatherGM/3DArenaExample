using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerAnimatorController : BaseAnimatorController
    {
        [SerializeField] private string _jumpState = "Jump";
        [SerializeField] private int _idleState = -1;

        public void Jump()
        {
            Animator.SetTrigger(_jumpState);
        }

        public void Idle()
        {
            Animator.SetInteger(STATE, _idleState);
        }
    }
}
