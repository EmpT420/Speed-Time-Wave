using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Tutorial10
{
    public partial class Form1 : Form
    {
        protected string max;
        public Form1()
        {
            InitializeComponent();
        }

        private void openWaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Wave File (*.wav)|*.wav;";
            if (open.ShowDialog() != DialogResult.OK) return;

            waveViewer1.SamplesPerPixel = 450;
            waveViewer1.WaveStream = new NAudio.Wave.WaveFileReader(open.FileName);

            chart1.Series.Add("wave");
            chart1.Series["wave"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            chart1.Series["wave"].ChartArea = "ChartArea1";

            NAudio.Wave.WaveChannel32 wave = new NAudio.Wave.WaveChannel32(new NAudio.Wave.WaveFileReader(open.FileName));

            byte[] buffer = new byte[16384];
            int read = 0;

            while (wave.Position < wave.Length)
            {
                read = wave.Read(buffer, 0, 16384);

                for (int i = 0; i < read / 4; i++)
                {
                    chart1.Series["wave"].Points.Add(BitConverter.ToSingle(buffer, i * 4));
                }
            }
            max = chart1.Series["wave"].Points.FindMaxByValue().ToString();
            max = max.Substring(max.IndexOf("Y=")+2);
            max = max.Remove(max.Length - 1, 1);
            label2.Text = max;
            label3.Text = GetSoundLength(open.FileName).ToString();
            label4.Text = ((18 / (GetSoundLength(open.FileName)/ 1000))*3.6).ToString();
            label10.Text = System.IO.Path.GetFileName(open.FileName);
            //label4.Text =((float.Parse(max)*100) * (float.Parse(GetSoundLength(open.FileName).ToString()))/1000).ToString();
        }

            [DllImport("winmm.dll")]
            private static extern uint mciSendString(
                string command,
                StringBuilder returnValue,
                int returnLength,
                IntPtr winHandle);

            public static int GetSoundLength(string fileName)
            {
                StringBuilder lengthBuf = new StringBuilder(32);

                mciSendString(string.Format("open \"{0}\" type waveaudio alias wave", fileName), null, 0, IntPtr.Zero);
                mciSendString("status wave length", lengthBuf, lengthBuf.Capacity, IntPtr.Zero);
                mciSendString("close wave", null, 0, IntPtr.Zero);

                int length = 0;
                int.TryParse(lengthBuf.ToString(), out length);

                return length;
            }



        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
