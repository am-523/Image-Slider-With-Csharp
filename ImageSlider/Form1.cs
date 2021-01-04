using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageSlider
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private int imganumber = 1;
        private void slider()
        {
            if(imganumber==7)
            {
                //pictureBox1.Image = imageList1.Images[imganumber];
              
                imganumber = 1;
            }
            pictureBox1.ImageLocation = string.Format(@"Img\{0}.png", imganumber);
            imganumber++;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            slider();
        }
    }
}
