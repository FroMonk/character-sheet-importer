using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CharacterSheet.Pathfinder.XMLReader
{
    public interface ICharacterParser
    {
        CharacterStats parseCharacterXML(Stream inputStream);
    }
}
