using LetraU3D;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Drawing;
using System.Numerics;

namespace LetraU3D
{
    public class Window : GameWindow
    {
        private readonly float[] _vertices =
        {
            // Front face
            -0.6f,  0.6f,  0.1f,
            -0.6f, -0.6f,  0.1f,
            -0.4f, -0.6f,  0.1f,
            -0.4f,  0.6f,  0.1f,

            0.4f,  0.6f,  0.1f,
            0.4f, -0.6f,  0.1f,
            0.6f, -0.6f,  0.1f,
            0.6f,  0.6f,  0.1f,

            // Back face
            -0.6f,  0.6f, -0.1f,
            -0.6f, -0.6f, -0.1f,
            -0.4f, -0.6f, -0.1f,
            -0.4f,  0.6f, -0.1f,

            0.4f,  0.6f, -0.1f,
            0.4f, -0.6f, -0.1f,
            0.6f, -0.6f, -0.1f,
            0.6f,  0.6f, -0.1f,

            // Base de la U (con grosor)
            -0.4f, -0.398f,  0.1f,
            0.4f, -0.398f,  0.1f,
            0.4f, -0.398f, -0.1f,
            -0.4f, -0.398f, -0.1f,

            -0.4f, -0.6f,  0.1f,
            0.4f, -0.6f,  0.1f,
            0.4f, -0.6f, -0.1f,
            -0.4f, -0.6f, -0.1f
        };

        private readonly uint[] _indices =
        {
            // Front face
            0, 1, 2, 0, 2, 3,
            4, 5, 6, 4, 6, 7,

            // Back face
            8, 9, 10, 8, 10, 11,
            12, 13, 14, 12, 14, 15,

            // Sides
            0, 1, 9, 0, 9, 8,
            1, 2, 10, 1, 10, 9,
            2, 3, 11, 2, 11, 10,
            3, 0, 8, 3, 8, 11,
            4, 5, 13, 4, 13, 12,
            5, 6, 14, 5, 14, 13,
            6, 7, 15, 6, 15, 14,
            7, 4, 12, 7, 12, 15,

            // Bottom face (Base gruesa de la U)
            16, 17, 21, 16, 21, 20,
            18, 19, 23, 18, 23, 22,
            16, 17, 18, 16, 18, 19,
            20, 21, 22, 20, 22, 23
        };

        private int _vao, _vbo, _ebo, _shaderProgram;
        private Matrix4 _mvpMatrix;
        private float _rotationAngle;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings) { }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(Color4.DarkOliveGreen); // Fondo gris
            GL.Enable(EnableCap.DepthTest);

            InitializeBuffers();
            LoadShaders();
        }

        private void InitializeBuffers()
        {
            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);

            _vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(0);
        }

        private void LoadShaders()
        {
            _shaderProgram = Shader.LoadShader("shader.vert", "shader.frag");
            GL.UseProgram(_shaderProgram);

            int vertexLocation = GL.GetAttribLocation(_shaderProgram, "aPosition");
            if (vertexLocation == -1)
            {
                Console.WriteLine("ERROR: No se encontró el atributo 'aPosition' en el shader.");
            }

            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        }


        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Rotación en el eje Y
            _rotationAngle += 0.001f;
            Matrix4 model = Matrix4.CreateRotationY(_rotationAngle);
            Matrix4 view = Matrix4.LookAt(new OpenTK.Mathematics.Vector3(0, 0, 2), OpenTK.Mathematics.Vector3.Zero, OpenTK.Mathematics.Vector3.UnitY);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), 800f / 600f, 0.1f, 100f);
            _mvpMatrix = model * view * projection;

            // Enviar la matriz al shader
            int mvpLocation = GL.GetUniformLocation(_shaderProgram, "mvp");
            GL.UseProgram(_shaderProgram);
            GL.UniformMatrix4(mvpLocation, false, ref _mvpMatrix);

            GL.BindVertexArray(_vao);
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            GL.DeleteBuffer(_vbo);
            GL.DeleteBuffer(_ebo);
            GL.DeleteVertexArray(_vao);
            GL.DeleteProgram(_shaderProgram);
        }
    }
}

