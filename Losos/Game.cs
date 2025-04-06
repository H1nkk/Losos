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


namespace ConsoleApp1
{
    internal class Game : GameWindow
    {
        int width, height;

        float[] vertices = 
        {
            -0.5f, 0.5f, 0f, // top left vertex - 0
            0.5f, 0.5f, 0f, // top right vertex - 1
            0.5f, -0.5f, 0f, // bottom right vertex - 2
            -0.5f, -0.5f, 0f // bottom left vertex - 3
        };
        float[] texCoords =
        {
                0f, 1f,
                1f, 1f,
                1f, 0f,
                0f, 0f
        };
        uint[] indices =
        {
            0, 1, 2, //top triangle
            2, 3, 0 //bottom triangle
        };
        int VAO;
        int VBO;
        int EBO;
        int textureVBO;
        int textureID;

        Shader shader;

        public Game(int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            this.CenterWindow(new Vector2i(width, height));
            this.height = height;
            this.width = width;
            shader = new Shader();
        }

        protected override void OnLoad()
        {

            textureID = GL.GenTexture();

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureID);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);

            StbImage.stbi_set_flip_vertically_on_load(1);

            string texturePath = "../../../Textures/unrealSF.png";
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
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length *
            sizeof(float), vertices, BufferUsageHint.StaticDraw);
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
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords.Length * sizeof(float), texCoords, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);

            GL.EnableVertexArrayAttrib(VAO, 1);

            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            shader.LoadShader();

            base.OnLoad();
        }

        protected override void OnUnload()
        {
            GL.DeleteVertexArray(VAO);
            GL.DeleteBuffer(VBO);
            GL.DeleteBuffer(EBO);
            // GL.DeleteBuffer(textureVBO); // моё нововведение 
            GL.DeleteTexture(textureID);
            shader.DeleteShader();
            base.OnUnload();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.BindTexture(TextureTarget.Texture2D, textureID);

            shader.UseShader();
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            Console.WriteLine(string.Join(", ", texCoords)); // debug thing
            Context.SwapBuffers();

            base.OnRenderFrame(args);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            base.OnUpdateFrame(args);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            this.width = e.Width;
            this.height = e.Height;
        }

        public static string LoadShaderSource(string filepath)
        {

            return Shader.LoadShaderSource(filepath);
        }


    }
}
