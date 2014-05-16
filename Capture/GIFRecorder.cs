using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gif.Components;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace Capture
{
   public class GIFRecorder:Recorder
    {

        public override void Record(object o)
        {
            var r = (RecData)o; 
            String outputFilePath=r.path;
           
            
            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }
            rec = true;
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
        
    }
}
