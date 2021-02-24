using System;
using System.Collections.Generic;

namespace Pattern_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            //EXPO expo = new EXPO();
            //expo.AddMuseum(new ChinaMuseum());
            //expo.AddMuseum(new EnglandMuseum());

            //Visitor civilizedvisitor = new CivilizedVisitor();
            //Visitor uncivilizedvisitor = new UnCivilizedVisitor();

            //expo.Welcome(civilizedvisitor);

            //expo.Welcome(uncivilizedvisitor);
            //

            string exp = "四千五百万亿五千六百万零二十三";

            Context context = new Context(exp);
            List<Expression> list = new List<Expression>();
            list.Add(new GeExpression());
            list.Add(new ShiExpression());
            list.Add(new BaiExpression());
            list.Add(new QianExpression());
            list.Add(new WanExpression());
            list.Add(new YiExpression());

            foreach (var item in list)
            {
                item.Interpreter(context);
            }
            Console.WriteLine($"{exp}={context.Data}");

            Console.WriteLine("");
        }

        /// <summary>
        /// 游客
        /// </summary>
        public abstract class Visitor
        {
            /// <summary>
            /// 访问中国馆
            /// </summary>
            /// <param name="chinaMuseum"></param>
            public abstract void VisitChinaMuseum(ChinaMuseum chinaMuseum);

            /// <summary>
            /// 访问英国馆
            /// </summary>
            /// <param name="englandMuseum"></param>
            public abstract void VisitEnglandMuseum(EnglandMuseum englandMuseum);
        }

        /// <summary>
        /// 文明游客
        /// </summary>
        public class CivilizedVisitor : Visitor
        {
            public override void VisitChinaMuseum(ChinaMuseum chinaMuseum)
            {
                Console.WriteLine("看完舞蹈，鼓掌！"); ;
            }

            public override void VisitEnglandMuseum(EnglandMuseum englandMuseum)
            {
                Console.WriteLine("看完演奏，鼓掌！"); ;
            }
        }

        /// <summary>
        /// 不文明游客
        /// </summary>
        public class UnCivilizedVisitor : Visitor
        {
            public override void VisitChinaMuseum(ChinaMuseum chinaMuseum)
            {
                Console.WriteLine("在馆内喧哗"); ;
            }

            public override void VisitEnglandMuseum(EnglandMuseum englandMuseum)
            {
                Console.WriteLine("在馆内打电话"); ;
            }
        }


        /// <summary>
        /// 场馆
        /// </summary>
        public abstract class Museum
        {
            /// <summary>
            /// 接带游客
            /// </summary>
            /// <param name="visitor"></param>
            public abstract void Accept(Visitor visitor);
        }

        /// <summary>
        /// 中国馆
        /// </summary>
        public class ChinaMuseum : Museum
        {
            public override void Accept(Visitor visitor)
            {
                Dance();
                visitor.VisitChinaMuseum(this);
            }

            /// <summary>
            /// 舞蹈
            /// </summary>
            private void Dance()
            {
                Console.WriteLine("56个名族，56朵花");
            }
        }

        /// <summary>
        /// 英国馆
        /// </summary>
        public class EnglandMuseum : Museum
        {
            public override void Accept(Visitor visitor)
            {
                Play();
                visitor.VisitEnglandMuseum(this);
            }

            /// <summary>
            /// 舞蹈
            /// </summary>
            private void Play()
            {
                Console.WriteLine("演奏风笛");
            }
        }

        /// <summary>
        /// 世博会
        /// </summary>
        public class EXPO
        {
            private List<Museum> museums = new List<Museum>();

            /// <summary>
            /// 增加分馆
            /// </summary>
            /// <param name="museum"></param>
            public void AddMuseum(Museum museum )
            {
                museums.Add(museum);
            }

            /// <summary>
            /// 欢迎游客
            /// </summary>
            /// <param name="visitor"></param>
            public void Welcome(Visitor visitor)
            {
                foreach (var item in museums)
                {
                    item.Accept(visitor);
                }
            }
        }

    }
}
