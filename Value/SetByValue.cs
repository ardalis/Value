// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="SetByValue.cs">
// //     Copyright 2016
// //           Thomas PIERRAIN (@tpierrain)    
// //     Licensed under the Apache License, Version 2.0 (the "License");
// //     you may not use this file except in compliance with the License.
// //     You may obtain a copy of the License at
// //         http://www.apache.org/licenses/LICENSE-2.0
// //     Unless required by applicable law or agreed to in writing, software
// //     distributed under the License is distributed on an "AS IS" BASIS,
// //     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// //     See the License for the specific language governing permissions and
// //     limitations under the License.b 
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------
namespace Value
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    ///     A Set with equality based on its content and not on the Set's reference 
    ///     (i.e.: 2 different instances containing the same items will be equals whatever their storage order).
    /// </summary>
    /// <remarks>This type is not thread-safe (for hashcode updates).</remarks>
    /// <typeparam name="T">Type of the listed items.</typeparam>
    public class SetByValue<T> : ISet<T>
    {
        private readonly ISet<T> hashSet;

        private int? hashCode;

        public SetByValue(ISet<T> hashSet)
        {
            this.hashSet = hashSet;
        }

        public SetByValue() : this(new HashSet<T>())
        {
        }

        public int Count => this.hashSet.Count;

        public bool IsReadOnly => this.hashSet.IsReadOnly;

        public void Add(T item)
        {
            this.ResetHashCode();
            this.hashSet.Add(item);
        }

        public void Clear()
        {
            this.ResetHashCode();
            this.hashSet.Clear();
        }

        public bool Contains(T item)
        {
            return this.hashSet.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.hashSet.CopyTo(array, arrayIndex);
        }

        public override bool Equals(object obj)
        {
            var other = obj as SetByValue<T>;
            if (other == null)
            {
                return false;
            }

            return this.hashSet.SetEquals(other);
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            this.ResetHashCode();
            this.hashSet.ExceptWith(other);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.hashSet.GetEnumerator();
        }

        public override int GetHashCode()
        {
            if (this.hashCode == null)
            {
                var code = 0;

                // Two instances with same elements added in different order must return the same hashcode
                // Let's compute and sort hashcodes of all elements (always in the same order)
                var sortedHashCodes = new SortedSet<int>();
                foreach (var element in this.hashSet)
                {
                    sortedHashCodes.Add(element.GetHashCode());
                }

                foreach (var elementHashCode in sortedHashCodes)
                {
                    code = (code * 397) ^ elementHashCode;
                }

                // Cache the result in a field
                this.hashCode = code;
            }

            return this.hashCode.Value;
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            this.ResetHashCode();
            this.hashSet.IntersectWith(other);
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return this.hashSet.IsProperSubsetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return this.hashSet.IsProperSupersetOf(other);
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return this.hashSet.IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return this.hashSet.IsSupersetOf(other);
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            return this.hashSet.Overlaps(other);
        }

        public bool Remove(T item)
        {
            this.ResetHashCode();
            return this.hashSet.Remove(item);
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            return this.hashSet.SetEquals(other);
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            this.ResetHashCode();
            this.hashSet.SymmetricExceptWith(other);
        }

        public void UnionWith(IEnumerable<T> other)
        {
            this.ResetHashCode();
            this.hashSet.UnionWith(other);
        }

        bool ISet<T>.Add(T item)
        {
            this.ResetHashCode();
            return this.hashSet.Add(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.hashSet).GetEnumerator();
        }

        protected virtual void ResetHashCode()
        {
            this.hashCode = null;
        }
    }
}