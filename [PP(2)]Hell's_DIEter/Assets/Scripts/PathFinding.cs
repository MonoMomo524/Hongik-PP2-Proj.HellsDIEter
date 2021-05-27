using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public GameObject slime;
    public Transform target, seeker;
    private Slime slimeScript;
    Grid grid;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Start()
    {
        slimeScript = slime.GetComponent<Slime>();
    }

    private void Update()
    {
        if (slimeScript.destination == null)
            return;
        target = slimeScript.destination;
        FindPath(seeker.transform.position, target.position);
    }

    // A*알고리즘을 통한 길찾기 메소드
    void FindPath(Vector3 startpos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startpos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            // openSet에 0을 넣었으니 1부터 시작!
            for(int i = 1; i < openSet.Count; i++)
            {
                if(openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost)
                {
                    currentNode = openSet[i];
                }
            }

            //
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            //
            foreach (Node neighbour in grid.GetNeighbors(currentNode))
            {
                if(!neighbour.walkable || closedSet.Contains(neighbour))
                    continue;

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.hCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;
    }

    // 노드 간의 거리 계산 메소드
    int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(distX > distY)
            return 14 * distX + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }
}
