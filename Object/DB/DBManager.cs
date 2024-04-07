using NET_MAUI_BLE.API;
using NET_MAUI_BLE.AppConfig;


namespace NET_MAUI_BLE.Object.DB;

public class DBManager
{
    // Manage Database: Add/Remove/Modify data
	public DBManager()
    {
        this._patientsManager = new PatientsManager<Patient>();
        this._recordsManager = new RecordsManager<Record>();
        this.currentRecords = new Dictionary<string, Record>();
	}

    private PatientsManager<Patient> _patientsManager;
    private RecordsManager<Record> _recordsManager;

    public Dictionary<string, Record> currentRecords;
    public Dictionary<string, Patient> currentPatients;

    public async Task LoadRecordDataAsync()
    {
        this.currentRecords = new Dictionary<string, Record>();
        foreach (Record r in await _recordsManager.GetRecordsAsync())
        {
            this.currentRecords.Add(r.GetId(), r);
        }
    }

    public async Task LoadPatientDataAsync()
    {
        this.currentPatients = new Dictionary<string, Patient>();
        foreach (Patient p in await _patientsManager.GetPatientsAsync())
        {
            this.currentPatients.Add(p.GetId(), p);
        }
    }

    public async Task<string> AddRecordAsync(string audioFilePath,
                                             string recordName)
    {
        var record = new Record(recordName, File.ReadAllBytes(audioFilePath), null);
        await _recordsManager.InsertRecordAsync(record);
        currentRecords.Add(record.GetId(), record);
        return record.GetId();
    }

    public async Task DeleteRecordAsync(string id)
    {
        await _recordsManager.DeleteRecordAsync(id);
        currentRecords.Remove(id);
    }

    public async Task<string> AddPatientAsync(string firstName, string lastName)
    {
        var patient = new Patient(firstName, lastName);
        await _patientsManager.InsertPatientAsync(patient);
        currentPatients.Add(patient.GetId(), patient);
        return patient.GetId();
    }

    public async Task DeletePatientAsync(string id)
    {
        await _patientsManager.DeletePatientAsync(id);
        currentPatients.Remove(id);
    }

    public void AttachPatientToRecord(string audioFilePath, string patientId)
    {
        // TODO - Implement Later
    }

    public void DetachAssignedPatientFromRecord(string audioFilePath)
    {
        // TODO - Implement Later
    }

    public async Task<string> ImportAudioFile()
    {
        var srcPath = await StorageAPI.GetFilePath();
        var filename = FileAPI.GetUniqueID() + ".wav";
        var dstPath = Path.Combine(Config.dataDirPath, filename);
        File.Copy(srcPath.ToString(), dstPath);
        return dstPath;
    }
}