using UnityEngine;
namespace Assets.Scripts.Enemies
{
    public class MeleeEnemyAnimator : BaseAnimatorController
{
    [SerializeField] protected GameObject _model;

    protected readonly Vector3 _rootOffset = new Vector3(0, 0.8f, 0);
    protected readonly Vector3 _despawnPosition = new Vector3(-999, -999, -999);

    protected const string WALK_NAME = "Walk";

    protected void OnEnable()
    {
        if (Animator == null)
            return;
        _model.transform.localPosition = _rootOffset;
        Animator.enabled = true;
        Animator.CrossFade(WALK_NAME, 0f);

        for (int i = 0; i < 2f; i++)
            Animator.Update(0f);
    }
    public override void Die()
    {
        _model.transform.localPosition = Vector3.zero;
        base.Die();
    }
    public void ResetAnimator()
    {
        transform.SetPositionAndRotation(_despawnPosition,Quaternion.identity);
        Animator.Rebind();
        Animator.Update(0f);
        Animator.Play(WALK_NAME, 0, 0f);
        Animator.Update(0f);
        Animator.enabled = false;
    }
}
    }