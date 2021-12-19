using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace gta_ovrd
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        char[] kchars = {'t','u','K','r','s','i'};

        FileStream fs;
        StreamWriter sw;
        StreamReader sr;
        string tmpstr = null;
        string[] files = { "gta_sa.exe", "data\\ovrd\\df.ovr", "data\\ovrd\\v.img",
                             "data\\ovrd\\hnd.cfg" };
        string[] files2 = { "LaunchGTAIV.exe", "GTAIV.exe", "common\\data\\gta.dat",
                              "common\\ovrd\\overrd.img", "common\\ovrd\\handling.dat"};

        private void button1_Click(object sender, EventArgs e)
        {
            if (File.Exists(files[0]))
            {
                File.Copy("gta_sa.exe", "gta_sa.exe.bak");
                fs = new FileStream("gta_sa.exe", FileMode.Open, FileAccess.Read);
                sr = new StreamReader(fs, Encoding.Default);
                tmpstr = sr.ReadToEnd();
                tmpstr = Regex.Replace(tmpstr, @"MODELS\\GTA3.IMG", @"DATA\OVRD\V.IMG", RegexOptions.IgnoreCase);
                tmpstr = Regex.Replace(tmpstr, @"DATA\\DEFAULT.DAT", @"DATA\OVRD\DF.OVR", RegexOptions.IgnoreCase);
                tmpstr = Regex.Replace(tmpstr, @"HANDLING.CFG", @"OVRD\HND.CFG", RegexOptions.IgnoreCase);
                sr.Close();
                fs.Close();
                fs = new FileStream("gta_sa.exe", FileMode.Truncate, FileAccess.Write);
                sw = new StreamWriter(fs, Encoding.Default);
                sw.Write(tmpstr);
                sw.Close();
                fs.Close();
                MessageBox.Show("Succesfully Patched!", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                button1.Enabled = false;
                button2.Enabled = true;
            }
            else if (File.Exists(files2[0]) && File.Exists(files2[2]))
            {
                File.SetAttributes("common\\data\\gta.dat", FileAttributes.Normal);
                File.Copy("common\\data\\gta.dat", "common\\data\\gta.dat.bak");
                fs = new FileStream("common\\data\\gta.dat", FileMode.Truncate, FileAccess.Write);
                sw = new StreamWriter(fs, Encoding.Default);
                sw.Write(Properties.Resources.gta);
                sw.Close();
                fs.Close();
                File.SetAttributes("common\\data\\default.dat", FileAttributes.Normal);
                File.Copy("common\\data\\default.dat", "common\\data\\default.dat.bak");
                fs = new FileStream("common\\data\\default.dat", FileMode.Truncate, FileAccess.Write);
                sw = new StreamWriter(fs, Encoding.Default);
                sw.Write(Properties.Resources.def);
                sw.Close();
                fs.Close();
                MessageBox.Show("Succesfully Patched!", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                button1.Enabled = false;
                button2.Enabled = true;
            }
            else MessageBox.Show("No GTA found! (SA or IV)", "Error",
                     MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (File.Exists("gta_sa.exe.bak"))
            {
                File.Delete("gta_sa.exe");
                File.Copy("gta_sa.exe.bak", "gta_sa.exe");
                File.Delete("gta_sa.exe.bak");
                MessageBox.Show("Succesfully Restored!", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                button1.Enabled = true;
                button2.Enabled = false;
            }
            else if (File.Exists("common\\data\\gta.dat.bak"))
            {
                File.Delete("common\\data\\gta.dat");
                File.Copy("common\\data\\gta.dat.bak", "common\\data\\gta.dat");
                File.Delete("common\\data\\gta.dat.bak");
                File.Delete("common\\data\\default.dat");
                File.Copy("common\\data\\default.dat.bak", "common\\data\\default.dat");
                File.Delete("common\\data\\default.dat.bak");
                MessageBox.Show("Succesfully Restored!", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                button1.Enabled = true;
                button2.Enabled = false;
            }
            else MessageBox.Show("No GTA found! (SA or IV)", "Error",
                     MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists("gta_sa.exe")) button2.Text += "SA";
            else if (File.Exists("LaunchGTAIV.exe")) button2.Text += "IV";
            else button2.Text += "?";

            if (File.Exists("gta_sa.exe.bak") || File.Exists("common\\data\\gta.dat.bak"))
            {
                button1.Enabled = false;
                button2.Enabled = true;
            }
            else
            {
                button1.Enabled = true;
                button2.Enabled = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button2.Text.Contains("GTA:SA"))
            {
                for (int i = 0; i < files.Length; i++)
                {
                    if (!File.Exists(files[i]))
                    {
                        MessageBox.Show(files[i] + " is missing!", "Error",
                             MessageBoxButtons.OK, MessageBoxIcon.Error);
                        goto badend;
                    }
                }
            }
            else if (button2.Text.Contains("GTA:IV"))
            {
                for (int i = 0; i < files2.Length; i++)
                {
                    if (!File.Exists(files2[i]))
                    {
                        MessageBox.Show(files2[i] + " is missing!", "Error",
                             MessageBoxButtons.OK, MessageBoxIcon.Error);
                        goto badend;
                    }
                }
            }
            else
            {
                MessageBox.Show("No GTA found! (SA or IV)", "Error",
                         MessageBoxButtons.OK, MessageBoxIcon.Error);
                goto badend;
            }
            if (button2.Enabled)
                MessageBox.Show(
                    "Override status:\n" +
                    "Override Files: INSTALLED\n" +
                    "Override Mode: INSTALLED", "Info",
                      MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(
                    "Override status:\n" +
                    "Override Files: INSTALLED\n" +
                    "Override Mode: NOT INSTALLED", "Info",
                      MessageBoxButtons.OK, MessageBoxIcon.Information);
        badend: { }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
            "It's a simple tool, that allows you to:\n" +
            "- Use a different img file for you mods (while all other imgs are untouched)\n" +
            "- Use custom handling mods (handling.cfg is not used while override is installed)\n\n" +
            "This tool can be only used with GTA San Andreas and GTA IV\n\n" +
            "WARNING!\nUsing col files, that exist in the original img files may crash the game.\n\n\n" +
            "This program is written by " +
            kchars[2] + kchars[1] + kchars[3] + kchars[0] + kchars[5] + kchars[4] +
            " with C# Express Edition 2008",
            "About GTA Override",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (button2.Text.Contains("GTA:SA"))
            {
                if (Directory.Exists(@"data\ovrd"))
                {
                    System.Diagnostics.Process.Start("explorer.exe", @"data\ovrd");
                }
                else
                    MessageBox.Show("Override folder not found!", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (button2.Text.Contains("GTA:IV"))
            {
                if (Directory.Exists(@"common\ovrd"))
                {
                    System.Diagnostics.Process.Start("explorer.exe", @"common\ovrd");
                }
                else
                    MessageBox.Show("Override folder not found!", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("No GTA found! (SA or IV)", "Error",
                             MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}