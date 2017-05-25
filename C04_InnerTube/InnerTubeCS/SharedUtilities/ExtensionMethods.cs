using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SharedUtilities
{
    public static class ExtensionMethods
    {
        public static Collection<T> ToCollection<T>(this IEnumerable<T> source)
        {
            Collection<T> result = new Collection<T>();

            foreach (var item in source)
            {
                result.Add(item);
            }
            return result; 
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            ObservableCollection<T> result = new ObservableCollection<T>();

            foreach (var item in source)
            {
                result.Add(item);
            }
            return result;
        }

    }
}
