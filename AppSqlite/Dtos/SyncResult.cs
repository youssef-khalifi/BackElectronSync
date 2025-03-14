namespace ProductsManage.Dtos;

public class SyncResult
{
    public int Added { get; set; }
    public int Updated { get; set; }
    public int Deleted { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}