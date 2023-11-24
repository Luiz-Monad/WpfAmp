using System.Drawing;
using System.Windows;
using WpfVisualizer.ViewModel;

namespace WpfVisualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var viewModel = DataContext as Waveform;
            if (viewModel != null)
            {
                viewModel.UpdateCanvas += UpdateCanvasWithBitmap;
            }
        }

        private void UpdateCanvasWithBitmap(Bitmap bmp)
        {
            Canvas.RenderBitmap(bmp);
        }

    }
}
