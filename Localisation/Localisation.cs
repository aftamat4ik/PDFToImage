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
            // some log entries
            { "WELCOME_BANNER", new Dictionary<string, string>
                {
                    { "en-EN", " ------- Welcome! ^-^ ----- " },
                    { "de-DE", " ------- Willkommen! ^-^ ----- " },
                    { "ru-RU", " ------- Добро пожаловать! ^-^ ----- " }
                }
            },
            { "LOG_OUTPUT_FOLDER_SET", new Dictionary<string, string>
                {
                    { "en-EN", "> Output folder set to: {0}" },
                    { "de-DE", "> Ausgabeordner festgelegt auf: {0}" },
                    { "ru-RU", "> Папка вывода установлена на: {0}" }
                }
            },
            { "LOADED_CULTURE", new Dictionary<string, string>
                {
                    { "en-EN", "> Loaded culture: {0}" },
                    { "de-DE", "> Geladene Sprache: {0}" },
                    { "ru-RU", "> Загружена локализация: {0}" }
                }
            },
            { "SUPPORT_THE_AUTHOR", new Dictionary<string, string>
                {
                    { "en-EN", "> Support the author, he is poor man: {0}" },
                    { "de-DE", "> Unterstützen Sie den Autor: {0}" },
                    { "ru-RU", "> Поддержите автора тушенкой, автор практически бомж: {0}" }
                }
            },
            { "LOG_CONVERTED_FILES", new Dictionary<string, string>
                {
                    { "en-EN", "> Converted {0} files to {1}{2}" },
                    { "de-DE", "> {0} Dateien konvertiert in {1}{2}" },
                    { "ru-RU", "> Конвертировано файлов: {0} в формат {1}{2}" }
                }
            },
            { "LOG_OUTPUT_DIR", new Dictionary<string, string>
                {
                    { "en-EN", "> Files can be found in Output directory:\n> {0}" },
                    { "de-DE", "> Dateien befinden sich im Ausgabeordner:\n> {0}" },
                    { "ru-RU", "> Файлы находятся в папке вывода:\n> {0}" }
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
