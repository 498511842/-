using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 拖动批量复制
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.GridLines = true;//在各数据之间形成网格线
            listView1.View = View.Details;//显示列名称
            listView1.FullRowSelect = true;//在单击某项时，对其进行选中
            listView1.HeaderStyle = ColumnHeaderStyle.Nonclickable;//隐藏列标题
            listView1.Columns.Add("文件路径", listView1.Width - 5, HorizontalAlignment.Right);
        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;       //设置拖放操作中目标放置类型为复制
            String[] str_Drop = (String[])e.Data.GetData(DataFormats.FileDrop, true);//检索数据格式相关联的数据
            Data_List(listView1, str_Drop[0]);
        }
        public void Data_List(ListView LV,string F)
        {
            ListViewItem item = new ListViewItem(F);
            LV.Items.Add(item);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }
        CopyClass copyClass = new CopyClass();
        private void button2_Click(object sender, EventArgs e)
        {
            string FileName = "";
            int tem_n = 0;
            string DName = "";
            if (textBox1.Text.Length > 0 && listView1.Items.Count > 0)
            {
                try
                {
                    for (int i = 0; i < listView1.Items.Count; i++)
                    {
                        FileName = listView1.Items[i].SubItems[0].Text;
                        tem_n = FileName.LastIndexOf("\\");
                        FileName = FileName.Substring(tem_n + 1, FileName.Length - tem_n - 1);
                        DName = textBox1.Text.Trim() + "\\" + FileName;
                        copyClass.CopyFile(listView1.Items[i].SubItems[0].Text, DName, 1024);
                        this.Text = "复制：" + listView1.Items[i].SubItems[0].Text;
                    }
                    MessageBox.Show("文件批量复制完成。");
                }
                catch
                {
                    MessageBox.Show("文件复制错误。");
                }
            }
        }
    }
}
