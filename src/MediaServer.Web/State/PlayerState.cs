
namespace MediaServer.Web.State;

public class PlayerState
{
    public bool CanPlay { get; set; }
    public int CurrentTime { get; set; }
    public int Duration { get; set; }
    public bool Playing { get; set; }
    public string Source { get; set; } = string.Empty;
}