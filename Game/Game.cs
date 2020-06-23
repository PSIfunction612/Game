using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;

namespace Game
{
    class Game : GameWindow
    {
        int VertexBufferObject;

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
            GL.ClearColor(1.0f, 0.0f, 1.0f, 1.0f);
            VertexBufferObject = GL.GenBuffer(); //выделили память под точки
            base.OnLoad(e);
        }

        protected override void OnUnload(EventArgs e) //удаляем из GPU то, что в нее загрузим
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0); //Не вникай!!!!
            GL.DeleteBuffer(VertexBufferObject); //удаляю свой буфер!
            base.OnUnload(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit); //закрашиваю фон окна цветом

            Context.SwapBuffers(); //двойная буферизация, скину видео на эту тему
            base.OnRenderFrame(e);
        }

        protected override void OnResize(EventArgs e) //пока не вникай!
        {
            GL.Viewport(0, 0, 600, 600);
            base.OnResize(e);
        }
    }
}
