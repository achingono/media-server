
namespace MediaServer.Web.State;

public class QueueState
{
    public bool AutoPlay { get; set; }
    public int CurrentIndex { get; set; }
    public Entities.Track CurrentTrack => Items.ElementAtOrDefault(CurrentIndex)!;
    public bool HasNext => CurrentIndex < Items.Count() - 1;
    public bool HasPrevious => CurrentIndex > 0;
    public IEnumerable<Entities.Track> Items { get; set; } = Enumerable.Empty<Entities.Track>();
}