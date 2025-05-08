using Newtonsoft.Json;
using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Configurator
{
    class FoodOptions
    {
        public string name { get; set; }
        public string id { get; set; }
        public int kCal { get; set; }        
        public int quality { get; set; }
        public bool canRot { get; set; }
        public int cyclesUntilRot { get; set; }
        public int fridgeTempC { get; set; }
        public int freezeTempC { get; set; }
        public List<string> effects { get; set; }



        public FoodOptions(string name, string id, int kCal, int quality, bool canRot, int cyclesUntilRot, int fridgeTempC, int freezeTempC, List<string> effects)
        {
            this.name = name;
            this.id = id;
            this.kCal = kCal;
            this.quality = quality;           
            this.canRot = canRot;
            this.cyclesUntilRot = cyclesUntilRot;
            this.fridgeTempC = fridgeTempC;
            this.freezeTempC = freezeTempC;
            this.effects = effects;
        }


    }
}
