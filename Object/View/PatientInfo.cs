namespace NET_MAUI_BLE.Object.View;

public class PatientInfo
{
    public string name { get; set; }
    public string id { get; set; }
    public PatientInfo(string name, string id)
    {
        this.name = name;
        this.id = id;
    }
}