using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BattleFieldBlock : MonoBehaviour
{
    public enum DistanceState
    {
        Default,
        IntermediateDistance,
        FarDistance,
        OutOfReach
    }

    public bool HasTopWall = false;
    public bool HasLeftWall = false;
    public bool HasRightWall = false;
    public bool HasBottomWall = false;

    public Sprite DefaultTileSprite;
    public Sprite IntermediateDistanceSprite;
    public Sprite FarDistanceSprite;

    public DistanceState State = DistanceState.Default;

    private SpriteRenderer _tileSpriteRenderer;
    private SpriteRenderer _backgroundSpriteRenderer;

    // Use this for initialization
    private void Start()
    {
        var spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        Assert.IsTrue(spriteRenderer.Length == 2);

        if (spriteRenderer[0].name == "Tile" && spriteRenderer[1].name == "Gras")
        {
            _tileSpriteRenderer = spriteRenderer[0];
            _backgroundSpriteRenderer = spriteRenderer[1];
        }
        else if (spriteRenderer[1].name == "Tile" && spriteRenderer[0].name == "Gras")
        {
            _tileSpriteRenderer = spriteRenderer[1];
            _backgroundSpriteRenderer = spriteRenderer[0];
        }
        else
        {
            // WTF don't know fatalError equivalent in c#
            Assert.IsTrue(false);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        switch (State)
        {
            case DistanceState.FarDistance:
                _tileSpriteRenderer.sprite = FarDistanceSprite;
                break;
            case DistanceState.IntermediateDistance:
                _tileSpriteRenderer.sprite = IntermediateDistanceSprite;
                break;
            default:
                _tileSpriteRenderer.sprite = DefaultTileSprite;
                break;
        }
    }
}