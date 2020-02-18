using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "CreatManagerVarsContainer")]
public class ManagerVars : ScriptableObject
{
    public static ManagerVars GetManagerVars()
    {
        return Resources.Load<ManagerVars>("ManagerVarsContainter");
    }
    public List<Sprite> bgThemeSpriteList = new List<Sprite>();

    public GameObject characterPre;
    public GameObject normalPlatformPre;

    public float nextXPos = 0.554f, nextYPos = 0.645f;
}
