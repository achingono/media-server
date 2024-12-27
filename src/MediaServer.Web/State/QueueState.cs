
using MediaServer.Web.Models;

namespace MediaServer.Web.State;

public enum RepeatMode
{
    None,
    Single,
    All
}

public class QueueState
{
    public bool AutoPlay { get; set; }
    public bool Shuffle { get; set; }
    public RepeatMode RepeatMode { get; set; }
    public int Position { get; set; }
    public Entities.Track? Track => _sequentialItems.FirstOrDefault(x => x.Position == Position)?.Item;
    public bool HasNext {
        get {
            switch (RepeatMode)
            {
                case RepeatMode.All:
                    return true;
                default:
                    return Items.Count() > 0 && (Position + 1) % Items.Count() != 0;
            }
        }
    }
    public bool HasPrevious {
        get {
            switch (RepeatMode)
            {
                case RepeatMode.All:
                    return true;
                default:
                    var count = Items.Count();
                    return count > 0 && (Position - 1 + count) % count != (count - 1);
            }
        }
    }
    private readonly Random _random = new();
    private IEnumerable<QueueModel<Entities.Track>> _sequentialItems = [];
    private IEnumerable<QueueModel<Entities.Track>> _shuffledItems = [];
    public IEnumerable<QueueModel<Entities.Track>> Items
    {
        get
        {
            if (Shuffle)
                return _shuffledItems ?? [];
            return _sequentialItems ?? [];
        }
        set
        {
            _shuffledItems = (_sequentialItems == value) ? _shuffledItems : value.OrderBy(x => _random.Next());
            _sequentialItems = (_sequentialItems == value) ? _sequentialItems : value;
        }
    }

    public void Next()
    {
        if (RepeatMode == RepeatMode.Single ||
            Items.Count() <= 0) return;
        
        var currentIndex = Items.Select((x, i) => new { Item = x, Index = i }).FirstOrDefault(x => x.Item.Position == Position)!.Index;

        if (currentIndex == Items.Count() - 1 && RepeatMode == RepeatMode.None) return;

        var nextIndex = (currentIndex + 1) % Items.Count();

        Position = Items.ElementAt(nextIndex).Position;
    }

    public void Previous()
    {
        var count = Items.Count();
        if (RepeatMode == RepeatMode.Single ||
            count <= 0) return;
        
        var currentIndex = Items.Select((x, i) => new { Item = x, Index = i }).FirstOrDefault(x => x.Item.Position == Position)!.Index;

        if (currentIndex == 0 && RepeatMode == RepeatMode.None) return;

        var nextIndex = (currentIndex - 1 + count) % count;

        Position = Items.ElementAt(nextIndex).Position;
    }
}