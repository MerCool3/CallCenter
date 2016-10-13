using System;
using System.Threading;
using System.Threading.Tasks;

namespace CallCenter
{
    class Program
    {
        static void Main(string[] args)
        {
            CallCenter call = new CallCenter();
            Task com = null;

            Console.WriteLine("\n*****New Day At Office*****\n");

            for (int i = 0; i < 20; i++)
            {
                com = new Task(call.Running);
                com.Start();
                Thread.Sleep(new Random().Next(1000, 3000));
            }

            Task.WaitAll(com);
            call.Statistics();
            Console.Read();
        }
    }

    class CallCenter
    {
        public Worker[] cc;

        public CallCenter()
        {
            cc = new Worker[6] {new Oper("Bill", 6), new TeamLead("Carl", 3), new Manager("God", 1),
                                    new Oper("Jane", 4), new TeamLead("Norma", 2), new Oper("Bob", 5)};
        }

        public void Running()
        {
            while (true)
            {
                foreach(Worker work in cc)
                {
                    if (work is Oper && !work.IsBusy)
                    {
                        work.TakeACall(new User());
                        return;
                    }
                    else if (work is TeamLead && FirstCheck() && !work.IsBusy)
                    {
                        work.TakeACall(new User());
                        return;
                    }
                    else if (work is Manager && FirstCheck() && SecondCheck() && !work.IsBusy)
                    {
                        work.TakeACall(new User());
                        return;
                    }
                }
            }
        }

        bool FirstCheck()
        {
            foreach(Worker wor in cc)
            {
                if (wor is Oper && !wor.IsBusy) return false;
            }
            return true;
        }

        bool SecondCheck()
        {
            foreach (Worker wor in cc)
            {
                if (wor is TeamLead && !wor.IsBusy) return false;
            }
            return true;
        }

        public void Statistics()
        {
            Console.WriteLine("\n*****Day Results*****\n");
            Console.WriteLine("Name\tScore");
            foreach (Worker wor in cc)
            {
                Console.WriteLine("{0}\t{1}", wor.Name, wor.Score);
            }
        }
    }

    class Worker
    {
        protected int speed;

        public Worker(string name, int id)
        {
            Name = name;
            Id = id;
            IsBusy = false;
        }

        public string Name { get; protected set; }
        public int Id { get; protected set; }
        public int Score { get; private set; }
        public bool IsBusy { get; private set; }

        public void TakeACall(User user)
        {
            IsBusy = true;
            Console.WriteLine(this.ToString() + " taking a Call from User №" + user.Id);
            Score++;
            Thread.Sleep(speed);
            Console.WriteLine(this.ToString() + " now is free");
            IsBusy = false;
        }

        public override string ToString()
        {
            return Name + " ID = " + Id;
        }
    }

    class Oper : Worker
    {
        public Oper(string name, int id) : base(name, id)
        {
            Name = name;
            Id = id;
            speed = new Random().Next(8000, 10000);
        }
    }

    class TeamLead : Worker
    {
        public TeamLead(string name, int id) : base(name, id)
        {
            Name = name;
            Id = id;
            speed = new Random().Next(5000, 8000);
        }
    }

    class Manager : Worker
    {
        public Manager(string name, int id) : base(name, id)
        {
            Name = name;
            Id = id;
            speed = new Random().Next(2000, 5000);
        }
    }

    class User
    {
        public User()
        {
            lock (new object()) GlobalId++;
            Id = GlobalId;
        }

        public static int GlobalId { get; private set; }
        public int Id { get; private set; }

        public void ToChat()
        {
            Console.WriteLine("User {0} is Chatting", Id);
        }
    }
}
