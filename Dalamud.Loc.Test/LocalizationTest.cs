﻿using System;
using System.Collections.Generic;

using Dalamud.Loc.Enums;
using Xunit;

namespace Dalamud.Loc.Test
{
    public class LocalizationTest
    {
        private const string SampleJson = "{\r\n  \"MyKey1\": \"Key Value\",\r\n  \"MyKey2\": \"Key Value 2\"\r\n}";

        private enum SampleEnum
        {
            MyKey1,
            MyKey2,
        }

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

        [Fact]
        public void ShouldLoadLanguages()
        {
            var loc = new Localization();
            loc.LoadLanguages(new List<Tuple<Language, string>> { new(Language.French, SampleJson) });
            loc.CurrentLanguage = Language.French;
            Assert.Equal("Key Value", loc.GetString("MyKey1"));
        }

        [Fact]
        public void ShouldUseFallBackIfEnabled()
        {
            var loc = new Localization();
            loc.LoadLanguages(new List<Tuple<Language, string>>
            {
                new(Language.English, SampleJson),
                new(Language.French, "{}"),
            });
            loc.CurrentLanguage = Language.French;
            Assert.Equal("Key Value", loc.GetString("MyKey1"));
        }

        [Fact]
        public void ShouldIgnoreFallBackIfDisabled()
        {
            var loc = new Localization();
            loc.LoadLanguages(new List<Tuple<Language, string>>
            {
                new(Language.English, SampleJson),
                new(Language.French, "{}"),
            });
            loc.CurrentLanguage = Language.French;
            loc.UseFallbacks = false;
            Assert.Equal("MyKey1", loc.GetString("MyKey1"));
        }

        [Fact]
        public void ShouldTranslateMultipleKeysAtOnce()
        {
            var loc = new Localization();
            loc.LoadLanguage(Language.French, SampleJson);
            loc.CurrentLanguage = Language.French;
            var keys = new[] { "MyKey1", "MyKey2" };
            var values = loc.GetStrings(keys);
            Assert.Equal("Key Value", values[0]);
            Assert.Equal("Key Value 2", values[1]);
        }

        [Fact]
        public void ShouldTranslateEnumsWithCurrentLanguage()
        {
            var loc = new Localization();
            loc.LoadLanguage(Language.English, SampleJson);
            var values = loc.GetStrings<SampleEnum>();
            Assert.Equal("Key Value", values[0]);
            Assert.Equal("Key Value 2", values[1]);
        }

        [Fact]
        public void ShouldTranslateEnumsWithSpecifiedLanguage()
        {
            var loc = new Localization();
            loc.LoadLanguages(new List<Tuple<Language, string>>
            {
                new(Language.English, SampleJson),
                new(Language.French, "{}"),
            });
            loc.CurrentLanguage = Language.French;
            var values = loc.GetStrings<SampleEnum>(Language.English);
            Assert.Equal("Key Value", values[0]);
            Assert.Equal("Key Value 2", values[1]);
        }
    }
}
