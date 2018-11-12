using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.IO;
using DlibDotNet;
using System.Drawing.Imaging;

namespace NumericalAnalysis1
{
    enum DeformationMethod
    {
        BSpline, TPS
    }

    enum InterpolatationMethod
    {
        Nearest, Bilinear, Bicubic
    }

    enum BitmapType
    {
        Source, Target, Result
    }

    class ImageProcesser
    {
        public ImageProcesser()
        {
            transformMat = new List<List<PointF>>();
        }

        public void setSourceImage(Bitmap bmp)
        {
            sourceBmp = bmp;
        }

        public void setTargetImage(Bitmap bmp)
        {
            targetBmp = bmp;
        }

        public bool setSourceKeyPoint(List<PointF> pointList)
        {
            if (sourceBmp != null)
                if (!checkKeyPointRange(sourceBmp.Size, pointList))
                    return false;
            sourceKeyPoint = pointList;
            return true;
        }

        public bool setTargetKeyPoint(List<PointF> pointList)
        {
            if (targetBmp != null)
                if (!checkKeyPointRange(targetBmp.Size, pointList))
                    return false;
            targetKeyPoint = pointList;
            return true;
        }

        public List<PointF> getSourceKeyPoint()
        {
            return sourceKeyPoint;
        }

        public List<PointF> getTargetKeyPoint()
        {
            return targetKeyPoint;
        }

        public Image getSourceBmp()
        {
            return sourceBmp;
        }

        public Image getTargerBmp()
        {
            return targetBmp;
        }

        public Image getResultBmp()
        {
            return resultBmp;
        }

        public bool isSourceKeyPointNull()
        {
            return sourceKeyPoint == null;
        }

        public bool isTargetKeyPointNull()
        {
            return targetKeyPoint == null;
        }

        public void setDeformationMethod(DeformationMethod m)
        {
            dMethod = m;
        }

        public void setInterpolationMethod(InterpolatationMethod m)
        {
            iMethod = m;
        }

        public Bitmap getDeformatedImage()
        {
            if(!checkKeyPointRange(sourceBmp.Size, sourceKeyPoint)||!checkKeyPointRange(targetBmp.Size, targetKeyPoint))
            {
                MessageBox.Show("关键点超出图片范围", "Warning");
                return null;
            }
            transformMat.Clear();
            for (int i = 0; i < sourceBmp.Width; ++i)
            {
                transformMat.Add(new List<PointF>());
                for (int j = 0; j < sourceBmp.Height; ++j)
                {
                    //transformMat[i].Add(new PointF((double)200.7+i/(double)5.0, (double)200.7+j/(double)5.0));
                    transformMat[i].Add(new PointF((float)i, (float)j));
                }
            }
            System.Drawing.Rectangle boundingBox = new System.Drawing.Rectangle(0, 0, sourceBmp.Width, sourceBmp.Height);
            switch (dMethod)
            {
                case DeformationMethod.BSpline:
                    boundingBox = BSpline();
                    break;
                case DeformationMethod.TPS:
                    thinPlateSpline();
                    break;
                default:
                    break;
            }
            switch (iMethod)
            {
                case InterpolatationMethod.Nearest:
                    return nearestInterpolate(boundingBox);
                case InterpolatationMethod.Bilinear:
                    return bilinearInterpolate(boundingBox);
                case InterpolatationMethod.Bicubic:
                    return bicubicInterpolate(boundingBox);
                default:
                    return sourceBmp;
            }
        }

        /*public void drawKeyPoint(Graphics g, Size imgSize, Size boxSize, List<PointF> keyPoint, Color color)
        {
            RectangleF bounds = new RectangleF(0, 0, imgSize.Width, imgSize.Height);
            SolidBrush brush = new SolidBrush(color);
            double kx = (double)(boxSize.Width - 4) / imgSize.Width;//-4是考虑pictureBox边框存在一定宽度
            double ky = (double)(boxSize.Height - 4) / imgSize.Height;
            double xBias = 0, yBias = 0;
            double ratio;
            if (kx < ky)
            {
                ratio = kx;
                yBias = (boxSize.Height - 4 - kx * imgSize.Height) / 2;
            }
            else
            {
                ratio = ky;
                xBias = (boxSize.Width - 4 - ky * imgSize.Width) / 2;
            }
            foreach (PointF p in keyPoint)
                g.FillRectangle(brush, (float)(p.X * ratio - 1 + xBias), (float)(p.Y * ratio - 1 + yBias), 2, 2);
            g.Dispose();
        }*/

        public Bitmap getSourceImageWithKeypoint()
        {
            if (checkKeyPointRange(sourceBmp.Size, sourceKeyPoint))
            {
                drawKeyPoint(BitmapType.Source, Color.Red);
                return sourceBmpWithKeyPoint;
            }
            MessageBox.Show("关键点超出图片范围", "Warning");
            return sourceBmp;
        }

        public Bitmap getTargetImageWithKeypoint()
        {
            if(checkKeyPointRange(targetBmp.Size, targetKeyPoint))
            {
                drawKeyPoint(BitmapType.Target, Color.Blue);
                return targetBmpWithKeyPoint;
            }
            MessageBox.Show("关键点超出图片范围", "Warning");
            return targetBmp;
        }

