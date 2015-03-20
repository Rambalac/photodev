using com.azi.Filters.ColorMap16ToRgb8;

namespace com.azi.Filters.ColorMap16
{
    public class ColorMatrixFilter : ColorToColorFilter<float, float>
    {
        public float[,] Matrix
        {
            get { return _matrix; }
            set { _matrix = value; }
        }

        private float[,] _matrix =
        {
            {1f,0f,0f},
            {0f,1f,0f},
            {0f,0f,1f}
        };

        public override void ProcessColor(float[] input, int inputOffset, float[] output, int outputOffset)
        {
            var rr = input[inputOffset + 0];
            var rg = input[inputOffset + 1];
            var rb = input[inputOffset + 2];
            output[outputOffset + 0] = _matrix[0, 0] * rr + _matrix[0, 1] * rg + _matrix[0, 2] * rb;
            output[outputOffset + 1] = _matrix[1, 0] * rr + _matrix[1, 1] * rg + _matrix[1, 2] * rb;
            output[outputOffset + 2] = _matrix[2, 0] * rr + _matrix[2, 1] * rg + _matrix[2, 2] * rb;
        }
    }
}
