using VectorEditor.Core.Structures;

namespace VectorEditor.Core.Composite;

public class Layer(string name) : ICanvas
{
    private string Name { get; set; } = name;
    private readonly List<ICanvas> _children = [];
    
    public IEnumerable<ICanvas> GetChildren() => _children;
    
    public void Add(ICanvas canvas)
    {
        _children.Add(canvas);
    }
    
    public void Remove(ICanvas canvas)
    {
        _children.Remove(canvas);
    }
    
    public bool IsWithinBounds(Point startPoint, Point oppositePoint)
    {
        return _children.Count != 0 && 
               _children.Any(child => child.IsWithinBounds(startPoint, oppositePoint));
    }


    public void ConsoleDisplay(int depth = 0)
    {
        Console.WriteLine(new string('-', depth) + Name);
        foreach (var child in _children)
        {
            child.ConsoleDisplay(depth + 2);
        }
    }
}