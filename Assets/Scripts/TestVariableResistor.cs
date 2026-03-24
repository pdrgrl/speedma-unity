using FMI2; // <-- Volta a colocar isto!
using UnityEngine;

public class TestVariableResistor : MonoBehaviour
{
    private FMU fmu; // Mantém-se FMU

    public string fmuName = "ShowVariableResistor";
    public float inputU = 1.0f;

    public float voltageVariableResistor;
    public float voltageR2;

    void Start()
    {
        // CORREÇÃO AQUI: A classe é FMU (não FMU2) e precisa de 2 strings!
        // O primeiro é o nome do FMU (fmuName), o segundo é um nome qualquer para a instância.
        fmu = new FMU(fmuName, fmuName + "_Instance");

        fmu.SetupExperiment(Time.time);
        fmu.EnterInitializationMode();
        fmu.ExitInitializationMode();

        Debug.Log($"FMU '{fmuName}' inicializado com sucesso!");
    }

    void FixedUpdate()
    {
        if (fmu == null)
            return;

        fmu.SetReal("u", inputU);
        fmu.DoStep(Time.time, Time.fixedDeltaTime);

        // Lembra-te de verificar os nomes exatos das variáveis no ficheiro gerado na pasta Resources
        voltageVariableResistor = (float)fmu.GetReal("VariableResistor.v");
        voltageR2 = (float)fmu.GetReal("R2.v");
    }

    void OnApplicationQuit()
    {
        if (fmu != null)
        {
            fmu.Terminate();
            fmu.FreeInstance(); // Boa prática de limpeza (baseado no teu código)
        }
    }
}
