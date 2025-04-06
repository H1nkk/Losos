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

namespace ConsoleApp1
{
    internal class Game : GameWindow
    {
        int width, height;

        float[] vertices = {
        -0.5f, 0.5f, 0f, // top left vertex - 0
        0.5f, 0.5f, 0f, // top right vertex - 1
        0.5f, -0.5f, 0f, // bottom right vertex - 2
        -0.5f, -0.5f, 0f // bottom left vertex - 3
        };
        uint[] indices =
        {
        0, 1, 2, //top triangle
        2, 3, 0 //bottom triangle
        };
        int VAO;
        int VBO;
        int EBO;

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
            GL.BindVertexArray(0);

            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length *
            sizeof(uint), indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            shader.LoadShader();

            //GL.DeleteProgram(shaderProgram);

            base.OnLoad();
        }

        protected override void OnUnload()
        {
            GL.DeleteBuffer(VAO);
            GL.DeleteBuffer(VBO);
            GL.DeleteBuffer(EBO);
            shader.DeleteShader();
            base.OnUnload();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            shader.UseShader();
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length,
            DrawElementsType.UnsignedInt, 0);

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
