using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlatformerGame
{
    public partial class Form2 : Form
    {
        bool goLeft, goRight, jumping, isGameOver;

        int jumpSpeed;
        int force;
        int score = 19;
        int playerSpeed = 7;
        int horizontalSpeed = 5;
        int verticalSpeed = 3;
        int enemyOneSpeed = 5;
        int enemyTwoSpeed = 3;
        int enemyThreeSpeed = 3;

        public Form2()
        {
            InitializeComponent();
        }

        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            txtScore.Text = "Score: " + score;
            jumping = false;
            goLeft = false;
            goRight = false;
            isGameOver = false;

            player.Top += jumpSpeed;
            //gameTimer.Start();

            //movement
            if (goLeft == true)
            {
                player.Left -= playerSpeed;
            }
            if (goRight == true)
            {
                player.Left += playerSpeed;
            }
            if (jumping == true && force < 0)
            {
                jumping = false;
            }
            if (jumping == true)
            {
                jumpSpeed = -8;
                force -= 1;
            }
            else
            {
                jumpSpeed = 10;
            }

            foreach (Control x in this.Controls)
            {
                //player stays on top of platform
                if (x is PictureBox)
                {
                    if ((string)x.Tag == "platform")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            force = 6;
                            player.Top = x.Top - player.Height;

                            //moves player with moving platform
                            if ((string)x.Name == "horizontalPlatform" && goLeft == false || (string)x.Name == "horizontalPlatform" && goRight == false
                                || (string)x.Name == "horizontalPlatformTwo" && goLeft == false || (string)x.Name == "horizontalPlatformTwo" && goRight == false)
                            {
                                player.Left -= horizontalSpeed;
                            }
                        }

                        x.BringToFront();
                    }
                    //makes coin invisible once player moves over it
                    if ((string)x.Tag == "coin")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                        {
                            x.Visible = false;
                            score++;
                        }
                    }

                    //enemy kills player
                    if ((string)x.Tag == "enemy")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            gameTimer.Stop();
                            isGameOver = true;
                            txtScore.Text = "Score: " + score + Environment.NewLine + "You were killed in your journey!!";
                            MessageBox.Show("You didn't make it! Try again." + Environment.NewLine + "Press 'Enter' to try again.");
                        }
                    }
                }
            }
            //platform moving
            horizontalPlatform.Left -= horizontalSpeed;
            if (horizontalPlatform.Left < 315 || horizontalPlatform.Left > 461)
            {
                horizontalSpeed = -horizontalSpeed;
            }

            horizontalPlatformTwo.Left -= horizontalSpeed;
            if (horizontalPlatformTwo.Left < 449 || horizontalPlatformTwo.Left > 607)
            {
                horizontalSpeed = -horizontalSpeed;
            }


            verticalPlatform.Top += verticalSpeed;
            if (verticalPlatform.Top < 512 || verticalPlatform.Top > 180)
            {
                verticalSpeed = -verticalSpeed;
            }

            //enemy moving
            enemyOne.Left += enemyOneSpeed;
            if (enemyOne.Left < pictureBox12.Left || enemyOne.Left + enemyOne.Width > pictureBox12.Left + pictureBox12.Width)
            {
                enemyOneSpeed = -enemyOneSpeed;
            }
            enemyTwo.Left += enemyTwoSpeed;
            if (enemyTwo.Left < pictureBox5.Left || enemyTwo.Left + enemyTwo.Width > pictureBox5.Left + pictureBox5.Width)
            {
                enemyTwoSpeed = -enemyTwoSpeed;
            }
            enemyThree.Left += enemyThreeSpeed;
            if (enemyTwo.Left < pictureBox2.Left || enemyTwo.Left + enemyTwo.Width > pictureBox2.Left + pictureBox2.Width)
            {
                enemyThreeSpeed = -enemyThreeSpeed;
            }


            //player goes out of bounds game reset
            if (player.Top + player.Height > this.ClientSize.Height + 50)
            {
                gameTimer.Stop();
                isGameOver = true;
                txtScore.Text = "Score: " + score + Environment.NewLine + "You fell to your death!";
                MessageBox.Show("You didn't make it! Try again." + Environment.NewLine + "Press 'Enter' to try again.");
            }

            //player gets to door
            if (player.Bounds.IntersectsWith(door.Bounds) && score == 38)
            {
                isGameOver = true;
                txtScore.Text = "Score: " + score + Environment.NewLine + "You did it!!";
                player.Left = 12;
                player.Top = 605;
                goLeft = false;
                goRight = false;

                MessageBox.Show("You win!!");
            }

            if (player.Bounds.IntersectsWith(door.Bounds) && score != 38)
            {
                txtScore.Text = "Score: " + score + Environment.NewLine + "Collect all the coins";
            }
        }


        //player movement
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Space && jumping == false)
            {
                jumping = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (jumping == true)
            {
                jumping = false;
            }

            if (e.KeyCode == Keys.Enter && isGameOver == true)
            {
                RestartGame();
            }
        }

        //restart game
        private void RestartGame()
        {
            jumping = false;
            goLeft = false;
            goRight = false;
            isGameOver = false;
            score = 0;

            txtScore.Text = "Score: " + score;

            //Form1 levelOne = new Form1();
            //levelOne.Show();
            //this.Hide();
            //Close();

        }
    }
}
