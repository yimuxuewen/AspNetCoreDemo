using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pattern_Demo
{
    public class Context
    {
        private string _statement;
        private Int64 _data;

        public Context(string statement)
        {
            _statement = statement;
        }
        public string Statement { get { return _statement; } set { _statement = value; } }

        public Int64 Data { get {return _data; } set { _data = value; } }
    }


    public abstract class Expression
    {
        protected Dictionary<string, int> NumTable = new Dictionary<string, int>();

        public Expression()
        {
            NumTable.Add("零", 0);
            NumTable.Add("一", 1);
            NumTable.Add("二", 2);
            NumTable.Add("三", 3);
            NumTable.Add("四", 4);
            NumTable.Add("五", 5);
            NumTable.Add("六", 6);
            NumTable.Add("七", 7);
            NumTable.Add("八", 8);
            NumTable.Add("九", 9);
        }

        public virtual void Interpreter(Context context)
        {
            if (context.Statement.Length==0)
            {
                return;
            }
            foreach (string key in NumTable.Keys)
            {
                int value = NumTable[key];
                if (context.Statement.EndsWith(key+GetPostfix()))
                {
                    context.Data += value * Multiplier();
                    context.Statement = context.Statement.Substring(0, context.Statement.Length - GetLength());
                }
                if (context.Statement.EndsWith("零"))
                {
                    context.Statement = context.Statement.Substring(0, context.Statement.Length - 1);
                }
            }
        }

        protected abstract string GetPostfix();

        protected abstract int Multiplier();
        protected virtual int GetLength()
        {
           return GetPostfix().Length + 1;
        }

    }


    /// <summary>
    /// 个位数
    /// </summary>
    public class GeExpression : Expression
    {
        protected override string GetPostfix()
        {
            return "";
        }

        protected override int Multiplier()
        {
            return 1;
        }

        protected override int GetLength()
        {
            return 1;
        }
    }


    /// <summary>
    /// 十位数
    /// </summary>
    public class ShiExpression : Expression
    {
        protected override string GetPostfix()
        {
            return "十";
        }

        protected override int Multiplier()
        {
            return 10;
        }

    }

    /// <summary>
    /// 百位数
    /// </summary>
    public class BaiExpression : Expression
    {
        protected override string GetPostfix()
        {
            return "百";
        }

        protected override int Multiplier()
        {
            return 100;
        }

    }


    /// <summary>
    /// 千位数
    /// </summary>
    public class QianExpression : Expression
    {
        protected override string GetPostfix()
        {
            return "千";
        }

        protected override int Multiplier()
        {
            return 1000;
        }

    }


    /// <summary>
    /// 万位数
    /// </summary>
    public class WanExpression : Expression
    {
        protected override string GetPostfix()
        {
            return "万";
        }

        public override void Interpreter(Context context)
        {
            if (context.Statement.Length==0)
            {
                return;
            }
            ArrayList list = new ArrayList();
            list.Add(new GeExpression());
            list.Add(new ShiExpression());
            list.Add(new BaiExpression());
            list.Add(new QianExpression());
            foreach (string key in NumTable.Keys)
            {
                if (context.Statement.EndsWith(GetPostfix()))
                {
                    Int64 tmp = context.Data;
                    context.Data = 0;

                    context.Statement = context.Statement.Substring(0, context.Statement.Length -1);
                    foreach (Expression expression in list)
                    {
                        expression.Interpreter(context);
                    }
                    context.Data = tmp + context.Data * Multiplier();
                }
            }

        }

        protected override int Multiplier()
        {
            return 10000;
        }

    }

    /// <summary>
    /// 亿位数
    /// </summary>
    public class YiExpression : Expression
    {
        protected override string GetPostfix()
        {
            return "亿";
        }

        public override void Interpreter(Context context)
        {
            if (context.Statement.Length == 0)
            {
                return;
            }
            ArrayList list = new ArrayList();
            list.Add(new GeExpression());
            list.Add(new ShiExpression());
            list.Add(new BaiExpression());
            list.Add(new QianExpression());
            list.Add(new WanExpression());
            foreach (string key in NumTable.Keys)
            {
                if (context.Statement.EndsWith(GetPostfix()))
                {
                    Int64 tmp = context.Data;
                    context.Data = 0;

                    context.Statement = context.Statement.Substring(0, context.Statement.Length - 1);
                    foreach (Expression expression in list)
                    {
                        expression.Interpreter(context);
                    }
                    context.Data = tmp + context.Data * Multiplier();
                }
            }

        }

        protected override int Multiplier()
        {
            return 10000;
        }

    }

}
