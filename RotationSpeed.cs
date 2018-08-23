using System;
using Unity.Entities;

// The rotation speed component simply stores the Speed value
[Serializable]
public struct RotationSpeed : IComponentData
{
    public float Value;
    public Unity.Mathematics.float3 PosValue;
}

// This wrapper component is currently necessary to add ComponentData to GameObjects.
// In the future we want to make this wrapper component automatic.
public class RotationSpeedComponent : ComponentDataWrapper<RotationSpeed> { }