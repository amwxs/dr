namespace Dr.Logging.Abstractions;
internal class LocalFileWriter : ILocalFileWriter
{
    private const string _suffix = ".log";
    public void Write(LocalFileMessage localLog)
    {
        try
        {
            if (!Directory.Exists(localLog.LocalPath))
            {
                Directory.CreateDirectory(localLog.LocalPath);
            }

            var witerPath = Path.Combine(localLog.LocalPath, localLog.FileName, DateTime.UtcNow.ToString("yyyy-MM-dd"), _suffix);
            using var writer = new StreamWriter(witerPath, true);
            writer.WriteAsync($"datetime: {DateTime.UtcNow} Message: {localLog.Message}");
        }
        catch (Exception)
        {
            throw;
        }

    }
}
