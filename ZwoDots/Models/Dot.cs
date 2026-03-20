namespace ZwoDots.Models
{
    /// <summary>
    /// Repräsentiert einen einzelnen Punkt ("Dot") auf dem Spielfeld.
    /// Jeder Punkt hat eine Farbe, eine Position im Raster
    /// und kann markiert werden, wenn der Spieler ihn auswählt.
    /// </summary>
    public class Dot
    {
        // X-Position im Spielfeld (Spalte)
        public int X { get; set; }

        // Y-Position im Spielfeld (Zeile)
        public int Y { get; set; }

        // Farbe des Punktes
        public DotColor Color { get; set; }

        // Gibt an, ob der Punkt aktuell in einer Verbindung markiert ist
        public bool IsSelected { get; set; }

        /// <summary>
        /// Erstellt einen Punkt mit Position und Farbe.
        /// </summary>
        public Dot(int x, int y, DotColor color)
        {
            X = x;
            Y = y;
            Color = color;
            IsSelected = false;
        }

        /// <summary>
        /// Setzt den Punkt zurück, z. B. wenn nach einem Zug neue Punkte
        /// ins Spielfeld fallen.
        /// </summary>
        public void Reset(DotColor newColor)
        {
            Color = newColor;
            IsSelected = false;
        }
    }

    /// <summary>
    /// Definiert die möglichen Farben, die ein Punkt haben kann.
    /// </summary>
    public enum DotColor
    {
        Red,
        Blue,
        Yellow,
        Green,
        Purple
    }
}
