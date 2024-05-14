using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Shooter
{
    internal class Camera
    {
        private bool w = true;
        private float SPEED = 8f;
        private float SCREENWIDTH;
        private float SCREENHEIGHT;
        private float SENSITIVITY = 180f;

        public Vector3 position;
        
        Vector3 right = Vector3.UnitX;
        Vector3 up = Vector3.UnitY;
        Vector3 front = -Vector3.UnitZ;

        // --- view rotations ---
        private float pitch = -10;
        private float yaw = 90;

        private bool firstMove = true;
        public Vector2 lastPos;

        public Camera(float width, float height, Vector3 position_) {
            SCREENHEIGHT = height;
            SCREENWIDTH = width;
            position = position_;
        }

        public Matrix4 getViewMatrix() {
            return Matrix4.LookAt(position, position + front, up);     
        }

        public Matrix4 GetProjectionMatrix() {
            return Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45.0f), SCREENWIDTH / SCREENHEIGHT, 0.1f, 100.0f);
        }


        private void UpdateVectors()
        {
            if (pitch > 89.0f)
            {
                pitch = 89.0f;
            }
            if (pitch < -89.0f)
            {
                pitch = -89.0f;
            }

            front.X = MathF.Cos(MathHelper.DegreesToRadians(pitch)) * MathF.Cos(MathHelper.DegreesToRadians(yaw));

            front.Y = MathF.Sin(MathHelper.DegreesToRadians(pitch));

            front.Z = MathF.Cos(MathHelper.DegreesToRadians(pitch)) * MathF.Sin(MathHelper.DegreesToRadians(yaw));

            front = Vector3.Normalize(front);
            right = Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
            up = Vector3.Normalize(Vector3.Cross(right, front));
 
        }


        public void InputController(KeyboardState input, MouseState mouse, FrameEventArgs e) {

            
            if (input.IsKeyDown(Keys.G))
            {
                w = !w;
            }
            if (w)
            { 
                if (input.IsKeyDown(Keys.W))
                {
                    //do something if Key is pressed
                    position += front * SPEED * (float)e.Time;
                }
                if (input.IsKeyDown(Keys.A))
                {
                    //do something if Key is pressed
                    position -= right * SPEED * (float)e.Time;
                }
                if (input.IsKeyDown(Keys.S))
                {
                    //do something if Key is pressed
                    position -= front * SPEED * (float)e.Time;
                }
                if (input.IsKeyDown(Keys.D))
                {
                    //do something if Key is pressed
                    position += right * SPEED * (float)e.Time;
                }
                if (input.IsKeyDown(Keys.Space))
                {
                    //do something if Key is pressed
                    position.Y += SPEED * (float)e.Time;
                }
                if (input.IsKeyDown(Keys.LeftShift))
                {
                    //do something if Key is pressed
                    position.Y -= SPEED * (float)e.Time;
                }



                if (firstMove)
                {
                    lastPos = new Vector2(mouse.X, mouse.Y);
                    firstMove = false;
                }
                else
                {
                    var deltaX = mouse.X - lastPos.X;
                    var deltaY = mouse.Y - lastPos.Y;
                    lastPos = new Vector2(mouse.X, mouse.Y);

                    yaw += deltaX * SENSITIVITY * (float)e.Time;
                    pitch -= deltaY * SENSITIVITY * (float)e.Time;
                }

            }





            UpdateVectors();

        }

        public void Update(KeyboardState input, MouseState mouse, FrameEventArgs e)
        {
            InputController(input, mouse, e);
        }


    }
}
