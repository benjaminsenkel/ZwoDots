using Microsoft.Maui.Controls;
using ZwoDots.Models;
using ZwoDots.UI;
using ZwoDots.GameLogic;

namespace ZwoDots
{
    public partial class MainPage : ContentPage
    {
        private UIController _uiController;
        private GameManager _gameManager;
        private InputHandler _inputHandler;

        // Variablen, um Drag-Bewegungen zu erkennen
        private bool _isDragging = false;

        public MainPage()
        {
            InitializeComponent();
            InitializeGame();
        }

        /// <summary>
        /// Erstellt Spielobjekte und verbindet sie mit der Oberfläche.
        /// </summary>
        private void InitializeGame()
        {
            // Level anlegen
            var level = new Level(
                name: "Level 1",
                targetScore: 500,
                maxMoves: 20,
                boardWidth: 6,
                boardHeight: 6
            );

            // GameManager, InputHandler, UIController initialisieren
            _gameManager = new GameManager(level);
            _uiController = new UIController(_gameManager);
            _inputHandler = new InputHandler(_gameManager.Board, _gameManager);

            // GraphicsView verbinden
            gameBoardView.Drawable = _uiController;
            _uiController.OnRedrawRequired += () => gameBoardView.Invalidate();

            // Optional Statusanzeige
            statusLabel.Text = $"Ziel: {level.TargetScore} Punkte in {level.MaxMoves} Zügen";

            // Pointer-Ereignisse für Touch/Maus verbinden
            gameBoardView.StartInteraction += GameBoardView_OnPressed;
            gameBoardView.DragInteraction += GameBoardView_OnDragged;
            gameBoardView.EndInteraction += GameBoardView_OnReleased;
        }

        /// <summary>
        /// Wird ausgelöst, wenn der Spieler auf das Spielfeld klickt oder tippt.
        /// </summary>
        private void GameBoardView_OnPressed(object sender, TouchEventArgs e)
        {
            var (x, y) = GetGridPosition(e.Touches[0]);
            _inputHandler.OnDotPressed(x, y);
            _isDragging = true;
            _uiController.RequestRedraw();
        }

        /// <summary>
        /// Wird aufgerufen, wenn über mehrere Punkte gezogen wird.
        /// </summary>
        private void GameBoardView_OnDragged(object sender, TouchEventArgs e)
        {
            if (!_isDragging) return;

            var (x, y) = GetGridPosition(e.Touches[0]);
            _inputHandler.OnDotDragged(x, y);
            _uiController.RequestRedraw();
        }

        /// <summary>
        /// Wird aufgerufen, wenn Spieler den Finger/Mouse loslässt.
        /// </summary>
        private void GameBoardView_OnReleased(object sender, TouchEventArgs e)
        {
            if (!_isDragging) return;

            _inputHandler.OnDotReleased();
            _isDragging = false;

            statusLabel.Text = $"Punkte: {_gameManager.Score} | Züge: {_gameManager.RemainingMoves}";
            _uiController.RequestRedraw();
        }

        /// <summary>
        /// Hilfsmethode: Wandelt die Touch-Koordinaten des Geräts in
        /// Spielfeld-Koordinaten (Rasterposition) um.
        /// </summary>
        private (int gridX, int gridY) GetGridPosition(PointF touchPoint)
        {
            float cellSize = GetCellSize();
            int gridX = (int)((touchPoint.X - 10) / cellSize);
            int gridY = (int)((touchPoint.Y - 10) / cellSize);

            // Begrenzung sicherstellen
            gridX = Math.Clamp(gridX, 0, _gameManager.Board.Width - 1);
            gridY = Math.Clamp(gridY, 0, _gameManager.Board.Height - 1);

            return (gridX, gridY);
        }

        /// <summary>
        /// Ermittelt die Zellgröße (muss mit UIController-Wert übereinstimmen!)
        /// </summary>
        private float GetCellSize() => 50f;
    }
}
