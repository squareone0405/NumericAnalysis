namespace NumericalAnalysis1
{
    partial class FaceWraper
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLoadSourceImage = new System.Windows.Forms.Button();
            this.pictureBoxSource = new System.Windows.Forms.PictureBox();
            this.groupBoxImageIO = new System.Windows.Forms.GroupBox();
            this.btnLoadTarget = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.openFileDialogImage = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.pictureBoxTarget = new System.Windows.Forms.PictureBox();
            this.pictureBoxResult = new System.Windows.Forms.PictureBox();
            this.lblSource = new System.Windows.Forms.Label();
            this.lblTarget = new System.Windows.Forms.Label();
            this.lblResult = new System.Windows.Forms.Label();
            this.radioButtonNearest = new System.Windows.Forms.RadioButton();
            this.radioButtonBilinear = new System.Windows.Forms.RadioButton();
            this.radioButtonBicubic = new System.Windows.Forms.RadioButton();
            this.interpolationOptions = new System.Windows.Forms.GroupBox();
            this.buttonDeformation = new System.Windows.Forms.Button();
            this.btnShowResult = new System.Windows.Forms.Button();
            this.groupBoxKeyPoint = new System.Windows.Forms.GroupBox();
            this.buttonShowKeyPoint = new System.Windows.Forms.Button();
            this.btnLoadTargetKeyPoint = new System.Windows.Forms.Button();
            this.btnLoadSourceKeyPoint = new System.Windows.Forms.Button();
            this.btnComputeKeyPoint = new System.Windows.Forms.Button();
            this.openFileDialogKeyPoint = new System.Windows.Forms.OpenFileDialog();
            this.deformationOptions = new System.Windows.Forms.GroupBox();
            this.radioButtonBSpline = new System.Windows.Forms.RadioButton();
            this.radioButtonTPS = new System.Windows.Forms.RadioButton();
            this.groupBoxDeformation = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSource)).BeginInit();
            this.groupBoxImageIO.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTarget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxResult)).BeginInit();
            this.interpolationOptions.SuspendLayout();
            this.groupBoxKeyPoint.SuspendLayout();
            this.deformationOptions.SuspendLayout();
            this.groupBoxDeformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLoadSourceImage
            // 
            this.btnLoadSourceImage.Location = new System.Drawing.Point(6, 24);
            this.btnLoadSourceImage.Name = "btnLoadSourceImage";
            this.btnLoadSourceImage.Size = new System.Drawing.Size(110, 30);
            this.btnLoadSourceImage.TabIndex = 0;
            this.btnLoadSourceImage.Text = "加载源图片";
            this.btnLoadSourceImage.UseVisualStyleBackColor = true;
            this.btnLoadSourceImage.Click += new System.EventHandler(this.btnLoadSourceImage_Clicked);
            // 
            // pictureBoxSource
            // 
            this.pictureBoxSource.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxSource.Location = new System.Drawing.Point(90, 10);
            this.pictureBoxSource.Name = "pictureBoxSource";
            this.pictureBoxSource.Size = new System.Drawing.Size(320, 320);
            this.pictureBoxSource.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxSource.TabIndex = 1;
            this.pictureBoxSource.TabStop = false;
            // 
            // groupBoxImageIO
            // 
            this.groupBoxImageIO.Controls.Add(this.btnLoadTarget);
            this.groupBoxImageIO.Controls.Add(this.btnLoadSourceImage);
            this.groupBoxImageIO.Controls.Add(this.btnSave);
            this.groupBoxImageIO.Location = new System.Drawing.Point(500, 337);
            this.groupBoxImageIO.Name = "groupBoxImageIO";
            this.groupBoxImageIO.Size = new System.Drawing.Size(355, 60);
            this.groupBoxImageIO.TabIndex = 2;
            this.groupBoxImageIO.TabStop = false;
            this.groupBoxImageIO.Text = "图片读写";
            // 
            // btnLoadTarget
            // 
            this.btnLoadTarget.Location = new System.Drawing.Point(122, 24);
            this.btnLoadTarget.Name = "btnLoadTarget";
            this.btnLoadTarget.Size = new System.Drawing.Size(110, 30);
            this.btnLoadTarget.TabIndex = 2;
            this.btnLoadTarget.Text = "加载目标图片";
            this.btnLoadTarget.UseVisualStyleBackColor = true;
            this.btnLoadTarget.Click += new System.EventHandler(this.btnLoadTargetImage_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(238, 24);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(110, 30);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "保存输出图片";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // openFileDialogImage
            // 
            this.openFileDialogImage.Filter = "图片|*.jpg;*.png;*.gif;*.jpeg;*.bmp\";";
            this.openFileDialogImage.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_FileOk);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "图片|*.jpg;*.png;*.gif;*.jpeg;*.bmp\";";
            this.saveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog_FileOk);
            // 
            // pictureBoxTarget
            // 
            this.pictureBoxTarget.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxTarget.Location = new System.Drawing.Point(515, 10);
            this.pictureBoxTarget.Name = "pictureBoxTarget";
            this.pictureBoxTarget.Size = new System.Drawing.Size(320, 320);
            this.pictureBoxTarget.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxTarget.TabIndex = 3;
            this.pictureBoxTarget.TabStop = false;
            // 
            // pictureBoxResult
            // 
            this.pictureBoxResult.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxResult.Location = new System.Drawing.Point(90, 340);
            this.pictureBoxResult.Name = "pictureBoxResult";
            this.pictureBoxResult.Size = new System.Drawing.Size(320, 320);
            this.pictureBoxResult.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxResult.TabIndex = 4;
            this.pictureBoxResult.TabStop = false;
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new System.Drawing.Point(18, 12);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(67, 15);
            this.lblSource.TabIndex = 5;
            this.lblSource.Text = "源图像：";
            // 
            // lblTarget
            // 
            this.lblTarget.AutoSize = true;
            this.lblTarget.Location = new System.Drawing.Point(427, 12);
            this.lblTarget.Name = "lblTarget";
            this.lblTarget.Size = new System.Drawing.Size(82, 15);
            this.lblTarget.TabIndex = 6;
            this.lblTarget.Text = "目标图像：";
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(3, 342);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(82, 15);
            this.lblResult.TabIndex = 7;
            this.lblResult.Text = "输出图像：";
            // 
            // radioButtonNearest
            // 
            this.radioButtonNearest.AutoSize = true;
            this.radioButtonNearest.Location = new System.Drawing.Point(12, 24);
            this.radioButtonNearest.Name = "radioButtonNearest";
            this.radioButtonNearest.Size = new System.Drawing.Size(103, 19);
            this.radioButtonNearest.TabIndex = 8;
            this.radioButtonNearest.TabStop = true;
            this.radioButtonNearest.Text = "最近邻插值";
            this.radioButtonNearest.UseVisualStyleBackColor = true;
            // 
            // radioButtonBilinear
            // 
            this.radioButtonBilinear.AutoSize = true;
            this.radioButtonBilinear.Location = new System.Drawing.Point(128, 24);
            this.radioButtonBilinear.Name = "radioButtonBilinear";
            this.radioButtonBilinear.Size = new System.Drawing.Size(103, 19);
            this.radioButtonBilinear.TabIndex = 9;
            this.radioButtonBilinear.TabStop = true;
            this.radioButtonBilinear.Text = "双线性插值";
            this.radioButtonBilinear.UseVisualStyleBackColor = true;
            // 
            // radioButtonBicubic
            // 
            this.radioButtonBicubic.AutoSize = true;
            this.radioButtonBicubic.Location = new System.Drawing.Point(244, 24);
            this.radioButtonBicubic.Name = "radioButtonBicubic";
            this.radioButtonBicubic.Size = new System.Drawing.Size(103, 19);
            this.radioButtonBicubic.TabIndex = 10;
            this.radioButtonBicubic.TabStop = true;
            this.radioButtonBicubic.Text = "双三次插值";
            this.radioButtonBicubic.UseVisualStyleBackColor = true;
            // 
            // interpolationOptions
            // 
            this.interpolationOptions.Controls.Add(this.radioButtonNearest);
            this.interpolationOptions.Controls.Add(this.radioButtonBicubic);
            this.interpolationOptions.Controls.Add(this.radioButtonBilinear);
            this.interpolationOptions.Location = new System.Drawing.Point(500, 543);
            this.interpolationOptions.Name = "interpolationOptions";
            this.interpolationOptions.Size = new System.Drawing.Size(355, 54);
            this.interpolationOptions.TabIndex = 11;
            this.interpolationOptions.TabStop = false;
            this.interpolationOptions.Text = "插值选项";
            // 
            // buttonDeformation
            // 
            this.buttonDeformation.Location = new System.Drawing.Point(122, 24);
            this.buttonDeformation.Name = "buttonDeformation";
            this.buttonDeformation.Size = new System.Drawing.Size(110, 30);
            this.buttonDeformation.TabIndex = 12;
            this.buttonDeformation.Text = "开始变换";
            this.buttonDeformation.UseVisualStyleBackColor = true;
            this.buttonDeformation.Click += new System.EventHandler(this.buttonDeformation_Click);
            // 
            // btnShowResult
            // 
            this.btnShowResult.Location = new System.Drawing.Point(238, 24);
            this.btnShowResult.Name = "btnShowResult";
            this.btnShowResult.Size = new System.Drawing.Size(110, 30);
            this.btnShowResult.TabIndex = 13;
            this.btnShowResult.Text = "展示输出图像";
            this.btnShowResult.UseVisualStyleBackColor = true;
            this.btnShowResult.Click += new System.EventHandler(this.btnShowResult_Click);
            // 
            // groupBoxKeyPoint
            // 
            this.groupBoxKeyPoint.Controls.Add(this.buttonShowKeyPoint);
            this.groupBoxKeyPoint.Controls.Add(this.btnLoadTargetKeyPoint);
            this.groupBoxKeyPoint.Controls.Add(this.btnLoadSourceKeyPoint);
            this.groupBoxKeyPoint.Location = new System.Drawing.Point(500, 406);
            this.groupBoxKeyPoint.Name = "groupBoxKeyPoint";
            this.groupBoxKeyPoint.Size = new System.Drawing.Size(355, 60);
            this.groupBoxKeyPoint.TabIndex = 3;
            this.groupBoxKeyPoint.TabStop = false;
            this.groupBoxKeyPoint.Text = "关键点";
            // 
            // buttonShowKeyPoint
            // 
            this.buttonShowKeyPoint.Location = new System.Drawing.Point(238, 24);
            this.buttonShowKeyPoint.Name = "buttonShowKeyPoint";
            this.buttonShowKeyPoint.Size = new System.Drawing.Size(110, 30);
            this.buttonShowKeyPoint.TabIndex = 3;
            this.buttonShowKeyPoint.Text = "显示关键点";
            this.buttonShowKeyPoint.UseVisualStyleBackColor = true;
            this.buttonShowKeyPoint.Click += new System.EventHandler(this.buttonShowKeyPoint_Click);
            // 
            // btnLoadTargetKeyPoint
            // 
            this.btnLoadTargetKeyPoint.Location = new System.Drawing.Point(122, 24);
            this.btnLoadTargetKeyPoint.Name = "btnLoadTargetKeyPoint";
            this.btnLoadTargetKeyPoint.Size = new System.Drawing.Size(110, 30);
            this.btnLoadTargetKeyPoint.TabIndex = 2;
            this.btnLoadTargetKeyPoint.Text = "目标图关键点";
            this.btnLoadTargetKeyPoint.UseVisualStyleBackColor = true;
            this.btnLoadTargetKeyPoint.Click += new System.EventHandler(this.btnLoadTargetKeyPoint_Click);
            // 
            // btnLoadSourceKeyPoint
            // 
            this.btnLoadSourceKeyPoint.Location = new System.Drawing.Point(6, 24);
            this.btnLoadSourceKeyPoint.Name = "btnLoadSourceKeyPoint";
            this.btnLoadSourceKeyPoint.Size = new System.Drawing.Size(109, 30);
            this.btnLoadSourceKeyPoint.TabIndex = 0;
            this.btnLoadSourceKeyPoint.Text = "源图片关键点";
            this.btnLoadSourceKeyPoint.UseVisualStyleBackColor = true;
            this.btnLoadSourceKeyPoint.Click += new System.EventHandler(this.btnLoadSourceKeyPoint_Click);
            // 
            // btnComputeKeyPoint
            // 
            this.btnComputeKeyPoint.Location = new System.Drawing.Point(6, 24);
            this.btnComputeKeyPoint.Name = "btnComputeKeyPoint";
            this.btnComputeKeyPoint.Size = new System.Drawing.Size(110, 30);
            this.btnComputeKeyPoint.TabIndex = 1;
            this.btnComputeKeyPoint.Text = "计算关键点";
            this.btnComputeKeyPoint.UseVisualStyleBackColor = true;
            this.btnComputeKeyPoint.Click += new System.EventHandler(this.btnComputeKeyPoint_Click);
            // 
            // openFileDialogKeyPoint
            // 
            this.openFileDialogKeyPoint.Filter = "文本文件|*.txt;";
            this.openFileDialogKeyPoint.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialogKeyPoint_FileOk);
            // 
            // deformationOptions
            // 
            this.deformationOptions.Controls.Add(this.radioButtonBSpline);
            this.deformationOptions.Controls.Add(this.radioButtonTPS);
            this.deformationOptions.Location = new System.Drawing.Point(500, 601);
            this.deformationOptions.Name = "deformationOptions";
            this.deformationOptions.Size = new System.Drawing.Size(355, 51);
            this.deformationOptions.TabIndex = 12;
            this.deformationOptions.TabStop = false;
            this.deformationOptions.Text = "变形选项";
            // 
            // radioButtonBSpline
            // 
            this.radioButtonBSpline.AutoSize = true;
            this.radioButtonBSpline.Location = new System.Drawing.Point(12, 24);
            this.radioButtonBSpline.Name = "radioButtonBSpline";
            this.radioButtonBSpline.Size = new System.Drawing.Size(96, 19);
            this.radioButtonBSpline.TabIndex = 8;
            this.radioButtonBSpline.TabStop = true;
            this.radioButtonBSpline.Text = "B样条变形";
            this.radioButtonBSpline.UseVisualStyleBackColor = true;
            // 
            // radioButtonTPS
            // 
            this.radioButtonTPS.AutoSize = true;
            this.radioButtonTPS.Location = new System.Drawing.Point(128, 24);
            this.radioButtonTPS.Name = "radioButtonTPS";
            this.radioButtonTPS.Size = new System.Drawing.Size(118, 19);
            this.radioButtonTPS.TabIndex = 9;
            this.radioButtonTPS.TabStop = true;
            this.radioButtonTPS.Text = "薄板样条变形";
            this.radioButtonTPS.UseVisualStyleBackColor = true;
            // 
            // groupBoxDeformation
            // 
            this.groupBoxDeformation.Controls.Add(this.buttonDeformation);
            this.groupBoxDeformation.Controls.Add(this.btnShowResult);
            this.groupBoxDeformation.Controls.Add(this.btnComputeKeyPoint);
            this.groupBoxDeformation.Location = new System.Drawing.Point(500, 474);
            this.groupBoxDeformation.Name = "groupBoxDeformation";
            this.groupBoxDeformation.Size = new System.Drawing.Size(355, 60);
            this.groupBoxDeformation.TabIndex = 3;
            this.groupBoxDeformation.TabStop = false;
            this.groupBoxDeformation.Text = "变形";
            // 
            // FaceWraper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(882, 663);
            this.Controls.Add(this.groupBoxDeformation);
            this.Controls.Add(this.deformationOptions);
            this.Controls.Add(this.groupBoxKeyPoint);
            this.Controls.Add(this.interpolationOptions);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.lblTarget);
            this.Controls.Add(this.lblSource);
            this.Controls.Add(this.pictureBoxResult);
            this.Controls.Add(this.pictureBoxTarget);
            this.Controls.Add(this.groupBoxImageIO);
            this.Controls.Add(this.pictureBoxSource);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FaceWraper";
            this.Text = "FaceWraper";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSource)).EndInit();
            this.groupBoxImageIO.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTarget)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxResult)).EndInit();
            this.interpolationOptions.ResumeLayout(false);
            this.interpolationOptions.PerformLayout();
            this.groupBoxKeyPoint.ResumeLayout(false);
            this.deformationOptions.ResumeLayout(false);
            this.deformationOptions.PerformLayout();
            this.groupBoxDeformation.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ImageProcesser imageProcesser;
        private bool isLoadSource;
        private bool isKeyPointShown;
        private string sourceImgPath;
        private string targetImgPath;

        private System.Windows.Forms.Button btnLoadSourceImage;
        private System.Windows.Forms.PictureBox pictureBoxSource;
        private System.Windows.Forms.GroupBox groupBoxImageIO;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.OpenFileDialog openFileDialogImage;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Button btnLoadTarget;
        private System.Windows.Forms.PictureBox pictureBoxTarget;
        private System.Windows.Forms.PictureBox pictureBoxResult;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.Label lblTarget;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.RadioButton radioButtonNearest;
        private System.Windows.Forms.RadioButton radioButtonBilinear;
        private System.Windows.Forms.RadioButton radioButtonBicubic;
        private System.Windows.Forms.GroupBox interpolationOptions;
        private System.Windows.Forms.Button buttonDeformation;
        private System.Windows.Forms.Button btnShowResult;
        private System.Windows.Forms.GroupBox groupBoxKeyPoint;
        private System.Windows.Forms.Button btnLoadTargetKeyPoint;
        private System.Windows.Forms.Button btnLoadSourceKeyPoint;
        private System.Windows.Forms.Button btnComputeKeyPoint;
        private System.Windows.Forms.OpenFileDialog openFileDialogKeyPoint;
        private System.Windows.Forms.GroupBox deformationOptions;
        private System.Windows.Forms.RadioButton radioButtonBSpline;
        private System.Windows.Forms.RadioButton radioButtonTPS;
        private System.Windows.Forms.GroupBox groupBoxDeformation;
        private System.Windows.Forms.Button buttonShowKeyPoint;
    }
}

