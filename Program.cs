using MediaToolkit.Services;
using MediaToolkit.Tasks;

if (args.Length == 0) throw new Exception("Argument needed");

var ffmpegPath = AppDomain.CurrentDomain.BaseDirectory + "ffmpeg\\bin\\ffmpeg.exe";

var videoInputPath = args[0];
string imageOutputPath;

if (args.Length == 2)
{
    // Output path provided by the user
    imageOutputPath = args[1];
}
else
{
    // Create the output directory in the root path by default
    imageOutputPath = AppDomain.CurrentDomain.BaseDirectory + "img";
    Directory.CreateDirectory(imageOutputPath);
}

Console.WriteLine($"Images output path : {imageOutputPath}");
Console.WriteLine($"Video input path : {videoInputPath}");

// Initialization of MediaToolkitService
var mediaToolkitService = MediaToolkitService.CreateInstance(ffmpegPath);
var metadataTask = new FfTaskGetMetadata(videoInputPath);
var metadata = await mediaToolkitService.ExecuteAsync(metadataTask);

Console.WriteLine("Starting extraction");

var i = 0;
string duration;

// Get the duration
if (metadata.Metadata.Streams.First().Duration.Contains('.'))
{
    duration = metadata.Metadata.Streams.First().Duration.Split('.') [0];
}
else
{
    duration = metadata.Metadata.Streams.First().Duration;
}

// Extracting frames
while (i < int.Parse(duration))
{
    var outputFile = string.Format("{0}\\image-{1:0000}.jpeg", imageOutputPath, i);
    var thumbTask = new FfTaskSaveThumbnail(videoInputPath, outputFile, TimeSpan.FromSeconds(i));
    _ = await mediaToolkitService.ExecuteAsync(thumbTask);
    Console.WriteLine($"File created : {outputFile}");
    i++;
}

Console.WriteLine("Extraction completed");
Console.Read();
Environment.Exit(0);