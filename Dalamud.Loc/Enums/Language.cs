using Dalamud.Loc.Attributes;

namespace Dalamud.Loc.Enums;

/// <summary>
/// Language enum with two character iso codes.
/// </summary>
/// <remarks>
/// tw is not ISO compliant but used by Dalamud to distinguish between simplified/traditional chinese.
/// </remarks>
public enum Language
{
    [LanguageCode("en")]
    English,

    [LanguageCode("de")]
    German,

    [LanguageCode("fr")]
    French,

    [LanguageCode("ja")]
    Japanese,

    [LanguageCode("it")]
    Italian,

    [LanguageCode("pt")]
    Portuguese,

    [LanguageCode("ru")]
    Russian,

    [LanguageCode("es")]
    Spanish,

    [LanguageCode("sv")]
    Swedish,

    [LanguageCode("ko")]
    Korean,

    [LanguageCode("no")]
    Norwegian,

    [LanguageCode("zh")]
    ChineseSimplified,

    [LanguageCode("tw")]
    ChineseTraditional,
}
