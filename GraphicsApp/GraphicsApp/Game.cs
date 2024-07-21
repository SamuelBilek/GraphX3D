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

        private Camera _camera = new Camera(new Vector3(-4.0f, 4.0f, 4.0f), (float)800 / 600);

        private float[] _verticesMainObj = {
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

        private float[] _verticesGrid = {
            -100.0f, 0.0f, -100.0f,
             100.0f, 0.0f, -100.0f,
             100.0f, 0.0f,  100.0f,
            -100.0f, 0.0f,  100.0f,
        };

        private int _vboMainObj;
        private int _vaoMainObj;

        private int _vboLamp;
        private int _vaoLamp;

        private int _vboGrid;
        private int _vaoGrid;

        private Shader _shaderMainObj;
        private Shader _shaderLamp;
        private Shader _shaderGrid;

        private Texture _texture1;
        private Texture _texture2;

        private Vector3 _lightPos = new Vector3(1.2f, 1.0f, 2.0f);

        private Stopwatch _timer = Stopwatch.StartNew();

        private bool _canMove = false;

        public Game(int width, int height, string title)
            : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title })
        {
            this._width = width;
            this._height = height;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            _camera.LookAt(Vector3.Zero);

            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(0.15f, 0.15f, 0.15f, 1.0f);

            // Enable blending
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            // Main obj
            _shaderMainObj = new Shader("Shaders/main_obj_vert.glsl", "Shaders/main_obj_frag.glsl");

            _vaoMainObj = GL.GenVertexArray();
            GL.BindVertexArray(_vaoMainObj);

            _vboMainObj = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboMainObj);
            GL.BufferData(BufferTarget.ArrayBuffer, _verticesMainObj.Length * sizeof(float), _verticesMainObj, BufferUsageHint.StaticDraw);

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

            // Grid
            _shaderGrid = new Shader("Shaders/grid_vert.glsl", "Shaders/grid_frag.glsl");

            _vaoGrid = GL.GenVertexArray();
            GL.BindVertexArray(_vaoGrid);

            _vboGrid = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboGrid);
            GL.BufferData(BufferTarget.ArrayBuffer, _verticesGrid.Length * sizeof(float), _verticesGrid, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
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

            _shaderMainObj.SetVector3("light.position", _lightPos);
            _shaderMainObj.SetVector3("light.ambient", new Vector3(0.2f, 0.2f, 0.2f));
            _shaderMainObj.SetVector3("light.diffuse", new Vector3(0.5f, 0.5f, 0.5f)); // darken the light a bit to fit the scene
            _shaderMainObj.SetVector3("light.specular", new Vector3(1.0f, 1.0f, 1.0f));

            _shaderMainObj.SetFloat("light.constant", 1.0f);
            _shaderMainObj.SetFloat("light.linear", 0.09f);
            _shaderMainObj.SetFloat("light.quadratic", 0.032f);

            _shaderMainObj.SetVector3("material.ambient", new Vector3(1.0f, 0.5f, 0.31f));
            _shaderMainObj.SetVector3("material.diffuse", new Vector3(1.0f, 0.5f, 0.31f));
            _shaderMainObj.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            _shaderMainObj.SetFloat("material.shininess", 32.0f);

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

            // Grid
            GL.BindVertexArray(_vaoGrid);

            _shaderGrid.Use();
            _shaderGrid.SetMatrix4("view", view);
            _shaderGrid.SetMatrix4("projection", projection);

            _shaderGrid.SetVector3("cameraPos", _camera.Position);

            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);

            Context.SwapBuffers();

            base.OnRenderFrame(e);
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButton.Right)
            {
                CursorState = CursorState.Grabbed;
                _canMove = true;
                _firstMove = true;
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButton.Right)
            {
                CursorState = CursorState.Normal;
                _canMove = false;
            }
        }   

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);

            if (!IsFocused) // check to see if the window is focused
            {
                return;
            }

            if (!_canMove)
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

            if (!_canMove)
            {
                return;
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