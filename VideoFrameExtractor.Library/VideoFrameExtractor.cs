using MediaToolkit.Services;
using MediaToolkit.Tasks;

namespace VideoFrameExtractor.Library
{
    /// <summary>
    /// VideoFrameExtractor class
    /// </summary>
    public class VideoFrameExtractor
    {
        private readonly string? _videoInputPath;
        private readonly string? _ffmpegPath;
        private string? _imageOutputPath;

        /// <summary>
        /// Constructor of VideoFrameExtractor class
        /// </summary>
        /// <param name="videoInputPath">The video path for frame extraction</param>
        /// <param name="ffmpegPath">The ffmpeg executable path</param>
        /// <param name="imageOutputPath">The image (jpeg) output path - optional</param>
        public VideoFrameExtractor(string videoInputPath, string ffmpegPath, string imageOutputPath = "")
        {
            _videoInputPath = videoInputPath;
            _ffmpegPath = ffmpegPath;
            _imageOutputPath = imageOutputPath;
        }

        /// <summary>
        /// Start extraction : it extracts one frame (jpeg) per second to the ouput folder. 
        /// </summary>
        /// <exception cref="Exception">When the video or ffmpeg path are not provided</exception>
        public async void StartExtractionAsync()
        {
            if (string.IsNullOrEmpty(_videoInputPath) || string.IsNullOrEmpty(_ffmpegPath)) throw new Exception("Argument videoInputPath needed");

            if (string.IsNullOrEmpty(_imageOutputPath))
            {
                // Create the output directory in the root path by default
                _imageOutputPath = AppDomain.CurrentDomain.BaseDirectory + "img";
                Directory.CreateDirectory(_imageOutputPath);
            }

            // Initialization of MediaToolkitService
            var mediaToolkitService = MediaToolkitService.CreateInstance(_ffmpegPath);
            var metadataTask = new FfTaskGetMetadata(_videoInputPath);
            var metadata = await mediaToolkitService.ExecuteAsync(metadataTask);

            var i = 0;
            string duration;

            // Get the duration
            if (metadata.Metadata.Streams.First().Duration.Contains('.'))
            {
                duration = metadata.Metadata.Streams.First().Duration.Split('.')[0];
            }
            else
            {
                duration = metadata.Metadata.Streams.First().Duration;
            }

            // Extracting frames
            while (i < int.Parse(duration))
            {
                var outputFile = string.Format("{0}\\image-{1:0000}.jpeg", _imageOutputPath, i);
                var thumbTask = new FfTaskSaveThumbnail(_videoInputPath, outputFile, TimeSpan.FromSeconds(i));
                _ = await mediaToolkitService.ExecuteAsync(thumbTask);
                i++;
            }
        }
    }
}