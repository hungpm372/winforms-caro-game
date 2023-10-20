using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Caro_PhanMinhHưng
{
    public class Cls_Hung
    {
        public static readonly int BOARD_SIZE = 16;
        public static readonly int CELL_SIZE = 40;
        public static readonly string FILE_NAME = "result.txt";

        private Panel chessBoard;

        public Cls_Hung(Panel chessBoard)
        {
            this.chessBoard = chessBoard;
        }

        public static bool isMarked(Button btn)
        {
            return btn.BackgroundImage != null;
        }

        public Button[,] getBoardButtons()
        {
            Button[,] board = new Button[BOARD_SIZE, BOARD_SIZE];

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                Panel panelRow = chessBoard.Controls[i] as Panel;

                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    Button btnTmp = panelRow.Controls[j] as Button;
                    board[i, j] = btnTmp;
                }
            }

            return board;
        }

        public static bool checkWin(Button[,] board, Button btn, ref List<Button> highlightedButtons)
        {
            int row = (int)btn.Parent.Tag;
            int col = (int)btn.Tag;

            int count = 0;
            int i = col;
            while (i >= 0 && board[row, i].BackgroundImage == btn.BackgroundImage)
            {
                highlightedButtons.Add(board[row, i]);
                count++;
                i--;
            }
            i = col + 1;
            while (i < BOARD_SIZE && board[row, i].BackgroundImage == btn.BackgroundImage)
            {
                highlightedButtons.Add(board[row, i]);
                count++;
                i++;
            }
            if (count >= 5) return true;
            else highlightedButtons.Clear();

            count = 0;
            int j = row;
            while (j >= 0 && board[j, col].BackgroundImage == btn.BackgroundImage)
            {
                highlightedButtons.Add(board[j, col]);
                count++;
                j--;
            }
            j = row + 1;
            while (j < BOARD_SIZE && board[j, col].BackgroundImage == btn.BackgroundImage)
            {
                highlightedButtons.Add(board[j, col]);
                count++;
                j++;
            }
            if (count >= 5) return true;
            else highlightedButtons.Clear();

            count = 0;
            i = col;
            j = row;
            while (i >= 0 && j >= 0 && board[j, i].BackgroundImage == btn.BackgroundImage)
            {
                highlightedButtons.Add(board[j, i]);
                count++;
                i--;
                j--;
            }
            i = col + 1;
            j = row + 1;
            while (i < BOARD_SIZE && j < BOARD_SIZE && board[j, i].BackgroundImage == btn.BackgroundImage)
            {
                highlightedButtons.Add(board[j, i]);
                count++;
                i++;
                j++;
            }
            if (count >= 5) return true;
            else highlightedButtons.Clear();

            count = 0;
            i = col;
            j = row;
            while (i >= 0 && j < BOARD_SIZE && board[j, i].BackgroundImage == btn.BackgroundImage)
            {
                highlightedButtons.Add(board[j, i]);
                count++;
                i--;
                j++;
            }
            i = col + 1;
            j = row - 1;
            while (i < BOARD_SIZE && j >= 0 && board[j, i].BackgroundImage == btn.BackgroundImage)
            {
                highlightedButtons.Add(board[j, i]);
                count++;
                i++;
                j--;
            }
            if (count >= 5) return true;
            else highlightedButtons.Clear();

            return false;
        }

        public static void saveGameResult(DateTime startTime, Player player1, Player player2, int winner)
        {
            GameResult result = new GameResult(startTime, player1.Name, player1.PlayTime, player1.NumberOfUndo, player2.Name, player2.PlayTime, player2.NumberOfUndo, winner);

            try
            {
                string json = JsonConvert.SerializeObject(result);

                using (StreamWriter writer = File.AppendText(FILE_NAME))
                {
                    writer.WriteLine(json);
                }

                Console.WriteLine("Lưu kết quả thành công.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi lưu kết quả: " + ex.Message);
            }
        }

        public static string[] getGameResultDatas()
        {
            if (File.Exists(FILE_NAME))
            {
                using (StreamReader sr = new StreamReader(FILE_NAME))
                {
                    List<string> lines = new List<string>();

                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }

                    return lines.ToArray();
                }
            }
             return new string[0];
        }


    }

    public class InputDialog : Form
    {
        private Label lblPrompt;
        private TextBox txtInput;
        private Button btnOK;
        private Button btnCancel;

        public string InputValue { get; private set; }

        public InputDialog(string prompt)
        {
            InitializeComponent();
            lblPrompt.Text = prompt;
        }

        private void InitializeComponent()
        {
            lblPrompt = new Label();
            txtInput = new TextBox();
            btnOK = new Button();
            btnCancel = new Button();

            lblPrompt.AutoSize = true;
            lblPrompt.Location = new Point(12, 9);
            lblPrompt.Size = new Size(0, 17);
            lblPrompt.TabIndex = 0;

            txtInput.Location = new Point(12, 36);
            txtInput.Name = "txtInput";
            txtInput.Size = new Size(250, 22);
            txtInput.TabIndex = 1;

            btnOK.DialogResult = DialogResult.OK;
            btnOK.Location = new Point(107, 75);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 2;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += new EventHandler(btnOK_Click);

            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(188, 75);
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;

            AcceptButton = btnOK;
            CancelButton = btnCancel;
            ClientSize = new Size(274, 110);
            Controls.Add(lblPrompt);
            Controls.Add(txtInput);
            Controls.Add(btnOK);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
            PerformLayout();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            InputValue = txtInput.Text;
        }
    }
}
