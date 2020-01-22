using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimplexNoise;

namespace MapMaker
{
    public partial class UserControl1: UserControl
    {
        private float[,] heightMap;
        private float[,] moistureMap;
        private int seed = 0;
        private Bitmap mask;
        public UserControl1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void genMap()
        {
            heightMap = applyMask(GenerateSimplexNoise(seed));
            moistureMap = applyMask(GenerateSimplexNoise(seed + 1));
            drawHeightMap(heightMap);
        }
        int length = 200, width = 200;
  
        private float[,] GenerateSimplexNoise(int seed)
        {

            SimplexNoise.Noise.Seed = seed;
            float[,] noise = new float[width, length];
            float[,] noiseValues = SimplexNoise.Noise.Calc2D(width, length, (float)numericUpDown8.Value * (float)numericUpDown1.Value);
            float[,] noiseValues1 = SimplexNoise.Noise.Calc2D(width, length, (float)numericUpDown7.Value * (float)numericUpDown1.Value);
            float[,] noiseValues2 = SimplexNoise.Noise.Calc2D(width, length, (float)numericUpDown6.Value * (float)numericUpDown1.Value);
            for (int x = 0; x < width;x++)
            {
                for (int y = 0; y < length; y++)
                {
                    noise[x, y] = noiseValues[x, y] * (float)numericUpDown2.Value + noiseValues1[x, y] * (float)numericUpDown4.Value + noiseValues2[x, y] * (float)numericUpDown3.Value;
                }
            }
            return noise;
        }
        private float[,] applyMask(float[,] noise)
        {
            mask = new Bitmap(Image.FromFile("C:/Users/chris/Downloads/mask_circular.png"));
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < length; y++)
                {
                    noise[x, y] = noise[x, y] * mask.GetPixel(x*(mask.Width/width), y * (mask.Height / length)).GetBrightness();
                }
            }
            return noise;
        }

        private void drawHeightMap(float[,] heightMap)
        {
            Bitmap map = new Bitmap(width, length);

            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < length;y++)
                {
                    Color color;
                    if (heightMap[x, y] < 10)
                    {
                        color = Color.DarkBlue;
                    }else
                    if (heightMap[x,y] < 20)
                    {
                        color = Color.Blue;
                    }
                    else if (heightMap[x, y] < 30)
                    {
                        color = Color.SandyBrown;
                    }

                    else if(heightMap[x, y] < 40)
                    {
                        color = Color.RosyBrown;
                    }
                    else if (heightMap[x, y] < 80)
                    {
                        if (moistureMap[x, y] < 50)
                            color = Color.Brown;
                        else
                        color = Color.DarkGreen;
                    }
                    else if (heightMap[x, y] < 200)
                    {
                        if (moistureMap[x, y] > 50 && moistureMap[x, y] < 150)
                            color = Color.HotPink;
                        else
                        color = Color.Green;
                    }
                    else if (heightMap[x, y] < 240)
                    {
                        color = Color.LightGray;
                    }
                    else
                    {
                        color = Color.White;
                    }

                    if(Convert.ToInt32(heightMap[x, y] / 3) + 100<255)
                    color = Color.FromArgb(Convert.ToInt32(heightMap[x, y]/3) + 100, color);
                    else
                        color = Color.FromArgb(255, color);

                    map.SetPixel(x, y, color);
                }
            }
            pictureBox1.Image = map;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            genMap();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            genMap();
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            genMap();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            genMap();
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            seed = (int)numericUpDown5.Value;
            genMap();
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            genMap();
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            genMap();
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            genMap();
        }

        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            genMap();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            heightMap = new float[length, width];
            moistureMap = new float[length, width];

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
        }
    }
}


