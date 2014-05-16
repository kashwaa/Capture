using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Capture
{
   public class RecorderFactory
    {
       public static Recorder CreateRecorder(string name)
       {
           switch (name)
           {
               case ".gif":
                   return new GIFRecorder();
               case ".avi":
                   return new VIDRecorder();
               default:
                   return null;
                  
           }
       }
    }
}
