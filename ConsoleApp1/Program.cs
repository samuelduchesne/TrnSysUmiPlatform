using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistrictEnergy;

namespace DistrictEnergy
{
    class Program
    {
        static void Main(string[] args)
        {
            //double timestep = 0.25;
            //var a = new List<object>();

            //var f1 = new ControlCards(0, 8760, timestep);
            //a.Add(f1);

            var trnsys_model = new TrnsysModel();
            trnsys_model.PlantSelection = "Plant 1";
            trnsys_model.ModelName = "MyModel";
            trnsys_model.WeatherFile = "CAN_PQ_Montreal.Intl.AP.716270_CWEC";
            trnsys_model.HourlyTimestep = 0.25;

            //var b = new CreateDckFile(trnsys_model);

            var b = new WriteDckFile(trnsys_model);

            var c = new RunTrnsys(trnsys_model);

            //Console.WriteLine("There are " + a.Count + " items");

            //Console.WriteLine("Press enter to close...");
            //Console.ReadLine();
        }
    }
}
