using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace DNDAPP.Entrails.Servis.Data
{
    internal class Parser
    {

        public Charactres? Parse(string filePath)
        {
            string outerJson = File.ReadAllText(filePath);

            using JsonDocument outerDocument = JsonDocument.Parse(outerJson);
            JsonElement outerRoot = outerDocument.RootElement;

            if (!outerRoot.TryGetProperty("data", out JsonElement dataElement))
                return null;

            string? innerJson = dataElement.GetString();

            if (string.IsNullOrWhiteSpace(innerJson))
                return null;

            using JsonDocument innerDocument = JsonDocument.Parse(innerJson);
            JsonElement characterRoot = innerDocument.RootElement;

            Charactres character = new Charactres
            {
                Name = GetString(characterRoot, "name", "value"),
                Race = GetString(characterRoot, "info", "race", "value"),
                ClassName = GetString(characterRoot, "info", "charClass", "value"),
                Level = GetInt(characterRoot, "info", "level", "value"),

                ArmorClass = GetInt(characterRoot, "vitality", "ac", "value"),
                MaxHp = GetInt(characterRoot, "vitality", "hp-max", "value"),
                CurrentHp = GetInt(characterRoot, "vitality", "hp-current", "value"),
                Speed = GetInt(characterRoot, "vitality", "speed", "value"),

                ProficiencyBonus = GetInt(characterRoot, "proficiency"),

                Abilities = new AbilityScores
                {
                    Strength = GetInt(characterRoot, "stats", "str", "score"),
                    Dexterity = GetInt(characterRoot, "stats", "dex", "score"),
                    Constitution = GetInt(characterRoot, "stats", "con", "score"),
                    Intelligence = GetInt(characterRoot, "stats", "int", "score"),
                    Wisdom = GetInt(characterRoot, "stats", "wis", "score"),
                    Charisma = GetInt(characterRoot, "stats", "cha", "score")
                }
            };

            return character;
        }

        private static string GetString(JsonElement root, params string[] path)
        {
            JsonElement current = root;

            foreach (string part in path)
            {
                if (!current.TryGetProperty(part, out current))
                    return "";
            }

            return current.GetString() ?? "";
        }

        private static int GetInt(JsonElement root, params string[] path)
        {
            JsonElement current = root;

            foreach (string part in path)
            {
                if (!current.TryGetProperty(part, out current))
                    return 0;
            }

            if (current.ValueKind == JsonValueKind.Number)
                return current.GetInt32();

            if (current.ValueKind == JsonValueKind.String &&
                int.TryParse(current.GetString(), out int value))
                return value;

            return 0;
        }
    }
}

