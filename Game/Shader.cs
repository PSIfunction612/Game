﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game
{
    class Shader : IDisposable
    {
        int handle;

        private readonly Dictionary<string, int> _uniformLocations;

        //переменная-флаг для удаления
        bool disposedValue = false;

        public Shader(string vertexPath, string fragmentPath)
        {
            string VertexShaderSource;
            //Прочитали файл с программой вертексного шейдера в строку
            using (StreamReader reader = new StreamReader(vertexPath, Encoding.UTF8))
            {
                VertexShaderSource = reader.ReadToEnd();
            }

            string FragmentShaderSource;
            //Прочитали файл с программой фрагментного шейдера в строку
            using (StreamReader reader = new StreamReader(fragmentPath, Encoding.UTF8))
            {
                FragmentShaderSource = reader.ReadToEnd();
            }

            //выделили память под шейдеры
            int VertexShader = GL.CreateShader(ShaderType.VertexShader);
            //положили исходник прочитанного шейдера в выделенный буфер
            GL.ShaderSource(VertexShader, VertexShaderSource);

            //то же с фрагментным шейдером
            int FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);

            //компиляция вертексного шейдера
            GL.CompileShader(VertexShader);
            //Ловим ошибки и обрабатываем их
            string infoLogVert = GL.GetShaderInfoLog(VertexShader);
            if (infoLogVert != String.Empty)
            {
                Console.WriteLine(infoLogVert);
                Console.Beep();
            }

            //то же для фрагментного
            GL.CompileShader(FragmentShader);

            string infoLogFrag = GL.GetShaderInfoLog(FragmentShader);
            if (infoLogFrag != String.Empty)
            {
                Console.WriteLine(infoLogFrag);
                Console.Beep();
            }

            //собираем полноценную программу для видеокарты (GPU)
            handle = GL.CreateProgram();
            //Приклепляем в программе шейдеры для корректной работы
            GL.AttachShader(handle, VertexShader);
            GL.AttachShader(handle, FragmentShader);
            //Отправляем программу на видеокарту
            GL.LinkProgram(handle);

            //отцепили шейдеры от программы
            GL.DetachShader(handle, VertexShader);
            GL.DetachShader(handle, FragmentShader);
            //удалили уже ненужные шейдеры
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);

            GL.GetProgram(handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

            _uniformLocations = new Dictionary<string, int>();

            for (var i = 0; i < numberOfUniforms; ++i)
            {
                var key = GL.GetActiveUniform(handle, i, out _, out _);
                var location = GL.GetUniformLocation(handle, key);
                _uniformLocations.Add(key, location);
            }

        }


        public void SetInt(string name, int data)
        {
            GL.UseProgram(handle);
            GL.Uniform1(_uniformLocations[name], data);
        }


        public void SetMatrix4(string name, Matrix4 data)
        {
            GL.UseProgram(handle);
            GL.UniformMatrix4(_uniformLocations[name], true, ref data);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(handle);

                disposedValue = true;
            }
        }

        ~Shader()
        {
            //Деструктор
            GL.DeleteProgram(handle);
        }

        public void Dispose()
        {
            Dispose(true);
            //Garbage collector - сборщик мусора
            GC.SuppressFinalize(this);
        }

        public void Use()
        {
            GL.UseProgram(handle);
        }


        //public int GetAttribLocation(string attribName)
        //{
        //    return GL.GetAttribLocation(handle, attribName);
        //}

    }
}
