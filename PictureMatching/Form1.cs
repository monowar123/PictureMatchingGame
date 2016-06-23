using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Resources;

namespace PictureMatching
{
    public partial class Form1 : Form
    {
        private Graphics pictureGraph;

        bool lbNormalFlag = true;
        bool lbMediumFlag = true;
        bool lbHardFlag = true;
        bool startFlag = true;

        private int numberOfDivision = 3;
        private static int imageNumber = 0;
        private int sec = 0;
        private int minute = 0;
        private int hour = 0;
        private int numberOfMoves = 0;
        
        private List<Image> ImageList = new List<Image>();
        private List<Bitmap> CropedImageList = new List<Bitmap>();
        private List<Bitmap> CompareImageList = new List<Bitmap>();

        private PictureBox[ , ] PictureBoxArray = new PictureBox[25, 25];
        private PictureBox FinalPictureBox = new PictureBox();
        private Random randomNumbers = new Random();

        public Form1()
        {
            InitializeComponent();
            pictureGraph = pictureBox1.CreateGraphics();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ImageList.Add(Properties.Resources.pic00);
            ImageList.Add(Properties.Resources.pic01);
            ImageList.Add(Properties.Resources.pic02);
            ImageList.Add(Properties.Resources.pic03);
            ImageList.Add(Properties.Resources.pic04);
            ImageList.Add(Properties.Resources.pic05);

            pictureBox1.Image = ImageList[imageNumber];
            startToolStripMenuItem.Enabled = false;

            btnPlay_Click(this, new EventArgs());  // call play button click method when the form load
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Pen penCurrent = new Pen(Color.Wheat);
            Rectangle Rect1 = new Rectangle(250, 40, 350, 350);
            e.Graphics.DrawRectangle(penCurrent, Rect1);            
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            this.Controls.Remove(FinalPictureBox);
            sec = 0; minute = 0; hour = 0; // start from new time
            timer1_Tick(this, new EventArgs());
            numberOfMoves = 0;
            lbMoves.Text = string.Format("{0:00000}", numberOfMoves);
           
            Croping_Image(numberOfDivision);
            CreatePictureBox(numberOfDivision);

            //RandomiseCropingImage();

            int cropedFlag = 0;
            for (int i = 0; i < numberOfDivision; i++)  //assign the randomise croped picture
            {
                for (int j = 0; j < numberOfDivision; j++)
                {
                    PictureBoxArray[i, j].Image = CropedImageList[cropedFlag];
                    PictureBoxArray[i, j].Cursor = Cursors.Hand;
                    PictureBoxArray[i, j].Click += new EventHandler(pictureBox_click); //call the click event
                    cropedFlag++;
                }
            }
        }

