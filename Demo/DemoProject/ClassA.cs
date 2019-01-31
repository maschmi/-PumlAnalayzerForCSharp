using System;
using System.Collections.Generic;

namespace TestProject
{
    public class ClassA
    {
        private readonly ClassB _stringClass;
        public int MyProperty { get; set; }
        public List<int> MyList {get; set;}

        public ClassA(int count)
        {
            MyProperty = count;
            _stringClass = new ClassB(count.ToString());
        }

        public void Increase()
        {
            MyProperty++;
            _stringClass.MyProperty = MyProperty.ToString();
            _stringClass.WriteToLog();
        }

        public void IncreaseList()
        {
            foreach(var entry in MyList)
            {
                DoNothing();
            }
            for(int i = 0; i < MyList.Count; i++)
            {
                MyList[i] = MyList[i]++;
                _stringClass.MyProperty = MyList[i].ToString();
                if (MyList.Count > 1)
                {
                    _stringClass.WriteList(MyList);
                }
                else
                    _stringClass.WriteToLog();                
            }
            DoNothing();
        }

        public string OnlyReturn(bool test)
        {
            if (_stringClass.ReturnStringValue(test) == "true")
                return _stringClass.ReturnStringValue(test);

            return false.ToString();
        }

        public void ConditionalIncrease(bool shallIGrow)
        {
            if (shallIGrow)
            { 
                Increase();
            }
            else if (!shallIGrow)
                DoTheOpposite();
            else
                DoNothing();
        }

        private void DoNothing()
        {
            throw new NotImplementedException();
        }

        private void DoTheOpposite()
        {
            _stringClass.CaseMethod(MyProperty);            
        }
    }
}