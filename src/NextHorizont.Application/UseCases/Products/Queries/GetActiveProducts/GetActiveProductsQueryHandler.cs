using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NextHorizont.Domain.Entities;
using NextHorizont.Domain.Interfaces;

namespace NextHorizont.Application.UseCases.Products.Queries.GetActiveProducts;

public class GetActiveProductsQueryHandler : IRequestHandler<GetActiveProductsQuery, IEnumerable<Product>>
{
    private readonly IProductRepository _productRepository;

    public GetActiveProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> Handle(GetActiveProductsQuery request, CancellationToken cancellationToken)
    {
        return await _productRepository.GetActiveProductsAsync(request.TenantId);
    }
}
