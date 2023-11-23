using System.Net;
using System.IO;

namespace OphalenWeerkaartenKnmi;

[TestClass]
public class UnitTest1
{
    private const string baseUrl = "https://cdn.knmi.nl/knmi/map/page/klimatologie/daggegevens/weerkaarten/analyse_{0}00.gif";

    [TestMethod]
    public async Task LeesWeerkaartenQuickAndDirty()
    {
        var epoch = new DateTime(2023, 1, 1);
        for (int dag = 0; dag<=365; dag++)
        {
            var current = epoch.AddDays(dag);

            var url = string.Format(baseUrl, current.ToString("yyyyMMdd"));
            var fileName = "c:\\git\\weerdata\\Analyse_" + current.ToString("yyyyMMdd") + ".gif";
            Console.WriteLine("Downloading " + url + " to " + fileName);
            await DownloadFile(url, fileName);
        }
        Assert.IsTrue(File.Exists("Analyse_20230101.gif"));
        Assert.IsTrue(File.Exists("Analyse_20231231.gif"));
    }

    private readonly HttpClient client = new HttpClient();

    public async Task DownloadFile(string remoteFilename, string localFilename)
    {

        try
        {
            // Create a request for the specified remote file name
            var request = await client.GetAsync(remoteFilename);
            if (request != null)
            {
                // Send the request to the server and retrieve the
                // WebResponse object 
                var response = await request.Content.ReadAsStreamAsync();
                if (response != null)
                {
                    // Once the WebResponse object has been retrieved,
                    // get the stream object associated with the response's data
                    var localStream = File.Create(localFilename);
                    await response.CopyToAsync(localStream);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

    }
}