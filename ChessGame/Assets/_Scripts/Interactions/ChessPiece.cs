using NaughtyAttributes;
using UnityEngine;

public class ChessPiece : MonoBehaviour
{
    [field: SerializeField] public PieceTypes PieceType { get; private set; }
    [field: SerializeField] public PieceColors PieceColor { get; private set; }

    [SerializeField, ReadOnly] private Outline outline;
    public Square CurrentSquare { get; private set; }
    public Square PreviousSquare { get; private set; }
    private Vector3 defaultEulers;
    [field: SerializeField] public int NumberOfTimesMoved;

    private void OnValidate()
    {
        if (outline == null) outline = GetComponent<Outline>();
    }

    private void Awake()
    {
        defaultEulers = transform.rotation.eulerAngles;
    }

    public void OnHoverEntered()
    {
        transform.rotation = Quaternion.Euler(defaultEulers + RandomVector3(-4f, 4f));
        if (outline) outline.enabled = true;
    }

    public void OnHoverExited()
    {
        transform.rotation = Quaternion.Euler(defaultEulers);
        if (outline) outline.enabled = false;
    }

    public void SetCurrentSquare(Square square)
    {
        if (NumberOfTimesMoved == 0 && CurrentSquare && !ReferenceEquals(square, CurrentSquare))
        {
            NumberOfTimesMoved++;
        }

        PreviousSquare = CurrentSquare;
        CurrentSquare = square;
    }

    public void SetOnSquareInitial(Square square)
    {
        NumberOfTimesMoved++;
        CurrentSquare = square;
    }

    private static Vector3 RandomVector3(float min, float max)
    {
        return new Vector3(
            Random.Range(min, max),
            Random.Range(min, max),
            Random.Range(min, max)
        );
    }
}

public enum PieceTypes { Pawn, Knight, Bishop, Rook, King, Queen }
public enum PieceColors { White, Black }
