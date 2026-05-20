using System;
using System.Collections.Generic;
using MediatR;
using NextHorizont.Domain.Entities;

namespace NextHorizont.Application.UseCases.Products.Queries.GetActiveProducts;

public record GetActiveProductsQuery(Guid TenantId) : IRequest<IEnumerable<Product>>;
