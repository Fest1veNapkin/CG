using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shooter.Graphics;

namespace Shooter.Objects
{

    internal class BackGround
    {
        public Vector3 position;
        public List<Vector3> vertices = new List<Vector3>();
        public List<Vector2> uv;
        public List<uint> ids;

        VAO vao;
        VBO vert_vbo;
        VBO uv_vbo;
        IBO ibo;
        Texture texture;

        public BackGround(Vector3 position_)
        {
            position = position_;
            List<Vector3> raw_vertices = new List<Vector3>()
            {
                new Vector3(-20f, 20f, -40f), // topleft vert
                new Vector3(20f, 20f, -40f), // topright vert
                new Vector3(20f, -20f, -40f), // bottomright vert
                new Vector3(-20f, -20f, -40f), // bottomleft vert
                // right face
                new Vector3(20f, 20f, -40f), // topleft vert
                new Vector3(20f, 20f, -70f), // topright vert
                new Vector3(20f, -20f, -70f), // bottomright vert
                new Vector3(20f, -20f, -40f), // bottomleft vert
                // back face
                new Vector3(20f, 20f, -70f), // topleft vert
                new Vector3(-20f, 20f, -70f), // topright vert
                new Vector3(-20f, -20f, -70f), // bottomright vert
                new Vector3(20f, -20f, -70f), // bottomleft vert
                // left face
                new Vector3(-20f, 20f, -70f), // topleft vert
                new Vector3(-20f, 20f, -40f), // topright vert
                new Vector3(-20f, -20f, -40f), // bottomright vert
                new Vector3(-20f, -20f, -70f), // bottomleft vert
                // top face
                new Vector3(-20f, 20f, -70f), // topleft vert
                new Vector3(20f, 20f, -70f), // topright vert
                new Vector3(20f, 20f, -40f), // bottomright vert
                new Vector3(-20f, 20f, -40f), // bottomleft vert
                // bottom face
                new Vector3(-20f, -20f, -40f), // topleft vert
                new Vector3(20f, -20f, -40f), // topright vert
                new Vector3(20f, -20f, -70f), // bottomright vert
                new Vector3(-20f, -20f, -70f), // bottomleft vert
            };
            uv = new List<Vector2>()
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
            ids = new List<uint>()
            {
                0, 1, 2,
                2, 3, 0,

                4, 5, 6,
                6, 7, 4,

                8, 9, 10,
                10, 11, 8,

                12, 13, 14,
                14, 15, 12,

                16, 17, 18,
                18, 19, 16,

                20, 21, 22,
                22, 23, 20
            };
            
            foreach (var vert in raw_vertices)
            {
                vertices.Add(vert + position);
            }

            vao = new VAO();
            vert_vbo = new VBO(vertices);
            ibo = new IBO(ids);
            uv_vbo = new VBO(uv);
            texture = new Texture("wall.PNG");
            Build();
        }

        public void Build()
        {

            vao.Bind();
            vert_vbo.Bind();

            vao.LinkToVAO(0, 3, vert_vbo);
            
            uv_vbo.Bind();
            vao.LinkToVAO(1, 2, uv_vbo);           
        }
        public void Render(ShaderProgram program) // drawing the chunk
        {
            program.Bind();
            vao.Bind();
            ibo.Bind();
            texture.Bind();
            GL.DrawElements(PrimitiveType.Triangles, ids.Count, DrawElementsType.UnsignedInt, 0);

        }
        public void Delete()
        {
            vao.Delete();
            vert_vbo.Delete();
            uv_vbo.Delete();
            ibo.Delete();
            texture.Delete();
        }

    }

    internal class Block
    {
        public Vector3 position;
        public List<Vector3> vertices = new List<Vector3>();
        public List<Vector2> uv = new List<Vector2>();
        public List<uint> ids;

        VAO vao;
        VBO vert_vbo;
        VBO uv_vbo;
        IBO ibo;
        Texture texture;


