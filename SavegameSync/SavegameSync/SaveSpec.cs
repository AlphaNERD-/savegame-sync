using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavegameSync
{
    public class SaveSpec
    {
        public string GameName { get; private set; }

        /// <summary>
        /// A list of file or directory paths, relative to the game's install directory, to include
        /// in the savegame
        /// </summary>
        public string[] SavePaths { get; private set; }

        /// <summary>
        /// A list of allowed file extensions within the specified directories
        /// </summary>
        public string[] FileExtensions { get; private set; }

        public SaveSpec(string gameName, string[] savePaths, string[] fileExtensions)
        {
            GameName = gameName;
            SavePaths = savePaths;
            FileExtensions = fileExtensions;
        }

    }
}
