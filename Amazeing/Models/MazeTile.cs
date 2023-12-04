namespace Amazeing.Models;
internal class MazeTile
{
    public Position Position { get; set; }
    public bool IsVisited { get; set; }
    public long? Tag { get; set; }
    public int Reward { get; set; }
    public bool ExitPoint { get; set; }
    public bool CollectionPoint { get; set; }
}