        public Bitmap getResultImageWithKeypoint()
        {
            if(checkKeyPointRange(sourceBmp.Size, alignedKeyPoint))
            {
                drawKeyPoint(BitmapType.Result, Color.Blue);
                return resultBmpWithKeyPoint;
            }
            MessageBox.Show("关键点超出图片范围", "Warning");
            return resultBmp;
        }

        public void drawKeyPoint(BitmapType type, Color color)
        {
            int x, y;
            switch (type)
            {
                case BitmapType.Source:
                    sourceBmpWithKeyPoint = (Bitmap)sourceBmp.Clone();
                    foreach (PointF p in sourceKeyPoint)
                    {
                        x = (int)p.X;
                        y = (int)p.Y;
                        for(int i = -1; i < 2; ++i)
                        {
                            for(int j = -1; j < 2; ++j)
                            {
                                if (x + i >= 0 && x + i < sourceBmp.Width && y + j >= 0 && y + j < sourceBmp.Height)
                                    sourceBmpWithKeyPoint.SetPixel(x + i, y + j, color);
                            }
                        }
                    }
                    break;
                case BitmapType.Target:
                    targetBmpWithKeyPoint = (Bitmap)targetBmp.Clone();
                    foreach (PointF p in targetKeyPoint)
                    {
                        x = (int)p.X;
                        y = (int)p.Y;
                        for (int i = -1; i < 2; ++i)
                        {
                            for (int j = -1; j < 2; ++j)
                            {
                                if (x + i >= 0 && x + i < targetBmp.Width && y + j >= 0 && y + j < targetBmp.Height)
                                    targetBmpWithKeyPoint.SetPixel(x + i, y + j, color);
                            }
                        }
                    }
                    break;
                case BitmapType.Result:
                    resultBmpWithKeyPoint = (Bitmap)resultBmp.Clone();
                    foreach (PointF p in alignedKeyPoint)
                    {
                        x = (int)p.X;
                        y = (int)p.Y;
                        for (int i = -1; i < 2; ++i)
                        {
                            for (int j = -1; j < 2; ++j)
                            {
                                if (x + i >= 0 && x + i < resultBmp.Width && y + j >= 0 && y + j < resultBmp.Height)
                                    resultBmpWithKeyPoint.SetPixel(x + i, y + j, color);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public Bitmap nearestInterpolate(System.Drawing.Rectangle boundingBox)
        {
            int x, y;
            int width = boundingBox.Width;
            int height = boundingBox.Height;           
            int originX = boundingBox.Left;
            int originY = boundingBox.Top;
            int imgWidth = sourceBmp.Width;
            resultBmp = (Bitmap)sourceBmp.Clone();
            Bitmap sourceBmpCopy = (Bitmap)sourceBmp.Clone();
            BitmapData resultBmpData = resultBmp.LockBits(boundingBox, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            BitmapData sourceBmpData = sourceBmpCopy.LockBits(new System.Drawing.Rectangle(0, 0, sourceBmpCopy.Width, sourceBmpCopy.Height), 
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            byte[] sourceData = new byte[Math.Abs(sourceBmpData.Stride * sourceBmpData.Height)];
            System.Runtime.InteropServices.Marshal.Copy(sourceBmpData.Scan0, sourceData, 0, sourceData.Length);
            int stride = resultBmpData.Stride;
            int offset = stride - imgWidth * 3;
            IntPtr iptr = resultBmpData.Scan0;
            for (int i = 0; i < height; i++)
            {
                byte[] pixelValues = new byte[width * 3];
                int posScan = 0;
                for (int j = 0; j < width; j++)
                {
                    x = (int)(transformMat[j + originX][i + originY].X + 0.5);
                    y = (int)(transformMat[j + originX][i + originY].Y + 0.5);
                    checkPixelRange(ref x, ref y, sourceBmpCopy.Size);
                    pixelValues[posScan++] = sourceData[(y * stride + x * 3)];
                    pixelValues[posScan++] = sourceData[(y * stride + x * 3) + 1];
                    pixelValues[posScan++] = sourceData[(y * stride + x * 3) + 2];
                }
                System.Runtime.InteropServices.Marshal.Copy(pixelValues, 0, iptr, width * 3);
                iptr = iptr + stride;
            }
            resultBmp.UnlockBits(resultBmpData);
            sourceBmpCopy.UnlockBits(sourceBmpData);
            /*int r, g, b;
            resultBmp = (Bitmap)sourceBmp.Clone();
            for (int i = 0; i < sourceBmp.Size.Width; ++i)
            {
                for (int j = 0; j < sourceBmp.Size.Height; ++j)
                {
                    x = (int)(transformMat[i][j].X + 0.5);
                    y = (int)(transformMat[i][j].Y + 0.5);
                    checkPixelRange(ref x, ref y, sourceBmp.Size);
                    r = sourceBmp.GetPixel(x, y).R;
                    g = sourceBmp.GetPixel(x, y).G;
                    b = sourceBmp.GetPixel(x, y).B;
                    resultBmp.SetPixel(i, j, Color.FromArgb(r, g, b));
                }
            }*/
            return resultBmp;
        }

        public Bitmap bilinearInterpolate(System.Drawing.Rectangle boundingBox)
        {   
            double x, y;
            byte r, g, b;
            int xFloor, yFloor;
            double deltax0, deltay0, deltax1, deltay1;
            int width = boundingBox.Width;
            int height = boundingBox.Height;
            int originX = boundingBox.Left;
            int originY = boundingBox.Top;
            int imgWidth = sourceBmp.Width;
            int imgHeight = sourceBmp.Height;
            resultBmp = (Bitmap)sourceBmp.Clone();
            Bitmap sourceBmpCopy = (Bitmap)sourceBmp.Clone();
            BitmapData resultBmpData = resultBmp.LockBits(boundingBox, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            BitmapData sourceBmpData = sourceBmpCopy.LockBits(new System.Drawing.Rectangle(0, 0, sourceBmpCopy.Width, sourceBmpCopy.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            byte[] sourceData = new byte[Math.Abs(sourceBmpData.Stride * sourceBmpData.Height)];
            System.Runtime.InteropServices.Marshal.Copy(sourceBmpData.Scan0, sourceData, 0, sourceData.Length);
            int stride = resultBmpData.Stride;
            IntPtr iptr = resultBmpData.Scan0;
            for (int i = 0; i < height; i++)
            {
                byte[] pixelValues = new byte[width * 3];
                int posScan = 0;
                for (int j = 0; j < width; j++)
                {
                    x = transformMat[j + originX][i + originY].X;
                    y = transformMat[j + originX][i + originY].Y;
                    xFloor = (int)x;
                    yFloor = (int)y;
                    deltax0 = x - xFloor;
                    deltax1 = 1 - deltax0;
                    deltay0 = y - yFloor;
                    deltay1 = 1 - deltay0;
                    checkPixelRange(ref xFloor, ref yFloor, sourceBmp.Size);
                    if (xFloor >= imgWidth - 1)//处理插值点位于边缘的情况
                        xFloor = imgWidth - 2;
                    if (yFloor >= imgHeight - 1)
                        yFloor = imgHeight - 2;
                    r = (byte)(sourceData[(yFloor * stride + xFloor * 3)] * deltax1 * deltay1
                        + sourceData[(yFloor * stride + (xFloor + 1) * 3)] * deltax0 * deltay1
                        + sourceData[((yFloor + 1) * stride + xFloor * 3)] * deltax1 * deltay0
                        + sourceData[((yFloor + 1) * stride + (xFloor + 1) * 3)] * deltax0 * deltay0 + 0.5);
                    g = (byte)(sourceData[(yFloor * stride + xFloor * 3) + 1] * deltax1 * deltay1
                        + sourceData[(yFloor * stride + (xFloor + 1) * 3) + 1] * deltax0 * deltay1
                        + sourceData[((yFloor + 1) * stride + xFloor * 3) + 1] * deltax1 * deltay0
                        + sourceData[((yFloor + 1) * stride + (xFloor + 1) * 3) + 1] * deltax0 * deltay0 + 0.5);
                    b = (byte)(sourceData[(yFloor * stride + xFloor * 3) + 2] * deltax1 * deltay1
                        + sourceData[(yFloor * stride + (xFloor + 1) * 3) + 2] * deltax0 * deltay1
                        + sourceData[((yFloor + 1) * stride + xFloor * 3) + 2] * deltax1 * deltay0
                        + sourceData[((yFloor + 1) * stride + (xFloor + 1) * 3) + 2] * deltax0 * deltay0 + 0.5);
                    pixelValues[posScan++] = r; 
                    pixelValues[posScan++] = g; 
                    pixelValues[posScan++] = b;
                }
                System.Runtime.InteropServices.Marshal.Copy(pixelValues, 0, iptr, width * 3);
                iptr = iptr + stride;
            }
            resultBmp.UnlockBits(resultBmpData);
            sourceBmpCopy.UnlockBits(sourceBmpData);
            /*int r, g, b;
            double x, y;
            int xFloor, yFloor;
            double deltax0, deltay0, deltax1, deltay1;
            resultBmp = new Bitmap(sourceBmp.Width, sourceBmp.Height);
            for (int i = 0; i < sourceBmp.Size.Width; ++i)
            {
                for (int j = 0; j < sourceBmp.Size.Height; ++j)
                {
                    x = transformMat[i][j].X;
                    y = transformMat[i][j].Y;
                    xFloor = (int)Math.Floor(x);
                    yFloor = (int)Math.Floor(y);
                    deltax0 = x - xFloor;
                    deltax1 = 1 - deltax0;
                    deltay0 = y - yFloor;
                    deltay1 = 1 - deltay0;
                    checkPixelRange(ref xFloor, ref yFloor, sourceBmp.Size);
                    if (xFloor >= sourceBmp.Size.Width - 1)//处理插值点位于边缘的情况
                        xFloor = sourceBmp.Size.Width - 2;
                    if (yFloor >= sourceBmp.Size.Height - 1)
                        yFloor = sourceBmp.Size.Height - 2;
                    r = (int)(sourceBmp.GetPixel(xFloor, yFloor).R * deltax1 * deltay1
                        + sourceBmp.GetPixel(xFloor + 1, yFloor).R * deltax0 * deltay1
                        + sourceBmp.GetPixel(xFloor, yFloor + 1).R * deltax1 * deltay0
                        + sourceBmp.GetPixel(xFloor + 1, yFloor + 1).R * deltax0 * deltay0);
                    g = (int)(sourceBmp.GetPixel(xFloor, yFloor).G * deltax1 * deltay1
                        + sourceBmp.GetPixel(xFloor + 1, yFloor).G * deltax0 * deltay1
                        + sourceBmp.GetPixel(xFloor, yFloor + 1).G * deltax1 * deltay0
                        + sourceBmp.GetPixel(xFloor + 1, yFloor + 1).G * deltax0 * deltay0);
                    b = (int)(sourceBmp.GetPixel(xFloor, yFloor).B * deltax1 * deltay1
                        + sourceBmp.GetPixel(xFloor + 1, yFloor).B * deltax0 * deltay1
                        + sourceBmp.GetPixel(xFloor, yFloor + 1).B * deltax1 * deltay0
                        + sourceBmp.GetPixel(xFloor + 1, yFloor + 1).B * deltax0 * deltay0);
                    resultBmp.SetPixel(i, j, Color.FromArgb(r, g, b));
                }
            }*/
            return resultBmp;
        }

        public Bitmap bicubicInterpolate(System.Drawing.Rectangle boundingBox)
        {
            double x, y;
            byte r, g, b;
            int rint, gint, bint;
            double rdouble, gdouble, bdouble;
            int xFloor, yFloor;
            double deltaX, deltaY;
            double[] matA = new double[4];
            double[] matC = new double[4];
            int width = boundingBox.Width;
            int height = boundingBox.Height;
            int originX = boundingBox.Left;
            int originY = boundingBox.Top;
            int imgWidth = sourceBmp.Width;
            int imgHeight = sourceBmp.Height;
            resultBmp = (Bitmap)sourceBmp.Clone();
            Bitmap sourceBmpCopy = (Bitmap)sourceBmp.Clone();
            BitmapData resultBmpData = resultBmp.LockBits(boundingBox, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            BitmapData sourceBmpData = sourceBmpCopy.LockBits(new System.Drawing.Rectangle(0, 0, sourceBmpCopy.Width, sourceBmpCopy.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            byte[] sourceData = new byte[Math.Abs(sourceBmpData.Stride * sourceBmpData.Height)];
            System.Runtime.InteropServices.Marshal.Copy(sourceBmpData.Scan0, sourceData, 0, sourceData.Length);
            int stride = resultBmpData.Stride;
            IntPtr iptr = resultBmpData.Scan0;
            for (int i = 0; i < height; i++)
            {
                byte[] pixelValues = new byte[width * 3];
                int posScan = 0;
                for (int j = 0; j < width; j++)
                {
                    x = transformMat[j + originX][i + originY].X;
                    y = transformMat[j + originX][i + originY].Y;
                    xFloor = (int)x;
                    yFloor = (int)y;
                    deltaX = x - xFloor;
                    deltaY = y - yFloor;
                    matA[0] = approxFunction(deltaX + 1);
                    matA[1] = approxFunction(deltaX);
                    matA[2] = approxFunction(deltaX - 1);
                    matA[3] = approxFunction(deltaX - 2);
                    matC[0] = approxFunction(deltaY + 1);
                    matC[1] = approxFunction(deltaY);
                    matC[2] = approxFunction(deltaY - 1);
                    matC[3] = approxFunction(deltaY - 2);
                    rdouble = 0;
                    gdouble = 0;
                    bdouble = 0;
                    int xPadding, yPadding;//处理插值点位于边缘情况
                    for (int k = 0; k < 4; ++k)
                    {
                        for (int l = 0; l < 4; ++l)
                        {
                            xPadding = xFloor - 1 + k;
                            xPadding = Math.Max(0, xPadding);
                            xPadding = Math.Min(xPadding, sourceBmp.Size.Width - 1);
                            yPadding = yFloor - 1 + l;
                            yPadding = Math.Max(0, yPadding);
                            yPadding = Math.Min(yPadding, sourceBmp.Size.Height - 1);
                            rdouble += (matA[k] * matC[l] * sourceData[yPadding * stride + xPadding * 3]);
                            gdouble += (matA[k] * matC[l] * sourceData[yPadding * stride + xPadding * 3 + 1]);
                            bdouble += (matA[k] * matC[l] * sourceData[yPadding * stride + xPadding * 3 + 2]);
                        }
                    }
                    rint = (int)(rdouble + 0.5);
                    gint = (int)(gdouble + 0.5);
                    bint = (int)(bdouble + 0.5);
                    checkColorRange(ref rint, ref gint, ref bint);
                    r = (byte)rint;
                    g = (byte)gint;
                    b = (byte)bint;
                    pixelValues[posScan++] = r;
                    pixelValues[posScan++] = g;
                    pixelValues[posScan++] = b;
                }
                System.Runtime.InteropServices.Marshal.Copy(pixelValues, 0, iptr, width * 3);
                iptr = iptr + stride;
            }
            resultBmp.UnlockBits(resultBmpData);
            sourceBmpCopy.UnlockBits(sourceBmpData);
            /*int r, g, b;
            double rdouble, gdouble, bdouble;
            double x, y;
            int xFloor, yFloor;
            double deltaX, deltaY;
            double[] matA = new double[4];
            double[] matC = new double[4];
            resultBmp = new Bitmap(sourceBmp.Width, sourceBmp.Height);
            for (int i = 0; i < sourceBmp.Size.Width; ++i)
            {
                for (int j = 0; j < sourceBmp.Size.Height; ++j)
                {
                    x = transformMat[i][j].X;
                    y = transformMat[i][j].Y;
                    xFloor = (int)Math.Floor(x);
                    yFloor = (int)Math.Floor(y);
                    deltaX = x - xFloor;
                    deltaY = y - yFloor;
                    matA[0] = approxFunction(deltaX + 1);
                    matA[1] = approxFunction(deltaX);
                    matA[2] = approxFunction(deltaX - 1);
                    matA[3] = approxFunction(deltaX - 2);
                    matC[0] = approxFunction(deltaY + 1);
                    matC[1] = approxFunction(deltaY);
                    matC[2] = approxFunction(deltaY - 1);
                    matC[3] = approxFunction(deltaY - 2);
                    rdouble = 0;
                    gdouble = 0;
                    bdouble = 0;
                    int xPadding, yPadding;//处理插值点位于边缘情况
                    for (int k = 0; k < 4; ++k)
                    {
                        for (int l = 0; l < 4; ++l)
                        {
                            xPadding = xFloor - 1 + k;
                            xPadding = Math.Max(0, xPadding);
                            xPadding = Math.Min(xPadding, sourceBmp.Size.Width - 1);
                            yPadding = yFloor - 1 + l;
                            yPadding = Math.Max(0, yPadding);
                            yPadding = Math.Min(yPadding, sourceBmp.Size.Height - 1);
                            rdouble += (matA[k] * matC[l] * sourceBmp.GetPixel(xPadding, yPadding).R);
                            gdouble += (matA[k] * matC[l] * sourceBmp.GetPixel(xPadding, yPadding).G);
                            bdouble += (matA[k] * matC[l] * sourceBmp.GetPixel(xPadding, yPadding).B);
                        }
                    }
                    r = (int)rdouble;
                    g = (int)gdouble;
                    b = (int)bdouble;
                    checkColorRange(ref r, ref g, ref b);
                    resultBmp.SetPixel(i, j, Color.FromArgb(r, g, b));
                }
            }*/
            return resultBmp;
        }

        private System.Drawing.Rectangle BSpline()
        {
            alignedKeyPoint = MathTools.alignFace(sourceKeyPoint, targetKeyPoint);
            RectangleF boundingBox = getBoundingBox();
            double learningRate = 0.7;
            int iter = 50;
            int gridNum = (int)(Math.Sqrt(boundingBox.Width * boundingBox.Height) / 50 + 0.5) + 9;//网格尺寸与boundingBox大小有关，参数设置为经验值
            int pointSize = alignedKeyPoint.Count();
            int pointBegin = 17;//关键点中0-16号点为外轮廓，可以选择不进行变换
            int gridSizeX = (int)boundingBox.Size.Width / gridNum + 1;
            int gridSizeY = (int)boundingBox.Size.Height / gridNum + 1;
            List<int>[,] pointToGridIdx = new List<int>[gridSizeX, gridSizeY];
            List<double>[,] pointToGridBase = new List<double>[gridSizeX, gridSizeY];
            double[] pointMovementX = new double[pointSize];
            double[] pointMovementY = new double[pointSize];
            double[] pointDistanceX = new double[pointSize];
            double[] pointDistanceY = new double[pointSize];
            double[,] gridMovementX = new double[gridSizeX, gridSizeY];
            double[,] gridMovementY = new double[gridSizeX, gridSizeY];
            double[,,] baseMat = new double[pointSize, 4, 4];
            int indexX, indexY;
            float tx, ty;
            double moveX, moveY;
            for (int i = pointBegin; i < pointSize; ++i)//使用基函数求系数
            {
                tx = Math.Abs((float)alignedKeyPoint[i].X / gridNum - (int)alignedKeyPoint[i].X / gridNum);
                ty = Math.Abs((float)alignedKeyPoint[i].Y / gridNum - (int)alignedKeyPoint[i].Y / gridNum);
                for (int j = 0; j < 4; ++j)
                {
                    for (int k = 0; k < 4; ++k)
                    {
                        baseMat[i, j, k] = baseFunction(j, tx) * baseFunction(k, ty);
                    }
                }
            }
            for (int i = pointBegin; i < pointSize; ++i)//计算关键点需要移动的距离
            {
                pointDistanceX[i] = sourceKeyPoint[i].X - alignedKeyPoint[i].X;
                pointDistanceY[i] = sourceKeyPoint[i].Y - alignedKeyPoint[i].Y;
            }
            for (int i = 0; i < gridSizeX; ++i)//初始化
            {
                for (int j = 0; j < gridSizeY; ++j)
                {
                    pointToGridIdx[i, j] = new List<int>();
                    pointToGridBase[i, j] = new List<double>();
                }
            }
            for (int i = pointBegin; i < pointSize; ++i)//求解网格点与关键点的对应关系
            {
                indexX = (int)(alignedKeyPoint[i].X - boundingBox.Left) / gridNum;
                indexY = (int)(alignedKeyPoint[i].Y - boundingBox.Top) / gridNum;
                for (int j = -1; j < 3; ++j)
                {
                    for (int k = -1; k < 3; ++k)
                    {
                        if ((indexX + j) >= 0 && (indexX + j) < gridSizeX && (indexY + k) >= 0 && (indexY + k) < gridSizeY)
                        {
                            pointToGridIdx[indexX + j, indexY + k].Add(i);
                            pointToGridBase[indexX + j, indexY + k].Add(baseMat[i, j + 1, k + 1]);
                        }
                    }
                }
            }
            double currError = 0.0;
            double lastError = 0.0;
            for (int iteration = 0; iteration < iter; ++iteration)
            {
                currError = 0.0;
                for (int i = pointBegin; i < pointSize; ++i)//计算关键点在网格移动下的移动量
                {
                    pointMovementX[i] = 0;
                    pointMovementY[i] = 0;
                    indexX = (int)(alignedKeyPoint[i].X - boundingBox.Left) / gridNum;
                    indexY = (int)(alignedKeyPoint[i].Y - boundingBox.Top) / gridNum;
                    for (int j = 0; j < 4; ++j)
                    {
                        for (int k = 0; k < 4; ++k)
                        {
                            int tempX = Math.Min(indexX + j - 1, gridSizeX - 1);
                            tempX = Math.Max(tempX, 0);
                            int tempY = Math.Min(indexY + k - 1, gridSizeY - 1);
                            tempY = Math.Max(tempY, 0);
                            pointMovementX[i] += gridMovementX[tempX, tempY] * baseMat[i, j, k];
                            pointMovementY[i] += gridMovementY[tempX, tempY] * baseMat[i, j, k];
                        }
                    }
                }
                double maxError = 0.0;
                int idx = 0;
                for(int i = pointBegin; i < pointSize; ++i)
                {
                    currError += (pointDistanceX[i] - pointMovementX[i]) * (pointDistanceX[i] - pointMovementX[i])
                               + (pointDistanceY[i] - pointMovementY[i]) * (pointDistanceY[i] - pointMovementY[i]);
                    if((pointDistanceX[i] - pointMovementX[i]) * (pointDistanceX[i] - pointMovementX[i])
                               + (pointDistanceY[i] - pointMovementY[i]) * (pointDistanceY[i] - pointMovementY[i]) > maxError)
                    {
                        idx = i;
                    }
                    maxError = Math.Max(maxError, (pointDistanceX[i] - pointMovementX[i]) * (pointDistanceX[i] - pointMovementX[i])
                               + (pointDistanceY[i] - pointMovementY[i]) * (pointDistanceY[i] - pointMovementY[i]));
                }
                currError /= (pointSize - pointBegin);
                double test37 = (pointDistanceX[37] - pointMovementX[37]) * (pointDistanceX[37] - pointMovementX[37])
                    + (pointDistanceY[37] - pointMovementY[37]) * (pointDistanceY[37] - pointMovementY[37]);
                double test38 = (pointDistanceX[38] - pointMovementX[38]) * (pointDistanceX[38] - pointMovementX[38])
                   + (pointDistanceY[38] - pointMovementY[38]) * (pointDistanceY[38] - pointMovementY[38]);
                if ((lastError - currError) > 0 && (lastError - currError) / currError < 0.005)
                    break;
                lastError = currError;               
                for (int i = 0; i < gridSizeX; ++i)//计算网格点移动
                {
                    for (int j = 0; j < gridSizeY; ++j)
                    {
                        moveX = 0.0;
                        moveY = 0.0;
                        for (int k = 0; k < pointToGridIdx[i, j].Count; ++k)
                        {
                            int pointIdx = pointToGridIdx[i, j][k];
                            moveX += (pointDistanceX[pointIdx] - pointMovementX[pointIdx]) * pointToGridBase[i, j][k];
                            moveY += (pointDistanceY[pointIdx] - pointMovementY[pointIdx]) * pointToGridBase[i, j][k];
                        }
                        if (pointToGridIdx[i, j].Count > 0 && (Math.Abs(moveX) > 0.0000000001 || Math.Abs(moveY) > 0.0000000001))
                        {
                            gridMovementX[i, j] += moveX * learningRate;
                            gridMovementY[i, j] += moveY * learningRate;
                        }
                    }
                }
                /*MathTools.saveAsCsv(gridMovementX, "./gridmoveX" + iteration + ".csv");
                MathTools.saveAsCsv(gridMovementY, "./gridmoveY" + iteration + ".csv");
                MathTools.saveAsCsv(pointMovementX, "./pointmoveX" + iteration + ".csv");
                MathTools.saveAsCsv(pointMovementY, "./pointmoveY" + iteration + ".csv");
                FileStream fsNew = new FileStream("../../../picture/new" + iteration + ".txt", FileMode.Create);
                StreamWriter swNew = new StreamWriter(fsNew);
                for (int i = 0; i < pointSize; ++i)
                {
                    swNew.Write(alignedKeyPoint[i].X + pointMovementX[i]);
                    swNew.Write(' ');
                    swNew.Write(alignedKeyPoint[i].Y + pointMovementY[i]);
                    swNew.WriteLine();
                }
                swNew.Close();
                fsNew.Close();*/
            }

            /*FileStream fsx = new FileStream("./sx.csv", FileMode.Create);
            StreamWriter swx = new StreamWriter(fsx);
            FileStream fsy = new FileStream("./sy.csv", FileMode.Create);
            StreamWriter swy = new StreamWriter(fsy);*/

            MathTools.saveAsCsv(gridMovementX, "./gridmoveX.csv");
            MathTools.saveAsCsv(gridMovementY, "./gridmoveY.csv");

            int left = sourceBmp.Width;
            int right = 0;
            int top = sourceBmp.Height;
            int buttom = 0;
            for (int i = (int)boundingBox.Left; i < (int)boundingBox.Right; ++i)
            {
                for (int j = (int)boundingBox.Top; j < (int)boundingBox.Bottom; ++j)
                {
                    moveX = 0.0;
                    moveY = 0.0;
                    indexX = (int)(i - boundingBox.Left) / gridNum;
                    indexY = (int)(j - boundingBox.Top) / gridNum;
                    tx = Math.Abs((float)(i - boundingBox.Left) / gridNum - indexX);
                    ty = Math.Abs((float)(j - boundingBox.Top) / gridNum - indexY);
                    for (int k = 0; k < 4; ++k)
                    {
                        for (int l = 0; l < 4; ++l)
                        {
                            int tempX = Math.Min(indexX - 1 + k, gridSizeX - 1);
                            tempX = Math.Max(tempX, 0);
                            int tempY = Math.Min(indexY - 1 + l, gridSizeY - 1);
                            tempY = Math.Max(tempY, 0);
                            moveX += gridMovementX[tempX, tempY] * baseFunction(k, tx) * baseFunction(l, ty);
                            moveY += gridMovementY[tempX, tempY] * baseFunction(k, tx) * baseFunction(l, ty);
                        }
                    }
                    if(moveX > 0.0000000001 || moveY > 0.0000000001)
                    {
                        left = Math.Min(i, left);
                        top = Math.Min(j, top);
                        right = Math.Max(i, right);
                        buttom = Math.Max(j, buttom);
                    }
                    /*swx.Write(sx);
                    swx.Write(',');
                    swy.Write(sy);
                    swy.Write(',');*/
                    transformMat[i][j] = new PointF(i + (float)moveX, j + (float)moveY);
                }
                /*swx.WriteLine();
                swy.WriteLine();*/
            }
            /*swx.Close();
            fsx.Close();
            swy.Close();
            fsy.Close();*/
            //return new System.Drawing.Rectangle(left, top, right - left, buttom - top);
            return new System.Drawing.Rectangle((int)boundingBox.X, (int)boundingBox.Y, (int)boundingBox.Width, (int)boundingBox.Height);
        }

        private void thinPlateSpline()
        {
            alignedKeyPoint = MathTools.alignFace(sourceKeyPoint, targetKeyPoint);
            double[,] A = new double[71, 71];
            for (int i = 0; i < 68; ++i)
            {
                for (int j = 0; j < 68; ++j)
                {
                    if (i == j)
                        A[i, j] = 0;
                    double u = U_r(alignedKeyPoint[i], alignedKeyPoint[j]);
                    A[i, j] = u;
                    A[j, i] = u;
                }
            }
            for (int i = 0; i < 68; ++i)
            {
                A[i, 68] = 1;
                A[68, i] = 1;
                A[i, 69] = alignedKeyPoint[i].X;
                A[69, i] = alignedKeyPoint[i].X;
                A[i, 70] = alignedKeyPoint[i].Y;
                A[70, i] = alignedKeyPoint[i].Y;
            }
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    A[68 + i, 68 + j] = 0;
                }
            }
            double[,] Y = new double[71, 2];
            for (int i = 0; i < 68; ++i)
            {
                Y[i, 0] = sourceKeyPoint[i].X;
                Y[i, 1] = sourceKeyPoint[i].Y;
            }
            for (int i = 68; i < 71; ++i)
            {
                Y[i, 0] = 0;
                Y[i, 1] = 0;
            }
            double[,] matW = new double[68, 2];
            double[] a1 = new double[2];
            double[] ax = new double[2];
            double[] ay = new double[2];
            double[,] testL = new double[2, 2];
            //MathTools.solveWithQR(A, Y, matW, a1, ax, ay);
            MathTools.solveWithLU(A, Y, matW, a1, ax, ay);
            double[] pos = new double[2];
            for (int i = 0; i < sourceBmp.Width; ++i)
            {
                for (int j = 0; j < sourceBmp.Height; ++j)
                {
                    pos[0] = a1[0] + ax[0] * i + ay[0] * j;
                    pos[1] = a1[1] + ax[1] * i + ay[1] * j;
                    double ur;
                    for (int k = 0; k < 68; ++k)
                    {
                        ur = U_r(new PointF(i, j), alignedKeyPoint[k]);
                        pos[0] += ur * matW[k, 0];
                        pos[1] += ur * matW[k, 1];
                    }
                    transformMat[i][j] = new PointF((float)pos[0], (float)pos[1]);
                }
            }
        }

        public void computeKeyPoint(string imgPath, bool isSource)
        {
            using (var detector = Dlib.GetFrontalFaceDetector())
            {
                using (var sp = ShapePredictor.Deserialize("../../../shape_predictor_68_face_landmarks.dat"))
                {
                    using (var img = Dlib.LoadImage<RgbPixel>(imgPath))
                    {
                        Dlib.PyramidUp(img);
                        var dets = detector.Operator(img);
                        if(dets.Length == 0)
                        {
                            MessageBox.Show("图中未检测到人脸", "Warning");
                            return;
                        }
                        else if (dets.Length > 1)
                        {
                            MessageBox.Show("图中检测到多张人脸，取其中一张进行变换", "Warning");
                            return;
                        }
                        var shape = sp.Detect(img, dets[0]);
                        if (isSource)
                        {
                            if (sourceKeyPoint != null)
                                sourceKeyPoint.Clear();
                            else
                                sourceKeyPoint = new List<PointF>(68);
                            for (uint i = 0; i < 68; ++i)
                                sourceKeyPoint.Add(new PointF((float)shape.GetPart(i).X / 2, (float)shape.GetPart(i).Y / 2));
                        }
                        else
                        {
                            if (targetKeyPoint != null)
                                targetKeyPoint.Clear();
                            else
                                targetKeyPoint = new List<PointF>(68);
                            for (uint i = 0; i < 68; ++i)
                                targetKeyPoint.Add(new PointF((float)shape.GetPart(i).X / 2, (float)shape.GetPart(i).Y / 2));
                        }
                    }
                }
            }
        }

        //TPS中的U函数
        private double U_r(PointF p1, PointF p2)
        {
            if (p1.Equals(p2))
                return 0;
            double r2 = (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);
            return r2 * (double)Math.Log(r2);
        }

        //双三次插值逼近函数
        private double approxFunction(double x)
        {
            double xAbs = Math.Abs(x);
            if (xAbs < 1)
            {
                return 1 - 2 * xAbs * xAbs + xAbs * xAbs * xAbs;
            }
            else if (xAbs < 2)
            {
                return 4 - 8 * xAbs + 5 * xAbs * xAbs - xAbs * xAbs * xAbs;
            }
            return 0;
        }

        //B样条变形基函数
        private double baseFunction(int n, double t)
        {
            double G;
            switch (n)
            {
                case 0:
                    G = (-t * t * t + 3 * t * t - 3 * t + 1) / 6.0;
                    break;
                case 1:
                    G = (3 * t * t * t - 6 * t * t + 4) / 6.0;
                    break;
                case 2:
                    G = (-3 * t * t * t + 3 * t * t + 3 * t + 1) / 6.0;
                    break;
                case 3:
                    G = t * t * t / 6.0;
                    break;
                default:
                    G = 0.0;
                    break;
            }
            return G;
        }

        //检查关键点是否在图片范围内
        private bool checkKeyPointRange(Size imgSize, List<PointF> keyPoint)
        {
            RectangleF bounds = new RectangleF(0, 0, imgSize.Width, imgSize.Height);   
            foreach (PointF p in keyPoint)
            {
                if (!bounds.Contains(p.X, p.Y))
                    return false;   
            }
            return true;
        }

        //双三次插值可能出现RGB范围在[0:255]之外，需要修正范围
        private void checkColorRange(ref int r, ref int g, ref int b)
        {
            r = Math.Max(r, 0);
            g = Math.Max(g, 0);
            b = Math.Max(b, 0);
            r = Math.Min(r, 255);
            g = Math.Min(g, 255);
            b = Math.Min(b, 255);
        }

        //对超出图像范围的点进行修正
        private void checkPixelRange(ref int x, ref int y, Size size)
        {
            x = Math.Max(x, 0);
            y = Math.Max(y, 0);
            x = Math.Min(x, size.Width - 1);
            y = Math.Min(y, size.Height - 1);
        }

        //获取面部的BoundingBox，减少计算范围
        private RectangleF getBoundingBox()
        {
            float xMin = 999999, yMin = 999999;
            float xMax = 0, yMax = 0;
            foreach (PointF p in sourceKeyPoint)
            {
                xMin = Math.Min(xMin, p.X);
                yMin = Math.Min(yMin, p.Y);
                xMax = Math.Max(xMax, p.X);
                yMax = Math.Max(yMax, p.Y);
            }
            float width = xMax - xMin;
            float height = yMax - yMin;
            xMin -= width / 3;
            xMax += width / 3;
            yMin -= height / 3;
            yMax += height / 3;
            xMin = Math.Max(xMin, 0);
            yMin = Math.Max(yMin, 0);
            xMax = Math.Min(xMax, sourceBmp.Width - 1);
            yMax = Math.Min(yMax, sourceBmp.Height - 1);
            return new RectangleF(xMin, yMin, xMax - xMin, yMax - yMin);
        }

        private Bitmap sourceBmp;
        private Bitmap targetBmp;
        private Bitmap resultBmp;
        private Bitmap sourceBmpWithKeyPoint;
        private Bitmap targetBmpWithKeyPoint;
        private Bitmap resultBmpWithKeyPoint;
        private List<PointF> sourceKeyPoint;
        private List<PointF> targetKeyPoint;
        private List<PointF> alignedKeyPoint;
        private List<List<PointF>> transformMat;  
        private DeformationMethod dMethod;
        private InterpolatationMethod iMethod;
    }
}
