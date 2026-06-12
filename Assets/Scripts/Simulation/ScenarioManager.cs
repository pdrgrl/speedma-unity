using UnityEngine;
using System.Collections.Generic;

namespace Chamusca.Simulation
{
    public enum SimulationScenario
    {
        ScenarioA, // Battery Only (c. 1923+)
        ScenarioB, // Crossley Engine + Dynamo (c. 1920)
        ScenarioC  // Public Grid + ASEA Motor (c. 1929)
    }

    public class ScenarioManager : MonoBehaviour
    {
        public SimulationScenario currentScenario = SimulationScenario.ScenarioA;

        [Header("Scenario A: Battery Discharge")]
        public List<GameObject> objectsA = new List<GameObject>();

        [Header("Scenario B: Crossley Generation")]
        public List<GameObject> objectsB = new List<GameObject>();

        [Header("Scenario C: AC Integration")]
        public List<GameObject> objectsC = new List<GameObject>();

        [Header("Shared Infrastructure")]
        public List<GameObject> sharedObjects = new List<GameObject>();

        /// <summary>
        /// Applies the current scenario by toggling GameObject visibility.
        /// </summary>
        public void ApplyScenario()
        {
            ToggleList(objectsA, currentScenario == SimulationScenario.ScenarioA);
            ToggleList(objectsB, currentScenario == SimulationScenario.ScenarioB);
            ToggleList(objectsC, currentScenario == SimulationScenario.ScenarioC);
            
            // Ensure shared objects (like the Room or General Control Board) are always on
            ToggleList(sharedObjects, true);

            Debug.Log($"[ScenarioManager] Switched to {currentScenario}");
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
