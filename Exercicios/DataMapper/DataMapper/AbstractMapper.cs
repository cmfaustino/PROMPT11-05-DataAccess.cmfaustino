using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common; // adicionado para DB
using System.Data.SqlClient; // adicionado para DB

namespace DataMapper
{
    public class AbstractMapper<T>
    {
        private String _connectionString = "";
        private DbConnection _connection = null;

        private DbConnection SqlConnection
        {
            get { return _connection ?? (_connection = new SqlConnection(_connectionString)); }
        }

        public void Create(T)
        {
            ;
        }

        public T Read()
        {
            ;
        }

        public void Update(T)
        {
            ;
        }

        public void Delete(T)
        {
            ;
        }

        public void GetAll(T)
        {
            ;
        }
    }
}
