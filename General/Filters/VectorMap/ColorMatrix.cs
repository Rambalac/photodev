﻿using System.Numerics;

namespace com.azi.Filters.VectorMapFilters
{
    public class ColorMatrixFilter : VectorToVectorFilter
    {
        private Matrix4x4 _matrix = Matrix4x4.Identity;

        public Matrix4x4 Matrix
        {
            get { return _matrix; }
            set { _matrix = value; }
        }

        public override void ProcessColor(ref Vector3 input, ref Vector3 output)
        {
            output = Vector3.Transform(input, _matrix);
        }
    }
}