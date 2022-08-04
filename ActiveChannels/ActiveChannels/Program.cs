using ActiveChannels.Models;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ActiveChannels
{
    class Program
    {
        public static void GetActiveTimes (string inputPath, List<ActiveChannel> channels)
        {
            List<ActiveTime> times = new List<ActiveTime>();
            bool firstRow = true;
            bool firstColumn = true;
            // Parsing CSV
            using (TextFieldParser parser = new TextFieldParser(inputPath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    // Processing row
                    string[] fields = parser.ReadFields();

                    if (firstRow)
                    {
                        foreach (var field in fields)
                        {
                            // If first row and first column then skip
                            if (firstColumn)
                            {
                                firstColumn = false;
                                continue;
                            }
                            // Fill times with hours from first row
                            else
                            {
                                var activeTime = new ActiveTime(field, false);
                                times.Add(activeTime);
                            }
                        }
                        firstRow = false;
                    }
                    else
                    {
                        if (fields.Length == 1)
                            continue;
                        firstColumn = true;
                        var index = 0;
                        var channel = new ActiveChannel();
                        foreach (string field in fields)
                        {
                            // firstColumn contains name of channel
                            if (firstColumn)
                            {
                                List<ActiveTime> activeTimes = times.ConvertAll(x => new ActiveTime(x)).ToList();
                                channel = new ActiveChannel
                                {
                                    Name = "Channel " + field,
                                    ActiveTimes = activeTimes
                                };
                                firstColumn = false;
                            }
                            else
                            {
                                // Handling exception in case number of columns does not match
                                if (fields.Length - 1 != times.Count)
                                {
                                    Console.WriteLine("Error: Number of columns does not match.");
                                    return;
                                }
                                // Checking active status for each hour and updating it's value
                                channel.ActiveTimes[index].Active = field == "Active";
                                // Incrementing index for the next hour
                                index++;
                            }
                        }
                        channels.Add(channel);
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            var inputPath = Directory.GetCurrentDirectory() + @"/Input.csv";
            var channels = new List<ActiveChannel>();

            // Calling function for parsing CSV and processing data
            GetActiveTimes(inputPath, channels);

            // Printing the output
            foreach(var channel in channels)
            {
                var activeTimes = channel.ActiveTimes.Where(x => x.Active).Select(x => x.Time).ToList();
                Console.Write(channel.Name + " active hours: " + String.Join(", ", activeTimes));
                Console.WriteLine();
            }
        }
    }
}
