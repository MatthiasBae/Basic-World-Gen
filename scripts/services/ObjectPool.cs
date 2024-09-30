
using System.Collections.Generic;

public class ObjectPool<T> where T : IPoolableObject {
    private int _Capacity = 50;
    
    public Stack<T> Pool = new ();
    
    public int Count {
        get {
            return this.Pool.Count;
        }
    }
    
    public ObjectPool(int capacity) {
        this._Capacity = capacity;
        this.Pool = new Stack<T>(capacity);
    }
    
    public bool TryRelease(out T item) {
        if (this.Pool.Count > 0) {
            item = this.Pool.Pop();
            return true;
        }
        
        item = default(T);
        return false;
    }
    
    //@ToFutureMe: Maybe on recycle we can execute a Reset method on the object which will be implemented by an interface
    //Interface like IPoolable which has method ResetToDefault, Destroy and Create
    public bool TryRecycle(T item) {
        if (this.Pool.Count >= this._Capacity) {
            return false;
        }
        
        item.Reset();
        this.Pool.Push(item);
        return true;
    }
}