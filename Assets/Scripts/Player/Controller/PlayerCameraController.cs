using UnityEngine;
namespace Assets.Scripts.Player
{
    public class PlayerCameraController : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private LayerMask _collisionLayers;

        [Header("Camera settings")]
        [SerializeField] private float _distance = 11f;
        [SerializeField] private float _rotationSpeed = 2.5f;
        [SerializeField] private float _minDistance = 4;
        [SerializeField] private float _smoothSpeed = 0.125f;
        [SerializeField] private float _rotationSmoothness = 100f;
        [SerializeField] private float _currentDistance;
        [SerializeField] private float _desktopRotationSpeed = 0.2f;
        [SerializeField] private float _minPitch = -80f;
        [SerializeField] private float _maxPitch = 80f;

        private Vector3 _currentRotation = new Vector3(5.8f, -178, 0);
        private const float ROTATE_THRESHOLD = 0.01f;

        public void RotateCamera()
        {
            if (Input.GetMouseButton(1))
                Rotate();
            else
                _currentDistance = _distance;
            CollisionHandler();
        }

        private void CollisionHandler()
        {
            if (HandleCameraCollision(out Vector3 desiredPosition))
                transform.position = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
        }

        private void Rotate()
        {
            float rotationY, rotationX;

            float inputX = Input.GetAxis("Mouse X");
            float inputY = Input.GetAxis("Mouse Y");

            if (Mathf.Abs(inputX) < ROTATE_THRESHOLD && Mathf.Abs(inputY) < ROTATE_THRESHOLD)
                return;

            rotationX = inputX * _rotationSpeed;
            rotationY = -inputY * _rotationSpeed;

            float targetYaw = _currentRotation.y + rotationX;
            float targetPitch = Mathf.Clamp(_currentRotation.x + rotationY, _minPitch, _maxPitch);

            var smooth = _rotationSmoothness * Time.deltaTime;
            float smoothYaw = Mathf.LerpAngle(_currentRotation.y, targetYaw, smooth);
            float smoothPitch = Mathf.LerpAngle(_currentRotation.x, targetPitch, smooth);

            _currentRotation = new Vector3(smoothPitch, smoothYaw, 0f);
            transform.rotation = Quaternion.Euler(_currentRotation);

            _currentDistance = Vector3.Distance(_target.position, transform.position);
            _currentDistance = _currentDistance > _distance ? _distance : _currentDistance;
            transform.position = _target.position - transform.forward * _currentDistance;
        }

        private bool HandleCameraCollision(out Vector3 hitPosition)
        {
            hitPosition = _target.position - transform.forward * _distance;
            RaycastHit hit;
            if (Physics.Raycast(_target.position, hitPosition - _target.position, out hit, _distance, _collisionLayers))
            {

                if (hit.distance < _minDistance)
                {
                    transform.position = _target.position - transform.forward * _minDistance;
                    return false;
                }

                hitPosition = hit.point;
                return true;
            }
            transform.position = _target.position - transform.forward * _currentDistance;
            return false;
        }
    }
}