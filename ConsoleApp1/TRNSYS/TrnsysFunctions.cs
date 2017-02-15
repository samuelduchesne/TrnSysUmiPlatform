using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistrictEnergy;
using System.IO;
using System.Diagnostics;

namespace DistrictEnergy
{
    class RunTrnsys
    {
        public RunTrnsys(TrnsysModel trnsys_model)
        {
            string deckfilename = Path.Combine(CreateDckFile.get_trnsys_file_path(), trnsys_model.ModelName + ".dck");
            string command = CreateDckFile.get_trnsys_exe() + deckfilename + "/n";
            int exitCode;
            ProcessStartInfo processInfo;
            Process process;

            processInfo = new ProcessStartInfo("cmd.exe", command);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            // Redirect the outputs
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            process = Process.Start(processInfo);
            process.WaitForExit();

            // Read the streams
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            exitCode = process.ExitCode;

            Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
            Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
            Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExeciteCommand");
            process.Close();
        }
    }

    class WriteDckFile
    {
        public WriteDckFile(TrnsysModel trnsys_model)
        {
            string filename = Path.Combine(CreateDckFile.get_trnsys_file_path(), trnsys_model.ModelName + ".dck");

            using (StreamWriter file = new StreamWriter(filename, false))
            {
                // Write the begining of the deck file.
                Intro intro = new Intro(trnsys_model.ProjectCreator, trnsys_model.CreationDate, trnsys_model.ModifiedDate, trnsys_model.Description);
                file.WriteLine(intro.WriteIntro());

                // Write the control cars.
                ControlCards controlcards = new ControlCards(0, 8760, trnsys_model.HourlyTimestep);
                file.WriteLine(controlcards.WriteControlCards());

                
                // Write Weather
                Type15 weather = new Type15(trnsys_model.WeatherFile);
                file.WriteLine(weather.WriteType());

                Type15_3 weather2 = new Type15_3(trnsys_model.WeatherFile);
                file.WriteLine(weather2.WriteType());

                // Write Type31
                Type31 pipe = new Type31(new int[,] { { 0, 0 }, { 0, 0 }, { weather2.Unit_number, 2 } }, 0.5, 525, 3, 998, 4.31, 10);
                file.WriteLine(pipe.WriteType());

                file.WriteLine("END\\n");
                file.Close();
            }
        }
    }
}