/*
PDFToImage Converter

Copyright (c) 2025 aftamat4ik

Licensed under the MIT License. 
See LICENSE.txt in the project root for license information. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFToImage.Localisation
{
    public static class LocalizationStore
    {
        // this contains all the localisation words
        // add your languages when they are necessary, then change your culture in 'settings.ini' and that's it
        public static Dictionary<string, Dictionary<string, string>> Translations = new()
        {
            { "THREADS_NUMBER", new Dictionary<string, string>
                {
                    { "en-EN", "Threads Number:" },
                    { "de-DE", "Anzahl der Threads:" },
                    { "ru-RU", "Количество потоков:" }
                }
            },
            { "QUALITY", new Dictionary<string, string>
                {
                    { "en-EN", "Quality:" },
                    { "de-DE", "Qualität:" },
                    { "ru-RU", "Качество:" }
                }
            },
            { "FORMAT", new Dictionary<string, string>
                {
                    { "en-EN", "Format:" },
                    { "de-DE", "Format:" },
                    { "ru-RU", "Формат:" }
                }
            },
            { "LOSSLESS_COMPRESSION", new Dictionary<string, string>
                {
                    { "en-EN", "Lossless Compression" },
                    { "de-DE", "Verlustfreie Kompression" },
                    { "ru-RU", "Сжатие без потерь" }
                }
            },
            { "MAX_WIDTH", new Dictionary<string, string>
                {
                    { "en-EN", "Max Width:" },
                    { "de-DE", "Maximale Breite:" },
                    { "ru-RU", "Макс. ширина:" }
                }
            },
            { "MAX_HEIGHT", new Dictionary<string, string>
                {
                    { "en-EN", "Max Height:" },
                    { "de-DE", "Maximale Höhe:" },
                    { "ru-RU", "Макс. высота:" }
                }
            },
            { "PDF_PASSWORDS", new Dictionary<string, string>
                {
                    { "en-EN", "PDF Passwords" },
                    { "de-DE", "PDF-Passwörter" },
                    { "ru-RU", "Пароли PDF" }
                }
            },
            // buttons
            { "START_CONVERSION", new Dictionary<string, string>
                {
                    { "en-EN", "Convert" },
                    { "de-DE", "Konvertieren" },
                    { "ru-RU", "Конвертировать" }
                }
            },
            { "STOP_CONVERSION", new Dictionary<string, string>
                {
                    { "en-EN", "Stop" },
                    { "de-DE", "Stopp" },
                    { "ru-RU", "Остановить" }
                }
            },
            { "ADD_FILES", new Dictionary<string, string>
                {
                    { "en-EN", "Add Files" },
                    { "de-DE", "Hinzufügen" },
                    { "ru-RU", "Добавить" }
                }
            },
            { "CLEAR_FILES", new Dictionary<string, string>
                {
                    { "en-EN", "Clear" },
                    { "de-DE", "Leeren" },
                    { "ru-RU", "Очистить" }
                }
            },
            { "REMOVE_SELECTED_FILES", new Dictionary<string, string>
                {
                    { "en-EN", "Remove Selected" },
                    { "de-DE", "Auswahl entfernen" },
                    { "ru-RU", "Удалить выбранное" }
                }
            },
            { "OPEN_OUTPUT_FOLDER", new Dictionary<string, string>
                {
                    { "en-EN", "Open Output" },
                    { "de-DE", "Ausgabeordner öffnen" },
                    { "ru-RU", "Открыть папку" }
                }
            },
        };
    }

    public static class L
    {
        public static string CurrentCulture { get; private set; } = "en-EN";

        public static string Get(string key)
        {
            if (LocalizationStore.Translations.TryGetValue(key, out var dict) &&
                dict.TryGetValue(CurrentCulture, out var value))
            {
                return value;
            }

            return $"[{key.ToUpper()}]";
        }

        public static void SetCulture(string cultureCode)
        {
            CurrentCulture = cultureCode;
        }
    }
}
