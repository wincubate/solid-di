using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wincubate.Solid.DomainLayer;

namespace Wincubate.Solid
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string sourceFilePath = @"..\..\..\..\Files\StockPositions1.csv";
            string inputDataAsString = await GetDataAsStringAsync(sourceFilePath);

            IEnumerable<StockPosition> stockPositions = Parse(inputDataAsString);

            IEnumerable<StockPosition> output = Execute(stockPositions);

            string outputDataAsString = SerializeData(output);

            string destinationFilePath = @"..\..\..\..\Files\Result.csv";
            await StoreDataAsStringAsync(destinationFilePath, outputDataAsString);
        }

        static Task<string> GetDataAsStringAsync(string sourceFilePath)
        {
            return File.ReadAllTextAsync(sourceFilePath);
        }

        static Task StoreDataAsStringAsync(
            string destinationFilePath,
            string outputDataAsString)
        {
            return File.WriteAllTextAsync(destinationFilePath, outputDataAsString);
        }

        static IEnumerable<StockPosition> Parse(string dataAsString)
        {
            try
            {
                return dataAsString
                    .Split('\n', '\r', '\t')
                    .Where(s => string.IsNullOrWhiteSpace(s) == false)
                    .Select(line => line.Split(','))
                    .Select(parts => new StockPosition(
                        parts[0],
                        int.Parse(parts[1]))
                    )
                    .ToList()
                    ;
            }
            catch (Exception exception)
            {
                string message = $"Could not parse CSV string: \"{dataAsString}\"";
                throw new StockException(message, exception);
            }
        }

        static IEnumerable<StockPosition> Execute(IEnumerable<StockPosition> inputData)
        {
            return inputData
                .GroupBy(stockPosition => stockPosition.Ticker)
                .Select(group => new StockPosition(
                    group.Key,
                    group.Sum(stockPosition => stockPosition.Size))
                )
                .ToList()
                ;
        }

        static string SerializeData(IEnumerable<StockPosition> outputData)
        {
            IEnumerable<string> outputLines = outputData
                .Select(stockPosition => $"{stockPosition.Ticker},{stockPosition.Size}")
                ;
            return string.Join(Environment.NewLine, outputLines);
        }
    }
}
