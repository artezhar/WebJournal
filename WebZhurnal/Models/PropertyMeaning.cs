using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebZhurnal.Models
{
    public static class PropertyMeaning
    {

        private static Dictionary<string, Dictionary<string, string>> _meanings;

        static PropertyMeaning()
        {
            _meanings = new Dictionary<string, Dictionary<string, string>>();
            _meanings.Add("ApplicationUser", new Dictionary<string, string>()
            {
                { "GroupId", "Номер группы" }
            });
        }

        public static Dictionary<string, string> OfClass<T>()
        {
            string name = typeof(T).GetType().Name;
            if (!_meanings.ContainsKey(name)) return null;
            return _meanings[name];
        }
    }
}
