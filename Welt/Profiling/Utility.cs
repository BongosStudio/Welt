#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Welt.Profiling
{
    public static class Utility
    {
        #region RotateToFace
        public static Matrix RotateToFace(Vector3 o, Vector3 p, Vector3 u)
        {
            var d = (o - p);
            var right = Vector3.Cross(u, d);
            Vector3.Normalize(ref right, out right);
            var backwards = Vector3.Cross(right, u);
            Vector3.Normalize(ref backwards, out backwards);
            var up = Vector3.Cross(backwards, right);
            var rot = new Matrix(right.X, right.Y, right.Z, 0, up.X, up.Y, up.Z, 0, backwards.X, backwards.Y, backwards.Z, 0, 0, 0, 0, 1);
            return rot;
        }
        #endregion

        public static void RemapModel(Microsoft.Xna.Framework.Graphics.Model model, Effect effect)
        {
            foreach (var part in model.Meshes.SelectMany(mesh => mesh.MeshParts))
            {
                part.Effect = effect;
            }
        }

        #region TransformBoundingBox
        public static BoundingBox TransformBoundingBox(BoundingBox origBox, Matrix matrix)
        {
            var origCorner1 = origBox.Min;
            var origCorner2 = origBox.Max;

            var transCorner1 = Vector3.Transform(origCorner1, matrix);
            var transCorner2 = Vector3.Transform(origCorner2, matrix);

            return new BoundingBox(transCorner1, transCorner2);
        }
        #endregion

        #region TransformBoundingSphere
        public static BoundingSphere TransformBoundingSphere(BoundingSphere originalBoundingSphere, Matrix transformationMatrix)
        {
            Vector3 trans;
            Vector3 scaling;
            Quaternion rot;
            transformationMatrix.Decompose(out scaling, out rot, out trans);

            var maxScale = scaling.X;
            if (maxScale < scaling.Y)
                maxScale = scaling.Y;
            if (maxScale < scaling.Z)
                maxScale = scaling.Z;

            var transformedSphereRadius = originalBoundingSphere.Radius * maxScale;
            var transformedSphereCenter = Vector3.Transform(originalBoundingSphere.Center, transformationMatrix);

            var transformedBoundingSphere = new BoundingSphere(transformedSphereCenter, transformedSphereRadius);

            return transformedBoundingSphere;
        }
        #endregion

        #region DrawBoundingBox
        public static void DrawBoundingBox(BoundingBox bBox, GraphicsDevice device, BasicEffect basicEffect, Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix, Color color)
        {
            var v1 = bBox.Min;
            var v2 = bBox.Max;

            var cubeLineVertices = new VertexPositionColor[8];
            cubeLineVertices[0] = new VertexPositionColor(v1, Color.White);
            cubeLineVertices[1] = new VertexPositionColor(new Vector3(v2.X, v1.Y, v1.Z), color);
            cubeLineVertices[2] = new VertexPositionColor(new Vector3(v2.X, v1.Y, v2.Z), color);
            cubeLineVertices[3] = new VertexPositionColor(new Vector3(v1.X, v1.Y, v2.Z), color);

            cubeLineVertices[4] = new VertexPositionColor(new Vector3(v1.X, v2.Y, v1.Z), color);
            cubeLineVertices[5] = new VertexPositionColor(new Vector3(v2.X, v2.Y, v1.Z), color);
            cubeLineVertices[6] = new VertexPositionColor(v2, color);
            cubeLineVertices[7] = new VertexPositionColor(new Vector3(v1.X, v2.Y, v2.Z), color);

            short[] cubeLineIndices = { 0, 1, 1, 2, 2, 3, 3, 0, 4, 5, 5, 6, 6, 7, 7, 4, 0, 4, 1, 5, 2, 6, 3, 7 };

            basicEffect.World = worldMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.Projection = projectionMatrix;
            basicEffect.VertexColorEnabled = true;

            //device.RenderState.FillMode = FillMode.WireFrame;
            //device.RasterizerState.FillMode = FillMode.WireFrame;
            //basicEffect.Begin();
            foreach (var pass in basicEffect.CurrentTechnique.Passes)
            {
                //pass.Begin();
                pass.Apply();
                //device.VertexDeclaration = new VertexDeclaration(device, VertexPositionColor.VertexElements);

                device.DrawUserIndexedPrimitives(PrimitiveType.LineList, cubeLineVertices, 0, 8, cubeLineIndices, 0, 12);
                //pass.End();
            }
            //basicEffect.End();
            //device.RenderState.FillMode =
            //device.RasterizerState.FillMode = FillMode.Solid;
        }
        #endregion

        #region DrawSphereSpikes
        public static void DrawSphereSpikes(BoundingSphere sphere, GraphicsDevice device, BasicEffect basicEffect, Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix)
        {
            var up = sphere.Center + sphere.Radius * Vector3.Up;
            var down = sphere.Center + sphere.Radius * Vector3.Down;
            var right = sphere.Center + sphere.Radius * Vector3.Right;
            var left = sphere.Center + sphere.Radius * Vector3.Left;
            var forward = sphere.Center + sphere.Radius * Vector3.Forward;
            var back = sphere.Center + sphere.Radius * Vector3.Backward;

            var sphereLineVertices = new VertexPositionColor[6];
            sphereLineVertices[0] = new VertexPositionColor(up, Color.White);
            sphereLineVertices[1] = new VertexPositionColor(down, Color.White);
            sphereLineVertices[2] = new VertexPositionColor(left, Color.White);
            sphereLineVertices[3] = new VertexPositionColor(right, Color.White);
            sphereLineVertices[4] = new VertexPositionColor(forward, Color.White);
            sphereLineVertices[5] = new VertexPositionColor(back, Color.White);

            basicEffect.World = worldMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.Projection = projectionMatrix;
            //basicEffect.VertexColorEnabled = true;
            //basicEffect.Begin();
            foreach (var pass in basicEffect.CurrentTechnique.Passes)
            {
                // pass.Begin();
                pass.Apply();
                //device.VertexDeclaration = new VertexDeclaration(device, VertexPositionColor.VertexElements);
                device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, sphereLineVertices, 0, 3);
                //pass.End();
            }
            //basicEffect.End();

        }
        #endregion

        public static VertexPositionColor[] VerticesFromVector3List(List<Vector3> pointList, Color color)
        {
            var vertices = new VertexPositionColor[pointList.Count];

            var i = 0;
            foreach (var p in pointList)
                vertices[i++] = new VertexPositionColor(p, color);

            return vertices;
        }

        #region CreateBoxFromSphere
        public static BoundingBox CreateBoxFromSphere(BoundingSphere sphere)
        {
            var radius = sphere.Radius;
            var outerPoint = new Vector3(radius, radius, radius);

            var p1 = sphere.Center + outerPoint;
            var p2 = sphere.Center - outerPoint;

            return new BoundingBox(p1, p2);
        }
        #endregion

        public static float GetAngleFromVector2(Vector2 vector)
        {
            vector.Normalize();

            var angle = (float)Math.Acos(vector.Y);
            if (-vector.X < 0.0f)
                angle = -angle;
            return angle;
        }

        #region Clerp
        public static float Clerp(float start, float end, float value)
        {
            var min = 0.0f;
            var max = 360.0f;

            var half = Math.Abs((max - min) / 2.0f);//half the distance between min and max
            var retval = 0.0f;
            var diff = 0.0f;

            if ((end - start) < -half)
            {
                diff = ((max - start) + end) * value;
                retval = start + diff;
            }
            else if ((end - start) > half)
            {
                diff = -((max - end) + start) * value;
                retval = start + diff;
            }
            else retval = start + (end - start) * value;

            // Debug.Log("Start: "  + start + "   End: " + end + "  Value: " + value + "  Half: " + half + "  Diff: " + diff + "  Retval: " + retval);
            return retval;
        }
        #endregion

        public static Vector3 Multiply(Vector3 v, Matrix m)
        {
            return
                new Vector3(v.X * m.M11 + v.Y * m.M21 + v.Z * m.M31 + m.M41,
                            v.X * m.M12 + v.Y * m.M22 + v.Z * m.M32 + m.M42,
                            v.X * m.M13 + v.Y * m.M23 + v.Z * m.M33 + m.M43);
        }

        //public static Matrix3 Abs(Matrix3 m3)
        //{
        //    Matrix3 absMatrix = new Matrix3(0);

        //    for (int r = 0; r < 3; r++)
        //    {
        //        for (int c = 0; c < 3; c++)
        //        {
        //            absMatrix[r, c] = Math.Abs(m3[r, c]);
        //        }
        //    }

        //    return absMatrix;
        //}
    }
}
