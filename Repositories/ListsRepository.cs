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
    public class ListsRepository : IListsRepository
    {
        private readonly IConfiguration _config;

        public ListsRepository(IConfiguration config)
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
            var userid_=userId;
            if(userid_<=0){
                userid_=1;
            }
            using (IDbConnection conn = Connection)
            {
                string sQuery = "SELECT LISTID, LISTTITLE ,USERID FROM LISTS WHERE USERID = @ID";
                conn.Open();
                var result = await conn.QueryAsync<Lists>(sQuery,new { ID=userid_});
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

            using(IDbConnection conn=Connection){

                Lists _list=new Lists {
                    LISTID=-1,
                    ListTitle=list.ListTitle,
                    USERID=list.USERID,
                    CARDDATA=new List<Cards>()
                };
                
                var _params=new DynamicParameters();
                _params.Add("@listtitle",_list.ListTitle);
                _params.Add("@userid",_list.USERID);
                _params.Add(name:"@v_listid",value:_list.LISTID,dbType:DbType.Int64,
                 direction:ParameterDirection.InputOutput);

                string sQuery=@" begin ";
                sQuery+=" insert into LISTS(LISTTITLE,USERID) values (@LISTTITLE,@USERID) ";
                sQuery+=" set @v_listid=SCOPE_IDENTITY()";
                sQuery+=" end ";
               //try{
                    var affectedRows=await conn.ExecuteAsync(sQuery,_params);
                        if(affectedRows>0){
                            Int64 newListId=_params.Get<Int64>("@v_listid");
                        _list.LISTID =newListId;
                    }

//                }catch(Exception ex){
  //                  return null;
    //            }
                
                return _list;

            }
        }
         public async Task<Lists> UpdateList(Lists list){

            using(IDbConnection conn=Connection){

                Lists _list=new Lists {
                    LISTID=list.LISTID,
                    ListTitle=list.ListTitle,
                    USERID=list.USERID,
                    CARDDATA=new List<Cards>()
                };
                
                var _params=new DynamicParameters();
                _params.Add("@v_listid",_list.LISTID);
                _params.Add("@v_listtitle",_list.ListTitle);

                string sQuery=@"update LISTS set ";
                sQuery+=" LISTTITLE=@v_listtitle ";
                sQuery+=" where LISTID=@v_listid";
                var affectedRows=await conn.ExecuteAsync(sQuery,_params);
                if(affectedRows==0){
                    return null;
                }
                return _list;

            }
        }
        public async Task<Int64> DeleteList(Int64 listid){

            using(IDbConnection conn=Connection){

                var _params=new DynamicParameters();
                _params.Add("@v_listid",listid);

                string sQuery=@"delete from LISTS where LISTID=@v_listid";
                var affectedRows=await conn.ExecuteAsync(sQuery,_params);
                if(affectedRows==0){
                    return -1;
                }
                return 0;

            }
        }
    }
}
