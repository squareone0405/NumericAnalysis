using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;


namespace NumericalAnalysis1
{
    public partial class FaceWraper : Form
    {
        public FaceWraper()
        {
            InitializeComponent();
            imageProcesser = new ImageProcesser();
            isKeyPointShown = false;
        }

        //选择源图像按键
        private void btnLoadSourceImage_Clicked(object sender, EventArgs e)
        {
            isLoadSource = true;
            openFileDialogImage.ShowDialog();
        }

        //选择目标图像按键
        private void btnLoadTargetImage_Click(object sender, EventArgs e)
        {
            isLoadSource = false;
            openFileDialogImage.ShowDialog();
        }

        //保存输出图像按键
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (pictureBoxResult.Image == null)
            {
                MessageBox.Show(this, "当前变换结果为空，无法保存", "Warning");
                return;
            }
            saveFileDialog.ShowDialog();
        }

        //图像文件选择确认
        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            Bitmap bmp = new Bitmap(openFileDialogImage.FileName);
            if (isLoadSource)
            {
                imageProcesser.setSourceImage(bmp);
                sourceImgPath = openFileDialogImage.FileName;
                pictureBoxSource.Image = bmp;
            }
            else
            {
                imageProcesser.setTargetImage(bmp);
                targetImgPath = openFileDialogImage.FileName;
                pictureBoxTarget.Image = bmp;
            }
        }

        //保存输出图像
        private void saveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            pictureBoxResult.Image.Save(saveFileDialog.FileName);
        }

        //变形
        private void buttonDeformation_Click(object sender, EventArgs e)
        {
            if (pictureBoxSource.Image == null || pictureBoxTarget.Image == null)
            {
                MessageBox.Show("请先加载待变换的图片", "Warning");
                return;
            }
            if (imageProcesser.isSourceKeyPointNull() || imageProcesser.isTargetKeyPointNull())
            {
                MessageBox.Show("请先加载待变换的图片的关键点", "Warning");
                return;
            }
            if (radioButtonNearest.Checked)
            {
                imageProcesser.setInterpolationMethod(InterpolatationMethod.Nearest);
            }
            else if (radioButtonBilinear.Checked)
            {
                imageProcesser.setInterpolationMethod(InterpolatationMethod.Bilinear);
            }
            else if (radioButtonBicubic.Checked)
            {
                imageProcesser.setInterpolationMethod(InterpolatationMethod.Bicubic);
            }
            else
            {
                MessageBox.Show("请选择插值方法", "Warning");
                return;
            }
            if (radioButtonBSpline.Checked)
            {
                imageProcesser.setDeformationMethod(DeformationMethod.BSpline);
            }
            else if (radioButtonTPS.Checked)
            {
                imageProcesser.setDeformationMethod(DeformationMethod.TPS);
            }
            else
            {
                MessageBox.Show("请选择变形方法", "Warning");
                return;
            }
            pictureBoxResult.Image = imageProcesser.getDeformatedImage();
        }

        //载入源关键点按钮
        private void btnLoadSourceKeyPoint_Click(object sender, EventArgs e)
        {
            isLoadSource = true;
            openFileDialogKeyPoint.ShowDialog();
        }

        //载入目标关键点按钮
        private void btnLoadTargetKeyPoint_Click(object sender, EventArgs e)
        {
            isLoadSource = false;
            openFileDialogKeyPoint.ShowDialog();
        }

        //载入关键点
        private void openFileDialogKeyPoint_FileOk(object sender, CancelEventArgs e)
        {
            string[] lines = File.ReadAllLines(openFileDialogKeyPoint.FileName);
            List<PointF> pointList = new List<PointF>(68);
            for (int i = 0; i < lines.Length; ++i)
            {
                try
                {
                    string[] bits = lines[i].Split(' ');
                    PointF point = new PointF();
                    point.X = float.Parse(bits[0]);
                    point.Y = float.Parse(bits[1]);
                    pointList.Add(point);
                }
                catch
                {
                    MessageBox.Show("关键点文件格式有误，无法解析", "Error");
                    return;
                }
            }
            if (isLoadSource)
            {
                if (!imageProcesser.setSourceKeyPoint(pointList))
                {
                    MessageBox.Show("关键点超出图片范围", "Warning");
                    return;
                }
            }
            else
            {
                if (!imageProcesser.setTargetKeyPoint(pointList))
                {
                    MessageBox.Show("关键点超出图片范围", "Warning");
                    return;
                }
            }
        }

        //以原尺寸显示图片按钮
        private void btnShowResult_Click(object sender, EventArgs e)
        {
            if(pictureBoxResult.Image == null)
            {
                MessageBox.Show("当前结果为空，无法显示", "Warning");
                return;
            }
            ImageForm imageForm = new ImageForm();
            imageForm.setImage((Bitmap)pictureBoxResult.Image);
            imageForm.Show();
        }

        //计算关键点按钮
        private void btnComputeKeyPoint_Click(object sender, EventArgs e)
        {
            if(sourceImgPath == null && targetImgPath == null)
            {
                MessageBox.Show("尚未加载图片", "Warning");
                return;
            }
            if(sourceImgPath != null)
                imageProcesser.computeKeyPoint(sourceImgPath, true);
            if (targetImgPath != null)
                imageProcesser.computeKeyPoint(targetImgPath, false);
        }

        //显示/隐藏关键点按钮
        private void buttonShowKeyPoint_Click(object sender, EventArgs e)
        {
            if (sourceImgPath == null && targetImgPath == null)
            {
                MessageBox.Show("尚未加载图片", "Warning");
                return;
            }
            if (!isKeyPointShown)
            {
                if (pictureBoxSource.Image != null && !imageProcesser.isSourceKeyPointNull())
                    pictureBoxSource.Image = imageProcesser.getSourceImageWithKeypoint();
                if (pictureBoxTarget.Image != null && !imageProcesser.isTargetKeyPointNull())
                    pictureBoxTarget.Image = imageProcesser.getTargetImageWithKeypoint();
                if (pictureBoxResult.Image != null)
                    pictureBoxResult.Image = imageProcesser.getResultImageWithKeypoint();
                Button btn = (Button)sender;
                btn.Text = "隐藏关键点";
                isKeyPointShown = true;
            }
            else
            {
                if (pictureBoxSource.Image != null)
                    pictureBoxSource.Image = imageProcesser.getSourceBmp();
                if (pictureBoxTarget.Image != null)
                    pictureBoxTarget.Image = imageProcesser.getTargerBmp();
                if (pictureBoxResult.Image != null)
                    pictureBoxResult.Image = imageProcesser.getResultBmp();
                Button btn = (Button)sender;
                btn.Text = "显示关键点";
                isKeyPointShown = false;
            }            
        }

    }
}
