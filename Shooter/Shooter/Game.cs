﻿using StbImageSharp;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Graphics;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shooter.Graphics;
using Shooter.Objects;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Reflection;

namespace Shooter
{
    // Game class that inherets from the Game Window Class
    internal class Game : GameWindow
    {
        // -- objects --
        BackGround background;
        BackGround win, lose;

        Wall user;
        Stick stick;
        Wall type_0, type_1;
        List<Wall> objects = new List<Wall>();
        List<bool> was = new List<bool>();
        int wall_count = 0;
        // -- objects --

        // angle and stick 

        int space_type = 0;
        double stick_angle = Math.PI / 4;
        int active = 0, counter = 1000;
        ShaderProgram program;
        // camera 
        Camera camera;
        // transform vars
        float yRot = 0f;

        bool need_update = true;
        float base_x, base_y, base_z;
        float user_x, user_y, user_z;
        float extra_x = 0f, extra_y = 0f, extra_z = 0f;
        float v0 = 0, dt = 0;
        // width and height of screen
        int width, height;
        // Constructor that sets the width, height, and calls the base constructor (GameWindow's Constructor) with default args
        public Game(int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            this.width = width;
            this.height = height;

            // center window
            CenterWindow(new Vector2i(width, height));
        }
        // called whenever window is resized
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            this.width = e.Width;
            this.height = e.Height;
        }
        

        // called once when game is started
        protected override void OnLoad()
        {
            base.OnLoad();

            base_x = -12f; user_x = 0f;
            base_y = -1f;  user_y = 0f;
            base_z = -22f; user_z = 0f;
            
            // backgrounds
            background = new BackGround(new Vector3(0, 10, 0), "windows.PNG");
            lose = new BackGround(new Vector3(0, 0, -20), "defeat.jpg");
            win = new BackGround(new Vector3(0, 0, -20), "win.jpg");

            // user
            user = new Wall(new Vector3(base_x, base_y, base_z), 1f, 1f, 1f, 0, "linux.jpg");
            stick = new Stick(new Vector3(base_x+1, base_y+1, base_z), new Vector3(base_x + 5, base_y + 5, base_z), 10000f, "apple.jpg");
            type_0 = new Wall(new Vector3(base_x-2, base_y + 10, base_z), 6f, 2f, 2f, 0, "stick.jpg");
            type_1 = new Wall(new Vector3(base_x-2, base_y + 10, base_z), 6f, 2f, 2f, 0, "force.jpg");
            // walls
            objects.Add(new Wall(new Vector3(0f, -3f, -22f), 60f, 1f, 36f, 1, "windows.PNG"));
            objects.Add(new Wall(new Vector3(-3f, 0.5f ,-22f), 2f, 6f, 10f,1 ,"android.PNG"));
            wall_count = 2;

            // create enemies
            for (float i = -4f; i <= 4; i += 1f)
            {
                for (float j = -4; j <= 4; j += 1)
                {
                    float distance = (float)Math.Sqrt(i * i + j * j);
                    if (distance > 4) continue;
                    Wall enemy = new Wall(new Vector3(11f + i, 2f + j, -22f), 1f, 1f, 1f, 2, "apple.jpg");
                    objects.Add(enemy);
                    was.Add(true);


                    if (distance <= 2)
                    {
                        enemy = new Wall(new Vector3(4f + i, 1f + j, -22f), 1f, 1f, 1f, 2, "compw.jpg");
                        objects.Add(enemy);
                        was.Add(true);
                    }
                }
            }

            active = was.Count();
            program = new ShaderProgram("Default.vert", "Default.frag");

            GL.Enable(EnableCap.DepthTest);
            camera = new Camera(width, height, new Vector3(0f, 3f, 3f));
            CursorState = CursorState.Grabbed;
            
        }
        // called once when game is closed
        protected override void OnUnload()
        {
            base.OnUnload();
            background.Delete();
            user.Delete();
            stick.Delete();
            type_0.Delete();
            type_1.Delete();
            foreach (var enemy in objects)
            {
                enemy.Delete();
            }
            
            program.Delete();
        }
        // called every frame. All rendering happens here
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            // Set the color to fill the screen with
            GL.ClearColor(0f, 0f, 0f, 1f);
            // Fill the screen with the color
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


       
            // transformation matrices
            Matrix4 model = Matrix4.Identity;
            Matrix4 view = camera.getViewMatrix();
            Matrix4 projection = camera.GetProjectionMatrix();


