using Box2D.NetStandard.Common;
using Box2D.NetStandard.Dynamics.World;
using Box2D.NetStandard.Dynamics.World.Callbacks;
using Unity.Mathematics;
using UnityEngine;
using b2Vec2 = System.Numerics.Vector2;
using Matrix4x4 = UnityEngine.Matrix4x4;

namespace Xedrial.Physics.b2D
{
    public class DebugDraw : b2DebugDraw
    {
        public DebugDraw(Camera camera, Material material, Mesh quadMesh)
        {
            m_Material = material;
            m_Camera = camera;
            m_QuadMesh = quadMesh;
        }

        private readonly Material m_Material;
        private readonly Camera m_Camera;
        private readonly Mesh m_QuadMesh;

        public override void DrawTransform(in b2Transform xf)
        {
            b2Vec2 p = xf.p;
            float3 position = new(p.X, p.Y, 0);
            quaternion rotation = quaternion.Euler(0, 0, xf.GetAngle());
            Matrix4x4 matrix = float4x4.TRS(position, rotation, new float3(0.2f));
            Graphics.DrawMesh(
                m_QuadMesh,
                matrix,
                m_Material,
                0
            );
        }

        public override void DrawPoint(in b2Vec2 position, float size, in b2Color color)
        {
            MaterialPropertyBlock properties = new();
            properties.SetColor("_Color", new Color(color.R, color.G, color.B, color.A));

            float3 pos = new(position.X, position.Y, 0);
            Matrix4x4 transform = float4x4.TRS(pos, quaternion.identity, new float3(size));

            Graphics.DrawMesh(
                m_QuadMesh,
                transform,
                m_Material,
                0,
                m_Camera,
                0,
                properties
            );
        }

        public override void DrawPolygon(in b2Vec2[] vertices, int vertexCount, in b2Color color)
        {
            MaterialPropertyBlock properties = new();
            Color uColor = new(color.R, color.G, color.B, color.A);
            properties.SetColor("_Color", uColor);

            var verts = new float3[vertexCount];
            for (int i = 0; i < vertexCount; i++)
            {
                b2Vec2 vertex = vertices[i];
                verts[i] = new float3(vertex.X, vertex.Y, 0.1f);
            }

            float3? lastVertex = null;
            foreach (float3 vertex in verts)
            {
                if (lastVertex != null)
                    UnityEngine.Debug.DrawLine(lastVertex.Value, vertex, uColor);

                Matrix4x4 mat = float4x4.TRS(vertex, quaternion.identity, new float3(0.2f));
                Graphics.DrawMesh(
                    m_QuadMesh,
                    mat,
                    m_Material,
                    0,
                    m_Camera,
                    0,
                    properties
                );
                lastVertex = vertex;
            }
        }

        public override void DrawSolidPolygon(in b2Vec2[] vertices, int vertexCount, in b2Color color)
        {
            MaterialPropertyBlock properties = new();
            Color uColor = new(color.R, color.G, color.B, color.A);
            properties.SetColor("_Color", uColor);

            var verts = new float3[vertexCount];
            for (int i = 0; i < vertexCount; i++)
            {
                b2Vec2 vertex = vertices[i];
                verts[i] = new float3(vertex.X, vertex.Y, 0);
            }

            float3? lastVertex = null;
            foreach (float3 vertex in verts)
            {
                if (lastVertex != null)
                    UnityEngine.Debug.DrawLine(lastVertex.Value, vertex, uColor);

                Matrix4x4 mat = float4x4.TRS(vertex, quaternion.identity, new float3(0.1f));
                Graphics.DrawMesh(
                    m_QuadMesh,
                    mat,
                    m_Material,
                    0,
                    m_Camera,
                    0,
                    properties
                );
                lastVertex = vertex;
            }
        }

        public override void DrawCircle(in b2Vec2 center, float radius, in b2Color color)
        {
            throw new System.NotImplementedException();
        }

        public override void DrawSolidCircle(in b2Vec2 center, float radius, in b2Vec2 axis, in b2Color color)
        {
            throw new System.NotImplementedException();
        }

        public override void DrawSegment(in b2Vec2 p1, in b2Vec2 p2, in b2Color color)
        {
            throw new System.NotImplementedException();
        }
    }
}
