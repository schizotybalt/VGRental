
using System.Collections.Generic;

namespace VGRental.Model
{
    public class System
    {
        public string Name { get; set; }
        public string Manf { get; set; }
        public string Version { get; set; }
        public virtual ICollection<Game> Games { get; set; }
    }
}
