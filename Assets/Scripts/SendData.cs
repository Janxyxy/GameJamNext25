using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class SendData : MonoBehaviour
{
    private string url = "https://gamejamv4api.bagros.eu/stats";

    private class StatsData
    {
        public string unity_string;
        public string team_name;
        public string unity_id;
        public int wood;
        public int food;
        public int stone;
        public int ants;
    }


    void Start()
    {
        StartCoroutine(SendStatsPeriodically());
    }

    private IEnumerator SendStatsPeriodically()
    {
        while (true)
        {
            yield return SendStats();
            yield return new WaitForSeconds(30f);
        }
    }

    private IEnumerator SendStats()
    {
        //string json = JsonUtility.ToJson(stats);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            ResourcesManager.Instance.GameResources.TryGetValue(ResourcesManager.GameResourceType.Wood, out int wood);
            ResourcesManager.Instance.GameResources.TryGetValue(ResourcesManager.GameResourceType.Food, out int food);
            ResourcesManager.Instance.GameResources.TryGetValue(ResourcesManager.GameResourceType.Stone, out int stone);
            ResourcesManager.Instance.GameResources.TryGetValue(ResourcesManager.GameResourceType.Ant, out int ants);

        string json = JsonUtility.ToJson(new StatsData
            {
                unity_string = "milujurizky",
                team_name = PlayerPrefs.GetString("username"),
                unity_id = SystemInfo.deviceUniqueIdentifier,
                wood = wood,
                food = food,
                stone = stone,
                ants = ants
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
            }
        }
    }

}
