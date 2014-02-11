using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace football
{
    class Program
    {
        static void Main(string[] args)
        {
            var finder = new LuckiestTeamsFinder(@"..\..\App_Data\football.csv");

            var luckiestTeams = finder.Find();

            Console.WriteLine("The luckiest team(s) is/are : ");
            
            Console.WriteLine(string.Join(",", (from team in luckiestTeams
                                                select team)));

            Console.Read();
        }

    }
}
