using UnityEngine;
using System.Collections;

public class TempleBuild : MonoBehaviour
{
    [SerializeField] private int _id; //Тип здания
    [SerializeField] private GameObject[] BuildLevelsPrefabs; //Модели Уровней здания
    public int BuildLevels; //уровень здания
    public GameObject DustBuilding;
    public static float chance;
    private GameObject Clone;

    public int GetId() { return _id; }

    private void Awake()
    {
        // BuildLevels = PlayerPrefs.GetInt("BuildLevels" + _id);
        BuildLevels = PlayerPrefs.GetInt("BuildLevels" + _id);
        // chance = PlayerPrefs.GetFloat("TempleChance" + _id);
        chance = PlayerPrefs.GetFloat("TempleChance" + _id);
        CheckerBuilding();
    }

    public void BuildNewBuilding()
    {
        BuildLevels++;
        chance = .1f;
        SaveBuildingsData();
        Clone = Instantiate(DustBuilding, transform.position, Quaternion.identity);
        StartCoroutine(CheckerBuildBuilding());
        Destroy(Clone, 7);
        GetComponent<AudioSource>().Play();
    }

    private void SaveBuildingsData()
    {
        PlayerPrefs.SetInt("BuildLevels" + _id, BuildLevels);
        PlayerPrefs.SetFloat("TempleChance" + _id, .1f);
    }

    IEnumerator CheckerBuildBuilding()
    {
        yield return new WaitForSeconds(7);
        CheckerBuilding();
    }

    private void CheckerBuilding()
    {
        foreach (GameObject buildings in BuildLevelsPrefabs)
        {
            buildings.SetActive(false);
        }

        if (BuildLevels >= 1)
        {
            BuildLevelsPrefabs[1].SetActive(true);
        }
        else
        {
            BuildLevelsPrefabs[0].SetActive(true);
        }

    }
}
