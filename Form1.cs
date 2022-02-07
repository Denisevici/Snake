using System;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        public const int FormWidth = 500;
        public const int FormHeight = 500;
        public const int CircleDiameter = 20;

        Snake snake = new Snake();
        Fruit fruit = new Fruit();

        public int score = 0;
        public bool changeDirection = true;
        public bool gameOver = false;
        public bool pause = false;
        public int speed = 150;
        
        public Form1()
        {
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint, true);

            Width = FormWidth + 200;
            Height = FormHeight + 200;
        }

        private PictureBox canvas = new PictureBox();

        private Timer timer = new Timer();

        private Label labelScore = new Label();
        private Label labelGameOver = new Label();
        private Label labelRestart = new Label();
        private Label labelPause = new Label();
        private Label labelQuit = new Label();
        private Label labelInfo = new Label();
        private Label labelSpeed = new Label();
        private Label labelSpeedInfo = new Label(); 

        private void Form1_Load(object sender, EventArgs e)
        {
            canvas.Size = new Size(FormWidth, FormHeight);
            canvas.BackColor = Color.Gray;
            canvas.Paint += new PaintEventHandler(CanvasPaint);
            Controls.Add(canvas);

            KeyDown += new KeyEventHandler(CurrentKey);

            timer.Interval = speed;
            timer.Enabled = true;
            timer.Tick += new EventHandler(TimerEventProcessor);

            labelInfo.Location = new System.Drawing.Point { X = 60, Y = FormHeight + 30 };
            labelInfo.AutoSize = true;
            labelInfo.Text = "Use buttons W A S D to choose movement's direction";
            Controls.Add(labelInfo);

            labelSpeedInfo.Location = new System.Drawing.Point { X = 60, Y = FormHeight + 60 };
            labelSpeedInfo.AutoSize = true;
            labelSpeedInfo.Text = "Press T to rise speed and Y to reduce";
            Controls.Add(labelSpeedInfo);

            labelPause.Location = new System.Drawing.Point { X = 60, Y = FormHeight + 90 };
            labelPause.AutoSize = true;
            labelPause.Text = "Press P to pause";
            Controls.Add(labelPause);

            labelQuit.Location = new System.Drawing.Point { X = 60, Y = FormHeight + 120 };
            labelQuit.AutoSize = true;
            labelQuit.Text = "Press Q to quit the game";
            Controls.Add(labelQuit);

            labelScore.Location = new System.Drawing.Point { X = FormWidth + 30, Y = 90 };
            labelScore.AutoSize = true;
            Controls.Add(labelScore);

            labelSpeed.Location = new System.Drawing.Point { X = FormWidth + 30, Y = 120 };
            labelSpeed.AutoSize = true;
            Controls.Add(labelSpeed);

            labelGameOver.Location = new System.Drawing.Point { X = FormWidth + 30, Y = 150 };
            labelGameOver.AutoSize = true;
            labelGameOver.Text = "Game Over";
            labelGameOver.Visible = false;
            Controls.Add(labelGameOver);

            labelRestart.Location = new System.Drawing.Point { X = FormWidth + 30, Y = 180 };
            labelRestart.AutoSize = true;
            labelRestart.Text = "Press R to restart";
            labelRestart.Visible = false;
            Controls.Add(labelRestart);
        }

        private void CurrentKey(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && snake.Direction != Directions.Right && changeDirection)
            {
                snake.Direction = Directions.Left;
                changeDirection = false;
            }
            else if (e.KeyCode == Keys.D && snake.Direction != Directions.Left && changeDirection)
            {
                snake.Direction = Directions.Right;
                changeDirection = false;
            }
            else if (e.KeyCode == Keys.W && snake.Direction != Directions.Down && changeDirection)
            {
                snake.Direction = Directions.Up;
                changeDirection = false;
            }
            else if (e.KeyCode == Keys.S && snake.Direction != Directions.Up && changeDirection)
            {
                snake.Direction = Directions.Down;
                changeDirection = false;
            }

            if (e.KeyCode == Keys.Y && speed < 200)
                speed += 10;
            else if (e.KeyCode == Keys.T && speed > 10)
                speed -= 10;

            if (e.KeyCode == Keys.R && gameOver == true)
                Restart();
            
            if (e.KeyCode == Keys.P && gameOver == false)
                if (pause)
                {
                    pause = false;
                    labelPause.Text = "Press P to pause";
                }
                else
                {
                    pause = true;
                    labelPause.Text = "Press P to unpause";
                }

            if (e.KeyCode == Keys.Q)
                Application.Exit();
        }

        private void CanvasPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.FillEllipse(Brushes.Red, snake.Location[0].X, snake.Location[0].Y, CircleDiameter, CircleDiameter);
            for (int i = 1; i < snake.Length; i++)
            {
                g.FillEllipse(Brushes.DarkRed, snake.Location[i].X, snake.Location[i].Y, CircleDiameter, CircleDiameter);
            }
            g.FillEllipse(Brushes.Yellow, fruit.Location.X, fruit.Location.Y, CircleDiameter, CircleDiameter);          
        }

        private void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            labelScore.Text = string.Format("Score: {0}", score);
            labelSpeed.Text = string.Format("Speed: {0}", 21 - speed / 10);

            if (pause)
            { 
            }
            else if (gameOver == false)
            {
                timer.Interval = speed;
                changeDirection = true;
                
                snake.MoveSnake();
                CheckDead();
                EatFruit();
                snake.CheckGrowSnake();
                Refresh();
            }
            else
            {
                labelGameOver.Visible = true;
                labelRestart.Visible = true;
            }
        }

        private void EatFruit()
        {
            if (fruit.Location.X == snake.Location[0].X && fruit.Location.Y == snake.Location[0].Y)
            {
                score++;

                snake.TailLocation.Add(new TailPoint { X = fruit.Location.X, Y = fruit.Location.Y, LengthToAppear = snake.Length });
                fruit.Create();

                int count = 0;
                while (count != snake.Length)
                {
                    count = 0;
                    for (int i = 0; i < snake.Length; i++)
                    {
                        if (snake.Location[i].X == fruit.Location.X && snake.Location[i].Y == fruit.Location.Y)
                        {
                            fruit.Create();
                            break;
                        }
                        else
                            count++;
                    }
                }
            }
        }

        private void CheckDead()
        {
            if (snake.Location[0].X < 0 || snake.Location[0].X >= FormWidth || snake.Location[0].Y < 0 || snake.Location[0].Y >= FormHeight)
                Die();

            for (int i = 1; i < snake.Length; i++)
            {
                if (snake.Location[0].X == snake.Location[i].X && snake.Location[0].Y == snake.Location[i].Y)
                    Die();
            }
        }

        private void Die()
        {
            gameOver = true;
            labelGameOver.Visible = true;
            labelRestart.Visible = true;
            labelPause.Visible = false;
        }

        private void Restart()
        {
            gameOver = false;
            changeDirection = true;
            labelGameOver.Visible = false;
            labelRestart.Visible = false;
            labelPause.Visible = true;
            score = 0;
            snake.Default();
            fruit.Create();
        }
    }
}