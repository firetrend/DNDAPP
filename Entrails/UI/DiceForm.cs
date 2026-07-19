using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNDAPP.Entrails.Mechanics;

namespace DNDAPP.Entrails.UI
{
    internal class DiceForm : Form
    {
        private NumericUpDown _quantityInput = null!;
        private NumericUpDown _sidesInput = null!;
        private NumericUpDown _modifierInput = null!;
        private TextBox _resultTextBox = null!;

        public DiceForm()
        {
            Text = "Бросок кубов";
            Width = 500;
            Height = 420;
            StartPosition = FormStartPosition.CenterParent;

            CreateControls();
        }

        private void CreateControls()
        {
            Label titleLabel = new Label
            {
                Text = "Бросок кубов",
                AutoSize = true,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Left = 170,
                Top = 20
            };

            Label quantityLabel = new Label
            {
                Text = "Количество кубов:",
                AutoSize = true,
                Left = 40,
                Top = 80
            };

            _quantityInput = new NumericUpDown
            {
                Left = 220,
                Top = 78,
                Width = 120,
                Minimum = 1,
                Maximum = 100,
                Value = 1
            };

            Label sidesLabel = new Label
            {
                Text = "Грани куба:",
                AutoSize = true,
                Left = 40,
                Top = 120
            };

            _sidesInput = new NumericUpDown
            {
                Left = 220,
                Top = 118,
                Width = 120,
                Minimum = 2,
                Maximum = 100,
                Value = 20
            };

            Label modifierLabel = new Label
            {
                Text = "Модификатор:",
                AutoSize = true,
                Left = 40,
                Top = 160
            };

            _modifierInput = new NumericUpDown
            {
                Left = 220,
                Top = 158,
                Width = 120,
                Minimum = -100,
                Maximum = 100,
                Value = 0
            };

            Button rollButton = new Button
            {
                Text = "Бросить",
                Left = 180,
                Top = 210,
                Width = 120,
                Height = 40
            };

            _resultTextBox = new TextBox
            {
                Left = 40,
                Top = 270,
                Width = 400,
                Height = 80,
                Multiline = true,
                ReadOnly = true
            };

            rollButton.Click += RollButton_Click;

            Controls.Add(titleLabel);
            Controls.Add(quantityLabel);
            Controls.Add(_quantityInput);
            Controls.Add(sidesLabel);
            Controls.Add(_sidesInput);
            Controls.Add(modifierLabel);
            Controls.Add(_modifierInput);
            Controls.Add(rollButton);
            Controls.Add(_resultTextBox);
        }

        private void RollButton_Click(object? sender, EventArgs e)
        {
            int quantity = (int)_quantityInput.Value;
            int sides = (int)_sidesInput.Value;
            int modifier = (int)_modifierInput.Value;

            DiceContext result = Diceroll.Dice(sides, quantity, modifier);

            _resultTextBox.Text =
                $"Бросок: {quantity}d{sides} + {modifier}{Environment.NewLine}" +
                $"Кубы: {string.Join(", ", result.Rolls)}{Environment.NewLine}" +
                $"Сумма: {result.Sum}{Environment.NewLine}" +
                $"Итог: {result.Total}";
        }
    }
}
