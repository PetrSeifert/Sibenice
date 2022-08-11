using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HraSibenice
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            GameStart();
        }

        private string sentence;
        private int errors = 0;
        private IEnumerable<char> choosedChars = new List<char>();

        private void GameStart()
        {
            sentence = "hello".ToUpper();
            errors = 0;
            choosedChars = new List<char>();
            labelSentence.Text = GetMask();
            LoadPicture(errors);
        }

        private string GetMask()
        {
            string mask = "";
            foreach (char c in sentence)
            {
                if (c == ' ' || choosedChars.Contains(c))
                {
                    mask = mask + c;
                }
                else
                {
                    mask = mask + "?";
                }
            }
            return mask;
        }

        private IList<char> GetCharList(char c)
        {
            IList<char> list = new List<char>();
            list.Add(c);
            return list;
        }

        private bool Hit(IList<char> list)
        {
            foreach (char c in sentence)
            {
                if (list.Contains(c))
                {
                    return true;
                }
            }
            return false;
        }

        private string GetAppDir
        {
            get 
            {
                FileInfo fi = new FileInfo(Application.ExecutablePath);
                return fi.DirectoryName;
            }
        }

        private bool LoadPicture(int i)
        {
            string dir = GetAppDir + @"\pics\";
            string fn = dir + i.ToString() + ".png";
            if (File.Exists(fn))
            {
                pictureBox.Image = new Bitmap(fn);
                return true;
            }
            return false;
        }

        private void GameOver()
        {
           
        }

        private void Miss()
        {
            errors++;
            bool ok = LoadPicture(errors);
            if (!ok)
            {
                GameOver();
            }
        }

        private void aButton_Click(object sender, EventArgs e)
        {
            Button btn = (sender as Button);
            char c = btn.Text[0];
            IList<char> list = GetCharList(c);
            bool hit = Hit(list);
            choosedChars = choosedChars.Concat(list);
            btn.Visible = false;
            if (hit)
            {
                labelSentence.Text = GetMask();
            }
            else
            {
                Miss();
            }
        }
    }
}
