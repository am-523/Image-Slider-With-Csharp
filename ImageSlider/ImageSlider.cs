using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

class ImageSlider1 : Panel
{
    Timer _timer;
    int _captionTextLeft = 20;
    int _captionPosX = 20;
    int _pageIndex = 0;

    protected List<Image> _imageList = new List<Image>();
    protected List<string> _captionList = new List<string>();
    protected List<Color> _captionBgColor = new List<Color>();

    xButton leftButton;
    xButton rightButton;

    public ImageSlider1()
    {
        this.Animation = true;
        this.CaptionAnimationSpeed = 50;
        this.CaptionTextLeft = 20;
        this.CaptionHeight = 50;
        this.CaptionBackgrounColor = Color.Black;
        this.CaptionOpacity = 100;

        leftButton = new xButton();
        leftButton.Text = "<";
        leftButton.Click += new EventHandler(leftButton_Click);

        rightButton = new xButton();
        rightButton.Text = ">";
        rightButton.Click += new EventHandler(rightButton_Click);

        this.Resize += ImageSlider_Resize;

        this.Controls.Add(leftButton);
        this.Controls.Add(rightButton);
    }

    void ImageSlider_Resize(object sender, EventArgs e)
    {
        leftButton.Location = new Point(0, (this.Height / 2) - (leftButton.Height / 2));
        rightButton.Location = new Point(this.Width - rightButton.Width, (this.Height / 2) - (rightButton.Height / 2));
    }

    void leftButton_Click(object sender, EventArgs e)
    {

        if (_pageIndex > 0)
        {
            --_pageIndex;
        }
        else
        {
            _pageIndex = _imageList.Count - 1;
        }

        if (Animation)
        {
            _captionPosX = this.Width;
            this.DoubleBuffered = true;

            _timer = new Timer();
            _timer.Interval = 1;
            _timer.Tick += new EventHandler(_timer_Tick);
            _timer.Start();
        }
        else
        {
            _captionPosX = _captionTextLeft;
            this.Invalidate();
        }
    }

    void rightButton_Click(object sender, EventArgs e)
    {

        if (_pageIndex < _imageList.Count - 1)
        {
            ++_pageIndex;
        }
        else
        {
            _pageIndex = 0;
        }

        if (Animation)
        {
            _captionPosX = this.Width;
            DoubleBuffered = true;

            _timer = new Timer();
            _timer.Interval = 1;
            _timer.Tick += new EventHandler(_timer_Tick);
            _timer.Start();
        }
        else
        {
            _captionPosX = _captionTextLeft;
            this.Invalidate();
        }
    }

    void _timer_Tick(object sender, EventArgs e)
    {
        if (_captionPosX >= _captionTextLeft)
        {
            int subtract = CaptionAnimationSpeed;

            int diff = _captionPosX - subtract;

            if (diff < subtract)
            {
                _captionPosX -= _captionPosX - _captionTextLeft;
            }
            else
            {
                _captionPosX -= subtract;
            }

            this.Invalidate();
        }
        else
        {
            this.DoubleBuffered = false;
            _timer.Dispose();
        }
    }

    public void AddImage(string path)
    {
        Image img = Image.FromFile(path);
        _AddImage(img, "", this.CaptionBackgrounColor);
    }

    public void AddImage(string path, string caption)
    {
        Image img = Image.FromFile(path);
        _AddImage(img, caption, this.CaptionBackgrounColor);
    }

    public void AddImage(string path, string caption, Color captionBackgroundColor)
    {
        Image img = Image.FromFile(path);
        _AddImage(img, caption, captionBackgroundColor);
    }

    public void AddImage(Image img)
    {
        _AddImage(img, "", this.CaptionBackgrounColor);
    }

    public void AddImage(Image img, string caption)
    {
        _AddImage(img, caption, this.CaptionBackgrounColor);
    }

    public void AddImage(Image img, string caption, Color captionBackgroundColor)
    {
        _AddImage(img, caption, captionBackgroundColor);
    }

    protected void _AddImage(Image img, string caption, Color captionBackgroundColor)
    {
        _imageList.Add(img);
        _captionList.Add(caption);
        _captionBgColor.Add(captionBackgroundColor);
    }

    public int CaptionHeight
    {
        set;
        get;
    }

    public int CaptionTextLeft
    {
        set
        {
            _captionPosX = value;
            _captionTextLeft = value;
        }
        get
        {
            return _captionTextLeft;
        }
    }

    public Color CaptionBackgrounColor
    {
        set;
        get;
    }

    public int CaptionOpacity
    {
        set;
        get;
    }

    public int CaptionAnimationSpeed
    {
        set;
        get;
    }

    public bool Animation
    {
        set;
        get;
    }

    public xButton LeftButton
    {
        get
        {
            return leftButton;
        }
    }

    public xButton RightButton
    {
        get
        {
            return rightButton;
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        Graphics g = e.Graphics;
        try
        {
            Color captionBgColor = Color.FromArgb(CaptionOpacity, _captionBgColor[_pageIndex].R, _captionBgColor[_pageIndex].G, _captionBgColor[_pageIndex].B);
            g.DrawImage(_imageList[_pageIndex], new Rectangle(0, 0, this.Width, this.Height));
            g.FillRectangle(new SolidBrush(captionBgColor), new Rectangle(0, this.Height - this.CaptionHeight, this.Width, this.Height));

            string caption = _captionList[_pageIndex];

            SizeF fontSize = g.MeasureString(_captionList[_pageIndex], this.Font);
            g.DrawString(_captionList[_pageIndex], this.Font, new SolidBrush(this.ForeColor), _captionPosX, this.Height - (int)(this.CaptionHeight - (fontSize.Height / 2)));
        }
        catch { }
    }

    public class xButton : Button
    {
        public xButton()
        {
            this.BackColor = Color.Black;
            this.Height = 50;
            this.Width = 50;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            Rectangle area = new Rectangle(0, 0, this.Width, this.Height);

            g.FillRectangle(new SolidBrush(this.BackColor), area);
            SizeF fontSize = g.MeasureString(this.Text, this.Font);
            g.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), (this.Width - fontSize.Width) / 2, (this.Height - fontSize.Height) / 2);
        }
    }
}