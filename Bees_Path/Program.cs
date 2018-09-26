using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace Bees_Path
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application - start here.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            Form1 frm = new Form1();
            Application.Run(frm);

        }
    }// end: class Program

    //===========================
    // Class: AirSpace
    //===========================
    public class c_AirSpace
    {
        //public int xMax;
        //public int yMax;
        //public int hMax;
        public int beesNum;
        public c_point3D maxLimits = new c_point3D();

        //private int launchedBees;
        public Bee[] bees = new Bee[1000];
        //public Bee bee1 = new Bee();
        //public System.Drawing.Point bee1DispPoint = new System.Drawing.Point(0, 0);        
        public double collisionDistance;
        public double commDistance;
        public int commMode;
        //public int 

        // Constructor
        public c_AirSpace(double x, double y, int h, int beesNum, double collDist, double commDist, int commModel)
        {
            //xMax = x;
            //yMax = y;
            //hMax = h;
            maxLimits.setPoint(x, y, h);
            collisionDistance = collDist;
            commDistance = commDist;
            commMode = commModel;

            this.beesNum = beesNum;
            launchBees();
            //for (int launchedBees = 1; launchedBees <= beesNum; launchedBees++)
            //{
            //    bees[launchedBees] = launchBees();
            //    //launchedBees++;
            //}

        }//end: c_AirSpace()

        // Starting new bee
        public void launchBees()
        {
            System.Random randGenerator = new System.Random();
            for (int j = 0; j < beesNum; j++)
            {
                Bee newBee = new Bee();
                int maxAmp = 20;
                //int initBeeLocation_X = 500;
                //int initBeeLocation_Y = 500;
                //System.Random randInitBeeLocation_X = new System.Random();
                //System.Random randTheta = new System.Random();
                //System.Random randAmp = new System.Random();
                //double initBeeLocation_X = randInitBeeLocation_X.NextDouble() * 20 + 500;
                //double initBeeTheta = randTheta.NextDouble() * 360;
                //double initBeeAmp = randAmp.NextDouble() * maxAmp + 1;
                double initBeeLocation_X = randGenerator.NextDouble() * maxLimits.getX();
                double initBeeLocation_Y = randGenerator.NextDouble() * maxLimits.getY();
                int initBeeLocation_H = Convert.ToInt32(randGenerator.NextDouble() * maxLimits.getH());
                double initBeeTheta = randGenerator.NextDouble() * 360;
                double initBeeAmp = randGenerator.NextDouble() * maxAmp + 1;
                //bees[++launchedBees].initAirSpaceLimits(xMax, yMax, hMax);
                //bees[launchedBees].initBeeLocation(initBeeLocation_X, initBeeLocation_Y, initBeeLocation_H, initBeeAmp, initBeeTheta);
                newBee.initAirSpaceLimits(maxLimits.getX(), maxLimits.getY(), maxLimits.getH());
                newBee.initBeeLocation(initBeeLocation_X, initBeeLocation_Y, initBeeLocation_H, initBeeTheta, initBeeAmp);
                bees[j] = newBee; // add to list!!!!!
               // return newBee;
            }
        }

        public void updateBees()
        {
            for (int j = 0; j < beesNum; j++)
            {
                bees[j].calculateNewLocation();
                bees[j].updateLocation();
            }
            //bees[1].calculateNewLocation();
            //bees[1].updateLocation();

        }//end: updateBees()

        //=====================================
        // checks if there is a collided Bees - test git 1 test git 2 test-branch_a 4 > the change for develop deleted
        // and removes the crashed - test 6; >> now wotking on ver 2.0 > now working on version 3.0
        public int checkCollision()
        {
            double distance = 0.0;
            double diffX = 0.0;
            double diffY = 0.0;
            int diffH;// = Convert.ToInt32(0);
            int iCrashedCnt = 0;

            for (int i = 0; i < beesNum; i++)
            {
                for (int j = 0; j < beesNum; j++)
                {
                    if (i == j || bees[i].getCollisionState()==true || bees[j].getCollisionState()==true) { }
                    else
                    {
                      diffX = bees[i].getCurrentLoc().getX() - bees[j].getCurrentLoc().getX();
                      diffY = bees[i].getCurrentLoc().getY() - bees[j].getCurrentLoc().getY();
                      diffH = bees[i].getCurrentLoc().getH() - bees[j].getCurrentLoc().getH();
                        
                      distance = Math.Sqrt((diffX * diffX) + (diffY * diffY));
                      if (distance <= collisionDistance && diffH == 0)
                      {
                          bees[i].setBeeCrashed(); iCrashedCnt++;
                          bees[j].setBeeCrashed(); iCrashedCnt++;
                      }
                    }
                }
            }
            return iCrashedCnt;
        } // end checkCollision()

        //========================
        // Prevent Collision
        // check that the two bees are getting too close and send one of them to higher path
        // the hier index will be sent to the higher path
        // each beecommunicates with the close ones and change its path
        public void preventCollision()
        {
            double distance = 0.0;
            double diffX = 0.0;
            double diffY = 0.0;
            int diffH;// = Convert.ToInt32(0);
            int HofMax = 0;
            int HofMin = 0;

            for (int i = 0; i < beesNum; i++)
            {
                for (int j = 0; j < beesNum; j++)
                {
                    if (i == j || bees[i].getCollisionState() == true || bees[j].getCollisionState() == true) { }
                    else
                    {
                        diffX = bees[i].getCurrentLoc().getX() - bees[j].getCurrentLoc().getX();
                        diffY = bees[i].getCurrentLoc().getY() - bees[j].getCurrentLoc().getY();
                        diffH = bees[i].getCurrentLoc().getH() - bees[j].getCurrentLoc().getH();

                        distance = Math.Sqrt((diffX * diffX) + (diffY * diffY));
                        if (distance <= commDistance && diffH == 0)
                        {
                            HofMax = bees[Math.Max(i, j)].getCurrentLoc().getH();
                            HofMin = bees[Math.Min(i, j)].getCurrentLoc().getH();
                            if (HofMax < maxLimits.getH())
                                bees[Math.Max(i, j)].setNew_H(HofMax + 1);
                            
                            if (HofMin > 0)
                                bees[Math.Min(i, j)].setNew_H(HofMax - 1);

                        }
                    }
                }
            }
        } // end preventCollision()


    } // end c_AirSpace

    //===========================
    // Class: c_point3D
    //===========================

    public class c_point3D
    {
        double X;
        double Y;
        int H;

        public c_point3D() 
        {
        }

        public double getX()
        {
            return X;
        }

        public double getY()
        {
            return Y;
        }

        public int getH()
        {
            return H;
        }

        public void setPoint(double x, double y, int h)
        {
            this.X = x;
            this.Y = y;
            this.H = h;
        }
    } // end: c_point3D

    //===========================
    // Class: dynamics
    //===========================

    public class c_dynamics
    {
        private double velocityThetaRad;
        private double velocityThetaDeg;
        private double velocityAmp;
        private double velocityX;
        private double velocityY;

        public c_dynamics()
        {
        }

        public void setDynamic(double theta, double amp)
        {
            velocityThetaDeg = theta;
            velocityThetaRad = deg2rad(theta);
            velocityAmp = amp;
        }

        double deg2rad(double deg)
        {
            return deg * 2 * Math.PI / 360;
        }

        double rad2deg(double rad)
        {
            return rad * 360 / (2 * Math.PI);
        }

        double Polar2X(double theta, double amp)
        {
            return amp * Math.Cos(deg2rad(theta));
       
        }

        double Polar2Y(double theta, double amp)
        {
            return amp * Math.Sin(deg2rad(theta));
        }

        double XY2theta(double x, double y)
        {
            if (x > 0 && (y >= 0 )) { return Math.Atan(y / x); } else
            if (x < 0 && y >= 0)    { return (Math.PI/2) + Math.Atan(y / (-1*x)); } else
            if (x > 0 && y <= 0)    { return (2 * Math.PI) - Math.Atan((-1 * y) / x); } else
            if (x < 0 && y <= 0)    { return (Math.PI) + Math.Atan((-1 * y) / (-1 * x)); } else
            if (x == 0 && (y >= 0)) { return Math.PI / 2; } else
            if (x == 0 && y <= 0)   { return 3 * Math.PI / 2; }
            else
            { return 0; }

        }

        double XY2amp(double x, double y)
        {
            return Math.Sqrt(x * x + y * y);
        }

        public double xAdvance()
        {
            return velocityAmp * Math.Cos(velocityThetaRad);
        }

        public double yAdvance()
        {
            return velocityAmp * Math.Sin(velocityThetaRad);
        }

        public void flipOnEast()
        {
            velocityThetaRad = (XY2theta((-1) * Polar2X(velocityThetaDeg, velocityAmp), Polar2Y(velocityThetaDeg, velocityAmp)));
            velocityThetaDeg = rad2deg(velocityThetaRad);
        }

        public void flipOnWest()
        {
            velocityThetaRad = (XY2theta((-1) * Polar2X(velocityThetaDeg, velocityAmp), Polar2Y(velocityThetaDeg, velocityAmp)));
            velocityThetaDeg = rad2deg(velocityThetaRad);
        }

        public void flipOnNorth()
        {
            velocityThetaRad = (XY2theta(Polar2X(velocityThetaDeg, velocityAmp), (-1) * Polar2Y(velocityThetaDeg, velocityAmp)));
            velocityThetaDeg = rad2deg(velocityThetaRad);
        }
        public void flipOnSouth()
        {
            velocityThetaRad = (XY2theta(Polar2X(velocityThetaDeg, velocityAmp), (-1) * Polar2Y(velocityThetaDeg, velocityAmp)));
            velocityThetaDeg = rad2deg(velocityThetaRad);
        }


    } // end: public class dynamics


    //===========================
    // Class: Bee
    //===========================
    public class Bee
    {
        // store the airspace limitations in each Bee class
        c_point3D MaxLimits = new c_point3D();
        c_point3D currentLoc = new c_point3D();
        c_point3D nextLoc = new c_point3D();
        Boolean collisionState;
        public System.Windows.Forms.PictureBox beeBox;

        //int Max_X;
        //int Max_Y;
        //int Max_H;

        // Currrent location in space
        //double currentLocX;
        //double currentLocY;
        //double currentLocH;

        // the next location after calculation
        //double nextLocX;
        //double nextLocY;
        //double nextLocH;
        // the dynamics
        c_dynamics dynamics = new c_dynamics();

        //double velocityTheta;
        //double velocityAmp;

        
        public Bee()
        {
            MaxLimits.setPoint(0.0, 0.0, 0);
            currentLoc.setPoint(0.0, 0.0, 0);
            nextLoc.setPoint(0.0, 0.0, 0);
            dynamics.setDynamic(0.0, 0.0);
            collisionState = false;

            //Max_H = 0;
            //Max_X = 0;
            //Max_Y = 0;
            //currentLocH = 0.0;
            //currentLocX = 0.0;
            //currentLocY = 0.0;
            //nextLocH = 0.0;
            //nextLocX = 0.0;
            //nextLocY = 0.0;
            //velocityTheta = 0.0;
            //velocityAmp = 0.0;
        }

        public c_point3D getCurrentLoc()
        {
            return currentLoc;
        }//end: getCurrentLoc()

        public Boolean getCollisionState()
        {
            return collisionState;
        }//end getCollisionState()

        public void initBeeLocation(double initX, double initY, int initH, double initVelocityTheta, double initVelocityAmp)
        {
            currentLoc.setPoint(initX, initY, initH);
            dynamics.setDynamic(initVelocityTheta, initVelocityAmp);
            beeBox = new System.Windows.Forms.PictureBox();
            this.beeBox.Location = new System.Drawing.Point(362, 200);
            //this.beeBox.Name = "spriteBox";
            this.beeBox.Size = new System.Drawing.Size(10, 10);
            this.beeBox.TabIndex = 2;
            this.beeBox.TabStop = false;
            //currentLocX = Convert.ToDouble(initX);
            //currentLocY = Convert.ToDouble(initY);
            //currentLocH = Convert.ToDouble(initH);
            //velocityTheta = initVelocityTheta;
            //velocityAmp = initVelocityAmp;
        }

        public void initAirSpaceLimits(double x, double y, int h)
        {
            //Max_X = x;
            //Max_Y = y;
            //Max_H = h;
            MaxLimits.setPoint(x, y, h);
        }

        public void calculateNewLocation()
        {
            double tmpX;
            double tmpY;
            int tmpH;

            tmpH = currentLoc.getH();
            tmpX = currentLoc.getX() + dynamics.xAdvance();
            tmpY = currentLoc.getY() + dynamics.yAdvance();

            //tmpX = currentLocX + dynamics.xAdvance();
            //tmpY = currentLocY + dynamics.yAdvance();

            if (Convert.ToInt32(tmpX) >= MaxLimits.getX())
            {
                //tmpX = Max_X;
                tmpX = MaxLimits.getX();
                dynamics.flipOnEast();
            }

            if (Convert.ToInt32(tmpX) <= 0)
            {
                tmpX = 0;
                dynamics.flipOnWest();
            }

            if (Convert.ToInt32(tmpY) >= MaxLimits.getY())
            {
                tmpY = MaxLimits.getY();
                dynamics.flipOnNorth();
            }

            if (Convert.ToInt32(tmpY) <= 0)
            {
                tmpY = 0;
                dynamics.flipOnSouth();
            }
            nextLoc.setPoint(tmpX, tmpY, tmpH); 
            //nextLocX = tmpX;
            //nextLocY = tmpY;

        }//end: CalculateNewLocation()


        public void updateLocation()
        {
            currentLoc.setPoint(nextLoc.getX(), nextLoc.getY(), nextLoc.getH());
                        
        }// end: updateLocation()

        public void setNew_H(int h)
        {
            currentLoc.setPoint(currentLoc.getX(), currentLoc.getY(), h);

        }// end: updateLocation()


        public void setBeeCrashed()
        {
            nextLoc = currentLoc;
            dynamics.setDynamic(0.0, 0.0);
            collisionState = true;
        } //end setBeeCrashed()


    } // end: public class Bee



    //===========================
    // Class: Form1
    //===========================
    public partial class Form1
    {
        public Boolean simStart = false;
        public Boolean simStop = false;

        Pen penBlack = new Pen(Color.Black, 3);
        Pen penRed = new Pen(Color.Red, 3);
        Rectangle rect = new Rectangle(0, 0, 5, 5);

                
            //private System.Windows.Forms.PictureBox spriteBox1;
        //private System.Windows.Forms.PictureBox[] spriteBox1 = new System.Windows.Forms.PictureBox[100];
        //private System.Windows.Forms.PictureBox spriteBox1 = new System.Windows.Forms.PictureBox();

        public void initBeeDisplay(c_AirSpace airSpace)
        {
            // sets the init values of the LIST m_bees.
            //for (int i = 1; i <= airSpace.beesNum; i++)
            //{
            m_bees.Add(spriteBox1);
            m_bees.Add(spriteBox2);
            m_bees.Add(spriteBox3);
            m_bees.Add(spriteBox4);
            m_bees.Add(spriteBox5);
            m_bees.Add(spriteBox6);
            m_bees.Add(spriteBox7);
            m_bees.Add(spriteBox8);
            m_bees.Add(spriteBox9);
            m_bees.Add(spriteBox10);
            //}
            //System.Drawing.Point newPoint = new System.Drawing.Point(345, 300);

            //for (int i = 1; i <= this.getInitValue_Bees(); ++i)
            //{
           //     newPoint.X = 300 + 5 * i;
           //     newPoint.Y = 300;
            //    spriteBox1[i] = new System.Windows.Forms.PictureBox();
           //     spriteBox1[i].Location = newPoint;
           //     //spriteBox1[i].Location.X = 362 + 5 * i;
           //     spriteBox1[i].Name = "spriteBox";
           //     spriteBox1[i].Size = new System.Drawing.Size(10, 10);
           //     spriteBox1[i].TabIndex = 2;
           //     spriteBox1[i].TabStop = false;
           //     Controls.Add(this.spriteBox1[i]);
           //     pictureBox1.Update();
           //     spriteBox1[i].Update();
 
            //}

                 //newPoint.X = 300 + 5;
                 //newPoint.Y = 300;
                 //spriteBox1 = new System.Windows.Forms.PictureBox();
                 //spriteBox1.Location = newPoint;
                 //spriteBox1[i].Location.X = 362 + 5 * i;
                 //spriteBox1.Name = "spriteBox";
                 //spriteBox1.Size = new System.Drawing.Size(10, 10);
                 //spriteBox1.TabIndex = 2;
                 //spriteBox1.TabStop = false;
                 //this.spriteBox.Size = new System.Drawing.Size(10, 10);


                 //pictureBox1.Update();
                 //spriteBox1.Update();
                 //spriteBox2.Update();

        }

        // convert the Bee location to display point
        public void updateBeesDisplay(c_AirSpace airSpace, int beeNum)
        {
            if (InvokeRequired) //alon
                BeginInvoke(new Action<c_AirSpace , int > (updateBeesDisplay),airSpace, beeNum);
            else //alon
            {
            double zeroPointX = pictureBox1.Location.X;
            double zeroPointY = pictureBox1.Location.Y + pictureBox1.Size.Height; 
            double dispScaleX = (pictureBox1.Size.Width-10) / airSpace.maxLimits.getX();
            double dispScaleY = (pictureBox1.Size.Height+10) / airSpace.maxLimits.getY();
            double x = airSpace.bees[beeNum].getCurrentLoc().getX();
            double y = airSpace.bees[beeNum].getCurrentLoc().getY();

            int X_point = 0; X_point = Convert.ToInt32(zeroPointX + x * dispScaleX);
            int Y_point = 0; Y_point = Convert.ToInt32(zeroPointY - y * dispScaleY);

            System.Drawing.Point beeDispPoint = new System.Drawing.Point(X_point, Y_point);

            
            //this.Controls[3].Location = beeDispPoint; //airSpace.bees[beeNum].beeBox.Location = beeDispPoint;
            //this.Controls[3].Update();// airSpace.bees[beeNum].beeBox.Update();
                m_bees[beeNum].Location = beeDispPoint;
                m_bees[beeNum].Update();
                m_bees[beeNum].BringToFront();
			// my code


            //pictureBox1.Update();
            //spriteBox1.Update();
            //spriteBox2.Update();
            //spriteBox3.Update();
            //spriteBox4.Update();
            //spriteBox5.Update();
            //spriteBox6.Update();
            //spriteBox7.Update();
            //spriteBox8.Update();
            //spriteBox9.Update();
            //spriteBox10.Update();
            //pictureBox1.Update();
            //this.airSpace.bees[b].beeBox;
            }
        }//end: updateBeesDisplay()

        public void updateBeesDisplay(c_AirSpace airSpace)
        {
            if (InvokeRequired) //alon
                BeginInvoke(new Action<c_AirSpace>(updateBeesDisplay), airSpace);
            else //alon 
            {
                double zeroPointX = 0;
                double zeroPointY = 0 + pictureBox1.Size.Height;
                double dispScaleX = (pictureBox1.Size.Width) / airSpace.maxLimits.getX();
                double dispScaleY = (pictureBox1.Size.Height) / airSpace.maxLimits.getY();
                double x = 0.0;
                double y = 0.0;

                Graphics g = pictureBox1.CreateGraphics();
                int X_point = 0;
                int Y_point = 0;

               // System.Drawing.Point beeDispPoint = new System.Drawing.Point(X_point, Y_point);
                
                // update all bees in the air space
                this.pictureBox1.BringToFront();
                this.pictureBox1.Refresh();

                for (int i = 0; i < airSpace.beesNum; i++)
                {
                    x = airSpace.bees[i].getCurrentLoc().getX();
                    y = airSpace.bees[i].getCurrentLoc().getY();

                    X_point = Convert.ToInt32(zeroPointX + x * dispScaleX);
                    Y_point = Convert.ToInt32(zeroPointY - y * dispScaleY);

                    rect.X = X_point;
                    rect.Y = Y_point;
                    //this.pictureBox1.CreateGraphics().DrawRectangle(penRed, rect);
                    if (airSpace.bees[i].getCollisionState())
                        g.DrawRectangle(penRed, rect);
                    else
                        g.DrawRectangle(penBlack, rect);





                    //this.Controls[3].Location = beeDispPoint; //airSpace.bees[beeNum].beeBox.Location = beeDispPoint;
                    //this.Controls[3].Update();// airSpace.bees[beeNum].beeBox.Update();
                    //m_bees[beeNum].Location = beeDispPoint;
                    //m_bees[beeNum].Update();
                    //m_bees[beeNum].BringToFront();
                }
                g.Dispose();
                //this.pictureBox1.Update();


            }
        }//end: updateBeesDisplay()

        

        public double getInitValue_X()
        {
            return Convert.ToDouble(numericUpDown_Scale.Value * pictureBox1.Size.Width);
        }

        public double getInitValue_Y()
        {
            return Convert.ToDouble(numericUpDown_Scale.Value * pictureBox1.Size.Height);
        }

        public int getInitValue_H()
        {
            return Convert.ToInt32(numericUpDown_H.Value);
        }

        public int getInitValue_Bees()
        {
            return (int)numericUpDown_Bees.Value;
        }

        public double getInitValue_CollDist()
        {
            return Convert.ToDouble(numericUpDownCollDist.Value);
        }

        public double getInitValue_commDist()
        {
            return Convert.ToDouble(numericUpDownCommDist.Value);
        }

        public int getInitValue_commMode()
        {
            return Convert.ToInt32(numericUpDownCommMode.Value);
        }



        //public void updateSpriteLocation(System.Drawing.Point xy)
        //{
        //
        //    spriteBox.Location = xy;
        //    pictureBox1.Update();
        //    spriteBox.Update();
        //}



    }//end Form1



}
