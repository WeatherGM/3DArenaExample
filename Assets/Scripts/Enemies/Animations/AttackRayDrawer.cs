using UnityEngine;
using System.Collections;
namespace Assets.Scripts.Enemies
{
    [RequireComponent(typeof(LineRenderer))]

    /// <summary>
    /// Отвечает за луч у дальнобойного врага 
    /// Вызывается через animation event, в клипе RangeAttack.
    /// </summary>
    public class AttackRayDrawer : MonoBehaviour
    {


        [SerializeField] private Color _color = Color.red;
        [SerializeField] private float _width = 0.05f;
        [SerializeField] private float _duration = 0.2f;
        [SerializeField] private Transform _attackOrigin;
        private LineRenderer _lr;
        private Transform _playerOrigin;
        private void Awake()
        {
            _lr = GetComponent<LineRenderer>();
            _lr.enabled = false;
            _playerOrigin = GameObject.FindGameObjectWithTag("Player").transform;
            _lr.startWidth = _width;
            _lr.endWidth = _width;
            _lr.startColor = _color;
            _lr.endColor = _color;
            _lr.positionCount = 2;
            _lr.useWorldSpace = true;
        }
        public void DrawRay()
        {
            StopAllCoroutines();
            StartCoroutine(DrawRayRoutine(_attackOrigin.position, _playerOrigin.position));
        }

        private IEnumerator DrawRayRoutine(Vector3 origin, Vector3 target)
        {
            _lr.SetPosition(0, origin);
            _lr.SetPosition(1, target);
            _lr.enabled = true;

            yield return new WaitForSeconds(_duration);

            _lr.enabled = false;
        }
    }
}