        public Block(Vector3 position_)
        {
            position = position_;

            List<Vector3> raw_vertices = new List<Vector3>()
            {
                new Vector3(-0.5f, 0.5f, 0.5f), // topleft vert
                new Vector3(0.5f, 0.5f, 0.5f), // topright vert
                new Vector3(0.5f, -0.5f, 0.5f), // bottomright vert
                new Vector3(-0.5f, -0.5f, 0.5f), // bottomleft vert
                // right face
                new Vector3(0.5f, 0.5f, 0.5f), // topleft vert
                new Vector3(0.5f, 0.5f, -0.5f), // topright vert
                new Vector3(0.5f, -0.5f, -0.5f), // bottomright vert
                new Vector3(0.5f, -0.5f, 0.5f), // bottomleft vert
                // back face
                new Vector3(0.5f, 0.5f, -0.5f), // topleft vert
                new Vector3(-0.5f, 0.5f, -0.5f), // topright vert
                new Vector3(-0.5f, -0.5f, -0.5f), // bottomright vert
                new Vector3(0.5f, -0.5f, -0.5f), // bottomleft vert
                // left face
                new Vector3(-0.5f, 0.5f, -0.5f), // topleft vert
                new Vector3(-0.5f, 0.5f, 0.5f), // topright vert
                new Vector3(-0.5f, -0.5f, 0.5f), // bottomright vert
                new Vector3(-0.5f, -0.5f, -0.5f), // bottomleft vert
                // top face
                new Vector3(-0.5f, 0.5f, -0.5f), // topleft vert
                new Vector3(0.5f, 0.5f, -0.5f), // topright vert
                new Vector3(0.5f, 0.5f, 0.5f), // bottomright vert
                new Vector3(-0.5f, 0.5f, 0.5f), // bottomleft vert
                // bottom face
                new Vector3(-0.5f, -0.5f, 0.5f), // topleft vert
                new Vector3(0.5f, -0.5f, 0.5f), // topright vert
                new Vector3(0.5f, -0.5f, -0.5f), // bottomright vert
                new Vector3(-0.5f, -0.5f, -0.5f), // bottomleft vert
            };
            uv = new List<Vector2>()
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
            ids = new List<uint>()
            {
                0, 1, 2,
                2, 3, 0,

                4, 5, 6,
                6, 7, 4,

                8, 9, 10,
                10, 11, 8,

                12, 13, 14,
                14, 15, 12,

                16, 17, 18,
                18, 19, 16,

                20, 21, 22,
                22, 23, 20
            };

            foreach (var vert in raw_vertices)
            {
                vertices.Add(vert + position);
            }

            vao = new VAO();
            vert_vbo = new VBO(vertices);
            ibo = new IBO(ids);
            uv_vbo = new VBO(uv);
            texture = new Texture("kasp.PNG");
            Build();
        }

        public void Build()
        {

            vao.Bind();
            vert_vbo.Bind();

            vao.LinkToVAO(0, 3, vert_vbo);

            uv_vbo.Bind();
            vao.LinkToVAO(1, 2, uv_vbo);
        }
        public void Render(ShaderProgram program) // drawing the chunk
        {
            program.Bind();
            vao.Bind();
            ibo.Bind();
            texture.Bind();
            GL.DrawElements(PrimitiveType.Triangles, ids.Count, DrawElementsType.UnsignedInt, 0);

        }
        public void Delete()
        {
            vao.Delete();
            vert_vbo.Delete();
            uv_vbo.Delete();
            ibo.Delete();
            texture.Delete();
        }
    }

    internal class Wall
    {
        public Vector3 position;
        public List<Vector3> vertices = new List<Vector3>();
        public List<Vector2> uv = new List<Vector2>();
        public List<uint> ids;

        VAO vao;
        VBO vert_vbo;
        VBO uv_vbo;
        IBO ibo;
        Texture texture;

        public Wall(Vector3 position_, float len_x, float len_y, float len_z)
        {
            position = position_;
            uv = new List<Vector2>()
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
            ids = new List<uint>()
            {
                0, 1, 2,
                2, 3, 0,

                4, 5, 6,
                6, 7, 4,

                8, 9, 10,
                10, 11, 8,

                12, 13, 14,
                14, 15, 12,

                16, 17, 18,
                18, 19, 16,

                20, 21, 22,
                22, 23, 20
            };
            Block block = new Objects.Block(new Vector3(0, 0, 0));

            foreach (var vert in block.vertices)
            {
                Vector3 dif = new Vector3(len_x/ (2f), len_y/ (2f), len_z/ (2f));
                if (vert.X < 0)
                {
                    dif.X = -(len_x/2);
                }
                if (vert.Y < 0)
                {
                    dif.Y = -(len_y / 2);
                }
                if (vert.Z < 0)
                {
                    dif.Z = -(len_z / 2);
                }

                vertices.Add(position + dif);
            }

            vao = new VAO();
            vert_vbo = new VBO(vertices);
            ibo = new IBO(ids);
            uv_vbo = new VBO(uv);
            texture = new Texture("kasp.PNG");
            Build();
        }
        public void Build()
        {

            vao.Bind();
            vert_vbo.Bind();

            vao.LinkToVAO(0, 3, vert_vbo);

            uv_vbo.Bind();
            vao.LinkToVAO(1, 2, uv_vbo);
        }
        public void Render(ShaderProgram program) // drawing the chunk
        {
            program.Bind();
            vao.Bind();
            ibo.Bind();
            texture.Bind();
            GL.DrawElements(PrimitiveType.Triangles, ids.Count, DrawElementsType.UnsignedInt, 0);

        }
        public void Delete()
        {
            vao.Delete();
            vert_vbo.Delete();
            uv_vbo.Delete();
            ibo.Delete();
            texture.Delete();
        }
    }
}
