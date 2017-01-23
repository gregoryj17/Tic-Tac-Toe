using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        int x;
        int y;
        int cellSize;
        int margin = 10;
        int row = -1;
        int col = -1;
        Random rand = new Random();
        int[,] board = new int[3,3];
        int turn = 0;

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            UpdateSize();
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    board[i, j] = -1;
                }
            }
            this.MinimumSize = new Size(100, 100);
        }

        private void UpdateSize()
        {
            cellSize = (Math.Min(ClientSize.Width, ClientSize.Height) - 2 * margin) / 3;
            if (ClientSize.Width > ClientSize.Height)
            {
                x = (ClientSize.Width - 3 * cellSize) / 2;
                y = margin;
            }
            else
            {
                x = margin;
                y = (ClientSize.Height - 3 * cellSize) / 2;
            }
        }

        protected override void OnResize(EventArgs e)
        {

            base.OnResize(e);
            UpdateSize();
            Refresh();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            col = (int)Math.Floor((e.X - x) * 1.0 / cellSize);
            row = (int)Math.Floor((e.Y - y) * 1.0 / cellSize);
            if (isOver()) resetBoard();
            else if (col > -1 && row > -1 && col < 3 && row < 3 && board[col,row]==-1)
            {
                board[col, row] = turn;
                turn = 1 - turn;
            }
            Refresh();
            //base.OnMouseDown(e);
        }

        private void resetBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    board[i, j] = -1;
                }
            }
            turn = 0;
        }

        private bool isOver()
        {
            return (isWon() || isFull());
        }

        private bool isFull()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == -1)return false;
                }
            }
            return true;
        }

        private bool isWon()
        {
            if (board[0,0] == board[0,1] && board[0,0] == board[0,2]) if (board[0,0] != -1) return true;
            if (board[1,0] == board[1,1] && board[1,0] == board[1,2]) if (board[1,0] != -1) return true;
            if (board[2,0] == board[2,1] && board[2,0] == board[2,2]) if (board[2,0] != -1) return true;
            if (board[0,0] == board[1,0] && board[0,0] == board[2,0]) if (board[0,0] != -1) return true;
            if (board[0,1] == board[1,1] && board[0,1] == board[2,1]) if (board[0,1] != -1) return true;
            if (board[0,2] == board[1,2] && board[0,2] == board[2,2]) if (board[0,2] != -1) return true;
            if (board[0,0] == board[1,1] && board[0,0] == board[2,2]) if (board[0,0] != -1) return true;
            if (board[0,2] == board[1,1] && board[0,2] == board[2,0]) if (board[0,2] != -1) return true;
            return false;
        }

        private int winner()
        {
            if (board[0, 0] == board[0, 1] && board[0, 0] == board[0, 2]) if (board[0, 0] != -1) return board[0,0];
            if (board[1, 0] == board[1, 1] && board[1, 0] == board[1, 2]) if (board[1, 0] != -1) return board[1,0];
            if (board[2, 0] == board[2, 1] && board[2, 0] == board[2, 2]) if (board[2, 0] != -1) return board[2,0];
            if (board[0, 0] == board[1, 0] && board[0, 0] == board[2, 0]) if (board[0, 0] != -1) return board[0,0];
            if (board[0, 1] == board[1, 1] && board[0, 1] == board[2, 1]) if (board[0, 1] != -1) return board[0,1];
            if (board[0, 2] == board[1, 2] && board[0, 2] == board[2, 2]) if (board[0, 2] != -1) return board[0,2];
            if (board[0, 0] == board[1, 1] && board[0, 0] == board[2, 2]) if (board[0, 0] != -1) return board[0,0];
            if (board[0, 2] == board[1, 1] && board[0, 2] == board[2, 0]) if (board[0, 2] != -1) return board[0,2];
            return -1;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    Rectangle rect = new Rectangle(x + i * cellSize, y + j * cellSize, cellSize, cellSize);
                    e.Graphics.DrawRectangle(Pens.Chocolate, rect);
                    Font font = new Font("Ubuntu", cellSize * 3 * 72 / 96 / 4);
                    e.Graphics.DrawString(board[i,j]==0?"X":board[i,j]==1?"O":"", font, board[i,j]==0?Brushes.Navy:Brushes.Crimson, x + (cellSize / 8) + (cellSize * i), y + (cellSize / 8) + (cellSize * j));
                }
            if (isOver())
            {
                Color color = Color.FromArgb(200, 125, 253, 0);
                if (isWon())
                {
                    if (winner() == 0) color = Color.FromArgb(200, 0, 125, 253);
                    else color = Color.FromArgb(200, 253, 0, 125);
                }
                Font font = new Font("Comic Sans MS", cellSize * 2 * 72 / 96 / 3);
                Font font2 = new Font("Comic Sans MS", cellSize * 72 / 96 / 3);
                e.Graphics.FillRectangle(new SolidBrush(color), new Rectangle(x, y, cellSize * 3, cellSize * 3));
                e.Graphics.DrawString(winner() == 0 ? "X wins." : winner() == 1 ? "O wins." : "Tie game.", font, Brushes.Black, x, y);
                e.Graphics.DrawString("Click to reset.", font2, Brushes.Black, x, y + cellSize * 2);
                
            }
        }


    }

}
