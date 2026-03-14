using System;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace FileStreamPractice
{
    internal class Program
    {
        private enum Level { INFO, WARN, ERROR }

        public enum LogMessage
        {
            UserLogin, UserLogout, ConnectionTimeout, DiskSpaceLow,
            FileUploaded, FileDownloaded, DatabaseConnected, DatabaseError,
            ServiceStarted, ServiceStopped, RequestProcessed, InvalidRequest,
            CacheCleared, MemoryUsageHigh, NetworkLatencyHigh
        }

        private static readonly string LogsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        
        private static readonly ConcurrentDictionary<string, long> LastPositions = new ConcurrentDictionary<string, long>();

        // Статистика
        private static int _totalLogs = 0;
        private static int _infoCount = 0;
        private static int _warnCount = 0;
        private static int _errorCount = 0;
        private static string _lastEvent = "Ожидание данных...";

        private static readonly object ConsoleLock = new object();

        private static async Task Main()
        {
            if (!Directory.Exists(LogsPath))
                Directory.CreateDirectory(LogsPath);

            Console.CursorVisible = false;

            Task monitorTask = StartServerAsync();

            Task agent1 = GenerateLogsAsync("SRV-01", 500);
            Task agent2 = GenerateLogsAsync("SRV-02", 800);
            Task agent3 = GenerateLogsAsync("SRV-03", 1200);

            await Task.WhenAll(monitorTask, agent1, agent2, agent3);
        }

        private static async Task GenerateLogsAsync(string machineId, int delayMs)
        {
            var random = new Random();
            var filePath = Path.Combine(LogsPath, $"{machineId}.txt");

            while (true)
            {
                await Task.Delay(delayMs);

                var time = DateTime.UtcNow.ToString("O");
                var logLevelValues = Enum.GetValues(typeof(Level));
                var logLevel = ((Level)logLevelValues.GetValue(random.Next(logLevelValues.Length))).ToString();
                var messageValues = Enum.GetValues(typeof(LogMessage));
                var logMessage = ((LogMessage)messageValues.GetValue(random.Next(messageValues.Length))).ToString();

                var logLine = $"{time}|{machineId}|{logLevel}|{logMessage}";

                using (var stream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                using (var writer = new StreamWriter(stream))
                {
                    await writer.WriteLineAsync(logLine);
                }
            }
        }

        private static async Task StartServerAsync()
        {
            var watcher = new FileSystemWatcher
            {
                Path = LogsPath,
                Filter = "*.txt",
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size
            };

            watcher.Changed += async (s, e) => await ProcessLogFileAsync(e.FullPath);
            watcher.EnableRaisingEvents = true;

            UpdateUi();

            await Task.Delay(-1);
        }

        private static async Task ProcessLogFileAsync(string filePath)
        {
            try
            {
                var lastPos = LastPositions.GetOrAdd(filePath, 0);

                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    if (stream.Length < lastPos)
                        lastPos = 0;

                    stream.Seek(lastPos, SeekOrigin.Begin);

                    using (var reader = new StreamReader(stream, System.Text.Encoding.UTF8, false, 1024, leaveOpen: true))
                    {
                        string line;
                        while ((line = await reader.ReadLineAsync()) != null)
                        {
                            if (string.IsNullOrWhiteSpace(line)) continue;
                            
                            ProcessLine(line);
                        }

                        LastPositions[filePath] = stream.Position;
                    }

                    if (stream.Length >= 10 * 1024 * 1024)
                    {
                        await RotateAndCompressAsync(stream, filePath);
                    }
                }
            }
            catch (IOException)
            {
                
            }
        }

        private static void ProcessLine(string line)
        {
            var parts = line.Split('|');
            if (parts.Length < 4) return;

            var level = parts[2];

            Interlocked.Increment(ref _totalLogs);

            switch (level)
            {
                case "INFO": Interlocked.Increment(ref _infoCount); break;
                case "WARN": Interlocked.Increment(ref _warnCount); break;
                case "ERROR": Interlocked.Increment(ref _errorCount); break;
            }

            lock (ConsoleLock)
            {
                _lastEvent = line;
                UpdateUi();
            }
        }

        private static async Task RotateAndCompressAsync(FileStream sourceStream, string filePath)
        {
            var archiveName = Path.Combine(LogsPath, $"{Path.GetFileNameWithoutExtension(filePath)}_{DateTime.Now:yyyyMMdd_HHmmss}.txt.gz");

            sourceStream.Seek(0, SeekOrigin.Begin);
            using (var targetStream = File.Create(archiveName))
            using (var compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
            {
                await sourceStream.CopyToAsync(compressionStream);
            }

            sourceStream.SetLength(0);
            LastPositions[filePath] = 0;
        }

        private static void UpdateUi()
        {
            Console.SetCursorPosition(0, 0);
            
            Console.WriteLine(new string(' ', Console.WindowWidth));
            Console.WriteLine(new string(' ', Console.WindowWidth));
            Console.WriteLine(new string(' ', Console.WindowWidth));
            Console.WriteLine(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, 0);

            Console.WriteLine($"[Монитор логов] Логов: {_totalLogs} | INFO: {_infoCount} | WARN: {_warnCount} | ERROR: {_errorCount}");
            Console.WriteLine($"Последнее событие: {_lastEvent}");

            if (_lastEvent.Contains("|ERROR|"))
            {
                Console.WriteLine("\n!!! CRITICAL ERROR !!!");
                Console.ResetColor();
            }
        }
    }
}