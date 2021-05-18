using System;
using System.Threading.Tasks;
using Bank.Issuer.Library.Interfaces;
using Mapster;

namespace Bank.Issuer.Core
{
    public class BankIssuerMapper:IBankIssuerMapper
    {
        private readonly MapsterMapper.IMapper _mapper;
        public BankIssuerMapper(MapsterMapper.IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<TDestination> Map<TDestination>(object source)
        {
            return await _mapper.From(source)
                .AdaptToTypeAsync<TDestination>();
        }

        public async Task<TDestination> Map<TSource, TDestination>(TSource source)
        {
            return await _mapper.From(source)
                .AdaptToTypeAsync<TDestination>();
        }
    }
}
