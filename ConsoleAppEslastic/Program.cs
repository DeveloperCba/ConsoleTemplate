

using ConsoleAppEslastic;
using ConsoleAppEslastic.Configurations;
using ConsoleAppEslastic.Helpers;
using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.ComponentModel.Design;


//DI
var serviceProvider = DependencieInjectionConfig.ConfigureService() as ServiceProvider;
var elasticSettgins = serviceProvider.GetService<IOptions<ElasticSettings>>().Value;

var url = new Uri(elasticSettgins.Url);
var client = new ElasticsearchClient(url);


var people = new People
{
    Id = 10,
    Name = "Breno",
    Description = "Morais teste",
};


//var response = await client.IndexAsync(people, "my-tweet-indexteste");

//if (response.IsValidResponse)
//{
//    Console.WriteLine($"Index document with ID {response.Id} succeeded.");
//}

//if (!response.IsValidResponse)
//{
//    // Handle errors
//    var debugInfo = response.DebugInformation;
//    var error = response.ElasticsearchServerError!.Error;
//}


// Get a list of all indices.
var indices = client.Indices.Get("customer");

// Iterate over the indices and print them to the console.
foreach (var index in indices.Indices.ToList())
{
    Console.WriteLine($"Key: {index.Key} - Values: {index.Value}");
}

//var 
//var response3 = await client.GetAsync<People>(10, idx => idx.Index("my-tweet-indexteste"));
var response33 = await client.GetAsync<object>(1, idx => idx.Index("customer"));


if (response33.IsValidResponse)
{
    var tweet = response33.Source;
}


Console.ReadKey();