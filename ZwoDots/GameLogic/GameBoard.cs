using System;
using System.Collections.Generic;
using ZwoDots.Models;

namespace ZwoDots.GameLogic
{
    /// <summary>
    /// Repräsentiert das gesamte Spielfeld als zweidimensionales Raster aus Dots.
    /// Diese Klasse ist vom UI unabhängig und kümmert sich ausschließlich
    /// um Spiellogik wie Erzeugen, Entfernen, Nachrutschen und Auffüllen.
    /// </summary>
    public class GameBoard
    {
        // Spielfeldgröße in Spalten (Breite) und Zeilen (Höhe)
        public int Width { get; private set; }
        public int Height { get; private set; }

        // 2D-Array für alle Punkte auf dem Spielfeld
        private Dot[,] dots;

        // Zufallsgenerator für neue Farben
        private Random random = new();

        /// <summary>
        /// Zugriff auf alle Dots des Spielfelds.
        /// </summary>
        public Dot[,] Dots => dots;

        /// <summary>
        /// Erstellt ein neues Spielfeld mit gegebener Größe.
        /// </summary>
        public GameBoard(int width, int height)
        {
            Width = width;
            Height = height;
            dots = new Dot[width, height];
            Initialize();
        }

        /// <summary>
        /// Initialisiert das Spielfeld mit zufällig gefärbten Punkten.
        /// </summary>
        public void Initialize()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var color = GetRandomColor();
                    dots[x, y] = new Dot(x, y, color);
                }
            }
        }

        /// <summary>
        /// Gibt den Punkt an einer bestimmten Position zurück.
        /// </summary>
        public Dot GetDot(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
                throw new ArgumentOutOfRangeException();

            return dots[x, y];
        }

        /// <summary>
        /// Entfernt eine Liste von Punkten (z. B. verbundene Dots gleicher Farbe)
        /// und markiert deren Positionen als leer (null).
        /// </summary>
        public void RemoveDots(List<Dot> toRemove)
        {
            foreach (var dot in toRemove)
            {
                dots[dot.X, dot.Y] = null;
            }
        }

        /// <summary>
        /// Lässt Punkte nach unten „fallen“, um Lücken zu füllen.
        /// (Standard‑Mechanik bei Puzzle‑Spielen)
        /// </summary>
        public void CollapseDots()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = Height - 1; y >= 0; y--)
                {
                    // Wenn Position leer ist -> nachrücken
                    if (dots[x, y] == null)
                    {
                        for (int aboveY = y - 1; aboveY >= 0; aboveY--)
                        {
                            if (dots[x, aboveY] != null)
                            {
                                dots[x, y] = dots[x, aboveY];
                                dots[x, aboveY] = null;
                                dots[x, y].Y = y; // Neue Position speichern
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Füllt leere Stellen oben im Spielfeld mit neuen, zufälligen Punkten.
        /// </summary>
        public void RefillBoard()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (dots[x, y] == null)
                    {
                        dots[x, y] = new Dot(x, y, GetRandomColor());
                    }
                }
            }
        }

        /// <summary>
        /// Hilfsmethode: Gibt eine zufällige Dot‑Farbe zurück.
        /// </summary>
        private DotColor GetRandomColor()
        {
            Array colors = Enum.GetValues(typeof(DotColor));
            return (DotColor)colors.GetValue(random.Next(colors.Length))!;
        }
    }
}
