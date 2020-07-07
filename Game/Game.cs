using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL4;

namespace Game
{
    class Game : GameWindow
    {
        /*float[] verticles =
        {
            -0.5f, -0.5f, 0.0f, 0.0f,  0.0f,
            0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
            0.0f, 0.5f, 0.0f,  0.5f, 1.0f,
        };*/

        float[] verticles =
        {
            0.5f, 0.5f, 0.0f, 1.0f,  1.0f,
            0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f,
            -0.5f, 0.5f, 0.0f, 0.0f, 1.0f,
        };

        uint[] indices =
        {
            0, 1, 3,
            1, 2, 3,
        };

        int ElementBufferObject; //EBO
        int VertexBufferObject; //VBO
        int VertexArrayObject; //VAO

        Shader shader;
        Texture texture;
        Texture texture1;

        public Game(int width, int height, string title)
            :base(width, height, GraphicsMode.Default, title){ }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState input = Keyboard.GetState();
            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }
            base.OnUpdateFrame(e);
        }

        // Здесь начали
        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.2f, 0.3f, 0.5f, 1.0f);

            //VBO
            VertexBufferObject = GL.GenBuffer(); //выделили память под точки
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, verticles.Length * sizeof(float), verticles, BufferUsageHint.StaticDraw);
            //EBO
            ElementBufferObject = GL.GenBuffer(); //генерация буфера
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer , indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            shader = new Shader("shader.vert", "shader.frag");
            shader.Use();

            texture = new Texture("container.png");
            texture.Use();

            texture1 = new Texture("awesomeface.png");
            texture1.Use(TextureUnit.Texture1);

            shader.SetInt("texture0", 0);
            shader.SetInt("texture1", 1);

            //VAO
            VertexArrayObject = GL.GenVertexArray(); //выделили память под VAO
            GL.BindVertexArray(VertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            //вершины
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            //для координат текстуры
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            base.OnLoad(e);
        }

        protected override void OnUnload(EventArgs e) //удаляем из GPU то, что в нее загрузим
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0); //Не вникай!!!!
            GL.DeleteBuffer(VertexBufferObject); //удаляю свой буфер!
            shader.Dispose();
            base.OnUnload(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit); //закрашиваю фон окна цветом
            //начали отрисовку
            GL.BindVertexArray(VertexArrayObject);

            texture.Use();
            texture1.Use(TextureUnit.Texture1);
            shader.Use();

            GL.DrawElements(BeginMode.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            Context.SwapBuffers(); //двойная буферизация, скину видео на эту тему
            base.OnRenderFrame(e);
        }

        protected override void OnResize(EventArgs e) //обработка события изменения размера окна
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }
    }
}
