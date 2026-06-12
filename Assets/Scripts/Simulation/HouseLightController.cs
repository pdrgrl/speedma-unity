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

        public void UpdateFromVoltage(float voltage)
        {
            float intensity = Mathf.Clamp01(voltage / nominalVoltage) * maxIntensity;
            
            // Add a bit of "glow" logic: lower voltage = warmer color
            float colorT = Mathf.InverseLerp(flickerThreshold, nominalVoltage, voltage);
            Color currentColor = Color.Lerp(lowVoltageColor, normalColor, colorT);

            foreach (var l in targetLights)
            {
                if (l == null) continue;
                l.intensity = intensity;
                l.color = currentColor;
                
                // If voltage is too low, disable light (blackout)
                l.enabled = voltage > 10f;
            }
        }
    }
}
