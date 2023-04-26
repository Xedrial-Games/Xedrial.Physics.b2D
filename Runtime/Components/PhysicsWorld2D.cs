using Unity.Entities;

using Box2D.NetStandard.Dynamics.World;

namespace Xedrial.Physics.b2D.Components
{
    public class PhysicsWorld2D : IComponentData
    {
        public b2World Value;
    }
}