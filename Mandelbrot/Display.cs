using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mandelbrot
{
    /// <summary>
    /// djm added, Java can work with the HSB/V colour system
    /// c# can only do RGB
    /// so I searched the internet for some code to convert
    /// see http://www.codeproject.com/dotnet/HSBColorClass.asp
    /// note I have removed some code from the downloaded class that isn't needed, just to make it clearer
    /// </summary>
    public struct HSBColor
    {
        float h;
        float s;
        float b;
        int a;

        public HSBColor(float h, float s, float b)
        {
            this.a = 0xff;
            this.h = Math.Min(Math.Max(h, 0), 255);
            this.s = Math.Min(Math.Max(s, 0), 255);
            this.b = Math.Min(Math.Max(b, 0), 255);
        }

        public HSBColor(int a, float h, float s, float b)
        {
            this.a = a;
            this.h = Math.Min(Math.Max(h, 0), 255);
            this.s = Math.Min(Math.Max(s, 0), 255);
            this.b = Math.Min(Math.Max(b, 0), 255);
        }

        public float H
        {
            get { return h; }
        }

        public float S
        {
            get { return s; }
        }

        public float B
        {
            get { return b; }
        }

        public int A
        {
            get { return a; }
        }

        public Color Color
        {
            get
            {
                return FromHSB(this);
            }
        }
        
        public static Color FromHSB(HSBColor hsbColor)
        {
            float r = hsbColor.b;
            float g = hsbColor.b;
            float b = hsbColor.b;
            if (hsbColor.s != 0)
            {
                float max = hsbColor.b;
                float dif = hsbColor.b * hsbColor.s / 255f;
                float min = hsbColor.b - dif;

                float h = hsbColor.h * 360f / 255f;

                if (h < 60f)
                {
                    r = max;
                    g = h * dif / 60f + min;
                    b = min;
                }
                else if (h < 120f)
                {
                    r = -(h - 120f) * dif / 60f + min;
                    g = max;
                    b = min;
                }
                else if (h < 180f)
                {
                    r = min;
                    g = max;
                    b = (h - 120f) * dif / 60f + min;
                }
                else if (h < 240f)
                {
                    r = min;
                    g = -(h - 240f) * dif / 60f + min;
                    b = max;
                }
                else if (h < 300f)
                {
                    r = (h - 240f) * dif / 60f + min;
                    g = min;
                    b = max;
                }
                else if (h <= 360f)
                {
                    r = max;
                    g = min;
                    b = -(h - 360f) * dif / 60 + min;
                }
                else
                {
                    r = 0;
                    g = 0;
                    b = 0;
                }
            }

            return Color.FromArgb
                ( 
                    hsbColor.a,
                    (int)Math.Round(Math.Min(Math.Max(r, 0), 255)),
                    (int)Math.Round(Math.Min(Math.Max(g, 0), 255)),
                    (int)Math.Round(Math.Min(Math.Max(b, 0), 255))
                    );
        }
       
    }

    public partial class Display : Form
    {
        private const int MAX = 256;      // max iterations
	    private const double SX = -2.025; // start value real
	    private const double SY = -1.125; // start value imaginary
	    private const double EX = 0.6;    // end value real
	    private const double EY = 1.125;  // end value imaginary
	    private static int x1, y1, xs, ys, xe, ye;
	    private static double xstart, ystart, xende, yende, xzoom, yzoom;
	    private static bool action, rectangle, finished;
	    private static float xy;
        //private Image picture;
        private Image offScreen = new Bitmap(730, 562);
        private Graphics g1;
        private Pen p;
        //private Cursor c1, c2;
        private HSBColor HSBcol;

        public void init() // all instances will be prepared
        {
            HSBcol = new HSBColor();
           // setSize(800, 600);
            finished = false;
            //addMouseListener(this);
            //addMouseMotionListener(this);
            //c1 = new Cursor(Cursor.WAIT_CURSOR);
            //c2 = new Cursor(Cursor.CROSSHAIR_CURSOR);
            p = new Pen(Color.Black);
            x1 = Width;
            y1 = Height;
            xy = (float)x1 / (float)y1;
            //picture.CreateControl();        //picture = createImage(x1, y1);
            g1 = Graphics.FromImage(offScreen);
            finished = true;
        }

        public void start()
        {
            action = false;
            rectangle = false;
            initvalues();
            xzoom = (xende - xstart) / (double)x1;
            yzoom = (yende - ystart) / (double)y1;
            mandelbrot();
        }

        private void mandelbrot() // calculate all points
        {
            int x, y;
            float h, b, alt = 0.0f;

            action = false;
         /* setCursor(c1);
            showStatus("Mandelbrot-Set will be produced - please wait...");  */
            for (x = 0; x < x1; x += 2)
                for (y = 0; y < y1; y++)
                {
                    h = pointcolour(xstart + xzoom * (double)x, ystart + yzoom * (double)y); // color value
                    if (h != alt)
                    {
                        b = 1.0f - h * h; // brightnes
                        ///djm added
                        ///HSBcol.fromHSB(h,0.8f,b); //convert hsb to rgb then make a Java Color
                        ///Color col = new Color(0,HSBcol.rChan,HSBcol.gChan,HSBcol.bChan);
                        ///g1.setColor(col);
                        //djm end
                        //djm added to convert to RGB from HSB

                        //g1.Clear((Color)HSBColor.FromHSB(new HSBColor(h, 0.8f, b)));     //Color(HSBColor.FromHSB(h, 0.8f, b));
                        //djm test
                        Mandelbrot.HSBColor hsb = new Mandelbrot.HSBColor(h * 255f, 0.8f * 255f, b * 255f);

                        Color col = hsb.Color;
                        //g1.Clear(Color.Black);
                        int red = col.R;
                        int green = col.G;
                        int blue = col.B;

                        //djm 
                        alt = h;
                        p.Color = Color.FromArgb(red, green, blue);
                    }
                    g1.DrawLine(p, x, y, x + 1, y);
                    //g1.DrawLine(p, x, y, x + 1, y);
                }
            
            offScreen.Save("Test.bmp");
         /* showStatus("Mandelbrot-Set ready - please select zoom area with pressed mouse.");
            setCursor(c2);  */
            action = true;
        }

        private float pointcolour(double xwert, double ywert) // color value from 0.0 to 1.0 by iterations
        {
            double r = 0.0, i = 0.0, m = 0.0;
            int j = 0;

            while ((j < MAX) && (m < 4.0))
            {
                j++;
                m = r * r - i * i;
                i = 2.0 * r * i + ywert;
                r = m + xwert;
            }
            return (float)j / (float)MAX;
        }

        private void initvalues() // reset start values
        {
            xstart = SX;
            ystart = SY;
            xende = EX;
            yende = EY;
            if ((float)((xende - xstart) / (yende - ystart)) != xy)
                xstart = xende - (yende - ystart) * (double)xy;
        }

        public Display()
        {
            InitializeComponent();
            init();
            start();
        }

        private void picture_Click(object sender, EventArgs e)
        {

        }

        private void picture_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(offScreen, 0, 0, Width, Height);      //(picture, 0, 0, this);
            if (rectangle)
            {
                p.Color = Color.Black;      //.setColor(Color.white);
                if (xs < xe)
                {
                    if (ys < ye) g.DrawRectangle(p, xs, ys, (xe - xs), (ye - ys));  //drawRect(xs, ys, (xe - xs), (ye - ys));
                    else g.DrawRectangle(p, xs, ye, (xe - xs), (ys - ye));
                }
                else
                {
                    if (ys < ye) g.DrawRectangle(p, xe, ys, (xs - xe), (ye - ys) );          //drawRect(xe, ys, (xs - xe), (ye - ys));
                    else g.DrawRectangle(p, xe, ye, (xs - xe), (ys - ye));
                }
            }
        }
    }
}
