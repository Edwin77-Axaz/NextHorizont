using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NextHorizont.Domain.Entities;
using NextHorizont.Domain.Interfaces;

namespace NextHorizont.Application.UseCases.Stays.Commands.CheckInStay;

public class CheckInStayCommandHandler : IRequestHandler<CheckInStayCommand, Guid>
{
    private readonly IStayRepository _stayRepository;

    public CheckInStayCommandHandler(IStayRepository stayRepository)
    {
        _stayRepository = stayRepository;
    }

    public async Task<Guid> Handle(CheckInStayCommand request, CancellationToken cancellationToken)
    {
        var stay = new Stay(
            Guid.NewGuid(),
            request.TenantId,
            request.LocationId,
            request.GuestId,
            request.CheckInDate,
            request.CheckOutDate,
            request.NightlyRate);

        await _stayRepository.AddAsync(stay);

        return stay.Id;
    }
}
