using UnityEngine;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


public enum PrefType { String, Int, Float, DateTime }

public class PlayerPrefManager : SingletonBehaviour<PlayerPrefManager>
{
    #region === PlayerPrefs Core ===

    private const string _keysIndex = "PlayerPrefs_Keys";
    private const string _typeSuffix = "_type";
    private const string _defaultSuffix = "_default";

    public static event Action<string, object> OnPrefChanged;

    public void Start(){
        if (!PlayerPrefs.HasKey(_keysIndex))
            PlayerPrefs.SetString(_keysIndex, "None");
        InitializeDefaultKey();
    }

    public static void SaveAuto(string key, object value){
        if (value is int i) SaveValue(key, i, PrefType.Int);
        else if (value is float f) SaveValue(key, f, PrefType.Float);
        else if (value is string s) SaveValue(key, s, PrefType.String);
        else if (value is DateTime d) SaveValue(key, d.ToString("o"), PrefType.DateTime);
    }

    public static void SaveValue(string key, object value, PrefType type){
        SaveKey(key);
        PlayerPrefs.SetString($"{key}{_typeSuffix}", type.ToString());

        string encrypted = type switch{
            PrefType.Int => Encrypt(((int)value).ToString()),
            PrefType.Float => Encrypt(((float)value).ToString()),
            _ => Encrypt(value.ToString())
        };

        PlayerPrefs.SetString(key, encrypted);
        PlayerPrefs.Save();
        OnPrefChanged?.Invoke(key, value);
    }

    public static object GetValue(string key){
        string type = PlayerPrefs.GetString($"{key}{_typeSuffix}", "String");
        string raw = Decrypt(PlayerPrefs.GetString(key));

        return type switch
        {
            "Int" => int.TryParse(raw, out var i) ? i : -1,
            "Float" => float.TryParse(raw, out var f) ? f : -1f,
            "DateTime" => DateTime.TryParse(raw, out var d) ? d : default,
            _ => raw
        };
    }

    public static void DeleteValue(string key){
        var keys = GetAllKeys();
        if (keys.Contains(key)){
            keys.Remove(key);
            PlayerPrefs.SetString(_keysIndex, string.Join(",", keys));
        }

        PlayerPrefs.DeleteKey(key);
        PlayerPrefs.DeleteKey($"{key}{_typeSuffix}");
        PlayerPrefs.DeleteKey($"{key}{_defaultSuffix}");
        PlayerPrefs.Save();
    }

    public static List<string> GetAllKeys(){
        string savedKeys = PlayerPrefs.GetString(_keysIndex, "");
        var keys = new List<string>(savedKeys.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

        if (keys.Contains("None") && keys.Count > 1){
            keys.Remove("None");
        }

        return keys;
    }


    private static void SaveKey(string key){
        List<string> keys = GetAllKeys();
        if (!keys.Contains(key)){
            keys.Add(key);
            PlayerPrefs.SetString(_keysIndex, string.Join(",", keys));
        }
    }

    private void InitializeDefaultKey(){
        if (GetAllKeys().Count == 1 && GetAllKeys()[0] == "None")
            SaveValue("DefaultKey", "DefaultValue", PrefType.String);
    }


    #endregion

    #region === Encryption ===

    private static string Encrypt(string value){
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
    }

    private static string Decrypt(string value){
        try { return Encoding.UTF8.GetString(Convert.FromBase64String(value)); }
        catch { return value; }
    }

    #endregion

    #region === TryGet ===

    public static bool TryGetInt(string key, out int value){
        value = (int?)GetValue(key) ?? -1;
        return value != -1;
    }

    public static bool TryGetFloat(string key, out float value){
        value = (float?)GetValue(key) ?? -1f;
        return value != -1f;
    }

    public static bool TryGetString(string key, out string value){
        value = GetValue(key)?.ToString();
        return !string.IsNullOrEmpty(value);
    }

    #endregion

    #region === Defaults & Export ===

    public static void RegisterDefault(string key, object defaultValue){
        PlayerPrefs.SetString($"{key}{_defaultSuffix}", defaultValue.ToString());
    }

    public static void ResetToDefaults(){
        foreach (var key in GetAllKeys()){
            string defKey = $"{key}{_defaultSuffix}";
            if (PlayerPrefs.HasKey(defKey)){
                string def = PlayerPrefs.GetString(defKey);
                string type = PlayerPrefs.GetString($"{key}{_typeSuffix}", "String");

                switch (type){
                    case "Int": SaveValue(key, int.Parse(def), PrefType.Int); break;
                    case "Float": SaveValue(key, float.Parse(def), PrefType.Float); break;
                    case "DateTime": SaveValue(key, DateTime.Parse(def), PrefType.DateTime); break;
                    default: SaveValue(key, def, PrefType.String); break;
                }
            }
        }
    }

    #endregion

    #region === Inspector Interface ===

    [Dropdown(nameof(GetAllKeys))] public string selectedKey;

    [ShowIf(nameof(ApplicationIsPlaying)), Button("Afficher Valeur")]
    public void PrintSelected(){
        Debug.Log($"Clé : {selectedKey}, Valeur : {GetValue(selectedKey)}");
    }

    [ShowIf(nameof(ApplicationIsPlaying)), Button("Supprimer la clé")]
    public void DeleteSelected(){
        DeleteValue(selectedKey);
        Debug.Log($"Clé supprimée : {selectedKey}");
    }

    [ShowIf(nameof(ApplicationIsPlaying)), Button("Reset Defaults")]
    public void ResetAll(){
        ResetToDefaults();
        Debug.Log("PlayerPrefs réinitialisés aux valeurs par défaut.");
    }

    [ShowIf(nameof(ApplicationIsPlaying))] public string newKey;
    [ShowIf(nameof(ApplicationIsPlaying))] public string newValue;
    [ShowIf(nameof(ApplicationIsPlaying))] public PrefType type = PrefType.String;

    [ShowIf(nameof(ApplicationIsPlaying)), Button("Ajouter Nouvelle Clé")]
    public void AddNewKey(){
        if (string.IsNullOrEmpty(newKey)) { Debug.Log("Clé vide."); return; }

        object finalVal = type switch{
            PrefType.Int => int.TryParse(newValue, out var i) ? i : 0,
            PrefType.Float => float.TryParse(newValue, out var f) ? f : 0f,
            PrefType.DateTime => DateTime.TryParse(newValue, out var d) ? d : DateTime.UtcNow,
            _ => newValue
        };

        SaveValue(newKey, finalVal, type);
        Debug.Log($"Clé ajoutée : {newKey} = {finalVal} ({type})");
    }

    private bool ApplicationIsPlaying() => Application.isPlaying;

    #endregion
}
