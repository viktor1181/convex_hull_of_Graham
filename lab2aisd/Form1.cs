using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace lab2aisd
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        
        private static bool Ccw(double[] a, double[] b, double[] c)
        {
            return ((b[0] - a[0]) * (c[1] - a[1])) > ((b[1] - a[1]) * (c[0] - a[0]));
        }
        bool cw(double[] a, double[] b, double[] c)
        {
            return a[0] * (b[1] - c[1]) + b[0] * (c[1] - a[1]) + c[0] * (a[1] - b[1]) < 0;
        }

        bool ccw(double[] a, double[] b, double[] c)
        {
            return a[0] * (b[1] - c[1]) + b[0] * (c[1] - a[1]) + c[0] * (a[1] - b[1]) > 0;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int n = int.Parse(textBox3.Text);
            List<double[]> points = new List<double[]>();
            Random rand = new Random();
            for (int i = 0; i < n; i++)
            { 
                points.Add(new double[2] {
                    rand.NextDouble() * 100, // ось x
                    rand.NextDouble() * 100  // ось y
                });
            }
            double min = points[0][0];
            int minI = 0;
            // ищем самую левую точку
            for (var i = 1; i < n; i++)
            {
                if (points[i][0] < min)
                {
                    min = points[i][0];
                    minI = i;
                }
            }
            
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (points[j][0] < points[j+1][0] || points[j][0] == points[j + 1][0] && points[j][1] < points[j + 1][1])
                    {
                        // меняем элементы местами
                        var temp = points[j];
                        points[j] = points[j + 1];
                        points[j + 1] = temp;
                    }
                }
            }


            List<double[]> h = new List<double[]>();
            List<double[]> up = new List<double[]>();
            List<double[]> down = new List<double[]>();
            var p1 = points[0];
            var p2 = points[points.Count - 1];
            up.Add(p1);
            down.Add(p1);
            for (var i = 1; i < points.Count; ++i)
            {
                if (i == points.Count - 1 || cw(p1, points[i], p2))
                {
                    while (up.Count >= 2 && !cw(up[up.Count - 2], up[up.Count - 1], points[i]))
                        up.RemoveAt(up.Count - 1);
                    up.Add(points[i]);
                }
                if (i == points.Count - 1 || ccw(p1, points[i], p2))
                {
                    while (down.Count >= 2 && !ccw(down[down.Count - 2], down[down.Count - 1], points[i]))
                        down.RemoveAt(down.Count - 1);
                    down.Add(points[i]);
                }
            }
            // lower hull
            /*
            foreach (var pt in points)
            {
                while (h.Count >= 2 && !Ccw(h[h.Count - 2], h[h.Count - 1], pt))
                {
                    h.RemoveAt(h.Count - 1);
                }
                h.Add(pt);
            }

            // upper hull
            int t = h.Count + 1;
            for (int i = points.Count - 1; i >= 0; i--)
            {
                double[] pt = points[i];
                while (h.Count >= t && Ccw(h[h.Count - 2], h[h.Count - 1], pt))
                {
                    h.RemoveAt(h.Count - 1);
                }
                h.Add(pt);
            }

            h.RemoveAt(h.Count - 1);
            */
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = points.Select(x => new { X = x[0], Y = x[1] }).ToList();
            dataGridView1.DataSource = bindingSource;
            for (int i = 0; i < up.Count; ++i)
                h.Add(up[i]);
            for (int i = down.Count - 2; i > 0; --i)
                h.Add(down[i]);
            chart1.ChartAreas[0].AxisX.ScaleView.Zoom(0, 50);
            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            foreach (var pt in h)
            {
                chart1.Series[0].Points.AddXY(pt[0], pt[1]);
            }
            chart1.Series[0].Points.AddXY(h[0][0], h[0][1]);
            chart1.Series[0].Points.AddXY(h[h.Count - 1][0], h[h.Count - 1][1]);
            foreach (var pt in points)
            {
                chart1.Series[1].Points.AddXY(pt[0], pt[1]);
            }
            

            points.Clear();
            h.Clear();

            /*for (int i = 0; i < n; i++)
            {
                chart1.Series[0].Points.AddXY(i, Math.Cos(i));
            }*/
        }

    }
}














//int[] ch = new int[n];
/*
 * StringBuilder stb = new StringBuilder();
            stb.Append("[");
            for (int i = 0; i < n; i++)
            {
                stb.Append("x:" + points[i][0] + "y:" + points[i][1] + " , ");
            }
            stb.Append("]");
            string result = stb.ToString();
            textBox1.Text = result;
 * 
 * 
 *   StringBuilder stb1 = new StringBuilder();
            stb1.Append("[");
            foreach (var pt in h)
            {
                stb1.Append("x:" + pt[0] + "y:" + pt[1] + " , ");
            }
            stb1.Append("]");
            string result1 = stb1.ToString();
            textBox2.Text = result;
 * 
 * 
 * StringBuilder stb = new StringBuilder();
stb.Append("[");
for (int i = 0; i < n; i++)
{
    stb.Append(points[0, i] + " , ");
}
stb.Append("]");
string result = stb.ToString();
textBox1.Text = result;*/
/*int minI = 0; //номер нижней левой точки
double min = points[0, 0];
// ищем нижнюю левую точку
for (var i = 1; i < n; i++)
{
    if (points[0, i] < min)
    {
        min = points[0, i];
        minI = i;
    }
}
// делаем нижнюю левую точку активной
ch[0] = minI;
ch[minI] = 0;
int temp;
double x1;
double y1;
double x2;
double y2;
// сортируем вершины в порядке "левизны"
for (var i = 1; i < minI - 1; i++)
{
    for (var j = i + 1; j < minI; j++)
    {
        x1 = points[0, ch[0]];
        y2 = points[1, ch[i]];
        x2 = points[0, ch[i]];
        y1 = points[1, ch[0]];
        double cl = (x2 - x1) * (points[1, ch[j]] - y1) - (y2 - y1) * (points[0, ch[j]] - x1); // векторное произведение.
        // если векторное произведение меньше 0, следовательно вершина j левее вершины i.Меняем их местами
        if (cl < 0)
        {
            temp = ch[i];
            ch[i] = ch[j];
            ch[j] = temp;
        }
    }
    //записываем в стек вершины, которые точно входят в оболочку
    Stack<double> h = new Stack<double>();
    h.Push(ch[0]);
    h.Push(ch[1]);

    x1 = points[0, ch[0]];
    y2 = points[1, ch[i]];
    x2 = points[0, ch[i]];
    y1 = points[1, ch[0]];

    for (int i = 2; i < minI; i++)
    {
        while ()
        {
            h.Pop(); // пока встречается правый поворот, убираем точку из оболочки
        }
        h.Push(ch[i]); // добавляем новую точку в оболочку
    }*/
