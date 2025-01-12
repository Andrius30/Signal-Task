using Unity.Entities;

public struct SignalEmitter : IComponentData
{
    public float EmitAngle; // Signal angle
    public float EmitFrequency; // How many signal per second
    public float EmitTimer; // Time to next signal

}
