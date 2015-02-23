using System;
using System.Collections;
using System.Collections.Generic;

namespace com.azi.image
{
    public class Color<T> : IEnumerator<Color<T>> where T : IComparable<T>
    {
        private readonly int _limit;
        private readonly ColorMap<T> _map;
        private int _index;

        internal Color(ColorMap<T> map, int x, int y)
        {
            _map = map;
            _index = (y * map.Width + x) * 3;
            _limit = _map.Height * _map.Width * 3;
        }

        internal Color(ColorMap<T> map, int y)
        {
            _map = map;
            _index = y * map.Width * 3;
            _limit = _index + _map.Width * 3;
        }

        public T R
        {
            get { return _map.Rgb[_index + 0]; }
            set { _map.Rgb[_index + 0] = value; }
        }

        public T G
        {
            get { return _map.Rgb[_index + 1]; }
            set { _map.Rgb[_index + 1] = value; }
        }

        public T B
        {
            get { return _map.Rgb[_index + 2]; }
            set { _map.Rgb[_index + 2] = value; }
        }

        public T this[int i]
        {
            get
            {
                //if (i > 2) throw new ArgumentException("Should be less than 3");
                return _map.Rgb[_index + i];
            }
            set
            {
                //if (i > 2) throw new ArgumentException("Should be less than 3");
                _map.Rgb[_index + i] = value;
            }
        }

        public bool MoveNext()
        {
            _index += 3;
            return _index < _limit;
        }

        public bool SetAndMoveNext(T r, T g, T b)
        {
            _map.Rgb[_index + 0] = r;
            _map.Rgb[_index + 1] = g;
            _map.Rgb[_index + 2] = b;
            _index += 3;
            return _index < _limit;
        }

        public void Reset()
        {
            _index = 0;
        }

        public Color<T> Current
        {
            get { return this; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public void Dispose()
        {
        }

        public IEnumerator<Color<T>> GetEnumerator()
        {
            return this;
        }

        public T MaxComponent()
        {
            return (R.CompareTo(G) > 0)
                ? (R.CompareTo(B) > 0) ? R : B
                : (G.CompareTo(B) > 0) ? G : B;
        }
    }
}