using UnityEngine;
[CreateAssetMenu(fileName = "LanguageDateabase", menuName = "ScriptableObjects/LanguageDateabase")]

public class LanguageManager : ScriptableObject
{
    public enum Language{
        Japan,
        English
    }
    [System.Serializable]
    public class TextData{
        [System.Serializable]
        public class LanguageData{
            public Language language;
            public string Text;
            public LanguageData(Language language,string Text){
                this.language = language;
                this.Text = Text;
            }
        }
        public LanguageData[] languageDatas = {new LanguageData(Language.Japan,""),new LanguageData(Language.English,"")};  
        public string GetTextFromLang(Language lang){
            string res = "ERR!!";
            foreach(LanguageData langData in languageDatas){
                if(langData.language == lang)res =langData.Text;
            }
            return res;
        }
    }
    public TextData[] textDatas;
    public Language nowLanguage;
    [HideInInspector]public static string textDatabasePath = "LanguageDatabase";
    public string GetTextFromID(int id,Language language){  
        string res = textDatas[id].GetTextFromLang(language);
        return res;
    }
    public string GetTextFromText(string FromText){
        string toText = "ERR!!";
        foreach(TextData textData in textDatas){
            foreach (TextData.LanguageData languageData in textData.languageDatas){
                if(FromText == textData.GetTextFromLang(languageData.language))toText = textData.GetTextFromLang(nowLanguage);
            }
        }
        return toText;
    }
}
