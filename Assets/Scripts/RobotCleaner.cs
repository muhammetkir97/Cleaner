using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotCleaner : MonoBehaviour
{
    private RobotStatus robotStatus = RobotStatus.Working;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movementDirection = new Vector3(ControllerSystem.Instance.GetAxis("Horizontal"), 0, ControllerSystem.Instance.GetAxis("Vertical"));
        transform.Translate(movementDirection * 0.1f);
    }

    public RobotStatus GetRobotStatus()
    {
        return robotStatus;
    }
}


public enum RobotStatus
{
    Working,
    Stopped
}