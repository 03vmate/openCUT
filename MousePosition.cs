using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenCUT
{
    class MousePosition
    {
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);
        public int[] screenCoordExtremes = new int[4];

        /// S
        /// <param name="screens">Array of Screen objects that are present on the current system. Use Screen.AllScreens</param>
        public MousePosition(Screen[] screens)
        {
            //Find total screen area coordinate space
            foreach (Screen screen in screens)
            {
                var bounds = screen.Bounds;
                screenCoordExtremes[0] = Math.Min(screenCoordExtremes[0], bounds.X);
                screenCoordExtremes[1] = Math.Min(screenCoordExtremes[1], bounds.Y);
                screenCoordExtremes[2] = Math.Max(screenCoordExtremes[2], bounds.Right);
                screenCoordExtremes[3] = Math.Max(screenCoordExtremes[3], bounds.Bottom);
            }
        }

        public byte[] GetMouseCoord()
        {
            Point pt = new Point();
            GetCursorPos(ref pt);
            PointScaler(ref pt);
            return new byte[] { (byte)pt.X, (byte)pt.Y };
        }

        public byte[] ScaledBounds()
        {
            Point pt = new Point(screenCoordExtremes[2], screenCoordExtremes[3]);
            PointScaler(ref pt);
            return new byte[] { (byte)pt.X, (byte)pt.Y };
        }
        
        public void PointScaler(ref Point pt)
        {
            //Scale coords so that they fit in a byte
            //Offset coordinates so that they start at 0
            pt.X -= screenCoordExtremes[0];
            pt.Y -= screenCoordExtremes[1];

            //Scale coordinates to end at 255
            int scaledCoordMax = Math.Max((Math.Abs(screenCoordExtremes[0]) + screenCoordExtremes[2]), (Math.Abs(screenCoordExtremes[1]) + screenCoordExtremes[3]));
            float scalar = 255F / scaledCoordMax;
            pt.X = (int)(scalar * pt.X);
            pt.Y = (int)(scalar * pt.Y);

            //Make sure coords are in the range of a byte(0-255)
            if (pt.X > 255) pt.X = 255;
            if (pt.Y > 255) pt.Y = 255;
            if (pt.X < 0) pt.X = 0;
            if (pt.Y < 0) pt.Y = 0;
        }
    }
}
