using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace Shooter.Graphics
{
    internal class VAO
    {

        public int ID;

        public VAO() {
            ID = GL.GenVertexArray();
            GL.BindVertexArray(ID);
        }
        
        public void LinkTaVao(int location, int size, VBO vbo)
        {
            Bind();
            vbo.Bind();
            GL.VertexAttribPointer(location, size, VertexAttribPointerType.Float, false,  0, 0);
            GL.EnableVertexAttribArray(location);
            UnBind();
        }

        public void Bind()
        {
            GL.BindVertexArray(ID);
        }

        public void UnBind()
        {
            GL.BindVertexArray(0);
        }

        public void Delete()
        {
            GL.DeleteVertexArray(ID);
        }

    }
}
