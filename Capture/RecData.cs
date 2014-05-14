using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Capture
{
   public class RecData
    {
       public RecData(int x, int y, int width, int height)
       {
           this.pos = new Point(x, y);
           this.width = width;
           this.height = height;
           
       }
        public Point pos { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string path  { get; set; }
    }

}
