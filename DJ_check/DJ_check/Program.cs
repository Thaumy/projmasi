﻿using System;
using System.IO;


namespace DJ_check
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string pathA = null;
                while (pathA == null || pathA == "")
                {
                    Console.WriteLine("键入文件夹物理路径A(自动分析DNG/JPG目录):");
                    pathA = Console.ReadLine();
                }

                string pathB = null;
                while (pathB == null || pathB == "")
                {
                    Console.WriteLine("键入文件夹物理路径B(自动分析DNG/JPG目录):");
                    pathB = Console.ReadLine();
                }

                try
                {
                    var sep = new Sparator(pathA, pathB);
                    var (_, jpgFolder) = sep.JPG();
                    var (dngPath, dngFolder) = sep.DNG();

                    int count = 0;//应删除计数

                    //建立存放被删除dng文件的should_be_deleted文件夹
                    Directory.CreateDirectory($@"{dngPath}\should_be_deleted");

                    foreach (FileInfo dngFile in dngFolder.GetFiles())
                    {
                        var dngFileName = dngFile.Name.Substring(0, dngFile.Name.LastIndexOf('.'));

                        //如果在jpg文件夹搜索到的指定文件个数为0
                        if (jpgFolder.GetFiles($"{dngFileName}*").Length == 0)
                        {
                            Console.WriteLine(dngFile.Name);//打印该dng的名字

                            //将dng文件移动到dng文件夹下的should_be_deleted文件夹
                            dngFile.MoveTo($@"{dngPath}\should_be_deleted\{dngFile.Name}");

                            //计数加一
                            count++;
                        }
                    }

                    Console.WriteLine($"统计完成，应删除的 dng 文件个数为:{count}");
                    Console.ReadKey();
                }
                catch
                {
                    Console.WriteLine("发生未知错误。");
                }
            }
        }
    }
}
