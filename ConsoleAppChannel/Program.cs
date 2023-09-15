using ConsoleAppChannel.Models;
using System.Threading.Channels;


// Padrões de criação não associados
var channelUnbounded = Channel.CreateUnbounded<Coordinates>(
    new UnboundedChannelOptions
    {
        SingleReader = false, 
        SingleWriter = false,
        AllowSynchronousContinuations = true
    }
);



//  Padrões de criação limitada
var channelBounded = Channel.CreateBounded<Coordinates>(
    new BoundedChannelOptions(200)
    {
        SingleWriter = true,
        SingleReader = false,
        AllowSynchronousContinuations = false,
        FullMode = BoundedChannelFullMode.DropWrite
    }
);




static void ProduceWithWhileAndTryWrite(
    ChannelWriter<Coordinates> writer, Coordinates coordinates)
{
    while (coordinates is { Latitude: < 90, Longitude: < 180 })
    {
        var tempCoordinates = coordinates with
        {
            Latitude = coordinates.Latitude + .5,
            Longitude = coordinates.Longitude + 1
        };

        if (writer.TryWrite(item: tempCoordinates))
        {
            coordinates = tempCoordinates;
        }
    }
}