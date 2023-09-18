using System;
using System.Collections.Generic;

namespace Mushakushi.DataStructures.Runtime
{
    /// <summary>
    /// Min-heap queue priority queue
    /// </summary>
    public class MinPriorityQueue<T> where T : IComparable<T>
    {
        private readonly List<T> heap;
        
        /// <summary>
        /// Gets the number of elements contained in the <see cref="MinPriorityQueue{T}"/>
        /// </summary>
        public int Count => heap.Count;

        public MinPriorityQueue()
        {
            heap = new List<T>();
        }

        /// <summary>
        /// Adds item to the queue and heapifies
        /// </summary>
        public void Enqueue(T item)
        {
            heap.Add(item);
            Swim(heap.Count - 1);
        }
        
        /// <summary>
        /// Adds and heapifies multiple items to queue.
        /// </summary>
        public void Enqueue(IEnumerable<T> items)
        {
            foreach (var item in items) Enqueue(item);
        }

        /// <summary>
        /// Swaps item at index upwards until it is less than its parent or at the start of the min heap.
        /// </summary>
        private void Swim(int item)
        {
            int parent;
            while (item > 0 && heap[item].CompareTo(heap[parent = (item - 1) / 2]) < 0) // swim while node is not less than parent
                Swap(item, parent); // swap with parent
        }

        /// <summary>
        /// Removes first (min) item from heap and heapifies.
        /// </summary>
        public T Dequeue()
        {
            if (heap.Count == 0) throw new InvalidOperationException("Queue contains no elements");
            var item = heap[0]; // save current min

            // swap first (min) and last item, remove min
            var lastItem = heap.Count - 1;
            Swap(0, lastItem);
            heap.RemoveAt(lastItem);
            Sink(0); // the bottom item most likely does not preserve heap, sink it 

            return item;
        }

        /// <summary>
        /// Swaps item at index downwards until it is greater than its parent or at the end of the min heap.
        /// </summary>
        private void Sink(int item)
        {
            var lastItem = heap.Count - 1;
            int child;
            while ((child = item * 2 + 1) <= lastItem) // left child exists
            {
                if (child < lastItem && heap[child].CompareTo(heap[child + 1]) > 0) child++; // determine whether left or right child is lesser
                if (heap[item].CompareTo(heap[child]) <= 0) break; // current item is not greater than child, heap is preserved

                // sink 
                Swap(item, child);
                item = child;
            }
        }

        /// <summary>
        /// Swaps two elements in the heap by index. 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        private void Swap(int lhs, int rhs)
        {
            (heap[lhs], heap[rhs]) = (heap[rhs], heap[lhs]); 
        }

        /// <summary>
        /// Returns true if the queue contains any elements, false otherwise.
        /// </summary>
        public bool Any()
        {
            return heap.Count > 0;
        }

        /// <summary>
        /// Converts the heap to a string. 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return heap.Count == 0 ? "" : string.Join(", ", heap);
        }
    }
}