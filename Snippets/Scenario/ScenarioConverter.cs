using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FileModifier
{

    class Program
    {

        static void Main(string[] args)
        {
            //Enable the line below to run without using the terminal
            ModifyAllFilesInSomeRandomDirectory(@"C:\production\converted");
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
                        bool bInStepsSection = false;
                        bool bInResultsSection = false;
                        char a = 'A';



                        while (null != (strCurrentLine = aReader.ReadLine()))
                        {

                            string strNewString;

                            // Modify lines
                            if (!bInStepsSection && !bInResultsSection)
                            {
                                if (strCurrentLine.Contains("Steps:") || strCurrentLine.Contains("Step:"))
                                {
                                    bInStepsSection = true;
                                    strNewString = Environment.NewLine + "|| Step:" + Environment.NewLine;
                                }
                                                              
                                else if (strCurrentLine.Contains("** P") || strCurrentLine.Contains("**P") || strCurrentLine.Contains("**F") || strCurrentLine.Contains("** F"))
                                {
                                    strNewString = "blank";
                                }

                                else if (strCurrentLine.Contains("�"))
                                {
                                    strNewString = strCurrentLine.Replace("�", "");
                                }
                               
                                else
                                {
                                    strNewString = strCurrentLine;
                                }
                            }

                            else if (bInStepsSection)
                            {
                                if (String.Empty == strCurrentLine)
                                {
                                    strNewString = "blank";
                                                                        
                                }
                                else if (strCurrentLine.Contains("** P") || strCurrentLine.Contains("**P") || strCurrentLine.Contains("**F") || strCurrentLine.Contains("** F"))
                                {
                                    strNewString = "blank";
                                }
                                
                                else if (strCurrentLine.Contains("Expected Outcome:") || strCurrentLine.Contains("Expected Outcomes:"))
                                {
                                    strNewString = Environment.NewLine + "|| Expected Outcome:" + Environment.NewLine;
                                    bInStepsSection = false;
                                    bInResultsSection = true;
                                    a = 'A';
                                }
                                else
                                {
                                    strNewString = strCurrentLine.Replace("�", "");
                                    strNewString = a + ". " + strNewString;
                                    a++;
                                }                                
                                
                            }
                            else if (bInResultsSection)
                            {
                                if (String.Empty == strCurrentLine)
                                {
                                    strNewString = "blank";
                                   //bInResultsSection = false;
                                    
                                }
                                else if (strCurrentLine.Contains("Step:") || strCurrentLine.Contains("Steps:"))
                                {
                                    strNewString = Environment.NewLine + Environment.NewLine + Environment.NewLine + "|| Step:" + Environment.NewLine;
                                    bInStepsSection = true;
                                    bInResultsSection = false;
                                    a = 'A';
                                }
                                    else if (strCurrentLine.Contains("** P") || strCurrentLine.Contains("**P") || strCurrentLine.Contains("**F") || strCurrentLine.Contains("** F"))
                                {
                                    strNewString = "blank";
                                }
                                else if (strCurrentLine.Contains("Trial") || (strCurrentLine.Contains("Scenario")) || (strCurrentLine.Contains("trial")) || (strCurrentLine.Contains("scenario")) || (strCurrentLine.Contains("option")) || (strCurrentLine.Contains("Option")))
                                {
                                    strNewString = Environment.NewLine + strCurrentLine + Environment.NewLine;
                                }
                                else
                                {
                                    strNewString = strCurrentLine.Replace("�", "");
                                    strNewString = a + ". " + strNewString;
                                    a++;
                                }      
                            }
                            else
                            {
                                strNewString = strCurrentLine; //(for testing: + "+++++NOT CHANGED+++++";)
                            }
                        


                            // Add them to the new file. If deleting a line, strNewString is set to blank otherwise replace with new text
                            if ("blank" == strNewString)
                            {
                                aWriter.Write(String.Empty);
                            }

                            else
                            {
                                aWriter.WriteLine(strNewString);
                            }

                        }
                    }
                }
            }
            Console.WriteLine("File Modified: " + strFullFilePath + " at: " + DateTime.Now);
            //Console.WriteLine("Scenarios Modified: " + nScenariosModified);
        }
    }
}

