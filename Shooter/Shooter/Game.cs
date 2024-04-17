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


namespace Shooter
{
    // Game class that inherets from the Game Window Class
    internal class Game : GameWindow
    {
        Block user;
        BackGround background;
        Wall wall;

        ShaderProgram program;
        // camera 
        Camera camera;
        // transform vars
        float yRot = 0f;

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

            user = new Block(new Vector3(0, 0, 0));
            wall = new Wall(new Vector3(-5, 0, 0), 10f, 1f, 1f);
            background = new BackGround(new Vector3(0, 0, 0));

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
            wall.Delete();
            // Delete, VAO, VBO, Shader Program
            
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

            model = Matrix4.CreateRotationY(yRot);
            //yRot += 0.005f;

            Matrix4 translation = Matrix4.CreateTranslation(0f, 0f, -3f);

            model *= translation;

            int modelLocation = GL.GetUniformLocation(program.ID, "model");
            int viewLocation = GL.GetUniformLocation(program.ID, "view");
            int projectionLocation = GL.GetUniformLocation(program.ID, "projection");

            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);

            background.Render(program);
            user.Render(program);
            wall.Render(program);

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

    }
}