using Dapper;
using BoardCore.Models;
using BoardCore.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BoardCore.Repositories
{
    public class ListsRepositoryMock : IListsRepository
    {
        private readonly IConfiguration _config;

        public ListsRepositoryMock(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("MyConnectionString"));
            }
        }

        public async Task<Lists> GetByID(Int64 listId)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = "SELECT LISTID, LISTTITLE,USERID FROM LISTS WHERE LISTID = @ID";
                conn.Open();
                var result = await conn.QueryAsync<Lists>(sQuery, new { ID = listId });
                return result.FirstOrDefault();
            }
        }

        public async Task<List<Lists>> GetByUserId(Int64 userId){
            using (IDbConnection conn = Connection)
            {
                string sQuery = "SELECT LISTID, LISTTITLE,USERID FROM LISTS WHERE USERID = @ID";
                conn.Open();
                var result = await conn.QueryAsync<Lists>(sQuery,new { ID=userId});
                return result.ToList();
            }
        }

        public async Task<List<Lists>> GetAll()
        {
            using(IDbConnection conn=Connection) 
            {
                string sQuery="SELECT LISTID,LISTTITLE FROM LISTS ";
                conn.Open();
                var result=await Connection.QueryAsync<Lists>(sQuery);
                return result.ToList();
            }
            
        }

        public async Task<Lists> AddList(Lists list){

                Lists _list=new Lists {
                    LISTID=1000,
                    LISTTITLE=list.LISTTITLE + " MOCK INSERT",
                    USERID=list.USERID
                };
                
                return await Task.FromResult(_list);

        }
         public async Task<Lists> UpdateList(Lists list){

                Lists _list=new Lists {
                    LISTID=-list.LISTID,
                    LISTTITLE=list.LISTTITLE + " MOCK UPDATE",
                    USERID=list.USERID
                };
               
               if(_list.LISTID<=0){
                   return  await Task.FromResult<Lists>(null);
               }
               return await Task.FromResult<Lists>(null);
        }

        public async Task<Int64> DeleteList(Int64 listid){
            if(listid<=0){
                   return  await Task.FromResult(-1);
               }

            return await Task.FromResult(0);
        }
    }
}
