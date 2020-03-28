using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 拖动批量复制
{
    class CopyClass
    {
        FileStream FormerOpen;
        FileStream ToFileOpen;
        /// <summary>
        /// 文件的复制
        /// </summary>
        /// <param FormerFile="string">源文件路径</param>
        /// <param toFile="string">目的文件路径</param> 
        /// <param SectSize="int">传输大小</param> 
        /// <param progressBar="ProgressBar">ProgressBar控件</param> 
        public void CopyFile(string FormerFile, string toFile, int SectSize)
        {
            FileStream fileToCreate = new FileStream(toFile, FileMode.Create);  //创建目的文件，如果已存在将被覆盖
            fileToCreate.Close();          //关闭所有资源
            fileToCreate.Dispose();          //释放所有资源
            FormerOpen = new FileStream(FormerFile, FileMode.Open, FileAccess.Read);//以只读方式打开源文件
            ToFileOpen = new FileStream(toFile, FileMode.Append, FileAccess.Write); //以写方式打开目的文件
            //根据一次传输的大小，计算传输的个数
            //int max = Convert.ToInt32(Math.Ceiling((double)FormerOpen.Length / (double)SectSize));

            int FileSize;            //要拷贝的文件的大小
            //如果分段拷贝，即每次拷贝内容小于文件总长度
            if (SectSize < FormerOpen.Length)
            {
                byte[] buffer = new byte[SectSize];       //根据传输的大小，定义一个字节数组
                int copied = 0;          //记录传输的大小
                while (copied <= ((int)FormerOpen.Length - SectSize))   //拷贝主体部分
                {
                    FileSize = FormerOpen.Read(buffer, 0, SectSize);   //从0开始读，每次最大读SectSize
                    FormerOpen.Flush();        //清空缓存
                    ToFileOpen.Write(buffer, 0, SectSize);     //向目的文件写入字节
                    ToFileOpen.Flush();         //清空缓存
                    ToFileOpen.Position = FormerOpen.Position;    //使源文件和目的文件流的位置相同
                    copied += FileSize;         //记录已拷贝的大小
                }
                int left = (int)FormerOpen.Length - copied;      //获取剩余大小
                FileSize = FormerOpen.Read(buffer, 0, left);     //读取剩余的字节
                FormerOpen.Flush();         //清空缓存
                ToFileOpen.Write(buffer, 0, left);       //写入剩余的部分
                ToFileOpen.Flush();         //清空缓存
            }
            //如果整体拷贝，即每次拷贝内容大于文件总长度
            else
            {
                byte[] buffer = new byte[FormerOpen.Length];    //获取文件的大小
                FormerOpen.Read(buffer, 0, (int)FormerOpen.Length);   //读取源文件的字节
                FormerOpen.Flush();         //清空缓存
                ToFileOpen.Write(buffer, 0, (int)FormerOpen.Length);   //写放字节
                ToFileOpen.Flush();         //清空缓存
            }
            FormerOpen.Close();          //释放所有资源
            ToFileOpen.Close();          //释放所有资源
        }
    }
}
