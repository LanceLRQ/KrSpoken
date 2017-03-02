using System;
using System.Collections;
using System.Text;
using KrSpoken.PlayListLib;
using System.Data.OleDb;
using System.Data;

namespace KrSpoken.XLSFileReader
{
    public class XLSFileReader
    {
        private DataSet OleDsExcle = new DataSet();
        private KrPlayList Kr = new KrPlayList();
        public XLSFileReader(string _filePath)
        {
            try
            {
                string strConn;
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + _filePath + ";Extended Properties='Excel 8.0;HDR=False;IMEX=1'";
                OleDbConnection OleConn = new OleDbConnection(strConn);
                OleConn.Open();
                String sql = "SELECT * FROM  [Information$]";//可是更改Sheet名称，比如sheet2，等等   

                OleDbDataAdapter OleDaExcel = new OleDbDataAdapter(sql, OleConn);

                OleDaExcel.Fill(OleDsExcle, "Information");
                OleConn.Close();
            }
            catch (Exception err)
            {
                throw new Exception("数据绑定Excel失败!失败原因：" + err.Message);
            }  
        }
        public void changeAList(string _outfilePath,string Title)
        {
            try
            {
                Kr.ListTitle = Title;
                Kr.ListVersion = "KrSpokenListFileV1.0";
                for (int i = 0; i < OleDsExcle.Tables["Information"].Rows.Count; i++)
                {
                    object[] Data = new object[7];
                    if (i > 1)
                    {
                        Data[0] = Convert.ToInt32(OleDsExcle.Tables["Information"].Rows[i][1].ToString());  //ItemID
                        Data[1] = Convert.ToInt32(OleDsExcle.Tables["Information"].Rows[i][2].ToString());  //SubItemID
                        Data[2] = Convert.ToInt32(OleDsExcle.Tables["Information"].Rows[i][4].ToString()); //FileType
                        Data[3] = OleDsExcle.Tables["Information"].Rows[i][5].ToString(); //FilePath
                        Data[4] = Convert.ToInt32(OleDsExcle.Tables["Information"].Rows[i][6].ToString()); //PlayTime
                        Data[5] = Convert.ToInt32(OleDsExcle.Tables["Information"].Rows[i][7].ToString()); //PlayFlag
                        Data[6] = OleDsExcle.Tables["Information"].Rows[i][9].ToString(); //PlayHint
                    }
                    else
                    {
                        Data[0] = (Int32)0;  //ItemID
                        Data[1] = (Int32)0;  //SubItemID
                        Data[2] = Convert.ToInt32(OleDsExcle.Tables["Information"].Rows[i][4].ToString()); //FileType
                        Data[3] = OleDsExcle.Tables["Information"].Rows[i][5].ToString(); //FilePath
                        Data[4] = Convert.ToInt32(OleDsExcle.Tables["Information"].Rows[i][6].ToString()); //PlayTime
                        Data[5] = Convert.ToInt32(OleDsExcle.Tables["Information"].Rows[i][7].ToString()); //PlayFlag
                        Data[6] = OleDsExcle.Tables["Information"].Rows[i][9].ToString(); //PlayHint
                    }
                    Kr.MyList.Add(Convert.ToInt32(OleDsExcle.Tables["Information"].Rows[i][0].ToString()), Data); 
                }
                KrPlayListRunTime.SetPlayListToFile(_outfilePath,Kr);
            }
            catch(Exception e)
            {
                throw new Exception("处理过程发生严重问题："+e.Message);
            }
        }
    }
}
