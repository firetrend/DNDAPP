using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNDAPP.Entrails.Mechanics.fightsystem;
using DNDAPP.Entrails.Servis.Data;
using DNDAPP.Entrails.Mechanics;

namespace DNDAPP.Entrails.UI
{
    internal class CombatForm : Form
    {
        private readonly List<Charactres> _characters;
        private readonly Combat _combat = new();

        private Label _turnLabel = null!;
        private ListBox _participantsListBox = null!;
        private ComboBox _targetComboBox = null!;
        private TextBox _logTextBox = null!;

        public CombatForm(List<Charactres> characters)
        {
            _characters = characters;

            Text = "Боевой режим";
            Width = 900;
            Height = 620;
            StartPosition = FormStartPosition.CenterParent;

            CreateCombat();
            CreateControls();
            UpdateView();
        }

        private void CreateCombat()
        {
            foreach (Charactres character in _characters)
            {
                Combatant combatant = CombatantFactory.FromCharacter(character);
                _combat.Participants.Add(combatant);
            }

            _combat.Start();
        }

        private void CreateControls()
        {
            _turnLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Left = 20,
                Top = 20
            };

            _participantsListBox = new ListBox
            {
                Left = 20,
                Top = 60,
                Width = 350,
                Height = 360
            };

            _logTextBox = new TextBox
            {
                Left = 390,
                Top = 60,
                Width = 470,
                Height = 360,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical
            };

            Label targetLabel = new Label
            {
                Text = "Цель атаки:",
                AutoSize = true,
                Left = 20,
                Top = 445
            };

            _targetComboBox = new ComboBox
            {
                Left = 110,
                Top = 441,
                Width = 260,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            Button attackButton = new Button
            {
                Text = "Атаковать",
                Left = 20,
                Top = 500,
                Width = 150,
                Height = 40
            };

            Button nextTurnButton = new Button
            {
                Text = "Следующий ход",
                Left = 190,
                Top = 500,
                Width = 150,
                Height = 40
            };

            Button endButton = new Button
            {
                Text = "Завершить бой",
                Left = 710,
                Top = 500,
                Width = 150,
                Height = 40
            };

            attackButton.Click += AttackButton_Click;
            nextTurnButton.Click += NextTurnButton_Click;
            endButton.Click += EndButton_Click;

            Controls.Add(_turnLabel);
            Controls.Add(_participantsListBox);
            Controls.Add(_logTextBox);
            Controls.Add(targetLabel);
            Controls.Add(_targetComboBox);
            Controls.Add(attackButton);
            Controls.Add(nextTurnButton);
            Controls.Add(endButton);
        }

        private void AttackButton_Click(object? sender, EventArgs e)
        {
            Combatant? attacker = _combat.GetCurrentCombatant();

            if (attacker == null)
            {
                MessageBox.Show("Нет текущего участника боя.");
                return;
            }

            Combatant? target = GetSelectedTarget();

            if (target == null)
            {
                MessageBox.Show("Выберите цель атаки.");
                return;
            }

            int attackBonus = ReadIntDialog("Бонус атаки", "Введите бонус атаки:");

            bool isHit = _combat.AttackCheck(attacker, target, attackBonus);

            if (!isHit)
            {
                UpdateView();
                return;
            }

            int quantity = ReadIntDialog("Урон", "Количество кубов урона:");
            int sides = ReadIntDialog("Урон", "Количество граней у куба:");
            int modifier = ReadIntDialog("Урон", "Модификатор урона:");

            if (quantity <= 0 || sides <= 0)
            {
                MessageBox.Show("Количество кубов и граней должно быть больше 0.");
                _combat.Log.Add("Расчёт урона отменён: некорректные параметры кубов.");
                UpdateView();
                return;
            }

            DiceContext damageRoll = Diceroll.Dice(sides, quantity, modifier);

            _combat.ApplyDamage(target, damageRoll.Total);
            _combat.Log.Add($"Урон рассчитан броском {quantity}d{sides} + {modifier}: {damageRoll.Total}.");

            UpdateView();
        }

        private void NextTurnButton_Click(object? sender, EventArgs e)
        {
            _combat.NextTurn();
            UpdateView();
        }

        private void EndButton_Click(object? sender, EventArgs e)
        {
            _combat.End();
            Close();
        }

        private Combatant? GetSelectedTarget()
        {
            if (_targetComboBox.SelectedItem == null)
                return null;

            string selectedName = _targetComboBox.SelectedItem.ToString()!;

            return _combat.Participants
                .FirstOrDefault(p => p.Name == selectedName);
        }

        private void UpdateView()
        {
            Combatant? current = _combat.GetCurrentCombatant();

            _turnLabel.Text = $"Раунд: {_combat.Round} | Ход: {current?.Name ?? "нет"}";

            _participantsListBox.Items.Clear();

            foreach (Combatant participant in _combat.Participants)
            {
                _participantsListBox.Items.Add(
                    $"{participant.Name} | HP: {participant.CurrentHp}/{participant.MaxHp} | КД: {participant.ArmorClass} | Иниц.: {participant.Initiative}"
                );
            }

            _targetComboBox.Items.Clear();

            foreach (Combatant participant in _combat.Participants)
            {
                if (participant != current)
                    _targetComboBox.Items.Add(participant.Name);
            }

            if (_targetComboBox.Items.Count > 0)
                _targetComboBox.SelectedIndex = 0;

            _logTextBox.Text = string.Join(Environment.NewLine, _combat.Log);
        }

        private int ReadIntDialog(string title, string message)
        {
            while (true)
            {
                string? input = Microsoft.VisualBasic.Interaction.InputBox(
                    message,
                    title,
                    "0"
                );

                if (int.TryParse(input, out int value))
                    return value;

                MessageBox.Show("Введите число.");
            }
        }
    }
}