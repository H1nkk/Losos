using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Audio.OpenAL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Losos
{
    class Coin: Mesh
    {
        public Coin(string textPath, List<Vector3> v, uint[] inds, List<Vector2> texCrds, Shader sh, Vector3 position, float scale = 1) : base(textPath, v, inds, texCrds, sh, position, scale)
        {

        }

        public new void Draw()
        {
            GL.BindTexture(TextureTarget.Texture2D, textureID);

            //Transformation
            model = Matrix4.Identity;
            model *= Matrix4.CreateScale(scale);
             
            //Hovering
            model *= Matrix4.CreateRotationX(yRot * 0.0001f);
            model *= Matrix4.CreateRotationY(yRot * 0.0001f);
            yRot += 0.5f;
            yHov += 0.001f;
            model *= Matrix4.CreateTranslation(0f, (float)Math.Sin(yHov) / 3f, 0f);

            Matrix4 translation = Matrix4.CreateTranslation(position);
            model *= translation;

            shader.UseShader();

            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }
    }
}
