using System;
using UnityEngine;

[Serializable]
public class Square : MonoBehaviour
{
    public int FileIndex;
    public int RankIndex;

    public ChessPiece OccupyingPiece { get; private set; }

    [SerializeField] private Collider thisCollider;
    [SerializeField] private MeshRenderer thisRenderer;

    private void OnValidate()
    {
        if (thisCollider == null) thisCollider = GetComponent<Collider>();
        if (thisRenderer == null) thisRenderer = GetComponent<MeshRenderer>();
    }

    private void Awake() => SetEnabled(false);

    public void SetOccupyingPiece(ChessPiece piece, bool isInital = false)
    {
        piece.transform.position = transform.position;
        OccupyingPiece = piece;

        if (isInital) piece.SetOnSquareInitial(this);
        else piece.SetCurrentSquare(this);
    }

    public void SetEnabled(bool setEnabled)
    {
        thisCollider.enabled = setEnabled;
        thisRenderer.enabled = setEnabled;
    }

    public void Clear() => OccupyingPiece = null;
}
