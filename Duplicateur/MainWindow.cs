using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Duplicateur
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void onBtnExploreClick(object sender, EventArgs e)
        {
            /*
            Stream myStream = null;
            OpenFileDialog fileExplorer = new OpenFileDialog();

            fileExplorer.InitialDirectory = "c:\\";
            //openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            //fileExplorer.FilterIndex = 2;
            fileExplorer.RestoreDirectory = true;

            if (fileExplorer.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = fileExplorer.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            // Insert code to read the stream here.
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
             */
            /*
            FileFolderDialog ffd = new FileFolderDialog();

            if (ffd.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show(ffd.SelectedPath);
            }
             */


        }
    }
}
