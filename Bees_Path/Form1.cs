using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;  //alon
using System.Windows.Forms;

namespace Bees_Path
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<PictureBox> m_bees;

        private void buttonRun_Click(object sender, EventArgs e)
        {
			//alon: 
			// creates new instance of m_bees list as a List of type PictureBox
            if (m_bees == null)
                m_bees = new List<PictureBox>(1000);

			// alon:
			// protect from restarting another time if not stopped
            if (simStart)
                return;         //dont run another thread
			//	
            simStart = true; 
            simStop = false;
            //int timeLine = 0;
            int startedBees = Convert.ToInt32(numericUpDown_Bees.Value);
            //int spriteX = 0;
            //int spriteY = 0;
            int iCrashedCnt = 0;
            int iMaxSimTime = 0;

            iMaxSimTime = Convert.ToInt32(TimeLineCounter.Value);

            System.Drawing.Point newPoint = new System.Drawing.Point(362, 300);

            c_AirSpace airSpace = new c_AirSpace(getInitValue_X(), getInitValue_Y(), getInitValue_H(), getInitValue_Bees(), getInitValue_CollDist(), getInitValue_commDist(), getInitValue_commMode());
            //MessageBox.Show("Start Simulation!!!");
            // temp initBeeDisplay(airSpace);            
            //spriteX = 300;// Convert.ToInt32(getInitValue_X());
            //spriteY = 300;// Convert.ToInt32(getInitValue_Y());
            double scaleX = getInitValue_X() / (pictureBox1.Size.Width - pictureBox1.Location.X);
            double scaleY = getInitValue_Y() / (pictureBox1.Size.Height - pictureBox1.Location.Y);

            //for (int b = 1; b <= startedBees; b++)
            //{
                //this.Controls.Add(new Control airSpace.bees[b].beeBox);
            //    this.Controls.Add(airSpace.bees[b].beeBox);
           // }
		   // alon: run the algo thread
            Task.Factory.StartNew(() => RunAlgo(startedBees, iCrashedCnt, iMaxSimTime, airSpace));


        }

        private void RunAlgo(int startedBees, int iCrashedCnt, int iMaxSimTime, c_AirSpace airSpace)
        {
            for (int timeLine = 0; timeLine < iMaxSimTime; timeLine++)
            {
                if (simStop)
                    break;
                // wait for a second
                System.Threading.Thread.Sleep(40);
                //pictureBox1.Update();

                // replace with TImer so the GUI will responsive

                //continue the loop
                //timeLine++;

                // Display the time in the display box
				// alon: collect update counters in a function
                UpdateCounter(iCrashedCnt, airSpace, timeLine);

                //airSpace.updateBees();

                //spriteX = Convert.ToInt32(airSpace.bee1DispPoint.X / scaleX);
                //spriteY = Convert.ToInt32(airSpace.bee1DispPoint.Y / scaleY);
                //newPoint.X = newPoint.X +1;
                //newPoint.Y = newPoint.Y + 1;
                //updateSpriteLocation(newPoint);
                airSpace.updateBees();
                if (timeLine > 10) { iCrashedCnt = iCrashedCnt + airSpace.checkCollision(); }
                updateBeesDisplay(airSpace);
                //<temp>for (int k = 0; k < m_bees.Count; k++)
                //<temp>{
                //<temp>    //updateBeesDisplay(airSpace, airSpace.bees[k]);
                //<temp>    updateBeesDisplay(airSpace, k);
                //<temp>}

            }
            //so another start will be available
            simStart = false;
            simStop = true;

        }

        private void UpdateCounter(int iCrashedCnt, c_AirSpace airSpace, int timeLine)
        {
            if (!InvokeRequired)
            {
                TimeLineCounter.Value = timeLine;
                TimeLineCounter.Update();
                numericUpDownX.Value = Convert.ToInt32(airSpace.bees[1].getCurrentLoc().getX());
                numericUpDownY.Value = Convert.ToInt32(airSpace.bees[1].getCurrentLoc().getY());
                numericUpDownH.Value = Convert.ToInt32(airSpace.bees[1].getCurrentLoc().getH());
                numericUpDownX.Update();
                numericUpDownY.Update();
                numericUpDownH.Update();
                numericUpDownX1.Value = Convert.ToInt32(airSpace.bees[2].getCurrentLoc().getX());
                numericUpDownY1.Value = Convert.ToInt32(airSpace.bees[2].getCurrentLoc().getY());
                numericUpDownH1.Value = Convert.ToInt32(airSpace.bees[2].getCurrentLoc().getH());
                numericUpDownX1.Update();
                numericUpDownY1.Update();
                numericUpDownH1.Update();

                // update Crashed counter
                numericUpDown_CrashedCnt.Value = iCrashedCnt;
                numericUpDown_CrashedCnt.Update();
            }
            else
                BeginInvoke(new Action<int, c_AirSpace, int>(UpdateCounter), iCrashedCnt,  airSpace,  timeLine);
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            simStart = false;
            simStop = true;
			// alon: clear the picture box
            foreach(PictureBox pb in m_bees)
                this.Controls.Remove(pb);
            m_bees.Clear();

        }

        private void process1_Exited(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown_CrashedCnt_ValueChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

    }
}
