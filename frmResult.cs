using Caro_PhanMinhHưng.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Caro_PhanMinhHưng
{
    public partial class frmResult : Form
    {
        public frmResult()
        {
            InitializeComponent();
        }

        private void frmResult_Load(object sender, EventArgs e)
        {
            string[] lines = Cls_Hung.getGameResultDatas();
            if (lines.Length > 0)
            {
                foreach (string item in lines)
                {
                    GameResult gameResult = JsonConvert.DeserializeObject<GameResult>(item);
                    createItemGameResult(gameResult);
                }
            } else
            {
                createEmptyItemGameResult();
            }
        }

        private void createEmptyItemGameResult()
        {
            Label lblEmpty = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 21.16F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Location = new Point(60, 70),
                Margin = new Padding(0),
                ForeColor = Color.White,
                Size = new Size(67, 21),
                TabIndex = 1,
                Text = "No data",
                TextAlign = ContentAlignment.MiddleCenter
            };

            pnlContainer.Controls.Add(lblEmpty);

        }
        private void createItemGameResult(GameResult result)
        {
            GroupBox item = new GroupBox()
            {
                Dock = DockStyle.Top,
                Font = new Font("Microsoft Sans Serif", 10.8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0))),
                Location = new Point(20, 20),
                Margin = new Padding(0),
                AutoSize = true,
                Padding = new Padding(20, 0, 0, 0),
                Text = result.StartTime.ToString(),
            };


            //Player 1
            Label lblPlayer1 = new Label
            {
                AutoSize = true,
                Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Location = new Point(60, 37),
                Margin = new Padding(0),
                Size = new Size(89, 23),
                TabIndex = 0,
                Text = "Player 1:"
            };

            Label lblNameLabel1 = new Label
            {
                AutoSize = true,
                Font = new Font("Arial", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Location = new Point(60, 70),
                Margin = new Padding(0),
                Size = new Size(67, 21),
                TabIndex = 1,
                Text = "Name: ",
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblNamePlayer1 = new Label
            {
                AutoSize = true,
                Font = new Font("Arial", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Location = new Point(158, 70),
                Margin = new Padding(0),
                Size = new Size(25, 21),
                TabIndex = 1,
                Text = result.Player1Name,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblTimeLabel1 = new Label
            {
                AutoSize = true,
                Font = new Font("Arial", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Location = new Point(60, 101),
                Margin = new Padding(0),
                Size = new Size(59, 21),
                TabIndex = 1,
                Text = "Time:",
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblTimePlayer1 = new Label
            {
                AutoSize = true,
                Font = new Font("Arial", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Location = new Point(158, 101),
                Margin = new Padding(0),
                Size = new Size(55, 21),
                TabIndex = 1,
                Text = frmHung.formatTime(result.Player1Time),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblUndoLabel1 = new Label
            {
                AutoSize = true,
                Font = new Font("Arial", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Location = new Point(60, 132),
                Margin = new Padding(0),
                Size = new Size(63, 21),
                TabIndex = 1,
                Text = "Undo: ",
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblUndoPlayer1 = new Label
            {
                AutoSize = true,
                Font = new Font("Arial", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Location = new Point(158, 132),
                Margin = new Padding(0),
                Size = new Size(20, 21),
                TabIndex = 1,
                Text = $"{result.Player1Undo}/{frmHung.MAX_UNDO_COUNT}",
                TextAlign = ContentAlignment.MiddleCenter
            };

            PictureBox ptbX = new PictureBox
            {
                BackgroundImage = Resources.x_icon,
                BackgroundImageLayout = ImageLayout.Stretch,
                Location = new Point(20, 30),
                Margin = new Padding(0),
                Size = new Size(30, 30),
                TabStop = false
            };

            PictureBox ptbWin = new PictureBox
            {
                BackgroundImage = Resources.winner_icon,
                BackgroundImageLayout = ImageLayout.Stretch,
                Margin = new Padding(0),
                Size = new Size(60, 60),
                TabIndex = 3,
                TabStop = false,
            };

            if (result.Winner == 0)
            {
                ptbWin.Location = new Point(260, 52);
            }
            else
            {
                ptbWin.Location = new Point(260, 197);
            }


            //Player 2
            Label lblPlayer2 = new Label
            {
                AutoSize = true,
                Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Location = new Point(60, 182),
                Margin = new Padding(0),
                Size = new Size(89, 23),
                TabIndex = 0,
                Text = "Player 2:"
            };

            Label lblNameLabel2 = new Label
            {
                AutoSize = true,
                Font = new Font("Arial", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Location = new Point(60, 215),
                Margin = new Padding(0),
                Size = new Size(67, 21),
                TabIndex = 1,
                Text = "Name: ",
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblNamePlayer2 = new Label
            {
                AutoSize = true,
                Font = new Font("Arial", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Location = new Point(158, 215),
                Margin = new Padding(0),
                Size = new Size(25, 21),
                TabIndex = 1,
                Text = result.Player2Name,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblTimeLabel2 = new Label
            {
                AutoSize = true,
                Font = new Font("Arial", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Location = new Point(60, 246),
                Margin = new Padding(0),
                Size = new Size(59, 21),
                TabIndex = 1,
                Text = "Time: ",
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblTimePlayer2 = new Label
            {
                AutoSize = true,
                Font = new Font("Arial", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Location = new Point(158, 246),
                Margin = new Padding(0),
                Size = new Size(55, 21),
                TabIndex = 1,
                Text = frmHung.formatTime(result.Player2Time),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblUndoLabel2 = new Label
            {
                AutoSize = true,
                Font = new Font("Arial", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Location = new Point(60, 277),
                Margin = new Padding(0),
                Size = new Size(63, 21),
                TabIndex = 1,
                Text = "Undo: ",
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblUndoPlayer2 = new Label
            {
                AutoSize = true,
                Font = new Font("Arial", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Location = new Point(158, 277),
                Margin = new Padding(0),
                Size = new Size(20, 21),
                TabIndex = 1,
                Text = $"{result.Player2Undo}/{frmHung.MAX_UNDO_COUNT}",
                TextAlign = ContentAlignment.MiddleCenter
            };

            PictureBox ptbO = new PictureBox
            {
                BackgroundImage = Resources.o_icon,
                BackgroundImageLayout = ImageLayout.Stretch,
                Location = new Point(20, 175),
                Margin = new Padding(0),
                Size = new Size(30, 30),
                TabIndex = 2,
                TabStop = false
            };

            // pnlSeparation
            Panel pnlSeparation = new Panel()
            {
                Dock = DockStyle.Top,
                Location = new Point(20, 330),
                Margin = new Padding(0),
                Size = new Size(442, 20),
                TabIndex = 1
            };

            item.Controls.Add(lblPlayer1);
            item.Controls.Add(lblNameLabel1);
            item.Controls.Add(lblNamePlayer1);
            item.Controls.Add(lblTimeLabel1);
            item.Controls.Add(lblTimePlayer1);
            item.Controls.Add(lblUndoLabel1);
            item.Controls.Add(lblUndoPlayer1);
            item.Controls.Add(ptbX);
            item.Controls.Add(lblPlayer2);
            item.Controls.Add(lblNameLabel2);
            item.Controls.Add(lblNamePlayer2);
            item.Controls.Add(lblTimeLabel2); 
            item.Controls.Add(lblTimePlayer2);
            item.Controls.Add(lblUndoLabel2);
            item.Controls.Add(lblUndoPlayer2);
            item.Controls.Add(ptbO);
            item.Controls.Add(ptbWin);

            pnlContainer.Controls.Add(pnlSeparation);
            pnlContainer.Controls.Add(item);
        }
    }
}
