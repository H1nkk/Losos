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
using StbImageSharp;

namespace Losos
{
    class Floor: Mesh
    {
        private float dz;
        private float speed;
        private float z;

        public Floor(string textPath, List<Vector3> v, uint[] inds, List<Vector2> texCrds, Shader sh, Vector3 position, float scale = 1.0f, float speed = 0f) : base(textPath, v, inds, texCrds, sh, position, scale)
        {
            this.speed = speed;
            z = position.Z;
        }

        public Vector3 getPos()
        {
            return new Vector3(position.X, position.Y, position.Z + dz); 
        }

        public new void Draw() 
        {
            GL.BindTexture(TextureTarget.Texture2D, textureID);

            //Transformation
            model = Matrix4.Identity;
            model *= Matrix4.CreateScale(scale);
            model *= Matrix4.CreateTranslation(0f, (float)Math.Sin(yHov) / 3f, 0f);

            dz += 0.01f * speed;
            Matrix4 translation = Matrix4.CreateTranslation(new Vector3(position.X , position.Y, position.Z + dz));
            if (position.Z + dz >= 48f)
            {
                position.Z = -48f;
                dz = 0.0f;
                position.Y += 2f;
            }
            model *= translation;

            shader.UseShader();

            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }


        protected new void Prepare()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureID);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.MirroredRepeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.MirroredRepeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);

            StbImage.stbi_set_flip_vertically_on_load(1);

            if (!File.Exists(texturePath))
                throw new FileNotFoundException("Текстура не найдена!");

            ImageResult boxTexture = ImageResult.FromStream(File.OpenRead(texturePath), ColorComponents.RedGreenBlueAlpha);
            if (boxTexture.Data == null)
                throw new Exception("Текстура не загружена или повреждена!");

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, boxTexture.Width, boxTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, boxTexture.Data);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            //Create VAO 
            VAO = GL.GenVertexArray();
            //Create VBO 
            VBO = GL.GenBuffer();
            //Bind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            //Copy vertices data to the buffer 
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * Vector3.SizeInBytes * sizeof(float), vertices.ToArray(), BufferUsageHint.StaticDraw);
            //Bind the VAO 
            GL.BindVertexArray(VAO);
            //Bind a slot number 0 
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(VAO, 0);
            //Unbind the VBO 
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);

            textureVBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords.Count * Vector3.SizeInBytes * sizeof(float), texCoords.ToArray(), BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);

            GL.EnableVertexArrayAttrib(VAO, 1);

            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }
        public void setSpeed(float nSpeed)
        {
            speed = nSpeed;
        }
    }
}
