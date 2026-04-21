using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//时间控制器
public class TimeController : MonoBehaviour
{
    public static float gravity = -100;

    public struct RecordedData              //记录发生过的数据
    {
        public Vector2 pos;                        //位置数据
        public Vector2 vel;                        //速度数据
    }

    RecordedData[,] recordedDatas;         //为啥要记录二维数据呢？因为可能有多个记录对象
    private int recordMax = 100000;         //最大记录数据量
    private int recordCount;                //当前记录总数
    private int recordIndex;                //当前数据位置
    private bool wasSteppingBack = false;

    TimeControlled[] timeObjects;

    private void Awake()
    {
        //这是Unity提供的一个静态方法，用于查找当前场景中所有激活的（active）游戏对象上挂载的指定类型组件。
        timeObjects = GameObject.FindObjectsOfType<TimeControlled>();

        recordedDatas = new RecordedData[timeObjects.Length, recordMax];
    }
    void Start()
    {

    }

    void Update()
    {
        Debug.Log("recordIndex = " + recordIndex);
        //定义控制按键
        bool pause = Input.GetKey(KeyCode.UpArrow);
        bool stepBack = Input.GetKey(KeyCode.LeftArrow);
        bool stepForward = Input.GetKey(KeyCode.RightArrow);

        if (stepBack)       //时间倒退
        {
            wasSteppingBack = true;
            
            if(recordIndex > 0)
            {
                recordIndex--;
                
                for (int objectIndex = 0; objectIndex < timeObjects.Length; objectIndex++)      //遍历所有时间控制对象，并记录数据
                {
                    TimeControlled timeObject = timeObjects[objectIndex];
                    RecordedData data = recordedDatas[objectIndex, recordIndex];
                    timeObject.transform.position = data.pos;
                    timeObject.velocity = data.vel;
                }
            }
            
            
        }
        else if (pause && stepForward)
        {
            wasSteppingBack = true;

            if (recordIndex < recordCount - 1)
            {
                recordIndex++;

                for (int objectIndex = 0; objectIndex < timeObjects.Length; objectIndex++)      //遍历所有时间控制对象，并记录数据
                {
                    TimeControlled timeObject = timeObjects[objectIndex];
                    RecordedData data = recordedDatas[objectIndex, recordIndex];
                    timeObject.transform.position = data.pos;
                    timeObject.velocity = data.vel;
                }
            }
        }
        else if (!pause && !stepBack)   //没有操作时间，此时时间正常流逝
        {
            if (wasSteppingBack)
            {
                recordCount = recordIndex;
                wasSteppingBack = false ;
            }

            for(int objectIndex = 0;  objectIndex < timeObjects.Length; objectIndex++)      //遍历所有时间控制对象，并记录数据
            {
                TimeControlled timeObject = timeObjects[objectIndex];
                RecordedData data = new RecordedData();
                data.pos = timeObject.transform.position;
                data.vel = timeObject.velocity;
                recordedDatas[objectIndex, recordCount] = data;
            }
            recordCount++;
            recordIndex = recordCount;          //正常时间流动当前记录数据位置就是记录总数

            foreach (TimeControlled timeObject in timeObjects)
            {
                timeObject.TimeUpdate();
            }
        }
    }
}
