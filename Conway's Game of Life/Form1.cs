using System;
using System.Drawing;
using System.Windows.Forms;

namespace Conway_s_Game_of_Life
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static int o = 7, l = 15;
        int w, h, mw, mh, state, step, ok = 0;
        bool[,] mat, matv;
        Bitmap bm;
        Graphics g;

        private void OnLoad(object sender, EventArgs e)
        {
            w = this.Width;
            h = this.Height;
            mw = w / l + 2 * o;
            mh = h / l + 2 * o;

            bm = new Bitmap(w, h);
            mat = new bool[mw, mh];
            matv = new bool[mw, mh];

            Init();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            if(ok == 0)
            {
                g = this.CreateGraphics();
                Redraw();
                if (state == 0)
                {
                    stepLabel.Text = "STOPPED, STEP " + step;
                }
                else
                {
                    stepLabel.Text = "RUNNING, STEP " + step;
                }
                ok = 1;
            }
        }

        private void OnTick(object sender, EventArgs e)
        {
            int i, j;
            NextStep();
            for(i = 0; i < mw; i++)
            {
                for(j = 0; j < mh; j++)
                {
                    if (mat[i, j] != matv[i, j])
                    {
                        if (i - o >= 0 && i + o < mw && j - o >= 0 && j + o < mh)
                        {
                            SetCellInBmp(i, j, mat[i, j]);
                        }
                    }
                }
            }
            Redraw();
            step++;
            stepLabel.Text = "RUNNING, STEP " + step;
        }

        private void OnClick(object sender, MouseEventArgs e)
        {
            int x, y, i, j;
            if(state == 0)
            {
                x = e.X;
                y = e.Y;
                i = o + x / l;
                j = o + y / l;
                mat[i, j] = !mat[i, j];
                SetCellInBmp(i, j, mat[i, j]);
                Redraw();
            }
        }

        private void OnStart(object sender, EventArgs e)
        {
            state = 1;
            timer.Interval = 2000 / trackBar1.Value;
            timer.Enabled = true;
            stepLabel.Text = "RUNNING, STEP " + step;
        }

        private void OnStop(object sender, EventArgs e)
        {
            timer.Enabled = false;
            state = 0;
            stepLabel.Text = "STOPPED, STEP " + step;
        }

        private void OnNext(object sender, EventArgs e)
        {
            int i, j;
            if(state == 0)
            {
                NextStep();
                for (i = 0; i < mw; i++)
                {
                    for (j = 0; j < mh; j++)
                    {
                        if (mat[i, j] != matv[i, j])
                        {
                            if (i - o >= 0 && i + o < mw && j - o >= 0 && j + o < mh)
                            {
                                SetCellInBmp(i, j, mat[i, j]);
                            }
                        }
                    }
                }
                Redraw();
                step++;
                stepLabel.Text = "STOPPED, STEP " + step;

            }
        }

        private void OnClear(object sender, EventArgs e)
        {
            Init();
            Redraw();
            stepLabel.Text = "STOPPED, STEP " + step;
        }

        private void OnSpeedChange(object sender, EventArgs e)
        {
            timer.Interval = 2000 / trackBar1.Value;
        }

        private void Init()
        {
            int i, j;
            state = 0; // 0 - stopped; 1 - running
            step = 0;
            timer.Enabled = false;

            for (i = 0; i < mw; i++)
            {
                for (j = 0; j < mh; j++)
                {
                    mat[i, j] = false;
                }
            }

            for (i = 0; i < w; i++)
            {
                for (j = 0; j < h; j++)
                {
                    if (j % l == 0 || (j % l != 0 && i % l == 0))
                    {
                        bm.SetPixel(i, j, Color.Gray);
                    }
                    else
                    {
                        bm.SetPixel(i, j, Color.White);
                    }
                }
            }
        }

        private void NextStep()
        {
            int i, j, nr;

            for (i = 0; i < mw; i++)
            {
                for (j = 0; j < mh; j++)
                {
                    matv[i, j] = mat[i, j];
                }
            }

            for (i = 1; i < mw - 1; i++)
            {
                for (j = 1; j < mh - 1; j++)
                {
                    nr = 0;
                    if (matv[i - 1, j - 1] == true)
                        nr++;
                    if (matv[i - 1, j] == true)
                        nr++;
                    if (matv[i - 1, j + 1] == true)
                        nr++;
                    if (matv[i, j - 1] == true)
                        nr++;
                    if (matv[i, j + 1] == true)
                        nr++;
                    if (matv[i + 1, j - 1] == true)
                        nr++;
                    if (matv[i + 1, j] == true)
                        nr++;
                    if (matv[i + 1, j + 1] == true)
                        nr++;

                    if(matv[i, j] == true)
                    {
                        if (nr == 2 || nr == 3)
                        {
                            mat[i, j] = true;
                        }
                        else
                        {
                            mat[i, j] = false;
                        }
                    }
                    else
                    {
                        if (nr == 3)
                        {
                            mat[i, j] = true;
                        }
                    }
                }
            }
        }

        private void SetCellInBmp(int x, int y, bool k)
        {
            int i, j;
            for(i = 1; i < l; i++)
            {
                for(j = 1; j < l; j++)
                {
                    if(k == true)
                    {
                        if (x - o >= 0 && x + o < mw && y - o >= 0 && y + o < mh)
                        {
                            bm.SetPixel(l * (x - o) + i, l * (y - o) + j, Color.Black);
                        }
                    }
                    else
                    {
                        if (x - o >= 0 && x + o < mw && y - o >= 0 && y + o < mh)
                        {
                            bm.SetPixel(l * (x - o) + i, l * (y - o) + j, Color.White);
                        }
                    }
                }
            }
        }

        private void Redraw()
        {
            g.DrawImage(bm, 0, 0);
        }
    }
}
