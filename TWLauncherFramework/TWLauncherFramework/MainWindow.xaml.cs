using System;
using System.Collections.Generic;
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
using System.IO;
using System.Collections.ObjectModel;
using System.Threading;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace TWLauncherFramework
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<modPack> mods = new ObservableCollection<modPack>();
        string modlistpath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)+"\\AppData\\Roaming\\The Creative Assembly\\Shogun2\\scripts\\user.script.txt";
        string datapath = Directory.GetCurrentDirectory() + "\\data\\";
        const string blankimage = "pack://application:,,,/pic/blank.png";
        const string interlimagepath = "pack://application:,,,/";
        //const string exeName = "Rome2";
        const string exeName = "Shogun2";

        int imageindex = 0;

        public MainWindow()
        {
            CheckEnvironment();
            InitializeComponent();
           // LoadPacks();
            
        }

        void CheckEnvironment()
        {
            if (!Directory.Exists(datapath))
            {
                System.Windows.MessageBox.Show("没有找到 data 文件夹！请确认程序被安装在了正确的目录下！");
                Environment.Exit(1);
            }
        }

        void LoadPacks()
        {
            Dictionary<String, int> modlist = new Dictionary<String, int>();
            if (File.Exists(modlistpath))
            {
                string[] lines = System.IO.File.ReadAllLines(modlistpath);
           
                int indx = 0;
                foreach (string line in lines)
                {
                    Regex rx = new Regex("mod\\s+\"(?<word>\\w+)\\.pack\"", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    MatchCollection matches = rx.Matches(line);
                    GroupCollection groups = matches[0].Groups;
                    string modname = groups["word"].Value;
                    if (File.Exists(datapath + modname + ".pack") && tools.check_is_mod(datapath+ modname + ".pack"))
                    {
                        modlist.Add(modname, indx);
                        indx += 1;
                    }
                }
            }
            string[] packpaths = Directory.GetFiles(datapath,"*.pack");
            List<string> packs = new List<string>();
            foreach (string packpath in packpaths)
            {
                string pack = System.IO.Path.GetFileNameWithoutExtension(packpath);
                if (tools.check_is_mod(datapath + pack + ".pack"))
                {
                    packs.Add(pack);
                }
            }
            foreach (string apack in modlist.Keys)
            {
                ImageSource modImage = new BitmapImage();
                if (File.Exists(datapath + apack + ".png"))
                {
                    modImage = tools.LoadImage(datapath + apack + ".png");
                }
                else
                {
                    modImage = tools.LoadImage(blankimage);
                }
                FileInfo fin = new FileInfo(datapath + apack + ".pack");
                string last_write_time = fin.LastWriteTime.ToString("u");
                string filesize = tools.BytesToString(fin.Length);
                mods.Add(new modPack() { isModActive = true, packname = apack, packdate = last_write_time, packsize = filesize, img = modImage });
            }
            foreach (string apack in packs)
            {
                if (!modlist.ContainsKey(apack))
                {
                    ImageSource modImage = new BitmapImage();
                    if (File.Exists(datapath + apack + ".png"))
                    {
                        modImage = tools.LoadImage(datapath + apack + ".png");
                        if (modImage == null) { 
                        System.Windows.MessageBox.Show("null");
                        }
                    }
                    else
                    {
                        modImage = tools.LoadImage(blankimage);
                    }

                    FileInfo fin = new FileInfo(datapath + apack + ".pack");
                    string last_write_time = fin.LastWriteTime.ToString("yyyy-MM-dd HH:mm");
                    //string last_write_time = fin.LastWriteTime.ToString("u");
                    string filesize = tools.BytesToString(fin.Length);

                    mods.Add(new modPack() { isModActive = false, packname = apack, packdate = last_write_time, packsize = filesize, img = modImage });
                }
            }
            Packs.ItemsSource = mods;


                
            if (mods.Count>0){
                modPack currentMod = mods[0];
                if (currentMod != null)
                {
                    ObservableCollection<modPack> cmod = new ObservableCollection<modPack>();
                    cmod.Add(currentMod);
                    first_mods_in_image.ItemsSource = cmod;
                   // prev.IsEnabled = false;
                }
            }
             
            if (mods.Count>1){
                modPack currentMod = mods[1];
                if (currentMod != null)
                {

                    ObservableCollection<modPack> cmod = new ObservableCollection<modPack>();
                    cmod.Add(currentMod);
                    second_mods_in_image.ItemsSource = cmod;
                }
            }
            else
            {
                second_mods_in_image.Visibility = Visibility.Hidden;
            }

            
        }

        private void prev_Click(object sender, RoutedEventArgs e)
        {
            first_mods_in_image.Visibility = Visibility.Visible;
            second_mods_in_image.Visibility = Visibility.Visible;

            imageindex -= 1;
            
            if (imageindex < 0)
            {
                imageindex = 0;
            }
      

            if (imageindex >= 0)
            {
                modPack currentMod = mods[imageindex];
                if (currentMod != null)
                {
                    ObservableCollection<modPack> cmod = new ObservableCollection<modPack>();
                    cmod.Add(currentMod);
                    second_mods_in_image.ItemsSource = cmod;
                }
            }

            imageindex -=1;
            if (imageindex  >= 0)
            {
                modPack currentMod = mods[imageindex];
                if (currentMod != null)
                {
                    ObservableCollection<modPack> cmod = new ObservableCollection<modPack>();
                    cmod.Add(currentMod);
                    first_mods_in_image.ItemsSource = cmod;
                }
            }
            else
            {
                first_mods_in_image.Visibility = Visibility.Hidden;
            }

            check_leftright_useful();


        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            first_mods_in_image.Visibility = Visibility.Visible;
            second_mods_in_image.Visibility = Visibility.Visible;
            
            imageindex += 1;
            
            if (imageindex >= mods.Count)
            {
                imageindex = mods.Count - 1;
            }

            if (imageindex < mods.Count)
            {
                modPack currentMod = mods[imageindex];
                if (currentMod != null)
                {
                    ObservableCollection<modPack> cmod = new ObservableCollection<modPack>();
                    cmod.Add(currentMod);
                    first_mods_in_image.ItemsSource = cmod;
                }
            }

            imageindex += 1;
            if (imageindex < mods.Count)
            {
                modPack currentMod = mods[imageindex];
                if (currentMod != null)
                {
                    ObservableCollection<modPack> cmod = new ObservableCollection<modPack>();
                    cmod.Add(currentMod);
                    second_mods_in_image.ItemsSource = cmod;
                }
            }
            else
            {
                second_mods_in_image.Visibility = Visibility.Hidden;
            }

            check_leftright_useful();
        }

        private void up_Click(object sender, RoutedEventArgs e)
        {
            int idx = Packs.SelectedIndex;
            if (idx != 0)
            {
                mods.Move(idx, idx - 1);
            }

            check_updown_useful();
        }

        private void down_Click(object sender, RoutedEventArgs e)
        {
            int idx = Packs.SelectedIndex;
            if (idx != mods.Count - 1)
            {
                mods.Move(idx, idx + 1);
            }

            check_updown_useful();
        }


        private void Chinese_start_Click(object sender, RoutedEventArgs e)
        {
            tools.write_back_to_modlist(modlistpath, mods, datapath);
            Process[] pname = Process.GetProcessesByName(exeName);
            if (pname.Length > 0)
            {
                System.Windows.MessageBox.Show("Shogun2TW is already running！");
            }
            else
            {
                if (!Directory.Exists(".\\clanlong"))
                {
                    System.Windows.MessageBox.Show("汉化文件缺失!!");
                    Environment.Exit(0);
                }
                try
                {
                    if (!File.Exists(".\\binkw32_ori.dll"))
                    {
                        while (tools.isFileLocked(".\\binkw32.dll"))
                        {
                            Thread.Sleep(1000);
                        }
                        tools.ReplaceFile(".\\binkw32.dll", ".\\binkw32_ori.dll");
                    }
                    if (!tools.md5check(".\\binkw32_ori.dll", "04B064676A2D466887581EA3FBE327EF"))
                    {
                        System.Windows.MessageBox.Show("缺少原生binkw32.dll！");
                        Environment.Exit(0);
                    }
                    tools.ReplaceFile(".\\clanlong\\clanlong_1.bin", ".\\binkw32.dll");
                    tools.ReplaceFile(".\\clanlong\\clanlong_2.bin", ".\\Loader.dll");
                    Process.Start(".\\Shogun2.exe");
                    this.WindowState = WindowState.Minimized;
                }
                catch (IOException x)
                {
                    System.Windows.MessageBox.Show(x.Message.ToString());
                    Environment.Exit(0);
                }
            }
        }

        private void English_start_Click(object sender, RoutedEventArgs e)
        {
            tools.write_back_to_modlist(modlistpath, mods, datapath);
            Process[] pname = Process.GetProcessesByName(exeName);
            if (pname.Length > 0)
            {
                System.Windows.MessageBox.Show("Shogun2TW is already running!");
            }
            else
            {
            //    if (!Directory.Exists(".\\clanlong"))
            //    {
            //        System.Windows.MessageBox.Show("汉化文件缺失!!");
            //        Environment.Exit(0);
            //    }
            //checkmd5:
            //    if (!tools.md5check(".\\binkw32.dll", "04B064676A2D466887581EA3FBE327EF"))
            //    {
            //        if (File.Exists(".\\binkw32_ori.dll"))
            //        {
            //            while (tools.isFileLocked(".\\binkw32_ori.dll"))
            //            {
            //                Thread.Sleep(1000);
            //            }
            //            tools.ReplaceFile(".\\binkw32_ori.dll", ".\\binkw32.dll");
            //            File.Delete(".\\binkw32_ori.dll");
            //            goto checkmd5;
            //        }
            //        else
            //        {
            //            System.Windows.MessageBox.Show("缺少原生binkw32.dll！");
            //            Environment.Exit(0);
            //        }
            //    }
            //    else
            //    {
                    //Process.Start(".\\Rome2.exe");
                    //Process.Start(".\\Attila.exe");
                Process.Start("steam://rungameid/315710");
                   // this.WindowState = WindowState.Minimized;


                Thread.Sleep(1000);
                Process proc = Process.GetProcessesByName(exeName)[0];
               // System.Windows.MessageBox.Show(proc.ProcessName + "Start!");
                uint dwAccl = 0x0002 | 0x0400 | 0x0008 | 0x0010 |0x0020;
                tools.InjectDLL((IntPtr)tools.OpenProcess(dwAccl, 1, proc.Id), "Loader.dll", proc);


           //     }
            }
        }



        private void Update_check_Click(object sender, RoutedEventArgs e)
        {
            tools.write_back_to_modlist(modlistpath, mods, datapath);
            Process.Start("http://www.clanlong.com/Totalwar_Rome2/1");
        }

        private void image_show_Click(object sender, RoutedEventArgs e)
        {
            ListGrid.Visibility = Visibility.Hidden;

            ImageGrid.Visibility = Visibility.Visible;
            check_leftright_useful();

        }

        private void list_show_Click(object sender, RoutedEventArgs e)
        {
            ListGrid.Visibility = Visibility.Visible;
            ImageGrid.Visibility = Visibility.Hidden;
            check_updown_useful();
        }

        private void close_window_Click(object sender, RoutedEventArgs e)
        {
            Window_Closing(sender, e);

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }

        }

        private void Window_Closing(object sender, EventArgs e)
        {
            tools.write_back_to_modlist(modlistpath, mods, datapath);
            foreach (Process pname in Process.GetProcessesByName(exeName))
            {
                pname.Kill();
            }
            foreach (Process pname in Process.GetProcessesByName("launcher"))
            {
                pname.Kill();
            }
            if (File.Exists(".\\binkw32.dll"))
            {

                if (!tools.md5check(".\\binkw32.dll", "04B064676A2D466887581EA3FBE327EF"))
                {
                    File.Delete(".\\binkw32.dll");
                    if (File.Exists(".\\binkw32_ori.dll"))
                    {
                        while (tools.isFileLocked(".\\binkw32_ori.dll"))
                        {
                            Thread.Sleep(1000);
                        }
                        tools.ReplaceFile(".\\binkw32_ori.dll", ".\\binkw32.dll");
                        File.Delete(".\\binkw32_ori.dll");
                    }
                }
            }
            //if (File.Exists(".\\Loader.dll"))
            //{
            //    File.Delete(".\\Loader.dll");
            //}
            //Application.Current.Shutdown();
            Environment.Exit(0);
        }

        private void Mod_manager_Click(object sender, RoutedEventArgs e)
        {
            ModView.Visibility = Visibility.Visible;
            StartView.Visibility = Visibility.Hidden;
            check_leftright_useful();
            check_updown_useful();
        }

        private void back_start_Click(object sender, RoutedEventArgs e)
        {
            ModView.Visibility = Visibility.Hidden;
            StartView.Visibility = Visibility.Visible;
        }

        private void Packs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            check_updown_useful();

        }

        private void check_updown_useful()
        {
            int idx = Packs.SelectedIndex;
           // System.Windows.MessageBox.Show(idx.ToString());
            if (idx <= 0)
            {
                up.IsEnabled = false;
            }
            else
            {
                up.IsEnabled = true;
            }
            if ((idx >= Packs.Items.Count - 1)||(idx == -1))
            {
                down.IsEnabled = false;
            }
            else
            {
                down.IsEnabled = true;
            }
        }

        private void check_leftright_useful()
        {
            //System.Windows.MessageBox.Show(imageindex.ToString());
            if (imageindex <= 0)
            {
                prev.IsEnabled = false;
            }
            else
            {
                prev.IsEnabled = true;
            }
            if (imageindex >= mods.Count - 1)
            {
                next.IsEnabled = false;
            }
            else
            {
                next.IsEnabled = true;
            }

        }


    }

    public class modPack
    {
        public bool isModActive { get; set; }
        public string packname { get; set; }
        public string packdate { get; set; }
        public string packsize { get; set; }
        public ImageSource img { get; set; }
    }

    public class tools
    {
        public static bool md5check(string filename, string md5value)
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

        public static bool isFileLocked(string filename)
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

        public static bool ReplaceFile(string sourceFile, string destFile)
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

        public static bool check_is_mod(string filename){
            const int PACKTYPE_MOD = 3;
            byte[] buffer = new byte[8];
            try{
                using(FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read)){
                    fs.Read(buffer,0,buffer.Length);
                    fs.Close();
                }

            }catch(System.UnauthorizedAccessException ex){
                 System.Windows.MessageBox.Show("无法读取mod列表!\n"+ex.ToString());
            }
            int packtype = buffer[4];
            if (packtype == PACKTYPE_MOD)
            {
                return true;
            }
            return false;
        }

        public static void write_back_to_modlist(string filename, ObservableCollection<modPack> mods, string datapath)
        {
            List<String> lines = new List<String>();
            foreach (modPack mod in mods)
            {
                if (mod.isModActive == true)
                {
                    if (File.Exists(datapath + mod.packname + ".pack"))
                    {
                        lines.Add("mod  \"" + mod.packname + ".pack\"");
                    }
                }
            }
            if (!File.Exists(filename))
            {
                FileInfo fileInfo = new FileInfo(filename);
                if (!fileInfo.Exists)
                {
                    Directory.CreateDirectory(fileInfo.Directory.FullName);
                }
                File.Create(filename).Close();
            }
            File.WriteAllLines(filename, lines);
        }

        public static ImageSource LoadImage(string filename)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            //bitmap.UriSource = new Uri(filename.Replace("\\", "/"),UriKind.Relative);
            bitmap.UriSource = new Uri(filename.Replace("\\", "/"));
            bitmap.EndInit();
            return bitmap;
        }

        public static String BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }

        [DllImport("kernel32")]
        public static extern IntPtr CreateRemoteThread(
          IntPtr hProcess,
          IntPtr lpThreadAttributes,
          uint dwStackSize,
          UIntPtr lpStartAddress, // raw Pointer into remote process  
          IntPtr lpParameter,
          uint dwCreationFlags,
          out IntPtr lpThreadId
        );

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(
            UInt32 dwDesiredAccess,
            Int32 bInheritHandle,
            Int32 dwProcessId
            );


        [DllImport("kernel32.dll")]
        public static extern Int32 CloseHandle(
        IntPtr hObject
        );

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool VirtualFreeEx(
            IntPtr hProcess,
            IntPtr lpAddress,
            UIntPtr dwSize,
            uint dwFreeType
            );

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
        public static extern UIntPtr GetProcAddress(
            IntPtr hModule,
            string procName
            );

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(
            IntPtr hProcess,
            IntPtr lpAddress,
            uint dwSize,
            uint flAllocationType,
            uint flProtect
            );


        [DllImport("kernel32.dll")]
        static extern bool WriteProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            string lpBuffer,
            UIntPtr nSize,
            out IntPtr lpNumberOfBytesWritten
        );

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(
            string lpModuleName
            );

        [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
        internal static extern Int32 WaitForSingleObject(
            IntPtr handle,
            Int32 milliseconds
            );

        [DllImport("kernel32.dll")]
        internal static extern Int32 ResumeThread(
            IntPtr handle
            );

         public static void InjectDLL(IntPtr hProcess, String strDLLName, Process proc)
        {
            IntPtr bytesout;

            // Length of string containing the DLL file name +1 byte padding
            Int32 LenWrite = strDLLName.Length + 1;
            // Allocate memory within the virtual address space of the target process
            IntPtr AllocMem = (IntPtr)VirtualAllocEx(hProcess, (IntPtr)null, (uint)LenWrite, 0x1000, 0x40); //allocation pour WriteProcessMemory

            // Write DLL file name to allocated memory in target process
            WriteProcessMemory(hProcess, AllocMem, strDLLName, (UIntPtr)LenWrite, out bytesout);
            // Function pointer "Injector"
            UIntPtr Injector = (UIntPtr)GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");

            if (Injector == null)
            {
                Console.WriteLine(" Injector Error! \n ");
                // return failed
                return;
            }

            // Create thread in target process, and store handle in hThread
            IntPtr hThread = (IntPtr)CreateRemoteThread(hProcess, (IntPtr)null, 0, Injector, AllocMem, 0, out bytesout);
            // Make sure thread handle is valid
            if (hThread == null)
            {
                //incorrect thread handle ... return failed
                Console.WriteLine(" hThread [ 1 ] Error! \n ");
                return;
            }
            // Time-out is 10 seconds...
            int Result = WaitForSingleObject(hThread, 10 * 1000);
            // Check whether thread timed out...
            if (Result == 0x00000080L || Result == 0x00000102L || Result == 0xFFFFFFFFL)
            {
                /* Thread timed out... */
                Console.WriteLine(" hThread [ 2 ] Error! \n ");
                // Make sure thread handle is valid before closing... prevents crashes.
                if (hThread != null)
                {
                    //Close thread in target process
                    CloseHandle(hThread);
                }
                return;
            }
            // Sleep thread for 1 second
            Thread.Sleep(1000);
            // Clear up allocated space ( Allocmem )
            VirtualFreeEx(hProcess, AllocMem, (UIntPtr)0, 0x8000);
            // Make sure thread handle is valid before closing... prevents crashes.
            if (hThread != null)
            {
                //Close thread in target process
                CloseHandle(hThread);
            }
            // return succeeded
            ResumeThread(hThread);
            System.Windows.MessageBox.Show("Inject!");
            return;
        }
    }

    public class ImageButton : Button
    {
        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton),
                                                     new FrameworkPropertyMetadata(typeof(ImageButton)));
        }

        #region properties

        public ImageSource HoverImage
        {
            get { return (ImageSource)GetValue(HoverImageProperty); }
            set { SetValue(HoverImageProperty, value); }
        }

        public ImageSource NormalImage
        {
            get { return (ImageSource)GetValue(NormalImageProperty); }
            set { SetValue(NormalImageProperty, value); }
        }

        public ImageSource PressedImage
        {
            get { return (ImageSource)GetValue(PressedImageProperty); }
            set { SetValue(PressedImageProperty, value); }
        }

        public ImageSource DisabledImage
        {
            get { return (ImageSource)GetValue(DisabledImageProperty); }
            set { SetValue(DisabledImageProperty, value); }
        }

        public ImageSource ShowedImage
        {
            get { return (ImageSource)GetValue(ShowedImageProperty); }
            set { SetValue(DisabledImageProperty, value); }
        }

        #endregion

        #region dependency properties

        public static readonly DependencyProperty DisabledImageProperty =
            DependencyProperty.Register(
                "DisabledImage", typeof(ImageSource), typeof(ImageButton));

        public static readonly DependencyProperty HoverImageProperty =
            DependencyProperty.Register(
                "HoverImage", typeof(ImageSource), typeof(ImageButton));

        public static readonly DependencyProperty NormalImageProperty =
            DependencyProperty.Register(
                "NormalImage", typeof(ImageSource), typeof(ImageButton));

        public static readonly DependencyProperty PressedImageProperty =
            DependencyProperty.Register(
                "PressedImage", typeof(ImageSource), typeof(ImageButton));

        public static readonly DependencyProperty ShowedImageProperty =
            DependencyProperty.Register(
                "ShowedImage", typeof(ImageSource), typeof(ImageButton));

        #endregion

    }
}
