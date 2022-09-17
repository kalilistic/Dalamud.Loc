namespace Dalamud.Loc.Attributes;

/// <summary>
/// Language code attribute for use with enums.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.All)]
public class LanguageCodeAttribute : System.Attribute
{
    /// <summary>
    /// Two letter language code per ISO 639-1.
    /// </summary>
    public string TwoLetterISOCode;

    /// <summary>
    /// Initializes a new instance of the <see cref="LanguageCodeAttribute"/> class.
    /// </summary>
    /// <param name="twoLetterIsoCode">Two digit iso language code.</param>
    public LanguageCodeAttribute(string twoLetterIsoCode)
    {
        this.TwoLetterISOCode = twoLetterIsoCode;
    }
}
