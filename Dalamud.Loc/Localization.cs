using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Dalamud.Loc.Attributes;
using Dalamud.Loc.Enums;
using Dalamud.Loc.Extensions;
using Dalamud.Loc.Interfaces;
using Dalamud.Plugin;
using Newtonsoft.Json;

namespace Dalamud.Loc;

/// <inheritdoc/>
public class Localization : ILocalization
{
    private readonly DalamudPluginInterface pluginInterface;

    // ReSharper disable once CollectionNeverQueried.Local
    private readonly Dictionary<Language, Dictionary<string, string>> strings = new ();
    private readonly Dictionary<string, Language> languageCodes = new ();
    private Language currentLanguage;

    /// <summary>
    /// Initializes a new instance of the <see cref="Localization"/> class.
    /// </summary>
    /// <param name="pluginInterface">Dalamud plugin interface.</param>
    public Localization(DalamudPluginInterface pluginInterface)
    {
        this.pluginInterface = pluginInterface;
        this.pluginInterface.LanguageChanged += this.LanguageChanged;

        foreach (Language language in Enum.GetValues(typeof(Language)))
        {
            var type = typeof(Language);
            var memInfo = type.GetMember(language.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(LanguageCodeAttribute), false);
            var twoLetterISOCode = ((LanguageCodeAttribute)attributes[0]).TwoLetterISOCode;
            this.languageCodes.Add(twoLetterISOCode, language);
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
    public List<Language> AvailableLanguages { get; } = new ();

    /// <inheritdoc/>
    public void Dispose()
    {
        this.pluginInterface.LanguageChanged -= this.LanguageChanged;
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
    public string GetString(string key, Language language) => this.strings[language].ContainsKey(key) ? this.strings[language][key] : key;

    /// <inheritdoc/>
    public string GetString(string key) => this.strings[this.currentLanguage].ContainsKey(key) ? this.strings[this.currentLanguage][key] : key;

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
