using System.Collections.Generic;
using System.Threading.Tasks;
using Wincubate.Solid.DomainLayer;

namespace Wincubate.Solid
{
    class StockAnalyzer
    {
        private readonly IReadStorage _readStorage;
        private readonly IWriteStorage _writeStorage;
        private readonly IParser _parser;
        private readonly ISerializer _serializer;

        public StockAnalyzer()
        {
            _readStorage = new WebStorage(@"http://solid.wincubate.net/stockpositions.json");
            //_writeStorage = new FileStorage( @"..\..\..\..\Files\StockPositions1.csv",@"..\..\..\..\Files\Result.json" );
            _writeStorage = new ConsoleStorage();
            //_storage = new FileStorage();
            //_parser = new CsvParser();
            _parser = new JsonParser();
            //_serializer = new CsvSerializer();
            _serializer = new JsonSerializer();
        }

        public async Task ProcessAsync()
        {
            string inputDataAsString = await _readStorage.GetDataAsStringAsync();
            IEnumerable<StockPosition> stockPositions = _parser.Parse(inputDataAsString);

            Computation computation = new Computation();
            IEnumerable<StockPosition> output = computation.Execute(stockPositions);

            string outputDataAsString = _serializer.SerializeData(output);

            await _writeStorage.StoreDataAsStringAsync(outputDataAsString);
        }
    }
}
