using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SoldierMovementManager : MonoBehaviour
{
    public float IntermediateWalkingDistance;
    public float FarWalkingDistance;
    public GameObject BattleFieldManagerObject;

    private Soldier _soldier;

    private List<GameObject> _previouslySelectedBlocks;

    private void Start()
    {
        _soldier = GetComponent<Soldier>();

        Assert.IsTrue(IntermediateWalkingDistance < FarWalkingDistance);
    }

    private void Update()
    {
        if (_soldier.IsSelected)
        {
            SelectBlocks();
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

        var intermediateBlocks = GetIntermediateDistanceBlocks();
        var farDistanceBlocks = GetFarDistanceBlocks();

        foreach (var farDistanceBlock in farDistanceBlocks)
        {
            farDistanceBlock.GetComponent<BattleFieldBlockManager>().State =
                BattleFieldBlockManager.DistanceState.FAR_DISTANCE;
            _previouslySelectedBlocks.Add(farDistanceBlock);
        }

        foreach (var intermediateBlock in intermediateBlocks)
        {
            intermediateBlock.GetComponent<BattleFieldBlockManager>().State =
                BattleFieldBlockManager.DistanceState.INTERMEDIATE_DISTANCE;
            _previouslySelectedBlocks.Add(intermediateBlock);
        }
    }

    private GameObject[] GetIntermediateDistanceBlocks()
    {
        return GetBlockAroundPlayerWithinRadius((int) IntermediateWalkingDistance);
    }

    private GameObject[] GetFarDistanceBlocks()
    {
        return GetBlockAroundPlayerWithinRadius((int) FarWalkingDistance);
    }

    private GameObject[] GetBlockAroundPlayerWithinRadius(int radius)
    {
        var currentPosition = new Vector2Int(
            (int) transform.position.x,
            (int) transform.position.y
        );
        var fieldBlocks = BattleFieldManagerObject.GetComponent<BattleFieldManager>().BattleFieldBlocks;
        var fieldsInRange = new List<GameObject>();

        for (var i = currentPosition.x - radius; i < currentPosition.x + radius; i++)
        {
            for (var j = currentPosition.y - radius; j < currentPosition.y + radius; j++)
            {
                if (i < 0 || j < 0 || i >= fieldBlocks.Length || j >= fieldBlocks[i].Length)
                {
                    continue;
                }

                if (!PointIsInsideRadius(new Vector2Int(i, j), currentPosition, radius))
                {
                    continue;
                }


                fieldsInRange.Add(fieldBlocks[i][j]);
            }
        }

        return fieldsInRange.ToArray();
    }

    private bool PointIsInsideRadius(Vector2Int point, Vector2Int center, int radius)
    {
        return Math.Pow(point.x - center.x, 2) + Math.Pow(point.y - center.y, 2) < Math.Pow(radius, 2);
    }

    private void DeselectAllPreviouslySelectedBlocks()
    {
        if (_previouslySelectedBlocks == null)
        {
            return;
        }

        foreach (var previouslySelectedBlock in _previouslySelectedBlocks)
        {
            previouslySelectedBlock.GetComponent<BattleFieldBlockManager>().State =
                BattleFieldBlockManager.DistanceState.DEFAULT;
        }

        _previouslySelectedBlocks = null;
    }
}