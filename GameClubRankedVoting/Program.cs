using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GameClubRankedVoting
{
    class Program
    {
        static List<string> gameListing = new List<string>();

        static List<int[]> votes = new List<int[]>();

        static Dictionary<int, int> results = new Dictionary<int, int>();

        static bool finished = false;

        static int result = 0;
        //bepis
        static void Main(string[] args)
        {
            //read in game listing

            StreamReader gameList = new StreamReader(@"..\\..\\currentGameChoices.txt");

            string line;
            while((line = gameList.ReadLine()) != null)
            {
                string[] split = line.Split(':');
                gameListing.Add(split[1]);
            }

            gameList.Close();

            //read in vote choices


            StreamReader voteList = new StreamReader(@"..\\..\\voteChoices.txt");
            
            while ((line = voteList.ReadLine()) != null)
            {
                string[] split = line.Split(' ');

                int[] vote = new int[3];

                string[] firstPlace = split[0].Split(':');
                string[] secondPlace = split[1].Split(':');
                string[] thirdPlace = split[2].Split(':');

                vote[0] = Convert.ToInt32(firstPlace[1]);
                vote[1] = Convert.ToInt32(secondPlace[1]);
                vote[2] = Convert.ToInt32(thirdPlace[1]);

                votes.Add(vote);
            }

            voteList.Close();

            //call looping RCV algorithm
            int rank = 0;
            while(finished == false)
            {
                RCV();

                rank++;
            }
            //print result
            Console.WriteLine(result);
            Console.ReadLine();
        }

        static void RCV()
        {
            int[] results = new int[gameListing.Count];

            float majorityFlt = ((float)gameListing.Count / 2.0f);

            int majority = (int)Math.Ceiling(majorityFlt);

            foreach(int[] vote in votes)
            {
                results[vote[0]]++;
                int check = results[vote[0]];
                
                
                if(check >= majority)
                {
                    finished = true;

                    result = vote[0];

                    return;
                }
            }

            if(finished == false)
            {
                foreach(int result in results)
                {
                    Console.WriteLine(result);
                }

                Console.ReadLine();
                // nobody got the majority, eliminate the lowest and reassign their votes


            }
        }
    }
}
