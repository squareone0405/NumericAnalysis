using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalAnalysis1
{
    class MathTools
    {
        //人脸对齐
        public static List<PointF> alignFace(List<PointF> source, List<PointF> target)
        {
            int matchLen = 27;//0-26号点为外轮廓+眉毛
            int pointLen = source.Count();
            List<PointF> aligned = new List<PointF>(pointLen);
            double[,] X = new double[matchLen, 3];
            double[,] Y = new double[matchLen, 2];
            double[,] transform = new double[3, 2];
            for (int i = 0; i < matchLen; ++i)
            {
                X[i, 0] = target[i].X;
                X[i, 1] = target[i].Y;
                X[i, 2] = 1;
                Y[i, 0] = source[i].X;
                Y[i, 1] = source[i].Y;
            }
            transform = solveLeastSquare(X, Y);         
            for (int i = 0; i < pointLen; ++i)
            {
                aligned.Add(new PointF((float)(target[i].X * transform[0, 0] + target[i].Y * transform[1, 0] + transform[2, 0]),
                    (float)(target[i].X * transform[0, 1] + target[i].Y * transform[1, 1] + transform[2, 1])));
            }
            return aligned;
        }

        //人脸对齐的naive方法，最小二乘+旋转
        public static List<PointF> alignFaceNaive(List<PointF> source, List<PointF> target)
        {
            int len = source.Count();
            List<PointF> aligned = new List<PointF>(len);
            List<PointF> rotatedTarget = getRotatedTarget(source, target);
            double[] x1 = new double[len];
            double[] y1 = new double[len];
            double[] x2 = new double[len];
            double[] y2 = new double[len];
            for (int i = 0; i < len; ++i)
            {
                x1[i] = rotatedTarget[i].X;
                x2[i] = rotatedTarget[i].Y;
                y1[i] = source[i].X;
                y2[i] = source[i].Y;
            }
            double k1 = 0.0, b1 = 0.0;
            solveLeastSquare(x1, y1, ref k1, ref b1);
            double k2 = 0.0, b2 = 0.0;
            solveLeastSquare(x2, y2, ref k2, ref b2);
            for (int i = 0; i < len; ++i)
            {
                aligned.Add(new PointF((float)(k1 * rotatedTarget[i].X + b1),
                    (float)(k2 * rotatedTarget[i].Y + b2)));
            }
            return aligned;
        }

        //求解最小二乘beta=inv(Xt*X)*Xt*Y
        private static double[,] solveLeastSquare(double[,] X, double[,] Y)
        {
            double[,] beta = new double[3, 2];
            double[,] Xt = transpose(X);
            double[,] XtX = matMul(Xt, X);
            double[,] iXtX = inverseWithLU(XtX);
            double[,] iXtXXt = matMul(iXtX, Xt);
            beta = matMul(iXtXXt, Y);
            return beta;
        }

        //旋转对齐，使用人脸外轮廓的0-7与9-16号点
        private static List<PointF> getRotatedTarget(List<PointF> source, List<PointF> target)
        {
            int len = source.Count();
            List<PointF> rotatedTarget = new List<PointF>(len);
            double angleSource = 0.0, angleTarget = 0.0;
            for (int i = 0; i < 8; ++i)
            {
                angleSource += Math.Atan2((source[16 - i].Y - source[i].Y), (source[16 - i].X - source[i].X));
                angleTarget += Math.Atan2((target[16 - i].Y - target[i].Y), (target[16 - i].X - target[i].X));
            }
            angleSource /= 8;
            angleTarget /= 8;
            float cosReletiveAngle = (float)Math.Cos(angleSource - angleTarget);
            float sinReletiveAngle = (float)Math.Sin(angleSource - angleTarget);
            for (int i = 0; i < len; ++i)
            {
                rotatedTarget.Add(new PointF(cosReletiveAngle * target[i].X - sinReletiveAngle * target[i].Y,
                    sinReletiveAngle * target[i].X + cosReletiveAngle * target[i].Y));
            }
            return rotatedTarget;
        }

        //最小二乘，求解y=kx+b，用于人脸对齐
        private static void solveLeastSquare(double[] x, double[] y, ref double k, ref double b)
        {
            int len = 27; //前27个点为外轮廓，用于对齐
            double xbar = 0.0;
            double ybar = 0.0;
            double xybar = 0.0;
            double x2bar = 0.0;
            for (int i = 0; i < len; ++i)
            {
                xbar += x[i];
                ybar += y[i];
                xybar += x[i] * y[i];
                x2bar += x[i] * x[i];
            }
            xbar /= len;
            ybar /= len;
            xybar /= len;
            x2bar /= len;
            k = (xybar - xbar * ybar) / (x2bar - xbar * xbar);
            b = ybar - k * xbar;
        }

        //使用LU分解进行矩阵求逆
        private static double[,] inverseWithLU(double[,] mat)
        {
            int len = mat.GetLength(0);
            double[,] L = new double[len, len];
            double[,] U = new double[len, len];
            LUDecomposition(mat, L, U);
            double[,] iL = new double[len, len];
            double[,] iU = new double[len, len];
            for (int i = 0; i < len; ++i)
            {
                iL[i, i] = 1.0 / L[i, i];
            }
            for (int i = 0; i < len; ++i)
            {
                for(int j = i + 1; j < len; ++j)
                {
                    double sum = 0.0;
                    for (int k = 0; k < j; ++k)
                    {
                        sum += L[j, k] * iL[k, i];
                    }
                    iL[j, i] = -sum / L[j, j];
                }               
            }
            double[,] Ut = new double[len, len];
            double[,] iUt = new double[len, len];
            Ut = transpose(U);
            for (int i = 0; i < len; ++i)
            {
                iUt[i, i] = 1.0 / Ut[i, i];
            }
            for (int i = 0; i < len; ++i)
            {
                for (int j = i + 1; j < len; ++j)
                {
                    double sum = 0.0;
                    for (int k = 0; k < j; ++k)
                    {
                        sum += Ut[j, k] * iUt[k, i];
                    }
                    iUt[j, i] = -sum / Ut[j, j];
                }
            }
            iU = transpose(iUt);
            return matMul(iU, iL);
        }

        //使用LU分解解线性方程组
        public static void solveWithLU(double[,] A, double[,] matY, double[,] matW,
            double[] a1, double[] ax, double[] ay)
        {
            int len = A.GetLength(0);
            double swap;
            for (int i = 0; i < len; ++i)
            {
                swap = A[0, i];
                A[0, i] = A[len - 1, i];
                A[len - 1, i] = swap;
            }
            for (int i = 0; i < 2; ++i)
            {
                swap = matY[0, i];
                matY[0, i] = matY[len - 1, i];
                matY[len - 1, i] = swap;
            }
            double[,] L = new double[len, len];
            double[,] U = new double[len, len];
            LUDecomposition(A, L, U);
            double[] UX1 = new double[len];
            double[] UX2 = new double[len];
            for (int i = 0; i < len; ++i)
            {
                double sum1 = 0;
                double sum2 = 0;
                for (int j = 0; j < i; ++j)
                {
                    sum1 += UX1[j] * L[i, j];
                    sum2 += UX2[j] * L[i, j];
                }
                UX1[i] = (matY[i, 0] - sum1) / L[i, i];
                UX2[i] = (matY[i, 1] - sum2) / L[i, i];
            }
            double[] X1 = new double[len];
            double[] X2 = new double[len];
            for (int i = 0; i < len; ++i)
            {
                double sum1 = 0;
                double sum2 = 0;
                for (int j = 0; j < i; ++j)
                {
                    sum1 += X1[len - j - 1] * U[len - i - 1, len - j - 1];
                    sum2 += X2[len - j - 1] * U[len - i - 1, len - j - 1];
                }
                X1[len - i - 1] = (UX1[len - i - 1] - sum1) / U[len - i - 1, len - i - 1];
                X2[len - i - 1] = (UX2[len - i - 1] - sum2) / U[len - i - 1, len - i - 1];
            }
            for (int i = 0; i < len - 3; ++i)
            {
                matW[i, 0] = X1[i];
                matW[i, 1] = X2[i];
            }
            a1[0] = X1[len - 3];
            a1[1] = X2[len - 3];
            ax[0] = X1[len - 2];
            ax[1] = X2[len - 2];
            ay[0] = X1[len - 1];
            ay[1] = X2[len - 1];
        }

        //LU分解
        private static void LUDecomposition(double[,] A, double[,] L, double[,] U)
        {
            int len = A.GetLength(0);
            for (int i = 0; i < len; ++i)
            {
                U[0, i] = A[0, i];
                L[i, 0] = A[i, 0] / U[0, 0];
            }
            for (int r = 1; r < len; ++r)
            {
                for (int i = r; i < len; ++i)
                {
                    double sum = 0.0;
                    for (int k = 0; k < r; k++)
                    {
                        sum += L[r, k] * U[k, i];
                    }
                    U[r, i] = A[r, i] - sum;
                    if (i == r) L[r, r] = 1;
                    else if (r == len) L[len, len] = 1;
                    else
                    {
                        sum = 0.0;
                        for (int k = 0; k < r; k++)
                        {
                            sum += L[i, k] * U[k, r];
                        }
                        L[i, r] = (A[i, r] - sum) / U[r, r];
                    }
                }

            }
        }

        //使用QR分解解线性方程组：LX = Y, L = QR -> QRX = Y -> RX = QtY
        public static void solveWithQR(double[,] A, double[,] matY, double[,] matW,
            double[] a1, double[] ax, double[] ay)
        {
            int len = A.GetLength(0);
            double[,] Q = new double[len, len];
            double[,] R = new double[len, len];
            QRDecomposition(A, Q, R);
            double[] X1 = new double[len];
            double[] X2 = new double[len];
            double[,] Qt = transpose(Q);
            double[,] QtY = matMul(Qt, matY);
            for (int i = 0; i < len; ++i)
            {
                double sum1 = 0;
                double sum2 = 0;
                for (int j = 0; j < i; ++j)
                {
                    sum1 += X1[len - j - 1] * R[len - i - 1, len - j - 1];
                    sum2 += X2[len - j - 1] * R[len - i - 1, len - j - 1];
                }
                X1[len - i - 1] = (QtY[len - i - 1, 0] - sum1) / R[len - i - 1, len - i - 1];
                X2[len - i - 1] = (QtY[len - i - 1, 1] - sum2) / R[len - i - 1, len - i - 1];
            }
            for (int i = 0; i < len - 3; ++i)
            {
                matW[i, 0] = X1[i];
                matW[i, 1] = X2[i];
            }
            a1[0] = X1[len - 3];
            a1[1] = X2[len - 3];
            ax[0] = X1[len - 2];
            ax[1] = X2[len - 2];
            ay[0] = X1[len - 1];
            ay[1] = X2[len - 1];
        }

        //将矩阵以CSV格式保存
        public static void saveAsCsv(double[,] mat, string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            for (int i = 0; i < mat.GetLength(0); ++i)
            {
                for (int j = 0; j < mat.GetLength(1); ++j)
                {
                    sw.Write(mat[i, j]);
                    sw.Write(',');
                }
                sw.WriteLine();
            }
            sw.Close();
            fs.Close();
        }

        //将向量以CSV格式保存
        public static void saveAsCsv(double[] mat, string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            for (int i = 0; i < mat.GetLength(0); ++i)
            {
                sw.Write(mat[i]);
                sw.WriteLine();
            }
            sw.Close();
            fs.Close();
        }

        //利用施密特正交化进行QR分解，Q矩阵仅在前几列能保证精度，后续误差无法接受，最终未使用
        private static void QRDecomposition(double[,] A, double[,] Q, double[,] R)
        {
            int len = A.GetLength(0);
            for (int i = 0; i < len; ++i)
            {
                for (int j = 0; j < i; ++j)
                {
                    R[i, j] = 0;
                }
            }
            double[] vecj = new double[len];
            double[] vecjAfterNorm = new double[len];
            double normVal;
            for (int k = 0; k < len; ++k)
            {
                for (int j = 0; j < len; ++j)
                    vecj[j] = A[j, k];//取出A的第j列
                if (k == 0)
                {
                    normVal = norm(vecj, len);
                    R[0, 0] = normVal;
                    vecjAfterNorm = scalarDev(vecj, normVal, len);
                    for (int j = 0; j < len; ++j)
                        Q[j, k] = vecjAfterNorm[j];
                }
                else
                {
                    double[] sum = new double[len];
                    double[] Qj = new double[len];
                    double[] scaledQj = new double[len];
                    for (int i = 0; i < len; ++i)
                        sum[i] = 0;
                    for (int j = 0; j < len; ++j)
                    {
                        for (int i = 0; i < len; ++i)
                            Qj[i] = Q[i, j];//取出Q的第j列
                        R[j, k] = innerProd(vecj, Qj, len);
                        scaledQj = scalarMul(Qj, R[j, k], len);
                        sum = add(scaledQj, sum, len);//将投影分量求和
                    }
                    double[] uk = new double[len];
                    uk = sub(vecj, sum, len);
                    normVal = norm(uk, len);
                    vecjAfterNorm = scalarDev(uk, normVal, len);
                    R[k, k] = normVal;
                    for (int j = 0; j < len; ++j)
                        Q[j, k] = vecjAfterNorm[j];
                }
            }
        }

        //向量求模
        private static double norm(double[] vec, int len)
        {
            double sum = 0;
            for (int i = 0; i < len; ++i)
                sum += vec[i] * vec[i];
            return (double)Math.Sqrt(sum);
        }

        //向量数乘
        private static double[] scalarMul(double[] vec, double scalar, int len)
        {
            double[] result = new double[len];
            for (int i = 0; i < len; ++i)
            {
                result[i] = vec[i] * scalar;
            }
            return result;
        }

        //向量除标量
        private static double[] scalarDev(double[] vec, double scalar, int len)
        {
            double[] result = new double[len];
            for (int i = 0; i < len; ++i)
            {
                result[i] = vec[i] / scalar;
            }
            return result;
        }

        //向量内积
        private static double innerProd(double[] vec1, double[] vec2, int len)
        {
            double result = 0;
            for (int i = 0; i < len; ++i)
                result += vec1[i] * vec2[i];
            return result;
        }

        //向量加法
        public static double[] add(double[] vec1, double[] vec2, int len)
        {
            double[] result = new double[len];
            for (int i = 0; i < len; ++i)
                result[i] = vec1[i] + vec2[i];
            return result;
        }

        //向量减法
        private static double[] sub(double[] vec1, double[] vec2, int len)
        {
            double[] result = new double[len];
            for (int i = 0; i < len; ++i)
                result[i] = vec1[i] - vec2[i];
            return result;
        }

        //矩阵转置
        private static double[,] transpose(double[,] mat)
        {
            int len1 = mat.GetLength(0);
            int len2 = mat.GetLength(1);
            double[,] result = new double[len2, len1];
            for (int i = 0; i < len2; ++i)
            {
                for (int j = 0; j < len1; ++j)
                {
                    result[i, j] = mat[j, i];
                }
            }
            return result;
        }

        //矩阵相乘
        private static double[,] matMul(double[,] mat1, double[,] mat2)
        {
            int len1 = mat1.GetLength(0);
            int len2 = mat1.GetLength(1);
            int len3 = mat2.GetLength(1);
            double[,] result = new double[len1, len3];
            for (int i = 0; i < len1; ++i)
            {
                for (int j = 0; j < len3; ++j)
                {
                    double sum = 0;
                    for (int k = 0; k < len2; ++k)
                    {
                        sum += mat1[i, k] * mat2[k, j];
                    }
                    result[i, j] = sum;
                }
            }
            return result;
        }
    }
}
