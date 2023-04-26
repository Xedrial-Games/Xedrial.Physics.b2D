using Box2D.NetStandard.Dynamics.Bodies;
using Unity.Entities;

namespace Xedrial.Physics.b2D.Components
{
    public class PhysicsBody2D : IComponentData
    {
        public b2Body RuntimeBody;
    }
}