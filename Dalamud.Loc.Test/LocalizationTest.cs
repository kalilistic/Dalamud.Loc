using System;

using Dalamud.Loc.Enums;
using Xunit;

namespace Dalamud.Loc.Test
{
    public class LocalizationTest
    {
        private const string SampleJson = "{\r\n  \"MyKey1\": \"Key Value\",\r\n  \"MyKey2\": \"Key Value 2\"\r\n}";

        [Fact]
        public void ConstructorShouldNotThrowError()
        {
            var loc = new Localization();
            Assert.NotNull(loc);
        }

        [Fact]
        public void GetCurrentLanguageNotSetShouldBeEnglish()
        {
            var loc = new Localization();
            Assert.Equal(Language.English, loc.CurrentLanguage);
        }

        [Fact]
        public void SetCurrentLanguageShouldUpdate()
        {
            var loc = new Localization();
            loc.LoadLanguage(Language.French, string.Empty);
            loc.CurrentLanguage = Language.French;
            Assert.Equal(Language.French, loc.CurrentLanguage);
        }

        [Fact]
        public void SetCurrentLanguageShouldThrowExceptionIfNotLoaded()
        {
            var loc = new Localization();
            Assert.Throws<Exception>(() => loc.CurrentLanguage = Language.French);
        }

        [Fact]
        public void DisposeShouldNotThrowException()
        {
            var loc = new Localization();
            loc.Dispose();
        }

        [Fact]
        public void ShouldLoadLanguage()
        {
            var loc = new Localization();
            loc.LoadLanguage(Language.French, SampleJson);
            loc.CurrentLanguage = Language.French;
            Assert.Equal("Key Value", loc.GetString("MyKey1"));
        }
    }
}
