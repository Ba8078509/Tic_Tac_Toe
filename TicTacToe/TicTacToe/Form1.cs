using System.Xml;

namespace TicTacToe
{
    public partial class Form1 : Form
    {
        private readonly List<Button> _buttons = new();
        private readonly Random _random = new();
        private bool _isGameActive = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            _buttons.Clear();
            panel.Controls.Clear();
            
            int buttonSize = 74;
            int spacing = 8;
            int startX = (panel.Width - (3 * buttonSize + 2 * spacing)) / 2;
            int startY = (panel.Height - (3 * buttonSize + 2 * spacing)) / 2;
            
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    var button = new Button
                    {
                        Left = startX + x * (buttonSize + spacing),
                        Top = startY + y * (buttonSize + spacing),
                        Size = new Size(buttonSize, buttonSize),
                        Font = new Font("Segoe UI", 28, FontStyle.Bold),
                        BackColor = Color.FromArgb(60, 63, 65),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Tag = x * 3 + y,
                        Cursor = Cursors.Hand
                    };
                    
                    button.FlatAppearance.BorderSize = 0;
                    button.FlatAppearance.MouseOverBackColor = Color.FromArgb(75, 78, 80);
                    button.FlatAppearance.MouseDownBackColor = Color.FromArgb(45, 45, 48);
                    
