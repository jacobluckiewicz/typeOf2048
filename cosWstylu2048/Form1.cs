using System.Diagnostics.SymbolStore;

namespace cosWstylu2048
{
    public partial class Form1 : Form
    {
        private const int gridSize = 4;
        private int[,] board = new int[gridSize, gridSize];
        private Random random = new Random();
        private int score = 0;
        public Form1()
        {
            InitializeComponent();
            StartGame();
        }

        private void StartGame()
        {
            Array.Clear(board, 0, board.Length);
            score = 0;
            AddRandomTile();
            AddRandomTile();
            UpdateUI();
        }
        private void AddRandomTile()
        {
            var EmptyTiles = board.Cast<int>()
                .Select((value, index) => new { value, index })
                .Where(x => x.value == 0)
                .ToArray();
            if (EmptyTiles.Length > 0)
            {
                var randomTile = EmptyTiles[random.Next(EmptyTiles.Length)];
                board[randomTile.index / gridSize, randomTile.index % gridSize] = random.Next(0, 10) == 0 ? 4 : 2;
            }
        }

        private void UpdateUI()
        {
            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize; col++)
                {
                    var label = (Label)tableLayoutPanel1.GetControlFromPosition(col, row);
                    int value = board[row, col];
                    label.Text = value == 0 ? "" : value.ToString();
                    label.BackColor = GetTileColor(value);
                }
            }
            lblScore.Text = "score: " + score;
        }

        private System.Drawing.Color GetTileColor(int value)
        {
            switch (value)
            {
                case 2: return System.Drawing.Color.LightGray;
                case 4: return System.Drawing.Color.LightYellow;
                case 8: return System.Drawing.Color.Orange;
                case 16: return System.Drawing.Color.Coral;
                case 32: return System.Drawing.Color.DarkOrange;
                case 64: return System.Drawing.Color.Red;
                case 128: return System.Drawing.Color.Gold;
                case 256: return System.Drawing.Color.Yellow;
                case 512: return System.Drawing.Color.LightGreen;
                case 1024: return System.Drawing.Color.Green;
                case 2048: return System.Drawing.Color.LightBlue;
                default: return System.Drawing.Color.White;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            bool moved = false;

            switch (e.KeyCode)
            {
                case Keys.Up: moved = MoveUp(); break;
                case Keys.Down: moved = MoveDown(); break;
                case Keys.Left: moved = MoveLeft(); break;
                case Keys.Right: moved = MoveRight(); break;
            }
            if (moved)
            {
                AddRandomTile();
                UpdateUI();
                if (CheckGameOver())
                {
                    MessageBox.Show("Game Over! Your score: " + score);
                    StartGame();
                }
            }


        }
        private bool CheckGameOver()
        {
            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize; col++)
                {
                    if (board[row, col] == 0) return false;
                    if (col < gridSize - 1 && board[row, col] == board[row, col + 1]) return false;
                    if (row < gridSize - 1 && board[row, col] == board[row + 1, col]) return false;
                }
            }
            return true;
        }
        private bool MoveLeft()
        {
            bool moved = false;
            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 1; col < gridSize; col++)
                {
                    if (board[row, col] == 0) continue;
                    int target = col - 1;
                    while (target > 0 && board[row, target] == 0)
                    {
                        target--;
                    }

                    if (board[row, target] == 0)
                    {
                        board[row, target] = board[row, col];
                        board[row, col] = 0;
                        moved = true;
                    }
                    else if (board[row, target] == board[row, col])
                    {
                        board[row, target] *= 2;
                        board[row, col] = 0;
                        score += board[row, target];
                        moved = true;
                    }
                    else if (target + 1 != col)
                    {
                        board[row, target + 1] = board[row, col];
                        board[row, col] = 0;
                        moved = true;
                    }
                }
            }
            return moved;
        }
        private bool MoveRight()
        {
            bool moved = false;
            for (int row = 0; row < gridSize; row++)
            {
                for (int col = gridSize - 2; col >= 0; col--)
                {
                    if (board[row, col] == 0) continue;
                    int target = col + 1;

                    while (target < gridSize - 1 && board[row, target] == 0)
                    {
                        target++;
                    }
                    if (board[row, target] == 0)
                    {
                        board[row, target] = board[row, col];
                        board[row, col] = 0;
                        moved = true;
                    }
                    else if (board[row, target] == board[row, col])
                    {
                        board[row, target] *= 2;
                        board[row, col] = 0;
                        score += board[row, target];
                        moved = true;
                    }
                    else if (target + 1 != col)
                    {
                        board[row, target - 1] = board[row, col];
                        board[row, col] = 0;
                        moved = true;
                    }
                }
            }
            return moved;
        }
        private bool MoveUp()
        {
            bool moved = false;
            for (int col = 0; col < gridSize; col++)
            {
                for (int row = 1; row < gridSize; row++)
                {
                    if (board[row, col] == 0) continue;
                    int target = row - 1;
                    while (target > 0 && board[target, col] == 0)
                    {
                        target--;
                    }
                    if (board[target, col] == 0)
                    {
                        board[target, col] = board[row, col];
                        board[row, col] = 0;
                        moved = true;
                    }
                    else if (board[target, col] == board[row, col])
                    {
                        board[target, col] *= 2;
                        board[row, col] = 0;
                        score += board[target, col];
                        moved = true;
                    }
                    else if (target + 1 != row)
                    {
                        board[target + 1, col] = board[row, col];
                        board[row, col] = 0;
                        moved = true;
                    }
                }
            }
            return moved;
        }
        private bool MoveDown()
        {
            bool moved = false;
            for (int col = 0; col < gridSize; col++)
            {
                for (int row = gridSize - 2; row >= 0; row--)
                {
                    if (board[row, col] == 0) continue;
                    int target = row + 1;
                    while (target < gridSize - 1 && board[target, col] == 0)
                    {
                        target++;
                    }
                    if (board[target, col] == 0)
                    {
                        board[target, col] = board[row, col];
                        board[row, col] = 0;
                        moved = true;
                    }
                    else if (board[target, col] == board[row, col])
                    {
                        board[target, col] *= 2;
                        board[row, col] = 0;
                        score += board[target, col];
                        moved = true;
                    }
                    else if (target - 1 != row)
                    {
                        board[target - 1, col] = board[row, col];
                        board[row, col] = 0;
                        moved = true;
                    }
                }
            }
            return moved;
        }
    }
}