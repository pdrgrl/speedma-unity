using UnityEngine;
using UnityEditor;
using Chamusca.Simulation;

namespace Chamusca.EditorExtensions
{
    [CustomEditor(typeof(ScenarioManager))]
    public class ScenarioManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw the default inspector
            DrawDefaultInspector();

            ScenarioManager manager = (ScenarioManager)target;

            GUILayout.Space(10);
            GUILayout.Label("Scenario Controls", EditorStyles.boldLabel);

            if (GUILayout.Button("Switch to Scenario A (Battery Only)"))
            {
                manager.currentScenario = SimulationScenario.ScenarioA;
                manager.ApplyScenario();
                EditorUtility.SetDirty(manager);
            }

            if (GUILayout.Button("Switch to Scenario B (Crossley Engine)"))
            {
                manager.currentScenario = SimulationScenario.ScenarioB;
                manager.ApplyScenario();
                EditorUtility.SetDirty(manager);
            }

            if (GUILayout.Button("Switch to Scenario C (Public Grid)"))
            {
                manager.currentScenario = SimulationScenario.ScenarioC;
                manager.ApplyScenario();
                EditorUtility.SetDirty(manager);
            }

            GUILayout.Space(5);
            if (GUILayout.Button("Setup Initial Hierarchy"))
            {
                CreateHierarchy(manager);
            }
        }

        private void CreateHierarchy(ScenarioManager manager)
        {
            // Helper to create empty groups if they don't exist
            manager.objectsA.Add(CreateGroup("Scenario_A_BatteryOnly", manager.transform));
            manager.objectsB.Add(CreateGroup("Scenario_B_CrossleyEngine", manager.transform));
            manager.objectsC.Add(CreateGroup("Scenario_C_ACIntegration", manager.transform));
            manager.sharedObjects.Add(CreateGroup("Shared_Infrastructure", manager.transform));
            
            manager.ApplyScenario();
            Debug.Log("[ScenarioManager] Initial Hierarchy created. Drag your models into the new folders.");
        }

        private GameObject CreateGroup(string name, Transform parent)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent);
            return go;
        }
    }
}
