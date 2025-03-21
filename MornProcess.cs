using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MornProcess
{
    public sealed class MornProcess : IDisposable
    {
        public readonly string WorkingDirectory;
        private readonly Process _process;

        private MornProcess(string workingDirectory, string fileName)
        {
            WorkingDirectory = workingDirectory;
            _process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = workingDirectory,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    StandardOutputEncoding = Encoding.UTF8,
                }
            };
        }

        private static string AssetsPath => Path.GetDirectoryName(Application.dataPath) ?? string.Empty;

        public static MornProcess CreateAtAssets(string fileName)
        {
            return new MornProcess(AssetsPath, fileName);
        }

        public static MornProcess CreateAtAssetsRelative(string relativePath, string fileName)
        {
            var path = Path.Combine(AssetsPath, relativePath);
            return new MornProcess(path, fileName);
        }

        public async UniTask<string> ExecuteAsync(string command, CancellationToken ct = default)
        {
            _process.StartInfo.Arguments = command;
            _process.Start();
            await UniTask.WaitUntil(() => _process.HasExited, cancellationToken: ct);
            var output = await _process.StandardOutput.ReadToEndAsync();
            var result = output.TrimEnd('\n');
            MornProcessGlobal.Log($"コマンド: {_process.StartInfo.FileName} {command}\n実行結果: {result}");
            return result;
        }

        public void Dispose()
        {
            _process?.Dispose();
        }
    }
}