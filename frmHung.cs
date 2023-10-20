using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace Caro_PhanMinhHưng
{
    public partial class frmHung : Form
    {
        // Constant
        public static readonly int MAX_UNDO_COUNT = 3;
        private static readonly int MAX_MOVE_TIME = 10; //seconds
        private static readonly int PROGRESS_BAR_STEP = 1;

        // Properties
        private Cls_Hung caro;
        private SoundPlayer spChess;
        private SoundPlayer spCountdown;
        private bool isCountdown = false;
        private int currentPlayer;
        private Player[] playerList;
        private Button[,] chessBoardButtons;
        private bool isGameStarted = false;
        private bool isHighlighted = false;
        private bool isBoardCleared = true;
        private bool allowUndo = false;
        private List<Button> highlightedButtons;
        private int totalGames = 0;
        private Stack<Button> moveStack;
        private DateTime startTime = DateTime.MinValue;


        // Method
        public frmHung()
        {
            spChess = new SoundPlayer("place_chess_pieces.wav");
            spCountdown = new SoundPlayer("countdown_sound.wav");

            currentPlayer = 0;

            playerList = new Player[]
            {
                new Player("", Properties.Resources.x_icon),
                new Player("", Properties.Resources.o_icon)
            };

            highlightedButtons = new List<Button>();

            moveStack = new Stack<Button>();

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            caro = new Cls_Hung(pnlLeft);
            drawBoard();
            chessBoardButtons = caro.getBoardButtons();

            pnlLeft.Size = new Size(Cls_Hung.BOARD_SIZE * Cls_Hung.CELL_SIZE, Cls_Hung.BOARD_SIZE * Cls_Hung.CELL_SIZE);
            pnlRight.Size = new Size(pnlRight.Width, pnlRight.Width);
            pnlRight.Location = new Point(pnlLeft.Location.X + pnlLeft.Width + 10, 0);
            pnlRight.Size = new Size(pnlRight.Width, pnlLeft.Height);

            ptbLogo.Size = new Size(240, 240);
            ptbLogo.Location = new Point((pnlRight.Width / 2) - (ptbLogo.Width / 2), (pnlRight.Width / 2) - (ptbLogo.Width / 2));

            ptbSignal1.Size = new Size(20, 20);
            ptbSignal2.Size = new Size(20, 20);

            btnUndoPlayer1.Size = new Size(20, 20);
            btnUndoPlayer2.Size = new Size(20, 20);

            pcbPlayer1.Maximum = MAX_MOVE_TIME;
            pcbPlayer1.Step = PROGRESS_BAR_STEP;
            pcbPlayer2.Maximum = MAX_MOVE_TIME;
            pcbPlayer2.Step = PROGRESS_BAR_STEP;

            ptbX.Size = new Size(50, 50);
            ptbO.Size = new Size(50, 50);

            resetNumberOfUndo();

            Height = pnlLeft.Height + 40;

        }

        private void drawBoard()
        {
            for (int i = 0; i < Cls_Hung.BOARD_SIZE; i++)
            {
                Panel panelRow = new Panel()
                {
                    Location = new Point(0, i * Cls_Hung.CELL_SIZE),
                    Size = new Size(Cls_Hung.BOARD_SIZE * Cls_Hung.CELL_SIZE, Cls_Hung.CELL_SIZE),
                    Tag = i
                };

                for (int j = 0; j < Cls_Hung.BOARD_SIZE; j++)
                {
                    Button btnTmp = new Button
                    {
                        Size = new Size(Cls_Hung.CELL_SIZE, Cls_Hung.CELL_SIZE),
                        Location = new Point(j * Cls_Hung.CELL_SIZE, 0),
                        BackColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        TabStop = false,
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Tag = j
                    };

                    btnTmp.Click += btnTmp_Click;

                    panelRow.Controls.Add(btnTmp);
                }
                pnlLeft.Controls.Add(panelRow);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (isGameStarted || !isBoardCleared) return;

            string playerName1, playerName2;
            InputDialog dialog = new InputDialog("Nhập tên người chơi 1:");

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                playerName1 = dialog.InputValue;
                if (string.IsNullOrWhiteSpace(playerName1))
                {
                    MessageBox.Show("Vui lòng nhập tên người chơi 1.");
                    return;
                }
            }
            else return;

            dialog = new InputDialog("Nhập tên người chơi 2:");

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                playerName2 = dialog.InputValue;
                if (string.IsNullOrWhiteSpace(playerName2))
                {
                    MessageBox.Show("Vui lòng nhập tên người chơi 2.");
                    return;
                }
            }
            else return;

            playerList[0].Name = playerName1;
            playerList[1].Name = playerName2;

            lblPlayer1.Text = $"Player 1: {playerName1}";
            lblPlayer2.Text = $"Player 2: {playerName2}";

            isGameStarted = true;
            isBoardCleared = false;
            allowUndo = true;
            startTime = DateTime.Now;

            countdownTimer.Start();

            spChess.SoundLocation = "place_chess_pieces.wav";

            markSignal();
        }

        private void btnTmp_Click(object sender, EventArgs e)
        {
            if (!isGameStarted) return;

            Button btnTmp = sender as Button;

            if (Cls_Hung.isMarked(btnTmp)) return;

            spChess.Stop();
            spCountdown.Stop();
            spChess.Play();

            isCountdown = false;

            btnTmp.BackgroundImage = playerList[currentPlayer].Mark;

            moveStack.Push(btnTmp);

            allowUndo = true;

            if (Cls_Hung.checkWin(chessBoardButtons, btnTmp, ref highlightedButtons))
            {
                gameOver();

                return;
            }

            changePlayer();

        }

        private void gameOver()
        {
            spCountdown.Stop();
            countdownTimer.Stop();

            updateNumberOfWins();

            playWinMusic();

            MessageBox.Show($"Player {currentPlayer + 1} {playerList[currentPlayer].Name} win");

            highlightTimer.Start();

            isGameStarted = false;

            allowUndo = false;

            Cls_Hung.saveGameResult(startTime, playerList[0], playerList[1], currentPlayer);
        }

        private void changePlayer()
        {
            countdownTimer.Stop();

            currentPlayer = currentPlayer == 0 ? 1 : 0;

            resetProgressBar();

            countdownTimer.Start();

            markSignal();
        }
        private void countdownTimer_Tick(object sender, EventArgs e)
        {
            if (currentPlayer == 0)
            {
                pcbPlayer1.PerformStep();
                playerList[0].PlayTime++;
                lblTimePlayer1.Text = formatTime(playerList[0].PlayTime);

                if (pcbPlayer1.Value > (MAX_MOVE_TIME/2) && !isCountdown)
                {
                    spCountdown.Play();
                    isCountdown = true;
                }

                if (pcbPlayer1.Value >= MAX_MOVE_TIME)
                {
                    currentPlayer = 1;
                    gameOver();
                }
            }
            else
            {
                pcbPlayer2.PerformStep();
                playerList[1].PlayTime++;
                lblTimePlayer2.Text = formatTime(playerList[1].PlayTime);

                if (pcbPlayer2.Value > (MAX_MOVE_TIME/2) && !isCountdown)
                {
                    spCountdown.Play();
                    isCountdown = true;
                }

                if (pcbPlayer2.Value >= MAX_MOVE_TIME)
                {
                    currentPlayer = 0;
                    gameOver();
                }
            }

        }

        public static string formatTime(int timeInSeconds)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(timeInSeconds);
            return timeSpan.ToString("mm\\:ss");
        }

        private void resetProgressBar()
        {
            pcbPlayer1.Value = pcbPlayer2.Value = 0;
        }

        private void resetPlayTime()
        {
            lblTimePlayer1.Text = lblTimePlayer2.Text = "00:00";
        }

        private void resetNumberOfUndo()
        {
            lblUndoPlayer1.Text = lblUndoPlayer2.Text = $"0/{MAX_UNDO_COUNT}";
        }
        private void updateNumberOfWins()
        {
            totalGames++;
            playerList[currentPlayer].IncreaseNumberOfWins();

            lblWinPlayer1.Text = $"{playerList[0].NumberOfWins}/{totalGames}";
            lblWinPlayer2.Text = $"{playerList[1].NumberOfWins}/{totalGames}";
        }

        private void highlightButtons()
        {
            if (highlightedButtons.Count < 5) return;

            if (isHighlighted)
            {
                foreach (Control c in highlightedButtons)
                    c.BackColor = Color.White;
            }
            else
            {
                foreach (Control c in highlightedButtons)
                    c.BackColor = Color.FromArgb(250, 228, 88);
            }
            isHighlighted = !isHighlighted;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            foreach (Control panelRow in pnlLeft.Controls)
            {
                foreach (Control buttonCell in panelRow.Controls)
                {
                    buttonCell.BackgroundImage = null;
                }
            }

            highlightTimer.Stop();
            countdownTimer.Stop();

            spCountdown.Stop();

            clearHighlighted();

            isGameStarted = false;

            isBoardCleared = true;

            isCountdown = false;

            lblPlayer1.Text = "Player 1:";
            lblPlayer2.Text = "Player 2:";

            currentPlayer = 0;

            foreach (Player player in playerList)
            {
                player.NumberOfUndo = 0;
                player.PlayTime = 0;
            }

            clearMarkSignal();

            resetProgressBar();

            resetPlayTime();

            resetNumberOfUndo();
        }

        private void highlightTimer_Tick(object sender, EventArgs e)
        {
            highlightButtons();
        }

        private void markSignal()
        {
            if (currentPlayer == 0)
            {
                ptbSignal1.BackgroundImage = Properties.Resources.signal_on;
                ptbSignal2.BackgroundImage = Properties.Resources.signal_off;
            }
            else
            {
                ptbSignal1.BackgroundImage = Properties.Resources.signal_off;
                ptbSignal2.BackgroundImage = Properties.Resources.signal_on;
            }
        }

        private void clearMarkSignal()
        {
            ptbSignal1.BackgroundImage = ptbSignal2.BackgroundImage = Properties.Resources.signal_off;
        }

        private void clearHighlighted()
        {
            if (highlightedButtons.Count == 0) return;

            foreach (Control c in highlightedButtons)
            {
                c.BackColor = Color.White;
            }

            highlightedButtons.Clear();
        }

        private void playWinMusic()
        {
            spChess.SoundLocation = "win_sound.wav";
            spChess.Play();
        }

        private void btnScore_Click(object sender, EventArgs e)
        {
            new frmResult().ShowDialog();
        }

        private void btnUndoPlayer1_Click(object sender, EventArgs e)
        {
            if (!allowUndo || currentPlayer == 1 || playerList[0].NumberOfUndo >= MAX_UNDO_COUNT) return;
            playerList[0].IncreaseNumberOfUndos();
            lblUndoPlayer1.Text = $"{playerList[0].NumberOfUndo}/{MAX_UNDO_COUNT}";

            performUndo();

            allowUndo = false;
        }

        private void btnUndoPlayer2_Click(object sender, EventArgs e)
        {
            if (!allowUndo || currentPlayer == 0 || playerList[1].NumberOfUndo >= MAX_UNDO_COUNT) return;
            playerList[1].IncreaseNumberOfUndos();
            lblUndoPlayer2.Text = $"{playerList[1].NumberOfUndo}/{MAX_UNDO_COUNT}";

            performUndo();

            allowUndo = false;
        }

        private void performUndo()
        {
            if (moveStack.Count >= 3)
            {
                for (int i = 1; i <= 2; i++)
                {
                    Button btnTmp = moveStack.Pop();
                    btnTmp.BackgroundImage = null;
                }
            }
        }
    }
}
