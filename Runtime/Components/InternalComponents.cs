using Unity.Entities;

using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Fixtures;
using Box2D.NetStandard.Dynamics.World;

namespace Xedrial.Physics.b2D.Components
{
    internal class PhysicsWorld2D : IComponentData
    {
        public b2World Value;
    }
    
    internal class PhysicsBody2D : IComponentData
    {
        // Storage for runtime
        public b2Body RuntimeBody;
    }
    
    internal class PhysicsCollider2D : IComponentData
    {
        // Storage for runtime
        public b2Fixture Fixture;
    }
}