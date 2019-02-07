using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace queue
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;

            List<int> tasksList = new List<int>();

            List<Server> servers = new List<Server> {
                new Server { name = "1" },
                new Server { name = "2" },
                new Server { name = "3" },
             };

            while (true)
            {
                line = Console.ReadLine();

                try
                {
                    tasksList = line.Split(',').Select(int.Parse).ToList();
                    tasksList = tasksList.OrderByDescending(item => item).ToList();
                }
                catch { Console.WriteLine("Введенная строка не соотвествует формату -  1,2,3,4 "); }

                //распределяем
                SetTasks(tasksList, servers);

                //выводим на экран
                Print(servers);
            }

        }

        private static void SetTasks(List<int> tasksList, List<Server> servers)
        {
            Server lazyserver = new Server();

            int min;
            for (int i = 0; i < tasksList.Count(); i++) //раздаем задачи
            {
                lazyserver = null;
                min = int.MaxValue;

                for (int j = 0; j < servers.Count; j++) //ищем самый незагруженный сервер
                {

                    if (servers[j].GetWorkTime() < min)
                    {
                        min = servers[j].GetWorkTime(); lazyserver = servers[j];
                    }

                }

                lazyserver.AddTask(tasksList[i]); //даем задачу
            }
        }

        private static void Print(List<Server> servers)
        {
            string serverTasks = "";

            foreach (var server in servers)
            {
                serverTasks = "";

                foreach (var taskTime in server.Tasks)
                {
                    serverTasks += "|" + new string('_', taskTime) + "|";
                }

                Console.WriteLine(server.name + ":" + serverTasks);

            }
        }
    }

    public class Server {
        public string name;

        public List<int> Tasks { get; set; }

        public void AddTask(int task)
        {
            Tasks.Add(task);
        }
  
        public int GetWorkTime() {
            int time = 0;

            foreach (var task in Tasks) {
                time += task;
            }
            return time;
        }

        public Server()
        {
            Tasks = new List<int>();
        }

    }

}
