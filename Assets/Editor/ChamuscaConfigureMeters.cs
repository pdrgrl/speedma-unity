#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Chamusca.Simulation;
using Speedma;

public class ChamuscaConfigureMeters : EditorWindow
{
    [MenuItem("Chamusca/Configure All Meters")]
    public static void ConfigureAllMeters()
    {
        GameObject parent = GameObject.Find("Scenario_A_BatteryOnly");
        if (parent == null)
        {
            Debug.LogError("[Configure] Could not find Scenario_A_BatteryOnly in the scene!");
            return;
        }

        Transform voltMeterObj = parent.transform.Find("_SM_Meter_Siemens_Halske_VOLT_Pivot");
        Transform amp10 = parent.transform.Find("_SM_Meter_Siemens_Halske_AMP10_Pivot");
        Transform amp30 = parent.transform.Find("_SM_Meter_Siemens_Halske_AMP30_Pivot");

        if (voltMeterObj == null || amp10 == null || amp30 == null)
        {
            foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
            {
                if (child.name == "_SM_Meter_Siemens_Halske_VOLT_Pivot") voltMeterObj = child;
                if (child.name == "_SM_Meter_Siemens_Halske_AMP10_Pivot") amp10 = child;
                if (child.name == "_SM_Meter_Siemens_Halske_AMP30_Pivot") amp30 = child;
            }
        }

        // 1. Center Meter (VOLT)
        if (voltMeterObj != null)
        {
            VoltmeterPointer vp = voltMeterObj.GetComponent<VoltmeterPointer>();
            if (vp == null) vp = voltMeterObj.gameObject.AddComponent<VoltmeterPointer>();

            vp.needle = voltMeterObj.Find("Needle");
            vp.minValue = 0f;
            vp.maxValue = 170f;
            vp.rotationAxis = new Vector3(0f, 1f, 0f);

            vp.voltageToAngle = new AnimationCurve(
                new Keyframe(0f, -13.877f),
                new Keyframe(100f, -22.138f),
                new Keyframe(120f, -36.525f),
                new Keyframe(140f, -56.091f),
                new Keyframe(160f, -75.273f),
                new Keyframe(170f, -84.863f)
            );

            EditorUtility.SetDirty(voltMeterObj.gameObject);
            Debug.Log("[Configure] Configured VOLT Meter with exact custom curve.");
        }

        // 2. Right Meter (AMP10 - Battery Ammeter)
        if (amp10 != null)
        {
            AmpController ac = amp10.GetComponent<AmpController>();
            if (ac == null) ac = amp10.gameObject.AddComponent<AmpController>();

            Transform needle = amp10.Find("Needle");
            
            SerializedObject so = new SerializedObject(ac);
            so.FindProperty("needleTransform").objectReferenceValue = needle;
            so.FindProperty("minAmp").floatValue = 0f;
            so.FindProperty("maxAmp").floatValue = 12f;
            so.FindProperty("minAngle").floatValue = -5.085f;
            so.FindProperty("maxAngle").floatValue = -85.457f;
            
            AnimationCurve curve = new AnimationCurve(
                new Keyframe(0f, -5.085f),
                new Keyframe(4f, -20.567f),
                new Keyframe(6f, -40.184f),
                new Keyframe(8f, -56.229f),
                new Keyframe(10f, -71.854f),
                new Keyframe(12f, -85.457f)
            );
            so.FindProperty("ampsToAngle").animationCurveValue = curve;
            so.ApplyModifiedProperties();

            EditorUtility.SetDirty(amp10.gameObject);
            Debug.Log("[Configure] Configured AMP10 (Battery Ammeter) with exact custom curve.");
        }

        // 3. Left Meter (AMP30 - Dynamo Ammeter)
        if (amp30 != null)
        {
            AmpController ac = amp30.GetComponent<AmpController>();
            if (ac == null) ac = amp30.gameObject.AddComponent<AmpController>();

            Transform needle = amp30.Find("Needle");

            SerializedObject so = new SerializedObject(ac);
            so.FindProperty("needleTransform").objectReferenceValue = needle;
            so.FindProperty("minAmp").floatValue = 0f;
            so.FindProperty("maxAmp").floatValue = 35f;
            so.FindProperty("minAngle").floatValue = -5.064f;
            so.FindProperty("maxAngle").floatValue = -85.386f;

            AnimationCurve curve = new AnimationCurve(
                new Keyframe(0f, -5.064f),
                new Keyframe(10f, -15.666f),
                new Keyframe(15f, -30.873f),
                new Keyframe(20f, -46.322f),
                new Keyframe(25f, -59.368f),
                new Keyframe(30f, -71.445f),
                new Keyframe(35f, -85.386f)
            );
            so.FindProperty("ampsToAngle").animationCurveValue = curve;
            so.ApplyModifiedProperties();

            EditorUtility.SetDirty(amp30.gameObject);
            Debug.Log("[Configure] Configured AMP30 (Dynamo Ammeter) with exact custom curve.");
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        Debug.Log("[Configure] All three meters configured successfully!");
    }
}
#endif
