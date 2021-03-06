﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Script;
using Xunit;

namespace WebJobs.Script.Tests
{
    public class FileTraceWriterTests
    {
        private string _logFilePath;

        public FileTraceWriterTests()
        {
            _logFilePath = Path.Combine(Path.GetTempPath(), "WebJobs.Script.Tests", "FileTraceWriterTests");

            if (Directory.Exists(_logFilePath))
            {
                Directory.Delete(_logFilePath, recursive: true);
            }
        }

        [Fact]
        public async Task MultipleConcurrentInstances_WritesAreSerialized()
        {
            int numLines = 100;

            // first ensure the file exists by writing a single entry
            // this ensures all the instances below will be operating on
            // the same file
            WriteLogs(_logFilePath, 1);

            // start 3 concurrent instances
            await Task.WhenAll(
                Task.Run(() => WriteLogs(_logFilePath, numLines)),
                Task.Run(() => WriteLogs(_logFilePath, numLines)),
                Task.Run(() => WriteLogs(_logFilePath, numLines)));

            string logFile = Directory.EnumerateFiles(_logFilePath).Single();
            string[] fileLines = File.ReadAllLines(logFile);
            Assert.Equal((3 * numLines) + 1, fileLines.Length);
        }

        private void WriteLogs(string logFilePath, int numLogs)
        {
            FileTraceWriter traceWriter = new FileTraceWriter(logFilePath, TraceLevel.Verbose);

            for (int i = 0; i < numLogs; i++)
            {
                traceWriter.Verbose(string.Format("Test message {0} {1}", Thread.CurrentThread.ManagedThreadId, i));
            }

            traceWriter.FlushToFile();
        }
    }
}
