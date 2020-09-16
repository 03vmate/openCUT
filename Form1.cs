using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        UInt32[,,] data = new UInt32[255, 255, 3]; //0:Left, 1:Right, 2:Middle
        public Form1()
        {
            InitializeComponent();
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
            mouseHook.LeftButtonDown += new MouseHook.MouseHookCallback(mouseEvent => { mouseClickEvent(0, mouseEvent); });
            mouseHook.RightButtonDown += new MouseHook.MouseHookCallback(mouseEvent => { mouseClickEvent(1, mouseEvent); });
            mouseHook.MiddleButtonDown += new MouseHook.MouseHookCallback(mouseEvent => { mouseClickEvent(2, mouseEvent); });
            mouseHook.Install();
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
    }
}