        private void pictureBox_click(object sender, EventArgs e)
        {
            if (startFlag)
            {
                int rowNumber = 0, collNumber = 0;
                for (int i = 0; i < numberOfDivision; i++)
                {
                    for (int j = 0; j < numberOfDivision; j++)
                    {
                        if (PictureBoxArray[i, j].Image == null)
                        {
                            rowNumber = i; collNumber = j;      // collect the row and coll number of the null picturebox    
                        }
                    }
                }
                Point destination = PictureBoxArray[rowNumber, collNumber].Location;
                Point source = ((PictureBox)sender).Location;  //coordinate of the clicked picture box
                int range = 350 / numberOfDivision;

                if ((destination.X == source.X + range && destination.Y == source.Y) ||  //compare if the clicked
                    (destination.X == source.X && destination.Y == source.Y + range) ||  //picture box is in 
                    (destination.X == source.X && destination.Y == source.Y - range) ||  //changable position
                    (destination.X == source.X - range && destination.Y == source.Y))
                {
                    PictureBoxArray[rowNumber, collNumber].Image = ((PictureBox)sender).Image;
                    ((PictureBox)sender).Image = null;
                    lbMoves.Text = string.Format("{0:00000}", ++numberOfMoves);
                }
            }
            int compareFlag = 0;
            int flag = 0;
            for (int i = 0; i < numberOfDivision; i++)
            {
                for (int j = 0; j < numberOfDivision; j++)
                {
                    if (PictureBoxArray[i, j].Image == CompareImageList[flag++])
                    {
                        compareFlag++;
                    }
                }
            }
            if (compareFlag == CompareImageList.Count - 1)
            {
                timer1.Stop();
                for (int i = 0; i < 25; i++)    // at first remove all picture box and then created and assign newly
                {
                    for (int j = 0; j < 25; j++)
                    {
                        this.Controls.Remove(PictureBoxArray[i, j]);
                    }
                }
                this.Controls.Add(FinalPictureBox);
                FinalPictureBox.Location = new Point(252, 42);
                FinalPictureBox.Size = new Size(350, 350);
                FinalPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                FinalPictureBox.BorderStyle = BorderStyle.Fixed3D;
                FinalPictureBox.Image = ImageList[imageNumber];
                MessageBox.Show("You have won the game.", "Congratulations", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void Croping_Image(int n)
        {
            CropedImageList.Clear();  //at first clear the previous image
            CompareImageList.Clear();
            int cropWidth = ImageList[imageNumber].Width / n;
            int cropHeight = ImageList[imageNumber].Height / n;
            Bitmap bmpImage = new Bitmap(ImageList[imageNumber]); //image that will be cloned
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Rectangle cropArea = new Rectangle(cropWidth * j, cropHeight * i, cropWidth, cropHeight);
                    Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
                    CropedImageList.Add(bmpCrop);
                    CompareImageList.Add(bmpCrop);
                }
            }
            CropedImageList[n * n - 1] = null;
        }

        private void CreatePictureBox(int n)
        {
            for (int i = 0; i < 25; i++)    // at first remove all picture box and then created and assign newly
            {
                for (int j = 0; j < 25; j++)
                {
                    this.Controls.Remove(PictureBoxArray[i, j]);
                }
            }
            int pointX = 252;
            int pointY = 42;
            int fieldRange = 350 / n;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    PictureBox pBox = new PictureBox();
                    pBox.Size = new Size(fieldRange, fieldRange);
                    pBox.BackColor = Color.Black;
                    this.Controls.Add(pBox);
                    pBox.Location = new Point(pointX + fieldRange * j, pointY + fieldRange * i);
                    pBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    pBox.BorderStyle = BorderStyle.Fixed3D;
                    PictureBoxArray[i, j] = pBox;
                }
            }
        }

