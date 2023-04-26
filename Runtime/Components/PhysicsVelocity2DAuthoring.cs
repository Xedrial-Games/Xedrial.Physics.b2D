using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Xedrial.Physics.b2D.Components
{
    public struct PhysicsVelocity2D : IComponentData
    {
        public float2 Linear;
        public float Angular;
    }

    public class PhysicsVelocity2DAuthoring : MonoBehaviour
    {
        [SerializeField] public float2 m_Linear;
        [SerializeField] public float m_Angular;

        public class PhysicsVelocity2DBaker : Baker<PhysicsVelocity2DAuthoring>
        {
            public override void Bake(PhysicsVelocity2DAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new PhysicsVelocity2D
                {
                    Linear = authoring.m_Linear,
                    Angular = authoring.m_Angular
                });
            }
        }
    }
}