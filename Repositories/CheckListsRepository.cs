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
    public class CheckListsRepository : ICheckListsRepository
    {
        private readonly IConfiguration _config;

        public CheckListsRepository(IConfiguration config)
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

        public async Task<List<CheckLists>> GetByCardId(Int64 cardid)
        {
            using (IDbConnection conn = Connection)
            {
                var listDictionary=new Dictionary<Int64,CheckLists>();

                string sQuery = "SELECT C.TITLE,C.USERID,C.CARDID,C.CHECKLISTID";
                sQuery+=" ,I.CLITEMID,I.ITEMTITLE,I.USERID,I.ISCHECKED,I.CHECKLISTID ";
                sQuery+=" FROM CHECKLISTS AS C LEFT OUTER JOIN CHECKLISTITEMS AS I ON C.CHECKLISTID=I.CHECKLISTID";
                sQuery+=" WHERE C.CARDID = @ID;";
                conn.Open();
                var result = await conn.QueryAsync<CheckLists,CheckListItems,CheckLists>(
                    sQuery,
                    map:(list,items) => {
                        CheckLists _list;

                        if(!listDictionary.TryGetValue(list.CHECKLISTID,out _list)){
                            _list=list;
                            _list.ITEMS=new List<CheckListItems>();
                            listDictionary.Add(_list.CHECKLISTID,_list);
                        }

                        _list.ITEMS.Add(items);
                        return _list;
                    },
                    splitOn:"CLITEMID",
                    param:new { @ID =cardid });
                    
                    return result
                    .Distinct()
                    .ToList();
               
            }
        }

        

        public async Task<CheckLists> AddCheckList(CheckLists list){

            using(IDbConnection conn=Connection){

                CheckLists _list=new CheckLists {
                    CHECKLISTID=-1,
                    TITLE=list.TITLE,
                    USERID=list.USERID,
                    CARDID=list.CARDID,
                    ITEMS=new List<CheckListItems>()
                };
                
                var _params=new DynamicParameters();
                _params.Add("@title",_list.TITLE);
                _params.Add("@userid",_list.USERID);
                _params.Add("@cardid",_list.CARDID);
                _params.Add(name:"@v_checklistid",value:_list.CHECKLISTID,dbType:DbType.Int64,
                 direction:ParameterDirection.InputOutput);

                string sQuery=@" begin ";
                sQuery+=" insert into CHECKLISTS(TITLE,USERID,CARDID) values (@title,@userid,@cardid) ";
                sQuery+=" set @v_checklistid=SCOPE_IDENTITY()";
                sQuery+=" end ";
               //try{
                    var affectedRows=await conn.ExecuteAsync(sQuery,_params);
                        if(affectedRows>0){
                            Int64 newListId=_params.Get<Int64>("@v_checklistid");
                        _list.CHECKLISTID =newListId;
                    }

//                }catch(Exception ex){
  //                  return null;
    //            }
                
                return _list;

            }
        }
         public async Task<CheckLists> UpdateCheckList(CheckLists list){

            using(IDbConnection conn=Connection){

                CheckLists _list=new CheckLists {
                    CHECKLISTID=list.CHECKLISTID,
                    TITLE=list.TITLE,
                    USERID=list.USERID,
                    CARDID=list.CARDID
                };
                
                var _params=new DynamicParameters();
                _params.Add("@title",_list.TITLE);
                _params.Add("@userid",_list.USERID);
                _params.Add("@cardid",_list.CARDID);
                _params.Add("@checklistid",_list.CHECKLISTID);

                string sQuery=@"update CHECKLISTS set ";
                sQuery+=" TITLE=@title, ";
                sQuery+=" CARDID=@CARDID, ";
                sQuery+=" USERID=@USERID ";
                sQuery+=" where CHECKLISTID=@checklistid";
                var affectedRows=await conn.ExecuteAsync(sQuery,_params);
                if(affectedRows==0){
                    return null;
                }
                return _list;

            }
        }
        public async Task<Int64> DeleteCheckList(Int64 checklistid){

            using(IDbConnection conn=Connection){

                var _params=new DynamicParameters();
                _params.Add("@v_listid",checklistid);

                string sQuery=@"delete from CHECKLISTS where CHECKLISTID=@v_listid";
                var affectedRows=await conn.ExecuteAsync(sQuery,_params);
                if(affectedRows==0){
                    return -1;
                }
                return 0;

            }
        }
    }
}
