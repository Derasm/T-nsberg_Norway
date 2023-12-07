
namespace Shared.Common.Models
{
    /// <summary>
    /// We make an interface of DateTimerProvider to be able to mock it in unit tests and switch implementation if need be later.
    /// </summary>
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
    /// <summary>
    /// Implement IDateTimeProvider
    /// </summary>
    public class RealDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
    /// <summary>
    /// Mock of FileWriter to be able to test Logger without dependency on file system
    /// </summary>
    public interface IFileWriter
    {
        void WriteLine(string line);
    }
    /// <summary>
    /// Implementation to ensure we have a real file writer, while being loosely coupled to the implementation 
    /// </summary>
    public class RealFileWriter : IFileWriter
    {
        private readonly StreamWriter _writer;

        public RealFileWriter(string path)
        {
            _writer = new StreamWriter(File.Open(path, FileMode.Append))
            {
                AutoFlush = true
            };
        }

        public void WriteLine(string line)
        {
            _writer.WriteLine(line);
        }
    }
    public class Logger
    {
        private readonly IFileWriter _writer;
        private readonly IDateTimeProvider _dateTimeProvider;
        /// <summary>
        /// Changed constructor for dependency injection, removing dependency on file system
        /// </summary>
        /// <param name="writer"></param>
        public Logger(IFileWriter writer, IDateTimeProvider dateTimeProvider)
        {
            _writer = writer;
            _dateTimeProvider = dateTimeProvider;

            Log("Logger initialized");
        }
        /// <summary>
        /// Logger should prefix input string with date and time, avoiding external input / output
        /// </summary>
        /// <param name="str"></param>
        public void Log(string str)
        {
            string logEntry = string.Format("[{0:dd.MM.yy HH:mm:ss}] {1}", _dateTimeProvider.Now, str);
            _writer.WriteLine(logEntry);
        }
    }

}