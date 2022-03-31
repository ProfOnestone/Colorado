using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using System.ComponentModel;
using System.IO;
using System.Collections.Generic;


namespace Colorado
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref System.Drawing.Point lpPoint);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        EnhancedColor currentColor = new EnhancedColor();
        EnhancedColor snappedColor = new EnhancedColor();

        //creating 1x1 pixel
        Bitmap screenPixel = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        public MainWindow()
        {
            InitializeComponent();

            currentColor.WinColor = GetColorAtCursorPosition();

            this.DataContext = currentColor;

            InitializeColorReadTimer();
        }

        private void InitializeColorReadTimer()
        {
            System.Windows.Threading.DispatcherTimer mytimer = new System.Windows.Threading.DispatcherTimer();
            mytimer.Tick += ColorReadTick;
            mytimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            mytimer.Start();
        }

        private void ColorReadTick(object sender, EventArgs e)
        {
            currentColor.WinColor = GetColorAtCursorPosition();
        }

        private System.Windows.Media.Color GetColorAtCursorPosition()
        {
            System.Drawing.Point cursorPosition = new System.Drawing.Point();
            GetCursorPos(ref cursorPosition);
            using (Graphics gdest = Graphics.FromImage(screenPixel))
            {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, cursorPosition.X, cursorPosition.Y, (int)CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }
            System.Drawing.Color drawingColor = screenPixel.GetPixel(0, 0);
            System.Windows.Media.Color mediaColor = ConvertToMediaColor(drawingColor);
            return mediaColor;
        }
        private System.Windows.Media.Color ConvertToMediaColor(System.Drawing.Color drawingColor)
        {
            System.Windows.Media.Color mc = System.Windows.Media.Color.FromArgb(drawingColor.A, drawingColor.R, drawingColor.G, drawingColor.B);
            return mc;
        }
        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.S)
            {
                snappedColor = currentColor;
                tb_snappedHex.Text = snappedColor.HexColor;
                lbl_snappedBrush.Background = snappedColor.BrushColor;
                tb_snappedRed.Text = snappedColor.RedHex;
                tb_snappedBlue.Text = snappedColor.BlueHex;
                tb_snappedGreen.Text = snappedColor.GreenHex;
            }
        }
    }
}
