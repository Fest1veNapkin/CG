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
        Block user;
        BackGround background;
        BackGround win, lose;
        Wall floor, wall;
        List<Block> enemies = new List<Block>();
        List<bool> was = new List<bool>();
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
            
            user = new Block(new Vector3(base_x, base_y, base_z), "linux.jpg");
            

            floor = new Wall(new Vector3(0f, -3f, -22f), 60f, 1f, 36f, "windows.PNG");
            background = new BackGround(new Vector3(0, 10, 0), "windows.PNG");
            lose = new BackGround(new Vector3(0, 0, -20), "defeat.jpg");
            win = new BackGround(new Vector3(0, 0, -20), "win.jpg");
            wall = new Wall(new Vector3(-3f, 0.5f ,-22f), 2f, 6f, 10f, "android.PNG");

            for (float i = -4f; i <= 4; i+=1f)
            {
                for (float j = -4; j <= 4; j+=1)
                {
                    float distance = (float)Math.Sqrt(i * i + j * j);
                    if (distance > 4) continue;
                    Block enemy = new Block(new Vector3(11f + i, 2f + j, -22f), "apple.jpg");
                    enemies.Add(enemy);
                    was.Add(true);
                }
            }
            for (float i = -2f; i <= 0; i += 1f)
            {
                for (float j = 0; j <= 2; j += 1)
                {
                    float distance = (float)Math.Sqrt(i * i + j * j);
                    if (distance > 2) continue;
                    Block enemy = new Block(new Vector3(4f + i, 1f + j, -22f), "compm.jpg");
                    enemies.Add(enemy);
                    was.Add(true);
                }
            }
            active = enemies.Count();
            program = new ShaderProgram("Default.vert", "Default.frag");

            GL.Enable(EnableCap.DepthTest);
            camera = new Camera(width, height, Vector3.Zero);
            CursorState = CursorState.Grabbed;
            
        }
        // called once when game is closed
        protected override void OnUnload()
        {
            base.OnUnload();
            background.Delete();
            user.Delete();
            floor.Delete();
            wall.Delete();
            foreach (var enemy in enemies)
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

            
            background.Render(program);
            floor.Render(program);
            wall.Render(program);
            for (int enemy = 0; enemy < enemies.Count; enemy++)
            {
                if (was[enemy])
                {
                    enemies[enemy].Render(program);
                }
            }
            if (user_x <= -20f || user_x <= -20f || user_x > 50 || user_y > 50)
            {
                reset();
            }
            if (!hit_wall(floor) && v0 != 0)
            { 
                dt += 0.01f;
                user_x = v0 * 0.5f * 2 * dt; 
                user_y = v0 * 0.9f * 2* dt - 9.81f * dt * dt / 2f;
            }
            hit_wall(wall);
            hit_block();
            update();

            model = Matrix4.Identity;
            Matrix4 translation = Matrix4.CreateTranslation(user_x + extra_x, user_y + extra_y, user_z);
            model *= translation;

            modelLocation = GL.GetUniformLocation(program.ID, "model");
            viewLocation = GL.GetUniformLocation(program.ID, "view");
            projectionLocation = GL.GetUniformLocation(program.ID, "projection");

            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);

            user.Render(program);

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

        protected bool hit_wall(Wall wall)
        {
            Vector3 user_pos = new Vector3(base_x + user_x + extra_x, base_y + user_y + extra_y, base_z + user_z + extra_z),
                wall_pos = wall.position;
            
            if (Math.Abs(wall_pos.X -  user_pos.X) <= (0.5 + wall.len_x/2) &&
                Math.Abs(wall_pos.Y - user_pos.Y) <= (0.5 + wall.len_y / 2) )
            {
                reset();
                return true;
            }
            return false;
        }

        protected void hit_block()
        {
            Vector3 user_pos = new Vector3(base_x + user_x + extra_x, base_y + user_y + extra_y, base_z + user_z + extra_z);

            for (int enemy = 0; enemy < enemies.Count; enemy++)
            {
                Block t = enemies[enemy];
                if (was[enemy])
                {
                    Vector3 block_pos = new Vector3(t.position);
                    float dist = Math.Abs(block_pos.X - user_pos.X) + Math.Abs(block_pos.Y - user_pos.Y) +
                        Math.Abs(block_pos.Z - user_pos.Z);
                    if (dist <= 2f)
                    {
                        reset();
                        was[enemy] = false;
                        active--;
                    }
                }
            }

        }

        protected void update()
        {
            KeyboardState input = KeyboardState;
            
            if (input.IsKeyDown(Keys.J))
            {
                need_update = !need_update;
            }
            if (!need_update) return;
            
            if (v0 < 1e-8)
            {
                if (input.IsKeyDown(Keys.D1))
                {
                    v0 = 1; counter--;
                }
                if (input.IsKeyDown(Keys.D2))
                {
                    v0 = 2; counter--;
                }
                if (input.IsKeyDown(Keys.D3))
                {
                    v0 = 3; counter--;
                }
                if (input.IsKeyDown(Keys.D4))
                {
                    v0 = 4; counter--;
                }
                if (input.IsKeyDown(Keys.D5))
                {
                    v0 = 5; counter--;
                }
                if (input.IsKeyDown(Keys.D6))
                {
                    v0 = 6; counter--;
                }
                if (input.IsKeyDown(Keys.D7))
                {
                    v0 = 7; counter--;
                }
                if (input.IsKeyDown(Keys.D8))
                {
                    v0 = 8; counter--;
                }
                if (input.IsKeyDown(Keys.D9))
                {
                    v0 = 9; counter--;
                }
                if (input.IsKeyDown(Keys.D))
                {
                    //do something if Key is pressed
                    extra_x += 0.01f;
                }
                if (input.IsKeyDown(Keys.A))
                {
                    //do something if Key is pressed
                    extra_x -= 0.01f;
                }
                if (input.IsKeyDown(Keys.W))
                {
                    //do something if Key is pressed
                    extra_y += 0.01f;
                }
                if (input.IsKeyDown(Keys.S))
                {
                    //do something if Key is pressed
                    extra_y -= 0.01f;
                }
            }

            if (input.IsKeyDown(Keys.Escape) || (((counter == 0) || (active == 0)) && (v0 < 1e-8)))
            {
                // -- stop --
                if (active == 0 || counter == 0)
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
                        
                        if(active == 0)
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