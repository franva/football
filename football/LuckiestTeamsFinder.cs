using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace football
{
    /// <summary>
    /// Use this class to find the target teams.
    /// Notice: In my code, I support the scenario when multiple teams have 
    ///         same smallest difference in "for" and "against" goals.
    /// </summary>
    public class LuckiestTeamsFinder
    {
        /// <summary>
        /// The aimed column which is the Total Number Of Goals Scored for.
        /// </summary>
        const string TotalNumberOfGoalsScoredFor = "F";

        /// <summary>
        /// The aimed column which is the Total Number of Goals Scored Against.
        /// </summary>
        const string TotalNumberOfGoalsScoredAgainst = "A";
        int rowCount = -1, aIndex = -1, fIndex = -1;
        int colIndex;
        int fValue = 0, aValue = 0;
        List<string> luckiestTeam = new List<string>();
        int smallestDifference = int.MaxValue;
        List<int> differences = new List<int>();

        string path;

        public LuckiestTeamsFinder(string filePath)
        {
            path = filePath;
        }

        /// <summary>
        /// The Find method provides the core function of this class which is to find the names of the teams
        /// with the smallest difference in "for" and "against" goals (I call them the luckiest teams in my way).
        /// </summary>
        /// <returns>A list of the luckiest teams.</returns>
        public IEnumerable<string> Find()
        {
            // 1. Open the csv file.
            TextFieldParser parser = new TextFieldParser(path);
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");

            // loop through lines of content.
            while (!parser.EndOfData)
            {
                ++rowCount;

                string[] fields = parser.ReadFields();
                colIndex = -1;

                // loop through columns
                foreach (string f in fields)
                {
                    ++colIndex;
                    var temp = f.Trim();

                    // check if it is running against header line.
                    if (rowCount == 0)
                    {
                        // if it is in header line then get the F column index and A column index.
                        if (string.Compare(temp, TotalNumberOfGoalsScoredFor, true) == 0)
                        {
                            fIndex = colIndex;
                        }
                        else
                            if (string.Compare(temp, TotalNumberOfGoalsScoredAgainst, true) == 0)
                            {
                                aIndex = colIndex;
                            }

                        continue;
                    }

                    // Validate the content of the current row.
                    bool isValid = validateRow(temp);
                    if (isValid == false)
                    {
                        continue;
                    }

                    // get the F value and A value when running against valid data rows.
                    if (colIndex == fIndex)
                    {
                        fValue = Convert.ToInt32(temp);
                    }
                    else
                        if (colIndex == aIndex)
                        {
                            aValue = Convert.ToInt32(temp);
                            break;
                        }

                }

                // Skip the header row.
                if (rowCount == 0)
                {
                    continue;
                }

                // Add the current smallest difference into an array for further use.
                int difference = Math.Abs(fValue - aValue);
                if (smallestDifference >= difference)
                {
                    smallestDifference = difference;
                    luckiestTeam.Add(fields[0].Split('.')[1].Trim());
                    differences.Add(difference);
                }

            }

            parser.Close();

            // This is where to find out multiple smallest difference teams.
            var smallestDifferenceInList = differences.Min();
            List<int> indexesOfSmallest = new List<int>();

            for (int i = 0; i < differences.Count; ++i)
            {
                if (differences[i] == smallestDifferenceInList)
                {
                    indexesOfSmallest.Add(i);

                    yield return luckiestTeam[i];
                }
            }
        }

        // TODO: we can define a set of rules(which can be a dictionary) which determine what content of a row is valid and what is not.
        /// <summary>
        /// The validateRow method is used to check whther the provided row has valid data.
        /// </summary>
        /// <param name="row">The row on which the validation will be performed.</param>
        /// <returns>True, if the row is valid.</returns>
        private static bool validateRow(string row)
        {
            if (string.IsNullOrWhiteSpace(row))
            {
                return false;
            }

            bool containDigits = row.Any(c => Char.IsLetter(c));
            bool containCharacters = row.Any(c => Char.IsDigit(c));

            if (containCharacters == false && containDigits == false)
            {
                return false;
            }

            return true;
        }
    }
}
