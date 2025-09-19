using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class SnakeGame : Form
{
    private System.Windows.Forms.Timer gameTimer = new System.Windows.Forms.Timer();
    private List<Point> snake = new List<Point>();
    private Point food;
    private int dx = 1, dy = 0;
    private int gridSize = 20;
    private int score = 0;
    private bool gameOver = false;
    private Random rand = new Random();

    public SnakeGame()
    {
        this.Text = "Snake Game";
        this.Width = 420;
        this.Height = 440;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.DoubleBuffered = true;

        this.KeyDown += new KeyEventHandler(OnKeyDown);

        // Initialize snake
        snake.Add(new Point(5, 5));

        // Spawn food
        SpawnFood();

        // Timer setup
        gameTimer.Interval = 120;
        gameTimer.Tick += Update;
        gameTimer.Start();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Up && dy != 1) { dx = 0; dy = -1; }
        else if (e.KeyCode == Keys.Down && dy != -1) { dx = 0; dy = 1; }
        else if (e.KeyCode == Keys.Left && dx != 1) { dx = -1; dy = 0; }
        else if (e.KeyCode == Keys.Right && dx != -1) { dx = 1; dy = 0; }
    }

    private void Update(object sender, EventArgs e)
    {
        if (gameOver) return;

        Point newHead = new Point(snake[0].X + dx, snake[0].Y + dy);

        // Check collision with walls
        if (newHead.X < 0 || newHead.Y < 0 ||
            newHead.X >= this.ClientSize.Width / gridSize ||
            newHead.Y >= this.ClientSize.Height / gridSize ||
            snake.Contains(newHead))
        {
            gameOver = true;
            gameTimer.Stop();
            MessageBox.Show("Game Over! Final Score: " + score);
            return;
        }

        snake.Insert(0, newHead);

        // Eat food
        if (newHead == food)
        {
            score++;
            SpawnFood();
        }
        else
        {
            snake.RemoveAt(snake.Count - 1); // remove tail
        }

        this.Invalidate();
    }

    private void SpawnFood()
    {
        food = new Point(rand.Next(this.ClientSize.Width / gridSize),
                         rand.Next(this.ClientSize.Height / gridSize));
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;

        // Draw snake
        foreach (Point p in snake)
        {
            g.FillRectangle(Brushes.Green, p.X * gridSize, p.Y * gridSize, gridSize - 1, gridSize - 1);
        }

        // Draw food
        g.FillRectangle(Brushes.Red, food.X * gridSize, food.Y * gridSize, gridSize - 1, gridSize - 1);

        // Draw score
        g.DrawString("Score: " + score, this.Font, Brushes.Black, 10, 10);
    }

    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new SnakeGame());
    }
}
