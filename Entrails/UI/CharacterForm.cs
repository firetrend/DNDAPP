using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNDAPP.Entrails.Servis.Data;

namespace DNDAPP.Entrails.UI
{
    internal class CharacterForm : Form
    {
        private readonly List<Charactres> _characters;

        public CharacterForm(List<Charactres> characters)
        {
            _characters = characters;

            Text = "Персонажи";
            Width = 700;
            Height = 500;
            StartPosition = FormStartPosition.CenterParent;

            CreateControls();
        }

        private void CreateControls()
        {
            ListBox charactersListBox = new ListBox
            {
                Left = 20,
                Top = 20,
                Width = 250,
                Height = 400
            };

            TextBox characterInfoTextBox = new TextBox
            {
                Left = 290,
                Top = 20,
                Width = 360,
                Height = 400,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical
            };

            foreach (Charactres character in _characters)
            {
                charactersListBox.Items.Add(character.Name);
            }

            charactersListBox.SelectedIndexChanged += (sender, args) =>
            {
                int index = charactersListBox.SelectedIndex;

                if (index < 0)
                    return;

                Charactres character = _characters[index];

                characterInfoTextBox.Text =
                    $"Имя: {character.Name}{Environment.NewLine}" +
                    $"Раса: {character.Race}{Environment.NewLine}" +
                    $"Класс: {character.ClassName}{Environment.NewLine}" +
                    $"Уровень: {character.Level}{Environment.NewLine}" +
                    $"HP: {character.CurrentHp}/{character.MaxHp}{Environment.NewLine}" +
                    $"КД: {character.ArmorClass}{Environment.NewLine}" +
                    $"Скорость: {character.Speed}{Environment.NewLine}" +
                    $"{Environment.NewLine}" +
                    $"Сила: {character.Abilities.Strength}{Environment.NewLine}" +
                    $"Ловкость: {character.Abilities.Dexterity}{Environment.NewLine}" +
                    $"Телосложение: {character.Abilities.Constitution}{Environment.NewLine}" +
                    $"Интеллект: {character.Abilities.Intelligence}{Environment.NewLine}" +
                    $"Мудрость: {character.Abilities.Wisdom}{Environment.NewLine}" +
                    $"Харизма: {character.Abilities.Charisma}";
            };

            Controls.Add(charactersListBox);
            Controls.Add(characterInfoTextBox);
        }
    }
}