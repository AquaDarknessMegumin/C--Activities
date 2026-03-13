using System.Collections.ObjectModel;
using System.Linq;
using ToDo.Models;

namespace ToDo.Services
{
    public static class ToDoService
    {
        private static ObservableCollection<ToDoClass> _allItems = new ObservableCollection<ToDoClass>();
        private static int _nextId = 1;

        public static void AddItem(ToDoClass item)
        {
            item.item_id = _nextId++;
            item.status = "Active";
            _allItems.Add(item);
        }

        public static void UpdateItem(ToDoClass item)
        {
            var existing = _allItems.FirstOrDefault(i => i.item_id == item.item_id);
            if (existing != null)
            {
                existing.item_name = item.item_name;
                existing.item_description = item.item_description;
                existing.status = item.status;
            }
        }

        public static ObservableCollection<ToDoClass> GetActiveItems(int userId)
        {
            var active = _allItems.Where(i => i.user_id == userId && i.status == "Active").ToList();
            return new ObservableCollection<ToDoClass>(active);
        }

        public static ObservableCollection<ToDoClass> GetCompletedItems(int userId)
        {
            var completed = _allItems.Where(i => i.user_id == userId && i.status == "Completed").ToList();
            return new ObservableCollection<ToDoClass>(completed);
        }

        public static ToDoClass GetItemById(int id)
        {
            return _allItems.FirstOrDefault(i => i.item_id == id);
        }
    }
}
