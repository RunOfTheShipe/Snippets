using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snippets.Scenario
{
    public class ScenarioConverter
    {
        /// <summary>
        /// Static function that is used to parse the command line arguments to the program and execute the converstion.
        /// </summary>
        /// <param name="args"></param>
        public static void RunWithArgs(string[] args)
        {
            //ModifyAllFilesInSomeRandomDirectory(@"B:\Production\Production Scenarios");
            if (0 < args.Length)
            {
                ModifyAllFilesInSomeRandomDirectory(args[0]);
            }
        }

        static void ModifyAllFilesInSomeRandomDirectory(string strDirectoryPath)
        {
            if (Directory.Exists(strDirectoryPath))
            {
                foreach (var strFile in Directory.GetFiles(strDirectoryPath))
                {
                    var aFileInfo = new FileInfo(strFile);
                    if (".txt" == aFileInfo.Extension)
                    {
                        ModifyFile(aFileInfo.FullName);
                    }
                }
            }
        }

        static void ModifyFile(string strFullFilePath)
        {
            string strNewFileName = strFullFilePath + "_Better.txt";

            if (File.Exists(strNewFileName))
                File.Delete(strNewFileName);

            if (File.Exists(strFullFilePath))
            {
                using (var aWriter = new StreamWriter(strNewFileName))
                {
                    using (var aReader = new StreamReader(strFullFilePath))
                    {
                        string strCurrentLine = null;
                        bool bInTheSpecialRegion = false;
                        bool bAfterTheSpecialRegion = false;
                        char a = 'A';



                        while (null != (strCurrentLine = aReader.ReadLine()))
                        {

                            string strNewString;

                            // Modify lines
                            if (!bInTheSpecialRegion)
                            {
                                if (strCurrentLine.Contains("Steps:"))
                                {
                                    bInTheSpecialRegion = true;
                                    strNewString = strCurrentLine.Replace("Steps:", "|| Step:");
                                    //strNewString = strCurrentLine.Replace("Step:", "|| Step:");
                                }

                                else if (strCurrentLine.Contains("Step:"))
                                {
                                    bInTheSpecialRegion = true;
                                    strNewString = strCurrentLine.Replace("Step:", "|| Step:");
                                }

                                else if (bAfterTheSpecialRegion)
                                {
                                    strNewString = strCurrentLine.Replace("Expected Outcome:", "|| Expected Outcome:");
                                    a = 'A';
                                    bAfterTheSpecialRegion = false;
                                }

                                else
                                {
                                    strNewString = strCurrentLine;
                                }
                            }

                            else if (bInTheSpecialRegion) // Andrew's happy time!
                            {
                                if (String.Empty == strCurrentLine)
                                {
                                    strNewString = String.Empty;
                                    bInTheSpecialRegion = false;
                                    bAfterTheSpecialRegion = true;
                                }
                                else
                                {
                                    strNewString = a + ". " + strCurrentLine;
                                    a++;
                                }
                            }
                            else
                            {
                                strNewString = strCurrentLine + "***";
                            }


                            // Add them to the new file.
                            aWriter.WriteLine(strNewString);

                        }


                    }
                }
            }
            Console.WriteLine("File Modified: " + strFullFilePath + " at: " + DateTime.Now);
            //Console.WriteLine("Scenarios Modified: " + nScenariosModified);
        }
    }
}

