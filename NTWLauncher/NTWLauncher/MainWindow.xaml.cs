using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace NTWLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CHN_Click(object sender, RoutedEventArgs e)
        {
            Process[] pname = Process.GetProcessesByName("Napoleon");
            if (pname.Length > 0)
            {
                MessageBox.Show("NTW is already running!");
            }
            else
            {
                if (!Directory.Exists(".\\clanlong"))
                {
                    MessageBox.Show("Vital File does not exist!!");
                    //Application.Current.Shutdown(0);
                    Environment.Exit(0);
                }
                if (ReplaceFile(".\\clanlong\\clanlong_1.bin", ".\\binkw32.dll")&& ReplaceFile(".\\clanlong\\clanlong_3.bin",".\\Loader.dll"))
                {
                    Process.Start(".\\Napoleon.exe");
                }
            }
        }

        private void ENG_Click(object sender, RoutedEventArgs e)
        {
            Process[] pname = Process.GetProcessesByName("Napoleon");
            if (pname.Length > 0)
            {
                MessageBox.Show("NTW is already running!");
            }
            else
            {
                if (!Directory.Exists(".\\clanlong"))
                {
                    MessageBox.Show("Vital File does not exist!!");
                    //Application.Current.Shutdown();
                    Environment.Exit(0);
                }
                if (!md5check(".\\binkw32.dll", "61d1d5d60ffc1c5e2d84a6e934a45072"))
                {
                    if (ReplaceFile(".\\clanlong\\clanlong_2.bin", ".\\binkw32.dll"))
                    {
                        Process.Start(".\\Napoleon.exe");
                    }
                }
                else
                {
                    Process.Start(".\\Napoleon.exe");
                }

            }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (Process pname in Process.GetProcessesByName("Napoleon"))
            {
                pname.Kill();
            }
            if (File.Exists(".\\binkw32.dll"))
            {

                if (!md5check(".\\binkw32.dll", "61d1d5d60ffc1c5e2d84a6e934a45072"))
                {
                    ReplaceFile(".\\clanlong\\clanlong_2.bin", ".\\binkw32.dll");
                }
            }

            if (File.Exists(".\\Loader.dll"))
            {
                File.Delete(".\\Loader.dll");
            }
            //Application.Current.Shutdown();
            Environment.Exit(0);
        }

        private bool ReplaceFile(string sourceFile, string destFile)
        {
            while (isFileLocked(destFile))
            {
                Thread.Sleep(1000);
            }
            try
            {
                File.Copy(sourceFile, destFile, true);
                return true;
            }
            catch (IOException x)
            {
                MessageBox.Show(x.Message.ToString());
                return false;
            }

        }

        private bool md5check(string filename, string md5value)
        {
            using (MD5 md5 = MD5.Create())
            {
                using (FileStream stream = File.OpenRead(filename))
                {
                    byte[] data = md5.ComputeHash(stream);
                    StringBuilder sBuilder = new StringBuilder();
                    for (int i = 0; i < data.Length; i++)
                    {
                        sBuilder.Append(data[i].ToString("x2"));
                    }
                    StringComparer comparer = StringComparer.OrdinalIgnoreCase;
                    if (0 == comparer.Compare(sBuilder.ToString(), md5value))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        private bool isFileLocked(string filename)
        {
            FileStream stream = null;
            if (!File.Exists(filename))
            {
                return false;
            }
            FileInfo file = new FileInfo(filename);
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return false;
        }

        private void CHECK_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("http://www.clanlong.com/Totalwar_Napoleon/");
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            foreach (Process pname in Process.GetProcessesByName("Napoleon"))
            {
                pname.Kill();
            }
            if (File.Exists(".\\binkw32.dll"))
            {

                if (!md5check(".\\binkw32.dll", "61d1d5d60ffc1c5e2d84a6e934a45072"))
                {
                    ReplaceFile(".\\clanlong\\clanlong_2.bin", ".\\binkw32.dll");
                }
            }

            if(File.Exists(".\\Loader.dll"))
            {
                File.Delete(".\\Loader.dll");
            }
            //Application.Current.Shutdown();
            Environment.Exit(0);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

    }
}
