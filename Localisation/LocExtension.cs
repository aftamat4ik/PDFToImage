using Avalonia.Markup.Xaml;
using PDFToImage.Localisation;
using System;
using System.Collections.Generic;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Data.Converters;
using System;

namespace PDFToImage.Localization
{
    /// <summary>
    /// This is Avalonia .axaml extension to connect with localisation class and get entries from it directly
    /// </summary>
    public class LocExtension : MarkupExtension
    {
        public string Key { get; set; } = string.Empty;

        public LocExtension() { }

        public LocExtension(string key)
        {
            Key = key;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return L.Get(Key ?? string.Empty);
        }
    }
}