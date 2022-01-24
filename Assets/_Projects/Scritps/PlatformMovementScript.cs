using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovementScript : MonoBehaviour
{
    [SerializeField] private Transform platform;
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private float moveSpeed;
    [SerializeField] private List<Transform> nodes;

    public bool gizmoView {get;set;}
    private int index;
    private void Start()
    {
        CheckIfNodesEmpty();
        int side = nodes.Count; // No. of nodes
        int index = 0;
        platform.position = nodes[index].position;
        index ++ ;
    }

//  If node is empty Create one
    private void CheckIfNodesEmpty()
    {
        if(nodes.Count < 1)
            GenerateNode();
    }

    void FixedUpdate()
    {
        NodeBasedMovement();  
    }

//  Move Platform Based on Node Positions
    public void NodeBasedMovement()
    {
        Vector3 platformPosition = platform.position;
        if(platformPosition == nodes[index].position)
        {
            index++;
            if(index >= nodes.Count)
                index = 0;
        }
        Vector3 endNode = nodes[index].position;

        platformPosition = Vector3.MoveTowards(platformPosition,endNode,moveSpeed *  Time.deltaTime);
        platform.position = platformPosition;
    }

//  Shows Platfrom Path
    void OnDrawGizmos()
    {
        if(gizmoView)
        {        
            for (int i = 0; i < nodes.Count; i++)
            {
                if(nodes[i] == null)
                    continue;
                Vector2 startNode = nodes[i].position;
                Vector2 endNode = nodes[(i+1 == nodes.Count)?0:i+1].position;
                Gizmos.color = Color.green;
                Gizmos.DrawLine(startNode,endNode);
            }   
        }
    }

//  Create node and Add to list
    public void GenerateNode()
    {
        Transform newNode = Instantiate<GameObject>(nodePrefab,transform.position,Quaternion.identity,transform).transform;
        newNode.name = string.Format("Node {0}",nodes.Count+1);
        nodes.Add(newNode);
        newNode.gameObject.SetActive(true);
    }

//  Delete the last node
    public void DeleteNode()
    {
        Transform lastNode = nodes[nodes.Count-1];
        nodes.Remove(lastNode);
        DestroyImmediate(lastNode.gameObject);
    }
}
