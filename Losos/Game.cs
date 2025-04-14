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
            new Vector2(0.6f, 0.2f)  // 12
        };
        List<Vector2> bgTexCoords = new List<Vector2>() // мб indices
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
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

    // Верхний плавник
    1, 14, 15,  // левая сторона
    5, 17, 16,  // правая сторона

    // Грани верхнего плавника
    14, 16, 17, 14, 17, 15,

    // Нижний плавник
    3, 19, 18,  // левая сторона
    7, 20, 21,  // правая сторона

    // Грани нижнего плавника
    18, 20, 21, 18, 21, 19
        };
        uint[] bgIndices = new uint[]
        {
            // Передняя грань
            0, 1, 2,
            2, 3, 0,
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
        List<Vector3> fishVertices = new List<Vector3>()
{
    // Тело (левая сторона)
    new Vector3(0.0f,  0.0f, -0.1f), // 0
    new Vector3(0.5f,  0.2f, -0.1f), // 1
    new Vector3(1.0f,  0.0f, -0.1f), // 2
    new Vector3(0.5f, -0.2f, -0.1f), // 3

    // Тело (правая сторона)
    new Vector3(0.0f,  0.0f,  0.1f), // 4
    new Vector3(0.5f,  0.2f,  0.1f), // 5
    new Vector3(1.0f,  0.0f,  0.1f), // 6
    new Vector3(0.5f, -0.2f,  0.1f), // 7

    // Хвост (левая сторона)
    new Vector3(1.2f,  0.3f, -0.05f), // 8
    new Vector3(1.4f,  0.0f, -0.05f), // 9
    new Vector3(1.2f, -0.3f, -0.05f), // 10

    // Хвост (правая сторона)
    new Vector3(1.2f,  0.3f,  0.05f), // 11
    new Vector3(1.4f,  0.0f,  0.05f), // 12
    new Vector3(1.2f, -0.3f,  0.05f), // 13

    // Верхний плавник (левая сторона)
    new Vector3(0.3f,  0.4f, -0.05f), // 14
    new Vector3(0.5f,  0.3f, -0.05f), // 15

    // Верхний плавник (правая сторона)
    new Vector3(0.3f,  0.4f,  0.05f), // 16
    new Vector3(0.5f,  0.3f,  0.05f), // 17

    // Нижний плавник (левая сторона)
    new Vector3(0.3f, -0.4f, -0.05f), // 18
    new Vector3(0.5f, -0.3f, -0.05f), // 19

    // Нижний плавник (правая сторона)
    new Vector3(0.3f, -0.4f,  0.05f), // 20
    new Vector3(0.5f, -0.3f,  0.05f), // 21
};
        List<Vector3> floorVertices = new List<Vector3>()
        {
            // front face (передняя грань)
            new Vector3(-8.0f,  1.0f,  24.0f), // top-left
            new Vector3( 8.0f,  1.0f,  24.0f), // top-right
            new Vector3( 8.0f, -1.0f,  24.0f), // bottom-right
            new Vector3(-8.0f, -1.0f,  24.0f), // bottom-left

            // right face (правая грань)
            new Vector3( 8.0f,  1.0f,  24.0f), // front-top
            new Vector3( 8.0f,  1.0f, -24.0f), // back-top
            new Vector3( 8.0f, -1.0f, -24.0f), // back-bottom
            new Vector3( 8.0f, -1.0f,  24.0f), // front-bottom

            // back face (задняя грань)
            new Vector3( 8.0f,  1.0f, -24.0f), // top-right
            new Vector3(-8.0f,  1.0f, -24.0f), // top-left
            new Vector3(-8.0f, -1.0f, -24.0f), // bottom-left
            new Vector3( 8.0f, -1.0f, -24.0f), // bottom-right

            // left face (левая грань)
            new Vector3(-8.0f,  1.0f, -24.0f), // back-top
            new Vector3(-8.0f,  1.0f,  24.0f), // front-top
            new Vector3(-8.0f, -1.0f,  24.0f), // front-bottom
            new Vector3(-8.0f, -1.0f, -24.0f), // back-bottom

            // top face (верхняя грань)
            new Vector3(-8.0f,  1.0f, -24.0f), // back-left
            new Vector3( 8.0f,  1.0f, -24.0f), // back-right
            new Vector3( 8.0f,  1.0f,  24.0f), // front-right
            new Vector3(-8.0f,  1.0f,  24.0f), // front-left

            // bottom face (нижняя грань)
            new Vector3(-8.0f, -1.0f,  24.0f), // front-left
            new Vector3( 8.0f, -1.0f,  24.0f), // front-right
            new Vector3( 8.0f, -1.0f, -24.0f), // back-right
            new Vector3(-8.0f, -1.0f, -24.0f)  // back-left
        };
        List<Vector3> bgVertices = new List<Vector3>() {             
            // Нижняя грань (Bottom)
            new Vector3(-0.5f, 0,  0.5f), // top-left
            new Vector3( 0.5f, 0,  0.5f), // top-right
            new Vector3( 0.5f, 0, -0.5f), // bottom-right
            new Vector3(-0.5f, 0, -0.5f)  // bottom-left
        };

        Shader shader;
        Camera camera;

        // Mesh fish1;
        Mesh bg;
        // Coin cube1;
        Floor floor1;
        Floor floor2;
        Player player;
        List<Obstacle> obstacles;

        int obstacleCount = 10;
        float gameSpeed = 10f;
        float dGameSpeed = 0.001f;
        bool gameOver = false;
        float score;

        private double _fps;
        private double _frameTime;
        private int _frameCount;
        private double _timeCounter;

        public Game(int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            this.CenterWindow(new Vector2i(width, height));
            this.height = height;
            this.width = width;
            shader = new Shader();
            obstacles = new List<Obstacle>();
        }

        protected override void OnLoad()
        {
            Random random = new Random();
            score = 0f;

            string fishPath = "../../../Textures/fishyfishy.png";
            string floorPath1 = "../../../Textures/sand.png";
            string floorPath2 = "../../../Textures/mirroredSand.png";
            string bgPath = "../../../Textures/bluebg.png";
            string boxPath = "../../../Textures/bigBox.png";

            shader.LoadShader();

            floor1 = new Floor(floorPath1, floorVertices, indices, texCoords, shader, new Vector3(0, -3f, -48), 1.0f);
            floor2 = new Floor(floorPath2, floorVertices, indices, texCoords, shader, new Vector3(0, -3f, 0), 1.0f);
            bg = new Mesh(bgPath, bgVertices, bgIndices, bgTexCoords, shader, new Vector3(0, -4.5f, 0), 100f);
            player = new Player(fishPath, fishVertices, fishIndices, fishTexCoords, shader, new Vector3(0f, -0.5f, 0.5f), 1f, 5f);

            for (int i = 0; i < obstacleCount; i++)
            {
                float newX = (float)(-3.3f + random.NextDouble() * 6.6f), newZ = -14f - (float)(random.NextDouble() * 34f);
                while (true)
                {
                    bool ok = true;
                    for (int j = 0; j < i; j++)
                    {
                        if (Math.Abs(obstacles[j].getZ() - newZ) < 2f)
                            if (Math.Abs(obstacles[j].getX() - newX) < 1f)
                                ok = false;
                    }
                    if (ok)
                    {
                        break; // new obstacle can be added
                    }
                    newX = (float)(-3.3f + random.NextDouble() * 6.6f);
                    newZ = -14f - (float)(random.NextDouble() * 34f);
                }
                obstacles.Add(new Obstacle(boxPath, vertices, indices, texCoords, shader, new Vector3(newX, 0f, newZ)));
            }

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
            float realSpeed = (float)MathHelper.Log2(gameSpeed) - 3f;
            floor1.setSpeed(realSpeed);
            floor2.setSpeed(realSpeed);
            for (int i = 0; i < obstacles.Count; i++)
                obstacles[i].setSpeed(realSpeed);

            score += realSpeed * 0.01f;

            shader.SetMatrix4("model", floor1.GetModel());
            floor1.Draw();            
            
            shader.SetMatrix4("model", floor2.GetModel());
            floor2.Draw();
            
            shader.SetMatrix4("model", bg.GetModel());
            bg.Draw();

            shader.SetMatrix4("model", player.GetModel());
            player.Draw();

            for (int i = 0; i < obstacles.Count; i++)
            {
                shader.SetMatrix4("model", obstacles[i].GetModel());
                obstacles[i].Draw();
            }

            Matrix4 view = camera.GetViewMatrix();
            Matrix4 projection = camera.GetProjection();

            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);

            Context.SwapBuffers();

            base.OnRenderFrame(args);
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

            for (int i = 0; i < obstacles.Count; i++)
            {
                Random random = new Random();

                if (Math.Abs(obstacles[i].getX() - player.getX()) <= 0.6f)
                {
                    if (Math.Abs(obstacles[i].getZ() - player.getZ()) < 0.05f)
                    {
                        GameOver();
                    }
                }

                if (obstacles[i].getZ() > 20f) // 30f
                {
                    float newX = (float)(-3.3f + random.NextDouble() * 6.6f), newZ = -14f - (float)(random.NextDouble() * 34f);
                    while (true)
                    {
                        bool ok = true;
                        for (int j = 0; j < obstacleCount; j++)
                        {
                            if (i == j) continue;
                            if (Math.Abs(obstacles[j].getZ() - newZ) < 2f)
                                if (Math.Abs(obstacles[j].getX() - newX) < 1f)
                                    ok = false;
                        }
                        if (ok)
                        {
                            break; // new obstacle can be added
                        }
                        newX = (float)(-3.3f + random.NextDouble() * 6.6f);
                        newZ = -14f - (float)(random.NextDouble() * 34f);
                    }
                    obstacles[i].setX(newX);
                    obstacles[i].setZ(newZ);
                }
            }

            base.OnUpdateFrame(args);
            camera.Update(input, mouse, args);
            player.Update(input, mouse, args);
            base.OnUpdateFrame(args);

            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"Pitch: {camera.pitch:F2}    ");
            Console.WriteLine($"Yaw: {camera.yaw:F2}    ");
            Console.WriteLine($"X, Y, Z: {camera.position.X:F2}, {camera.position.Y:F2}, {camera.position.Z:F2}    ");
            Console.WriteLine($"Speed: {gameSpeed:F2}, estimated: {(float)MathHelper.Log2(gameSpeed) - 3f:F2} ");
            for (int i = 0; i < obstacleCount; i++)
                 Console.WriteLine($"Obstacle{i} X : {obstacles[i].getX():F2}, Z: {obstacles[i].getZ():F2} ");
            Console.WriteLine($"Player X : {player.getX():F2}, Z: {player.getZ():F2} ");
            Console.WriteLine($"Score: {score:F0}    ");
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
            gameOver = false;
            gameSpeed = 10f;
            dGameSpeed = 0.001f;
            score = 0f;

            string boxPath = "../../../Textures/bigBox.png";
            Random random = new Random();

            for (int i = 0; i < obstacleCount; i++)
            {
                float newX = (float)(-3.3f + random.NextDouble() * 6.6f), newZ = -14f - (float)(random.NextDouble() * 34f);
                while (true)
                {
                    bool ok = true;
                    for (int j = 0; j < i; j++)
                    {
                        if (Math.Abs(obstacles[j].getZ() - newZ) < 2f)
                            if (Math.Abs(obstacles[j].getX() - newX) < 1f)
                                ok = false;
                    }
                    if (ok)
                    {
                        break; // new obstacle can be added
                    }
                    newX = (float)(-3.3f + random.NextDouble() * 6.6f);
                    newZ = -14f - (float)(random.NextDouble() * 34f);
                }
                obstacles[i].setX(newX);
                obstacles[i].setZ(newZ);
            }
        }

        private void FPS(FrameEventArgs e)
        {
            _frameTime = e.Time; // Время кадра в секундах
            _timeCounter += _frameTime;
            _frameCount++;

            if (_timeCounter >= 1.0) // Каждую секунду
            {
                _fps = _frameCount / _timeCounter;
                Title = $"Game - FPS: {_fps:0.0}"; // Выводим в заголовок окна
                _frameCount = 0;
                _timeCounter = 0;
            }
        }
    }
}
