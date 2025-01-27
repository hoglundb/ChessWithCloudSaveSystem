using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interactions
{

    public class ChessboardInteractor : MonoBehaviour
    {
        public event Action PiecePickedUp;
        public event Action PiecePlaced;

        [SerializeField] private Chessboard chessboard;
        [SerializeField] private LegalMovesCalculator legalMovesCalculator;
        [SerializeField] private InputActionReference selectPieceInput;
        private ChessPiece currentPiece;
        private bool isMovingPiece;
        private ChessPiece lastHoveredPiece; // Track last hovered piece


        private void Awake()
        {
            selectPieceInput.action.Enable();
            selectPieceInput.action.performed += _ => SelectOrPlacePiece(Input.mousePosition);
        }

        private void Update()
        {
            if (isMovingPiece && currentPiece != null)
            {
                MovePieceWithCursor();
            }

            // Track piece under cursor and handle hover events
            TrackHoverOverPiece();
        }

        public void SelectOrPlacePiece(Vector3 pointerPosition)
        {
            if (!currentPiece)
            {
                currentPiece = TrySelectPiece(pointerPosition);

                if (currentPiece != null)
                {
                    isMovingPiece = true;
                    chessboard.OnPiecePickedUp();
                    PiecePickedUp?.Invoke();

                    EnableLegalMoves(currentPiece);  // Show legal moves
                }
            }
            else
            {
                var targetSquare = TryGetSquare(pointerPosition);
                if (targetSquare != null)
                {
                    DisableAllSquares();
                    chessboard.OnPiecePlaced();
                    PlacePiece(targetSquare);
                }
            }
        }

        private void TrackHoverOverPiece()
        {
            var hoveredPiece = TryGetPieceUnderCursor();

            if (hoveredPiece != lastHoveredPiece)
            {
                // Exit hover for the previous piece
                if (lastHoveredPiece != null)
                {
                    lastHoveredPiece.OnHoverExited();
                }

                // Enter hover for the new piece
                if (hoveredPiece != null)
                {
                    hoveredPiece.OnHoverEntered();
                }

                // Update last hovered piece
                lastHoveredPiece = hoveredPiece;
            }
        }

        private void MovePieceWithCursor()
        {
            // Move the piece with the cursor position while it is being dragged
            var mousePosition = Mouse.current.position.ReadValue();
            var worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));  // Assuming 10f is the correct Z-depth
            var newPos = new Vector3(worldPosition.x, currentPiece.transform.position.y, worldPosition.z);
            currentPiece.transform.position = newPos;
        }

        private void EnableLegalMoves(ChessPiece piece)
        {
            DisableAllSquares();

            piece.CurrentSquare.SetEnabled(true);
            var legalMoves = legalMovesCalculator.GetLegalMoves(piece, piece.CurrentSquare, chessboard.Squares);
            foreach (var square in legalMoves)
            {
                // Highlight or mark the valid moves (e.g., change color of squares)
                square.SetEnabled(true);
            }
        }

        private ChessPiece TrySelectPiece(Vector3 pointerPosition)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(pointerPosition), out var hit))
            {
                return hit.collider.GetComponent<ChessPiece>();
            }
            return null;
        }

        private Square TryGetSquare(Vector3 pointerPosition)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(pointerPosition), out var hit))
            {
                return hit.collider.GetComponent<Square>();
            }
            return null;
        }

        private ChessPiece TryGetPieceUnderCursor()
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out var hit))
            {
                return hit.collider.GetComponent<ChessPiece>();
            }
            return null;
        }

        private void PlacePiece(Square targetSquare)
        {
            if (targetSquare.OccupyingPiece && targetSquare.OccupyingPiece.PieceColor != currentPiece.PieceColor)
            {
                chessboard.DestroyPieceOnSquare(targetSquare);
            }

            currentPiece.CurrentSquare.Clear();
            targetSquare.SetOccupyingPiece(currentPiece);
            currentPiece = null;
            isMovingPiece = false;
            PiecePlaced?.Invoke();
        }

        private void DisableAllSquares()
        {
            foreach (var square in chessboard.Squares)
                square.SetEnabled(false);
        }
    }
}
