using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.Audio;

using Object.MyAudio;
using MyAPI;


namespace NET_MAUI_BLE.ViewModel;

[QueryProperty("Text", "Text")]
public partial class RecordViewModel : ObservableObject
{
	[ObservableProperty]
	private string playText;

    public RecordViewModel(IAudioManager audioManager)
    {
        audioController = new AudioController(audioManager);
		playText = "Play";
    }

    private AudioController audioController;
	[ObservableProperty]
	string text;

    [RelayCommand]
	void Play(string path)
    {
		audioController.OpenFile(path);
		audioController.Play();
		audioController.AddEventHandler(new EventHandler(HandlePlayEnded));
		PlayText = "Stop";
		// TODO - Add Stop / Pause Button

	}

	void HandlePlayEnded(object sender, EventArgs e){
		PlayText = "Play";
	}



    [RelayCommand]
	async Task Share(string path)
    {
		await StorageAPI.ExportData("file.wav", FileAPI.ReadData(path));
	}
}