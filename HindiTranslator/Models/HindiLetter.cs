using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Models
{

    class HindiLetter : IHindiLetter
    {
        public string letter { get; set; } = string.Empty;
        public string readable { get; set; } = string.Empty;
    }
}
