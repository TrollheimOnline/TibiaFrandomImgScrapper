using System.Net;

async Task DownloadImageAsync(string directoryPath, string fileName, Uri uri)
{
    using var httpClient = new HttpClient();
    var path = Path.Combine(directoryPath, $"{fileName}");

    // Download the image and write to the file
    var imageBytes = await httpClient.GetByteArrayAsync(uri);
    await File.WriteAllBytesAsync(path, imageBytes);
}

string directoryPath = "imgs";

WebClient webClient = new WebClient();
string page = webClient.DownloadString("https://tibia.fandom.com/wiki/List_of_Creatures");
HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
doc.LoadHtml(page);

var listOfHtmlMonsters = doc.DocumentNode.SelectSingleNode("//table[@class='wikitable sortable']")
.Descendants("tr")
.Skip(1)
.Where(tr => tr.Elements("td").Count() > 1)
.Select(tr => tr.Elements("td"));

List<KeyValuePair<string, string>> ListOfMonsters = new List<KeyValuePair<string, string>>();

foreach (var node in listOfHtmlMonsters)
{
    string monsterName = node.FirstOrDefault().InnerText;
    var image = node.Skip(1).FirstOrDefault().ChildNodes[0].Attributes[0];
    var imageUrlString = image.Value.ToString().Replace("&amp;","&");
    ListOfMonsters.Add(new KeyValuePair<string, string>(monsterName, imageUrlString));
}

int monstersAmount = ListOfMonsters.Count;
int index = 0;
Directory.CreateDirectory(directoryPath);
Console.WriteLine("\n>>  Tibia Fandom monster image scrapper  <<\n");
Console.WriteLine("Downloading " + ListOfMonsters.Count + " monster images...");
foreach (var monster in ListOfMonsters)
{
    ++index;
    if (monster.Value.Contains("static.wikia.nocookie.net/tibia/images/"))
    {
        Console.WriteLine(String.Concat("(", index, "/", monstersAmount, ")", "\t", monster.Key));
        await DownloadImageAsync(directoryPath, monster.Key.ToLower() + ".webp", new Uri(monster.Value));
    }
    else
        Console.WriteLine(String.Concat("Skipped - (", index, "/", monstersAmount, ")", "\t", monster.Key, " - Skipped - there is no image file!"));
}

Console.WriteLine("Everything is done. Hit Enter to close the console.");
Console.ReadLine();