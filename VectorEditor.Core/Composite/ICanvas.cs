using VectorEditor.Core.Structures;
namespace VectorEditor.Core.Composite;

public interface ICanvas
{
    Layer? ParentLayer { get; set; }
    bool IsLocked { get; set; } 
    void ConsoleDisplay(int depth = 0);
    bool IsWithinBounds(Point startPoint, Point oppositePoint);

}