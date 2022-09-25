# Dalamud.Loc
[![Nuget](https://img.shields.io/nuget/v/Dalamud.Loc)](https://www.nuget.org/packages/Dalamud.Loc/)

A localization library to use with dalamud plugin projects for FFXIV. Avoid boring boilerplate code in your plugin project and use this instead. The library supports JSON and can load strings directly, embedded resources, and local/remote files. Brought to you by [SheepGoMeh](https://github.com/SheepGoMeh) and [kalilistic](https://github.com/kalilistic).

### Sample Json
```json
{
  "OptionShowFriends": "Afficher les amis",
  "OptionLanguage": "Langue",
  "LanguageName": "Français"
}
```

### Basic Example

```csharp

// create localization obj
var loc = new Localization(dalamudPluginInterface);

// load language
loc.LoadLanguage(Language.French, frenchStringsJson);

// set current language
loc.CurrentLanguage = Language.French;

// use localized strings
var text = loc.GetString("MyStringKey");

// clean up when done
loc.Dispose();

```

### Other ways to load strings

```csharp

// load from string
loc.LoadLanguage(Language.French, frenchStringsJson);

// load from local file
loc.LoadLanguage(Language.French, pluginInterface.ConfigDirectory + "/loc/fr.json");

// load from embedded resource
loc.LoadLanguage(Language.French, "MyPlugin.Resource.translation.fr.json");

// load from remote file
loc.LoadLanguage(Language.French, urlToJson);

```