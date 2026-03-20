namespace ZwoDots.Models
{
    /// <summary>
    /// Repräsentiert ein einzelnes Level im Spiel.
    /// Definiert Spielfeldgröße, Zielpunktzahl und maximale Züge.
    /// </summary>
    public class Level
    {
        // Anzeigename des Levels (z. B. "Level 1")
        public string Name { get; set; }

        // Zielpunkte, die der Spieler erreichen muss
        public int TargetScore { get; set; }

        // Maximale Anzahl an erlaubten Zügen
        public int MaxMoves { get; set; }

        // Spielfeldgröße für dieses Level
        public int BoardWidth { get; set; }
        public int BoardHeight { get; set; }

        // Flag, ob das Level abgeschlossen wurde
        public bool IsCompletedFlag { get; private set; }

        /// <summary>
        /// Erstellt ein neues Level mit den wesentlichen Parametern.
        /// </summary>
        public Level(string name, int targetScore, int maxMoves, int boardWidth, int boardHeight)
        {
            Name = name;
            TargetScore = targetScore;
            MaxMoves = maxMoves;
            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
            IsCompletedFlag = false;
        }

        /// <summary>
        /// Prüft, ob das Levelziel erreicht wurde.
        /// Wird vom GameManager aufgerufen.
        /// </summary>
        public bool IsCompleted(int currentScore)
        {
            if (currentScore >= TargetScore)
            {
                IsCompletedFlag = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Setzt den Fortschritt für einen Neustart zurück.
        /// </summary>
        public void Reset()
        {
            IsCompletedFlag = false;
        }
    }
}
