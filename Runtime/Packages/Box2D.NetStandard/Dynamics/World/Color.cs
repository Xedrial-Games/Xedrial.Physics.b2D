namespace Box2D.NetStandard.Dynamics.World
{
    /// <summary>
    ///  Color for debug drawing. Each value has the range [0,1].
    /// </summary>
    public struct b2Color
    {
        public float R, G, B, A;

        public b2Color(float r, float g, float b, float a = 1f)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public void Set(float r, float g, float b, float a = 1f)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
    }
}