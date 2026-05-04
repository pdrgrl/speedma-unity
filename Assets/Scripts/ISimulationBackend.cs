/// <summary>
/// Abstraction over the FMU simulation so gameplay scripts have zero
/// knowledge of whether the FMU runs locally (Editor) or on the server
/// (WebGL).  Both implementations expose the same surface.
/// </summary>
public interface ISimulationBackend
{
    /// <summary>True once the backend is initialised and ready to step.</summary>
    bool IsReady { get; }

    void SetBoolean(string variableName, bool value);
    void SetReal(string variableName, double value);
    void SetInteger(string variableName, int value);

    /// <summary>
    /// Request one simulation step of <paramref name="dt"/> seconds.
    /// In the remote implementation this is fire-and-forget (coroutine);
    /// results become available on the NEXT frame via Get*.
    /// </summary>
    void Step(float dt);

    float  GetReal(string variableName);
    bool   GetBoolean(string variableName);
    int    GetInteger(string variableName);

    /// <summary>Reset the simulation to t = 0.</summary>
    void Reset();
}
