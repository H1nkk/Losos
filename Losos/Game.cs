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
        float yRot = 0.1f;
        float yHov= 0f;
        float dy = 0f;
        float numb = 0.002f;

        List<Vector2> texCoords = new List<Vector2>() // мб indices
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),


        };


        /*        float[] texCoords =
                {
                        0f, 1f,
                        1f, 1f,
                        1f, 0f,
                        0f, 0f
                };
        */
        uint[] indices =
        {
            // Передняя грань
            0, 1, 2,
            2, 3, 0,
    
            // Правая грань
            4, 5, 6,
            6, 7, 4,
    
            // Задняя грань
            8, 9, 10,
            10, 11, 8,
    
            // Левая грань
            12, 13, 14,
            14, 15, 12,
    
            // Верхняя грань
            16, 17, 18,
            18, 19, 16,
    
            // Нижняя грань
            20, 21, 22,
            22, 23, 20
        };

        List<Vector3> vertices = new List<Vector3>()
        {
            //front face
            new Vector3(-0.5f, 0.5f, 0.5f), //top-left vertice
            new Vector3( 0.5f, 0.5f, 0.5f), //top-right vertice
            new Vector3( 0.5f, -0.5f, 0.5f), //bottom-right vertice
            new Vector3(-0.5f, -0.5f, 0.5f), //bottom-left vertice

            // Правая грань (Right)
            new Vector3( 0.5f,  0.5f,  0.5f), // top-left
            new Vector3( 0.5f,  0.5f, -0.5f), // top-right
            new Vector3( 0.5f, -0.5f, -0.5f), // bottom-right
            new Vector3( 0.5f, -0.5f,  0.5f), // bottom-left 

            // Задняя грань (Back)
            new Vector3( 0.5f,  0.5f, -0.5f), // top-left
            new Vector3(-0.5f,  0.5f, -0.5f), // top-right
            new Vector3(-0.5f, -0.5f, -0.5f), // bottom-right
            new Vector3( 0.5f, -0.5f, -0.5f), // bottom-left

            // Левая грань (Left)
            new Vector3(-0.5f,  0.5f, -0.5f), // top-left
            new Vector3(-0.5f,  0.5f,  0.5f), // top-right
            new Vector3(-0.5f, -0.5f,  0.5f), // bottom-right
            new Vector3(-0.5f, -0.5f, -0.5f), // bottom-left

            // Верхняя грань (Top)
            new Vector3(-0.5f,  0.5f, -0.5f), // top-left
            new Vector3( 0.5f,  0.5f, -0.5f), // top-right
            new Vector3( 0.5f,  0.5f,  0.5f), // bottom-right
            new Vector3(-0.5f,  0.5f,  0.5f), // bottom-left

            // Нижняя грань (Bottom)
            new Vector3(-0.5f, -0.5f,  0.5f), // top-left
            new Vector3( 0.5f, -0.5f,  0.5f), // top-right
            new Vector3( 0.5f, -0.5f, -0.5f), // bottom-right
            new Vector3(-0.5f, -0.5f, -0.5f)  // bottom-left
        };

        int VAO;
        int VBO;
        int EBO;
        int textureVBO;
        int textureID;
        int i = 0;

        Shader shader;
        Camera camera;

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

            shader.LoadShader();

            GL.Enable(EnableCap.DepthTest);

            base.OnLoad();
            camera = new Camera(width, height, Vector3.Zero);
            CursorState = CursorState.Grabbed;
        }

        protected override void OnUnload()
        {
            GL.DeleteVertexArray(VAO);
            GL.DeleteBuffer(VBO);
            GL.DeleteBuffer(EBO);
            GL.DeleteBuffer(textureVBO); // моё нововведение 
            GL.DeleteTexture(textureID);
            shader.DeleteShader();
            base.OnUnload();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.BindTexture(TextureTarget.Texture2D, textureID);

            //Transformation
            Matrix4 model = Matrix4.CreateRotationY(yRot) * Matrix4.CreateRotationX(yRot / 10f) * Matrix4.CreateTranslation(0f, (float)(yHov),0f);
            yRot += 0.000005f;
            if (i % 10 == 0)
            {
                float dist = 0.002f;
                yHov += dy;
                dy = numb - dist;
                numb += 0.00001f;
                if (numb > dist * 2) numb = 0f;
                i = 0;
            }
            i++;

            Matrix4 view = camera.GetViewMatrix();
            Matrix4 projection = camera.GetProjection();

            Matrix4 translation = Matrix4.Identity;
//            Matrix4 translation = Matrix4.CreateTranslation(0f, 0f, -1f);
            model *= translation;

            int modelLocation = GL.GetUniformLocation(shader.shaderHandle, "model");
            int viewLocation = GL.GetUniformLocation(shader.shaderHandle, "view");
            int projectionLocation = GL.GetUniformLocation(shader.shaderHandle, "projection");

            shader.UseShader();
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);


            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);

            Context.SwapBuffers();

            base.OnRenderFrame(args);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            MouseState mouse = MouseState;
            KeyboardState input = KeyboardState;
            base.OnUpdateFrame(args);
            camera.Update(input, mouse, args);
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
