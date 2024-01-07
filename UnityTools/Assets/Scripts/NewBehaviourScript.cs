using System.Collections.Generic;
using UnityEngine;

public class LinklistNode
{
    public int val;
    public LinklistNode next;

    public LinklistNode(int val)
    {
        this.val = val;
        this.next = null;
    }
}

public class StSMap
{
    public int length;
    public List<List<Node>> levelsArr;

    public StSMap()
    {
        length = 16;
        levelsArr = new List<List<Node>>();
        CreateNodesNum();
    }

    public void CreateNodesNum()
    {
        int id = 1;
        for (int i = 0; i < length - 1; i++)
        {
            List<Node> level = new List<Node>();
            for (int j = 0; j < 5; j++)
            {
                level.Add(new Node
                {
                    type = AssignNodeType(),
                    next1 = null,
                    next2 = null,
                    level = i + 1,
                    beConnected = false,
                    id = id++
                });
            }
            levelsArr.Add(level);
        }
        for (int i = 0; i < levelsArr[0].Count; i++)
        {
            levelsArr[0][i].beConnected = true;
        }
        levelsArr.Add(new List<Node>
        {
            new Node
            {
                type = 5,
                level = length,
                beConnected = true,
                toConnect = true,
                id = 5
            }
        });
        BuildPaths();
    }

    public int AssignNodeType()
    {
        return UnityEngine.Random.value < 0.4f
            ? 0
            : UnityEngine.Random.value < 0.5f
                ? 1
                : UnityEngine.Random.value < 0.5f
                    ? 2
                    : UnityEngine.Random.value < 0.5f
                        ? 3
                        : 4;
    }

    public void BuildPaths()
    {
        for (int i = 0; i < length - 2; i++)
        {
            NodeConnected nodeConnected = new NodeConnected { index = 0, connectedNodesNum = 0 };
            for (int j = 0; j < levelsArr[i].Count; j++)
            {
                if (!levelsArr[i][j].beConnected && i != 0)
                    break;

                if ((UnityEngine.Random.value <= 0.1f || nodeConnected.connectedNodesNum == 2) && j != 0)
                {
                    nodeConnected.index++;
                    nodeConnected.connectedNodesNum = 0;
                }

                if (UnityEngine.Random.value <= 0.45f)
                {
                    levelsArr[i][j].next1 = levelsArr[i + 1][nodeConnected.index];
                    if (levelsArr[i + 1][nodeConnected.index] != null)
                        levelsArr[i + 1][nodeConnected.index].beConnected = true;

                    nodeConnected.index++;
                    if (nodeConnected.index > 4)
                        break;

                    levelsArr[i][j].next2 = levelsArr[i + 1][nodeConnected.index];
                    if (nodeConnected.index > 4)
                        break;

                    levelsArr[i + 1][nodeConnected.index].beConnected = true;
                    nodeConnected.connectedNodesNum = 1;
                }
                else
                {
                    levelsArr[i][j].next1 = levelsArr[i + 1][nodeConnected.index];
                    nodeConnected.connectedNodesNum++;
                    if (nodeConnected.index > 4)
                        break;

                    levelsArr[i + 1][nodeConnected.index].beConnected = true;
                }

                if (levelsArr[i][j].next1 != null)
                    levelsArr[i][j].toConnect = true;
            }
        }

        for (int i = 0; i < levelsArr[levelsArr.Count - 2].Count; i++)
        {
            if (levelsArr[levelsArr.Count - 2][i].beConnected)
            {
                levelsArr[levelsArr.Count - 2][i].next1 = levelsArr[levelsArr.Count - 1][0];
                levelsArr[levelsArr.Count - 2][i].toConnect = true;
            }
        }

        ShakeTree();
    }

    public void ShakeTree()
    {
        for (int i = 0; i < levelsArr.Count; i++)
        {
            for (int j = 0; j < levelsArr[i].Count; j++)
            {
                if (!levelsArr[i][j].beConnected || !levelsArr[i][j].toConnect)
                {
                    levelsArr[i].RemoveAt(j);
                    j--;
                }
            }
        }
    }
}

public class NodeConnected
{
    public int index;
    public int connectedNodesNum;
}

public class Node
{
    public int type;
    public Node next1;
    public Node next2;
    public int level;
    public bool beConnected;
    public int id;
    public bool toConnect;
}

public class StSMapVisualizer : MonoBehaviour
{
    public StSMap a;

    void Start()
    {
        a = new StSMap();
    }
}