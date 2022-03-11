using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Concurrent;

namespace AuditSample.Providers
{
    public class ValuesProvider : IValuesProvider
    {
        private static Random _random = new Random();
        private static IDictionary<int, string> _data = new ConcurrentDictionary<int, string>();

        public async Task<string> GetAsync(int id)
        {
            _data.TryGetValue(id, out string value);
            return await Task.FromResult(value);
        }

        public IEnumerable<string> GetValues()
        {
            return _data.Values;
        }

        public async Task<int> InsertAsync(string value)
        {
            int key = _random.Next();
            _data[key] = value;
            return await Task.FromResult(key);
        }

        public async Task ReplaceAsync(int id, string value)
        {
            _data[id] = value;
            await Task.CompletedTask;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await Task.FromResult(_data.Remove(id));
        }

        public async Task<int> DeleteMultipleAsync(int[] ids)
        {
            int c = 0;
            foreach (int id in ids)
            {
                c += await DeleteAsync(id) ? 1 : 0;
            }
            return c;
        }

    }
}
