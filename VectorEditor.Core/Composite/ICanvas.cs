using VectorEditor.Core.Structures;
namespace VectorEditor.Core.Composite;

public interface ICanvas
{
    void ConsoleDisplay(int depth = 0);
    bool IsWithinBounds(Point startPoint, Point oppositePoint);

}