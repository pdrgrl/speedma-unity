using UnityEngine;

namespace Chamusca.Simulation
{
    public class VoltmeterPointer : MonoBehaviour
    {
        [Header("Visuals")]
        public Transform needle;
        public Vector3 rotationAxis = new Vector3(0f, 1f, 0f);

        [Header("Calibration")]
        [Tooltip("Maps voltage (X) to needle angle (Y). Set points from your measured dial positions.")]
        public AnimationCurve voltageToAngle = new AnimationCurve(
            new Keyframe(0f, -7f),
            new Keyframe(120f, -36.7f),
            new Keyframe(200f, -85f)
        );

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

            // Map voltage to a calibrated dial angle.
            float clampedVoltage = Mathf.Clamp(_currentDisplayedValue, minValue, maxValue);
            float angle = voltageToAngle != null ? voltageToAngle.Evaluate(clampedVoltage) : 0f;

            Vector3 axis = rotationAxis.sqrMagnitude > 0f ? rotationAxis.normalized : Vector3.up;
            needle.localRotation = Quaternion.AngleAxis(angle, axis);
        }
    }
}
