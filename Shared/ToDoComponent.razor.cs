using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorToDoList.Data;
using Microsoft.AspNetCore.Components;

namespace BlazorToDoList.Shared
{
    public partial class ToDoComponent
    {
        [Parameter] public LoginContext LoginContext { get; set; }

        private List<ToDoItem> _toDoItems;

        private readonly ToDoItem _toDoItem = new();

        protected override async Task OnInitializedAsync()
        {
            await GetAllToDosAsync();
        }

        private async Task HandleValidSubmit()
        {
            await _toDoService.InsertToDoAsync(_toDoItem);
            await GetAllToDosAsync();
        }

        private async Task GetAllToDosAsync()
        {
            _toDoItems = await _toDoService.GetAllToDosAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task MarkAsComplete(int id)
        {
            await _toDoService.MarkAsCompleteAsync(id);
            await GetAllToDosAsync();
        }
    }
}