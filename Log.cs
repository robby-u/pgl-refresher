using System;
using System.IO;
using System.Threading.Tasks;

namespace PGLRefresher
{
    public class Log
    {
        public enum MessageType
        {
            INFO,
            ERROR,
        };

        private static DateTime Time;

        public static void Write(string Message, MessageType Type)
        {
            Time = DateTime.Now;

            switch (Type)
            {
                case MessageType.INFO:
                    Console.WriteLine("[{0}] [INFO] {1}", Time, Message);
                    _ = WriteToFile(Time, Message, Type);
                    break;
                case MessageType.ERROR:
                    Console.WriteLine("[{0}] [ERROR] {1}", Time, Message);
                    _ = WriteToFile(Time, Message, Type);
                    break;
            };
        }

        private static async Task WriteToFile(DateTime Time, string Message, MessageType Type)
        {
            string writePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string filePath = Path.Combine(writePath, "PGLRefreshLog.txt");

            using StreamWriter sw = new StreamWriter(filePath, append: true);

            switch (Type)
            {
                case MessageType.INFO:
                    await sw.WriteLineAsync("[" + Time + "] [INFO] " + Message);
                    break;
                case MessageType.ERROR:
                    await sw.WriteLineAsync("[" + Time + "] [ERROR] " + Message);
                    break;
            };
        }
    }
}
