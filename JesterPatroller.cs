using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JesterPatroller : MonoBehaviour
{
#region Variables

    public GameObject nodeParent;

    private List<GameObject> nodes, checkedNodes;

    public NavMeshAgent agent;
    public Animator anim;

#endregion

#region Monobehavior

    void Awake() {
        nodes = new List<GameObject>();
        checkedNodes = new List<GameObject>();
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Node")) {
            ChangeLists(other.gameObject);
        }
    }

    #endregion

    #region SetupAI

    public void StartAI(GameObject _nodeParent)
    {
        nodeParent = _nodeParent;
        for (int i = 0; i <= nodeParent.transform.childCount - 1; i++)
        {
            nodes.Add(nodeParent.transform.GetChild(i).gameObject);
        }

        agent = gameObject.GetComponent<NavMeshAgent>();
        anim = gameObject.GetComponentInChildren<Animator>();
        anim.SetTrigger("isSwimming");

        FindRandomNode();
    }

    #endregion

    #region List Handling

    void ChangeLists(GameObject swapNode) {
        nodes.Remove(swapNode);
        if (checkedNodes.Contains(swapNode) == false) {
            checkedNodes.Add(swapNode);
        }
        
        if (nodes.Count < 1) {
            RefillList();
            FindRandomNode();
        }
        else if (agent.remainingDistance <= 2) {
            FindRandomNode();
        }
    }

    void RefillList() {
        checkedNodes.Clear();
        for (int i = 0; i <= nodeParent.transform.childCount - 1; i++) {
            nodes.Add(nodeParent.transform.GetChild(i).gameObject);
        }
    }

#endregion

#region Node Handling

    void FindFurthestNode()
    {
        float maxDistance = 0f;
        GameObject furthestNode = null;

        foreach (GameObject node in nodes) {
            float currDistance = Vector3.Distance(this.transform.position, node.transform.position);
            
            if (currDistance > maxDistance) {
                maxDistance = currDistance;
                furthestNode = node;
            }
        }
        agent.destination = furthestNode.transform.position;
    }

    void FindRandomNode() {
        int rand = Random.Range(0, nodes.Count);
        agent.destination = nodes[rand].transform.position;
    }

#endregion

#region Test Pathfinding

    /* void Update() {
        gameObject.transform.LookAt(nodes[0].transform);
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, nodes[0].transform.position, Time.deltaTime *2);
    } */

#endregion
}
