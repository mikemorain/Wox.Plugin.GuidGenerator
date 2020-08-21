using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Wox.Plugin.GuidGenerator
{
    public class Main : IPlugin
    {
        private static string _pluginDirectory;
        private static PluginInitContext _context;
        public List<Result> Query(Query query)
        {
            var results = new List<Result>();
            var guid = Guid.NewGuid();
            // see if there is a format parameter
            var formatString = query.Search;

            // Default formatting
            if (string.IsNullOrWhiteSpace(formatString))
            {
                results.Add(new Result
                {
                    Title = guid.ToString(),
                    SubTitle = "Copy this GUID with hyphens to the clipboard",
                    IcoPath = $"{_pluginDirectory}\\images\\id.jpg",
                    Action = c =>
                    {
                        try
                        {
                            Clipboard.SetText(guid.ToString());
                            return true;
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                    },
                    ContextData = guid
                });

                return results;
            }
            else 
            // this block has specified a format parameter; acceptable ones are: n, d, b, p
            // Documentation here: https://docs.microsoft.com/en-us/dotnet/api/system.guid.tostring?view=netframework-4.5.2
            {
                // TODO: Validate the character
                if (formatString.Length != 1 || formatString.ToLower().IndexOfAny("ndbpx".ToCharArray()) == -1)
                {
                    results.Add(new Result
                    {
                        Title = "Please use a valid format parameter",
                        SubTitle = "Valid format parameters: N (nothing), D (hyphens), B (hyphens and braces), P (hyphens and parens)",
                        IcoPath = $"{_pluginDirectory}\\images\\id.jpg",
                        Action = c => false,
                        ContextData = guid
                    });
                }
                else // valid format param, lets format it
                {
                    results.Add(new Result
                    {
                        Title = guid.ToString(formatString.ToUpper()),
                        SubTitle = "Copy this GUID with hyphens to the clipboard",
                        IcoPath = $"{_pluginDirectory}\\images\\id.jpg",
                        Action = c =>
                        {
                            try
                            {
                                Clipboard.SetText(guid.ToString(formatString.ToUpper()));
                                return true;
                            }
                            catch (Exception)
                            {
                                return false;
                            }
                        },
                        ContextData = guid
                    });
                }

                return results;
            }
        }

        public void Init(PluginInitContext context)
        {
            _context = context;
            _pluginDirectory = context.CurrentPluginMetadata.PluginDirectory;
        }
    }
}
