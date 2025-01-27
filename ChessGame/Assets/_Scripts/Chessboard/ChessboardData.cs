//using System.Collections.Generic;
//using UnityEngine;

//public class ChessboardData : MonoBehaviour
//{
//    public enum PieceType { None, Pawn, Rook, Knight, Bishop, Queen, King }
//    public enum PieceColor { None, White, Black }

//    [System.Serializable]
//    public class ChessPieceData
//    {
//        public PieceType pieceType;
//        public PieceColor pieceColor;
//        public Vector2Int position;

//        public ChessPieceData()
//        {
//            pieceType = PieceType.None;
//            pieceColor = PieceColor.None;
//            position = new Vector2Int(-1, -1);
//        }

//        public ChessPieceData(PieceType type, PieceColor color, Vector2Int pos)
//        {
//            pieceType = type;
//            pieceColor = color;
//            position = pos;
//        }
//    }

//    public List<ChessPieceData> pieces = new List<ChessPieceData>();

//    public void InitializeBoard()
//    {
//        pieces.Clear();

//        // Standard chess starting positions
//        for (int x = 0; x < 8; x++)
//        {
//            pieces.Add(new ChessPieceData(PieceType.Pawn, PieceColor.White, new Vector2Int(x, 1)));
//            pieces.Add(new ChessPieceData(PieceType.Pawn, PieceColor.Black, new Vector2Int(x, 6)));
//        }
//        pieces.Add(new ChessPieceData(PieceType.Rook, PieceColor.White, new Vector2Int(0, 0)));
//        pieces.Add(new ChessPieceData(PieceType.Rook, PieceColor.White, new Vector2Int(7, 0)));
//        pieces.Add(new ChessPieceData(PieceType.Rook, PieceColor.Black, new Vector2Int(0, 7)));
//        pieces.Add(new ChessPieceData(PieceType.Rook, PieceColor.Black, new Vector2Int(7, 7)));
//        pieces.Add(new ChessPieceData(PieceType.Knight, PieceColor.White, new Vector2Int(1, 0)));
//        pieces.Add(new ChessPieceData(PieceType.Knight, PieceColor.White, new Vector2Int(6, 0)));
//        pieces.Add(new ChessPieceData(PieceType.Knight, PieceColor.Black, new Vector2Int(1, 7)));
//        pieces.Add(new ChessPieceData(PieceType.Knight, PieceColor.Black, new Vector2Int(6, 7)));
//        pieces.Add(new ChessPieceData(PieceType.Bishop, PieceColor.White, new Vector2Int(2, 0)));
//        pieces.Add(new ChessPieceData(PieceType.Bishop, PieceColor.White, new Vector2Int(5, 0)));
//        pieces.Add(new ChessPieceData(PieceType.Bishop, PieceColor.Black, new Vector2Int(2, 7)));
//        pieces.Add(new ChessPieceData(PieceType.Bishop, PieceColor.Black, new Vector2Int(5, 7)));
//        pieces.Add(new ChessPieceData(PieceType.Queen, PieceColor.White, new Vector2Int(3, 0)));
//        pieces.Add(new ChessPieceData(PieceType.Queen, PieceColor.Black, new Vector2Int(3, 7)));
//        pieces.Add(new ChessPieceData(PieceType.King, PieceColor.White, new Vector2Int(4, 0)));
//        pieces.Add(new ChessPieceData(PieceType.King, PieceColor.Black, new Vector2Int(4, 7)));
//    }

//    public PieceType[,] GetBoardArray()
//    {
//        PieceType[,] boardArray = new PieceType[8, 8];

//        for (int x = 0; x < 8; x++)
//        {
//            for (int y = 0; y < 8; y++)
//            {
//                boardArray[x, y] = PieceType.None;
//            }
//        }

//        foreach (ChessPieceData piece in pieces)
//        {
//            if (piece.position.x >= 0 && piece.position.x < 8 && piece.position.y >= 0 && piece.position.y < 8)
//            {
//                boardArray[piece.position.x, piece.position.y] = piece.pieceType;
//            }
//        }

//        return boardArray;
//    }

//    public PieceColor[,] GetColorArray()
//    {
//        PieceColor[,] colorArray = new PieceColor[8, 8];

//        for (int x = 0; x < 8; x++)
//        {
//            for (int y = 0; y < 8; y++)
//            {
//                colorArray[x, y] = PieceColor.None;
//            }
//        }

//        foreach (ChessPieceData piece in pieces)
//        {
//            if (piece.position.x >= 0 && piece.position.x < 8 && piece.position.y >= 0 && piece.position.y < 8)
//            {
//                colorArray[piece.position.x, piece.position.y] = piece.pieceColor;
//            }
//        }

//        return colorArray;
//    }

//    public ChessPieceData GetPieceAt(Vector2Int pos)
//    {
//        foreach (ChessPieceData piece in pieces)
//        {
//            if (piece.position == pos)
//            {
//                return piece;
//            }
//        }
//        return null;
//    }
//}