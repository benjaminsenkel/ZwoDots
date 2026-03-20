using System;
using System.Collections.Generic;
using ZwoDots.Models;

namespace ZwoDots.GameLogic
{
    /// <summary>
    /// Steuert das gesamte Spielgeschehen.
    /// Verwaltet das Spielfeld, Levelziele, Punktestand und Restzüge.
    /// </summary>
    public class GameManager
    {
        // Das Spielfeld
        public GameBoard Board { get; private set; }

        // Der aktuelle Punktestand
        public int Score { get; private set; }

        // Anzahl verbleibender Spielzüge
        public int RemainingMoves { get; private set; }

        // Aktuelles Level (Zielbedingungen)
        public Level CurrentLevel { get; private set; }

        // Gibt an, ob das Spiel aktuell läuft
        public bool IsActive { get; private set; }

        /// <summary>
        /// Erstellt einen neuen GameManager mit Board und Level.
        /// </summary>
        public GameManager(Level level)
        {
            CurrentLevel = level;
            RemainingMoves = level.MaxMoves;
            Board = new GameBoard(level.BoardWidth, level.BoardHeight);
            Score = 0;
            IsActive = true;
        }

        /// <summary>
        /// Behandelt den Spielerzug: Entfernt Punkte, lässt sie nachrücken und füllt nach.
        /// </summary>
        public void MakeMove(List<Dot> selectedDots)
        {
            if (!IsActive)
                return;

            if (selectedDots == null || selectedDots.Count < 2)
                return; // Kein gültiger Zug

            // Punkte entfernen
            Board.RemoveDots(selectedDots);

            // Punkte berechnen (z. B. 10 Punkte pro Dot)
            int points = selectedDots.Count * 10;
            Score += points;

            // Nachrücken + Auffüllen
            Board.CollapseDots();
            Board.RefillBoard();

            // Einen Zug abziehen
            RemainingMoves--;

            // Sieg/Niederlage prüfen
            CheckGameState();
        }

        /// <summary>
        /// Prüft ob das Level gewonnen oder verloren wurde.
        /// </summary>
        private void CheckGameState()
        {
            if (CurrentLevel.IsCompleted(Score))
            {
                IsActive = false;
                OnGameWon?.Invoke(this, EventArgs.Empty);
            }
            else if (RemainingMoves <= 0)
            {
                IsActive = false;
                OnGameOver?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Setzt das Spiel (und Spielfeld) zurück.
        /// </summary>
        public void Reset()
        {
            Score = 0;
            RemainingMoves = CurrentLevel.MaxMoves;
            Board.Initialize();
            IsActive = true;
        }

        // Ereignisse für UI oder später SoundManager etc.
        public event EventHandler? OnGameWon;
        public event EventHandler? OnGameOver;
    }
}
