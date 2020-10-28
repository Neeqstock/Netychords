using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using HANDMIs_TestSuite.Utils;
using HANDMIsTestSuite.Modules.Polling;
using System.Linq;

namespace HANDMIsTestSuite.Printer
{
    public class PrinterModule
    {
        private string baseDirectory = @"C:\HANDMIs_TestSuite";
        private string failureString = "_FAILURE";
        private string ValuesFileName = @"Values.csv";

        public string BaseDirectory { get => baseDirectory; set => baseDirectory = value; }

        public void MakeBaseDirectory()
        {
            Directory.CreateDirectory(baseDirectory);
        }

        public void MakeUserDirectory(string SubjectName)
        {
            Directory.CreateDirectory(BaseDirectory + @"\" + SubjectName);
        }

        public void MakeTestDirectory(string SubjectName, string TestName)
        {
            Directory.CreateDirectory(BaseDirectory + @"\" + SubjectName + @"\" + TestName);
        }

        public void PrintList(List<StopWatchStamp> PollsList, string SubjectName, string TestName, int PollNumber)
        {
            Rack.DMIBox.TestStarted = false;
            MakeTestDirectory(SubjectName, TestName);
            using (var writer = new StreamWriter(BaseDirectory + @"\" + SubjectName + @"\" + TestName + @"\" + PollNumber.ToString() + ".csv"))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords(PollsList);
            }
            Rack.DMIBox.TestStarted = true;
        }

        public void PrintFailure(List<StopWatchStamp> PollsList, string SubjectName, string TestName, int PollNumber)
        {
            Rack.DMIBox.TestStarted = false;
            MakeTestDirectory(SubjectName, TestName);
            using (var writer = new StreamWriter(BaseDirectory + @"\" + SubjectName + @"\" + TestName + @"\" + PollNumber.ToString() + failureString + ".csv"))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords(PollsList);
            }
            Rack.DMIBox.TestStarted = true;
        }

        public void PrintTargetDistancesFile()
        {
            using (var writer = new StreamWriter(BaseDirectory + @"\Values.csv"))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords(Rack.DMIBox.TargetDistances);
            }
        }

        public void ReadTargetDistancesFile()
        {
            try
            {
                using (var reader = new StreamReader(BaseDirectory + @"\" + ValuesFileName))
                using (var csv = new CsvReader(reader))
                {
                    Rack.DMIBox.TargetDistances = csv.GetRecords<TargetDistance>().ToList();
                }
            }
            catch
            {
                throw new Exception("Unable to read the target distances file at " + baseDirectory + @"\" + ValuesFileName);
            }
            
        }
    }
}
