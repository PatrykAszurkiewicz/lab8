using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private int width = 0, height = 0;
        Bitmap bitmap;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bitmap = (Bitmap)Bitmap.FromFile(openFileDialog1.FileName);
                width = bitmap.Width;
                height = bitmap.Height;
                pictureBox1.Image = bitmap;
            }
        }
        private void modyfikuj(double[,] M)
        {
            double pomoc_r = 0;
            double pomoc_g = 0;
            double pomoc_b = 0;
            Bitmap bitmap2 = (Bitmap)bitmap.Clone();

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    pomoc_r = 0;
                    pomoc_g = 0;
                    pomoc_b = 0;
                    for (int k = -1; k <= 1; k++)
                    {
                        for (int l = -1; l <= 1; l++)
                        {
                            int x1 = i + k;
                            int y1 = j + l;
                            if (x1 < 0)
                            {
                                x1 = 0;
                            }
                            if (x1 >= height)
                            {
                                x1 = height - 1;
                            }
                            if (y1 < 0)
                            {
                                y1 = 0;
                            }
                            if (y1 >= width)
                            {
                                y1 = width - 1;
                            }
                            Color p = bitmap.GetPixel(y1, x1);
                            int r = p.R;
                            int g = p.G;
                            int b = p.B;
                            pomoc_r += r * M[k + 1, l + 1];
                            pomoc_g += g * M[k + 1, l + 1];
                            pomoc_b += b * M[k + 1, l + 1];
                        }
                    }

                    double r1 = pomoc_r;
                    double g1 = pomoc_g;
                    double b1 = pomoc_b;

                    if (r1 > 255)
                    {
                        r1 = 255;
                    }
                    if (r1 < 0)
                    {
                        r1 = 0;
                    }
                    if (g1 > 255)
                    {
                        g1 = 255;
                    }
                    if (g1 < 0)
                    {
                        g1 = 0;
                    }
                    if (b1 > 255)
                    {
                        b1 = 255;
                    }
                    if (b1 < 0)
                    {
                        b1 = 0;
                    }

                    bitmap2.SetPixel(j, i, Color.FromArgb((int)r1, (int)g1, (int)b1));
                }
            }
            pictureBox2.Image = bitmap2;
        }

        private void max()
        {
            int radius = (int)numericUpDown1.Value;
            Bitmap bitmap2 = (Bitmap)bitmap.Clone();

            for (int x = 0; x < width - 1; ++x)
            {
                int x0 = Math.Max(1, x - radius);
                int x1 = Math.Min(width - 1, x + radius);
                for (int y = 0; y < height - 1; ++y)
                {
                    int y0 = Math.Max(0, y - radius);
                    int y1 = Math.Min(height - 1, y + radius);
                    Color max_pixel = bitmap.GetPixel(x, y);
                    for (int u = x0; u < x1; ++u)
                    {
                        for (int v = y0; v < y1; ++v)
                        {
                            if (bitmap.GetPixel(u, v).R > max_pixel.R & bitmap.GetPixel(u, v).G > max_pixel.G & bitmap.GetPixel(u, v).B > max_pixel.B)
                            {
                                max_pixel = bitmap.GetPixel(u, v);
                            }
                        }
                    }
                    bitmap2.SetPixel(x, y, max_pixel);
                }
            }
            pictureBox2.Image = bitmap2;
        }

        private void min()
        {
            int radius = (int)numericUpDown1.Value;
            Bitmap bitmap2 = (Bitmap)bitmap.Clone();

            for (int x = 0; x < width - 1; ++x)
            {
                int x0 = Math.Max(1, x - radius);
                int x1 = Math.Min(width - 1, x + radius);
                for (int y = 0; y < height - 1; ++y)
                {
                    int y0 = Math.Max(0, y - radius);
                    int y1 = Math.Min(height - 1, y + radius);
                    Color max_pixel = bitmap.GetPixel(x, y);
                    for (int u = x0; u < x1; ++u)
                    {
                        for (int v = y0; v < y1; ++v)
                        {
                            if (bitmap.GetPixel(u, v).R < max_pixel.R & bitmap.GetPixel(u, v).G < max_pixel.G & bitmap.GetPixel(u, v).B < max_pixel.B)
                            {
                                max_pixel = bitmap.GetPixel(u, v);
                            }
                        }
                    }
                    bitmap2.SetPixel(x, y, max_pixel);
                }
            }
            pictureBox2.Image = bitmap2;
        }

        private void median()
        {

            int radius = (int)numericUpDown1.Value;
            Color[] matrix = new Color[((2 * radius) + 1) * ((2 * radius) + 1)];
            Bitmap bitmap2 = (Bitmap)bitmap.Clone();
            int index = 0;
            for (int x = 0; x < width - 1; ++x)
            {
                int x0 = Math.Max(1, x - radius);
                int x1 = Math.Min(width - 1, x + radius);
                for (int y = 0; y < height - 1; ++y)
                {
                    int y0 = Math.Max(0, y - radius);
                    int y1 = Math.Min(height - 1, y + radius);
                    Color max_pixel = bitmap.GetPixel(x, y);
                    for (int u = x0; u <= x1; ++u)
                    {
                        for (int v = y0; v <= y1; ++v)
                        {
                            matrix[index] = bitmap.GetPixel(u, v);
                            index++;
                        }
                    }
                    index = 0;
                    int ind = Median(matrix);
                    bitmap2.SetPixel(x, y, matrix[ind]);
                    Array.Clear(matrix, 0, matrix.Length);
                }
            }
            pictureBox2.Image = bitmap2;
        }

        int Median(Color[] xs)
        {
            Color[] x2 = new Color[xs.Length];
            SortedList mySL = new SortedList();
            for (int i = 0; i < xs.Length; i++)
            {
                mySL.Add(i, 0.21 * xs[i].R + 0.72 * xs[i].G + 0.07 * xs[i].B);
            }
            double[] arr = new double[xs.Length];
            for (int i = 0; i < xs.Length; i++)
            {
                arr[i] = 0.21 * xs[i].R + 0.72 * xs[i].G + 0.07 * xs[i].B;
            }
            Array.Sort(arr);
            double v = arr[arr.Length / 2];
            int wynik = 0;
            for (int i = 0; i < xs.Length; i++)
            {
                if (v == 0.21 * xs[i].R + 0.72 * xs[i].G + 0.07 * xs[i].B)
                {
                    wynik = i;
                }
            }
            return wynik;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                modyfikuj(new double[,] { { 0, 0, 0 }, { 0, 1, -1 }, { 0, 0, 0 } });
            }
            else if (radioButton2.Checked)
            {
                modyfikuj(new double[,] { { 0, 0, 0 }, { 0, 1, 0 }, { 0, -1, 0 } });
            }
            else if (radioButton3.Checked)
            {
                modyfikuj(new double[,] { { 1, 1, 1 }, { 0, 0, 0 }, { -1, -1, -1 } });
            }
            else if (radioButton4.Checked)
            {
                modyfikuj(new double[,] { { 1, 0, -1 }, { 1, 0, -1 }, { 1, 0, -1 } });
            }
            else if (radioButton5.Checked)
            {
                modyfikuj(new double[,] { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } });
            }
            else if (radioButton6.Checked)
            {
                modyfikuj(new double[,] { { 1, 0, -1 }, { 2, 0, -2 }, { 1, 0, -1 } });
            }
            else if (radioButton7.Checked)
            {
                modyfikuj(new double[,] { { 0, -1, 0 }, { -1, 4, -1 }, { 0, -1, 0 } });
            }
            else if (radioButton8.Checked)
            {
                max();
            }
            else if (radioButton9.Checked)
            {
                min();
            }
            else if (radioButton10.Checked)
            {
                median();
            }
        }
    }
}
