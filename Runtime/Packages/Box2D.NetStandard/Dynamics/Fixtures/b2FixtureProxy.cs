using Box2D.NetStandard.Collision;

namespace Box2D.NetStandard.Dynamics.Fixtures
{
    internal class b2FixtureProxy
    {
        internal AABB aabb;
        internal int childIndex;
        internal b2Fixture fixture;
        internal int proxyId = -1;
    }
}