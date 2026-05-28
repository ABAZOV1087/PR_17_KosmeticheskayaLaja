using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR_17_KosmeticheskayaLaja

{
    public static class Core
    {
        public static KosmeticheskayaLajaEntities1 Context = new KosmeticheskayaLajaEntities1();
        public static Users CurrentUser { get; set; }
        public static List<Products> Cart { get; set; } = new List<Products>();
    }
}
