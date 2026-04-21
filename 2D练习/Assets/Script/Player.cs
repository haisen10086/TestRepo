using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : TimeControlled            //继承操控时间组件
{
    float moveSpeed = 5;
    float jumpVelocity = 40;

    void Start()
    {

    }

    public override void TimeUpdate()
    {
        Vector2 pos = transform.position;

        pos.y += velocity.y * Time.deltaTime;
        velocity.y += TimeController.gravity * Time.deltaTime;

        if (pos.y < 1)  //人造地板
        {
            pos.y = 1;
            velocity.y = 0;
        }

        if (Input.GetKeyDown(KeyCode.W) && pos.y ==1)
        {
            velocity.y = jumpVelocity;
        }
        if (Input.GetKey(KeyCode.A))
        {
            pos.x -= moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            pos.x += moveSpeed * Time.deltaTime;
        }

        transform.position = pos;

    }
}
