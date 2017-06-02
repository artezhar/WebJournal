using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebZhurnal.Models
{
    public class LogItem
    {
        public DateTime DateTime { get; set; }
        public virtual LogItemType Type { get; internal set; }
        internal string Field1 { get; set; }
        internal string Field2 { get; set; }
        internal string Field3 { get; set; }
        internal string Field4 { get; set; }

        public int Id { get; set; }

    }

    public enum LogItemType
    {
        Registration, Rate
    }

    public class RateItem : LogItem
    {
        public string Teacher
        {
            get { return Field2; }
            set { Field2 = value; }
        }

        public string Student
        {
            get { return Field3; }
            set { Field3 = value; }
        }
        public string Subject
        {
            get { return Field4; }
            set { Field4 = value; }
        }


        public int Rate
        {
            get
            {
                try
                {
                    return int.Parse(Field1);
                }
                catch
                {
                    return 0;
                }
            }

            set
            {
                Field1 = value.ToString();
            }

        }
    }
}
