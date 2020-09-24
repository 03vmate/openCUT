using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenCUT
{
    public partial class Form1 : Form
    {
        MousePosition mousePosition = new MousePosition(Screen.AllScreens);
        MouseHook mouseHook = new MouseHook();

        Graphics graphics;

        UInt32[,,] data;
        public Form1()
        {
            InitializeComponent();
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);

            data = new UInt32[mousePosition.ScaledBounds()[0], mousePosition.ScaledBounds()[1], 3]; //0:Left, 1:Right, 2:Middle
            mouseHook.LeftButtonDown += new MouseHook.MouseHookCallback(mouseEvent => { mouseClickEvent(0, mouseEvent); });
            mouseHook.RightButtonDown += new MouseHook.MouseHookCallback(mouseEvent => { mouseClickEvent(1, mouseEvent); });
            mouseHook.MiddleButtonDown += new MouseHook.MouseHookCallback(mouseEvent => { mouseClickEvent(2, mouseEvent); });
            mouseHook.Install();

            graphics = CreateGraphics();
        }

        private void drawData(byte dim, byte mul = 2)
        {
            graphics.Clear(Color.White);
            Pen basec = new Pen(Color.Black, 1);
            float mult = 255F / maxDataValue(dim);
            graphics.DrawRectangle(basec, 0, 0, data.GetLength(0) * mul+2, data.GetLength(1) * mul+2);
            for (int x = 0; x < data.GetLength(0); x++)
            {
                for (int y = 0; y < data.GetLength(1); y++)
                {   
                    if(data[x, y, dim] != 0)
                    {
                        SolidBrush color = new SolidBrush(Color.FromArgb((byte)(mult * data[x, y, dim]), 0, 0, 0));
                        graphics.FillRectangle(color, x * 2, y * 2, 2, 2);
                    }
                }
            }
        }

        private UInt32 maxDataValue(byte dim)
        {
            UInt32 max = 0;
            for (int x = 0; x < data.GetLength(0); x++)
            {
                for (int y = 0; y < data.GetLength(1); y++)
                {
                    if (data[x, y, dim] > max) max = data[x, y, dim];
                }
            }
            return max;
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            mouseHook.LeftButtonDown -= new MouseHook.MouseHookCallback(mouseEvent => { mouseClickEvent(0, mouseEvent); });
            mouseHook.RightButtonDown -= new MouseHook.MouseHookCallback(mouseEvent => { mouseClickEvent(1, mouseEvent); });
            mouseHook.MiddleButtonDown -= new MouseHook.MouseHookCallback(mouseEvent => { mouseClickEvent(2, mouseEvent); });
            mouseHook.Uninstall();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void mouseClickEvent(byte id, MouseHook.MSLLHOOKSTRUCT mouseEvent)
        {
            byte[] coord = mousePosition.GetMouseCoord();
            data[coord[0], coord[1], id]++;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            drawData(0);
        }
    }
}
