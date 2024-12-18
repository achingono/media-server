
namespace MediaServer.Web.State;

public class QueueState
{
    public bool AutoPlay { get; set; }
    public int CurrentIndex { get; set; }
    public Entities.Track CurrentTrack { get => Items.ElementAtOrDefault(CurrentIndex); }
    public IEnumerable<Entities.Track> Items { get; set; } = Enumerable.Empty<Entities.Track>();
}