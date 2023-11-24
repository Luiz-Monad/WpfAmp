using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfVisualizer.Util
{
    internal struct Lab
    {
        public float L;
        public float a;
        public float b;
    }

    internal class Oklab
    {
        public static Color toColor(Lab lab)
        {
            float l_ = lab.L + 0.3963377774f * lab.a + 0.2158037573f * lab.b;
            float m_ = lab.L - 0.1055613458f * lab.a - 0.0638541728f * lab.b;
            float s_ = lab.L - 0.0894841775f * lab.a - 1.2914855480f * lab.b;

            float l = l_ * l_ * l_;
            float m = m_ * m_ * m_;
            float s = s_ * s_ * s_;

            return Color.FromArgb(
                alpha: 1,
                red: (byte)((+4.0767416621f * l - 3.3077115913f * m + 0.2309699292f * s) * 255),
                green: (byte)((-1.2684380046f * l + 2.6097574011f * m - 0.3413193965f * s) * 255),
                blue: (byte)((-0.0041960863f * l - 0.7034186147f * m + 1.7076147010f * s) * 255)
            );
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute();
        }

        public void Execute(object parameter)
        {
            execute();
        }
    }


}
