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

        thecolor test = new thecolor();

        Bitmap screenPixel = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        public MainWindow()
        {
             
            InitializeComponent();

            System.Windows.Media.Color mc = getcolorfromposition();

            test.actualcolor = new SolidColorBrush(mc);
            test.valuered = mc.R.ToString();
            test.valuegreen = mc.G.ToString();
            test.valueblue = mc.B.ToString();

            this.DataContext = test;

            System.Windows.Threading.DispatcherTimer mytimer= new System.Windows.Threading.DispatcherTimer();
            mytimer.Tick += Mytimer_Tick;
            mytimer.Interval = new TimeSpan(0, 0, 0, 0,200);
            mytimer.Start();

            load_colorlist();

        }

        private void Mytimer_Tick(object sender, EventArgs e)
        {
            if (!TheList.IsMouseOver)
            {
                System.Windows.Media.Color mcc = getcolorfromposition();

                test.actualcolor = new SolidColorBrush(mcc);
                test.valuered = mcc.R.ToString();
                test.valuegreen = mcc.G.ToString();
                test.valueblue = mcc.B.ToString();

            }

        }

        private System.Windows.Media.Color getcolorfromposition()
        {
            System.Drawing.Point cursor = new System.Drawing.Point();
            GetCursorPos(ref cursor);
            System.Drawing.Color dc = GetColorAt(cursor);
            System.Windows.Media.Color mc = System.Windows.Media.Color.FromArgb(dc.A, dc.R, dc.G, dc.B);

            return mc;
        }

        private System.Drawing.Color GetColorAt(System.Drawing.Point location)
        {
            using (Graphics gdest = Graphics.FromImage(screenPixel))
            {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }

            return screenPixel.GetPixel(0, 0);
        }

        private void lbl_color_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {

        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.S)
            {
                string hex_code = test.actualcolor.ToString();
                StreamWriter mysw = File.AppendText(@"D:\Visual Studio\C#\Colorado\Colorado\knowncolours.txt");
                mysw.WriteLine(hex_code);
                mysw.Dispose();
            }
            load_colorlist();
            

        }

        private void load_colorlist()
        {
            List<string> colorhexes = new List<string>();
            string[] listofcolours = File.ReadAllLines(@"D:\Visual Studio\C#\Colorado\Colorado\knowncolours.txt",System.Text.Encoding.UTF8);
            foreach (var item in listofcolours)
            {
                
                colorhexes.Add(item.ToString());
            }



            TheList.ItemsSource = colorhexes;



        }

        private void TheList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            test.actualcolor = (SolidColorBrush)(new BrushConverter().ConvertFrom((sender as System.Windows.Controls.ListBox).SelectedItem));
            test.valuered = test.actualcolor.Color.R.ToString();
            test.valuegreen = test.actualcolor.Color.B.ToString(); ;
            test.valueblue = test.actualcolor.Color.G.ToString(); ;


        }
    }





    //-------------------------------------------------------Color-Klasse-------------------------------------------------------------//

    public class thecolor : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private SolidColorBrush _actualcolor;
        public SolidColorBrush actualcolor
        {
            get { return _actualcolor; }          
            set {
                _actualcolor = value;
                OnPropertyChanged("actualcolor");
            }
        }

        private string _valuered;
        public string valuered
        {
            get { return _valuered; }
            set
            {
                _valuered = value;
                OnPropertyChanged("valuered");
            }
        }

        private string _valuegreen;
        public string valuegreen
        {
            get { return _valuegreen; }
            set
            {
                _valuegreen = value;
                OnPropertyChanged("valuegreen");
            }
        }

        private string _valueblue;
        public string valueblue
        {
            get { return _valueblue; }
            set
            {
                _valueblue = value;
                OnPropertyChanged("valueblue");
            }
        }




        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }





  

}
