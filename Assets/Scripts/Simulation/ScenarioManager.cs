using UnityEngine;
using System.Collections.Generic;

namespace Chamusca.Simulation
{
    public enum SimulationScenario
    {
        ScenarioA, // Base battery-only operation
        ScenarioB, // Battery base + Crossley generation
        ScenarioC  // Battery base + AC integration
    }

    public class ScenarioManager : MonoBehaviour
    {
        public SimulationScenario currentScenario = SimulationScenario.ScenarioA;

        [Header("Shared Infrastructure")]
        public List<GameObject> sharedObjects = new List<GameObject>();

        [Header("Base Layer - Scenario A")]
        [Tooltip("Objects that should be active in all scenarios because they belong to the base installation.")]
        public List<GameObject> baseObjects = new List<GameObject>();

        [Header("Layer Additions - Scenario B")]
        [Tooltip("Objects added when the Crossley generation system is available.")]
        public List<GameObject> scenarioBObjects = new List<GameObject>();

        [Header("Layer Additions - Scenario C")]
        [Tooltip("Objects added when the AC integration system is available.")]
        public List<GameObject> scenarioCObjects = new List<GameObject>();

        [Header("Objects Disabled In Scenario C")]
        [Tooltip("Objects that exist in A/B but must be hidden or disconnected in C, such as the Crossley engine.")]
        public List<GameObject> disabledInScenarioC = new List<GameObject>();

        /// <summary>
        /// Applies the current scenario by toggling GameObject visibility.
        /// Scenario A is the base layer, B adds to A, and C adds to A+B while optionally disabling specific objects.
        /// </summary>
        public void ApplyScenario()
        {
            ToggleList(sharedObjects, true);
            ToggleList(baseObjects, true);
            ToggleList(scenarioBObjects, currentScenario >= SimulationScenario.ScenarioB);
            ToggleList(scenarioCObjects, currentScenario >= SimulationScenario.ScenarioC);

            // These objects exist in earlier scenarios, but should not be active in Scenario C.
            ToggleList(disabledInScenarioC, currentScenario != SimulationScenario.ScenarioC);

            Debug.Log($"[ScenarioManager] Switched to {currentScenario}");
        }

        public void SetScenario(SimulationScenario scenario)
        {
            if (currentScenario == scenario) return;
            currentScenario = scenario;
            ApplyScenario();
        }

        private void ToggleList(List<GameObject> list, bool state)
        {
            foreach (var obj in list)
            {
                if (obj != null) obj.SetActive(state);
            }
        }

        // Auto-apply on start
        private void Awake()
        {
            ApplyScenario();
        }
    }
}
