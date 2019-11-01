using System;
using System.Collections.Generic;
using System.Linq;

namespace assignment1.structures
{
	public class MinHeap<T> where T : IComparable
	{
		// Holds the elements in the heap using a vector based implementation
		private List<T> _heapVector;

		// Default constructor
		public MinHeap()
		{
			// Initializes a new list for the vector
			_heapVector = new List<T>();

			// Add empty element
			_heapVector.Add(default(T));
		}

		// Parameterized constructor to take in an initial sequence
		public MinHeap(List<T> sequence)
		{
			// Clones the list
			var usableSequence = sequence.Select(elem => elem).ToList();

			// Initializes a new list for the vector
			_heapVector = new List<T>();

			// Add first element of the list
			_heapVector.Add(default(T)); // Blank because first is unused
			_heapVector.Add(usableSequence[0]);
			usableSequence.RemoveAt(0);

			// Add the rest of the elements and heapify for each
			foreach (var element in usableSequence)
			{
				_heapVector.Add(element);
				_heapifyUp();
			}
		}

		public void Add(T element)
		{
			// Add the element
			_heapVector.Add(element);
			// Heapify up
			_heapifyUp();
		}

		public T RemoveMin()
		{
			// If the heap is empty, return an empty element
			if (_heapVector.Count() < 2)
			{
				return default(T);
			}
			// Assign a reference to the top element, the min
			var element = _heapVector[1];
			// If the heap only has one element, remove it
			if (_heapVector.Count() == 2)
			{
				_heapVector.RemoveAt(1);
			}
			else
			{
				// Bring the last element to the top
				_heapVector[1] = _heapVector.Last();
				// Remove the reference to the last element
				_heapVector.RemoveAt(_heapVector.Count() - 1);
				// Heapify down
				_heapifyDown();
			}
			return element;
		}

		// Simply print the elements in order
		public void Print()
		{
			foreach (var element in _heapVector)
			{
				Console.Write($"{element} ");

			}
		}

		// Return whether or not the heap is empty
		public bool isEmpty()
		{
			return _heapVector.Count() == 1;
		}

		// Restore heap order by heapifying up
		private void _heapifyUp()
		{
			// Get the initial index of the inserted element
			var i = _heapVector.Count() - 1;

			// Procede with heapify up
			while (i > 1 && _heapVector[i / 2].CompareTo(_heapVector[i]) > 0)
			{
				var temp = _heapVector[i];
				_heapVector[i] = _heapVector[i / 2];
				_heapVector[i / 2] = temp;
				i = i / 2;
			}
		}

		// Restore heap order by heapifying down
		private void _heapifyDown()
		{
			var i = 1;
			while (i < _heapVector.Count())
			{
				// Check for two children
				if ((2 * i) + 1 < _heapVector.Count())
				{
					if (_heapVector[(2 * i) + 1].CompareTo(_heapVector[i]) > 0 && _heapVector[2 * i].CompareTo(_heapVector[i]) > 0)
					{
						// We have restored order
						return;
					}
					// Let j be the smaller of the two children
					int j;
					if (_heapVector[(2 * i) + 1].CompareTo(_heapVector[2 * i]) <= 0)
					{
						j = (2 * i) + 1;
					}
					else
					{
						j = 2 * i;
					}
					var temp = _heapVector[i];
					_heapVector[i] = _heapVector[j];
					_heapVector[j] = temp;
					i = j;

				}
				// Check for one child
				else if (2 * i < _heapVector.Count())
				{
					int j = 2 * i;
					if (_heapVector[j].CompareTo(_heapVector[i]) > 0)
					{
						// We have restored order
						return;
					}

					var temp = _heapVector[i];
					_heapVector[i] = _heapVector[j];
					_heapVector[j] = temp;
					i = j;
				}
				else
				{
					// No children, return
					return;
				}
			}
		}
	}
}
