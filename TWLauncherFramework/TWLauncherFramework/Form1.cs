using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TWLauncherFramework
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ModView_SelectedIndexChanged(object sender, EventArgs e)
        {
            ModView.View = View.List;
            ModView.LabelEdit = true;
            ModView.CheckBoxes = true;
            ModView.FullRowSelect = true;
            ModView.GridLines = true;
            ListViewItem item1 = new ListViewItem("item1", 0);
            item1.Checked = true;
            item1.SubItems.Add("1");
            item1.SubItems.Add("2");
            ListViewItem item2 = new ListViewItem("item2", 0);
            item2.Checked = true;
            item2.SubItems.Add("3");
            item2.SubItems.Add("4");

            ImageList imglist = new ImageList();
            imglist.Images.Add(Bitmap.FromFile("C:\\1.png"));
            ModView.LargeImageList = imglist;

            this.Controls.Add(ModView);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ModView.View = View.LargeIcon;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ModView.View = View.List;
        }
    }
}
