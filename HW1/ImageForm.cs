using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NumericalAnalysis1
{
    public partial class ImageForm : Form
    {
        public ImageForm()
        {
            InitializeComponent();
        }

        public void setImage(Bitmap img)
        {
            this.Width = img.Size.Width + 30;
            this.Height = img.Size.Height + 60;
            pictureBoxDisplay.Width = img.Width;
            pictureBoxDisplay.Height = img.Height;
            pictureBoxDisplay.Image = img;
        }
    }
}
