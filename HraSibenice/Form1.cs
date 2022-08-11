using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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

        private Random random = new Random();
        private string sentence;
        private int errors = 0;
        private IEnumerable<char> choosedChars = new List<char>();
        private DialogResult result;

        private void GameStart()
        {
            sentence = GetSentence;
            errors = 0;
            choosedChars = new List<char>();
            labelSentence.Text = GetMask();
            LoadPicture(errors);
            ShowButtons();
        }

        private string GetSentence
        {
            get
            {
                string fn = GetAppDir + @"\sentences.txt";
                if (File.Exists(fn))
                {
                    string[] sentences = File.ReadAllLines(fn);
                    int i = random.Next(0, sentences.Count());
                    return sentences[i];
                }
                else
                {
                    MessageBox.Show(String.Format("File {0} wasn´t found!", fn));
                    Close();
                    return "";
                }
            }
        }

        private void ShowButtons()
        {
            foreach (Button btn in buttonsPanel.Controls)
            {
                btn.Visible = true;
            }
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

        private void LoadPicture(int i)
        {
            string dir = GetAppDir + @"\pics\";
            string fn = dir + i.ToString() + ".png";
            if (File.Exists(fn))
            {
                pictureBox.Image = new Bitmap(fn);
            }
        }

        private void GameOver()
        {
            result = MessageBox.Show("GAME OVER!!! \nThe sentece was:" + sentence + "\nNew game?", "GameOver", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                GameStart();
            }
            else
            {
                Close();
            }
        }

        private void Miss()
        {
            errors++;
            LoadPicture(errors);
            if (errors == 10)
            {
                GameOver();
            }
        }

        private bool Win()
        {
            foreach (char c in sentence)
            {
                if (!(c == ' ' || choosedChars.Contains(c)))
                {
                    return false;
                }
            }
            return true;
        }

        private void GameWin()
        {
            string dir = GetAppDir + @"\pics\";
            string fn = dir + "win.png";
            if (File.Exists(fn))
            {
                pictureBox.Image = new Bitmap(fn);
            }
            result = MessageBox.Show("You won!!!\nNew game?", "win", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                GameStart();
            }
            else
            {
                Close();
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
                bool win = Win();
                if (win)
                {
                    GameWin();
                }
            }
            else
            {
                Miss();
            }
        }
    }
}
