using VectorEditor.Core.Composite;
using VectorEditor.Core.Strategy;

namespace VectorEditor.Core.Structures;

public class Rectangle : IShape
{
    // Pola prywatne - brak bezpośredniego dostępu z zewnątrz
    private string _contentColor;
    private string _contourColor;
    private int _width;
    private Point _startPoint;
    private Point _oppositePoint;
    private Point? _helperPoint1;
    private Point? _helperPoint2;

    public Layer? ParentLayer { get; set; }
    public bool IsBlocked { get; set; }
    public bool IsVisible { get; set; } = true;
    public string Name => "Rectangle";

    public Rectangle(Point startPoint, Point oppositePoint, string contentColor, string contourColor, int width)
    {
        _startPoint = startPoint;
        _oppositePoint = oppositePoint;
        _contentColor = contentColor;
        _contourColor = contourColor;
        _width = width;
        UpdateHelperPoints();
    }
    
    // --- GETTERY (Publiczne) ---
    public string GetContentColor() => _contentColor;
    public string GetContourColor() => _contourColor;
    public int GetWidth() => _width;
    public Point GetStartPoint() => _startPoint;
    public Point GetOppositePoint() => _oppositePoint;
    
    public void SetContentColor(string color)
    {
        if (IsBlocked) return;
        _contentColor = color;
    }

    public void SetContourColor(string color)
    {
        if (IsBlocked) return;
        _contourColor = color;
    }

    public void SetWidth(int width)
    {
        if (IsBlocked) return;
        _width = width;
    }
    
    // --- GEOMETRIA (Również pilnuje blokady) ---
    
    public void Move(int dx, int dy)
        {
            if (IsBlocked) return;
            _startPoint = new Point(_startPoint.X + dx, _startPoint.Y + dy);
            _oppositePoint = new Point(_oppositePoint.X + dx, _oppositePoint.Y + dy);
            // Aktualizujemy punkty pomocnicze, jeśli są używane w renderowaniu
            _helperPoint1 = new Point(_startPoint.X, _oppositePoint.Y);
            _helperPoint2 = new Point(_oppositePoint.X, _startPoint.Y);
        }

    public void Scale(ScaleHandle handle, Point newPos)
    {
        throw new NotImplementedException();
    }
    
    private void UpdateHelperPoints()
    {
        _helperPoint1 = new Point(_startPoint.X, _oppositePoint.Y);
        _helperPoint2 = new Point(_oppositePoint.X, _startPoint.Y);
    }

    public bool IsWithinBounds(Point startPoint, Point oppositePoint)
        {
            // 1. Wyznaczamy krawędzie prostokąta obiektu
            var rectLeft = Math.Min(_startPoint.X, _oppositePoint.X);
            var rectRight = Math.Max(_startPoint.X, _oppositePoint.X);
            var rectTop = Math.Min(_startPoint.Y, _oppositePoint.Y);
            var rectBottom = Math.Max(_startPoint.Y, _oppositePoint.Y);
    
            // 2. Wyznaczamy krawędzie prostokąta zaznaczenia
            var selLeft = Math.Min(startPoint.X, oppositePoint.X);
            var selRight = Math.Max(startPoint.X, oppositePoint.X);
            var selTop = Math.Min(startPoint.Y, oppositePoint.Y);
            var selBottom = Math.Max(startPoint.Y, oppositePoint.Y);
    
            // 3. Test nachodzenia (AABB)
            var overlapX = rectLeft <= selRight && 
                           rectRight >= selLeft;
            var overlapY = rectTop <= selBottom && 
                           rectBottom >= selTop;
    
            return overlapX && overlapY;
        }
    
    public override string ToString() => 
        $"Rectangle from ({_startPoint}), ({_helperPoint1}), ({_oppositePoint}), ({_helperPoint2}) Color: {_contentColor} and {_contourColor}";
    
    public void ConsoleDisplay(int depth = 0)
    {
        if (!IsVisible) return;
        Console.WriteLine(new string('-', depth) + Name + ": " + ToString());
    }

    
}