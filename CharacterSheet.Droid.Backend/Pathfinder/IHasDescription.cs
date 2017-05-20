using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    public interface IHasDescription
    {
        string Name { get; }
        string Description { get; set; }
    }
}
