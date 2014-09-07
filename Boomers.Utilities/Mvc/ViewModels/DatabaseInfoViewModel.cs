using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Boomers.Mvc.ViewModels
{
    public class DatabaseInfoViewModel
    {
        private string _databaseName;

        public string database_name
        {
            get { return _databaseName; }
            set { _databaseName = value; }
        }
        private string _databaseSize;

        public string database_size
        {
            get { return _databaseSize; }
            set { _databaseSize = value; }
        }
        private string _unallocatedSpace;

        public string UnallocatedSpace
        {
            get { return _unallocatedSpace; }
            set { _unallocatedSpace = value; }
        }
        private string _reserved;

        public string reserved
        {
            get { return _reserved; }
            set { _reserved = value; }
        }
        private string _data;

        public string data
        {
            get { return _data; }
            set { _data = value; }
        }
        private string _indexSize;

        public string index_size
        {
            get { return _indexSize; }
            set { _indexSize = value; }
        }
        private string _unused;

        public string unused
        {
            get { return _unused; }
            set { _unused = value; }
        }
        public string TotalRows { get; set; }

        private List<SqlTableInfoViewModel> _tables;

        public List<SqlTableInfoViewModel> Tables
        {
            get
            {
                if (_tables == null)
                {
                    return new List<SqlTableInfoViewModel>();
                }
                else
                {
                    return _tables;
                }
            }
            set { _tables = value; }
        }
    }


}