using ImageMagick;


string imgsDirectory = "imgs";
string gifDirectory = "gifs";
int index = 0;

Console.WriteLine("\n>>  Webp to Gif Converter  <<\n");
try
{
    var imageFiles = Directory.GetFileSystemEntries(imgsDirectory);
    int filesAmount = imageFiles.Count();
    if (filesAmount > 0)
    {
        Console.WriteLine("Converting " + imageFiles.Count() + " image files...");
        Thread.Sleep(1500);
        Directory.CreateDirectory(gifDirectory);
        foreach (var imageFile in imageFiles)
        {
            var image = new FileInfo(imageFile);
            var animatedImageConverted = new MagickImageCollection(imageFile.ToString());
            index++;
            Console.WriteLine(String.Concat("(",index,"/",filesAmount,")\t",image.Name));
            animatedImageConverted.Write(gifDirectory + "/" + image.Name.Replace(image.Extension, ".gif"), MagickFormat.Gif);
        }
        Console.WriteLine("Everything is done. Hit Enter to close the console.");
    }
    else
        Console.WriteLine("There is no files inside imgs folder.");
}
catch (Exception ex)
{
    Console.WriteLine("There is no \"imgs\" folder in this directory.");
}
Console.ReadLine();