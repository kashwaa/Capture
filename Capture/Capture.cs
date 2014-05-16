using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gif.Components;
using System.Threading;
using System.IO;
using System.Diagnostics;
using AForge.Video.FFMPEG;

namespace Capture
{
    public partial class Capture : Form
    {
        RecData r = null;
        Recorder recorder = null;
        public Capture()
        {
            InitializeComponent();
            this.Visible = false;
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }
       
        private void recordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Select s =r== null?new Select():new Select(r);
            recordToolStripMenuItem.Enabled = false;
            if (s.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                recordToolStripMenuItem.Enabled = true;
                return;
            }
            r = s.recData;
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                r.path = saveFileDialog1.FileName;
            }
            else
            {
                recordToolStripMenuItem.Enabled = true;
                return;
            }
            recordToolStripMenuItem.Enabled = true;
            
            ParameterizedThreadStart pts = null;
            recorder = RecorderFactory.CreateRecorder(Path.GetExtension(r.path).ToLower());
            pts = new ParameterizedThreadStart(recorder.Record);
            Thread t = new Thread(pts);
            t.Start(r);
            recordToolStripMenuItem.Enabled = false;
            stopToolStripMenuItem.Enabled = true;
            pauseToolStripMenuItem.Enabled = true;
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            recorder.Stop();
            recordToolStripMenuItem.Enabled = true;
            stopToolStripMenuItem.Enabled = false;
            pauseToolStripMenuItem.Enabled = false;
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!recorder.IsPaused)
            {
                recorder.TogglePause();
                pauseToolStripMenuItem.Text = "Resume";
            }
            else
            {
                recorder.TogglePause();
                pauseToolStripMenuItem.Text = "Pause";
            }
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        
    }
   
}

