using System.Collections.Generic;

namespace ChromeManagedBookmarksEditor.Models.Serializers
{
    // TODO: stop being lazy and use the enum with a converter
    public class SerializationOutputs
    {
        public List<string> AvailableTypes { get; } = new List<string>()
        {
            "json",
            "html"
        };
    }
}
