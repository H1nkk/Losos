using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Losos;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using StbImageSharp;


namespace Losos
{
    internal class Game : GameWindow
    {
        int width, height;

        List<Vector2> texCoords = new List<Vector2>()
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
        List<Vector2> fishTexCoords = new List<Vector2>()
        {
            new Vector2(0.5f, 0.5f), // 0
            new Vector2(0.7f, 0.6f), // 1
            new Vector2(1.0f, 0.5f), // 2
            new Vector2(0.7f, 0.4f), // 3

            new Vector2(0.5f, 0.5f), // 4
            new Vector2(0.7f, 0.6f), // 5
            new Vector2(1.0f, 0.5f), // 6
            new Vector2(0.7f, 0.4f), // 7

            new Vector2(1.1f, 0.6f), // 8
            new Vector2(1.2f, 0.5f), // 9
            new Vector2(1.1f, 0.4f), // 10

            new Vector2(0.6f, 0.8f), // 11
            new Vector2(0.6f, 0.2f), // 12
            new Vector2(0.2f, 0.2f)  // 13
        };
        List<Vector2> bgTexCoords = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };
        List<Vector2> floorTexCoords = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f)
        };

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
        uint[] fishIndices = new uint[]
        {
            // Тело (левая и правая стороны)
            0, 1, 2,    0, 2, 3,
            4, 6, 5,    4, 7, 6,

            // Грани тела
            0, 1, 5,    0, 5, 4,
            1, 2, 6,    1, 6, 5,
            2, 3, 7,    2, 7, 6,
            3, 0, 4,    3, 4, 7,

            // Хвост
            2, 8, 9,    2, 9, 10,    // левая сторона
            6, 12, 11,  6, 13, 12,   // правая сторона

            // Грани хвоста
            8, 11, 12,  8, 12, 9,
            9, 12, 13,  9, 13, 10,
            10, 13, 11, 10, 11, 8,
        };
        uint[] bgIndices = new uint[]
        {

            0, 1, 2,
            2, 3, 0,
        };
        uint[] floorIndices =
        {
            0, 1, 2,
            2, 3, 0,

            4, 5, 6,
            6, 7, 4,
        };

        List<Vector3> vertices = new List<Vector3>()
        {
            // передняя грань
            new Vector3(-0.5f, 0.5f, 0.5f), //top-left vertice
            new Vector3( 0.5f, 0.5f, 0.5f), //top-right vertice
            new Vector3( 0.5f, -0.5f, 0.5f), //bottom-right vertice
            new Vector3(-0.5f, -0.5f, 0.5f), //bottom-left vertice

            // правая грань 
            new Vector3( 0.5f,  0.5f,  0.5f), // top-left
            new Vector3( 0.5f,  0.5f, -0.5f), // top-right
            new Vector3( 0.5f, -0.5f, -0.5f), // bottom-right
            new Vector3( 0.5f, -0.5f,  0.5f), // bottom-left 

            // задняя грань
            new Vector3( 0.5f,  0.5f, -0.5f), // top-left
            new Vector3(-0.5f,  0.5f, -0.5f), // top-right
            new Vector3(-0.5f, -0.5f, -0.5f), // bottom-right
            new Vector3( 0.5f, -0.5f, -0.5f), // bottom-left

            // левая грань 
            new Vector3(-0.5f,  0.5f, -0.5f), // top-left
            new Vector3(-0.5f,  0.5f,  0.5f), // top-right
            new Vector3(-0.5f, -0.5f,  0.5f), // bottom-right
            new Vector3(-0.5f, -0.5f, -0.5f), // bottom-left

            // верхняя грань 
            new Vector3(-0.5f,  0.5f, -0.5f), // top-left
            new Vector3( 0.5f,  0.5f, -0.5f), // top-right
            new Vector3( 0.5f,  0.5f,  0.5f), // bottom-right
            new Vector3(-0.5f,  0.5f,  0.5f), // bottom-left

            // нижняя грань
            new Vector3(-0.5f, -0.5f,  0.5f), // top-left
            new Vector3( 0.5f, -0.5f,  0.5f), // top-right
            new Vector3( 0.5f, -0.5f, -0.5f), // bottom-right
            new Vector3(-0.5f, -0.5f, -0.5f)  // bottom-left
        };
        List<Vector3> fishVertices = new List<Vector3>()
        {
            // тело - лево
            new Vector3(0.0f,  0.0f, -0.1f), // 0
            new Vector3(0.5f,  0.2f, -0.1f), // 1
            new Vector3(1.0f,  0.0f, -0.1f), // 2
            new Vector3(0.5f, -0.2f, -0.1f), // 3

            // тело - право
            new Vector3(0.0f,  0.0f,  0.1f), // 4
            new Vector3(0.5f,  0.2f,  0.1f), // 5
            new Vector3(1.0f,  0.0f,  0.1f), // 6
            new Vector3(0.5f, -0.2f,  0.1f), // 7

            // хвост - лево
            new Vector3(1.2f,  0.3f, -0.05f), // 8
            new Vector3(1.4f,  0.0f, -0.05f), // 9
            new Vector3(1.2f, -0.3f, -0.05f), // 10

            // хвост - право
            new Vector3(1.2f,  0.3f,  0.05f), // 11
            new Vector3(1.4f,  0.0f,  0.05f), // 12
            new Vector3(1.2f, -0.3f,  0.05f), // 13
        };
        List<Vector3> floorVertices = new List<Vector3>()
        {
            // top face
            new Vector3(-8.0f,  1.0f, -24.01f), // back-left
            new Vector3( 8.0f,  1.0f, -24.01f), // back-right
            new Vector3( 8.0f,  1.0f,  24.01f), // front-right
            new Vector3(-8.0f,  1.0f,  24.01f), // front-left

            // 
            new Vector3(-8.0f,  1.0f,  24.0f), // top-left
             new Vector3( 8.0f,  1.0f,  24.0f), // top-right
             new Vector3( 8.0f, -1.0f,  24.0f), // bottom-right
             new Vector3(-8.0f, -1.0f,  24.0f), // bottom-left
        };
        List<Vector3> bgVertices = new List<Vector3>() {
            new Vector3(-0.5f, 0,  0.5f), // top-left
            new Vector3( 0.5f, 0,  0.5f), // top-right
            new Vector3( 0.5f, 0, -0.5f), // bottom-right
            new Vector3(-0.5f, 0, -0.5f)  // bottom-left
        };

        Shader shader;
        Camera camera;

        Mesh bg;
        Floor floor1;
        Floor floor2;
        Player player;

        float gameSpeed = 10f;
        float dGameSpeed = 0.001f;
        bool gameOver = false;
        float score;

        private double fps;
        private double frameTime;
        private int frameCount;
        private double timeCounter;

        float realSpeed;

        float toGo = 0;
        bool ascended = false;
        public Game(int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            this.CenterWindow(new Vector2i(width, height));
            this.height = height;
            this.width = width;
            shader = new Shader();
        }

        protected override void OnLoad()
        {
            Random random = new();
            score = 0f;

            string fishPath = "../../../Textures/lososfish.png";
            string floorPath1 = "../../../Textures/sand.png";
            string floorPath2 = "../../../Textures/mirroredSand.png";
            string bgPath = "../../../Textures/bluebg.png";
            string boxPath = "../../../Textures/bigBox.png";

            shader.LoadShader();

            floor1 = new Floor(floorPath1, floorVertices, floorIndices, floorTexCoords, shader, new Vector3(0, -1f, -48), 1.0f);
            floor2 = new Floor(floorPath2, floorVertices, floorIndices, floorTexCoords, shader, new Vector3(0, -2f, 0), 1.0f);
            bg = new Mesh(bgPath, vertices, indices, texCoords, shader, new Vector3(0, -4.5f, 0), 100f);
            player = new Player(fishPath, fishVertices, fishIndices, fishTexCoords, shader, new Vector3(0f, 0f, 0.5f), 1f, 5f);

            GL.Enable(EnableCap.DepthTest);

            base.OnLoad();
            camera = new Camera(width, height, new Vector3(0, 3f, 4.5f), new Vector3(0,0,0));
            CursorState = CursorState.Grabbed;
        }

        protected override void OnUnload()
        {

            floor1.Unload();
            floor2.Unload();
            player.Unload();

            shader.DeleteShader();
            base.OnUnload();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(0.2f, 0.6f, 1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            gameSpeed += dGameSpeed;
            realSpeed = (float)MathHelper.Log2(gameSpeed) - 3f;
            floor1.setSpeed(realSpeed);
            floor2.setSpeed(realSpeed);
 
            score += realSpeed * 0.01f;

            shader.SetMatrix4("model", floor1.GetModel());
            floor1.Draw();            
            
            shader.SetMatrix4("model", floor2.GetModel());
            floor2.Draw();
            
            shader.SetMatrix4("model", bg.GetModel());
            bg.Draw();

            shader.SetMatrix4("model", player.GetModel());
            player.Draw();

            Matrix4 view = camera.GetViewMatrix();
            Matrix4 projection = camera.GetProjection();

            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);

            Context.SwapBuffers();

            base.OnRenderFrame(args);
            if (!gameOver)
                FPS(args);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            if (gameOver && KeyboardState.IsKeyDown(Keys.R))
            {
                Restart();
            }

            MouseState mouse = MouseState;
            KeyboardState input = KeyboardState;

            base.OnUpdateFrame(args);
            if (!gameOver)
            {
                camera.Update(input, mouse, args);
                player.Update(input, mouse, args);
            }
            base.OnUpdateFrame(args);


            if (Math.Abs(floor1.getPos().Z - floor1.getLength() / 2 - 1 - player.getZ()) < 1f)
            {
                if (Math.Abs(floor1.getPos().Y + 2 - player.getPos().Y) < 0.5)
                    GameOver();
                else
                {
                    if (!ascended)
                    {
                        player.Ascend();
                        camera.Ascend();
                        toGo = 12f;
                    }
                    ascended = true;
                }
            }
            if (Math.Abs(floor2.getPos().Z - floor1.getLength() / 2 - 1 - player.getZ()) < 1f)
            {
                if (Math.Abs(floor2.getPos().Y + 2 - player.getPos().Y) < 0.5)
                     GameOver();
                else
                {
                    if (!ascended)
                    {
                        player.Ascend();
                        camera.Ascend();
                        toGo = 12f;
                    }
                    ascended = true;
                }
            }

            toGo -= 0.01f * realSpeed;

            if (toGo < 0)
            {
                ascended = false;
            }
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"Pitch: {camera.pitch:F2}    ");
            Console.WriteLine($"Yaw: {camera.yaw:F2}    ");
            Console.WriteLine($"X, Y, Z: {camera.position.X:F2}, {camera.position.Y:F2}, {camera.position.Z:F2}    ");
            Console.WriteLine($"Speed: {gameSpeed:F2}, estimated: {(float)MathHelper.Log2(gameSpeed) - 3f:F2} ");
            Console.WriteLine($"Floor1 X: {floor1.getPos().X:F2}, Y:  {floor1.getPos().Y + 2:F2}, Z:  {floor1.getPos().Z:F2} ");
            Console.WriteLine($"Floor2 X: {floor2.getPos().X:F2}, Y:  {floor2.getPos().Y + 2:F2}, Z:  {floor2.getPos().Z:F2} ");
            
            Console.WriteLine($"Player X : {player.getX():F2}, Y: {player.getY():F2},  Z: {player.getZ():F2} ");
            Console.WriteLine($"Player jumping : {player.getJumpng()}, velocity : {player.jumpVelocity}");
            Console.WriteLine($"Score: {score:F0}    ");
            Console.WriteLine($"TOGO: {toGo:F0}    ");

            if (gameOver)
                Title = "GAME OVER. PRESS \"R\" TO RESTART";
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

        private void GameOver()
        {
            gameOver = true;
            gameSpeed = 8f; // 2^3 -> realspeed = 0
            dGameSpeed = 0f;
        }

        private void Restart()
        {
            player.Revive();
            camera.Revive();
            string floorPath1 = "../../../Textures/sand.png";
            string floorPath2 = "../../../Textures/mirroredSand.png";
            floor1 = new Floor(floorPath1, floorVertices, floorIndices, floorTexCoords, shader, new Vector3(0, -1f, -48), 1.0f);
            floor2 = new Floor(floorPath2, floorVertices, floorIndices, floorTexCoords, shader, new Vector3(0, -2f, 0), 1.0f);
            Title = "NEW GAME STARTED";
            gameOver = false;
            gameSpeed = 10f;
            dGameSpeed = 0.001f;
            score = 0f;
            toGo = 0;
            Random random = new Random();

        }

        private void FPS(FrameEventArgs e)
        {
            timeCounter += e.Time;
            frameCount++;

            if (timeCounter >= 1.0)
            {
                fps = frameCount / timeCounter;
                Title = $"Game - FPS: {fps:0.0}";
                frameCount = 0;
                timeCounter = 0;
            }
        }
    }
}
