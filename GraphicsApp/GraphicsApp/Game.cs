using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

namespace GraphicsApp
{
    public class Game : GameWindow
    {
        private int _width = 800;
        private int _height = 600;

        private bool _firstMove = true;

        private Vector2 _lastPos = new Vector2();

        private float _speed = 1.5f;

        private Camera _camera = new Camera(Vector3.UnitZ * 3, (float)800 / 600);

        private float[] _vertices = {
            //Position             //Texture coordinates     // Normals
            // Front face
            // Triangle 1
             0.5f,  0.5f,  0.5f,   1.0f, 1.0f,               0.0f,  0.0f,  1.0f,  // Top-right
             0.5f, -0.5f,  0.5f,   1.0f, 0.0f,               0.0f,  0.0f,  1.0f,  // Bottom-right
            -0.5f, -0.5f,  0.5f,   0.0f, 0.0f,               0.0f,  0.0f,  1.0f,  // Bottom-left
            // Triangle 2                                    
            -0.5f, -0.5f,  0.5f,   0.0f, 0.0f,               0.0f,  0.0f,  1.0f,  // Bottom-left
            -0.5f,  0.5f,  0.5f,   0.0f, 1.0f,               0.0f,  0.0f,  1.0f,  // Top-left
             0.5f,  0.5f,  0.5f,   1.0f, 1.0f,               0.0f,  0.0f,  1.0f,  // Top-right
                                                             
            // Back face                                     
            // Triangle 1                                    
             0.5f,  0.5f, -0.5f,   1.0f, 1.0f,               0.0f,  0.0f, -1.0f,  // Top-right
             0.5f, -0.5f, -0.5f,   1.0f, 0.0f,               0.0f,  0.0f, -1.0f,  // Bottom-right
            -0.5f, -0.5f, -0.5f,   0.0f, 0.0f,               0.0f,  0.0f, -1.0f,  // Bottom-left
            // Triangle 2                                    
            -0.5f, -0.5f, -0.5f,   0.0f, 0.0f,               0.0f,  0.0f, -1.0f,  // Bottom-left
            -0.5f,  0.5f, -0.5f,   0.0f, 1.0f,               0.0f,  0.0f, -1.0f,  // Top-left
             0.5f,  0.5f, -0.5f,   1.0f, 1.0f,               0.0f,  0.0f, -1.0f,  // Top-right
                                                             
            // Top face                                      
            // Triangle 1                                    
             0.5f,  0.5f,  0.5f,   1.0f, 1.0f,               0.0f,  1.0f,  0.0f,  // Top-right
            -0.5f,  0.5f,  0.5f,   0.0f, 1.0f,               0.0f,  1.0f,  0.0f,  // Top-left
            -0.5f,  0.5f, -0.5f,   0.0f, 0.0f,               0.0f,  1.0f,  0.0f,  // Bottom-left
            // Triangle 2                                    
            -0.5f,  0.5f, -0.5f,   0.0f, 0.0f,               0.0f,  1.0f,  0.0f,  // Bottom-left
             0.5f,  0.5f, -0.5f,   1.0f, 0.0f,               0.0f,  1.0f,  0.0f,  // Bottom-right
             0.5f,  0.5f,  0.5f,   1.0f, 1.0f,               0.0f,  1.0f,  0.0f,  // Top-right
                                                             
            // Bottom face                                   
            // Triangle 1                                    
             0.5f, -0.5f,  0.5f,   1.0f, 1.0f,               0.0f, -1.0f,  0.0f,  // Top-right
            -0.5f, -0.5f,  0.5f,   0.0f, 1.0f,               0.0f, -1.0f,  0.0f,  // Top-left
            -0.5f, -0.5f, -0.5f,   0.0f, 0.0f,               0.0f, -1.0f,  0.0f,  // Bottom-left
            // Triangle 2                                    
            -0.5f, -0.5f, -0.5f,   0.0f, 0.0f,               0.0f, -1.0f,  0.0f,  // Bottom-left
             0.5f, -0.5f, -0.5f,   1.0f, 0.0f,               0.0f, -1.0f,  0.0f,  // Bottom-right
             0.5f, -0.5f,  0.5f,   1.0f, 1.0f,               0.0f, -1.0f,  0.0f,  // Top-right
                                                             
            // Left face                                     
            // Triangle 1                                    
            -0.5f,  0.5f,  0.5f,   1.0f, 1.0f,              -1.0f,  0.0f,  0.0f,  // Top-right
            -0.5f, -0.5f,  0.5f,   1.0f, 0.0f,              -1.0f,  0.0f,  0.0f,  // Bottom-right
            -0.5f, -0.5f, -0.5f,   0.0f, 0.0f,              -1.0f,  0.0f,  0.0f,  // Bottom-left
            // Triangle 2                                    
            -0.5f, -0.5f, -0.5f,   0.0f, 0.0f,              -1.0f,  0.0f,  0.0f,  // Bottom-left
            -0.5f,  0.5f, -0.5f,   0.0f, 1.0f,              -1.0f,  0.0f,  0.0f,  // Top-left
            -0.5f,  0.5f,  0.5f,   1.0f, 1.0f,              -1.0f,  0.0f,  0.0f,  // Top-right
                                                             
            // Right face                                    
            // Triangle 1                                    
             0.5f,  0.5f,  0.5f,   1.0f, 1.0f,               1.0f,  0.0f,  0.0f,  // Top-right
             0.5f, -0.5f,  0.5f,   1.0f, 0.0f,               1.0f,  0.0f,  0.0f,  // Bottom-right
             0.5f, -0.5f, -0.5f,   0.0f, 0.0f,               1.0f,  0.0f,  0.0f,  // Bottom-left
            // Triangle 2                                    
             0.5f, -0.5f, -0.5f,   0.0f, 0.0f,               1.0f,  0.0f,  0.0f,  // Bottom-left
             0.5f,  0.5f, -0.5f,   0.0f, 1.0f,               1.0f,  0.0f,  0.0f,  // Top-left
             0.5f,  0.5f,  0.5f,   1.0f, 1.0f,               1.0f,  0.0f,  0.0f   // Top-right
        };

