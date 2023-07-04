namespace Dr.Logging.Abstractions;

public class LocalFileMessage
{
    public string FileName { get; set; } = "error";
    public string? Message { get; set; }
    public DateTime CreateTime { get; set; } = DateTime.UtcNow;
};
