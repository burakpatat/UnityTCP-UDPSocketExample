using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameServerLog
{
    public partial class Form1 : Form
    {
        string PATH = @"D:\Projects\Server2020\ServerLog.txt";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (File.Exists(PATH))
            {
                File.WriteAllText(PATH, String.Empty);
            }
            richTextBox1.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Timer timer = new Timer();
            timer.Interval = 1500; 
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            if (File.Exists(PATH))
            {
                File.WriteAllText(PATH, String.Empty);
            }
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            StreamReader _streamReader = File.OpenText(PATH);
            string log;
            while ((log = _streamReader.ReadLine()) != null)
            {
                richTextBox1.AppendText("\n" + log);
            }
            _streamReader.Close();

            if (File.Exists(PATH))
            {
                File.WriteAllText(PATH, String.Empty);
            }
        }
    }
}
