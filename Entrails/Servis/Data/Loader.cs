using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNDAPP.Entrails.Servis.Data
{
    internal class Loader
    {
            private readonly CharFinder _finder;
            private readonly Parser _parser;

            public Loader(string folderPath)
            {
                _finder = new CharFinder(folderPath);
                _parser = new Parser();
            }

            public List<Charactres> LoadAll()
            {
                List<Charactres> characters = new();

                string[] files = _finder.JFinder();

                foreach (string file in files)
                {
                    Charactres? character = _parser.Parse(file);

                    if (character != null)
                        characters.Add(character);
                }

                return characters;
            }
        
    }
}
