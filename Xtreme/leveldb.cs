using LevelDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xtreme
{
    public class LevelDBHelper
    {
       

        private LevelDB.DB levelDB;
        private string dbName;
        //private static readonly LevelDBHelper _instance = new LevelDBHelper();
        public  LevelDBHelper(string dbpath)
        {
            dbName = dbpath;
            Options options = new Options()
            {
                CreateIfMissing = true,
            };
            this.levelDB = LevelDB.DB.Open(this.dbName, options);
        }
        public bool Exists(string k)
        {
            return levelDB.TryGet(new ReadOptions() { FillCache = true }, k, out _);
        }
        
        public void WriteData(string key, string value)
        {
            this.levelDB.Put(new WriteOptions(), key, value);
        }
        //删除值
        public void DelDataWithKey(string key)
        {
            this.levelDB.Delete(new WriteOptions() { Sync = true }, key);
        }
        //获取值
        public string GetValueWithKey(string key)
        {
            var val = this.levelDB.Get(new ReadOptions(), key).ToString();
            return val == null ? null : val;
        }
        //统计数量和大小
        public void getDataSize(ref long dataCount, ref long dataSize)
        {
            dataCount = 0;
            dataSize = 0;
            Iterator iterator = this.levelDB.NewIterator(new ReadOptions());
            iterator.SeekToFirst();
            while (iterator.Valid())
            {
                ++dataCount;
                byte[] valBytes = Convert.FromBase64String(iterator.Value().ToString());
                dataSize += valBytes.LongLength;
                iterator.Next();
            }
            iterator.Dispose();
        }
        //使用完数据库以后Dispose
        public void closeDB()
        {
            this.levelDB.Dispose();
        }

    }
}
