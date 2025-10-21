using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text txtSoul;
    public int soulsCount = 0;
    public int maxSouls = 0;


    public static ScoreManager Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {
        txtSoul.text = soulsCount + "/" + maxSouls;
    }

    public void AddSoul()
    {
        soulsCount++;
        txtSoul.text = soulsCount + "/" + maxSouls;
    }
}
