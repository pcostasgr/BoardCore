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
    public class CardsRepository : ICardsRepository
    {
        private readonly IConfiguration _config;

        public CardsRepository(IConfiguration config)
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

        public async Task<Cards> GetByID(Int64 cardId)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = "SELECT CARDID,CARDTITLE,CARDDESCR,LISTID,USERID,CARDDATE FROM CARDS WHERE CARDID = @ID";
                conn.Open();
                var result = await conn.QueryAsync<Cards>(sQuery, new { ID = cardId });
                return result.FirstOrDefault();
            }
        }

        public async Task<List<Cards>> GetByListId(Int64 listId){
            using (IDbConnection conn = Connection)
            {
                string sQuery = "SELECT CARDID,CARDTITLE,CARDDESCR,LISTID,USERID,CARDDATE FROM CARDS WHERE LISTID = @ID";
                conn.Open();
                var result = await conn.QueryAsync<Cards>(sQuery,new { ID=listId});
                return result.ToList();
            }
        }

       
        public async Task<Cards> AddCard(Cards card){

            using(IDbConnection conn=Connection){

               Cards _card=new Cards {
                    CARDID=-1,
                    CARDTITLE=card.CARDTITLE,
                    CARDDESCR=card.CARDDESCR,
                    LISTID=card.LISTID,
                    USERID=card.USERID,
                    CARDDATE=card.CARDDATE
                };
                
                
                var _params=new DynamicParameters();
                _params.Add("@v_cardtitle",_card.CARDTITLE);
                _params.Add("@v_carddescr",_card.CARDDESCR);
                _params.Add("@v_listid",_card.LISTID);
                _params.Add("@v_userid",_card.USERID);
                _params.Add("@v_carddate",_card.CARDDATE);
                _params.Add(name:"@v_cardid",value:_card.CARDID,dbType:DbType.Int64,
                 direction:ParameterDirection.InputOutput);

                string sQuery=@" begin ";
                sQuery+=" insert into CARDS(CARDTITLE,CARDDESCR,LISTID,USERID,CARDDATE) ";
                sQuery+="values (@v_cardtitle,@v_carddescr,@v_listid,@v_userid,@v_carddate) ";
                sQuery+=" set @v_cardid=SCOPE_IDENTITY()";
                sQuery+=" end ";
                var affectedRows=await conn.ExecuteAsync(sQuery,_params);
                if(affectedRows>0){
                    var newListId=_params.Get<Int64>("@v_cardid");
                    _card.CARDID =newListId;
                }
                return _card;

            }
        }
         public async Task<Cards> UpdateCard(Cards card){

            using(IDbConnection conn=Connection){

                Cards _card=new Cards {
                    CARDID=card.CARDID,
                    CARDTITLE=card.CARDTITLE,
                    CARDDESCR=card.CARDDESCR,
                    LISTID=card.LISTID,
                    CARDDATE=card.CARDDATE
                };
                
                var _params=new DynamicParameters();
                _params.Add("@v_cardid",_card.CARDID);
                _params.Add("@v_cardtitle",_card.CARDTITLE);
                _params.Add("@v_carddescr",_card.CARDDESCR);
                _params.Add("@v_listid",_card.LISTID);
                _params.Add("@v_carddate",_card.CARDDATE);

                string sQuery=@"update CARDS set ";
                sQuery+=" CARDTITLE=@v_cardtitle, ";
                sQuery+=" CARDDESCR=@v_carddescr, ";
                sQuery+=" CARDDATE=@v_carddate, ";
                sQuery+=" LiSTID=@v_listid ";
                sQuery+=" where CARDID=@v_cardid";
                var affectedRows=await conn.ExecuteAsync(sQuery,_params);
                if(affectedRows==0){
                    return null;
                }
                return _card;

            }
        }
        public async Task<Int64> DeleteCard(Int64 cardId){

            using(IDbConnection conn=Connection){

                var _params=new DynamicParameters();
                _params.Add("@v_cardid",cardId);

                string sQuery=@"delete from CARDS where CARDID=@v_cardid";
                var affectedRows=await conn.ExecuteAsync(sQuery,_params);
                if(affectedRows==0){
                    return -1;
                }
                return 0;

            }
        }
    }
}
