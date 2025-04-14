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
using static System.Net.Mime.MediaTypeNames;


namespace Losos
{
    internal class Obstacle : Mesh
    {
        private float speed;
        private float dz = 0.0f;
        public Obstacle(string textPath, List<Vector3> v, uint[] inds, List<Vector2> texCrds, Shader sh, Vector3 position, float scale = 1, float speed = 1f) : base(textPath, v, inds, texCrds, sh, position, scale)
        {
            this.speed = speed;
        }

        public void setSpeed(float speed)
        {
            this.speed = speed;
        }

        public new void Draw()
        {
            GL.BindTexture(TextureTarget.Texture2D, textureID);

            //Transformation
            model = Matrix4.Identity;
            model *= Matrix4.CreateScale(scale);


            dz += 0.01f * speed;
            Matrix4 translation = Matrix4.CreateTranslation(new Vector3(position.X, position.Y, position.Z + dz));
            if (position.Z + dz >= 48f)
            {
                position.Z = -48f;
                dz = 0.0f;
            }
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
            return position.Z + dz;
        }


    }
}
