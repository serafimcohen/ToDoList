using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ToDoList.Models
{
    public partial class Task : ObservableObject
    {
        public long Id { get; set; }

        [ObservableProperty]
        public string title = string.Empty;

        [ObservableProperty]
        public string description = string.Empty;
        public bool IsCompleted { get; set; }
    }
}
