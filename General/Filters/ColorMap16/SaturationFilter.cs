namespace com.azi.Filters.ColorMap16
{
    public class SaturationFilter : ColorToColorFilter<float, float>
    {
        private float _saturation = 1;

        public float Saturation
        {
            set { _saturation = value; }
            get { return _saturation; }
        }

        public override void ProcessColor(float[] input, int inputOffset, float[] output, int outputOffset)
        {
            float r = input[inputOffset + 0];
            float g = input[inputOffset + 1];
            float b = input[inputOffset + 2];
            float chroma = (r + g + b)/3;
            output[outputOffset + 0] = chroma + (r - chroma)*_saturation;
            output[outputOffset + 1] = chroma + (g - chroma)*_saturation;
            output[outputOffset + 2] = chroma + (b - chroma)*_saturation;
        }
    }
}