# VideoFrameExtractor
CLI tool to extract frames from a video. FFMPEG embedded in the executable. 

## Use

Open a CMD in the app folder then execute this command and provide your video directory : 

> VideoFrameExtractor.exe "full-path-to-your-video"

And eventually you can provide the output path :

> VideoFrameExtractor.exe "full-path-to-your-video" "your-output-path"

By default the frames are created in the app folder under "img" directory. 

## Requirements to dev

- [ffmpeg](https://ffmpeg.org/) (Put in bin folder in ffmpeg folder of the project) 

## Stack & libraries

- .NET 6 
- [MediaToolkit.NetCore](https://github.com/mtebenev/MediaToolkit.NetCore)
