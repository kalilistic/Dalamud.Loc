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
    /// Loads language by loading file, deserializing json and adding to available languages.
    /// </summary>
    /// <param name="language">Language code.</param>
    /// <param name="filePath">Filepath to JSON file.</param>
    void LoadLanguageFromFile(Language language, string filePath);

    /// <summary>
    /// Loads language by loading files, deserializing json and adding to available languages.
    /// </summary>
    /// <param name="languageFilePaths">Tuples with language key and filepath for language.</param>
    void LoadLanguagesFromFiles(IEnumerable<Tuple<Language, string>> languageFilePaths);

    /// <summary>
    /// Loads language from embedded assembly resource, deserializing json and adding to available languages.
    /// </summary>
    /// <param name="language">Language code.</param>
    /// <param name="resourcePath">Resource path to embedded json resource.</param>
    void LoadLanguageFromAssembly(Language language, string resourcePath);

    /// <summary>
    /// Loads language from embedded assembly resources, deserializing json and adding to available languages.
    /// </summary>
    /// <param name="resourcePaths">Resource path to embedded json resource.</param>
    void LoadLanguagesFromAssembly(IEnumerable<Tuple<Language, string>> resourcePaths);

    /// <summary>
    /// Loads language from url, deserializing json and adding to available languages.
    /// </summary>
    /// <param name="language">Language code.</param>
    /// <param name="url">URL with json content.</param>
    void LoadLanguageFromUrl(Language language, string url);

    /// <summary>
    /// Loads languages from urls, deserializing json and adding to available languages.
    /// </summary>
    /// <param name="urls">URLs with json content.</param>
    void LoadLanguagesFromUrls(IEnumerable<Tuple<Language, string>> urls);

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
