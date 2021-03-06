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
    public partial class Form1 : Form
    {
        bool goLeft, goRight, jumping, isGameOver;

        int jumpSpeed;
        int force;
        int score = 0;
        int playerSpeed = 7;
        int horizontalSpeed = 5;
        int verticalSpeed = 3;
        int enemyOneSpeed = 5;
        int enemyTwoSpeed = 3;

        public Form1()
        {
            InitializeComponent();
            MessageBox.Show("Use the arrow keys or 'a' and 'd' to move the player to collect all the coins. Avoid the enemies and reach the door to win!");
        }

        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            txtScore.Text = "Score: " + score;
            player.Top += jumpSpeed;

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
                            if ((string)x.Name == "horizontalPlatform" && goLeft == false || (string)x.Name == "horizontalPlatform" && goRight == false)
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
            if (horizontalPlatform.Left < 200 || horizontalPlatform.Left + horizontalPlatform.Width > this.ClientSize.Width)
            {
                horizontalSpeed = -horizontalSpeed;
            }

            verticalPlatform.Top += verticalSpeed;
            if (verticalPlatform.Top < 181 || verticalPlatform.Top > 480)
            {
                verticalSpeed = -verticalSpeed;
            }

            //enemy moving
            enemyOne.Left += enemyOneSpeed;
            if (enemyOne.Left < pictureBox2.Left || enemyOne.Left + enemyOne.Width > pictureBox2.Left + pictureBox2.Width)
            {
                enemyOneSpeed = -enemyOneSpeed;
            }
            enemyTwo.Left += enemyTwoSpeed;
            if (enemyTwo.Left < pictureBox7.Left || enemyTwo.Left + enemyTwo.Width > pictureBox7.Left + pictureBox7.Width)
            {
                enemyTwoSpeed = -enemyTwoSpeed;
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
            if (player.Bounds.IntersectsWith(door.Bounds) && score == 19)
            {
                gameTimer.Stop();
                isGameOver = false;
                txtScore.Text = "Score: " + score + Environment.NewLine + "You did it!!";
                LoadNextLevel();
            }
            if (player.Bounds.IntersectsWith(door.Bounds) && score != 19)

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

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Visible == false)
                {
                    x.Visible = true;
                }
            }

            //reset position of player, enemies, platforms, timer
            player.Left = 12;
            player.Top = 605;
            verticalPlatform.Location = new System.Drawing.Point(11, 480);
            horizontalPlatform.Location = new System.Drawing.Point(378, 108);
            enemyOne.Location = new System.Drawing.Point(457, 511);
            enemyTwo.Location = new System.Drawing.Point(315, 177);

            gameTimer.Start();
        }
        private void LoadNextLevel()
        {
            Form2 levelTwo = new Form2();
            this.Hide();
            levelTwo.ShowDialog();
            this.Close();

        }
    }
}


