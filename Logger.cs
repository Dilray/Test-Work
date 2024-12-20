﻿using System;
using System.IO;

namespace Test_Work
{
    /// <summary>
    /// Класс для логирования приложения.
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Поле для файла, куда будут записаны логи.
        /// </summary>
        private string logFilePath;

        public Logger(string filePath)
        {
            logFilePath = filePath;
        }

        public void Log(string message)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now}: {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка записи в лог: {ex.Message}");
            }
        }
    }

}
