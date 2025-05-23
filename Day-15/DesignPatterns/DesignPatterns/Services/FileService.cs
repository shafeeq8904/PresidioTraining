using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.Services
{
    public class FileService : IDisposable
    {
        private static FileService _instance;
        private StreamWriter _writer;
        private static readonly object _lock = new object();
        private bool _disposed = false;

        private FileService()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output.txt");
            _writer = new StreamWriter(filePath, false, Encoding.UTF8);
            _writer.AutoFlush = true;
        }

        public static FileService Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new FileService();
                    return _instance;
                }
            }
        }

        public void Write(string message)
        {
            _writer.WriteLine(message);
           
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _writer?.Close();
                _writer?.Dispose();
                _disposed = true;
            }
        }
    }
}
