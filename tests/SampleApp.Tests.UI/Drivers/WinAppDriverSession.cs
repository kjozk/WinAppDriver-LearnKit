using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace SampleApp.Tests.UI.Drivers
{
    public static class WinAppDriverSession
    {
        private static WindowsDriver<WindowsElement> _session;
        private static Process _winAppDriverProcess;

        public static WindowsDriver<WindowsElement> Start(string appPath, string winAppDriverPath = null)
        {
            if (_session != null)
            {
                return _session;
            }

            // ------------------------
            // 1. AppPath の絶対パス変換
            // ------------------------
#if DEBUG
            // デバッグ時はテストプロジェクトからの相対パスを想定
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            appPath = Path.GetFullPath(Path.Combine(baseDir, appPath));
#endif
            if (!File.Exists(appPath))
            {
                throw new FileNotFoundException($"指定されたアプリケーションが見つかりません: {appPath}");
            }

            // ------------------------
            // 3. WinAppDriver が起動しているか確認
            // ------------------------
            StartWinAppDriverIfNotRunning(winAppDriverPath);

            // ------------------------
            // 4. WindowsDriver セッション開始
            // ------------------------
            return InitializeWindowsDriverSession(appPath);
        }

        private static WindowsDriver<WindowsElement> InitializeWindowsDriverSession(string appPath)
        {
            var options = new AppiumOptions();
            options.AddAdditionalCapability("app", appPath);
            options.AddAdditionalCapability("deviceName", "WindowsPC");

            _session = new WindowsDriver<WindowsElement>(
                new Uri("http://127.0.0.1:4723"),
                options
            );

            return _session;
        }

        private static void StartWinAppDriverIfNotRunning(string winAppDriverPath)
        {
            bool isRunning = Process.GetProcessesByName("WinAppDriver").Any();
            if (!isRunning)
            {
                bool devModeError = false;
                bool started = false;
                _winAppDriverProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = winAppDriverPath,
                        Arguments = "127.0.0.1 4723",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        StandardOutputEncoding = System.Text.Encoding.Unicode,
                        StandardErrorEncoding = System.Text.Encoding.Unicode,
                        CreateNoWindow = true
                    }
                };

                _winAppDriverProcess.OutputDataReceived += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        Console.WriteLine($"[WinAppDriver] {e.Data}");

                        if (e.Data.Contains("Developer mode is not enabled"))
                            devModeError = true;

                        if (e.Data.Contains("127.0.0.1:4723"))
                            started = true;
                    }
                };
                _winAppDriverProcess.ErrorDataReceived += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                        Console.Error.WriteLine($"[WinAppDriver ERROR] {e.Data}");
                };

                _winAppDriverProcess.Exited += (s, e) =>
                {
                    Console.Error.WriteLine($"WinAppDriver プロセスが終了しました。ExitCode={_winAppDriverProcess.ExitCode}");
                };


                _winAppDriverProcess.Start();
                _winAppDriverProcess.BeginOutputReadLine();
                _winAppDriverProcess.BeginErrorReadLine();

                // 起動待ちループ
                int waitMs = 0;
                while (!started && waitMs < 5000)
                {
                    if (devModeError)
                    {
                        Assert.Inconclusive("開発者モードが無効です。設定アプリ（システム > 詳細設定 > 開発者向け）で有効にしてください。");
                    }

                    Thread.Sleep(200);
                    waitMs += 200;
                }

                if (!started)
                {
                    _winAppDriverProcess.Kill();
                    throw new Exception("WinAppDriver の起動に失敗しました。ログを確認してください。");
                }
            }
        }

        public static void Quit()
        {
            _session?.Quit();
            _session = null;

            if (_winAppDriverProcess != null && !_winAppDriverProcess.HasExited)
            {
                _winAppDriverProcess.Kill();
                _winAppDriverProcess.Dispose();
                _winAppDriverProcess = null;
            }
        }
    }
}
