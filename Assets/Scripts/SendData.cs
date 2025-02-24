using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SendData : MonoBehaviour
{
    private string url = "https://gamejamv4api.bagros.eu/stats";

    [Serializable]
    private class StatsData
    {
        public string unity_string;
        public string team_name;
        public string unity_id;
        public int wood;
        public int food;
        public int stone;
        public int ants;
        public int gems;
    }


    void Start()
    {
        StartCoroutine(SendStatsPeriodically());
    }

    private IEnumerator SendStatsPeriodically()
    {
        while (true)
        {
            yield return new WaitForSeconds(30f);
            yield return SendStats();
        }
    }

    private IEnumerator SendStats()
    {
        if (GameManager.Instance.DevMode)
        {
            yield break;
        }

        //string json = JsonUtility.ToJson(stats);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            ResourcesManager.Instance.GameResources.TryGetValue(ResourcesManager.GameResourceType.Wood, out int wood);
            ResourcesManager.Instance.GameResources.TryGetValue(ResourcesManager.GameResourceType.Food, out int food);
            ResourcesManager.Instance.GameResources.TryGetValue(ResourcesManager.GameResourceType.Stone, out int stone);
            int ants = ResourcesManager.Instance.CountAllAnts();
            //ResourcesManager.Instance.GameResources.TryGetValue(ResourcesManager.GameResourceType.Ant, out int ants);
            ResourcesManager.Instance.GameResources.TryGetValue(ResourcesManager.GameResourceType.Gem, out int gems);

        string json = JsonUtility.ToJson(new StatsData
            {
                unity_string = "milujurizky",
                team_name = PlayerPrefs.GetString("username"),
                unity_id = SystemInfo.deviceUniqueIdentifier,
                wood = wood,
                food = food,
                stone = stone,
                ants = ants,
                gems = gems
        });

            byte[] jsonToSend = Encoding.UTF8.GetBytes(json);

            request.uploadHandler = new UploadHandlerRaw(jsonToSend);

            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("accept", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Chyba při odesílání dat: " + request.error);
                Debug.LogError(request.downloadHandler.text);
            }
            else
            {
                Debug.Log("Úspěšně odesláno, odpověď serveru: " + request.downloadHandler.text);

                UIManager.Instance.DataSendOK();
            }
        }
    }

}
