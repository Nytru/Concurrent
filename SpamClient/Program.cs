using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using ConcurentModels.Models;

const string url = "http://localhost:5253/api/add";
var httpClient = new HttpClient();
var bag = new ConcurrentBag<string>();

var st = new Stopwatch();
st.Start();
try
{
    Console.WriteLine("All start");
    const int requestsCount = 40;

    await Parallel.ForAsync(0, requestsCount, async (i, token) =>
    {
        var rq = new AddRequest(i);
        using var ctnt = new StringContent(JsonSerializer.Serialize(rq), Encoding.Default, mediaType: "application/json");
        var message = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = ctnt,
        };
        using var response = await httpClient.SendAsync(message);
        var str = await response.Content.ReadAsStringAsync(token);
        bag.Add(str);
    });
    st.Stop();
    Console.WriteLine($"All end: count: {bag.Count} time taken: {st.Elapsed}");
}
finally
{
    Console.WriteLine("Dispose");
    httpClient.Dispose();
}
