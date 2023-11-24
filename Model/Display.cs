using System.Drawing;
using NAudio.Wave;
using WpfVisualizer.Util;

namespace WpfVisualizer
{
    internal class Display
    {
        private Bitmap b;
        private ISampleProvider provider;

        public Size Size {
            get => b.Size;
            set {
                b = new Bitmap(width: value.Width, height: value.Height);
            }
        }

        public Display(ISampleProvider provider)
        {
            b = new Bitmap(width: 800, height: 600);
            this.provider = provider;
        }

        private int frameCount = 0;
        private Lab baseColour = new Lab { L = 0, a = 1, b = 1 };

        public Bitmap RenderFrame()
        {
            frameCount++;
            var point = new Point { X = 0, Y = Size.Height };
            var samples = new float[Size.Width - 1];
            provider.Read(samples, 0, samples.Length);
            using (var g = Graphics.FromImage(b))
            {
                for (var x = 1; x < Size.Width - 1; x++)
                {
                    var sample = samples[x];
                    baseColour.L = frameCount % 100;
                    var colour = new SolidBrush(Oklab.toColor(baseColour));
                    var pen = new Pen(colour);
                    var next = new Point { X = x, Y = (int)(sample / float.MaxValue * Size.Height / 2) };
                    g.DrawLine(pen, point, next);
                    point = next;
                }
                g.DrawRectangle(new Pen(Oklab.toColor(baseColour)), new Rectangle(x: 0, y: 0, width: Size.Width, height: Size.Height));
            }
            return b;
        }

    }
}
