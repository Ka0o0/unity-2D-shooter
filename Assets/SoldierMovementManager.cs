using System;
using System.Collections.Generic;
using Game;
using Game.Round;
using PathFinding;
using UnityEngine;
using UnityEngine.Assertions;

public class SoldierMovementManager : MonoBehaviour
{
    public GameObject BattleFieldManagerObject;

    public int IntermediateWalkingDistance = 3;
    public int FarWalkingDistance = 5;
    public bool ShowDistanceCircles { get; set; }
    public bool IsMoving { get; private set; }
    public int WalkingDistanceInCurrentRound = 0;

    public Vector2Int MinWalkingPoint
    {
        get
        {
            var minX = (int) transform.position.x - FarWalkingDistance;
            var minY = (int) transform.position.y - FarWalkingDistance;

            return new Vector2Int(
                minX >= 0 ? minX : 0,
                minY >= 0 ? minY : 0
            );
        }
    }

    public Vector2Int MaxWalkingPoint
    {
        get
        {
            return new Vector2Int(
                (int) transform.position.x + FarWalkingDistance,
                (int) transform.position.y + FarWalkingDistance
            );
        }
    }

    public int RemainingIntermediateDistance
    {
        get { return IntermediateWalkingDistance - WalkingDistanceInCurrentRound; }
    }

    public int RemainingFarDistance
    {
        get { return FarWalkingDistance - WalkingDistanceInCurrentRound; }
    }

    private List<GameObject> _previouslySelectedBlocks;
    private Vector2Int[] _movementPath;
    private int _currentPathPosition;
    private Vector2Int _targetPosition;
    private float _lastMovementTime = 0;
    private float CenterOffset = (float) 0.5;

    public void MoveToPositionUsingPath(Vector2Int position, Vector2Int[] path)
    {
        if (IsMoving)
        {
            return;
        }

        _currentPathPosition = 0;
        _targetPosition = position;
        _movementPath = path;
        IsMoving = true;
        WalkingDistanceInCurrentRound += DistanceOfPath(path);
    }

    private void Start()
    {
        IsMoving = false;
        Assert.IsTrue(IntermediateWalkingDistance < FarWalkingDistance);
    }

    private void FixedUpdate()
    {
        if (IsTimeToMoveForward() && IsMoving && _movementPath != null)
        {
            _currentPathPosition++;
            if (_currentPathPosition < _movementPath.Length)
            {
                MoveToPosition(_movementPath[_currentPathPosition]);
                _lastMovementTime = Time.timeSinceLevelLoad;
            }
            else
            {
                _movementPath = null;
                _currentPathPosition = 0;
                IsMoving = false;
                NotifyFinishedMoving();
            }
        }
    }

    private void Update()
    {
        if (ShowDistanceCircles)
        {
            if (_previouslySelectedBlocks == null)
            {
                SelectBlocks();
            }
        }
        else
        {
            DeselectAllPreviouslySelectedBlocks();
        }
    }

    private bool IsTimeToMoveForward()
    {
        return Time.timeSinceLevelLoad - _lastMovementTime >= 0.5;
    }

    private void MoveToPosition(Vector2Int position)
    {
        transform.position = new Vector3(position.x + CenterOffset, position.y + CenterOffset, 0);
    }

    private void SelectBlocks()
    {
        DeselectAllPreviouslySelectedBlocks();
        _previouslySelectedBlocks = new List<GameObject>();

        var battleFieldManager = BattleFieldManagerObject.GetComponent<BattleFieldManager>();
        var djikstra = MakeNewPathFinder();

        var paths = djikstra.GetReachablePaths();

        foreach (var tuple in paths)
        {
            var blockPosition = tuple.Key;
            var block = battleFieldManager.BattleFieldBlocks[blockPosition.x, blockPosition.y];
            if (DistanceOfPath(tuple.Value) > RemainingIntermediateDistance)
            {
                block.GetComponent<BattleFieldBlock>().State = BattleFieldBlock.DistanceState.FarDistance;
            }
            else
            {
                block.GetComponent<BattleFieldBlock>().State = BattleFieldBlock.DistanceState.IntermediateDistance;
            }
            _previouslySelectedBlocks.Add(block);
        }
    }

    private int DistanceOfPath(Vector2Int[] tupleItem2)
    {
        return tupleItem2.Length;
    }

    private void DeselectAllPreviouslySelectedBlocks()
    {
        if (_previouslySelectedBlocks == null)
        {
            return;
        }

        foreach (var previouslySelectedBlock in _previouslySelectedBlocks)
        {
            previouslySelectedBlock.GetComponent<BattleFieldBlock>().State =
                BattleFieldBlock.DistanceState.Default;
        }

        _previouslySelectedBlocks = null;
    }

    private void NotifyFinishedMoving()
    {
        var battleFieldManager = BattleFieldManagerObject.GetComponent<BattleFieldManager>();
        battleFieldManager.PlayerStateMachine.GetComponent<PlayerGameStateMachine>()
            .HandleEvent(new SoldierFinishedMovementEvent());
    }

    public SoldierMovementGameFieldPathFinding MakeNewPathFinder()
    {
        var battleFieldManager = BattleFieldManagerObject.GetComponent<BattleFieldManager>();
        return new SoldierMovementGameFieldPathFinding(this, battleFieldManager.BattleField,
            battleFieldManager.BattleFieldBlocks);
    }
}