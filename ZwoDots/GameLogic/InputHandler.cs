using System.Collections.Generic;
using ZwoDots.Models;

namespace ZwoDots.GameLogic
{
    /// <summary>
    /// Verarbeitet Benutzereingaben wie Klicks, Drags und Releases.
    /// Diese Klasse sammelt die aktuell verbundenen Punkte
    /// und übergibt sie nach Loslassen eines Zuges an den GameManager.
    /// </summary>
    public class InputHandler
    {
        // Referenz auf das Spielfeld
        private GameBoard _board;

        // Referenz auf den GameManager (um Züge zu melden)
        private GameManager _gameManager;

        // Liste der aktuell ausgewählten Punkte
        private List<Dot> _selectedDots = new();

        // Farbe der gerade verbundenen Punkte
        private DotColor? _currentColor;

        /// <summary>
        /// Konstruktor: benötigt Zugriff auf Board und Manager.
        /// </summary>
        public InputHandler(GameBoard board, GameManager manager)
        {
            _board = board;
            _gameManager = manager;
        }

        /// <summary>
        /// Wird aufgerufen, wenn der Spieler auf einen Punkt tippt oder klickt.
        /// </summary>
        public void OnDotPressed(int x, int y)
        {
            var dot = _board.GetDot(x, y);
            if (dot == null)
                return;

            _selectedDots.Clear();
            _currentColor = dot.Color;
            dot.IsSelected = true;
            _selectedDots.Add(dot);
        }

        /// <summary>
        /// Wird aufgerufen, wenn der Spieler den Finger oder die Maus über weitere Punkte zieht.
        /// </summary>
        public void OnDotDragged(int x, int y)
        {
            var dot = _board.GetDot(x, y);
            if (dot == null || _currentColor == null)
                return;

            // Prüfen: gleiche Farbe & Nachbarpunkt?
            var last = _selectedDots[^1];
            if (dot.Color == _currentColor && IsNeighbor(dot, last) && !_selectedDots.Contains(dot))
            {
                dot.IsSelected = true;
                _selectedDots.Add(dot);
            }
        }

        /// <summary>
        /// Wird aufgerufen, wenn der Spieler loslässt (Finger hebt).
        /// Dadurch wird der Zug abgeschlossen und an den GameManager gesendet.
        /// </summary>
        public void OnDotReleased()
        {
            if (_selectedDots.Count >= 2)
            {
                _gameManager.MakeMove(new List<Dot>(_selectedDots));
            }

            // Reset für nächsten Zug
            foreach (var dot in _selectedDots)
                dot.IsSelected = false;

            _selectedDots.Clear();
            _currentColor = null;
        }

        /// <summary>
        /// Prüft, ob zwei Punkte direkt benachbart sind (horizontal oder vertikal).
        /// </summary>
        private bool IsNeighbor(Dot a, Dot b)
        {
            int dx = System.Math.Abs(a.X - b.X);
            int dy = System.Math.Abs(a.Y - b.Y);
            return (dx + dy == 1); // Nur direkte Nachbarn erlaubt
        }
    }
}
