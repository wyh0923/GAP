using System.Drawing;
using Size2F = SharpDX.Size2F;
using Size2 = SharpDX.Size2;

namespace Stas.GA {
    public static class SpriteHelper {
        public static readonly Size2F MapIconsSize = new Size2F(14, 18);
        //public static readonly Size2F MyMapIcons = new Size2F(7, 8);
        public static RectangleF GetUV(MapIconsIndex index) {
            return GetUV((int)index, MapIconsSize);
        }

        public static RectangleF GetUV(int index, Size2F size) {
            if (index % (int)size.Width == 0) {
                return new RectangleF((size.Width - 1) / size.Width, ((int)(index / size.Width) - 1) / size.Height, 1 / size.Width,
                    1 / size.Height);
            }

            return new RectangleF((index % size.Width - 1) / size.Width, index / (int)size.Width / size.Height, 1 / size.Width,
                1 / size.Height);
        }

        public static RectangleF GetUV(Size2 index, Size2F size) {
            return new RectangleF((index.Width - 1) / size.Width, (index.Height - 1) / size.Height, 1 / size.Width, 1 / size.Height);
        }

        public static RectangleF GetUV(int x, int y, float width, float height) {
            return new RectangleF((x - 1) / width, (y - 1) / height, 1 / width, 1 / height);
        }
    }
   
}
