using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace LetraU3D
{
    public static class Shader
    {
        public static int LoadShader(string vertexPath, string fragmentPath)
        {
            if (!File.Exists(vertexPath) || !File.Exists(fragmentPath))
            {
                Console.WriteLine($"ERROR: No se encontraron los archivos {vertexPath} o {fragmentPath}");
                return -1;
            }

            string vertexSource = File.ReadAllText(vertexPath);
            string fragmentSource = File.ReadAllText(fragmentPath);

            if (string.IsNullOrWhiteSpace(vertexSource) || string.IsNullOrWhiteSpace(fragmentSource))
            {
                Console.WriteLine("ERROR: Uno o ambos archivos de shader están vacíos.");
                return -1;
            }

            Console.WriteLine("Vertex Shader:\n" + vertexSource);
            Console.WriteLine("Fragment Shader:\n" + fragmentSource);

            int vertexShader = CompileShader(ShaderType.VertexShader, vertexSource);
            int fragmentShader = CompileShader(ShaderType.FragmentShader, fragmentSource);

            return LinkProgram(vertexShader, fragmentShader);
        }


        private static int CompileShader(ShaderType type, string source)
        {
            int shader = GL.CreateShader(type);
            GL.ShaderSource(shader, source);
            GL.CompileShader(shader);
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string errorLog = GL.GetShaderInfoLog(shader);
                Console.WriteLine($"ERROR::SHADER_COMPILATION_ERROR of type: {type}\n{errorLog}");
            }
            return shader;
        }

        private static int LinkProgram(int vertexShader, int fragmentShader)
        {
            int program = GL.CreateProgram();
            GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);
            GL.LinkProgram(program);

            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string errorLog = GL.GetProgramInfoLog(program);
                Console.WriteLine($"ERROR::PROGRAM_LINKING_ERROR\n{errorLog}");
            }

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
            return program;
        }
    }
}

