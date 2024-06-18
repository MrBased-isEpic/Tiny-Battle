using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RoadCell : BaseCell
{
    public PathFinder.PathFindingRoadCell pathFindingRoadCell {get; set;}
    private int neighboursCode;

    public void RefreshVisuals(bool notifyNeighbours)
    {
        neighboursCode = 0;
        if(GetNeighbourRoadCell(GridManager.Direction.Left) != null)
        {
            neighboursCode += 1000;
            if (notifyNeighbours)
            {
                GetNeighbourRoadCell(GridManager.Direction.Left).RefreshVisuals(false);
            }
        }
        if(GetNeighbourRoadCell(GridManager.Direction.Up) != null)
        {
            neighboursCode += 100;
            if (notifyNeighbours)
            {
                GetNeighbourRoadCell(GridManager.Direction.Up).RefreshVisuals(false);
            }
        }
        if(GetNeighbourRoadCell(GridManager.Direction.Right) != null)
        {
            neighboursCode += 10;
            if (notifyNeighbours)
            {
                GetNeighbourRoadCell(GridManager.Direction.Right).RefreshVisuals(false);
            }
        }
        if(GetNeighbourRoadCell(GridManager.Direction.Down) != null)
        {
            neighboursCode += 1;
            if (notifyNeighbours)
            {
                GetNeighbourRoadCell(GridManager.Direction.Down).RefreshVisuals(false);
            }
        }


        switch(neighboursCode)
        {
            case 0000:
                GetComponent<SpriteRenderer>().sprite = GridManager.Instance.RoadCellSprites[0];
                break;
            case 0001:
                GetComponent<SpriteRenderer>().sprite = GridManager.Instance.RoadCellSprites[4];
                break;
            case 0010:
                GetComponent<SpriteRenderer>().sprite = GridManager.Instance.RoadCellSprites[1];
                break;
            case 0011:
                GetComponent<SpriteRenderer>().sprite = GridManager.Instance.RoadCellSprites[5];
                break;
            case 0100:
                GetComponent<SpriteRenderer>().sprite = GridManager.Instance.RoadCellSprites[12];
                break;
            case 0101:
                GetComponent<SpriteRenderer>().sprite = GridManager.Instance.RoadCellSprites[8];
                break;
            case 0110:
                GetComponent<SpriteRenderer>().sprite = GridManager.Instance.RoadCellSprites[13];
                break;
            case 0111:
                GetComponent<SpriteRenderer>().sprite = GridManager.Instance.RoadCellSprites[9];
                break;
            case 1000:
                GetComponent<SpriteRenderer>().sprite = GridManager.Instance.RoadCellSprites[3];
                break;
            case 1001:
                GetComponent<SpriteRenderer>().sprite = GridManager.Instance.RoadCellSprites[7];
                break;
            case 1010:
                GetComponent<SpriteRenderer>().sprite = GridManager.Instance.RoadCellSprites[2];
                break;
            case 1011:
                GetComponent<SpriteRenderer>().sprite = GridManager.Instance.RoadCellSprites[6];
                break;
            case 1100:
                GetComponent<SpriteRenderer>().sprite = GridManager.Instance.RoadCellSprites[15];
                break;
            case 1101:
                GetComponent<SpriteRenderer>().sprite = GridManager.Instance.RoadCellSprites[11];
                break;
            case 1110:
                GetComponent<SpriteRenderer>().sprite = GridManager.Instance.RoadCellSprites[6];
                break;
            case 1111:
                GetComponent<SpriteRenderer>().sprite = GridManager.Instance.RoadCellSprites[10];
                break;
        }
    }
    public RoadCell GetNeighbourRoadCell(GridManager.Direction direction) 
    {
        Vector2 findPosition = Vector2.zero;

        switch(direction)
        {
            case GridManager.Direction.Up:
                findPosition = transform.position + new Vector3(0, 1);
                break; 
            case GridManager.Direction.Down:
                findPosition = transform.position + new Vector3(0, -1);
                break;
            case GridManager.Direction.Left:
                findPosition = transform.position + new Vector3(-1, 0);
                break;
            case GridManager.Direction.Right:
                findPosition = transform.position + new Vector3(1, 0);
                break;
        }
        return GridManager.Instance.GetCellFromPosition(findPosition) as RoadCell;
    }
}
