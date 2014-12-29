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
using System.Threading;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;


namespace R2TWLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process[] pname = Process.GetProcessesByName("Rome2");
            if (pname.Length > 0)
            {
                System.Windows.MessageBox.Show("Rome2TW is already running!");
            }
            else
            {
                if (!Directory.Exists(".\\clanlong"))
                {
                    System.Windows.MessageBox.Show("汉化文件缺失!!");
                    Environment.Exit(0);
                }
            checkmd5:
                if (!md5check(".\\binkw32.dll", "04B064676A2D466887581EA3FBE327EF"))
                {
                    if (File.Exists(".\\binkw32_ori.dll"))
                    {
                        while (isFileLocked(".\\binkw32_ori.dll"))
                        {
                            Thread.Sleep(1000);
                        }
                        ReplaceFile(".\\binkw32_ori.dll", ".\\binkw32.dll");
                        File.Delete(".\\binkw32_ori.dll");
                        goto checkmd5;
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("缺少原生binkw32.dll！");
                        Environment.Exit(0);
                    }
                }
                else
                {
                    Process.Start(".\\Rome2.exe");
                    this.WindowState = WindowState.Minimized;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Process[] pname = Process.GetProcessesByName("Rome2");
            if (pname.Length > 0)
            {
                System.Windows.MessageBox.Show("Rome2TW is already running！");
            }
            else
            {
                if (!Directory.Exists(".\\clanlong"))
                {
                    System.Windows.MessageBox.Show("汉化文件缺失!!");
                    Environment.Exit(0);
                }
                try{
                    if (!File.Exists(".\\binkw32_ori.dll")){
                        while (isFileLocked(".\\binkw32.dll"))
                        {
                            Thread.Sleep(1000);
                        }
                        ReplaceFile(".\\binkw32.dll", ".\\binkw32_ori.dll");
                    }
                    if (!md5check(".\\binkw32_ori.dll", "04B064676A2D466887581EA3FBE327EF"))
                    {
                        System.Windows.MessageBox.Show("缺少原生binkw32.dll！");
                        Environment.Exit(0);
                    }
                    ReplaceFile(".\\clanlong\\clanlong_1.bin", ".\\binkw32.dll");
                    ReplaceFile(".\\clanlong\\clanlong_2.bin", ".\\Loader.dll");
                    Process.Start(".\\Rome2.exe");
                    this.WindowState = WindowState.Minimized;
                }
                catch (IOException x)
                {
                    System.Windows.MessageBox.Show(x.Message.ToString());
                    Environment.Exit(0);
                }
            }
        }

        private bool md5check(string filename, string md5value)
        {
            using (MD5 md5 = MD5.Create())
            {
                using (FileStream stream = File.OpenRead(filename))
                {
                    byte[] data = md5.ComputeHash(stream);
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < data.Length; i++)
                    {
                        sb.Append(data[i].ToString("x2"));
                    }
                    StringComparer cmp = StringComparer.OrdinalIgnoreCase;
                    if (0 == cmp.Compare(sb.ToString(), md5value))
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
                if (stream != null) stream.Close();
            }

            return false;
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
                System.Windows.MessageBox.Show(x.Message.ToString());
                return false;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            foreach (Process pname in Process.GetProcessesByName("Rome2"))
            {
                pname.Kill();
            }
            foreach (Process pname in Process.GetProcessesByName("launcher"))
            {
                pname.Kill();
            }
            if (File.Exists(".\\binkw32.dll"))
            {

                if (!md5check(".\\binkw32.dll", "04B064676A2D466887581EA3FBE327EF"))
                {
                    File.Delete(".\\binkw32.dll");
                    if (File.Exists(".\\binkw32_ori.dll"))
                    {
                        while (isFileLocked(".\\binkw32_ori.dll"))
                        {
                            Thread.Sleep(1000);
                        }
                        ReplaceFile(".\\binkw32_ori.dll", ".\\binkw32.dll");
                        File.Delete(".\\binkw32_ori.dll");
                    }
                }
            }
            if (File.Exists(".\\Loader.dll"))
            {
                File.Delete(".\\Loader.dll");
            }
            //Application.Current.Shutdown();
            Environment.Exit(0);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Process.Start("http://www.clanlong.com/Totalwar_Rome2/1");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Window_Closed(sender, e);
        }

    }
}
