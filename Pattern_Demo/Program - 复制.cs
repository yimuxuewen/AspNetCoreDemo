using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace Pattern_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Classes<Student> classes = new Classes<Student>();

            Student student1 = new Student("张三");
            Student student2 = new Student("李四");
            Student student3 = new Student("王五");

            System.Collections.Generic.IEnumerator<Student> iterator = (classes as System.Collections.Generic.IEnumerable<Student>).GetEnumerator();
            classes.AddStudent(student1);
            classes.AddStudent(student2);
            classes.AddStudent(student3);

            while (iterator.MoveNext())
            {
                Student student = iterator.Current;
                Console.WriteLine(student);
            }


            Console.WriteLine("Hello World!");
        }

        /// <summary>
        /// 班级类
        /// </summary>
        public class Classes<T> : System.Collections.Generic.IEnumerable<T>
        {
            /// <summary>
            /// 学生集合
            /// </summary>
            private List<T> students = new List<T>();

            /// <summary>
            /// 集合中添加学生
            /// </summary>
            /// <param name="student"></param>
            public void AddStudent(T student)
            {
                students.Add(student);
            }


            //public IEnumerator<T> GetEnumerator()
            //{
            //    foreach (var student in students)
            //    {
            //        yield return student;
            //    };
            //}

            public IEnumerator<T> GetEnumerator()
            {
                return new ClassIterator<T>(this);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new ClassIterator<T>(this);
            }

            /// <summary>
            /// 获取当前集合的总数
            /// </summary>
            public int GetCounts { get { return students.Count; } }

            /// <summary>
            /// 返回当前索引的学生对象
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public T this[int index]
            {
                get { return students[index]; }
            }

        }

        /// <summary>
        /// 班级类迭代器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class ClassIterator<T> : System.Collections.Generic.IEnumerator<T>
        {
            /// <summary>
            /// 班级类
            /// </summary>
            private Classes<T> _classes = null;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="classes"></param>
            public ClassIterator(Classes<T> classes)
            {
                _classes = classes;
            }

            private int _currentindex =-1;


            T IEnumerator<T>.Current
            {
                get { return  this._classes[_currentindex]; }
            }

            public object Current
            {
                get { return this._classes[_currentindex]; }
            }

            public bool MoveNext()
            {
                _currentindex++;
                return _currentindex != this._classes.GetCounts;
            }

            public void Reset()
            {
                _currentindex=-1;
            }

            public void Dispose()
            {
                this.Dispose();
            }
        }



        /// <summary>
        /// 学生类
        /// </summary>
        public class Student
        {
            /// <summary>
            /// 学生姓名
            /// </summary>
            private string _name;

            /// <summary>
            /// 构造函数初始化
            /// </summary>
            /// <param name="Name"></param>
            public Student(string Name)
            {
                _name = Name;
            }

            public override string ToString()
            {
                return $"学生姓名：{_name}";
            }
        }
    }
}
