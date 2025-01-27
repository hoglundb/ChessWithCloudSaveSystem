using System.Collections.Generic;
using UnityEngine;

public class LegalMovesCalculator : MonoBehaviour
{
    public List<Square> GetLegalMoves(ChessPiece piece, Square currentSquare, Square[,] board)
    {
        return piece.PieceType switch
        {
            PieceTypes.Pawn => GetPawnMoves(piece, currentSquare, board),
            PieceTypes.Rook => GetRookMoves(currentSquare, board),
            PieceTypes.Bishop => GetBishopMoves(currentSquare, board),
            PieceTypes.Queen => GetQueenMoves(currentSquare, board),
            PieceTypes.Knight => GetKnightMoves(currentSquare, board),
            PieceTypes.King => GetKingMoves(currentSquare, board),
            _ => new List<Square>()
        };
    }

    private List<Square> GetPawnMoves(ChessPiece piece, Square currentSquare, Square[,] squares)
    {
        // Simplified logic for pawns
        legalMoves.Clear();
        var direction = piece.PieceColor == PieceColors.White ? 1 : -1;
        var targetRank = currentSquare.RankIndex + direction;

        // One square forward
        if (IsInBounds(currentSquare.FileIndex, targetRank) && squares[currentSquare.FileIndex, targetRank].OccupyingPiece == null)
        {
            legalMoves.Add(squares[currentSquare.FileIndex, targetRank]);
        }

        // Diagonals
        foreach (var fileOffset in new[] { -1, 1 })
        {
            var targetFile = currentSquare.FileIndex + fileOffset;
            if (IsInBounds(targetFile, targetRank) && squares[targetFile, targetRank]?.OccupyingPiece?.PieceColor != piece.PieceColor)
            {
                if (squares[targetFile, targetRank].OccupyingPiece && squares[targetFile, targetRank].OccupyingPiece.PieceColor != piece.PieceColor)
                    legalMoves.Add(squares[targetFile, targetRank]);
            }
        }

        // Two squares forward
        targetRank += direction;
        if (piece.NumberOfTimesMoved == 0 && IsInBounds(currentSquare.FileIndex, targetRank) && squares[currentSquare.FileIndex, targetRank].OccupyingPiece == null)
        {
            legalMoves.Add(squares[currentSquare.FileIndex, targetRank]);
        }

        return legalMoves;
    }

    private bool IsInBounds(int file, int rank) => file >= 0 && file < 8 && rank >= 0 && rank < 8;

    private List<Square> GetRookMoves(Square currentSquare, Square[,] board, bool clearList = true)
    {
        if(clearList) legalMoves.Clear();

        var directions = new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        // Loop through each direction the rook can move
        foreach (var direction in directions)
        {
            var nextSquare = GetNextSquareInDirection(currentSquare, direction, board);

            // While there are still squares to check in this direction
            while (nextSquare != null)
            {
                // If the square is empty, it is a valid move
                if (nextSquare.OccupyingPiece)
                {
                    if (nextSquare.OccupyingPiece.PieceColor != currentSquare.OccupyingPiece.PieceColor) legalMoves.Add(nextSquare);
                    break;
                }

                legalMoves.Add(nextSquare);

                // If the square is occupied by a friendly piece, stop checking further
                if (nextSquare.OccupyingPiece != null && nextSquare.OccupyingPiece.PieceColor == currentSquare.OccupyingPiece.PieceColor)
                    break;

                // If the square is occupied by an opponent's piece, it's a valid capture, but stop checking further in this direction
                if (nextSquare.OccupyingPiece != null && nextSquare.OccupyingPiece.PieceColor != currentSquare.OccupyingPiece.PieceColor)
                    break;

                // Move to the next square in the same direction
                nextSquare = GetNextSquareInDirection(nextSquare, direction, board);
            }
        }
        return legalMoves;
    }


    [SerializeField] private List<Square> legalMoves;
    private List<Square> GetBishopMoves(Square currentSquare, Square[,] board, bool clearList = true)
    {
        if(clearList) legalMoves.Clear();
     
        var directions = new Vector2Int[]
        {
        Vector2Int.up + Vector2Int.right,    // Top-right
        Vector2Int.up + Vector2Int.left,     // Top-left
        Vector2Int.down + Vector2Int.right,  // Bottom-right
        Vector2Int.down + Vector2Int.left    // Bottom-left
        };

        // Iterate through each direction the bishop can move
        foreach (var direction in directions)
        {
            var nextSquare = GetNextSquareInDirection(currentSquare, direction, board);

            while (nextSquare != null)
            {
                // If the next square is occupied by a friendly piece, stop there
                if (nextSquare.OccupyingPiece != null && nextSquare.OccupyingPiece.PieceColor == currentSquare.OccupyingPiece.PieceColor)
                    break;

                // Add valid square to the list of moves
                legalMoves.Add(nextSquare);

                // If the next square is occupied by an opponent's piece, it is a valid capture
                if (nextSquare.OccupyingPiece != null && nextSquare.OccupyingPiece.PieceColor != currentSquare.OccupyingPiece.PieceColor)
                    break;

                // Continue moving in the same direction
                nextSquare = GetNextSquareInDirection(nextSquare, direction, board);
            }
        }
        
        return legalMoves;
    }


    private List<Square> GetQueenMoves(Square currentSquare, Square[,] board)
    {
        GetRookMoves(currentSquare, board);
        GetBishopMoves(currentSquare, board, false);

        return legalMoves;
    }

    private List<Square> GetKnightMoves(Square currentSquare, Square[,] board)
    {
        var moves = new List<Square>();
        var knightOffsets = new Vector2Int[]
        {
        new Vector2Int(2, 1), new Vector2Int(2, -1), new Vector2Int(-2, 1), new Vector2Int(-2, -1),
        new Vector2Int(1, 2), new Vector2Int(1, -2), new Vector2Int(-1, 2), new Vector2Int(-1, -2)
        };

        foreach (var offset in knightOffsets)
        {
            var targetSquare = GetSquareAtOffset(currentSquare, offset, board);
            if (targetSquare != null)
                moves.Add(targetSquare);
        }
        return moves;
    }

    private List<Square> GetKingMoves(Square currentSquare, Square[,] board)
    {
        legalMoves.Clear();
        var directions = new Vector2Int[]
        {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right,
        Vector2Int.up + Vector2Int.right, Vector2Int.up + Vector2Int.left, Vector2Int.down + Vector2Int.right, Vector2Int.down + Vector2Int.left
        };

        foreach (var direction in directions)
        {
            var targetSquare = GetSquareAtOffset(currentSquare, direction, board);
            if (targetSquare != null)
                legalMoves.Add(targetSquare);
        }
        return legalMoves;
    }

    private Square GetNextSquareInDirection(Square currentSquare, Vector2Int direction, Square[,] board)
    {
        var newFile = currentSquare.FileIndex + direction.x;
        var newRank = currentSquare.RankIndex + direction.y;
        if (newFile >= 0 && newFile < 8 && newRank >=0 && newRank < 8)
        {
            return board[newFile, newRank];
        }
            
        return null;
    }

    private Square GetSquareAtOffset(Square currentSquare, Vector2Int offset, Square[,] board)
    {
        var newRow = currentSquare.RankIndex + offset.x;
        var newCol = currentSquare.FileIndex + offset.y;
        if (newRow >= 0 && newRow < board.GetLength(0) && newCol >= 0 && newCol < board.GetLength(1))
            return board[newRow, newCol];
        return null;
    }
}