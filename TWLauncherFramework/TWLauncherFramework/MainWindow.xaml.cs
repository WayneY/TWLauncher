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

namespace TWLauncherFramework
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<modPack> mods = new ObservableCollection<modPack>();
        const string modlistpath = @".\modlist.txt";
        string datapath = Directory.GetCurrentDirectory() + "\\data\\";
        const string blankimage = "pack://application:,,,/pic/blank.png";
        int imageindex = 0;
        public MainWindow()
        {
            InitializeComponent();
            LoadPacks();
            
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
                    if (File.Exists(datapath + line + ".pack") && tools.check_is_mod(datapath+ line + ".pack"))
                    {
                        modlist.Add(line, indx);
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
                mods.Add(new modPack() { isModActive = true, packname = apack, img = modImage });
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
                    mods.Add(new modPack() { isModActive = false, packname = apack, img = modImage });
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
        }

        private void prev_Click(object sender, RoutedEventArgs e)
        {
            //leftCB.Visibility = Visibility.Visible;
            //leftImage.Visibility = Visibility.Visible;
            //rightCB.Visibility = Visibility.Visible;
            //rightImage.Visibility = Visibility.Visible;
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
                 //   rightCB.IsChecked = currentMod.isModActive;
                    //rightCB.Content = currentMod.packname;
                    //rightImage.Source = currentMod.img;
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
                 //   leftCB.IsChecked = currentMod.isModActive;
                    //leftCB.Content = currentMod.packname;
                    //leftImage.Source = currentMod.img;
                    ObservableCollection<modPack> cmod = new ObservableCollection<modPack>();
                    cmod.Add(currentMod);
                    first_mods_in_image.ItemsSource = cmod;
                }
            }
            else
            {
                //leftCB.Visibility = Visibility.Hidden;
                //leftImage.Visibility = Visibility.Hidden;
                first_mods_in_image.Visibility = Visibility.Hidden;
            }


        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            //leftCB.Visibility = Visibility.Visible;
            //leftImage.Visibility = Visibility.Visible;
            //rightCB.Visibility = Visibility.Visible;
            //rightImage.Visibility = Visibility.Visible;
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
                 //   leftCB.IsChecked = currentMod.isModActive;
                    //leftCB.Content = currentMod.packname;
                    //leftImage.Source = currentMod.img;
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
                 //   rightCB.IsChecked = currentMod.isModActive;
                    //rightCB.Content = currentMod.packname;
                    //rightImage.Source = currentMod.img;
                    ObservableCollection<modPack> cmod = new ObservableCollection<modPack>();
                    cmod.Add(currentMod);
                    second_mods_in_image.ItemsSource = cmod;
                }
            }
            else
            {
                //rightCB.Visibility = Visibility.Hidden;
                //rightImage.Visibility = Visibility.Hidden;
                second_mods_in_image.Visibility = Visibility.Hidden;
            }
        }

        private void up_Click(object sender, RoutedEventArgs e)
        {
            int idx = Packs.SelectedIndex;
            if (idx != 0)
            {
                mods.Move(idx, idx - 1);
            }
            
        }

        private void down_Click(object sender, RoutedEventArgs e)
        {
            int idx = Packs.SelectedIndex;
            if (idx != mods.Count - 1)
            {
                mods.Move(idx, idx + 1);
            }

        }


        private void Chinese_start_Click(object sender, RoutedEventArgs e)
        {
            tools.write_back_to_modlist(modlistpath, mods);
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

        private void English_start_Click(object sender, RoutedEventArgs e)
        {
            tools.write_back_to_modlist(modlistpath, mods);
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
                if (!tools.md5check(".\\binkw32.dll", "04B064676A2D466887581EA3FBE327EF"))
                {
                    if (File.Exists(".\\binkw32_ori.dll"))
                    {
                        while (tools.isFileLocked(".\\binkw32_ori.dll"))
                        {
                            Thread.Sleep(1000);
                        }
                        tools.ReplaceFile(".\\binkw32_ori.dll", ".\\binkw32.dll");
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



        private void Update_check_Click(object sender, RoutedEventArgs e)
        {
            tools.write_back_to_modlist(modlistpath, mods);
            Process.Start("http://www.clanlong.com/Totalwar_Rome2/1");
        }

        private void image_show_Click(object sender, RoutedEventArgs e)
        {
            Packs.Visibility = Visibility.Hidden;
            up.Visibility = Visibility.Hidden;
            down.Visibility = Visibility.Hidden;
            //leftImage.Visibility = Visibility.Visible;
            //rightImage.Visibility = Visibility.Visible;
            first_mods_in_image.Visibility = Visibility.Visible;
            second_mods_in_image.Visibility = Visibility.Visible;
            ImageGrid.Visibility = Visibility.Visible;
            prev.Visibility = Visibility.Visible;
            next.Visibility = Visibility.Visible;
        }

        private void list_show_Click(object sender, RoutedEventArgs e)
        {
            Packs.Visibility = Visibility.Visible;
            up.Visibility = Visibility.Visible;
            down.Visibility = Visibility.Visible;
            //leftImage.Visibility = Visibility.Hidden;
            //rightImage.Visibility = Visibility.Hidden;
            ImageGrid.Visibility = Visibility.Hidden;
            prev.Visibility = Visibility.Hidden;
            next.Visibility = Visibility.Hidden;
        }

        private void close_window_Click(object sender, RoutedEventArgs e)
        {
            tools.write_back_to_modlist(modlistpath, mods);

        }



    }

    public class modPack
    {
        public bool isModActive { get; set; }
        public string packname { get; set; }
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

        public static void write_back_to_modlist(string filename, ObservableCollection<modPack> mods)
        {
            List<String> lines = new List<String>();
            foreach (modPack mod in mods)
            {
                if (mod.isModActive == true)
                {
                    lines.Add(mod.packname);
                }
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
    }
}