                    button.Click += Button_Click;
                    _buttons.Add(button);
                    panel.Controls.Add(button);
                }
            }
        }

        /// <summary>
        /// Проверяет состояние игры. Возвращает:
        /// 0 - игра продолжается, 1 - победил X, 2 - победил O, 3 - ничья
        /// </summary>
        private int CheckGameState()
        {
            // Проверка строк
            for (int i = 0; i < 3; i++)
            {
                if (IsWinningLine(i * 3, i * 3 + 1, i * 3 + 2))
                    return _buttons[i * 3].Text == "X" ? 1 : 2;
            }

            // Проверка столбцов
            for (int i = 0; i < 3; i++)
            {
                if (IsWinningLine(i, i + 3, i + 6))
                    return _buttons[i].Text == "X" ? 1 : 2;
            }

            // Проверка диагоналей
            if (IsWinningLine(0, 4, 8) || IsWinningLine(2, 4, 6))
                return _buttons[4].Text == "X" ? 1 : 2;

            // Проверка на ничью
            return _buttons.All(b => b.Text != "") ? 3 : 0;
        }

        private bool IsWinningLine(int a, int b, int c)
        {
            return _buttons[a].Text != "" &&
                   _buttons[a].Text == _buttons[b].Text &&
                   _buttons[a].Text == _buttons[c].Text;
        }

        private async void Button_Click(object? sender, EventArgs e)
        {
            if (sender is not Button currentButton || !_isGameActive || currentButton.Text != "")
                return;

            // Ход игрока
            currentButton.Text = "X";
            
            var gameState = CheckGameState();
            if (gameState != 0)
            {
                EndGame(gameState);
                return;
            }

            // Блокируем поле на время хода компьютера
            SetButtonsEnabled(false);
            
            // Имитация раздумья компьютера (асинхронно, без блокировки UI)
            await Task.Delay(_random.Next(500, 1000));
            
            // Ход компьютера
            MakeComputerMove();
            
            gameState = CheckGameState();
            if (gameState != 0)
            {
                EndGame(gameState);
            }
            else
            {
                SetButtonsEnabled(true);
            }
        }

        private void MakeComputerMove()
        {
            var emptyButtons = _buttons.Where(b => b.Text == "").ToList();
            if (emptyButtons.Count == 0) return;

            var randomIndex = _random.Next(emptyButtons.Count);
            emptyButtons[randomIndex].Text = "O";
        }

        private void EndGame(int gameState)
        {
            _isGameActive = false;
            string message = gameState switch
            {
                1 => "VICTORY! You won the game!",
                2 => "DEFEAT! Computer won the game.",
                3 => "DRAW!",
                _ => ""
            };

            // Обновляем статус перед показом сообщения
            UpdateStatus(gameState);
            
            MessageBox.Show(message, "Game Over", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateStatus(int gameState)
        {
            lblStatus.Text = gameState switch
            {
                1 => "YOU WIN! (X)",
                2 => "COMPUTER WINS! (O)",
                3 => "DRAW!",
                _ => lblStatus.Text
            };
            
            // Меняем цвет статуса в зависимости от результата
            lblStatus.ForeColor = gameState switch
            {
                1 => Color.FromArgb(40, 200, 80),   // Зеленый для победы
                2 => Color.FromArgb(220, 60, 60),   // Красный для поражения
                3 => Color.FromArgb(255, 193, 7),   // Желтый для ничьей
                _ => Color.FromArgb(255, 193, 7)
            };
        }

        private void SetButtonsEnabled(bool enabled)
        {
            foreach (var button in _buttons)
            {
                button.Enabled = enabled;
            }
            
            // Обновляем статус при блокировке/разблокировке кнопок
            if (enabled && _isGameActive)
            {
                lblStatus.Text = "YOUR TURN (X)";
                lblStatus.ForeColor = Color.FromArgb(255, 193, 7);
            }
            else if (!enabled && _isGameActive)
            {
                lblStatus.Text = "COMPUTER THINKING...";
                lblStatus.ForeColor = Color.FromArgb(100, 180, 255);
            }
        }

        private void ClearField()
        {
            foreach (var button in _buttons)
            {
                button.Text = "";
            }
            _isGameActive = true;
            SetButtonsEnabled(true);
            
            // Сбрасываем статус на начальный
            lblStatus.Text = "YOUR TURN (X)";
            lblStatus.ForeColor = Color.FromArgb(255, 193, 7);
        }

        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearField();
        }

        private void saveGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using var folderBrowser = new FolderBrowserDialog();
                if (folderBrowser.ShowDialog() != DialogResult.OK) return;

                var cellX = string.Concat(_buttons
                    .Where((b, i) => b.Text == "X")
                    .Select(b => ((int)b.Tag! + 1).ToString()));
                
                var cellO = string.Concat(_buttons
                    .Where((b, i) => b.Text == "O")
                    .Select(b => ((int)b.Tag! + 1).ToString()));

                var xmlDoc = new XmlDocument();
                var root = xmlDoc.CreateElement("TicTacToe");
                xmlDoc.AppendChild(root);

                var cellsNode = xmlDoc.CreateElement("Cells");
                root.AppendChild(cellsNode);

                var xNode = xmlDoc.CreateElement("X");
                xNode.InnerText = cellX;
                cellsNode.AppendChild(xNode);

                var oNode = xmlDoc.CreateElement("O");
                oNode.InnerText = cellO;
                cellsNode.AppendChild(oNode);

                var fileName = $"file_{DateTime.Now:yyyyMMdd_HHmmss}.xml";
                var filePath = Path.Combine(folderBrowser.SelectedPath, fileName);
                xmlDoc.Save(filePath);

                MessageBox.Show($"XML файл '{fileName}' успешно создан в '{folderBrowser.SelectedPath}'!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loadGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ClearField();
                using var openFileDialog = new OpenFileDialog
                {
                    Filter = "XML файлы (*.xml)|*.xml"
                };

                if (openFileDialog.ShowDialog() != DialogResult.OK) return;

                var xmlDoc = new XmlDocument();
                xmlDoc.Load(openFileDialog.FileName);

                var xNode = xmlDoc.SelectSingleNode("//X");
                var oNode = xmlDoc.SelectSingleNode("//O");

                if (xNode == null || oNode == null)
                {
                    MessageBox.Show("Неверный формат файла сохранения", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                foreach (char c in xNode.InnerText)
                {
                    if (int.TryParse(c.ToString(), out int index) && index >= 1 && index <= 9)
                        _buttons[index - 1].Text = "X";
                }

                foreach (char c in oNode.InnerText)
                {
                    if (int.TryParse(c.ToString(), out int index) && index >= 1 && index <= 9)
                        _buttons[index - 1].Text = "O";
                }

                // Проверяем состояние загруженной игры
                var gameState = CheckGameState();
                if (gameState != 0)
                {
                    _isGameActive = false;
                    SetButtonsEnabled(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            // Заглушка для события
        }
    }
}
