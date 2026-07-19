using DNDAPP.Entrails.Servis.Data;
using DNDAPP.Entrails.UI;

namespace DNDAPP.UI
{
    internal class MainForm : Form
    {
        private readonly Loader _loader;
        private List<Charactres> _characters = new();

        public MainForm(Loader loader)
        {
            _loader = loader;

            Text = "DnD DM Assistant";
            Width = 700;
            Height = 500;
            StartPosition = FormStartPosition.CenterScreen;

            CreateControls();
        }

        private void CreateControls()
        {
            Label titleLabel = new Label
            {
                Text = "DnD DM Assistant",
                AutoSize = true,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Left = 230,
                Top = 30
            };

            Button loadButton = new Button
            {
                Text = "Загрузить персонажей",
                Width = 220,
                Height = 50,
                Left = 100,
                Top = 120
            };

            Button showCharactersButton = new Button
            {
                Text = "Показать персонажей",
                Width = 220,
                Height = 50,
                Left = 380,
                Top = 120
            };

            Button diceButton = new Button
            {
                Text = "Бросок кубов",
                Width = 220,
                Height = 50,
                Left = 100,
                Top = 200
            };

            Button combatButton = new Button
            {
                Text = "Боевой режим",
                Width = 220,
                Height = 50,
                Left = 380,
                Top = 200
            };

            Button exitButton = new Button
            {
                Text = "Выход",
                Width = 220,
                Height = 50,
                Left = 240,
                Top = 330
            };

            loadButton.Click += LoadButton_Click;
            showCharactersButton.Click += ShowCharactersButton_Click;
            diceButton.Click += DiceButton_Click;
            combatButton.Click += CombatButton_Click;
            exitButton.Click += (sender, args) => Close();

            Controls.Add(titleLabel);
            Controls.Add(loadButton);
            Controls.Add(showCharactersButton);
            Controls.Add(diceButton);
            Controls.Add(combatButton);
            Controls.Add(exitButton);
        }

        private void LoadButton_Click(object? sender, EventArgs e)
        {
            _characters = _loader.LoadAll();

            MessageBox.Show(
                $"Загружено персонажей: {_characters.Count}",
                "Загрузка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void ShowCharactersButton_Click(object? sender, EventArgs e)
        {
            if (_characters.Count == 0)
            {
                MessageBox.Show("Сначала загрузите персонажей.");
                return;
            }

            CharacterForm characterForm = new CharacterForm(_characters);
            characterForm.ShowDialog();
        }

        private void DiceButton_Click(object? sender, EventArgs e)
        {
            DiceForm diceForm = new DiceForm();
            diceForm.ShowDialog();
        }

        private void CombatButton_Click(object? sender, EventArgs e)
        {
            if (_characters.Count == 0)
            {
                MessageBox.Show("Сначала загрузите персонажей.");
                return;
            }

            if (_characters.Count < 2)
            {
                MessageBox.Show("Для боевого режима нужно минимум 2 персонажа.");
                return;
            }

            CombatForm combatForm = new CombatForm(_characters);
            combatForm.ShowDialog();

        }
    }
}