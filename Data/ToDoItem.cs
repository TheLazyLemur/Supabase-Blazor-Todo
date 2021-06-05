using System.Text.Json.Serialization;

namespace BlazorToDoList.Data
{
    public class ToDoItem
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        public string Task { get; set; }
        [JsonPropertyName("is_complete")]
        public bool IsComplete { get; set; }
    }
}