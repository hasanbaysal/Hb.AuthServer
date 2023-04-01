using Hb.AuthServer.Common.Dtos;
using Hb.AuthServer.Core.Entity;
using Hb.AuthServer.Core.Repositories;
using Hb.AuthServer.Core.Services;
using Hb.AuthServer.Core.UnitOfWork;
using Hb.AuthServer.Data.Repositories;
using Hb.AuthServer.Service.Mappings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hb.AuthServer.Service.Services
{
    public class GenericService<TEntity, TDto> : IGenericService<TEntity, TDto> where TEntity : class where TDto : class
    {

        private IUnitOfWork unitOfWork;
        private IGenericRepository<TEntity> repository;

        public GenericService(IUnitOfWork unitOfWork, IGenericRepository<TEntity> repository)
        {
            this.unitOfWork = unitOfWork;
            this.repository = repository;
        }

        public async Task<Response<TDto>> AddAsync(TDto entity)
        {
            

            var newEntity = ObjectMapper.Mapper.Map<TEntity>(entity);
            await repository.AddAsync(newEntity);
            await unitOfWork.CommitAsync();
            var newDto = ObjectMapper.Mapper.Map<TDto>(newEntity);
            return Response<TDto>.Succes(newDto, 200);

        }

        public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {

            var products = ObjectMapper.Mapper.Map<IEnumerable<TDto>>(await repository.GetAllAsync());

            return Response<IEnumerable<TDto>>.Succes(products, 200);
        }

        public async Task<Response<TDto>> GetByIdAsync(int id)
        {
          
            var data = await repository.GetByIdAsync(id);

            if (data==null)
            {
                return Response<TDto>.Fail(new("data not found",true),404);
            }

            var mapping = ObjectMapper.Mapper.Map<TDto>(data);

            return Response<TDto>.Succes(mapping, 200);


        }

        public async Task<Response<NoDataDto>> Remove(int id)
        {

            var isExist = await repository.GetByIdAsync(id);

            if (isExist == null)
            {
                return Response<NoDataDto>.Fail("data not found",404,true);   
            }

            repository.Remove(isExist);
            await  unitOfWork.CommitAsync();

            return Response<NoDataDto>.Succes(204);
        }

        public async Task<Response<NoDataDto>> Update(TDto entity,int id)
        {
            var isExist = await repository.GetByIdAsync(id);

            if (isExist == null)
            {
                return Response<NoDataDto>.Fail("data not found", 404, true);
            }


          var updated=  ObjectMapper.Mapper.Map<TEntity>(entity);

            repository.Update(updated);
            await unitOfWork.CommitAsync();
            return Response<NoDataDto>.Succes(204);

        }

        public async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> filter)
        {
            var data = await repository.Where(filter).ToListAsync();

            var map = ObjectMapper.Mapper.Map<List<TDto>>(data);

            return Response<IEnumerable<TDto>>.Succes(map, 200);


        }
    }
}
