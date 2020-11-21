using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowLvl : MonoBehaviour
{
    private int lvl;

    [SerializeField]
    private LevelsCount levelsCount;


    public Text currentLvl, nextLvl;

    private void Start()
    {
        lvl = levelsCount.Lvl + 1;
        currentLvl.text = lvl.ToString();
        nextLvl.text = (lvl + 1).ToString();
    }
}
