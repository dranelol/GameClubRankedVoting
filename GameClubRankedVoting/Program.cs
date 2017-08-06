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
                printVotes();
                Console.ReadLine();
                RCV();
                rank++;
            }

            Console.WriteLine(gameListing[result]);

            Console.ReadLine();
        }

        static void printVotes()
        {
            int count = 0;
            foreach (int[] vote in votes)
            {
                Console.WriteLine("Voter: " + count);
                Console.WriteLine("1: " + vote[0]);
                Console.WriteLine("2: " + vote[1]);
                Console.WriteLine("3: " + vote[2]);

                count++;
            }
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
                // nobody got the majority,  reassign their votes and eliminate the lowest

                int lowest = int.MaxValue;

                int lowestIndex = 0;

                for(int i = 0; i < results.Length; i++)
                {
                    if(gameListing[i] != "-1")
                    {

                        if (results[i] < lowest)
                        {
                            lowest = results[i];
                            lowestIndex = i;
                        }
                    }
                }

                // loop through the votes
                //  if anyone voted the lowest candidate first, shift their second-place vote to first-place
                //  if anyone voted the lowest candidate second, shift their third-place vote to second-place

                
                foreach (int[] vote in votes)
                {
                    if(vote[0] == lowestIndex)
                    {
                        vote[0] = vote[1];
                    }

                    if (vote[1] == lowestIndex)
                    {
                        vote[1] = vote[2];
                    }
                }

                // finally, eliminate the lowest from the list
                gameListing[lowestIndex] = "-1";

            }
        }
    }
}
