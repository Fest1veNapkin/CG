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
        RayTracing ray;
        //BackGround backGround;
        // -- objects --

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
            //backGround = new BackGround(new Vector3(0, 0, -10), "hgd.png");
            ray = new RayTracing(new Vector3(0f, 0f, 0), "hgd.png");
            program = new ShaderProgram("Default.vert", "Default.frag");

            GL.Enable(EnableCap.DepthTest);
            camera = new Camera(width, height, new Vector3(0f, 0f, -5f));
            CursorState = CursorState.Grabbed;
            
        }
        // called once when game is closed
        protected override void OnUnload()
        {
            base.OnUnload();
            ray.Delete();
            //backGround.Delete();
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

            program.Bind();
            //backGround.Render(program);
            ray.Render(program);
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