        private int _vboMainObj;
        private int _vaoMainObj;

        private int _vboLamp;
        private int _vaoLamp;

        private Shader _shaderMainObj;
        private Shader _shaderLamp;

        private Texture _texture1;
        private Texture _texture2;

        private Vector3 _lightPos = new Vector3(1.2f, 1.0f, 2.0f);

        private Stopwatch _timer = Stopwatch.StartNew();

        public Game(int width, int height, string title)
            : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title })
        {
            this._width = width;
            this._height = height;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            CursorState = CursorState.Grabbed;

            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(0.3f, 0.3f, 0.3f, 1.0f);

            // Main obj
            _shaderMainObj = new Shader("Shaders/main_obj_vert.glsl", "Shaders/main_obj_frag.glsl");

            _vaoMainObj = GL.GenVertexArray();
            GL.BindVertexArray(_vaoMainObj);

            _vboMainObj = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboMainObj);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            int texCoordLocation = GL.GetAttribLocation(_shaderMainObj.Handle, "aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));

            _texture1 = new Texture("container.jpg", TextureUnit.Texture0);
            _texture2 = new Texture("awesomeface.png", TextureUnit.Texture1);

            // Light proxy
            _shaderLamp = new Shader("Shaders/light_proxy_vert.glsl", "Shaders/light_proxy_frag.glsl");

            //Initialize the vao for the lamp, this is mostly the same as the code for the model cube
            _vaoLamp = GL.GenVertexArray();
            GL.BindVertexArray(_vaoLamp);
            //We only need to bind to the VBO, the container's VBO's data already contains the correct data.
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboMainObj);
            //Set the vertex attributes (only position data for our lamp)
            int vertexLocation = GL.GetAttribLocation(_shaderLamp.Handle, "aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            _shaderMainObj.Dispose();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 view = _camera.GetViewMatrix();
            Matrix4 projection = _camera.GetProjectionMatrix();

            // Main obj
            GL.BindVertexArray(_vaoMainObj);

            _texture1.Use(TextureUnit.Texture0);
            _texture2.Use(TextureUnit.Texture1);

            Matrix4 modelMainObj = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(0));
           
            _shaderMainObj.Use();

            //_shaderMainObj.SetInt("texture1", 0);
            //_shaderMainObj.SetInt("texture2", 1);

            _shaderMainObj.SetVector3("viewPos", _camera.Position);

            _shaderMainObj.SetVector3("lightPos", _lightPos);
            _shaderMainObj.SetVector3("objectColor", new Vector3(1.0f, 0.5f, 0.31f));
            _shaderMainObj.SetVector3("lightColor", new Vector3(1.0f, 1.0f, 1.0f));

            _shaderMainObj.SetMatrix4("model", modelMainObj);
            _shaderMainObj.SetMatrix4("view", view);
            _shaderMainObj.SetMatrix4("projection", projection);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            // Light proxy
            GL.BindVertexArray(_vaoLamp);

            _shaderLamp.Use();
            
            Matrix4 modelLamp = Matrix4.CreateScale(0.2f) * Matrix4.CreateTranslation(_lightPos);

            _shaderLamp.SetMatrix4("model", modelLamp);
            _shaderLamp.SetMatrix4("view", _camera.GetViewMatrix());
            _shaderLamp.SetMatrix4("projection", _camera.GetProjectionMatrix());

            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            Context.SwapBuffers();

            base.OnRenderFrame(e);
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);

            if (!IsFocused) // check to see if the window is focused
            {
                return;
            }

            if (_firstMove)
            {
                _lastPos = new Vector2(e.X, e.Y);
                _firstMove = false;
            }
            else
            {
                float deltaX = e.X - _lastPos.X;
                float deltaY = e.Y - _lastPos.Y;
                _lastPos = new Vector2(e.X, e.Y);
                _camera.Yaw += deltaX * 0.1f;
                _camera.Pitch -= deltaY * 0.1f; // reversed since y-coordinates range from bottom to top
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            
            if (!IsFocused) // check to see if the window is focused
            {
                return;
            }

            KeyboardState input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            if (input.IsKeyDown(Keys.W))
            {
                _camera.Position += _camera.Front * _speed * (float)e.Time; //Forward 
            }

            if (input.IsKeyDown(Keys.S))
            {
                _camera.Position -= _camera.Front * _speed * (float)e.Time; //Backwards
            }

            if (input.IsKeyDown(Keys.A))
            {
                _camera.Position -= Vector3.Normalize(Vector3.Cross(_camera.Front, _camera.Up)) * _speed * (float)e.Time; //Left
            }

            if (input.IsKeyDown(Keys.D))
            {
                _camera.Position += Vector3.Normalize(Vector3.Cross(_camera.Front, _camera.Up)) * _speed * (float)e.Time; //Right
            }

            if (input.IsKeyDown(Keys.Space))
            {
                _camera.Position += _camera.Up * _speed * (float)e.Time; //Up 
            }

            if (input.IsKeyDown(Keys.LeftShift))
            {
                _camera.Position -= _camera.Up * _speed * (float)e.Time; //Down
            }
        }
    }
}