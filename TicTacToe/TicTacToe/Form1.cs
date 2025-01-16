using System.Xml;
using System.Xml.Linq;

namespace TicTacToe
{
    public partial class Form1 : Form
    {
        int status = 0;
        string tag = string.Empty;
        List<Button> buttons = new List<Button>();


        public Form1()
        {
            InitializeComponent();
        }

        /*void GenerateTag()
        {
            var r = new Random();
            tag = r.Next(0, 2) == 0 ? "X" : "O";
        }*/
        private void Form1_Load(object sender, EventArgs e)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    var button = new Button();
                    button.Left = 10 + 74 * x;
                    button.Top = 10 + 74 * y;
                    button.Size = new System.Drawing.Size(74, 74);
                    button.Font = new Font(FontFamily.GenericSansSerif, 24);
                    button.BackColor = System.Drawing.Color.Navy;
                    button.ForeColor = System.Drawing.Color.White;
                    button.Click += Button_Click;
                    //button.MouseEnter += Button_MouseEnter;
                    //button.MouseLeave += Button_MouseLeave;

                    buttons.Add(button);

                    // Прикрепить кнопку к форме
                    panel.Controls.Add(button);
                }
            }
        }
        int CheckWin()
        {
            int status = 0;

            for (int i = 0; i < 3; i++)
            {
                if (buttons[i * 3].Text == "X" &&
                    buttons[i * 3 + 1].Text == "X" &&
                    buttons[i * 3 + 2].Text == "X")
                {
                    return 1;
                }

                if (buttons[i * 3].Text == "O" &&
                    buttons[i * 3 + 1].Text == "O" &&
                    buttons[i * 3 + 2].Text == "O")
                {
                    return 2;
                }
            }

            // Проверка по столбцам на "X" и "O"
            for (int i = 0; i < 3; i++)
            {
                if (buttons[i].Text == "X" &&
                    buttons[i + 3].Text == "X" &&
                    buttons[i + 6].Text == "X")
                {
                    return 1;
                }

                if (buttons[i].Text == "O" &&
                    buttons[i + 3].Text == "O" &&
                    buttons[i + 6].Text == "O")
                {
                    return 2;
                }
            }

            // Проверка по диагоналям на "X" и "O"
            if (buttons[0].Text == "X" &&
                buttons[4].Text == "X" &&
                buttons[8].Text == "X")
            {
                return 1;
            }

            if (buttons[2].Text == "X" &&
                buttons[4].Text == "X" &&
                buttons[6].Text == "X")
            {
                return 1;
            }

            if (buttons[0].Text == "O" &&
                buttons[4].Text == "O" &&
                buttons[8].Text == "O")
            {
                return 2;
            }

            if (buttons[2].Text == "O" &&
                buttons[4].Text == "O" &&
                buttons[6].Text == "O")
            {
                return 2;
            }

            return 0;
        }

        bool fullnessCheck()
        {
            int status = 0;
            foreach (Button s in buttons)
            {
                if (s.Text != "")
                {
                    status++;
                }
                if (status == 9)
                {
                    return true;
                }
            }
            return false;
        }


        private void Button_Click(object? sender, EventArgs e)
        {
            Random rnd = new Random();
            int value;

            Button? currentButton = sender as Button;

            if (currentButton.Text != "X" && currentButton.Text != "O")
            {
                currentButton.Text = "X";
                //currentButton.Text = tag;
                //tag = tag == "X" ? "O" : "X";
                int timer = rnd.Next(1);
                if (timer == 0)
                {
                    foreach (Button s in buttons)
                    {
                        s.Enabled = false;
                    }
                    Thread.Sleep(500);
                    foreach (Button s in buttons)
                    {
                        s.Enabled = true;
                    }
                }
                else if (timer == 1)
                {
                    foreach (Button s in buttons)
                    {
                        s.Enabled = false;
                    }
                    Thread.Sleep(1000);
                    foreach (Button s in buttons)
                    {
                        s.Enabled = true;
                    }
                }
                value = rnd.Next(9);
                while (true)
                {

                    if (!fullnessCheck())
                    {
                        if (buttons[value].Text != "X" && buttons[value].Text != "O")
                        {
                            buttons[value].Text = "O";
                            Refresh();

                            if (CheckWin() == 1)
                            {
                                MessageBox.Show(
                                "Победа! Вы одержали победу.",
                                "Сообщение",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information,
                                MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.DefaultDesktopOnly);
                            }
                            else if (CheckWin() == 2)
                            {
                                MessageBox.Show(
                                "Поражение! Компьютер одержал победу.",
                                "Сообщение",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information,
                                MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.DefaultDesktopOnly);
                            }

                            break;

                        }
                        else { value = rnd.Next(9); }
                    }
                    else if (fullnessCheck() && CheckWin() == 0)
                    {
                        MessageBox.Show(
                        "Ничья!",
                        "Сообщение",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
                        break;
                    }
                    else if (fullnessCheck() && CheckWin() > 0)
                    {
                        if (CheckWin() == 1)
                        {
                            MessageBox.Show(
                            "Победа! Вы одержали победу.",
                            "Сообщение",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                        }
                        else if (CheckWin() == 2)
                        {
                            MessageBox.Show(
                            "Поражение! Компьютер одержал победу.",
                            "Сообщение",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                        }

                        break;

                    }
                }
            }
        }
        void clearField()
        {
            foreach (Button s in buttons)
            {
                s.Text = null;
            }
        }
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearField();
        }

        private void saveGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowser = new FolderBrowserDialog())
            {
                string cellX = "";
                string cellO = "";
                int status = 0;

                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    // Генерация имени файла на основе текущей даты и времени
                    string fileName = $"file_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xml";
                    string filePath = Path.Combine(folderBrowser.SelectedPath, fileName);

                    // Создание XML-документа
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlElement root = xmlDoc.CreateElement("TicTacToe");
                    xmlDoc.AppendChild(root);

                    foreach (Button cell in buttons)
                    {
                        status++;
                        if (cell.Text != "" && cell.Text == "X")
                        {
                            cellX += status;
                        }
                        else if (cell.Text != "" && cell.Text == "O")
                        {
                            cellO += $"{status}";
                        }
                    }

                    // Пример добавления узла
                    XmlElement childNode = xmlDoc.CreateElement("Cells");
                    root.AppendChild(childNode);

                    XmlElement Xcells = xmlDoc.CreateElement("X");
                    Xcells.InnerText = cellX;
                    childNode.AppendChild(Xcells);

                    XmlElement Ocells = xmlDoc.CreateElement("O");
                    Ocells.InnerText = cellO;
                    childNode.AppendChild(Ocells);

                    xmlDoc.Save(filePath);

                    // Подтверждение создания файла
                    MessageBox.Show($"XML файл '{fileName}' успешно создан в '{folderBrowser.SelectedPath}'!");
                }
            }
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {

        }

        private void loadGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearField();
            // Открытие диалога для выбора файла
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "XML файлы (*.xml)|*.xml"; // Фильтр для XML-файлов

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName; // Путь к выбранному файлу

                    // Загрузка XML-документа
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(filePath);

                    XmlNode XNode = xmlDoc.SelectSingleNode("//X");
                    XmlNode ONode = xmlDoc.SelectSingleNode("//O");

                    foreach (char c in XNode.InnerText)
                    {
                        buttons[Convert.ToInt32(c.ToString()) - 1].Text = "X";
                    }
                    foreach (char c in ONode.InnerText)
                    {
                        buttons[Convert.ToInt32(c.ToString()) - 1].Text = "O";
                    }
                }
            }
        }
    }
}
