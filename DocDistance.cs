using System;
using System.Collections.Generic;
using System.IO;

namespace DocumentDistance
{
    class DocDistance
    {
        private static Dictionary<string, double> doc1List;
        private static Dictionary<string, double> doc2List;
        /// <param name="doc1FilePath">File path of 1st document</param>
        /// <param name="doc2FilePath">File path of 2nd document</param>
        /// <returns>The angle (in degree) between the 2 documents</returns>
        public static double CalculateDistance(string doc1FilePath, string doc2FilePath)
        {
            doc1List = getFrequency(doc1FilePath);
            doc2List = getFrequency(doc2FilePath);

            double dotProduct = getDotProduct(doc1List, doc2List);
            double magnitudeOfDoc1 = getMagnitude(doc1List);
            double magnitudeOfDoc2 = getMagnitude(doc2List);

            double ceta =  dotProduct / Math.Sqrt( magnitudeOfDoc1 * magnitudeOfDoc2 ) ;
            double calcInRedian = Math.Acos(ceta);
            double calcInDegree = calcInRedian * 180 / Math.PI;
            return calcInDegree;
        }
        private static double getMagnitude(Dictionary<string, double> doc)
        {
            double summOfPower = 0;
            foreach (KeyValuePair<string, double> word in doc)
            {
                summOfPower += (word.Value * word.Value);
            }
            return summOfPower;
        }
        private static double getDotProduct(Dictionary<string, double> doc1List, Dictionary<string, double> doc2List)
        {
            double dotProuduct = 0;
            if (doc1List.Count <= doc2List.Count)
            {
                foreach (KeyValuePair<string, double> w in doc1List)
                {
                    if (doc1List.TryGetValue(w.Key, out double wordFreqInDoc1))
                    {
                        if (doc2List.TryGetValue(w.Key, out double wordFreqInDoc2))
                        {
                            dotProuduct += wordFreqInDoc1 * wordFreqInDoc2;
                        }
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<string, double> w in doc2List)
                {
                    if (doc2List.TryGetValue(w.Key, out double wordFreqInDoc2))
                    {
                        if (doc1List.TryGetValue(w.Key, out double wordFreqInDoc1))
                        {
                            dotProuduct += wordFreqInDoc1 * wordFreqInDoc2;
                        }
                    }
                }
            }
            return dotProuduct;
        }
        private static Dictionary<string, double> getFrequency(string docFilePath)
        {
            string docStr = File.ReadAllText(docFilePath).ToLower().Trim();
            Dictionary<string, double> wordCounters = new Dictionary<string, double>();


            string[] docWordsArray = docStr.Split(new char[] {' ', '.', '\n','\r', '\t', '\"', '#', '.', '*', '[', ']',
            ',', ':', '/', '~', '(', ')', '-', '_', '!', ';', '?', '@','$', '%', '^', '&',
            '{', '}','<', '>', '|', '`', '=', '+', '\'', '�', '\\'}, StringSplitOptions.RemoveEmptyEntries);

            foreach (string w in docWordsArray)
            {
                if (wordCounters.TryGetValue(w, out double val))
                {
                    val++;
                    wordCounters[w] = val;
                }
                else
                {
                    wordCounters.Add(w, 1);
                }
            }
            if (wordCounters.ContainsKey(""))
            {
                wordCounters.Remove("");
            }
            return wordCounters;
        }
    }
}