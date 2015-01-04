using com.azi.image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace General.Debayer
{
    class MatrixDebayer
    {
        enum ColorComponent : int
        {
            Red = 0,
            Green = 1,
            Blue = 2
        }
        int colorMapSize;
        float[][,] colorMap;
        public float[][,] ColorMap {get{
            return colorMap;
        } set{
            if (value.GetUpperBound(0) != 3) 
                throw new ArgumentException("ColorMap should be exctly three 2-dimensions arrays");
            if (value[0].GetUpperBound(0) != value[0].GetUpperBound(1)) 
                throw new ArgumentException("ColorMap should have should be square");
            var size = value[0].GetUpperBound(0);
            if (size % 2 != 1) throw new ArgumentException("ColorMap size should be even");
            for (int i = 1; i < 3; i++)
                if (value[0].GetUpperBound(0) != size || value[0].GetUpperBound(1) != size)
                    throw new ArgumentException("ColorMap sizes should be same");

            colorMapSize = size;
            colorMap = value;
        }}

        int mapheight;
        int mapwith;
        ColorComponent[,] componentsMap;
        public ColorComponent[,] ComponentsMap
        {
            get { return componentsMap; }
            set
            {
                componentsMap = value;
                mapheight = componentsMap.GetUpperBound(0) + 1;
                mapwith = componentsMap.GetUpperBound(1) + 1;
            }
        }

        public Color16[,] debayer(RawImageFile file)
        {
            Color16[,] res = new Color16[file.Height, file.Width];

            for (int y = 0; y < file.Height; y++)
                for (int x = 0; x < file.Width; x++)
                {
                    ColorComponent component = componentsMap[y % mapheight, x % mapwith];
                    
                }
        }
    }
}
