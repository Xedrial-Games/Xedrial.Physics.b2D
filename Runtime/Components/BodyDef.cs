using Unity.Entities;
using Unity.Mathematics;

using Box2D.NetStandard.Dynamics.Bodies;

namespace Xedrial.Physics.b2D.Components
{
    public struct BodyDef : IComponentData
    {
        public b2BodyType BodyType;
    }
}