using System.Numerics;
using System.Runtime.CompilerServices;

namespace com.azi.Image
{
    public class VectorPixel
    {
        private readonly int _limit;
        private readonly VectorMap _map;
        private int _index;
        public VectorPixel(VectorMap map, int x, int y, int limit)
        {
            _map = map;
            _limit = limit;
            _index = y * map.Width + x;
        }

        public VectorPixel(VectorMap map)
            : this(map, 0, 0, map.Width*map.Height)
        {
        }

        public VectorPixel(VectorMap map, int x, int y)
            : this(map, x, y, map.Width*map.Height)
        {
        }

        public Vector3 Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return _map.Rgb[_index]; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)] set { _map.Rgb[_index] = value; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MoveNext()
        {
            _index ++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetAndMoveNext(Vector3 val)
        {
            _map.Rgb[_index] = val;
            MoveNext();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNextAndCheck()
        {
            _index ++;
            return _index < _limit;
        }

    }
}
