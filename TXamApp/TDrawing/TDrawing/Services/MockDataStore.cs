using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDrawing.Models;

namespace TDrawing.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        readonly List<Item> items;

        public MockDataStore()
        {
            items = new List<Item>()
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "Thiên nhiên", Description="Hoa quả, cây cối, rừng núi." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Động vật", Description="Vẽ các loại động vật." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Chân dung", Description="Vẽ người." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Truyện Tranh", Description="Vẽ nhân vật truyện tranh." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Vẽ Tranh Màu", Description="Vẽ màu." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Vẽ chibi", Description="Vẽ chibi." }
            };
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}