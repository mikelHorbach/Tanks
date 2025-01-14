﻿using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float dampTime = 0.2f;
    public float screenEdgeBuffer = 4f;
    public float minSize = 6.5f;
    [HideInInspector] public Transform[] targets;

    private Camera cam;
    private float zoomSpeed;
    private Vector3 moveVelocity;
    private Vector3 desiredPosition;


    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
    }


    private void FixedUpdate()
    {
        Move();
        Zoom();
    }


    private void Move()
    {
        FindAveragePosition();

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref moveVelocity, dampTime);
    }


    private void FindAveragePosition()
    {
        Vector3 averagePosition = new Vector3();
        int numTargets = 0;
        for (int i =0; i < targets.Length; i++)
        {
            if (!targets[i].gameObject.activeSelf) continue;
            averagePosition += targets[i].position;
            numTargets++;
        }

        if (numTargets > 0) averagePosition /= numTargets;

        averagePosition.y = transform.position.y;

        desiredPosition = averagePosition;
    }


   
    private void Zoom()
    {
        float requiredSize = FindRequiredSize();
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, requiredSize, ref zoomSpeed, dampTime);
    }


    private float FindRequiredSize()
    {
        Vector3 desiredLocalPosition = transform.InverseTransformPoint(desiredPosition);

        float size = 0f;

        for(int i =0;i < targets.Length;i++)
        {
            if (!targets[i].gameObject.activeSelf) continue;

            Vector3 targetLocalPos = transform.InverseTransformPoint(targets[i].position);

            Vector3 desiredPositionToTarget = targetLocalPos - desiredLocalPosition;

            size = Mathf.Max(size, Mathf.Abs(desiredPositionToTarget.y));
            size = Mathf.Max(size, Mathf.Abs(desiredPositionToTarget.x));


        }
        size += screenEdgeBuffer;

        size = Mathf.Max(size, minSize);

        return size;
    }


    public void SetStartPositionAndSize()
    {
        FindAveragePosition();

        transform.position = desiredPosition;

        cam.orthographicSize = FindRequiredSize();
    }
}