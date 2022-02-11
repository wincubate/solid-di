using System.IO;
using System.Threading.Tasks;

namespace Wincubate.Solid
{
    class FileStorage : IStorage
    {
        public Task<string> GetDataAsStringAsync(string sourceFilePath)
        {
            return File.ReadAllTextAsync(sourceFilePath);
        }

        public Task StoreDataAsStringAsync(
            string destinationFilePath,
            string outputDataAsString)
        {
            return File.WriteAllTextAsync(destinationFilePath, outputDataAsString);
        }
    }
}
