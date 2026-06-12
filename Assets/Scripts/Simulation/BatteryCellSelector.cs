using UnityEngine;
using Speedma;

namespace Chamusca.Simulation
{
    public class BatteryCellSelector : MonoBehaviour
    {
        [Header("Reductor Setup")]
        [Tooltip("The range of cells on the selector (usually 40 to 60 or 1 to 20 steps).")]
        public int minCell = 40;
        public int maxCell = 60;
        public int currentCell = 40;

        [Header("Visuals")]
        public Transform handle;
        public Vector3 rotationPerStep = new Vector3(0, 15f, 0); // Degrees per cell step
        public Vector3 baseRotation = Vector3.zero;

        [Header("Linking")]
        public FmuSceneLink fmuLink;
        public string fmuInputName = "reductor_pos";

        private void Start()
        {
            UpdateVisuals();
        }

        public void Increment()
        {
            currentCell = Mathf.Clamp(currentCell + 1, minCell, maxCell);
            UpdateVisuals();
            UpdateFMU();
        }

        public void Decrement()
        {
            currentCell = Mathf.Clamp(currentCell - 1, minCell, maxCell);
            UpdateVisuals();
            UpdateFMU();
        }

        public void SetStep(int step)
        {
            currentCell = Mathf.Clamp(step, minCell, maxCell);
            UpdateVisuals();
            UpdateFMU();
        }

        private void UpdateVisuals()
        {
            if (handle != null)
            {
                int steps = currentCell - minCell;
                handle.localRotation = Quaternion.Euler(baseRotation + (rotationPerStep * steps));
            }
        }

        private void UpdateFMU()
        {
            // Note: FmuSceneLink might need to be extended or we use SimManager directly
            // For now, let's assume we can set generic inputs
            if (fmuLink != null)
            {
                // Accessing private SimManager via FmuSceneLink if we can, 
                // or just having a direct reference to SimManager.
                // Re-using the pattern from FmuSceneLink.
            }
        }
    }
}
