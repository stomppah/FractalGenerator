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
        private Image offScreen;
        private Graphics g1;
        private Pen p;
        //private Cursor c1, c2;

        public void init() // all instances will be prepared
        {
            finished = false;
            action = false;
            // setSize(800, 600);
            //addMouseListener(this);
            //addMouseMotionListener(this);
            //c1 = new Cursor(Cursor.WAIT_CURSOR);
            //c2 = new Cursor(Cursor.CROSSHAIR_CURSOR);
            p = new Pen(Color.Black);
            x1 = Width;
            y1 = Height;
            xy = (float)x1 / (float)y1;
            offScreen = new Bitmap(picture.Width, picture.Height);
            g1 = Graphics.FromImage(offScreen);     //picture = createImage(x1, y1);
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
            /* setCursor(c1); */
            Text = "Mandelbrot-Set will be produced - please wait...";
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
                }

            offScreen.Save("Test.bmp");

            Text = "Mandelbrot-Set ready - please select zoom area with pressed mouse.";
            /* setCursor(c2);  */

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

        private void picture_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            Graphics g = e.Graphics;
            g.DrawImage(offScreen, 0, 0);      //(picture, 0, 0, this);
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
                    if (ys < ye) g.DrawRectangle(p, xe, ys, (xs - xe), (ye - ys));          //drawRect(xe, ys, (xs - xe), (ye - ys));
                    else g.DrawRectangle(p, xe, ye, (xs - xe), (ys - ye));
                }
            }
        }

        private void mousePressed(object sender, MouseEventArgs e)
        {
            //e.consume();
            action = (e.Button == MouseButtons.Right) ? false : true;
            if (action)
            {
                xs = e.X;
                ys = e.Y;
            }
        }

        private void mouseReleased(object sender, MouseEventArgs e)
        {
            int z, w;

            //e.consume();
            if (action)
            {
                xe = e.X;
                ye = e.Y;
                if (xs > xe)
                {
                    z = xs;
                    xs = xe;
                    xe = z;
                }
                if (ys > ye)
                {
                    z = ys;
                    ys = ye;
                    ye = z;
                }
                w = (xe - xs);
                z = (ye - ys);
                if ((w < 2) && (z < 2)) initvalues();
                else
                {
                    if (((float)w > (float)z * xy)) ye = (int)((float)ys + (float)w / xy);
                    else xe = (int)((float)xs + (float)z * xy);
                    xende = xstart + xzoom * (double)xe;
                    yende = ystart + yzoom * (double)ye;
                    xstart += xzoom * (double)xs;
                    ystart += yzoom * (double)ys;
                }
                xzoom = (xende - xstart) / (double)x1;
                yzoom = (yende - ystart) / (double)y1;
                mandelbrot();
                rectangle = false;
                Refresh();      //repaint();
            }
            action = false;
        }

        private void mouseDragged(object sender, MouseEventArgs e)
        {
            //e.consume();
            if (action)
            {
                xe = e.X;
                ye = e.Y;
                rectangle = true;
                Refresh();      //repaint();
            }
        }

        private void copyToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetImage(offScreen);
        }

        private void saveFile_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string saveFile = "";
            saveImageDialog.Title = "Save current display to image file";
            saveImageDialog.FileName = "";

            saveImageDialog.Filter = "Bitmap Files|*.bmp";

            if (saveImageDialog.ShowDialog() != DialogResult.Cancel)
            {
                saveFile = saveImageDialog.FileName;
                offScreen.Save(saveFile, System.Drawing.Imaging.ImageFormat.Bmp);
            }
        }
    }

}