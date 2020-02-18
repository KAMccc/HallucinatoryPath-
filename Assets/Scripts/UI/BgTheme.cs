using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgTheme : MonoBehaviour
{
    private SpriteRenderer m_spriteRenderer;
    private ManagerVars vars;

    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();

        m_spriteRenderer = GetComponent<SpriteRenderer>();
        int ranValue = Random.Range(0, vars.bgThemeSpriteList.Count);
        m_spriteRenderer.sprite = vars.bgThemeSpriteList[ranValue];

    }
}
