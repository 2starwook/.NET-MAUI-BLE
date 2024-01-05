using Object.MyAudio;
using Object.MyFileController;
using MyConfig;


namespace Object.MyDatabase;
public class DatabaseManager {
    // Manager Database: Add/Remove/Modify data
    private AudioFileCollection audioFileCollection;
    
	public DatabaseManager() {
        if(!File.Exists(Config.dataDirPath)){
            FileController.CreateDirectory(Config.dataDirPath);
        }
		var files = FileController.GetFiles(Config.dataDirPath);
        this.audioFileCollection = new AudioFileCollection(files);
	}

    public List<string> GetPathList(){
        return audioFileCollection.GetPathList();
    }

}
