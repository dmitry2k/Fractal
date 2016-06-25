using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Fractal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Dictionary<char, string> rules;
        public string generic_string;
        public int length;
        public double angle, direction;

        public class MyPoint
        {
            private double x, y;
            private Color c;

            public double X
            {
                get
                {
                    return x;
                }
            }

            public double Y
            {
                get
                {
                    return y;
                }
            }

            public Color Color
            {
                get
                {
                    return c;
                }
            }

            public MyPoint(double x, double y, Color c)
            {
                this.x = x;
                this.y = y;
                this.c = c;
            }

            public MyPoint(double x, double y)
            {
                this.x = x;
                this.y = y;
                this.c = Color.Black;
            }

            public void RotationPoint(MyPoint p, double rotation)
            {
                double new_x = (x - p.X) * Math.Cos(rotation) - (y - p.Y) * Math.Sin(rotation) + p.X;
                double new_y = (x - p.X) * Math.Sin(rotation) + (y - p.Y) * Math.Cos(rotation) + p.Y;
                x = new_x;
                y = new_y;
            }

            public Bitmap DrawPoint(Bitmap bm)
            {
                if (this.x > 0 && this.x < bm.Width && this.y > 0 && this.y < bm.Height)
                    bm.SetPixel((int)this.x, (int)this.y, this.c);
                return bm;
            }

            public Bitmap DrawBigPoint(Bitmap bm)
            {
                if (this.x > 1 && this.x < bm.Width - 1 && this.y > 1 && this.y < bm.Height - 1)
                    for (int i = -1; i < 2; ++i)
                        for (int j = -1; j < 2; ++j)
                            bm.SetPixel((int)this.x + i, (int)this.y + j, this.c);
                else
                    bm = this.DrawPoint(bm);
                return bm;
            }

        }
        public class MyEdge
        {
            private MyPoint p1, p2;

            public MyPoint P1
            {
                get { return p1; }
            }

            public MyPoint P2
            {
                get { return p2; }
            }

            public MyEdge(MyPoint p1, MyPoint p2)
            {
                this.p1 = p1;
                this.p2 = p2;
            }

            public void RotationEdge(MyPoint p, double rotation)
            {
                p1.RotationPoint(p, rotation);
                p2.RotationPoint(p, rotation);
            }

            public Bitmap DrawEdge(Bitmap bm)
            {
                int x1 = (int)p1.X;
                int y1 = (int)p1.Y;
                int x2 = (int)p2.X;
                int y2 = (int)p2.Y;
                int dx = Math.Abs(x2 - x1);
                int dy = Math.Abs(y2 - y1);
                int sx, sy;
                if (x2 >= x1)
                    sx = 1;
                else
                    sx = -1;
                if (y2 >= y1)
                    sy = 1;
                else
                    sy = -1;
                if (dy < dx)
                {
                    int d = 2 * dy - dx;
                    int d1 = 2 * dy;
                    int d2 = 2 * (dy - dx);
                    bm.SetPixel(x1, y1, p1.Color);
                    int x = x1 + sx;
                    int y = y1;
                    for (int i = 1; i <= dx; i++)
                    {
                        if (d > 0)
                        {
                            d += d2;
                            y += sy;
                        }
                        else
                            d += d1;
                        bm.SetPixel(x, y, p1.Color);
                        x += sx;
                    }
                }
                else
                {
                    int d = 2 * dx - dy;
                    int d1 = 2 * dx;
                    int d2 = 2 * (dx - dy);
                    bm.SetPixel(x1, y1, p1.Color);
                    int x = x1;
                    int y = y1 + sy;
                    for (int i = 1; i <= dy; i++)
                    {
                        if (d > 0)
                        {
                            d += d2;
                            x += sx;
                        }
                        else
                            d += d1;
                        bm.SetPixel(x, y, p1.Color);
                        y += sy;
                    }
                }
                return bm;
            }

            static public Bitmap DrawEdge(Bitmap bm, MyPoint p1, MyPoint p2)
            {
                int x1 = (int)p1.X;
                int y1 = (int)p1.Y;
                int x2 = (int)p2.X;
                int y2 = (int)p2.Y;
                int dx = Math.Abs(x2 - x1);
                int dy = Math.Abs(y2 - y1);
                int sx, sy;
                if (x2 >= x1)
                    sx = 1;
                else
                    sx = -1;
                if (y2 >= y1)
                    sy = 1;
                else
                    sy = -1;
                if (dy < dx)
                {
                    int d = 2 * dy - dx;
                    int d1 = 2 * dy;
                    int d2 = 2 * (dy - dx);
                    bm.SetPixel(x1, y1, p1.Color);
                    int x = x1 + sx;
                    int y = y1;
                    for (int i = 1; i <= dx; i++)
                    {
                        if (d > 0)
                        {
                            d += d2;
                            y += sy;
                        }
                        else
                            d += d1;
                        bm.SetPixel(x, y, p1.Color);
                        x += sx;
                    }
                }
                else
                {
                    int d = 2 * dx - dy;
                    int d1 = 2 * dx;
                    int d2 = 2 * (dx - dy);
                    bm.SetPixel(x1, y1, p1.Color);
                    int x = x1;
                    int y = y1 + sy;
                    for (int i = 1; i <= dy; i++)
                    {
                        if (d > 0)
                        {
                            d += d2;
                            x += sx;
                        }
                        else
                            d += d1;
                        bm.SetPixel(x, y, p1.Color);
                        y += sy;
                    }
                }
                return bm;
            }

            public int WherePoint(MyPoint p)
            {
                double s = (p2.X - p1.X) * (p.Y - p1.Y) - (p2.Y - p1.Y) * (p.X - p1.X);
                if (s > 0)
                    return 1;
                else if (s < 0)
                    return -1;
                else
                    return 0;
            }

            public MyPoint IntersectionEdge(MyEdge edge)
            {
                double k1 = (p2.X - p1.X) / (p2.Y - p1.Y);
                double k2 = (edge.P2.X - edge.P1.X) / (edge.P2.Y - edge.P1.Y);
                if (k1 == k2)
                    return null;
                double x = ((p1.X * p2.Y - p2.X * p1.Y) * (edge.P2.X - edge.P1.X) - (edge.P1.X * edge.P2.Y - edge.P2.X * edge.P1.Y) * (p2.X - p1.X)) / ((p1.Y - p2.Y) * (edge.P2.X - edge.P1.X) - (edge.P1.Y - edge.P2.Y) * (p2.X - p1.X));
                double y = ((edge.P1.Y - edge.P2.Y) * x - (edge.P1.X * edge.P2.Y - edge.P2.X * edge.P1.Y)) / (edge.P2.X - edge.P1.X);
                if (x >= Math.Min(Math.Min(p1.X, p2.X), Math.Min(edge.P1.X, edge.P2.X)) && x <= Math.Max(Math.Max(p1.X, p2.X), Math.Max(edge.P1.X, edge.P2.X)) && y >= Math.Min(Math.Min(p1.Y, p2.Y), Math.Min(edge.P1.Y, edge.P2.Y)) && y <= Math.Max(Math.Max(p1.Y, p2.Y), Math.Max(edge.P1.Y, edge.P2.Y)))
                    return new MyPoint(x, y);
                else
                    return null;
            }
        }

        public string CreateFractal(Dictionary<char, string> rules, string generic_string, int count_iterations)
        {
            string result = generic_string;
            for (int i = 0; i < count_iterations; ++i)
            {
                int l = result.Length - 1;
                for (int j = l; j >= 0; --j)
                    if (rules.ContainsKey(result[j]))
                    {
                        result=result.Insert(j+1,rules[result[j]]);
                        result=result.Remove(j, 1);
                    }
            }
            return result;
        }

        //+(-) - поворот по (против) часовой стрелке на угол
        //F,G,D - рисуют прямую
        public Bitmap DrawFractal(string fractal, double direction, double angle, int length, Bitmap bm)
        {
            if (fractal==null)
                return bm;
            MyPoint p1 = new MyPoint(bm.Width/2, bm.Height / 2);
            MyPoint p2 = new MyPoint(bm.Width/2, bm.Height / 2 - length);
            Stack<KeyValuePair<MyPoint, double>> s = new Stack<KeyValuePair<MyPoint, double>>();
            for (int i = 0; i < fractal.Length; ++i)
            {
                if (fractal[i] == '+')
                    direction += angle;
                if (fractal[i] == '-')
                    direction -= angle;
                if (fractal[i] == 'F' || fractal[i] == 'G' || fractal[i] == 'D')
                {
                    p2.RotationPoint(p1, direction);
                    if (0<p1.X && 0<p1.Y && 0<p2.X && 0<p2.Y &&  bm.Width>p1.X && bm.Height>p1.Y && bm.Width>p2.X && bm.Height>p2.Y)
                        bm = MyEdge.DrawEdge(bm, p1, p2);
                    p1 = p2;
                    p2 = new MyPoint(p1.X, p1.Y - length);
                }
                if (fractal[i] == '[')
                    s.Push(new KeyValuePair<MyPoint, double>(p1, direction));
                if (fractal[i] == ']')
                {
                    p1 = s.Peek().Key;
                    direction = s.Pop().Value;
                    p2 = new MyPoint(p1.X, p1.Y - length);
                }
            }
            return bm;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rules = new Dictionary<char, string>();
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                string fn = openFileDialog1.FileName;
                FileInfo fi = new FileInfo(fn);
                StreamReader sr = new StreamReader(fn);
                generic_string = sr.ReadLine();
                string read = null;
                while ((read = sr.ReadLine()) != null)
                {
                    if (read.Length>1 && read[1] == '-')
                        rules.Add(read[0], read.Substring(3));
                    else
                    {
                        direction = Double.Parse(read) * Math.PI / 180;
                        angle = Double.Parse(sr.ReadLine()) * Math.PI / 180;
                        read = sr.ReadLine();
                        if (read != null)
                        {
                            length = Int32.Parse(read);
                            numericUpDown2.Value = length;
                        }
                    }
                }
                sr.Close();
            }
            numericUpDown1.Value = 0;
            button1_Click(sender, e);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (rules == null)
                return;
            Bitmap bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            bm = DrawFractal(CreateFractal(rules, generic_string, Int32.Parse(numericUpDown1.Value.ToString())), direction, angle, length, bm);
            pictureBox1.Image = bm;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (rules == null)
                return;
            length = Int32.Parse(numericUpDown2.Value.ToString());
            button1_Click(sender, e);
        }
    }
}
