using UnityEngine;

namespace Chamusca.Simulation
{
    public class HouseLightController : MonoBehaviour
    {
        [Header("Lights")]
        public Light[] targetLights;
        
        [Header("Parameters")]
        public float nominalVoltage = 110f;
        public float maxIntensity = 1.0f;
        public float flickerThreshold = 80f;

        [Header("Feedback")]
        public Color normalColor = Color.white;
        public Color lowVoltageColor = new Color(1f, 0.5f, 0.2f); // Dim orange
        public bool useVoltageCurve = true;

        private float _currentIntensity = 0f;
        private Color _currentColor = Color.white;

        public void UpdateFromVoltage(float voltage)
        {
            float intensity = Mathf.Clamp01(voltage / nominalVoltage) * maxIntensity;
            float colorT = Mathf.InverseLerp(flickerThreshold, nominalVoltage, voltage);
            Apply(intensity, Color.Lerp(lowVoltageColor, normalColor, colorT), voltage > 10f);
        }

        public void UpdateFromIntensity(float intensity, bool enabled = true)
        {
            _currentIntensity = Mathf.Clamp01(intensity) * maxIntensity;
            _currentColor = Color.Lerp(lowVoltageColor, normalColor, Mathf.Clamp01(intensity));
            Apply(_currentIntensity, _currentColor, enabled && _currentIntensity > 0.001f);
        }

        private void Apply(float intensity, Color color, bool enabled)
        {
            foreach (var l in targetLights)
            {
                if (l == null)
                    continue;
                l.intensity = intensity;
                l.color = color;
                l.enabled = enabled;
            }
        }
    }
}
