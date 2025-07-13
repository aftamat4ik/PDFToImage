/*
PDFToImage Converter

Copyright (c) 2025 aftamat4ik

Licensed under the MIT License. 
See LICENSE.txt in the project root for license information. */

using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;

namespace PDFToImage.Models
{
    /// <summary>
    /// simpliest .ini writer-reader
    /// </summary>
    public class SettingsManager : IDisposable
    {
        private readonly string _settingsFilePath;
        private Dictionary<string, string> _settings;
        private bool _disposed;

        public SettingsManager()
        {
            _settingsFilePath = Path.Combine(Helpers.GetBaseDirectory(), "settings.ini");
            _settings = new Dictionary<string, string>();
            _disposed = false;
            LoadSettings();
        }

        // Gets given setting value
        public string GetSetting(string key, string defaultValue = "")
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(SettingsManager));

            return _settings.ContainsKey(key) ? _settings[key] : defaultValue;
        }

        // Sets setting value
        public void SetSetting(string key, string value)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(SettingsManager));

            _settings[key] = value;
            SaveSettings();
        }

        // Load all settings from _settingsFilePath
        private void LoadSettings()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(SettingsManager));

            try
            {
                if (!File.Exists(_settingsFilePath)) // settings not exist
                {
                    SetSetting("", ""); // write empty setting for file to appear
                }
                foreach (string line in File.ReadAllLines(_settingsFilePath))
                {
                    if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith(";") && !line.StartsWith("#"))
                    {
                        int separatorIndex = line.IndexOf('=');
                        if (separatorIndex > 0)
                        {
                            string key = line.Substring(0, separatorIndex).Trim();
                            string value = line.Substring(separatorIndex + 1).Trim();
                            _settings[key] = value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading settings: {ex.Message}");
            }
        }

        // Save settings to file
        private void SaveSettings()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(SettingsManager));

            try
            {
                List<string> lines = new List<string>
            {
                "; Settings file",
                "; aftamat4ik",
                $"; Last modified: {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                ""
            };

                foreach (var setting in _settings)
                {
                    lines.Add($"{setting.Key}={setting.Value}");
                }

                File.WriteAllLines(_settingsFilePath, lines);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving settings: {ex.Message}");
            }
        }

        // Dispose pattern
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources
                    _settings?.Clear();
                    _settings = null!;
                }

                // No unmanaged resources to dispose
                _disposed = true;
            }
        }

        ~SettingsManager()
        {
            Dispose(false);
        }
    }
}