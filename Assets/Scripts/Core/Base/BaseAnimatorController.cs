using UnityEngine;

public abstract class BaseAnimatorController : MonoBehaviour
{
    [SerializeField] protected Animator Animator;

    [Header("States index")]
    [SerializeField] protected int AttackState = 0;
    [SerializeField] protected int MoveState = 1;
    [SerializeField] protected string DieState = "Die";

    protected const string STATE = "State";

    public virtual void Attack()
    {
        Animator.SetInteger(STATE, AttackState);
    }

    public virtual void Move()
    {
        Animator.SetInteger(STATE, MoveState);
    }

    public virtual void Die()
    {
        Animator.SetTrigger(DieState);
    }
}