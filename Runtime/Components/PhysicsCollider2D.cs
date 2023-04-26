using Box2D.NetStandard.Dynamics.Fixtures;
using Unity.Entities;

namespace Xedrial.Physics.b2D.Components
{
    public class PhysicsCollider2D : IComponentData
    {
        // Storage for runtime
        public b2Fixture Fixture;
    }
}