            int modelLocation = GL.GetUniformLocation(program.ID, "model");
            int viewLocation = GL.GetUniformLocation(program.ID, "view");
            int projectionLocation = GL.GetUniformLocation(program.ID, "projection");

            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);

            
            // -- game logic --
            background.Render(program);
            if (space_type != 1)
            {
                type_0.Render(program);
            }
            else type_1.Render(program);
            // render objects
            for (int wall = 0; wall < wall_count; ++wall)
            {
                objects[wall].Render(program);
            }

            for (int enemy = wall_count; enemy < objects.Count; enemy++)
            {
                if (was[enemy - wall_count])
                {
                    objects[enemy].Render(program);
                }
            }

            if (user_x <= -20f || user_x <= -20f || user_x > 50 || user_y > 50)
            {
                reset();
            }

            bool hitted = hit();
            
            if (!hitted && v0 != 0 && space_type == 0)
            { 
                dt += 0.007f;
                user_x = 2 * v0 * dt * (float)Math.Cos(stick_angle);
                user_y = 2 * v0 * dt * (float)Math.Sin(stick_angle) - 0.5f * 9.81f * dt * dt;
            }

            update();
            model = Matrix4.Identity;
            Matrix4 translation = Matrix4.CreateTranslation(user_x + extra_x, user_y + extra_y, user_z);
            model *= translation;
            
            // -- game logic --

            modelLocation = GL.GetUniformLocation(program.ID, "model");
            viewLocation = GL.GetUniformLocation(program.ID, "view");
            projectionLocation = GL.GetUniformLocation(program.ID, "projection");
            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);
            user.Render(program);
            stick.move(stick_angle);
            stick.Render(program);
            // swap the buffers
            Context.SwapBuffers();
            base.OnRenderFrame(args);

            
        }
        // called every frame. All updating happens here
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            MouseState mouse = MouseState;
            KeyboardState input = KeyboardState;
            base.OnUpdateFrame(args);
            camera.Update(input, mouse, args);
        }


        

        protected void reset()
        {
            user_x = 0;
            user_y = 0;
            user_z = 0;
            extra_x = 0;
            extra_y = 0;
            extra_z = 0;
            dt = 0f;
            v0 = 0;
        }

        protected bool hit()
        {
            Vector3 user_pos = new Vector3(base_x + user_x + extra_x, base_y + user_y + extra_y, base_z + user_z + extra_z);
            bool res = false;

            // wall hit test
            for (int wall = 0; wall < wall_count; ++wall)
            {
                if (Math.Abs(objects[wall].position.X - user_pos.X) <= (0.5 + objects[wall].len_x / 2) &&
                        Math.Abs(objects[wall].position.Y - user_pos.Y) <= (0.5 + objects[wall].len_y / 2))
                {
                    res = true;
                    break;
                }

            }

            // enemies hit test
            for (int enemy = wall_count; enemy < objects.Count; enemy++)
            {
                if (was[enemy - wall_count] == true)
                {
                    Vector3 block_pos = new Vector3(objects[enemy].position);
                    float dist = Math.Abs(block_pos.X - user_pos.X) + Math.Abs(block_pos.Y - user_pos.Y) +
                        Math.Abs(block_pos.Z - user_pos.Z);
                    if (dist <= 2f)
                    {
                        res = true;
                        was[enemy - wall_count] = false;
                        active--;
                    }
                }
            }
            if (res) reset();
            return res;
        }
        
        protected void update()
        {
            KeyboardState input = KeyboardState;
            
            if (input.IsKeyDown(Keys.J))
            {
                need_update = !need_update;
            }
            if (!need_update) return;

            if (input.IsKeyDown(Keys.Space))
            { 
                if ((space_type == 0 && v0 < 1e-8) || (space_type != 0))
                {
                    space_type++;
                }
            }
            if (space_type == 0 && v0 < 1e-8)
            {
                if (input.IsKeyDown(Keys.Up))
                {
                    if (stick_angle <= Math.PI / 2.5) stick_angle += 0.001f;
                    
                }
                if (input.IsKeyDown(Keys.Down))
                {
                    if (stick_angle > Math.PI / 12) stick_angle -= 0.001f;
                }
            }
            if (space_type == 1)
            {
                if (input.IsKeyDown(Keys.Up))
                {
                    v0 += 0.05f;
                }
                if (input.IsKeyDown(Keys.Down))
                {
                    v0 -= 0.05f;
                }

                if (input.IsKeyDown(Keys.D1))
                {
                    v0 = 1;
                }
                if (input.IsKeyDown(Keys.D2))
                {
                    v0 = 2;
                }
                if (input.IsKeyDown(Keys.D3))
                {
                    v0 = 3;
                }
                if (input.IsKeyDown(Keys.D4))
                {
                    v0 = 4;
                }
                if (input.IsKeyDown(Keys.D5))
                {
                    v0 = 5;
                }
                if (input.IsKeyDown(Keys.D6))
                {
                    v0 = 6;
                }
                if (input.IsKeyDown(Keys.D7))
                {
                    v0 = 7;
                }
                if (input.IsKeyDown(Keys.D8))
                {
                    v0 = 8;
                }
                if (input.IsKeyDown(Keys.D9))
                {
                    v0 = 9;
                }
                if (input.IsKeyDown(Keys.D))
                {
                    extra_x += 0.01f;
                }
                if (input.IsKeyDown(Keys.A))
                {
                    extra_x -= 0.01f;
                }
                if (input.IsKeyDown(Keys.W))
                {
                    extra_y += 0.01f;
                }
                if (input.IsKeyDown(Keys.S))
                {
                    extra_y -= 0.01f;
                }

            }
            if (space_type == 2)
            {
                space_type = 0;
            }
            if (input.IsKeyDown(Keys.Escape) || (((counter == 0) || (active == 0)) && (v0 < 1e-8)) || (counter <= 0 || active <= 0))
            {
                // -- stop --
                if (active <= 0 || counter <= 0)
                {
                    int flag = 1000;
                    while (flag > 0)
                    {
                        flag--;
                        GL.ClearColor(0f, 0f, 0f, 1f);
                        // Fill the screen with the color
                        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                        // transformation matrices
                        Matrix4 model = Matrix4.Identity;
                        Matrix4 view = camera.getViewMatrix();
                        Matrix4 projection = camera.GetProjectionMatrix();


                        int modelLocation = GL.GetUniformLocation(program.ID, "model");
                        int viewLocation = GL.GetUniformLocation(program.ID, "view");
                        int projectionLocation = GL.GetUniformLocation(program.ID, "projection");

                        GL.UniformMatrix4(modelLocation, true, ref model);
                        GL.UniformMatrix4(viewLocation, true, ref view);
                        GL.UniformMatrix4(projectionLocation, true, ref projection);
                        
                        if(active <= 0)
                        {
                            win.Render(program);
                        }
                        else
                        {
                            lose.Render(program);
                        }

                        Context.SwapBuffers();
                    }
                } 
                Close();
            }
        }
    }
}