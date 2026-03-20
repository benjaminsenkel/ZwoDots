using System;
using Microsoft.Maui.Graphics;
using ZwoDots.GameLogic;
using ZwoDots.Models;

namespace ZwoDots.UI
{
    /// <summary>
    /// Verantwortlich für das Rendern des Spielfelds und Anzeigen von Punktestand & Status.
    /// Diese Klasse koppelt Spiellogik (GameManager/GameBoard) an die MAUI-Grafik.
    /// </summary>
    public class UIController : IDrawable
    {
        private readonly GameManager _gameManager;
        private readonly float _cellSize = 50f; // Größe eines Dots in Pixeln
        private readonly float _padding = 10f;  // Abstand am Rand

        public Action? OnRedrawRequired { get; set; }

        public UIController(GameManager gameManager)
        {
            _gameManager = gameManager;

            // Events abonnieren, um Sieg oder Niederlage anzuzeigen
            _gameManager.OnGameWon += (s, e) => ShowEndScreen(true);
            _gameManager.OnGameOver += (s, e) => ShowEndScreen(false);
        }

        /// <summary>
        /// Wird automatisch von MAUI aufgerufen, wenn das Spielfeld neu gezeichnet werden muss.
        /// </summary>
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            DrawBackground(canvas, dirtyRect);
            DrawDots(canvas);
            DrawHUD(canvas);
        }

        /// <summary>
        /// Zeichnet den Hintergrundbereich.
        /// </summary>
        private void DrawBackground(ICanvas canvas, RectF area)
        {
            canvas.FillColor = Colors.LightGray;
            canvas.FillRectangle(area);
        }

        /// <summary>
        /// Zeichnet alle Punkte des Spielfelds.
        /// </summary>
        private void DrawDots(ICanvas canvas)
        {
            var board = _gameManager.Board;

            for (int x = 0; x < board.Width; x++)
            {
                for (int y = 0; y < board.Height; y++)
                {
                    var dot = board.Dots[x, y];
                    if (dot == null) continue;

                    // Setze die Farbe basierend auf DotColor
                    canvas.FillColor = dot.Color switch
                    {
                        DotColor.Red => Colors.IndianRed,
                        DotColor.Blue => Colors.SteelBlue,
                        DotColor.Green => Colors.MediumSeaGreen,
                        DotColor.Yellow => Colors.Goldenrod,
                        DotColor.Purple => Colors.MediumPurple,
                        _ => Colors.Gray
                    };

                    // Berechne Position
                    float px = _padding + x * _cellSize;
                    float py = _padding + y * _cellSize;
                    float radius = _cellSize * 0.4f;

                    // Optional visueller Effekt: ausgewählt → größer zeichnen
                    if (dot.IsSelected)
                    {
                        radius *= 1.2f;
                        canvas.FillColor = Colors.White.WithAlpha(0.8f);
                        canvas.FillCircle(px + _cellSize / 2, py + _cellSize / 2, radius + 3);
                        canvas.FillColor = dot.Color switch
                        {
                            DotColor.Red => Colors.IndianRed,
                            DotColor.Blue => Colors.SteelBlue,
                            DotColor.Green => Colors.MediumSeaGreen,
                            DotColor.Yellow => Colors.Goldenrod,
                            DotColor.Purple => Colors.MediumPurple,
                            _ => Colors.Gray
                        };
                    }

                    canvas.FillCircle(px + _cellSize / 2, py + _cellSize / 2, radius);
                }
            }
        }

        /// <summary>
        /// Zeichnet Punktestand und verbleibende Züge über dem Spielfeld.
        /// </summary>
        private void DrawHUD(ICanvas canvas)
        {
            canvas.FontColor = Colors.Black;
            canvas.FontSize = 18;

            canvas.DrawString(
                $"Score: {_gameManager.Score}",
                10, 10, HorizontalAlignment.Left
            );

            canvas.DrawString(
                $"Moves left: {_gameManager.RemainingMoves}",
                10, 30, HorizontalAlignment.Left
            );
        }

        /// <summary>
        /// Wird bei Spielende aufgerufen (Sieg oder Niederlage).
        /// Zeigt rudimentäre finale Anzeige (wird über GUI später verbessert).
        /// </summary>
        private void ShowEndScreen(bool won)
        {
            string message = won ? "🎉 Level geschafft!" : "😔 Keine Züge mehr!";
            Console.WriteLine(message);
        }

        /// <summary>
        /// Erzwingt ein Neuzeichnen der Oberfläche (z. B. wenn sich der Zustand geändert hat).
        /// </summary>
        public void RequestRedraw()
        {
            OnRedrawRequired?.Invoke();
        }
    }
}
