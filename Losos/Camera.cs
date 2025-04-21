﻿using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Losos
{
    internal class Camera
    {
        private float SPEED = 8f;
        private int SCREENWIDTH;
        private int SCREENHEIGHT;
        private float SENSITIVITY = 100f;
        public Vector3 position;
        Vector3 up = Vector3.UnitY;
        Vector3 front = -Vector3.UnitZ;
        Vector3 right = Vector3.UnitX;
        public float pitch; // вернуть private
        public float yaw;// вернуть private
        private int firstTenMoves = 0;
        public Vector2 lastPos;
        Vector3 defaultPosition;

        bool isJumping = false;
        bool isDead = false;
        public float jumpVelocity = 5f;
        const float jumpForce = 5f; // Сила прыжка
        const float gravity = -9.8f; // Гравитация
        float neededY;

        public Camera(int width, int height, Vector3 position, Vector3 target, float pitch = -41f, float yaw = 270f)
        {
            SCREENWIDTH = width;
            SCREENHEIGHT = height;
            this.position = position;
            defaultPosition = position;
            this.pitch = pitch;
            this.yaw = yaw;
            UpdateVectors();

            neededY = defaultPosition.Y;
            lastPos = Vector2.Zero;
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(position, position + front, up);
        }

        public Matrix4 GetProjection()
        {
            return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60f), SCREENWIDTH / SCREENHEIGHT, 0.1f, 100f);
        }
        public void InputController(KeyboardState input, MouseState mouse, FrameEventArgs e)
        {
            if (input.IsKeyDown(Keys.Space) && !isJumping)
            {
                isJumping = true;
                jumpVelocity = 5f;
            }
            /*if (input.IsKeyDown(Keys.W))
            {
                position += front * SPEED * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.A))
            {
                position -= right * SPEED * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.S))
            {
                position -= front * SPEED * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.D))
            {
                position += right * SPEED * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.Space))
            {
                position += up * SPEED * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                position -= up * SPEED * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.F))
            {
                pitch = -41f;
                yaw = 270f;
                position = defaultPosition;
            }
            if (firstTenMoves < 10)
            {
                lastPos = new Vector2(mouse.X, mouse.Y);
                firstTenMoves++;
                this.pitch = -41f;
                this.yaw = 270f;
            }
            else
            {
                var deltaX = mouse.X - lastPos.X;
                var deltaY = mouse.Y - lastPos.Y;
                lastPos = new Vector2(mouse.X, mouse.Y);
                yaw += deltaX * SENSITIVITY * (float)e.Time;
                pitch -= deltaY * SENSITIVITY * (float)e.Time;
                if (pitch > 89f)
                {
                    pitch = 89f;
                }
                if (pitch < -89f)
                {
                    pitch = -89f;
                }
            }*/
            UpdateVectors();
        }
        public void Update(KeyboardState input, MouseState mouse, FrameEventArgs e)
        {
            float deltaTime = (float)e.Time;

            // Обработка прыжка
            if (isJumping && !isDead)
            {
                // Применяем гравитацию
                jumpVelocity += gravity * deltaTime;
                position.Y += jumpVelocity * deltaTime;

                // Проверяем, приземлились ли
                if (position.Y <= neededY) // 0f - уровень земли
                {
                    position.Y = neededY;
                    isJumping = false;
                }
            }

            if (isDead)
            {
                position.Y = (float)Math.Floor(position.Y);
            }


            InputController(input, mouse, e);
        }
        private void UpdateVectors()
        {
            front.X = MathF.Cos(MathHelper.DegreesToRadians(pitch)) * MathF.Cos(MathHelper.DegreesToRadians(yaw));
            front.Y = MathF.Sin(MathHelper.DegreesToRadians(pitch));
            front.Z = MathF.Cos(MathHelper.DegreesToRadians(pitch)) * MathF.Sin(MathHelper.DegreesToRadians(yaw));
            front = Vector3.Normalize(front);
            right = Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
            up = Vector3.Normalize(Vector3.Cross(right, front));
        }

        public void Revive()
        {
            isDead = false;
            position = defaultPosition;
            neededY = defaultPosition.Y;
        }

        public void Ascend()
        {
            neededY += 1f;
        }

    }
}
