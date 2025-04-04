﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository;
using SSRepository.IRepository.Master;
using SSRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Repository
{
    public class GridLayoutRepository : Repository<TblGridStructer>, IGridLayoutRepository
    {
        public GridLayoutRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
            
        }
        public TblGridStructer GetSingleRecord( long FkFormId,string GridName, List<ColumnStructure> columns)
        {
            bool flag = true;
            long FkUserId = GetUserID();
            TblGridStructer data = new TblGridStructer();
            var _entity = __dbContext.TblGridStructer
                .Where(cou => cou.FkUserId == FkUserId && cou.FkFormId == FkFormId 
                && (!string.IsNullOrEmpty(cou.GridName) ? cou.GridName : "") == (!string.IsNullOrEmpty(GridName) ? GridName : "")).FirstOrDefault();
            if (_entity == null)
            {
formbase:
                _entity = __dbContext.TblGridStructer.Where(cou => cou.FkFormId == FkFormId && cou.GridName == GridName).FirstOrDefault();
                if (_entity == null)
                {
                    if (!flag)
                    {
                        throw new Exception("Not Found");
                    }
                    flag = false;
                    object dd = new GridStructerModel()
                    {
                        PkGridId = 0,
                        FkFormId = FkFormId,
                        FkUserId = FkUserId,
                        GridName = GridName,
                        JsonData = JsonConvert.SerializeObject(columns)
                    };
                    long ID = 0;
                    string res = CreateAsync(dd, "Create", ID, "").Result;
                    goto formbase;

                }
            }

            data = new TblGridStructer()
            {
                PkGridId = _entity.PkGridId,
                FkFormId = _entity.FkFormId,
                FkUserId = _entity.FkUserId,
                GridName = _entity.GridName,
                JsonData = _entity.JsonData
            };


            return data;
        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            GridStructerModel model = (GridStructerModel)objmodel;
            TblGridStructer Tbl = new TblGridStructer();
            //Tbl.PkGridId = model.PkGridId;
            Tbl.FkFormId = model.FkFormId;
            Tbl.FkUserId = model.FkUserId;
            Tbl.GridName = model.GridName;
            Tbl.JsonData = model.JsonData;
            if (Mode == "Create")
            {
                //obj.PkcountryId = ID = getIdOfSeriesByEntity("PkcountryId", null, obj);
                AddData(Tbl, false);
            }
            else
            {
                var _entity = __dbContext.TblGridStructer
               .Where(cou => cou.FkUserId == model.FkUserId && cou.FkFormId == model.FkFormId
               && (string.IsNullOrEmpty(cou.GridName) ? "" : cou.GridName)== (string.IsNullOrEmpty(model.GridName)?"":model.GridName)
               ).FirstOrDefault();
                if (_entity != null)
                {
                    _entity.JsonData = model.JsonData;
                    UpdateData(_entity, false);
                }
                else { AddData(Tbl, false); }
            }
        }

    }
}