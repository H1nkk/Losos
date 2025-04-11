using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ConsoleApp1
{
    internal class Shader
    {
        public int shaderHandle;


        public Shader()
        {
            LoadShader();
        }
        public void LoadShader()
        {
            shaderHandle = GL.CreateProgram();
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, LoadShaderSource("shader.vert"));
            GL.CompileShader(vertexShader);
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, LoadShaderSource("shader.frag"));
            GL.CompileShader(fragmentShader);
            GL.AttachShader(shaderHandle, vertexShader);
            GL.AttachShader(shaderHandle, fragmentShader);
            GL.LinkProgram(shaderHandle);

            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int success1);
            if (success1 == 0)
            {
                string infoLog = GL.GetShaderInfoLog(vertexShader);
                Console.WriteLine(infoLog);
            }
            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out int success2);
            if (success2 == 0)
            {
                string infoLog = GL.GetShaderInfoLog(fragmentShader);
                Console.WriteLine(infoLog);
            }
            GL.GetProgram(shaderHandle, GetProgramParameterName.LinkStatus, out int success3);
            if (success3 == 0)
            {
                string infoLog = GL.GetProgramInfoLog(shaderHandle);
                Console.WriteLine("Shader program linking error:\n" + infoLog);
            }
        }

        public void SetMatrix4(string name, Matrix4 matrix)
        {
            int location = GL.GetUniformLocation(shaderHandle, name);
            GL.UniformMatrix4(location, true, ref matrix);
        }

        public void SetVector3(string name, Vector3 vector)
        {
            int location = GL.GetUniformLocation(shaderHandle, name);
            GL.Uniform3(location, vector);
        }

        public static string LoadShaderSource(string filepath)
        {
            string shaderSource = "";
            try
            {
                using (StreamReader reader = new StreamReader("../../../Shaders/" + filepath))
                {
                    shaderSource = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to load shader source file: " + e.Message);
            }

            return shaderSource;
        }

        public void UseShader()
        {
            GL.UseProgram(shaderHandle);
        }
        public void DeleteShader()
        {
            GL.DeleteProgram(shaderHandle);
        }

    }
}
