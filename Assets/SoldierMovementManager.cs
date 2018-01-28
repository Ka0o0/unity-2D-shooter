using System;
using System.Collections;
using System.Collections.Generic;
using PathFinding;
using UnityEngine;
using UnityEngine.Assertions;

public class SoldierMovementManager : MonoBehaviour
{
    public GameObject BattleFieldManagerObject;

    private Soldier _soldier;

    private List<GameObject> _previouslySelectedBlocks;

    private void Start()
    {
        _soldier = GetComponent<Soldier>();
    }

    private void Update()
    {
        if (_soldier.IsSelected)
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

    private void SelectBlocks()
    {
        DeselectAllPreviouslySelectedBlocks();
        _previouslySelectedBlocks = new List<GameObject>();

        var battleFieldManager = BattleFieldManagerObject.GetComponent<BattleFieldManager>();
        var battleField = battleFieldManager.BattleField;
        var djikstra = new SoldierMovementGameFieldPathFinding(_soldier, battleField);

        var paths = djikstra.GetReachablePaths();

        foreach (var tuple in paths)
        {
            var blockPosition = tuple.Item1;
            var block = battleFieldManager.BattleFieldBlocks[blockPosition.x, blockPosition.y];
            if (DistanceOfPath(tuple.Item2) > _soldier.IntermediateWalkingDistance)
            {
                block.GetComponent<BattleFieldBlock>().State = BattleFieldBlock.DistanceState.FAR_DISTANCE;
            }
            else
            {
                block.GetComponent<BattleFieldBlock>().State = BattleFieldBlock.DistanceState.INTERMEDIATE_DISTANCE;
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
                BattleFieldBlock.DistanceState.DEFAULT;
        }

        _previouslySelectedBlocks = null;
    }
}