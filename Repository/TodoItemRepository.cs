namespace TODOCore.Repository
{
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using System.IO;

    public class TodoItemRepository : ITodoItemRepository
    {
        public List<TodoItem> list;
        private readonly string filePath;

        public TodoItemRepository(string filePath)
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }

            string json = File.ReadAllText(filePath);
            list =json.Length!=0? JsonConvert.DeserializeObject<List<TodoItem>>(json): new List<TodoItem>();
            this.filePath = filePath;
        }

        public IEnumerable<TodoItem> GetAll()
        {
            return list;
        }

        public TodoItem GetById(int id)
        {
            return list.FirstOrDefault(x => x.Id.Equals(id));
        }

        public void Delete(TodoItem modifiedItem)
        {
            list.Remove(modifiedItem);
        }

        public void Save(TodoItem modifiedItem)
        {
            var exisitingItem = GetById(modifiedItem.Id);

            if (exisitingItem is null)
            {
                list.Add(modifiedItem);
            }
            else
            {
                exisitingItem.IsCompleted = modifiedItem.IsCompleted;
                exisitingItem.Description = modifiedItem.Description;
            }
        }

        public void Persist()
        {
            string json = JsonConvert.SerializeObject(list);
            File.WriteAllText(filePath, json);
        }
    }
}