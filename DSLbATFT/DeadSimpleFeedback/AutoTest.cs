using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DeadSimpleFeedback
{
    class AutoTest
    {
        private readonly string _projectRoot;
        private FileSystemWatcher _watcher;
        
        public AutoTest(string projectRoot)
        {
            _projectRoot = projectRoot;
        }

        public Action OnAllGreen { get; set; }
        public Action OnFailure { get; set; }
        public Action OnTestInProgress { get; set; }
        
        public void Start()
        {
            _watcher = new FileSystemWatcher(_projectRoot, "*.clj")
            {
                IncludeSubdirectories = true,
            };

            _watcher.Changed += (sender, e) => RunTests();
            _watcher.Created += (sender, e) => RunTests();
            _watcher.Deleted += (sender, e) => RunTests();
            _watcher.Renamed += (sender, e) => RunTests();

            RunTests();

            _watcher.EnableRaisingEvents = true;
        }


        private void RunTests()
        {
            Task.Factory.StartNew(() =>
            {
                OnTestInProgress();

                using (var leinTest = Process.Start(new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    WorkingDirectory = _projectRoot,
                    FileName = "c:\\Windows\\lein.bat",
                    Arguments = "test",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                }))
                {
                    ActOnTestResult(leinTest.StandardOutput.ReadToEnd());
                }
            });
        }

        private void ActOnTestResult(string output)
        {
            if (output.Contains("0 failures") && output.Contains("0 errors"))
                OnAllGreen();
            else
                OnFailure();
        }
    }
}
