using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class JsonSerializer : MonoBehaviour
{
    public DataClass dataObj;
    public string filePath;
    public TMP_InputField userName, score, wins, firstName, lastName;
    public List<PlayerDatum> fullDataList;
    public GameObject holdCanvas;
    public TMP_Text displayText;
    public PlayerDatum currentlySelectedEntry;
    public bool entryExists = false;

    // Start is called before the first frame update
    void Start()
    {
        //displayText.autoSizeTextContainer = true;
        filePath = Path.Combine(Application.dataPath, "saveData.txt");
        //dataObj = new DataClass();
        //dataObj.level = 1;
        //dataObj.timeElapsed = 44443.232323f;
        //dataObj.name = "Jordan";
        string json = JsonUtility.ToJson(dataObj);
        Debug.Log(json);
        //StartCoroutine(SendWebData(json));
        File.WriteAllText(filePath, json);
        GameObject.DontDestroyOnLoad(holdCanvas);
        StartCoroutine(GetRequest("http://localhost:3000/getUnity"));

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SendButton()
    {
        DataClass dataClass = new DataClass();
        dataClass.score = currentlySelectedEntry.score;
        dataClass.wins = currentlySelectedEntry.wins;
        dataClass.userName = currentlySelectedEntry.userName;
        dataClass.firstName = currentlySelectedEntry.firstName;
        dataClass.lastName = currentlySelectedEntry.lastName;
        string json = JsonUtility.ToJson(dataClass);
        filePath = Path.Combine(Application.dataPath, "formData.txt");
        File.WriteAllText(filePath, json);
        StartCoroutine(SendWebData(json));
        SceneManager.LoadScene("GameOver");
        fullDataList = SortArrayByWins(fullDataList);
        //displayText = GameObject.Find("DisplayText").GetComponent<TMP_Text>();
        DeconstructAndDisplay(fullDataList);
    }

    public void GetButton()
    {
        
        StartCoroutine(GetRequest("http://localhost:3000/getUnity"));

    }

    public void FindButton()
    {
        Debug.Log(userName.text);
        FindEntry(userName.text);
        if (!entryExists)
        {
            FillUnfilledSelected();
        }
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene("Game");
    }

    IEnumerator SendWebData(string json)
    {
        using (UnityWebRequest request = UnityWebRequest.Post("http://localhost:3000/unity", json, "application-json"))
        {
            request.SetRequestHeader("content-type", "application/json");

            request.uploadHandler.contentType = "application/json";
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("Data Posted");
            }
            request.uploadHandler.Dispose();
        }

    }

    IEnumerator UpdateWebData(string json)
    {
        using (UnityWebRequest request = UnityWebRequest.Post("http://localhost:3000/updateUnity", json, "application-json"))
        {
            request.SetRequestHeader("content-type", "application/json");

            request.uploadHandler.contentType = "application/json";
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("Data Posted");
            }
            request.uploadHandler.Dispose();
        }
    }

    IEnumerator DeleteWebData(string json)
    {
        using (UnityWebRequest request = UnityWebRequest.Post("http://localhost:3000/deleteUnity", json, "application-json"))
        {
            request.SetRequestHeader("content-type", "application/json");

            request.uploadHandler.contentType = "application/json";
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("Data Posted");
            }
            request.uploadHandler.Dispose();
        }
    }


    public void UpdateEntryButton()
    {
        if (currentlySelectedEntry.userName == string.Empty)
        {
            Debug.Log("No entry loaded");

        }
        else
        {
            currentlySelectedEntry.userName = userName.text;
            currentlySelectedEntry.firstName = firstName.text;
            currentlySelectedEntry.lastName = lastName.text;
            //currentlySelectedEntry.score = int.Parse(score.text);
            //currentlySelectedEntry.wins = int.Parse(wins.text);
            Debug.Log(FindEntryByID(currentlySelectedEntry._id));

            fullDataList[FindEntryByID(currentlySelectedEntry._id)].SetTo(currentlySelectedEntry);
            Debug.Log(fullDataList[FindEntryByID(currentlySelectedEntry._id)].userName);
            //SortArray(fullDataList);

            //DeconstructAndDisplay(fullDataList, displayText);

            string json = JsonUtility.ToJson(currentlySelectedEntry);
            filePath = Path.Combine(Application.dataPath, "formData.txt");
            File.WriteAllText(filePath, json);
            StartCoroutine(UpdateWebData(json));
            SceneManager.LoadScene("GameOver");
            fullDataList = SortArrayByWins(fullDataList);
            
            DeconstructAndDisplay(fullDataList);
        }
    }

    public List<PlayerDatum> SortArrayByWins(List<PlayerDatum> data)
    {
        var n = data.Count;
        for (int i = 0; i < n - 1; i++)
            for (int j = 0; j < n - i - 1; j++)
                if (data[j].wins < data[j + 1].wins)
                {
                    var tempVar = data[j];
                    data[j] = data[j + 1];
                    data[j + 1] = tempVar;
                }
        return data;
    }

    public void DeleteEntryButton()
    {

        if (currentlySelectedEntry.userName == string.Empty)
        {
            Debug.Log("No entry loaded");

        }
        else
        {
            fullDataList.Remove(fullDataList[FindEntryByID(currentlySelectedEntry._id)]);

            SortArray(fullDataList);

            DeconstructAndDisplay(fullDataList);

            string json = JsonUtility.ToJson(currentlySelectedEntry);
            filePath = Path.Combine(Application.dataPath, "formData.txt");
            File.WriteAllText(filePath, json);
            StartCoroutine(DeleteWebData(json));
        }
    }

    public void FindEntry(string datumName)
    {
        bool isFound = false;
        for (int i = 0; i < fullDataList.Count - 1; i++)
        {
            if (fullDataList[i].userName.ToLower() == datumName.ToLower())
            {
                Debug.Log(fullDataList[i].userName);
                //score.text = fullDataList[i].score.ToString();
                //wins.text = fullDataList[i].wins.ToString();
                //firstName.text = fullDataList[i].firstName;
                //lastName.text = fullDataList[i].lastName;
                currentlySelectedEntry = fullDataList[i];
                isFound = true;
                entryExists = true;
                break;
            }
        }
        if (!isFound)
        {
            Debug.Log("No entry of that name found!");
            entryExists = false;
        }
            
    }

    public void FillUnfilledSelected()
    {
        currentlySelectedEntry.userName = userName.text; 
        currentlySelectedEntry.firstName = firstName.text; 
        currentlySelectedEntry.lastName = lastName.text; 
    }

    public int FindEntryByID(string datumID)
    {
        for (int i = 0; i < fullDataList.Count - 1; i++)
        {
            if (fullDataList[i]._id == datumID)
            {
                return i;
            }
        }
        Debug.Log("No entry of that id found!");
        return 0000;
    }

    public List<PlayerDatum> SortArray(List<PlayerDatum> data)
    {
        var n = data.Count;
        for (int i = 0; i < n - 1; i++)
            for (int j = 0; j < n - i - 1; j++)
                if (string.Compare(data[j].userName, data[j + 1].userName) == 1)
                {
                    var tempVar = data[j];
                    data[j] = data[j + 1];
                    data[j + 1] = tempVar;
                }
        return data;
    }
    public void DeconstructAndDisplay(List<PlayerDatum> playerData)
    {
        holdCanvas.SetActive(true);
        //displayText = GameObject.Find("DisplayText").GetComponent<TMP_Text>();
        for (int i = 0; i < 10; i++)
        {
            displayText.text += "Username: " + playerData[i].userName + "\nFirst Name: " + playerData[i].firstName + "\nLast Name: " + playerData[i].lastName + "\nScore: " + playerData[i].score + "\nWins: " + playerData[i].wins + "\n-----------------------------------------\n";

        }
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest getRequest = UnityWebRequest.Get(uri))
        {
            yield return getRequest.SendWebRequest();

            var newData = System.Text.Encoding.UTF8.GetString(getRequest.downloadHandler.data);
            Debug.Log(newData);
            var newGetRequestData = JsonUtility.FromJson<DataClass>(newData);
            Debug.Log(newGetRequestData);
            Debug.Log(newGetRequestData);

            var newGetRoot = JsonUtility.FromJson<Root>(newData);
            Debug.Log(newGetRoot.playerdata[0].score);
            fullDataList.Clear();
            for (int i = 0; i < newGetRoot.playerdata.Length; i++)
            {
                Debug.Log(newGetRoot.playerdata[i].userName);
                Debug.Log(newGetRoot.playerdata[i].firstName);
                Debug.Log(newGetRoot.playerdata[i].lastName);
                Debug.Log(newGetRoot.playerdata[i].score);
                Debug.Log(newGetRoot.playerdata[i].wins);
                fullDataList.Add(newGetRoot.playerdata[i]);
                Debug.Log("Added user " + fullDataList[i].userName);
            }

            //fullDataList = SortArray(fullDataList);
            fullDataList = SortArrayByWins(fullDataList);
            //DeconstructAndDisplay(fullDataList, displayText);
            //Debug.Log(newGetRequestData.name);
            //Debug.Log(newGetRequestData.level);
            //Debug.Log(newGetRequestData.timeElapsed);
        }
    }
}
