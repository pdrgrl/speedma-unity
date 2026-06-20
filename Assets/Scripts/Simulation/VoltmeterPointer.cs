using UnityEngine;

namespace Chamusca.Simulation
{
    public class VoltmeterPointer : MonoBehaviour
    {
        [Header("Visuals")]
        public Transform needle;
        public float minAngle = 0f;
        public float maxAngle = 180f;
        public Vector3 rotationAxis = new Vector3(0f, 1f, 0f);

        public float minValue = 0f;
        public float maxValue = 200f;

        [Header("Movement")]
        public float smoothSpeed = 5f;

        private float _targetValue = 0f;
        private float _currentDisplayedValue = 0f;

        public void SetValue(float val)
        {
            _targetValue = val;
        }

        private void Update()
        {
            if (needle == null) return;

            // Smooth the needle movement
            _currentDisplayedValue = Mathf.Lerp(_currentDisplayedValue, _targetValue, Time.deltaTime * smoothSpeed);

            // Map value to rotation angle
            float t = Mathf.InverseLerp(minValue, maxValue, _currentDisplayedValue);
            float angle = Mathf.Lerp(minAngle, maxAngle, t);

            Vector3 axis = rotationAxis.sqrMagnitude > 0f ? rotationAxis.normalized : Vector3.up;
            needle.localRotation = Quaternion.AngleAxis(angle, axis);
        }
    }
}
