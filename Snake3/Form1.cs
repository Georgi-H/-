using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake3
{
    public partial class Form1 : Form
    {

        private List<Circle> Snake = new List<Circle>(); //list array for snake 
        private Circle food = new Circle(); // single circle class - the snake's food

        public Form1()
        {
            InitializeComponent();

            new Settings();//linking settings class

            gameTimer.Interval = 1000 / Settings.Speed; //changing game time
            gameTimer.Tick += updateScreen;//linking an update function to the timer
            gameTimer.Start();//starting the timer when starting the game

            startGame();//start game function

        }
        private void updateScreen(object sender, EventArgs e)
        {
            if (Settings.GameOver == true) //starts a new game
            {
                if (Input.KeyPress(Keys.Enter))
                {
                    startGame();
                }
            }
            else
            {
                //if game is not over then
                //bellow are the keys the player presses
                if (Input.KeyPress(Keys.Right) && Settings.direction != Directions.Left)
                {
                    Settings.direction = Directions.Right;
                }
                else if (Input.KeyPress(Keys.Left) && Settings.direction != Directions.Right)
                {
                    Settings.direction = Directions.Left;
                }
                else if (Input.KeyPress(Keys.Up) && Settings.direction != Directions.Down)
                {
                    Settings.direction = Directions.Up;
                }
                else if (Input.KeyPress(Keys.Down) && Settings.direction != Directions.Up)
                {
                    Settings.direction = Directions.Down;
                }
                movePlayer();//run direction
            }
            pbCanvas.Invalidate();//refresh picture box
        }
        private void movePlayer()
        {
            //loop for snake and head
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                //if snake head is active
                if (i == 0)
                {
                    //move rest of body acordinglly
                    switch (Settings.direction)
                    {
                        case Directions.Right:
                            Snake[i].X++;
                            break;
                        case Directions.Left:
                            Snake[i].X--;
                            break;
                        case Directions.Up:
                            Snake[i].Y--;
                            break;
                        case Directions.Down:
                            Snake[i].Y++;
                            break;

                    }
                    //restrict the snake from leaving the canvas
                    int maxXpos = pbCanvas.Size.Width / Settings.Width;
                    int maxYpos = pbCanvas.Size.Height / Settings.Height;

                    if (Snake[i].X < 0 || Snake[i].Y < 0 || Snake[i].X > maxXpos || Snake[i].Y > maxYpos)
                    {
                        //end game if snake hits it's head
                        die();
                    }
                    //detect if snake hits itself
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            die();
                        }
                    }
                    //if snake eats
                    if (Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        //if it's true we run eat function
                        eat();
                    }
                }
                else
                {
                    //if no collision snake keeps moving
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }
        private void keyisdown(object sender, KeyEventArgs e)
        {
            Input.changeState(e.KeyCode, true);
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            Input.changeState(e.KeyCode, false);
        }

        private void updateGraphics(object sender, PaintEventArgs e)
        {
            //this is where we see the snake and it's parts
            Graphics canvas = e.Graphics;
            if (Settings.GameOver == false)
            {
                Brush snakeColour;
                for (int i = 0; i < Snake.Count; i++)
                {
                    if (i == 0)
                    {
                        snakeColour = Brushes.White;
                    }
                    else
                    {
                        snakeColour = Brushes.Green;
                    }
                    //draw snake
                    canvas.FillEllipse(snakeColour,
                        new Rectangle(
                            Snake[i].X * Settings.Width,
                            Snake[i].Y * Settings.Height,
                            Settings.Width, Settings.Height
                            ));
                    //draw food
                    canvas.FillEllipse(Brushes.Red,
                        new Rectangle(
                            food.X * Settings.Width,
                            food.Y * Settings.Height,
                            Settings.Width, Settings.Height
                            ));
                }
            }
            else
            {
                //when the game is over
                string gameOver = "                 Game over\n" + "Final score is " + Settings.Score + "\nPrees ENTER to restart the game\n";
                label3.Text = gameOver;
                label3.Visible = true;
            }
        }
        private void startGame()
        {
            label3.Visible = false;//set end game lable to invisible
            new Settings();
            Snake.Clear();//clear all old snake body parts
            Circle head = new Circle { X = 10, Y = 5 };//new head for the snake
            Snake.Add(head);

            label2.Text = Settings.Score.ToString();//show the score in the second label
            generateFood();//generate food function
        }
        private void generateFood()
        {
            int maxXpos = pbCanvas.Size.Width / Settings.Width;
            int maxYpos = pbCanvas.Size.Height / Settings.Height;
            Random rnd = new Random();
            food = new Circle
            { X = rnd.Next(0, maxXpos), Y = rnd.Next(0, maxYpos) };
        }
        private void eat()
        {
            //add a part to the body
            Circle body = new Circle()
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            Snake.Add(body);//add a circle to the snake array
            Settings.Score += Settings.Points;//increase the score when the snake grows up
            label2.Text = Settings.Score.ToString();//show the score in the second label
            generateFood();//generate new food for Snakey
        }
        private void die()
        {
            Settings.GameOver = true;

        }

    }
}
