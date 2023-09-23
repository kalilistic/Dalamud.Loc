using System.Numerics;

using Dalamud.Interface;
using ImGuiNET;

namespace Dalamud.Loc.ImGui;

/// <summary>
/// Dalamud.Loc backed ImGui components.
/// </summary>
public static class LocGui
{
    private static Localization loc = null!;

    /// <summary>
    /// Initialize LocGui.
    /// </summary>
    /// <param name="localization">plugin localization.</param>
    public static void Initialize(Localization localization) => loc = localization;

    /// <summary>
    /// Localized TextColored.
    /// </summary>
    /// <param name="key">primary key.</param>
    /// <param name="color">text color.</param>
    public static void TextColored(string key, Vector4 color) => ImGuiNET.ImGui.TextColored(color, loc.GetString(key));

    /// <summary>
    /// Localized Text.
    /// </summary>
    /// <param name="key">primary key.</param>
    public static void Text(string key) => ImGuiNET.ImGui.Text(loc.GetString(key));

    /// <summary>
    /// Localized ImGuiHelpers.SafeTextWrapped.
    /// </summary>
    /// <param name="key">primary key.</param>
    public static void SafeTextWrapped(string key) => ImGuiHelpers.SafeTextWrapped(loc.GetString(key));

    /// <summary>
    /// Localized InputText.
    /// </summary>
    /// <param name="key">primary key.</param>
    /// <param name="input">input text.</param>
    /// <param name="maxLength">max length.</param>
    /// <returns>indicator if changed.</returns>
    public static bool InputText(string key, ref string input, uint maxLength) => ImGuiNET.ImGui.InputText(loc.GetString(key), ref input, maxLength);

    /// <summary>
    /// Localized InputTextWithHint.
    /// </summary>
    /// <param name="key">primary key.</param>
    /// <param name="hintKey">hint text key.</param>
    /// <param name="input">text input.</param>
    /// <param name="length">max length.</param>
    /// <returns>Indicates if input text has changed.</returns>
    public static bool InputTextWithHint(string key, string hintKey, ref string input, uint length) =>
        ImGuiNET.ImGui.InputTextWithHint($"###{key}", loc.GetString(hintKey), ref input, length);

    /// <summary>
    /// Localized SmallButton.
    /// </summary>
    /// <param name="key">key for string/value.</param>
    /// <returns>indicator if button was clicked.</returns>
    public static bool SmallButton(string key) => ImGuiNET.ImGui.SmallButton(loc.GetString(key));

    /// <summary>
    /// Localized Button.
    /// </summary>
    /// <param name="key">key for string/value.</param>
    /// <returns>indicator if button was clicked.</returns>
    public static bool Button(string key) => ImGuiNET.ImGui.Button(loc.GetString(key));

    /// <summary>
    /// Localized Button with size.
    /// </summary>
    /// <param name="key">key for string/value.</param>
    /// <param name="size">vector2 size.</param>
    /// <returns>indicator if button was clicked.</returns>
    public static bool Button(string key, Vector2 size) => ImGuiNET.ImGui.Button(loc.GetString(key), size);

    /// <summary>
    /// Localized BeginMenu.
    /// </summary>
    /// <param name="key">key for string/value.</param>
    /// <returns>indicator if menu was clicked.</returns>
    public static bool BeginMenu(string key) => ImGuiNET.ImGui.BeginMenu(loc.GetString(key));

    /// <summary>
    /// Localized MenuItem.
    /// </summary>
    /// <param name="key">key for string/value.</param>
    /// <returns>indicator if menu item was clicked.</returns>
    public static bool MenuItem(string key) => ImGuiNET.ImGui.MenuItem(loc.GetString(key));

    /// <summary>
    /// Localized SetHoverTooltip.
    /// </summary>
    /// <param name="key">key for string/value.</param>
    public static void SetHoverTooltip(string key)
    {
        if (ImGuiNET.ImGui.IsItemHovered())
        {
            ImGuiNET.ImGui.SetTooltip(loc.GetString(key));
        }
    }

    /// <summary>
    /// Localized BeginTabItem.
    /// </summary>
    /// <param name="key">primary key.</param>
    /// <returns>indicator whether tab selected.</returns>
    public static bool BeginTabItem(string key)
    {
        if (ImGuiNET.ImGui.BeginTabItem(loc.GetString(key)))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Localized CollapsingHeader.
    /// </summary>
    /// <param name="key">primary key.</param>
    /// <param name="flags">tree node flags (defaults to none).</param>
    /// <returns>indicator whether header selected.</returns>
    public static bool CollapsingHeader(string key, ImGuiTreeNodeFlags flags = ImGuiTreeNodeFlags.None)
    {
        if (ImGuiNET.ImGui.CollapsingHeader(loc.GetString(key), flags))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Localized Checkbox.
    /// </summary>
    /// <param name="key">primary key.</param>
    /// <param name="value">value to set.</param>
    /// <returns>indicator whether checkbox changed.</returns>
    public static bool Checkbox(string key, ref bool value)
    {
        if (ImGuiNET.ImGui.Checkbox(loc.GetString(key), ref value))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Localized BeginPopup.
    /// </summary>
    /// <param name="key">primary key.</param>
    /// <returns>indicator whether popup is open.</returns>
    public static bool BeginPopup(string key)
    {
        if (ImGuiNET.ImGui.BeginPopup(loc.GetString(key)))
        {
            return true;
        }

        return false;
    }
}
