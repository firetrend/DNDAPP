using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNDAPP.Entrails.Servis.Data
{
    internal class CharFinder
    {
        private readonly string _folderPath;

        public CharFinder(string folderPath)
        {
            _folderPath = folderPath;
        }

        public string[] JFinder()
        {
            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
                return Array.Empty<string>();
            }

            return Directory.GetFiles(_folderPath, "*.json");
        }

    }
}
