using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TurtleNav : MonoBehaviour
{
    public GameObject nodeParent;
    private List<GameObject> nodes, checkedNodes;
    NavMeshAgent agent;


    void Awake() {
        nodes = new List<GameObject>();
        checkedNodes = new List<GameObject>();
        agent = gameObject.GetComponent<NavMeshAgent>();

        for (int i = 0; i <= nodeParent.transform.childCount - 1; i++)
        {
            nodes.Add(nodeParent.transform.GetChild(i).gameObject);
        }

        FindRandomNode();
    }
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Node")) {
            ChangeLists(other.gameObject);
        }
    }

    void ChangeLists(GameObject swapNode) {
        nodes.Remove(swapNode);
        if (checkedNodes.Contains(swapNode) == false) {
            checkedNodes.Add(swapNode);
        }
        
        if (nodes.Count < 1) {
            RefillList();
            FindRandomNode();
        }
        else if (agent.remainingDistance <= 3) {
            FindRandomNode();
        }
    }

    void RefillList() {
        checkedNodes.Clear();
        for (int i = 0; i <= nodeParent.transform.childCount - 1; i++) {
            nodes.Add(nodeParent.transform.GetChild(i).gameObject);
        }
    }

    void FindRandomNode() {
        int rand = Random.Range(0, nodes.Count);
        agent.destination = nodes[rand].transform.position;
    }
}
