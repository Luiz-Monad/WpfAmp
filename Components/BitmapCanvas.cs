using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System;

namespace WpfVisualizer.Components
{
    public class BitmapCanvas : Canvas
    {
        private Drawing drawing;
        private WriteableBitmap writableBitmap;

        public BitmapCanvas()
        {
            writableBitmap = new WriteableBitmap(800, 600, 96, 96, PixelFormats.Bgra32, null);
            this.Background = new ImageBrush(writableBitmap);
        }

        public void RenderBitmap(System.Drawing.Bitmap bmp)
        {
            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                bmp.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            writableBitmap.Lock();
            bitmapSource.CopyPixels(
                new System.Windows.Int32Rect(0, 0, bmp.Width, bmp.Height),
                writableBitmap.BackBuffer,
                bmp.Width * bmp.Height * 4,
                bmp.Width * 4);
            writableBitmap.AddDirtyRect(new System.Windows.Int32Rect(0, 0, bmp.Width, bmp.Height));
            writableBitmap.Unlock();
        }

        protected override void OnRender(DrawingContext dc)
        {
            //dc.DrawDrawing(drawing);
            base.OnRender(dc);
        }
    }

}
