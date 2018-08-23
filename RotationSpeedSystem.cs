using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

// Using IJobProcessComponentData to iterate over all entities matching the required component types.
// Processing of entities happens in parallel. The main thread only schedules jobs.
public class RotationSpeedSystem : JobComponentSystem
{

    // IJobProcessComponentData is a simple way of iterating over all entities given the set of required compoenent types.
    // It is also more efficient than IJobParallelFor and more convenient.
    [BurstCompile]
    private struct RotationSpeedRotation : IJobProcessComponentData<Position,Rotation, RotationSpeed>
    {
        public float dt;
        
        public void Execute(ref Position pos,ref Rotation rotation, [ReadOnly]ref RotationSpeed speed)
        {
            rotation.Value = math.mul(math.normalize(rotation.Value), quaternion.axisAngle(math.up(), speed.Value * dt));
            pos.Value = speed.PosValue+dt;
        }
    }

    // We derive from JobComponentSystem, as a result the system proviides us 
    // the required dependencies for our jobs automatically.
    //
    // IJobProcessComponentData declares that it will read RotationSpeed and write to Rotation.
    //
    // Because it is declared the JobComponentSystem can give us a Job dependency, which contains all previously scheduled
    // jobs that write to any Rotation or RotationSpeed.
    // We also have to return the dependency so that any job we schedule 
    // will get registered against the types for the next System that might run.
    // This approach means:
    // * No waiting on main thread, just scheduling jobs with dependencies (Jobs only start when dependencies have completed)
    // * Dependencies are figured out automatically for us, so we can write modular multithreaded code
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new RotationSpeedRotation() { dt = Time.deltaTime };
        return job.Schedule(this, 64, inputDeps);
    }
}