using System;
using System.Collections.Generic;
using System.Linq;
using MagicChunks.Core;
using MagicChunks.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MagicChunks.Documents
{
    public class JsonDocument : IDocument
    {
        protected readonly JObject Document;

        public JsonDocument(string source)
        {
            try
            {
                Document = (JObject)JsonConvert.DeserializeObject(source);
            }
            catch (JsonReaderException ex)
            {
                throw new ArgumentException("Wrong document format", nameof(source), ex);
            }
        }

        public void ReplaceKey(string[] path, string value)
        {
            if ((path == null) || (path.Any() == false))
                throw new ArgumentException("Path is not speicified.", nameof(path));

            if (path.Any(String.IsNullOrWhiteSpace))
                throw new ArgumentException("There is empty items in the path.", nameof(path));

            JObject current = (JObject)Document.Root;

            if (current == null)
                throw new ArgumentException("Root element is not present.", nameof(path));

            current = FindPath(path.Take(path.Length - 1), current);

            UpdateTargetElement(current, path.Last(), value);
        }

        public void RemoveKey(string[] path)
        {
            if ((path == null) || (path.Any() == false))
                throw new ArgumentException("Path is not speicified.", nameof(path));

            if (path.Any(String.IsNullOrWhiteSpace))
                throw new ArgumentException("There is empty items in the path.", nameof(path));

            JObject current = (JObject)Document.Root;

            if (current == null)
                throw new ArgumentException("Root element is not present.", nameof(path));

            current = FindPath(path.Take(path.Length - 1), current);
            current.Remove(path.Last());
        }

        private static JObject FindPath(IEnumerable<string> path, JObject current)
        {
            foreach (string pathElement in path)
            {
                var element = current.GetChildPropertyValue(pathElement);
                if (element is JObject)
                {
                    current = (JObject) element;
                }
                else
                {
                    current[pathElement] = new JObject();
                    current = (JObject) current[pathElement];
                }
            }
            return current;
        }

        private static void UpdateTargetElement(JObject current, string targetElementName, string value)
        {
            var targetElement = current.GetChildProperty(targetElementName);
            if (targetElement != null)
                targetElement.Value = value;
            else
                current.Add(targetElementName, value);
        }

        public override string ToString()
        {
            return Document?.ToString() ?? String.Empty;
        }

        public void Dispose()
        {
        }
    }
}