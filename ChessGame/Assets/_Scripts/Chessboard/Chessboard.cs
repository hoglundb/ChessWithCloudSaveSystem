using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chessboard : MonoBehaviour
{
    [SerializeField] private Square squarePrefab;
    [SerializeField] private Transform boardStartCorner;
    [SerializeField] private List<ChessPiece> chessPiecePrefabs;
    [SerializeField] private PieceSpawner pieceSpawner;

    public Square[,] Squares { get; private set; } = new Square[8, 8];
    private List<ChessPiece> pieces = new();

    private void Start()
    {
        SetupSquares();
        pieceSpawner.SpawnAllPieces(); // Spawn pieces after setting up the board
    }

    public void OnPiecePickedUp() => SetLayerForAllPieces("Ignore Raycast");

    public void OnPiecePlaced() => SetLayerForAllPieces("Default");

    public ChessPiece SpawnPiece(PieceTypes type, PieceColors color)
    {
        var prefab = chessPiecePrefabs.FirstOrDefault(p => p.PieceType == type && p.PieceColor == color);
        if (!prefab) throw new ArgumentException("Piece prefab not found");

        var piece = Instantiate(prefab);
        pieces.Add(piece);
        return piece;
    }

    private void SetupSquares()
    {
        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                var square = Instantiate(squarePrefab, boardStartCorner.position + new Vector3(i, 0, j), Quaternion.identity);
                square.FileIndex = i;
                square.RankIndex = j;
                Squares[i, j] = square;
            }
        }
    }

    public void DestroyPieceOnSquare(Square targetSquare)
    {
        var pieceToDestroy = targetSquare.OccupyingPiece;
        if (pieceToDestroy == null) return;

        pieces.Remove(pieceToDestroy);
        Destroy(pieceToDestroy.gameObject);
    }

    private void SetLayerForAllPieces(string layerName)
    {
        foreach(var piece in pieces)
        {
            piece.gameObject.layer = LayerMask.NameToLayer(layerName);
        }
    }
}
