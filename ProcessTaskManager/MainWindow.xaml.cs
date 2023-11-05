using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ProcessTaskManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public MainWindow()
        {
            CurrentProcessess = new();
            ProcessIgnoreList = new();
            InitializeComponent();
            DataContext = this;
            FIllProcesses();
            CountsEverything();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private ObservableCollection<MyProcess> currentProcessess;

        public ObservableCollection<MyProcess> CurrentProcessess
        {
            get { return currentProcessess; }
            set { currentProcessess = value; OnPropertyChanged(nameof(currentProcessess)); }
        }
        private MyProcess selectedProcess;

        public MyProcess SelectedProcess
        {
            get { return selectedProcess; }
            set { selectedProcess = value; OnPropertyChanged(nameof(SelectedProcess)); }
        }

        private ObservableCollection<string> processIgnoreList;

        public ObservableCollection<string> ProcessIgnoreList
        {
            get { return processIgnoreList; }
            set { processIgnoreList = value; OnPropertyChanged(nameof(processIgnoreList)); }
        }


        public void FIllProcesses()
        {
            ObservableCollection<MyProcess> TempCollection = new();
            var PList = Process.GetProcesses().ToList();
            foreach (var P in PList)
            {
                MyProcess TempProc = new(P.Id, P.ProcessName, P.Threads.Count, P.HandleCount);
                TempCollection.Add(TempProc);
            }
            currentProcessess = TempCollection;

        }

        public void CreateProcess(string processName)
        {
            try
            {
                Process.Start(processName);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public void KillProcess()
        {
            if (SelectedProcess == null) throw new Exception("Select the Process");
            Process.GetProcessById(SelectedProcess.Id).Kill();
        }

        public void AddProcessToIgnoreList()
        {
            if (SelectedProcess == null) throw new Exception("Select the Process");
            ProcessIgnoreList.Add(Process.GetProcessById(SelectedProcess.Id).ProcessName);
        }

        public void KillTheIgnoredProcessess()
        {
            if (ProcessIgnoreList.Count > 0 && ProcessIgnoreList != null)
            {
                foreach (var P in Process.GetProcesses().ToList())
                {
                    foreach (var IgnoredProcess in ProcessIgnoreList)
                    {
                        if (P.ProcessName == IgnoredProcess)
                        {
                            P.Kill();
                        }
                    }
                }
            }
        }

        public void CountsEverything()
        {
            Processess_Count_TB.Text = Process.GetProcesses().Count().ToString();
            Threads_Count_TB.Text = Process.GetProcesses().Sum(p => p.Threads.Count).ToString();
            Handles_Count_TB.Text = Process.GetProcesses().Sum(p => p.HandleCount).ToString();
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private DispatcherTimer timer;

        private void Timer_Tick(object sender, EventArgs e)
        {
            FIllProcesses();
            CountsEverything();
            KillTheIgnoredProcessess();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            timer.Stop();
        }

        private void Kill_Selected_Process_Button(object sender, RoutedEventArgs e)
        {
            try
            {

                KillProcess();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void Create_Process_Button_Event(object sender, RoutedEventArgs e)
        {
            CreateProcess(Create_Process_TB.Text);
        }

        private void Add_Proc_Into_Ignore_List(object sender, RoutedEventArgs e)
        {
            try
            {
                AddProcessToIgnoreList();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
