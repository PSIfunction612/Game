using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;

namespace Game
{
    class Game : GameWindow
    {
        float[] verticles =
        {
            -0.5f, -0.5f, 0.0f,
            0.5f, -0.5f, 0.0f,
            0.0f, 0.5f, 0.0f
        };

        int VertexBufferObject;

        Shader shader;

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
            shader = new Shader("shader.vert", "shader.frag");
            GL.ClearColor(1.0f, 0.0f, 1.0f, 1.0f);
            VertexBufferObject = GL.GenBuffer(); //выделили память под точки
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
            //начали отрисовку
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, verticles.Length * sizeof(float), verticles, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3*sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            shader.Use();

            //рисуем!!!!
            //Здесь будет функция для трегольника


            GL.Clear(ClearBufferMask.ColorBufferBit); //закрашиваю фон окна цветом

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
