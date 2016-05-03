using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe
{
    [PlayerAttributeMapping(
        NameProperty = "Name",
        AuthorProperty = "Author"
    )]
    public sealed class PrimordialPlayerAttribute : PlayerAttribute
    {
        public string Name { get; set; }
        public string Author { get; set; }
    }
}
