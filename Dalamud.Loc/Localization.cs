using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;

using Dalamud.Loc.Attributes;
using Dalamud.Loc.Enums;
using Dalamud.Loc.Extensions;
using Dalamud.Loc.Interfaces;
using Dalamud.Plugin;
using Dalamud.Utility;
using Newtonsoft.Json;

namespace Dalamud.Loc;

/// <inheritdoc/>
public class Localization : ILocalization
{
    // ReSharper disable once CollectionNeverQueried.Local
    private readonly Dictionary<Language, Dictionary<string, string>> strings = new ();
    private readonly DalamudPluginInterface? pluginInterface;
    private readonly HttpClient httpClient;
    private readonly Dictionary<string, Language> languageCodes = new ();
    private Language currentLanguage;

    /// <summary>
    /// Initializes a new instance of the <see cref="Localization"/> class.
    /// </summary>
    /// <param name="pluginInterface">Dalamud plugin interface.</param>
    /// <param name="httpClient">httpClient (will init if not passed).</param>
    public Localization(DalamudPluginInterface? pluginInterface = null, HttpClient? httpClient = null)
    {
        if (pluginInterface != null)
        {
            this.pluginInterface = pluginInterface;
            this.pluginInterface.LanguageChanged += this.LanguageChanged;
        }

        this.httpClient = httpClient ?? new HttpClient();

        foreach (Language lang in Enum.GetValues(typeof(Language)))
        {
            var type = typeof(Language);
            var memInfo = type.GetMember(lang.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(LanguageCodeAttribute), false);
            var twoLetterISOCode = ((LanguageCodeAttribute)attributes[0]).TwoLetterISOCode;
            this.languageCodes.Add(twoLetterISOCode, lang);
        }
    }

    /// <inheritdoc/>
    public Language CurrentLanguage
    {
        get => this.currentLanguage;

        set
        {
            if (!this.AvailableLanguages.Contains(value))
            {
                throw new Exception($"Can't set current language to {value}, since it's not loaded.");
            }

            this.currentLanguage = value;
        }
    }

    /// <inheritdoc/>
    public bool UseFallbacks { get; set; } = true;

    /// <inheritdoc/>
    public List<Language> AvailableLanguages { get; } = new ();

    /// <inheritdoc/>
    public void Dispose()
    {
        if (this.pluginInterface != null)
        {
            this.pluginInterface.LanguageChanged -= this.LanguageChanged;
        }

        this.httpClient.Dispose();
    }

    /// <inheritdoc/>
    public void LoadLanguage(Language language, string jsonString)
    {
        this.LoadStrings(jsonString, language);
    }

    /// <inheritdoc/>
    public void LoadLanguages(IEnumerable<Tuple<Language, string>> keyedJsonStrings)
    {
        foreach (var kvp in keyedJsonStrings)
        {
            this.LoadLanguage(kvp.Item1, kvp.Item2);
        }
    }

    /// <inheritdoc/>
    public void LoadLanguageFromFile(Language language, string filePath)
    {
        var jsonString = File.ReadAllText(filePath);
        this.LoadStrings(jsonString, language);
    }

    /// <inheritdoc/>
    public void LoadLanguagesFromFiles(IEnumerable<Tuple<Language, string>> languageFilePaths)
    {
        foreach (var kvp in languageFilePaths)
        {
            var jsonString = File.ReadAllText(kvp.Item2);
            this.LoadLanguage(kvp.Item1, jsonString);
        }
    }

    /// <inheritdoc/>
    public void LoadLanguageFromAssembly(Language language, string resourcePath)
    {
        var jsonString = Assembly.GetCallingAssembly().GetResourceContent(resourcePath);
        this.LoadStrings(jsonString, language);
    }

    /// <inheritdoc/>
    public void LoadLanguagesFromAssembly(IEnumerable<Tuple<Language, string>> resourcePaths)
    {
        foreach (var kvp in resourcePaths)
        {
            var jsonString = Assembly.GetCallingAssembly().GetResourceContent(kvp.Item2);
            this.LoadLanguage(kvp.Item1, jsonString);
        }
    }

    /// <inheritdoc/>
    public void LoadLanguagesFromAssembly(string baseResourcePath)
    {
        foreach (Language lang in Enum.GetValues(typeof(Language)))
        {
            var type = typeof(Language);
            var memInfo = type.GetMember(lang.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(LanguageCodeAttribute), false);
            var twoLetterISOCode = ((LanguageCodeAttribute)attributes[0]).TwoLetterISOCode;
            var jsonString = Assembly.GetCallingAssembly().GetResourceContent($"{baseResourcePath}.{twoLetterISOCode}.json");
            if (!jsonString.IsNullOrEmpty())
            {
                this.LoadLanguage(lang, jsonString);
            }
        }
    }

    /// <inheritdoc/>
    public void LoadLanguageFromUrl(Language language, string url)
    {
        var jsonString = this.httpClient.GetStringAsync(url).Result;
        this.LoadStrings(jsonString, language);
    }

    /// <inheritdoc/>
    public void LoadLanguagesFromUrls(IEnumerable<Tuple<Language, string>> urls)
    {
        foreach (var kvp in urls)
        {
            var jsonString = this.httpClient.GetStringAsync(kvp.Item2).Result;
            this.LoadLanguage(kvp.Item1, jsonString);
        }
    }

    /// <inheritdoc/>
    public string GetString(string key, Language language)
    {
        var str = this.strings[language].ContainsKey(key) ? this.strings[language][key] : key;
        if (str.Equals(key) && this.UseFallbacks && language != Language.English)
        {
            // ReSharper disable once TailRecursiveCall
            return this.GetString(key, Language.English);
        }

        return str;
    }

    /// <inheritdoc/>
    public string GetString(string key)
    {
        return this.GetString(key, this.currentLanguage);
    }

    /// <inheritdoc/>
    public string[] GetStrings(IEnumerable<string> keys)
    {
        return this.GetStrings(keys, this.currentLanguage);
    }

    /// <inheritdoc/>
    public string[] GetStrings(IEnumerable<string> keys, Language language)
    {
        var translated = new List<string>();
        foreach (var key in keys)
        {
            translated.Add(this.GetString(key, language));
        }

        return translated.ToArray();
    }

    /// <inheritdoc/>
    public string[] GetStrings<T>()
    {
        return this.GetStrings<T>(this.currentLanguage);
    }

    /// <inheritdoc/>
    public string[] GetStrings<T>(Language language)
    {
        var names = Enum.GetNames(typeof(T));
        var localizedOptions = new List<string>();
        foreach (var optionName in names)
        {
            localizedOptions.Add(this.GetString(optionName, language));
        }

        return localizedOptions.ToArray();
    }

    private void LoadStrings(string jsonString, Language language)
    {
        this.AvailableLanguages.Add(language);
        this.strings[language] = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString) !;
    }

    private void LanguageChanged(string langCode)
    {
        this.languageCodes.TryGetValue(langCode, out var language);
        this.CurrentLanguage = language;
    }
}
