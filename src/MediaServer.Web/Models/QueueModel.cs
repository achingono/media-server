namespace MediaServer.Web.Models;

public class QueueModel<T>
{
    public int Position { get; set; }
    public T Item { get; set; } = default!;
    public bool IsSelected { get; set; }
}