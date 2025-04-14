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
    internal class Player : Mesh
    {
        float SPEED;
/*        Vector3 front = -Vector3.UnitZ;*/
        Vector3 right = Vector3.UnitX;
        public Player(string textPath, List<Vector3> v, uint[] inds, List<Vector2> texCrds, Shader sh, Vector3 position, float scale = 1, float speed = 1f) : base(textPath, v, inds, texCrds, sh, position, scale)
        {
            this.SPEED = speed;
        }

        public new void Draw()
        {
            GL.BindTexture(TextureTarget.Texture2D, textureID);

            //Transformation
            model = Matrix4.Identity;
            model *= Matrix4.CreateScale(scale);

            //Hovering
            model *= Matrix4.CreateRotationY(-MathHelper.Pi / 2);
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

        public float getX()
        {
            return position.X;
        }

        public float getZ()
        {
            return position.Z;
        }

        public void InputController(KeyboardState input, MouseState mouse, FrameEventArgs e)
        {
            if (input.IsKeyDown(Keys.Left) || input.IsKeyDown(Keys.A))
            {
                position -= right * SPEED * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.Right) || input.IsKeyDown(Keys.D))
            {
                position += right * SPEED * (float)e.Time;
            }
            if (position.X > 3f)
            {
                position.X = 3f;
            }
            if (position.X < -3f)
            {
                position.X = -3f;
            }
        }

        public void Update(KeyboardState input, MouseState mouse, FrameEventArgs e)
        {
            InputController(input, mouse, e);
        }
    }
}
