using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Windows.Threading;

namespace DeadSimpleFeedback
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SetGreen()
        {
            Dispatcher.Invoke(DispatcherPriority.Normal,
                new MethodInvoker(() => base.Background = new SolidColorBrush(Colors.Green)));
        }

        private void SetRed()
        {
            Dispatcher.Invoke(DispatcherPriority.Normal,
                new MethodInvoker(() => base.Background = new SolidColorBrush(Colors.Red)));
        }

        private void SetUndecided()
        {
            Dispatcher.Invoke(DispatcherPriority.Normal,
                new MethodInvoker(() => base.Background = new SolidColorBrush(Colors.Yellow)));            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var folderSelector = new FolderBrowserDialog()
            {
                Description = "Select your leiningen project root folder",
            };

            if (folderSelector.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                new AutoTest(folderSelector.SelectedPath)
                {
                    OnAllGreen = SetGreen,
                    OnFailure = SetRed,
                    OnTestInProgress = SetUndecided,
                }
                .Start();
            }
        }
    }
}
