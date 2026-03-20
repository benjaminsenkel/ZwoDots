using System;

namespace ZwoDots.GameLogic
{
    /// <summary>
    /// Verwaltet die Punktevergabe und Berechnung des Spielstands.
    /// Der ScoreManager ist unabhängig vom Spielfeld
    /// und kann einfach im GameManager verwendet werden.
    /// </summary>
    public class ScoreManager
    {
        // Aktueller Punktestand
        public int CurrentScore { get; private set; }

        // Optionaler Multiplikator für besondere Züge (z. B. Kreise oder lange Ketten)
        public int Multiplier { get; private set; } = 1;

        /// <summary>
        /// Fügt eine bestimmte Punktezahl hinzu (z. B. pro Zug).
        /// </summary>
        public void AddPoints(int basePoints)
        {
            int total = basePoints * Multiplier;
            CurrentScore += total;
        }

        /// <summary>
        /// Setzt den Punktestand und den Multiplikator zurück.
        /// </summary>
        public void Reset()
        {
            CurrentScore = 0;
            Multiplier = 1;
        }

        /// <summary>
        /// Erhöhe den Multiplikator (z. B. bei Sonderkombo).
        /// </summary>
        public void IncreaseMultiplier(int step = 1)
        {
            Multiplier += step;
        }

        /// <summary>
        /// Setze den Multiplikator wieder auf 1 zurück.
        /// </summary>
        public void ResetMultiplier()
        {
            Multiplier = 1;
        }
    }
}
