using System;
using System.Collections.Generic;

using Dalamud.Loc.Enums;

namespace Dalamud.Loc.Interfaces;

/// <summary>
/// Localization class.
/// </summary>
public interface ILocalization
{
    /// <summary>
    /// Gets or sets current UI language if already loaded.
    /// </summary>
    Language CurrentLanguage { get; set; }

    /// <summary>
    /// Gets list of available languages with loaded localization.
    /// </summary>
    List<Language> AvailableLanguages { get; }

    /// <summary>
    /// Dispose localization service.
    /// </summary>
    void Dispose();

    /// <summary>
    /// Loads language by deserializing json and adding to available languages.
    /// </summary>
    /// <param name="language">Language code.</param>
    /// <param name="jsonString">String in json format of key value pairs for a language.</param>
    void LoadLanguage(Language language, string jsonString);

    /// <summary>
    /// Loads language by deserializing json and adding to available languages.
    /// </summary>
    /// <param name="keyedJsonStrings">Tuples with language key and json strings for language.</param>
    void LoadLanguages(IEnumerable<Tuple<Language, string>> keyedJsonStrings);

    /// <summary>
    /// Gets string by key in target language.
    /// </summary>
    /// <param name="key">String key.</param>
    /// <param name="language">Language to return string.</param>
    /// <returns>translated string.</returns>
    string GetString(string key, Language language);

    /// <summary>
    /// Gets string by key in current language.
    /// </summary>
    /// <param name="key">String key.</param>
    /// <returns>translated string.</returns>
    string GetString(string key);
}
