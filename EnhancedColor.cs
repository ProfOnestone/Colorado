using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Colorado
{
    public class EnhancedColor :INotifyPropertyChanged
    {
        private System.Windows.Media.Color _WinColor;
        public System.Windows.Media.Color WinColor
        { 
            get 
            { 
                return _WinColor; 
            } 
            set 
            { 
                _WinColor = value;
                this.BrushColor = new SolidColorBrush(value);
                NotifyPropertyChanged("RedHex");
                NotifyPropertyChanged("GreenHex");
                NotifyPropertyChanged("BlueHex");
                NotifyPropertyChanged("HexColor");
            } 
        }

        public string RedHex { get { return _WinColor.R.ToString(); } }
        public string GreenHex { get { return _WinColor.G.ToString(); } }
        public string BlueHex { get { return _WinColor.B.ToString(); } }
        public string HexColor { get { return "#" + _WinColor.R.ToString("X2") + _WinColor.G.ToString("X2") + _WinColor.B.ToString("X2"); } }

        private System.Windows.Media.SolidColorBrush _BrushColor;
        public System.Windows.Media.SolidColorBrush BrushColor { get { return _BrushColor; } set { _BrushColor = value; NotifyPropertyChanged();  } }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
