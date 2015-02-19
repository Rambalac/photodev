﻿namespace com.azi.image
{
    public class Color16
    {
        private ColorMap16 map;
        int index;
        internal Color16(ColorMap16 map, int x, int y)
        {
            this.map = map;
            index = (y * map.width + x) * 3;
        }

        public void Next()
        {
            index += 3;
        }
        public void Set(ushort[] values)
        {
            map.rgb[index + 0] = values[0];
            map.rgb[index + 1] = values[1];
            map.rgb[index + 2] = values[2];
        }
        public ushort[] Get()
        {
            return new ushort[3] {
                map.rgb[index + 0],
                map.rgb[index + 1],
                map.rgb[index + 2]
            };
        }

        public ushort this[int i]
        {
            get
            {
                //if (i > 2) throw new ArgumentException("Should be less than 3");
                return map.rgb[index + i];
            }
            set
            {
                //if (i > 2) throw new ArgumentException("Should be less than 3");
                map.rgb[index + i] = value;
            }
        }
    }
    public class ColorMap16
    {
        public readonly int width;
        public readonly int height;
        public readonly ushort[] rgb;

        public ColorMap16(int w, int h)
        {
            width = w;
            height = h;
            rgb = new ushort[w * h * 3];
        }

        public Color16 GetPixel(int x, int y)
        {
            return new Color16(this, x, y);
        }
    }

}
