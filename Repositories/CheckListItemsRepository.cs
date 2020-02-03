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
    public class CheckListItemsRepository : ICheckListItemsRepository
    {
        private readonly IConfiguration _config;

        public CheckListItemsRepository(IConfiguration config)
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



        public async Task<List<CheckListItems>> GetByCheckListId(Int64 checklistid){
            using (IDbConnection conn = Connection)
            {
                string sQuery = "select CLITEMID,ITEMTITLE,ISCHECKED,CHECKLISTID,USERID from CHECKLISTITEMS = @ID";
                conn.Open();
                var result = await conn.QueryAsync<CheckListItems>(sQuery,new { ID=checklistid});
                return result.ToList();
            }
        }

        public async Task<CheckListItems> AddItem(CheckListItems item){

            using(IDbConnection conn=Connection){

                CheckListItems _item=new CheckListItems {
                    CLITEMID=-1,
                    ITEMTITLE=item.ITEMTITLE,
                    USERID=item.USERID,
                    CHECKLISTID=item.CHECKLISTID,
                    ISCHECKED=item.ISCHECKED
                };
                
                var _params=new DynamicParameters();
                _params.Add("@itemtitle",_item.ITEMTITLE);
                _params.Add("@userid",_item.USERID);
                _params.Add("@checklistid",_item.CHECKLISTID);
                _params.Add("@ischecked",_item.ISCHECKED);
                _params.Add(name:"@v_clitemid",value:_item.CLITEMID,dbType:DbType.Int64,
                 direction:ParameterDirection.InputOutput);

                string sQuery=@" begin ";
                sQuery+=" insert into CHECKLISTITEMS(ITEMTITLE,ISCHECKED,CHECKLISTID,USERID)";
                sQuery+=" values (@ITEMTITLE,@ISCHECKED,@CHECKLISTID,@USERID) ";
                sQuery+=" set @v_clitemid=SCOPE_IDENTITY()";
                sQuery+=" end ";
               //try{
                    var affectedRows=await conn.ExecuteAsync(sQuery,_params);
                        if(affectedRows>0){
                            Int64 newListId=_params.Get<Int64>("@v_clitemid");
                        _item.CLITEMID =newListId;
                    }

//                }catch(Exception ex){
  //                  return null;
    //            }
                
                return _item;

            }
        }
         public async Task<CheckListItems> UpdateItem(CheckListItems item){

            using(IDbConnection conn=Connection){

                CheckListItems _item=new CheckListItems {
                    CLITEMID=item.CLITEMID,
                    ITEMTITLE=item.ITEMTITLE,
                    USERID=item.USERID,
                    CHECKLISTID=item.CHECKLISTID,
                    ISCHECKED=item.ISCHECKED
                };
                
                var _params=new DynamicParameters();
                _params.Add("@itemtitle",_item.ITEMTITLE);
                _params.Add("@userid",_item.USERID);
                _params.Add("@checklistid",_item.CHECKLISTID);
                _params.Add("@ischecked",_item.ISCHECKED);
                _params.Add("@clitemid",_item.CLITEMID);

                string sQuery=@"update CHECKLISTITEMS set ";
                sQuery+=" ITEMTITLE=@itemtitle,";
                sQuery+=" ISCHECKED=@ischecked, ";
                sQuery+=" USERID=@USERID, ";
                sQuery+=" CHECKLISTID=@CHECKLISTID ";
                sQuery+=" where CLITEMID=@clitemid";
                var affectedRows=await conn.ExecuteAsync(sQuery,_params);
                if(affectedRows==0){
                    return null;
                }
                return _item;

            }
        }
        public async Task<Int64> DeleteItem(Int64 itemId){

            using(IDbConnection conn=Connection){

                var _params=new DynamicParameters();
                _params.Add("@v_itemid",itemId);

                string sQuery=@"delete from CHECKLISTITEMS where CLITEMID=@v_itemid";
                var affectedRows=await conn.ExecuteAsync(sQuery,_params);
                if(affectedRows==0){
                    return -1;
                }
                return 0;

            }
        }
    }
}
