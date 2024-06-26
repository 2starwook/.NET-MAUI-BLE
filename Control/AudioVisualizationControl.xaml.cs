using SkiaSharp;
using NAudio.Wave;

using NET_MAUI_BLE.API;
using System.Diagnostics;

namespace NET_MAUI_BLE.Controls;

public partial class AudioVisualizationControl : ContentView
{
    public AudioVisualizationControl()
    {
        InitializeComponent();
        Loaded += OnLoading;
        Unloaded += OnUnloading;
    }

    ~AudioVisualizationControl()
    {
    }

    private string name;
    private string imagePath;
    private ImageSource audioImageSource;
    private WaveFileReader reader;
    public string AudioSource
    {
        get => GetValue(AudioSourceProperty) as string;
        set => SetValue(AudioSourceProperty, value);
    }
    public static readonly BindableProperty AudioSourceProperty = BindableProperty.Create(
        nameof(AudioSource), typeof(string), typeof(AudioVisualizationControl),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (AudioVisualizationControl)bindable;
        }
        );
    public ImageSource AudioImageSource
    {
        get => audioImageSource;
        set
        {
            audioImageSource = value;
            OnPropertyChanged(nameof(AudioImageSource));
        }
    }


    private async void OnLoading(object sender, EventArgs e)
    {
        this.name = FileAPI.GetFileName(AudioSource, false);
        this.imagePath = FilesystemAPI.GetImageFilePath(name);
        if (!FilesystemAPI.IsImageExist(name))
        {
            await StartVisualization(AudioSource);
        }
        MainThread.BeginInvokeOnMainThread(() =>
        {
            UpdateAudioImageSource();
        });
    }

    private void OnUnloading(object sender, EventArgs e)
    {
        AudioImageSource = ImageSource.FromFile("spinner.png");
    }

    private async Task StartVisualization(string wavFilePath)
    {
        // Initialize the WaveFileReader to read audio data
        var stream = new FileStream(wavFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        try
        {
            reader = new WaveFileReader(stream);
        }
        catch (FormatException)
        {
            Debug.WriteLine($"Given file format isn't WAVE, instance of RIFF");
            return;
        }

        SKImage wavImage = await Task.FromResult(CalculateWaveformImage(reader));
        SaveWaveformImageToFile(wavImage);

        stream.Close();
    }

    private SKImage CalculateWaveformImage(WaveFileReader reader)
    {
        // Check if the audio file is mono or stereo
        if (reader.WaveFormat.Channels != 1)
        {
            Console.WriteLine("Warning: Audio file is not mono. The code assumes mono audio.");
        }

        // Get the number of samples and sample rate
        int sampleCount = (int)reader.SampleCount;
        int sampleRate = reader.WaveFormat.SampleRate;

        // Create a SkiaSharp bitmap for the waveform
        int width = 1200; // Width of the image in pixels
        int height = 300; // Height of the image in pixels
        using (SKBitmap bitmap = new SKBitmap(width, height))
        {
            // Create a SkiaSharp canvas for drawing
            using (var canvas = new SKCanvas(bitmap))
            {
                // Clear the canvas (set background color to white)
                canvas.Clear(SKColors.White);

                // Calculate the scaling factors
                int samplesPerPixel = Math.Max(sampleCount / width, 1);
                float amplitudeScaling = height / 0.5f;

                // Draw the waveform
                using (var paint = new SKPaint())
                {
                    paint.Color = SKColors.Black;

                    paint.StrokeWidth = 1; // Adjust the stroke width if necessary
                    SKPoint? prevPoint = null;

                    for (int x = 0; x < width; x++)
                    {
                        // Calculate the average sample for the current pixel
                        float sum = 0f;
                        for (int i = 0; i < samplesPerPixel; i++)
                        {
                            if (reader.Position < reader.Length)
                            {
                                // Read the next sample frame and use the first channel
                                float[] frame = reader.ReadNextSampleFrame();
                                sum += frame[0] * frame[0]; // Assuming mono audio or taking the left channel of stereo audio
                            }
                        }
                        float averageSample = sum / samplesPerPixel;

                        // Calculate the y-coordinate of the waveform
                        float y = (float)Math.Sqrt(averageSample) * amplitudeScaling;
                        y = Math.Abs(height - y);

                        // Debug output for the average sample and coordinates
                        Console.WriteLine($"x: {x}, y: {y}, averageSample: {averageSample}, height: {height / 2}");

                        // Draw a vertical line representing the sample
                        //canvas.DrawLine(x, y, x, height / 2, paint);
                        //Draw a line from the previous point to the current point
                        if (prevPoint.HasValue)
                        {
                            canvas.DrawLine(prevPoint.Value, new SKPoint(x, y), paint);
                        }

                        // Update the previous point to the current point
                        prevPoint = new SKPoint(x, y);
                    }
                }
            }
            return SKImage.FromBitmap(bitmap);
        }
    }

    private void SaveWaveformImageToFile(SKImage waveForImage)
    {
        // Encode the waveform bitmap as PNG
        using (var data = waveForImage.Encode(SKEncodedImageFormat.Png, 100))
        using (var fileStream = File.OpenWrite(imagePath))
        {
            data.SaveTo(fileStream);
        }
    }

    private void UpdateAudioImageSource()
    {
        AudioImageSource = ImageSource.FromFile(imagePath);
    }
}