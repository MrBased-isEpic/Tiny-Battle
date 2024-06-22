using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    private const int MOVE_COST = 1;
    private bool foundPath = false;

    public bool FindPath(RoadCell startCell, RoadCell endCell)
    {
        PathFindingRoadCell currentCell;
        
        List<PathFindingRoadCell> OpenList  = new List<PathFindingRoadCell>();
        List<PathFindingRoadCell> CloseList = new List<PathFindingRoadCell>();

        //startCell.pathFindingRoadCell = new PathFindingRoadCell(startCell, endCell);

        foreach(RoadCell roadCell in GameManager.Instance.mapRoadCells)
        {
            roadCell.pathFindingRoadCell = new PathFindingRoadCell(roadCell, endCell);
            roadCell.pathFindingRoadCell.CalculateFCost();
            OpenList.Add(roadCell.pathFindingRoadCell);
        }

        //Debug.Log("OpenList Size: "+OpenList.Count);
        currentCell = startCell.pathFindingRoadCell;
        //Debug.Log("currentCell's PathFindingRoadCell: "+ currentCell.roadCell.transform.position);
        if(currentCell != null)
        {
            //Debug.Log("StartCell has a pathFindingRoadCell Attached");
        }

        CloseList.Add(currentCell);
        currentCell.SetGCost(0);
        currentCell.CalculateHCost(endCell);
        currentCell.CalculateFCost();
        //Debug.Log("currentCell fCost: "+currentCell.fCost);

        Debug.Log("Pathfinding Started");

        while (OpenList.Count != 0) // check if we haven't checked all roadCells
        {
            currentCell = findLowestFCostCell(OpenList);
            if(currentCell.roadCell == endCell)
            {
                foundPath = true;
                Debug.Log("found set true");
                break;
            }

            OpenList.Remove(currentCell);
            CloseList.Add(currentCell);
            List<PathFindingRoadCell> Neighbours = new List<PathFindingRoadCell>();

            Neighbours = GetNeighbourCellList(currentCell);

            foreach(PathFindingRoadCell neighbourCell in Neighbours) // for each NeighbourCell 
            {
                if(CloseList.Contains(neighbourCell))
                {
                    continue;
                }

                int tentativeGCost = currentCell.gCost + CalculateDistance(currentCell.roadCell, neighbourCell.roadCell);

                if(tentativeGCost < neighbourCell.gCost)
                {
                    //Debug.Log(neighbourCell.roadCell.transform.position.y - currentCell.roadCell.transform.position.y);
                    neighbourCell.SetPrevRoadCell(currentCell.roadCell);
                    neighbourCell.SetGCost(tentativeGCost);
                    neighbourCell.CalculateHCost(endCell);
                    neighbourCell.CalculateFCost();

                    if (!OpenList.Contains(neighbourCell))
                    {
                        OpenList.Add(neighbourCell);
                    }
                }
            }
        }

        RoadCell nowRoadCell = currentCell.roadCell;
        RoadCell prevRoadCell = null;
        Debug.Log("nowRoadCell: "+nowRoadCell.transform.position);
        while(nowRoadCell != startCell)
        {
            prevRoadCell = nowRoadCell.pathFindingRoadCell.prevRoadCell;
            Debug.Log("prevRoadCell: " + prevRoadCell.transform.position);
            if (prevRoadCell != null)
            {
                if (Mathf.Abs(nowRoadCell.transform.position.x - prevRoadCell.transform.position.x) <= 1 && Mathf.Abs(nowRoadCell.transform.position.y - prevRoadCell.transform.position.y) <= 1)
                {
                    
                }
                else
                {
                    foundPath = false;
                    Debug.Log("found set false");
                    break;
                }
                nowRoadCell = prevRoadCell;
            }
            else
            {
                break;
            }
        }

        foreach(PathFindingRoadCell roadCell in OpenList)
        {
            roadCell.prevRoadCell = null;
        }
        if (foundPath)
            Debug.Log("Path Found");
        else
            Debug.Log("Path Not Found");
        return foundPath;
    }
    protected void GetPath(Truck truck, RoadCell startCell, List<RoadCell> ReversePathList, List<RoadCell> PathList)
    {     
        PathList.Clear();
        while(truck.currentCell != startCell)
        {
            //Debug.Log("CurrentCell's position:"+truck.currentCell.transform.position);
            RoadCell NextRoadCell = truck.currentCell.pathFindingRoadCell.prevRoadCell;
            //Debug.Log("PrevCell's position:"+NextRoadCell.transform.position);
            truck.SetCurrentCell(NextRoadCell);
            ReversePathList.Add(truck.currentCell);
            //Debug.Log("Added a roadcell to reversed list");
        }
        
        for(int i = ReversePathList.Count - 1; i >= 0; i--)
        {
            PathList.Add(ReversePathList[i]);
            //Debug.Log(ReversePathList[i].transform.position);
        }
    }
    private List<PathFindingRoadCell> GetNeighbourCellList(PathFindingRoadCell currentCell)
    {
        List<PathFindingRoadCell> Neighbours = new List<PathFindingRoadCell>();
        //Debug.Log("CurrentCell's position: "+currentCell.roadCell.transform.position);

        foreach(GridManager.Direction direction in GridManager.Directions)
        {
            RoadCell neighbourRoadCell = currentCell.roadCell.GetNeighbourRoadCell(direction); //Take a RoadCell from each direction
            PathFindingRoadCell NeighbourPathFindingRoadCell = null;

            if(neighbourRoadCell != null) //Check if it exists
            {
                NeighbourPathFindingRoadCell = neighbourRoadCell.pathFindingRoadCell;
            }
            if(NeighbourPathFindingRoadCell != null) //Check if NeighbourPathFindingRoadCell was assigned
            {
                Neighbours.Add(NeighbourPathFindingRoadCell);
                //Debug.Log("NeighBourCell's position: " + neighbourRoadCell.transform.position);
            }
        }

        return Neighbours;
    }
    private PathFindingRoadCell findLowestFCostCell(List<PathFindingRoadCell> openList)
    {
        PathFindingRoadCell lowestFCostPathFindingRoadCell = openList[0];
        for(int i = 0; i<openList.Count ; i++)
        {
            if(openList[i].fCost < lowestFCostPathFindingRoadCell.fCost)
            {
                lowestFCostPathFindingRoadCell = openList[i];
            }
        }
        return lowestFCostPathFindingRoadCell;
    }
    private int CalculateDistance(RoadCell startRoadcell, RoadCell endRoadCell)
    {
        int xDistance = (int)Mathf.Abs(startRoadcell.transform.position.x - endRoadCell.transform.position.x);
        int yDistance = (int)Mathf.Abs(startRoadcell.transform.position.y - endRoadCell.transform.position.y);
        return MOVE_COST * (xDistance + yDistance);
    }


    #region PathFindingRoadCell Class
    public class PathFindingRoadCell
    {
        public RoadCell roadCell;
        public int gCost;
        public int hCost;
        public int fCost;
        public RoadCell prevRoadCell;

        public PathFindingRoadCell(RoadCell roadCell, RoadCell endingCell)
        {
            this.roadCell = roadCell;
            gCost = Mathf.Abs(int.MaxValue);
            prevRoadCell = null;
            hCost = 0;
        }
        public void SetGCost(int gCost)
        {
            this.gCost = gCost;
        }
        public void CalculateHCost(RoadCell endingCell)
        {
            hCost = (int)(Mathf.Abs(roadCell.transform.position.x - endingCell.transform.position.x) + Mathf.Abs(roadCell.transform.position.y - endingCell.transform.position.y));
        }
        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }
        public void SetPrevRoadCell(RoadCell roadCell)
        {
            prevRoadCell = roadCell;
        }
    }
    #endregion
}