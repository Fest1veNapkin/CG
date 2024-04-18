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

namespace Shooter
{
    // Game class that inherets from the Game Window Class
    internal class Game : GameWindow
    {
        Block user;
        BackGround background;
        Wall floor, wall;
        List<Block> enemies = new List<Block>();
        List<bool> was = new List<bool>();
        ShaderProgram program;
        // camera 
        Camera camera;
        // transform vars
        float yRot = 0f;
        
        
        float base_x, base_y, base_z;
        float user_x, user_y, user_z;
        float v0 = 0, a = 9.81f, t = 0.001f, strong = 0, middle = 0;
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

            user = new Block(new Vector3(-12f, -3f, -22f), "linux.jpg");
            
            base_x = -12f; user_x = 0f;
            base_y = -3f;  user_y = 0f;
            base_z = -22f; user_z = 0f;

            floor = new Wall(new Vector3(0f, -4f, -22f), 50f, 1f, 30f, "red.PNG");
            background = new BackGround(new Vector3(0, 0, 0), "red.PNG");
            wall = new Wall(new Vector3(0f, -1f ,-22f), 4f, 6f, 10f, "hgd.PNG");

            for (float i = -5f; i < 6; i+=1f)
            {
                for (float j = -5; j < 5; j+=1)
                {
                    Block enemy = new Block(new Vector3(12f, -3f + j, -22f + i), "apple.jpg");
                    enemies.Add(enemy);
                    was.Add(true);
                }
            }


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
            
            if (strong > 1e-8)
            { 
                strong -= 0.01f;
                user_x += 0.03f;
                if (middle <= strong)
                {
                    user_y += 0.02f;
                }
                else user_y -= 0.02f;
            }
            else
            {
                reset();
            }
            hit_wall(wall);
            hit_block();

            update();
            model = Matrix4.Identity;
            Matrix4 translation = Matrix4.CreateTranslation(user_x, user_y, user_z);
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


        protected void update()
        {
            KeyboardState input = KeyboardState;
            if (strong < 1e-8)
            {
                if (input.IsKeyDown(Keys.D1))
                {
                    strong = 1;
                }
                if (input.IsKeyDown(Keys.D2))
                {
                    strong = 2;
                }
                if (input.IsKeyDown(Keys.D3))
                {
                    strong = 3;
                }
                if (input.IsKeyDown(Keys.D4))
                {
                    strong = 4;
                }
                if (input.IsKeyDown(Keys.D5))
                {
                    strong = 5;
                }
                if (input.IsKeyDown(Keys.D6))
                {
                    strong = 6;
                }
                if (input.IsKeyDown(Keys.D7))
                {
                    strong = 7;
                }
                if (input.IsKeyDown(Keys.D8))
                {
                    strong = 8;
                }
                if (input.IsKeyDown(Keys.D9))
                {
                    strong = 9;
                }
                middle = strong / 2;
            }
        }

        protected void reset()
        {
            user_x = 0;
            user_y = 0;
            user_z = 0;
            strong = 0f;
            middle = 0f;
        }

        protected void hit_wall(Wall wall)
        {
            Vector3 user_pos = new Vector3(base_x + user_x, base_y + user_y, base_z + user_z),
                wall_pos = wall.position;
            
            if (Math.Abs(wall_pos.X -  user_pos.X) <= (0.5 + wall.len_x/2) &&
                Math.Abs(wall_pos.Y - user_pos.Y) <= (0.5 + wall.len_y / 2) )
            {
                reset();
            }
        }

        protected void hit_block()
        {
            Vector3 user_pos = new Vector3(base_x + user_x, base_y + user_y, base_z + user_z);

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
                    }
                }
            }

        }
    }
}