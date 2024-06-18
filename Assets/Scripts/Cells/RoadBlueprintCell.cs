using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBlueprintCell : BaseCell
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform cantPlaceHereVisual;
    public bool canBePlaced;

    GridManager.Direction prevCellDirection = GridManager.Direction.None;
    GridManager.Direction nextCellDirection = GridManager.Direction.None;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void TurnOnCantPlaceHereVisual()
    {
        cantPlaceHereVisual.gameObject.SetActive(true);
        canBePlaced = false;
    }
    public void TurnOffCantPlaceHereVisual()
    {
        cantPlaceHereVisual.gameObject.SetActive(false);
        canBePlaced = true;
    }

    public void FindNextCellDirection(RoadBlueprintCell roadBlueprintCell)
    {
        Vector2 diff = transform.position - roadBlueprintCell.transform.position;

        if(diff == Vector2.right)
        {
            nextCellDirection = GridManager.Direction.Left;
        }
        else if(diff == Vector2.down)
        {
            nextCellDirection = GridManager.Direction.Up;
        }
        else if(diff == Vector2.left)
        {
            nextCellDirection = GridManager.Direction.Right;
        }
        else if(diff == Vector2.up)
        {
            nextCellDirection = GridManager.Direction.Down;
        }
        RefreshVisuals();
    }
    public void FindPrevCellDirection(Transform roadBlueprintCell)
    {
        Vector2 diff = transform.position - roadBlueprintCell.position;

        if(diff == Vector2.right)
        {
            prevCellDirection = GridManager.Direction.Left;
        }
        else if(diff == Vector2.down)
        {
            prevCellDirection = GridManager.Direction.Up;
        }
        else if(diff == Vector2.left)
        {
            prevCellDirection = GridManager.Direction.Right;
        }
        else if(diff == Vector2.up)
        {
            prevCellDirection = GridManager.Direction.Down;
        }
        RefreshVisuals();
    }

    private void RefreshVisuals()
    {
        if(nextCellDirection == GridManager.Direction.None)
        {
            if(prevCellDirection == GridManager.Direction.Right)
            {
                spriteRenderer.sprite = GridManager.Instance.RoadBlueprintCellSprites[0];
            }
            else if (prevCellDirection == GridManager.Direction.Down)
            {
                spriteRenderer.sprite = GridManager.Instance.RoadBlueprintCellSprites[1];
            }
            else if(prevCellDirection == GridManager.Direction.Left)
            {
                spriteRenderer.sprite = GridManager.Instance.RoadBlueprintCellSprites[2];
            }
            else if (prevCellDirection == GridManager.Direction.Up)
            {
                spriteRenderer.sprite = GridManager.Instance.RoadBlueprintCellSprites[3];
            }
        }
        else
        {
            if (prevCellDirection == GridManager.Direction.Right)
            {
                if(nextCellDirection == GridManager.Direction.Down)
                {
                    spriteRenderer.sprite = GridManager.Instance.RoadBlueprintCellSprites[8];
                }
                else if(nextCellDirection == GridManager.Direction.Left)
                {
                    spriteRenderer.sprite = GridManager.Instance.RoadBlueprintCellSprites[4];
                }
                else if(nextCellDirection == GridManager.Direction.Up)
                {
                    spriteRenderer.sprite = GridManager.Instance.RoadBlueprintCellSprites[7];
                }
            }
            else if(prevCellDirection == GridManager.Direction.Down)
            {
                if (nextCellDirection == GridManager.Direction.Left)
                {
                    spriteRenderer.sprite = GridManager.Instance.RoadBlueprintCellSprites[9];
                }
                else if (nextCellDirection == GridManager.Direction.Up)
                {
                    spriteRenderer.sprite = GridManager.Instance.RoadBlueprintCellSprites[5];
                }
                else if (nextCellDirection == GridManager.Direction.Right)
                {
                    spriteRenderer.sprite = GridManager.Instance.RoadBlueprintCellSprites[8];
                }
            }
            else if (prevCellDirection == GridManager.Direction.Left)
            {
                if (nextCellDirection == GridManager.Direction.Up)
                {
                    spriteRenderer.sprite = GridManager.Instance.RoadBlueprintCellSprites[6];
                }
                else if (nextCellDirection == GridManager.Direction.Right)
                {
                    spriteRenderer.sprite = GridManager.Instance.RoadBlueprintCellSprites[4];
                }
                else if (nextCellDirection == GridManager.Direction.Down)
                {
                    spriteRenderer.sprite = GridManager.Instance.RoadBlueprintCellSprites[9];
                }
            }
            else if (prevCellDirection == GridManager.Direction.Up)
            {
                if (nextCellDirection == GridManager.Direction.Right)
                {
                    spriteRenderer.sprite = GridManager.Instance.RoadBlueprintCellSprites[7];
                }
                else if (nextCellDirection == GridManager.Direction.Down)
                {
                    spriteRenderer.sprite = GridManager.Instance.RoadBlueprintCellSprites[5];
                }
                else if (nextCellDirection == GridManager.Direction.Left)
                {
                    spriteRenderer.sprite = GridManager.Instance.RoadBlueprintCellSprites[6];
                }
            }
        }
    }
}