        private void RandomiseCropingImage()
        {
            for (int first = 0; first < CropedImageList.Count; first++)
            {            
                int second = randomNumbers.Next(CropedImageList.Count);

                Bitmap temp = CropedImageList[first];
                CropedImageList[first] = CropedImageList[second];
                CropedImageList[second] = temp;   
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (imageNumber + 1 == ImageList.Count)
                imageNumber = 0;
            else
                imageNumber++;
            pictureBox1.Image = ImageList[imageNumber];  // update with new image
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (imageNumber == 0)
                imageNumber = ImageList.Count - 1;
            else
                imageNumber--;
            pictureBox1.Image = ImageList[imageNumber];   // update with new image
        }

        private void lbNormal_MouseEnter(object sender, EventArgs e)
        {
            lbNormal.Cursor = Cursors.Hand;
            if (lbNormalFlag)
            {
                lbNormal.BackColor = Color.Gray;
            }
            PictureBoxLine(3);  //draw all the lines 
        }

        private void lbNormal_MouseLeave(object sender, EventArgs e)
        {
            if (lbNormalFlag)
            {
                lbNormal.BackColor = Color.CadetBlue;
            }
            pictureBox1.Image = ImageList[imageNumber];
        }

        private void lbMedium_MouseEnter(object sender, EventArgs e)
        {
            lbMedium.Cursor = Cursors.Hand;
            if (lbMediumFlag)
            {
                lbMedium.BackColor = Color.Gray;
            }
            PictureBoxLine(4);  //draw all the lines
        }

        private void lbMedium_MouseLeave(object sender, EventArgs e)
        {
            if (lbMediumFlag)
            {
                lbMedium.BackColor = Color.CadetBlue;
            }
            pictureBox1.Image = ImageList[imageNumber];
        }

        private void lbHard_MouseEnter(object sender, EventArgs e)
        {
            lbHard.Cursor = Cursors.Hand;
            if (lbHardFlag)
            {
                lbHard.BackColor = Color.Gray;
            }
            PictureBoxLine(5);  //draw all the lines
        }

        private void lbHard_MouseLeave(object sender, EventArgs e)
        {
            if (lbHardFlag)
            {
                lbHard.BackColor = Color.CadetBlue;
            }
            pictureBox1.Image = ImageList[imageNumber];
        }

        private void PictureBoxLine(int n)
        {
            int oneThirdWidth = pictureBox1.ClientSize.Width / n;
            int oneThirdHeight = pictureBox1.ClientSize.Height / n;
            for (int i = 1; i < n; i++)   // draw all the lines.
            {
                pictureGraph.DrawLine(Pens.White, i * oneThirdWidth, 0, i * oneThirdWidth, oneThirdHeight * n);
                pictureGraph.DrawLine(Pens.White, 0, i * oneThirdHeight, oneThirdWidth * n, i * oneThirdHeight);
            }
        }

        private void lbNormal_Click(object sender, EventArgs e)
        {
            numberOfDivision = 3;
            lbNormal.BackColor = Color.Wheat;
            lbNormalFlag = false;
            lbMediumFlag = true;
            lbHardFlag = true;
            lbMedium.BackColor = Color.CadetBlue;
            lbHard.BackColor = Color.CadetBlue;
        }

        private void lbMedium_Click(object sender, EventArgs e)
        {
            numberOfDivision = 4;
            lbMedium.BackColor = Color.Wheat;
            lbMediumFlag = false;
            lbNormalFlag = true;
            lbHardFlag = true;
            lbNormal.BackColor = Color.CadetBlue;
            lbHard.BackColor = Color.CadetBlue;
        }

        private void lbHard_Click(object sender, EventArgs e)
        {
            numberOfDivision = 5;
            lbHard.BackColor = Color.Wheat;
            lbHardFlag = false;
            lbNormalFlag = true;
            lbMediumFlag = true;
            lbNormal.BackColor = Color.CadetBlue;
            lbMedium.BackColor = Color.CadetBlue;
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {

            lbtime.Text = string.Format("{0:00}:{1:00}:{2:00}", hour, minute, sec++);
            if (sec == 60)
            {
                sec = 0;
                minute++;
                if (minute == 60)
                {
                    minute = 0;
                    hour++;
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help helpwindow = new Help();
            helpwindow.Show();
        }

        private void developerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Developer developerWindow = new Developer();
            developerWindow.Show();
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startToolStripMenuItem.Enabled = false;
            pauseToolStripMenuItem.Enabled = true;
            startFlag = true;
            timer1.Start();
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startToolStripMenuItem.Enabled = true;
            pauseToolStripMenuItem.Enabled = false;
            startFlag = false;
            timer1.Stop();
            
        }

        private void highScoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HighScore scoreWindow = new HighScore();
            scoreWindow.Show();
        }

        private void loadPictToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files(*.jpeg,*.jpg,*.bmp)|*.jpeg;*.jpg;*.bmp|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            ResourceWriter rWritter = new ResourceWriter("myResources.resources");
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Image newImage = Image.FromFile(openFileDialog1.FileName);
                    if (ImageList.Contains(newImage))   //here is an problem
                    {
                        MessageBox.Show("This image is already exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        pictureBox1.Image = newImage;
                        ImageList.Add(newImage);
                        imageNumber = ImageList.Count - 1;
                        //rWritter.AddResourceData("picNew", "*.jpg", newImage);
                    }
                }
                catch
                {
                    MessageBox.Show("Image Format is not correct.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void deletePictureToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
