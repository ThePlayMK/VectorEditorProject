using VectorEditor.Core.Composite;

namespace VectorEditor.Core.Structures;

public class CustomShape(List<Point> points, string contentColor, string contourColor, int width) : IShape
{
    private List<Point> _points = points;
    private string _contentColor = contentColor;
    private string _contourColor = contourColor;
    private int _width = width;
    public string Name => "Custom";

    public override string ToString()
    {
        var pointsStr = string.Join(",\n", _points.Select(p => p.ToString()));
        return
            $"Custom shape with points: [{pointsStr}], Content: {_contentColor}, Contour: {_contourColor}, Width: {_width}px";
    }

    public void ConsoleDisplay(int depth = 0)
    {
        Console.WriteLine(new string('-', depth) + Name + ": " + ToString());
    }

    public bool IsWithinBounds(Point startPoint, Point oppositePoint)
    {
        if (_points.Count < 2) return false;

        // 1. Sprawdź, czy jakikolwiek punkt kształtu jest wewnątrz zaznaczenia
        if (_points.Any(p => p.X >= startPoint.X && p.X <= oppositePoint.X && p.Y >= startPoint.Y && p.Y <= oppositePoint.Y))
            return true;

        // 2. Sprawdź przecięcia krawędzi (w tym domykającej ostatni z pierwszym)
        for (var i = 0; i < _points.Count; i++)
        {
            var pStart = _points[i];
            var pEnd = _points[(i + 1) % _points.Count]; // To zapewnia "niewidzialną krawędź" domykającą

            if (LineIntersectsRect(pStart, pEnd, startPoint, oppositePoint))
                return true;
        }

        // 3. Czy środek zaznaczenia jest wewnątrz kształtu? 
        // (Obsługa przypadku, gdy zaznaczenie jest całkowicie w środku dużego kształtu)
        var center = new Point((startPoint.X + oppositePoint.X) / 2, (startPoint.Y + oppositePoint.Y) / 2);

        return IsPointInPolygon(center, _points);

    }

    private static bool LineIntersectsRect(Point p1, Point p2, Point tl, Point br)
    {
        // Uproszczone sprawdzanie przecięcia linii z krawędziami prostokąta
        // (Można użyć algorytmu Lianga-Barsky'ego lub po prostu sprawdzić 4 krawędzie prostokąta)
        return LineIntersectsLine(p1, p2, tl, new Point(br.X, tl.Y)) || // Góra
               LineIntersectsLine(p1, p2, new Point(br.X, tl.Y), br) || // Prawo
               LineIntersectsLine(p1, p2, br, new Point(tl.X, br.Y)) || // Dół
               LineIntersectsLine(p1, p2, new Point(tl.X, br.Y), tl); // Lewo
    }

    private static bool LineIntersectsLine(Point a1, Point a2, Point b1, Point b2)
    {
        // Standardowy test orientacji punktów (CCW)
        var d = (a2.X - a1.X) * (b2.Y - b1.Y) - (a2.Y - a1.Y) * (b2.X - b1.X);
        if (d == 0) return false; // Równoległe

        var u = ((b1.X - a1.X) * (b2.Y - b1.Y) - (b1.Y - a1.Y) * (b2.X - b1.X)) / d;
        var v = ((b1.X - a1.X) * (a2.Y - a1.Y) - (b1.Y - a1.Y) * (a2.X - a1.X)) / d;

        return u >= 0 && u <= 1 && v >= 0 && v <= 1;
    }

    private static bool IsPointInPolygon(Point p, List<Point> poly)
    {
        var inside = false;
        for (int i = 0, j = poly.Count - 1; i < poly.Count; j = i++)
        {
            if (((poly[i].Y > p.Y) != (poly[j].Y > p.Y)) &&
                (p.X < (poly[j].X - poly[i].X) * (p.Y - poly[i].Y) / (poly[j].Y - poly[i].Y) + poly[i].X))
            {
                inside = !inside;
            }
        }
        return inside;
    }
}