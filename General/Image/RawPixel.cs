using System;
using System.Runtime.CompilerServices;

namespace com.azi.Image
{
    public class RawPixel<T> where T : IComparable<T>
    {
        private readonly int _limit;
        private readonly RawMap<T> _map;
        private int _index;

        public RawPixel(RawMap<T> map, int x, int y, int limit)
        {
            _map = map;
            _limit = limit;
            _index = y * map.Width + x;
        }

        public RawPixel(RawMap<T> map)
            : this(map, 0, 0, map.Width * map.Height)
        {
        }

        public RawPixel(RawMap<T> map, int x, int y)
            : this(map, x, y, map.Width * map.Height)
        {
        }

        public T Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return _map.Raw[_index]; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { _map.Raw[_index] = value; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetRel(int x, int y)
        {
            return _map.Raw[_index + x + y * _map.Width];
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T SetRel(int x, int y, T val)
        {
            return _map.Raw[_index + x + y * _map.Width] = val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MoveNext()
        {
            _index += 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetAndMoveNext(T val)
        {
            _map.Raw[_index] = val;
            MoveNext();
        }
    }
}