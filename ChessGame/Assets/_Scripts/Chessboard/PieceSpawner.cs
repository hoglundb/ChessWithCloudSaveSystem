using UnityEngine;
public class PieceSpawner : MonoBehaviour
{
    [SerializeField] private Chessboard chessboard;

    public void SpawnAllPieces()
    {
        SpawnMainPieces(PieceColors.White, 0);
        SpawnPawns(PieceColors.White, 1);

        SpawnMainPieces(PieceColors.Black, 7);
        SpawnPawns(PieceColors.Black, 6);
    }

    private void SpawnMainPieces(PieceColors color, int rank)
    {
        var pieceOrder = new[]
        {
                PieceTypes.Rook, PieceTypes.Knight, PieceTypes.Bishop, PieceTypes.Queen,
                PieceTypes.King, PieceTypes.Bishop, PieceTypes.Knight, PieceTypes.Rook
            };

        for (var file = 0; file < 8; file++)
        {
            var piece = chessboard.SpawnPiece(pieceOrder[file], color);
            var square = chessboard.Squares[file, rank];
            square.SetOccupyingPiece(piece);
        }
    }

    private void SpawnPawns(PieceColors color, int rank)
    {
        for (var file = 0; file < 8; file++)
        {
            var piece = chessboard.SpawnPiece(PieceTypes.Pawn, color);
            var square = chessboard.Squares[file, rank];
            square.SetOccupyingPiece(piece);
        }
    }
}
