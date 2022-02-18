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
using System.Windows.Input;
using CefSharp;
using CefSharp.WinForms;

namespace QuickSearch
{
    public partial class BrowserWindow : Form
    {
        Point moveOffset;
        ChromiumWebBrowser chrome;
        LowLevelKeyboardListener listener;
        BrowserController cont;

        public BrowserWindow(ChromiumWebBrowser chrome, BrowserController cont)
        {
            InitializeComponent();
            this.chrome = chrome;
            this.cont = cont;
            BrowserPanel.Controls.Add(chrome);
            chrome.Dock = DockStyle.Fill;
            listener = new LowLevelKeyboardListener();
            listener.OnKeyPressed += ListenToKeyboard;
            listener.HookKeyboard();
            TopMost = true;
        }

        public void Exit()
        {
            listener.UnHookKeyboard();
        }

        public void AutoPosition()
        {
            Location = MousePosition;
            Focus();
        }

        private void Close_Click(object sender, EventArgs e)
        {
            cont.SoftClose();
        }

        private void Minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Handlebar_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            moveOffset = e.Location;
            Timer.Enabled = true;
        }

        private void Handlebar_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Timer.Enabled = false;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Location = new Point(MousePosition.X - moveOffset.X, MousePosition.Y - moveOffset.Y);
        }

        private void ListenToKeyboard(object sender, KeyPressedArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.LeftShift) && Keyboard.IsKeyDown(Key.G))
            {
                cont.StartSearching();
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.Q))
            {
                cont.SoftClose();
            }
        }
    }
}
