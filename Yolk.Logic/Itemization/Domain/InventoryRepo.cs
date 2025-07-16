namespace Yolk;

using System;
using System.Collections.Generic;
using System.Linq;
using Chickensoft.Collections;
using Godot;


public interface IInventoryRepo : IDisposable {
  public IAutoProp<IEnumerable<IItem>> Items { get; }

  public event Action<IItem>? ItemAdded;
  public event Action<IItem>? ItemRemoved;

  public void AddItem(IItem item);
  public void RemoveItem(IItem item);
  public bool Contains(IItem item);
  public bool Contains(string name, int amount = 1);
  public IItem? Get(int index);
}

public class InventoryRepo : IInventoryRepo {
  private readonly AutoProp<IEnumerable<IItem>> _items = new([]);
  public IAutoProp<IEnumerable<IItem>> Items => _items;

  public event Action<IItem>? ItemAdded;
  public event Action<IItem>? ItemRemoved;

  public void AddItem(IItem item) {
    var modified = _items.Value.Append(item);
    _items.OnNext(modified);
    ItemAdded?.Invoke(item);
  }

  public void RemoveItem(IItem item) {
    var items = _items.Value.ToList();  // NOTE allocations allocations...
    items.Remove(item);
    _items.OnNext(items);
    ItemRemoved?.Invoke(item);
  }

  public bool Contains(IItem item) => _items.Value.Contains(item);
  public bool Contains(string name, int amount = 1) => _items.Value.Count(item => item.ItemName == name) >= amount;

  public IItem? Get(int index) => _items.Value.ElementAtOrDefault(index);


  public void Dispose() {
    ItemAdded = null;
    ItemRemoved = null;

    _items.OnCompleted();
    _items.Dispose();

    GC.SuppressFinalize(this);
  }

}
