using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shooter.Graphics;
using System.Drawing;

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

        public BackGround(Vector3 position_, string texture_name)
        {
            position = position_;
            List<Vector3> raw_vertices = new List<Vector3>()
            {
                new Vector3(-40f, 40f, -40f), // topleft vert
                new Vector3(40f, 40f, -40f), // topright vert
                new Vector3(40f, -40f, -40f), // bottomright vert
                new Vector3(-40f, -40f, -40f), // bottomleft vert
                // right face
                new Vector3(40f, 40f, -40f), // topleft vert
                new Vector3(40f, 40f, -70f), // topright vert
                new Vector3(40f, -40f, -70f), // bottomright vert
                new Vector3(40f, -40f, -40f), // bottomleft vert
                // back face
                new Vector3(40f, 40f, -70f), // topleft vert
                new Vector3(-40f, 40f, -70f), // topright vert
                new Vector3(-40f, -40f, -70f), // bottomright vert
                new Vector3(40f, -40f, -70f), // bottomleft vert
                // left face
                new Vector3(-40f, 40f, -70f), // topleft vert
                new Vector3(-40f, 40f, -40f), // topright vert
                new Vector3(-40f, -40f, -40f), // bottomright vert
                new Vector3(-40f, -40f, -70f), // bottomleft vert
                // top face
                new Vector3(-40f, 40f, -70f), // topleft vert
                new Vector3(40f, 40f, -70f), // topright vert
                new Vector3(40f, 40f, -40f), // bottomright vert
                new Vector3(-40f, 40f, -40f), // bottomleft vert
                // bottom face
                new Vector3(-40f, -40f, -40f), // topleft vert
                new Vector3(40f, -40f, -40f), // topright vert
                new Vector3(40f, -40f, -70f), // bottomright vert
                new Vector3(-40f, -40f, -70f), // bottomleft vert
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
            texture = new Texture(texture_name);
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
        /*
         * type = 0 --> user
         * type = 1 --> wall (user can`t touch it)
         * type = 2 --> enemies (blocks which we need to destroy)
         */
        public int type;

        VAO vao;
        VBO vert_vbo;
        VBO uv_vbo;
        IBO ibo;
        Texture texture;

        public float len_x, len_y, len_z;
        public Wall(Vector3 position_, float len_x_, float len_y_, float len_z_, int type_,  string texture_name)
        {
            len_x = len_x_;
            len_y = len_y_;
            len_z = len_z_;
            type = type_;
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
            texture = new Texture(texture_name);
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

    internal class Stick
    {
        public Vector3 position;
        public List<Vector3> vertices = new List<Vector3>();
        Vector3 bas = new Vector3();
        public List<Vector2> uv = new List<Vector2>();
        public List<uint> ids;
        /*
         * type = 0 --> user
         * type = 1 --> wall (user can`t touch it)
         * type = 2 --> enemies (blocks which we need to destroy)
         */
        public int type;
        double angle = Math.PI / 4;
        VAO vao;
        VBO vert_vbo;
        VBO uv_vbo;
        IBO ibo;
        Texture texture;
        float Thickness;
        double len;

        public float len_x, len_y, len_z;
        public Stick(Vector3 position_, Vector3 position__, float thickness, string texture_name)
        {
            
            uv = new List<Vector2>()
            {
                new Vector2(0f, 0f),
                new Vector2(1f, 1f),
            };
            ids = new List<uint>()
            {
                0, 1
            };
            Thickness = thickness;
            vertices.Add(position_);
            vertices.Add(position__);
            bas = new Vector3(vertices[1]);
            vao = new VAO();
            ibo = new IBO(ids);
            uv_vbo = new VBO(uv);
            texture = new Texture(texture_name);
            len = Math.Sqrt((position_.X - position__.X) * (position_.X - position__.X)
                * (position_.Y - position__.Y) * (position_.Y - position__.Y)
                * (position_.Z - position__.Z) * (position_.Z - position__.Z));
            Build();
        }
        public void Build()
        {

            vao.Bind();
            vert_vbo = new VBO(vertices);
            vert_vbo.Bind();

            vao.LinkToVAO(0, 3, vert_vbo);

            uv_vbo.Bind();
            vao.LinkToVAO(1, 2, uv_vbo);
        }
        public void Render(ShaderProgram program) // drawing the chunk
        {
            program.Bind();
            Build();
            vao.Bind();
            ibo.Bind();
            texture.Bind();  
            GL.DrawElements(PrimitiveType.Lines, ids.Count, DrawElementsType.UnsignedInt, 0);
  

        }
        public void Delete()
        {
            vao.Delete();
            vert_vbo.Delete();
            uv_vbo.Delete();
            ibo.Delete();
            texture.Delete();
        }

        public void move(double new_angle)
        {
            angle = new_angle;
            vertices[1] = new Vector3(bas.X, (float)Math.Tan(angle) * bas.Y, bas.Z);
        }
    }

    internal class RayTracing
    {


        public Vector3 position;
        public List<Vector3> vertices = new List<Vector3>();
        public List<uint> ids;
        VAO vao;
        VBO vert_vbo;
        IBO ibo;
        public RayTracing(Vector3 position_, string texture_name)
        {
            vertices = new List<Vector3>()
            {
                new Vector3(-1f, 1f, 0f) + position_,
                new Vector3( 1f, 1f, 0f) + position_,
                new Vector3( 1f, -1f, 0f) + position_,
                new Vector3(-1f, -1f, 0f) + position_
            };
            position = position_;
            ids = new List<uint>()
            {
                0, 1, 2,
                2, 3, 0
            };
            vao = new VAO();
            ibo = new IBO(ids);
            vert_vbo = new VBO(vertices);
            Build();
        }
        public void Build()
        {

            vao.Bind();
            vert_vbo.Bind();
            vao.LinkToVAO(0, 3, vert_vbo);   
        }
        public void Render(ShaderProgram program) // drawing the chunk
        {
            program.Bind();
            Build();
            vao.Bind();
            ibo.Bind();
            //texture.Bind();
            GL.DrawElements(PrimitiveType.Triangles, ids.Count, DrawElementsType.UnsignedInt, 0);
        }
        public void Delete()
        {
            vao.Delete();
            vert_vbo.Delete();
            ibo.Delete();
        }
    }
}
