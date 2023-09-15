using ConsoleAppEslastic.Helpers;
using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Options;

namespace ConsoleAppEslastic.Data
{
    public  interface IElasticRepository<T>
    {
        Task<T> Get(string index, string nameIndex);
    }

    public class ElasticRepository<T> : IElasticRepository<T>
    {
        private readonly ElasticSettings _settings;
        private readonly ElasticsearchClient _client;
        public ElasticRepository(IOptions<ElasticSettings> settings)
        {
            _settings = settings.Value;
            _client = ConnectionFactory();
        }

        public async Task<T> Get(string index,string nameIndex)
        {
            var response = await _client.GetAsync<T>(index, idx => idx.Index(nameIndex));
            if (response.IsValidResponse)
            {
                return response.Source?? default!;
            }
            return default!;
        }

        private ElasticsearchClient ConnectionFactory()
        {
            var url = new Uri(_settings.Url);
            var client = new ElasticsearchClient(url);
            return client;
        }
    }
}
