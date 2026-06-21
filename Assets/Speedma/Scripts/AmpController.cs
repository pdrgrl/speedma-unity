// Assets/Speedma/Scripts/AmpController.cs
// ============================================================
// Drives an ammeter needle GameObject from a float value.
//
// Needle rotates on LOCAL Y axis.
// Calibration points (from model measurements):
//   0 A  -> Y = -4.951
//   10 A -> Y = -72.828
//   12 A -> Y = -87.359
//
// Calibrate with a curve or the default measured points below.
// Assign NeedleTransform to the needle child GameObject.
// ============================================================

using UnityEngine;

namespace Speedma
{
    public class AmpController : MonoBehaviour
    {
        [Header("Needle")]
        [SerializeField]
        private Transform needleTransform;

        [Header("Calibration")]
        [SerializeField]
        private float minAmp = 0f;

        [SerializeField]
        private float maxAmp = 12f;

        [SerializeField]
        private float minAngle = -5.064f; // Y rotation at 0 A

        [SerializeField]
        private float maxAngle = -85.556f; // Y rotation at 12 A

        [SerializeField]
        private AnimationCurve ampsToAngle = new AnimationCurve(
            new Keyframe(0f, -5.064f),
            new Keyframe(4f, -20.72f),
            new Keyframe(6f, -40.02f),
            new Keyframe(8f, -56f),
            new Keyframe(10f, -71.617f),
            new Keyframe(12f, -85.556f)
        );

        [Header("Smoothing")]
        [SerializeField]
        private float smoothSpeed = 8f; // deg/s lerp; 0 = instant

        private float _targetAngle;
        private float _currentAngle;

        private void Start()
        {
            _currentAngle = minAngle;
            _targetAngle = minAngle;
            ApplyAngle(_currentAngle);
        }

        private void Update()
        {
            if (smoothSpeed <= 0f)
                _currentAngle = _targetAngle;
            else
                _currentAngle = Mathf.LerpAngle(
                    _currentAngle,
                    _targetAngle,
                    Time.deltaTime * smoothSpeed
                );

            ApplyAngle(_currentAngle);
        }

        /// <summary>Set the displayed amperage value (called by FmuSceneLink).</summary>
        [SerializeField]
        private bool useSquareLaw = false;

        public void SetValue(float amps)
        {
            float clamped = Mathf.Clamp(amps, minAmp, maxAmp);

            if (ampsToAngle != null && ampsToAngle.length > 0)
            {
                _targetAngle = ampsToAngle.Evaluate(clamped);
                return;
            }

            float normalized = Mathf.Clamp01(clamped / maxAmp);
            float t = useSquareLaw ? normalized * normalized : normalized;
            _targetAngle = Mathf.LerpAngle(minAngle, maxAngle, t);
        }

        private void ApplyAngle(float yAngle)
        {
            if (needleTransform == null)
                return;
            Vector3 e = needleTransform.localEulerAngles;
            needleTransform.localEulerAngles = new Vector3(e.x, yAngle, e.z);
        }

#if UNITY_EDITOR
        // Live preview in Editor without Play mode
        [Header("Editor Preview")]
        [SerializeField]
        [Range(0f, 12f)]
        private float previewAmps = 0f;

        private void OnValidate() => SetValue(previewAmps);
#endif
    }
}
