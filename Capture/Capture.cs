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

namespace Capture
{
    public partial class Capture : Form
    {
        RecData r = null;
        bool rec = false;
        bool pause = false;

        public Capture()
        {
            InitializeComponent();
            this.Visible = false;
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        private void Capture_Load(object sender, EventArgs e)
        {
           
            
        }

        void Record(object o)
        {
            var r = (RecData)o; 
            String outputFilePath=r.path;
           
            
            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }
           
            AnimatedGifEncoder en = new AnimatedGifEncoder();
            en.Start(outputFilePath);
            en.SetRepeat(0);
          
            while (rec)
            {
                if (!pause)
                {
                    Stopwatch st = new Stopwatch();
                    st.Start();
                    using (Bitmap bmpScreenCapture = new Bitmap(r.width, r.height))
                    {
                        using (Graphics g = Graphics.FromImage(bmpScreenCapture))
                        {
                            g.CopyFromScreen(r.pos,
                                             new Point(0, 0),
                                             bmpScreenCapture.Size,
                                             CopyPixelOperation.SourceCopy);
                            Rectangle cursorBounds = new Rectangle(new Point(Cursor.Position.X-r.pos.X,Cursor.Position.Y-r.pos.Y), Cursors.Default.Size);
                            Cursors.Default.Draw(g, cursorBounds);
                        }
                        en.AddFrame(bmpScreenCapture);
                        st.Stop();
                        var t = st.ElapsedMilliseconds;
                        en.SetDelay((int)(200+t));
                        if(200-t>0)
                        Thread.Sleep((int)(200 - t));
                    }
                    
                    
                }

            }
            
           
            en.Finish();
            en.SetDispose(0);
        }
        
        private void recordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Select s =r== null?new Select():new Select(r);
            s.ShowDialog();
           
            r = s.recData;
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                r.path = saveFileDialog1.FileName+".gif";
            }
            else
            {
                return;
            }
            rec = true;
            Thread t = new Thread(new ParameterizedThreadStart(Record));
            t.Start(r);
            recordToolStripMenuItem.Enabled = false;
            stopToolStripMenuItem.Enabled = true;
            pauseToolStripMenuItem.Enabled = true;
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rec = false;
            recordToolStripMenuItem.Enabled = true;
            stopToolStripMenuItem.Enabled = false;
            pauseToolStripMenuItem.Enabled = false;
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!pause)
            {
                pause = true;
                pauseToolStripMenuItem.Text = "Resume";
            }
            else
            {
                pause = false;
                pauseToolStripMenuItem.Text = "Pause";
            }
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            //contextMenuStrip1.Show(Control.MousePosition);
        }
    }
   
}

