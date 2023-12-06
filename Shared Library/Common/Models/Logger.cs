
namespace Shared.Common.Models
{
    public class Logger
    {
        private readonly StreamWriter _writer;
        /// <summary>
        /// private constructor to prevent instantiation
        /// </summary>
        /// <param name="path"></param>
        private Logger(string path)
        {
            // has dependency on Filesystem which is an issue. 
            _writer = new StreamWriter(File.Open(path, FileMode.Append))
            {
                AutoFlush = true
            };

            Log("Logger initialized");
        }
        /// <summary>
        /// Changed constructor for dependency inversion, removing dependency on file system
        /// </summary>
        /// <param name="writer"></param>
        public Logger(StreamWriter writer)
        {
            _writer = writer;
            Log("Logger initialized");
        }

        public void Log(string str)
        {
            _writer.WriteLine(string.Format("[{0:dd.MM.yy HH:mm:ss}] {1}", DateTime.Now, str));
        }
    }

}