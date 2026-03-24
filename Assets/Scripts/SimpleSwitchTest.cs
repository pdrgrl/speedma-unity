using FMI2; // A tua API
using UnityEngine;

public class SimpleSwitchTest : MonoBehaviour
{
    private FMU fmu;

    [Header("Controla o Switch em Play Mode")]
    [Tooltip("Liga ou desliga a corrente do circuito")]
    public bool switchOn = false;

    [Header("Resultado lido da Simulação")]
    [Tooltip("Se o switch estiver ligado, deve ser perto de 5. Se desligado, 0.")]
    public float outputVoltage = 0.0f;

    void Start()
    {
        // Instancia e inicializa o FMU super simples
        fmu = new FMU("SimpleSwitchTest", "SwitchInstance");
        fmu.SetupExperiment(Time.time);
        fmu.EnterInitializationMode();
        fmu.ExitInitializationMode();

        Debug.Log("FMU SimpleSwitchTest carregado! Clica na checkbox 'Switch On' para testar.");
    }

    void FixedUpdate()
    {
        if (fmu == null)
            return;

        // 1. Enviar o Input Booleano.
        // A API FMI2 que definimos tem o método SetBoolean.
        fmu.SetBoolean("switchOn", switchOn);

        // 2. Avançar o passo de simulação
        fmu.DoStep(Time.time, Time.fixedDeltaTime);

        // 3. Ler o Output final do circuito diretamente para o Inspector
        outputVoltage = (float)fmu.GetReal("outputVoltage");
    }

    void OnApplicationQuit()
    {
        if (fmu != null)
        {
            fmu.Terminate();
            fmu.FreeInstance();
        }
    }
}
