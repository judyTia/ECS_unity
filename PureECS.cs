using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;

public class PureECS : MonoBehaviour {
    public float mySpeed;
    public Mesh cubeMesh;
    public Material myMaterial;
	// Use this for initialization
	void Start () {
        
        var entityManager = World.Active.GetOrCreateManager<EntityManager>();
        for (int i = 0; i < 5000; i++)
        {
            var objectEnity = entityManager.CreateEntity(
                ComponentType.Create<RotationSpeed>(),
                ComponentType.Create<TransformMatrix>(),
                ComponentType.Create<Rotation>(),
                 ComponentType.Create<Position>(),
                 ComponentType.Create<MeshFilter>(),
                 ComponentType.Create<MeshInstanceRenderer>(),
                 ComponentType.Create<BoxCollider>()
                );
            entityManager.SetComponentData(objectEnity, new RotationSpeed { Value = mySpeed ,PosValue = Random.Range(-100,100) });

            entityManager.SetSharedComponentData(objectEnity, new MeshInstanceRenderer
            {
                mesh = cubeMesh,
                material = myMaterial
            });

        }
	}
	

}
