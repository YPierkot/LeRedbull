#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Static{
    public static class YOP_StaticEditor{
        #region Variables
        //Size of Image
        public static int iconSize = 25;
        public static int textSize = iconSize - 4;

        //GUISTYLE for Text
        public static GUIStyle labelTitleStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 15 };
        public static GUIStyle labelStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 };

        //GUISTYLE for Text Button
        public static GUIStyle buttonStyle = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fontSize = 13, fontStyle = FontStyle.Bold, padding = new RectOffset(0, 0, 0, 0) };

        //GUISTYLE for Icon Button
        static int iconPadding = 2;
        public static GUIStyle buttonIconStyle = new GUIStyle(GUI.skin.button) { padding = new RectOffset(iconPadding, iconPadding, iconPadding, iconPadding) };

        //GUISTYLE for Toggle
        public static GUIStyle toggleStyle = new GUIStyle(GUI.skin.toggle) { padding = new RectOffset(0, 0, 0, 0) };

        #endregion Variables

        #region Box
        /// <summary>
        /// Create a Horizontal Box
        /// </summary>
        public static void BeginHorizontalBox(Color color, bool expandWidth = true, bool expandHeight = true){
            GUI.backgroundColor = color;
            GUILayout.BeginHorizontal("box", GUILayout.ExpandWidth(expandWidth), GUILayout.ExpandHeight(expandHeight));
            GUI.backgroundColor = Color.white;
        }

        /// <summary>
        /// Create a Vertical Box
        /// </summary>
        public static void BeginVerticalBox(Color color, bool expandWidth = true, bool expandHeight = true){
            GUI.backgroundColor = color;
            GUILayout.BeginVertical("box", GUILayout.ExpandWidth(expandWidth), GUILayout.ExpandHeight(expandHeight));
            GUI.backgroundColor = Color.white;
        }
        #endregion Box

        /// <summary>
        /// Draw a custom Property with a customLabel
        /// </summary>
        /// <param name="name"></param>
        /// <param name="prop"></param>
        public static void DrawProperty(string name, SerializedProperty prop){
            GUILayout.BeginHorizontal();
            GUILayout.Label(name, labelStyle, GUILayout.Width(120));
            EditorGUILayout.PropertyField(prop, GUIContent.none);
            GUILayout.EndHorizontal();
        }

        #region Other
        /// <summary>
        /// Create a line with a button which link to an application and property field
        /// </summary>
        /// <param name="buttonPath"></param>
        /// <param name="prop"></param>
        /// <param name="obj"></param>
        public static void DrawDataLine(string buttonPath, SerializedProperty prop, SerializedObject obj, bool disable){
            EditorGUI.BeginDisabledGroup(disable);
            GUILayout.BeginHorizontal();
            if(GUILayout.Button("+", buttonStyle, GUILayout.Width(textSize), GUILayout.Height(textSize))){
                Application.OpenURL(buttonPath);
            }
            GUILayout.Label(prop.displayName + " :", labelStyle, GUILayout.Width(90), GUILayout.Height(textSize));
            GUI.backgroundColor = Color.gray;
            EditorGUILayout.PropertyField(prop, GUIContent.none);
            GUI.backgroundColor = Color.white;
            obj.ApplyModifiedProperties();
            GUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
        }
        #endregion Other
    }

    #region JSON
    /// <summary>
    /// Split JSON files into strings
    /// </summary>
    public static class Json{
        /// <summary>
        /// Split a text to only get the necessary value
        /// </summary>
        /// <param name="splitText"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<object> SearchJsonData(string splitText, string json, FindDataType dataType){ 
            List<object> dataList = new List<object>();
            string newTxt = "";

            string[] jsonSplitter;
            string newJson;
            string[] jsonList;
            string[] shorterJsonList;
            string[] finalList;

            switch(dataType){
                case FindDataType.None:
                    break;

                //Get all lists on a board
                case FindDataType.CardList:
                    jsonSplitter = json.Split(new string[] { splitText }, StringSplitOptions.RemoveEmptyEntries);
                    jsonList = jsonSplitter[1].Split(new string[] { "\":[{" }, StringSplitOptions.RemoveEmptyEntries);
                    shorterJsonList = jsonList[0].Split(new string[] { "}]" }, StringSplitOptions.RemoveEmptyEntries);
                    finalList = shorterJsonList[0].Split(new string[] { "},{" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach(string txt in finalList){
                        newTxt = "{" + txt + "}";

                        CardListData data = JsonUtility.FromJson<CardListData>(newTxt);
                        if(!data.closed){
                            dataList.Add(data);
                        }
                    }
                    break;
                case FindDataType.BoardList:
                    jsonList = json.Split(new string[] { "[{\"n" }, StringSplitOptions.RemoveEmptyEntries);
                    shorterJsonList = jsonList[0].Split(new string[] { "}]}]" }, StringSplitOptions.RemoveEmptyEntries);
                    finalList = shorterJsonList[0].Split(new string[] { "]},{" }, StringSplitOptions.RemoveEmptyEntries);

                    int id = 0;
                    foreach(string txt in finalList){
                        if(id == 0)newTxt = "{\"n" + txt + "]}";
                        else if(id == finalList.Length - 1) newTxt = "{" + txt + "}]}";
                        else newTxt = "{" + txt + "]}";

                        BoardListData data = JsonUtility.FromJson<BoardListData>(newTxt);
                        if(!data.closed){
                            dataList.Add(data);
                        }
                        id++;
                    }
                    break;

                case FindDataType.Cards:
                    newJson = json.Remove(0, 7);
                    shorterJsonList = newJson.Split(new string[] { "}}]" }, StringSplitOptions.RemoveEmptyEntries);
                    finalList = shorterJsonList[0].Split(new string[] { "}},{\"id\":" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach(string txt in finalList){
                        if(System.Array.IndexOf(finalList, txt) != finalList.Length - 1) newTxt = "{\"id\":" + txt + "}}";
                        else newTxt = "{\"id\":" + txt + "}}";

                        CardsData data = JsonUtility.FromJson<CardsData>(newTxt);
                        dataList.Add(data);
                    }
                    break;

                case FindDataType.Labels:
                    newJson = json.Remove(0, 2);
                    shorterJsonList = newJson.Split(new string[] { "}]" }, StringSplitOptions.RemoveEmptyEntries);
                    finalList = shorterJsonList[0].Split(new string[] { "},{" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach(string txt in finalList){
                        newTxt = "{" + txt + "}";

                        CardLabel data = JsonUtility.FromJson<CardLabel>(newTxt);
                        dataList.Add(data);
                    }
                    break;
            }

            return dataList;
        }
	}

    /// <summary>
    /// Enum which allow Json to know which data to find
    /// </summary>
    public enum FindDataType{
        None,
        CardList,
        BoardList,
        Cards,
        Labels
    }

    [System.Serializable]
    /// <summary>
    /// Class for the cardList
    /// </summary>
    public class CardListData{
        public string id = "";
        public string name = "";
        public bool closed = false;
        public int pos = 0;
        public object softLimit = null;
        public string idBoard = "";
        public bool usefullForPlayer = false;

        public CardListData(string id, string name, bool closed, int pos, object softLimit, string idBoard){
            this.id = id;
            this.name = name;
            this.closed = closed;
            this.pos = pos;
            this.softLimit = softLimit;
            this.idBoard = idBoard;
        }
    }

    [System.Serializable]
    /// <summary>
    /// Class for the BoardList
    /// </summary>
    public class BoardListData{
        public string name = "";
        public string desc = "";
        public bool closed = false;
        public string idOrganization = "";
        public string id = "";
        public string url = "";
        public List<object> memberships = new List<object>();

        public BoardListData(string name, string desc, bool closed, string idOrganization, string id, string url, List<object> memberships){
            this.name = name;
            this.desc = desc;
            this.closed = closed;
            this.idOrganization = idOrganization;
            this.id = id;
            this.url = url;
            this.memberships = memberships;
        }
    }

    [System.Serializable]
    /// <summary>
    /// Class for all cards
    /// </summary>
    public class CardsData{
        public string name = "";
        public string id = "";
        public bool closed = false;
        public string desc = "";
        public string idBoard = "";
        public string idList = "";
        public List<string> idLabels = new List<string>();
        public int pos = 0;
        public List<object> idMembers = new List<object>();
        public string shortUrl = "";

        public CardsData(string name, string id, bool closed, string desc, string idBoard, string idList, List<string> idLabels, int pos, List<object> idMembers, string shortUrl){
            this.name = name;
            this.id = id;
            this.closed = closed;
            this.desc = desc;
            this.idBoard = idBoard;
            this.idList = idList;
            this.idLabels = idLabels;
            this.pos = pos;
            this.idMembers = idMembers;
            this.shortUrl = shortUrl;
        }
    }

    /// <summary>
    /// Class for all the labels on a board
    /// </summary>
    public class CardLabel{
        public string id = "";
        public string idBoard = "";
        public string name = "";
        public string color = "";

        public CardLabel(string id, string idBoard, string name, string color){
            this.id = id;
            this.idBoard = idBoard;
            this.name = name;
            this.color = color;
        }
    }

    #endregion JSON
}
#endif
