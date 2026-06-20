using UnityEngine;

namespace Chamusca.Simulation
{
    /// <summary>
    /// Discrete tap selector for the Scenario A battery discharge side.
    ///
    /// Physical mapping:
    /// - currentCell is the tap/cell index selected on the redutor duplo discharge disk.
    /// - This component stores the logical state only.
    /// - ReductorDuploWheel handles interaction and visuals.
    /// - ChamuscaSimController reads currentCell and sends it to the FMU as reductor_pos.
    /// </summary>
    public class BatteryCellSelector : MonoBehaviour
    {
        [Header("Reductor Setup")]
        [Tooltip("Minimum cell/tap index for the discharge selector.")]
        public int minCell = 40;

        [Tooltip("Maximum cell/tap index for the discharge selector.")]
        public int maxCell = 60;

        [Tooltip("Initial selected tap/cell index.")]
        public int currentCell = 40;

        private void Start()
        {
            currentCell = Mathf.Clamp(currentCell, minCell, maxCell);
        }

        public void Increment()
        {
            currentCell = Mathf.Clamp(currentCell + 1, minCell, maxCell);
        }

        public void Decrement()
        {
            currentCell = Mathf.Clamp(currentCell - 1, minCell, maxCell);
        }

        public void SetStep(int step)
        {
            currentCell = Mathf.Clamp(step, minCell, maxCell);
        }
    }
}
