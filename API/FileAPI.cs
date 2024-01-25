using MyConfig;


namespace MyAPI;
public static class FileAPI
{

    public static void WriteData(string filename, byte[] data){
        var fullPath = Path.Combine(Config.rootPath, filename);
        File.WriteAllBytes(fullPath, data);
    }

    public static byte[] ReadData(string filename){
        var fullPath = Path.Combine(Config.rootPath, filename);
        if (File.Exists(fullPath)){
            return File.ReadAllBytes(fullPath);
        }
        return null;
    }

    public static void WriteCacheData(string filename, byte[] data){
        var fullPath = Path.Combine(Config.rootCachePath, filename);
        File.WriteAllBytes(fullPath, data);
    }

    public static byte[] ReadCacheData(string filename){
        var fullPath = Path.Combine(Config.rootCachePath, filename);
        if (File.Exists(fullPath)){
            return File.ReadAllBytes(fullPath);
        }
        return null;
    }

    public static void CreateDirectory(string path){
        Directory.CreateDirectory(path);
    }

    public static string [] GetFiles(string path){
        return Directory.GetFiles(path);
    }

    public static void DeleteFile(string path){
        File.Delete(path);
    }

}