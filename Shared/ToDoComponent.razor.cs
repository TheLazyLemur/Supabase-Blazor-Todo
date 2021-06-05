using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorToDoList.Data;
using Microsoft.AspNetCore.Components;

namespace BlazorToDoList.Shared
{
    public partial class ToDoComponent
    {
        [Parameter] public LoginContext LoginContext { get; set; }

        private List<ToDoItem> ToDoItems;

        private readonly ToDoItem _toDoItem = new();

        protected override async Task OnInitializedAsync()
        {
            await GetAllToDosAsync();
        }

        private async Task HandleValidSubmit()
        {
            await _toDoService.InsertToDoAsync(LoginContext, _toDoItem);
            await GetAllToDosAsync();
        }

        private async Task GetAllToDosAsync()
        {
            ToDoItems = await _toDoService.GetAllToDosAsync(LoginContext);
            await InvokeAsync(StateHasChanged);
        }

        private async Task MarkAsComplete(int id)
        {
            await _toDoService.MarkAsCompleteAsync(LoginContext, id);
            await GetAllToDosAsync();
        }
    }
}