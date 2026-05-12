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
// Set MinAngle / MaxAngle / MinAmp / MaxAmp in Inspector.
// Assign NeedleTransform to the needle child GameObject.
// ============================================================

using UnityEngine;

namespace Speedma
{
    public class AmpController : MonoBehaviour
    {
        [Header("Needle")]
        [SerializeField] private Transform needleTransform;

        [Header("Calibration")]
        [SerializeField] private float minAmp   =  0f;
        [SerializeField] private float maxAmp   = 12f;
        [SerializeField] private float minAngle = -4.951f;    // Y rotation at minAmp
        [SerializeField] private float maxAngle = -87.359f;   // Y rotation at maxAmp

        [Header("Smoothing")]
        [SerializeField] private float smoothSpeed = 8f;      // deg/s lerp; 0 = instant

        private float _targetAngle;
        private float _currentAngle;

        private void Start()
        {
            _currentAngle = minAngle;
            _targetAngle  = minAngle;
            ApplyAngle(_currentAngle);
        }

        private void Update()
        {
            if (smoothSpeed <= 0f)
                _currentAngle = _targetAngle;
            else
                _currentAngle = Mathf.LerpAngle(_currentAngle, _targetAngle, Time.deltaTime * smoothSpeed);

            ApplyAngle(_currentAngle);
        }

        /// <summary>Set the displayed amperage value (called by FmuSceneLink).</summary>
        public void SetValue(float amps)
        {
            float t      = Mathf.InverseLerp(minAmp, maxAmp, Mathf.Clamp(amps, minAmp, maxAmp));
            _targetAngle = Mathf.LerpAngle(minAngle, maxAngle, t);
        }

        private void ApplyAngle(float yAngle)
        {
            if (needleTransform == null) return;
            Vector3 e = needleTransform.localEulerAngles;
            needleTransform.localEulerAngles = new Vector3(e.x, yAngle, e.z);
        }

#if UNITY_EDITOR
        // Live preview in Editor without Play mode
        [Header("Editor Preview")]
        [SerializeField] [Range(0f, 12f)] private float previewAmps = 0f;
        private void OnValidate() => SetValue(previewAmps);
#endif
    }
}
