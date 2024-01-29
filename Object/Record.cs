namespace Object.MyDB;
public class Record : Item
{
    public string recordName {get; set;}
    public byte[] binaryData {get; set;}
    public string assignedPatientId {get; set;}

    private readonly string audioFilePath;

	public Record(string recordName, byte[] binaryData, string assignedPatientId) {
        this.recordName = recordName;
        this.binaryData = binaryData;
        this.assignedPatientId = assignedPatientId;
	}

    public string GetAudioFile() {
        return this.audioFilePath;
    }

    public bool IsPatientAssigned(){
        return this.assignedPatientId != null;
    }

    public void AssignPatient(string patientId){
        this.assignedPatientId = patientId;
    }

    public void DeleteAssignedPatient(){
        this.assignedPatientId = null;
    }
}
