﻿using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multiplication_Tests
{
    public partial class formMain : Form
    {
        static int timerTickCounter = 0;
        static Random randomizer = new Random();

        public formMain()
        {
            InitializeComponent();
        }

        private void formMain_Load(object sender, EventArgs e)
        {
            textBoxMinutes.Enabled = true;
            textBoxMinutes.Focus();
            textBoxMinutes.Text = "1";
            textBoxResult.Enabled = false;
            this.AcceptButton = buttonStart;
            labelSymbol.Text = "x";
            if (!Directory.Exists("d:\\log"))
            {
                Directory.CreateDirectory("d:\\log");
            }

            if (File.Exists("d:\\log\\乘法测试" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + ".log"))
            {
                Log("测试开始!", "d:\\log\\", "乘法测试" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + ".log");
            }
            else
            {
                FileStream newLogFile = new FileStream("d:\\log\\乘法测试" + 
                                                        DateTime.Now.Year.ToString() + "-" + 
                                                        DateTime.Now.Month.ToString() + "-" + 
                                                        DateTime.Now.Day.ToString() + ".log", 
                                                        FileMode.OpenOrCreate);
                newLogFile.Close();
                Log("测试开始!", "d:\\log\\", "乘法测试" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + ".log");
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            int intMinutes = 0;
            string strRemainMinutes = "";
            string strRemainSeconds = "";
            bool isNumber = int.TryParse(textBoxMinutes.Text, out intMinutes);

            timerTickCounter = timerTickCounter + 1;

            int intRemainMinutes = (intMinutes * 60 - timerTickCounter) / 60;
            int intRemainSeconds = (intMinutes * 60 - timerTickCounter) % 60;
            if (intRemainMinutes < 10)
            {
                strRemainMinutes = "0" + intRemainMinutes.ToString();
            }
            else
            {
                strRemainMinutes = intRemainMinutes.ToString();
            }
            if (intRemainSeconds < 10)
            {
                strRemainSeconds = "0" + intRemainSeconds.ToString();
            }
            else
            {
                strRemainSeconds = intRemainSeconds.ToString();
            }

            labelTime.Text = strRemainMinutes + ":" + strRemainSeconds;

            if (timerTickCounter / 60 == intMinutes)
            {
                timer.Stop();
                timerTickCounter = 0;           //停止计时器，并将激活次数清零
                textBoxMinutes.Enabled = true;
                textBoxResult.Enabled = false;
                buttonStart.Enabled = true;
                textBoxMinutes.Focus();
                Log(textBoxMinutes.Text + "分钟乘法测试完成，共计" + labelScore.Text + "题.",
                    "d:\\log\\",
                    "乘法测试" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + ".log");
            }

        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            int intMinutes = 0;
            if (int.TryParse(textBoxMinutes.Text, out intMinutes))
            {
                labelTime.Text = intMinutes.ToString() + ":00";
                timer.Interval = 1000;
                timer.Start();
                textBoxMinutes.Enabled = false;
                textBoxResult.Enabled = true;
                textBoxResult.Text = "";
                textBoxResult.Focus();
                labelScore.Text = "0";
                buttonStart.Enabled = false;
                NewQuiz();
            }
        }

        private void textBoxMinutes_TextChanged(object sender, EventArgs e)
        {
            int intMinutes = 0;
            if (int.TryParse(textBoxMinutes.Text, out intMinutes))
            {
                labelTime.Text = intMinutes.ToString() + ":00";
            }
        }

        private void textBoxResult_TextChanged(object sender, EventArgs e)
        {
            int intNumber1, intNumber2, intResult, intScore, intAnswer;
            if (int.TryParse(textBoxResult.Text, out intResult))
            {
                intNumber1 = System.Convert.ToInt32(labelNumber1.Text);
                intNumber2 = System.Convert.ToInt32(labelNumber2.Text);
                intAnswer = intNumber1 * intNumber2;
                if (intAnswer == intResult)
                {
                    intScore = System.Convert.ToInt32(labelScore.Text);
                    intScore = intScore + 1;
                    labelScore.Text = intScore.ToString();
                    textBoxResult.Text = "";
                    NewQuiz();
                }
            }
        }

        private void NewQuiz()
        {
            int intNumber1, intNumber2;

            //随机生成2个1-9的整数
            intNumber1 = randomizer.Next(9) + 1;
            intNumber2 = randomizer.Next(9) + 1;
            if (intNumber1 < intNumber2)
            {
                labelNumber1.Text = intNumber1.ToString();
                labelNumber2.Text = intNumber2.ToString();
            }
            else
            {
                labelNumber1.Text = intNumber2.ToString();
                labelNumber2.Text = intNumber1.ToString();
            }
        }

        static void Log(string logMessage, string logFolder, string logFilename)
        {
            using (StreamWriter w = File.AppendText(logFolder + logFilename))
            {
                w.Write("\r\n");
                w.Write("{0} {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
                w.Write("  :{0}", logMessage);
            }
        }
    }
}
