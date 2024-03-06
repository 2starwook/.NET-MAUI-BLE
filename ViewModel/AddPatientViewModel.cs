using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Object.MyData;
using NET_MAUI_BLE.Pages;

namespace NET_MAUI_BLE.ViewModel;

public partial class AddPatientViewModel : ObservableObject
{
	public AddPatientViewModel(DatabaseManager databaseManager)
	{
        this._databaseManager = databaseManager;
    }

    private DatabaseManager _databaseManager;
    [ObservableProperty]
	private string patientFirstName;
    [ObservableProperty]
	private string patientLastName;
    [ObservableProperty]
    private DateTime dateOfBirth = DateTime.Now;

    [RelayCommand]
    void Appearing()
    {
        try
        {
            MYAPI.UIAPI.HideTab();
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine($"{e.ToString()}");
        }
    }

    [RelayCommand]
	async Task Submit()
	{
        await _databaseManager.AddPatientAsync(PatientFirstName, PatientLastName);
        await Shell.Current.GoToAsync($"{nameof(AddRecordPage)}");
	}
}
