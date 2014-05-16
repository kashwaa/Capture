using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using AForge.Video.FFMPEG;

namespace Capture
{
    public class VIDRecorder : Recorder
    {
        public override void Record(object o)
        {
            var r = (RecData)o;
            String outputFilePath = r.path;

            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }
            rec = true;
            var vFWriter = new VideoFileWriter();

            vFWriter.Open(outputFilePath, 800, 600, 5, VideoCodec.MPEG4);


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
                            Rectangle cursorBounds = new Rectangle(new Point(Cursor.Position.X - r.pos.X, Cursor.Position.Y - r.pos.Y), Cursors.Default.Size);
                            Cursors.Default.Draw(g, cursorBounds);
                        }
                        vFWriter.WriteVideoFrame(ReduceBitmap(bmpScreenCapture, 800, 600));
                        st.Stop();
                        var t = st.ElapsedMilliseconds;

                        if (200 - t > 0)
                            Thread.Sleep((int)(200 - t));
                    }


                }

            }


            vFWriter.Close();
        }

        public Bitmap ReduceBitmap(Bitmap original, int reducedWidth, int reducedHeight)
        {
            var reduced = new Bitmap(reducedWidth, reducedHeight);
            using (var dc = Graphics.FromImage(reduced))
            {
                // you might want to change properties like
                dc.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                dc.DrawImage(original, new Rectangle(0, 0, reducedWidth, reducedHeight), new Rectangle(0, 0, original.Width, original.Height), GraphicsUnit.Pixel);
            }

            return reduced;
        }
    }
}