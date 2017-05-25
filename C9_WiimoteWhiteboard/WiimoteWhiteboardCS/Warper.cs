namespace WiimoteWhiteboard
{
    public class Warper
    {
        private float[] _srcX = new float[4];
        private float[] _srcY = new float[4];
        private float[] _dstX = new float[4];
        private float[] _dstY = new float[4];
        private float[] _srcMat = new float[16];
        private float[] _dstMat = new float[16];
        private float[] _warpMat = new float[16];
        private bool _dirty;

        public Warper()
        {
            SetIdentity();
        }

public void SetIdentity()
{
    SetSource(0.0f, 0.0f,
              1.0f, 0.0f,
              1.0f, 1.0f,
              0.0f, 1.0f);

    SetDestination(0.0f, 0.0f,
                   1.0f, 0.0f,
                   1.0f, 1.0f,
                   0.0f, 1.0f);
    ComputeWarp();
}

        public void SetSource(	float x0,
                        float y0, 
                        float x1, 
                        float y1, 
                        float x2,
                        float y2, 
                        float x3, 
                        float y3)
        {
            _srcX[0] = x0;
            _srcY[0] = y0;
            _srcX[1] = x1;
            _srcY[1] = y1;
            _srcX[2] = x2;
            _srcY[2] = y2;
            _srcX[3] = x3;
            _srcY[3] = y3;
            _dirty = true;
        }

        public void SetDestination(float x0,
                                   float y0, 
                                   float x1, 
                                   float y1, 
                                   float x2,
                                   float y2, 
                                   float x3, 
                                   float y3)
        {
            _dstX[0] = x0;
            _dstY[0] = y0;
            _dstX[1] = x1;
            _dstY[1] = y1;
            _dstX[2] = x2;
            _dstY[2] = y2;
            _dstX[3] = x3;
            _dstY[3] = y3;
            _dirty = true;
        }

        public void ComputeWarp()
        {
            ComputeSquareToQuad(_dstX[0], _dstY[0],
                                _dstX[1], _dstY[1],
                                _dstX[2], _dstY[2],
                                _dstX[3], _dstY[3],
                                _dstMat);
            ComputeQuadToSquare(_srcX[0], _srcY[0],
                                _srcX[1], _srcY[1],
                                _srcX[2], _srcY[2],
                                _srcX[3], _srcY[3],
                                _srcMat);
            MultiplyMatrices(_srcMat, _dstMat, _warpMat);
            _dirty = false;
        }

        public void MultiplyMatrices(float[] srcMat, float[] dstMat, float[] resMat)
        {
            // [ a b ]   [w x]   [(aw + by) (ax + bz)] 
            // [ c d ] * [y z] = [(cw + dy) (cz + dz)]
            for (int row = 0; row < 4; row++)
            {
                int rowIndex = row * 4;
                for (int col = 0; col < 4; col++)
                {
					resMat[rowIndex + col] = (srcMat[rowIndex    ] * dstMat[col     ] +
											  srcMat[rowIndex + 1] * dstMat[col +  4] +
											  srcMat[rowIndex + 2] * dstMat[col +  8] +
											  srcMat[rowIndex + 3] * dstMat[col + 12]);
                }
            }
        }

        public void ComputeSquareToQuad(float x0,
                                        float y0, 
                                        float x1, 
                                        float y1, 
                                        float x2,
                                        float y2, 
                                        float x3, 
                                        float y3,
                                        float[] mat)
        {

            float dx1 = x1 - x2, 	dy1 = y1 - y2;
            float dx2 = x3 - x2, 	dy2 = y3 - y2;
            float sx = x0 - x1 + x2 - x3;
            float sy = y0 - y1 + y2 - y3;
            float g = (sx  * dy2 - dx2 * sy)  / (dx1 * dy2 - dx2 * dy1);
            float h = (dx1 * sy  - sx  * dy1) / (dx1 * dy2 - dx2 * dy1);
            float a = x1 - x0 + g * x1;
            float b = x3 - x0 + h * x3;
            float c = x0;
            float d = y1 - y0 + g * y1;
            float e = y3 - y0 + h * y3;
            float f = y0;

            mat[ 0] = a;	mat[ 1] = d;	mat[ 2] = 0;	mat[ 3] = g;
            mat[ 4] = b;	mat[ 5] = e;	mat[ 6] = 0;	mat[ 7] = h;
            mat[ 8] = 0;	mat[ 9] = 0;	mat[10] = 1;	mat[11] = 0;
            mat[12] = c;	mat[13] = f;	mat[14] = 0;	mat[15] = 1;
        }

        public void ComputeQuadToSquare(float x0,
                                        float y0, 
                                        float x1, 
                                        float y1, 
                                        float x2,
                                        float y2, 
                                        float x3, 
                                        float y3,
                                        float[] mat)
        {
            ComputeSquareToQuad(x0, y0, x1, y1, x2, y2, x3, y3, mat);

            // invert through adjoint
            // ignore 3rd column
            float a = mat[ 0],	d = mat[ 1],		g = mat[ 3];
            float b = mat[ 4],	e = mat[ 5],		h = mat[ 7];
            /* ignore 3rd row */
            float c = mat[12],	f = mat[13];

            float a1 =     e - f * h;
            float b1 = c * h - b;
            float c1 = b * f - c * e;
            float d1 = f * g - d;
            float e1 =     a - c * g;
            float f1 = c * d - a * f;
            float g1 = d * h - e * g;
            float h1 = b * g - a * h;
            float i1 = a * e - b * d;

            float idet = 1.0f / (a * a1           + b * d1           + c * g1);

            mat[ 0] = a1 * idet;	mat[ 1] = d1 * idet;	mat[ 2] = 0;	mat[ 3] = g1 * idet;
            mat[ 4] = b1 * idet;	mat[ 5] = e1 * idet;	mat[ 6] = 0;	mat[ 7] = h1 * idet;
            mat[ 8] = 0        ;	mat[ 9] = 0        ;	mat[10] = 1;	mat[11] = 0;
            mat[12] = c1 * idet;	mat[13] = f1 * idet;	mat[14] = 0;	mat[15] = i1 * idet;
        }

        public float[] GetWarpMatrix()
        {
            return _warpMat;
        }

        public void Warp(float srcX, float srcY, ref float dstX, ref float dstY)
        {
            if (_dirty)
                ComputeWarp();

            Warp(_warpMat, srcX, srcY, ref dstX, ref dstY);
        }

        public static void Warp(float[] mat, float srcX, float srcY, ref float dstX, ref float dstY)
        {
            float[] result = new float[4];
            float z = 0;
            result[0] = srcX * mat[0] + srcY*mat[4] + z*mat[ 8] + mat[12];
            result[1] = srcX * mat[1] + srcY*mat[5] + z*mat[ 9] + mat[13];
            result[2] = srcX * mat[2] + srcY*mat[6] + z*mat[10] + mat[14];
            result[3] = srcX * mat[3] + srcY*mat[7] + z*mat[11] + mat[15];        
            dstX = result[0]/result[3];
            dstY = result[1]/result[3];
        }
    }
}
